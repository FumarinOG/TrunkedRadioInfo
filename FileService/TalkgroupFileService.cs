using DataLibrary.Interfaces;
using DataLibrary.TempData;
using FileService.Interfaces;
using ObjectLibrary;
using ObjectLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TalkgroupHistoryService;

namespace FileService
{
    public sealed class TalkgroupFileService : ServiceBase, ITalkgroupFileService
    {
        private readonly int _systemID;
        private readonly ITalkgroupRepository _talkgroupRepo;
        private readonly ITalkgroupHistoryService _talkgroupHistoryService;
        private readonly ITempService<TempTalkgroup, Talkgroup> _tempTalkgroupService;
        private readonly ITempService<TempTalkgroupHistory, TalkgroupHistory> _tempTalkgroupHistoryService;
        private readonly IMergeService _mergeService;
        private ICollection<Talkgroup> _currentTalkgroups;
        private ICollection<TalkgroupHistory> _currentTalkgroupHistory;

        public TalkgroupFileService(int systemID, ITalkgroupRepository talkgroupRepository, ITalkgroupHistoryService talkgroupHistoryService,
            ITempService<TempTalkgroup, Talkgroup> tempTalkgroupService, ITempService<TempTalkgroupHistory, TalkgroupHistory> tempTalkgroupHistoryService,
            IMergeService mergeService)
        {
            _systemID = systemID;
            _talkgroupRepo = talkgroupRepository;
            _talkgroupHistoryService = talkgroupHistoryService;
            _tempTalkgroupService = tempTalkgroupService;
            _tempTalkgroupHistoryService = tempTalkgroupHistoryService;
            _mergeService = mergeService;
        }

        private async Task GetTalkgroupsAsync(bool loadHistory)
        {
            _currentTalkgroups = (await _talkgroupRepo.GetForSystemAsync(_systemID)).ToList();

            if (loadHistory)
            {
                _currentTalkgroupHistory = (await _talkgroupHistoryService.GetForSystemAsync(_systemID)).ToList();
            }
        }

        public async Task<int> ProcessRecordsAsync(IEnumerable<IRecord> records, Action<string, string, int, int> updateProgress,
            Action<bool> completedTasks, bool showCompleted)
        {
            var recordCount = 0;

            await Task.Run(async () =>
            {
                var talkgroupCount = 0;
                var historyCount = 0;
                var recordList = (IList<Talkgroup>)records;

                updateProgress("Loading talkgroups", $"Loading talkgroups ({await _talkgroupRepo.GetCountForSystemAsync(_systemID):#,##0})",
                    talkgroupCount, talkgroupCount);
                var talkgroups = (await _talkgroupRepo.GetForSystemAsync(_systemID)).ToList();
                updateProgress("Loading talkgroup history", "Loading radio history", historyCount, historyCount);
                var currentTalkgroupHistory = (await _talkgroupHistoryService.GetForSystemAsync(_systemID)).ToList();

                foreach (var talkgroup in recordList)
                {
                    var current = talkgroups.SingleOrDefault(tg => tg.TalkgroupID == talkgroup.TalkgroupID);
                    var talkgroupHistory = currentTalkgroupHistory.SingleOrDefault(ctgh => ctgh.TalkgroupID == talkgroup.TalkgroupID &&
                        ctgh.Description.Equals(talkgroup.Description, StringComparison.OrdinalIgnoreCase));

                    if (talkgroupHistory == null)
                    {
                        talkgroupHistory = _talkgroupHistoryService.CreateTalkgroupHistory(talkgroup.SystemID, talkgroup.TalkgroupID, talkgroup.Description,
                            talkgroup.LastSeen);
                        currentTalkgroupHistory.Add(talkgroupHistory);
                        historyCount++;
                        updateProgress($"Processing talkgroup history ({recordCount:#,##0})", $"{historyCount:#,##0} records written", historyCount, recordList.Count);
                    }

                    recordCount++;

                    if (current == null)
                    {
                        talkgroups.Add(talkgroup);
                        talkgroupCount++;
                    }
                    else
                    {
                        talkgroup.Assign(current);

                        if (!current.Equals(talkgroup))
                        {
                            talkgroup.IsNew = false;
                            talkgroups.Add(talkgroup);
                            talkgroupCount++;
                        }
                    }

                    updateProgress($"Processing talkgroup ({recordCount:#,##0})", $"{talkgroupCount:#,##0} updated records found", recordCount, recordList.Count);
                }

                try
                {
                    await WriteTempDataAsync(_tempTalkgroupService, talkgroups, updateProgress);
                    await WriteTempDataAsync(_tempTalkgroupHistoryService, currentTalkgroupHistory, updateProgress);
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error writing temp tables");
                    await _mergeService.DeleteTempTablesAsync(_sessionID, updateProgress);
                    throw;
                }

                await _mergeService.MergeRecordsAsync(_sessionID, updateProgress);
            });

            completedTasks(showCompleted);
            return recordCount;
        }
    }
}
