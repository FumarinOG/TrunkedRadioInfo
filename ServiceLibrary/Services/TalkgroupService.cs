using DataLibrary.Interfaces;
using DataLibrary.TempData;
using ObjectLibrary;
using ObjectLibrary.Interfaces;
using ServiceLibrary.Abstracts;
using ServiceLibrary.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TalkgroupHistoryService;
using static ServiceLibrary.Factory;

namespace ServiceLibrary.Services
{
    public class TalkgroupService : ServiceBase, ITalkgroupService
    {
        private readonly int _systemID;
        private readonly ITalkgroupRepository _talkgroupRepo;
        private readonly ITalkgroupHistoryService _talkgroupHistoryService;
        private readonly ITempService<TempTalkgroup, Talkgroup> _tempTalkgroupService;
        private readonly ITempService<TempTalkgroupHistory, TalkgroupHistory> _tempTalkgroupHistoryService;
        private readonly IMergeService _mergeService;
        private ICollection<Talkgroup> _currentTalkgroups;
        private ICollection<TalkgroupHistory> _currentTalkgroupHistory;

        public TalkgroupService(int systemID, ITalkgroupRepository talkgroupRepository, ITalkgroupHistoryService talkgroupHistoryService,
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

        private void GetTalkgroups(bool loadHistory)
        {
            _currentTalkgroups = _talkgroupRepo.GetForSystem(_systemID).ToList();

            if (loadHistory)
            {
                _currentTalkgroupHistory = _talkgroupHistoryService.GetForSystem(_systemID).ToList();
            }
        }

        public IEnumerable<Talkgroup> GetForSystem()
        {
            return _talkgroupRepo.GetForSystem(_systemID);
        }

        public async Task<int> ProcessRecordsAsync(IEnumerable<IRecord> records, Action<string, string, int, int> updateProgress, Action<bool> completedTasks, bool showCompleted)
        {
            var recordCount = 0;

            await Task.Run(() =>
            {
                var talkgroupCount = 0;
                var historyCount = 0;
                var recordList = (IList<Talkgroup>)records;

                updateProgress("Loading talkgroups", $"Loading talkgroups ({_talkgroupRepo.GetCountForSystem(_systemID):#,##0})", talkgroupCount, talkgroupCount);
                var talkgroups = _talkgroupRepo.GetForSystem(_systemID).ToList();
                updateProgress("Loading talkgroup history", "Loading radio history", historyCount, historyCount);
                var currentTalkgroupHistory = _talkgroupHistoryService.GetForSystem(_systemID).ToList();

                foreach (var talkgroup in recordList)
                {
                    var current = talkgroups.SingleOrDefault(tg => tg.TalkgroupID == talkgroup.TalkgroupID);
                    var talkgroupHistory = currentTalkgroupHistory.SingleOrDefault(ctgh => ctgh.TalkgroupID == talkgroup.TalkgroupID &&
                        ctgh.Description.Equals(talkgroup.Description, StringComparison.OrdinalIgnoreCase));

                    if (talkgroupHistory == null)
                    {
                        talkgroupHistory = _talkgroupHistoryService.CreateTalkgroupHistory(talkgroup.SystemID, talkgroup.TalkgroupID, talkgroup.Description, talkgroup.LastSeen);
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
                    WriteTempData(_tempTalkgroupService, talkgroups, updateProgress);
                    WriteTempData(_tempTalkgroupHistoryService, currentTalkgroupHistory, updateProgress);
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error writing temp tables");
                    _mergeService.DeleteTempTables(_sessionID, updateProgress);
                    throw;
                }

                _mergeService.MergeRecords(_sessionID, updateProgress);
            });

            completedTasks(showCompleted);
            return recordCount;
        }

        public Talkgroup Create(int systemID, int talkgroupID, string description, DateTime timeStamp)
        {
            var talkgroup = Create<Talkgroup>();

            talkgroup.SystemID = systemID;
            talkgroup.TalkgroupID = talkgroupID;

            if (description.IsNullOrWhiteSpace())
            {
                talkgroup.Description = $"<Unknown> ({talkgroupID:0})";
            }
            else
            {
                talkgroup.Description = description;
            }

            talkgroup.FirstSeen = timeStamp;
            talkgroup.LastSeen = timeStamp;

            return talkgroup;

        }
    }
}
