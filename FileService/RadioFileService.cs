using DataLibrary.Interfaces;
using DataLibrary.TempData;
using FileService.Interfaces;
using ObjectLibrary;
using ObjectLibrary.Interfaces;
using RadioHistoryService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileService
{
    public sealed class RadioFileService : ServiceBase, IRadioFileService
    {
        private readonly int _systemID;
        private readonly IRadioRepository _radioRepo;
        private readonly IRadioHistoryService _radioHistoryService;
        private readonly ITempService<TempRadio, Radio> _tempRadioService;
        private readonly ITempService<TempRadioHistory, RadioHistory> _tempRadioHistoryService;
        private readonly IMergeService _mergeService;

        public RadioFileService(int systemID, IRadioRepository radioRepository, IRadioHistoryService radioHistoryService,
            ITempService<TempRadio, Radio> tempRadioService, ITempService<TempRadioHistory, RadioHistory> tempRadioHistoryService, IMergeService mergeService)
        {
            _systemID = systemID;
            _radioRepo = radioRepository;
            _radioHistoryService = radioHistoryService;
            _tempRadioService = tempRadioService;
            _tempRadioHistoryService = tempRadioHistoryService;
            _mergeService = mergeService;
        }

        public async Task<int> ProcessRecordsAsync(IEnumerable<IRecord> records, Action<string, string, int, int> updateProgress, Action<bool> completedTasks,
            bool showCompleted)
        {
            var recordCount = 0;

            await Task.Run(async () =>
            {
                var radioCount = 0;
                var historyCount = 0;
                var recordList = (IList<Radio>)records;

                updateProgress("Loading radios", $"Loading radios ({await _radioRepo.GetCountForSystemAsync(_systemID):#,##0})", radioCount, radioCount);
                var radios = (await _radioRepo.GetListForSystemAsync(_systemID)).ToList();
                updateProgress("Loading radio history", "Loading radio history", historyCount, historyCount);
                var currentRadioHistory = (await _radioHistoryService.GetForSystemAsync(_systemID)).ToList();

                foreach (var radio in recordList)
                {
                    var current = radios.SingleOrDefault(cr => cr.RadioID == radio.RadioID);
                    var radioHistory = currentRadioHistory.SingleOrDefault(crh => crh.RadioID == radio.RadioID && 
                        crh.Description.Equals(radio.Description, StringComparison.OrdinalIgnoreCase));

                    if (radioHistory == null)
                    {
                        radioHistory = _radioHistoryService.CreateRadioHistory(radio.SystemID, radio.RadioID, radio.Description, radio.LastSeen);
                        currentRadioHistory.Add(radioHistory);
                        historyCount++;
                        updateProgress($"Processing radio history ({recordCount:#,##0})", $"{historyCount:#,##0} records written", historyCount, recordList.Count);
                    }

                    recordCount++;

                    if (current == null)
                    {
                        radios.Add(radio);
                        radioCount++;
                    }
                    else
                    {
                        radio.Assign(current);

                        if (!current.Equals(radio))
                        {
                            radio.IsNew = false;
                            radios.Add(radio);
                            radioCount++;
                        }
                    }

                    updateProgress($"Processing radio ({recordCount:#,##0})", $"{radioCount:#,##0} updated records found", recordCount, recordList.Count);
                }

                try
                {
                    await WriteTempDataAsync(_tempRadioService, radios, updateProgress);
                    await WriteTempDataAsync(_tempRadioHistoryService, currentRadioHistory, updateProgress);
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
