using DataLibrary.Interfaces;
using DataLibrary.TempData;
using ObjectLibrary;
using ObjectLibrary.Interfaces;
using RadioHistoryService;
using ServiceLibrary.Abstracts;
using ServiceLibrary.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ServiceLibrary.Factory;

namespace ServiceLibrary.Services
{
    public class RadioService : ServiceBase, IRadioService
    {
        private readonly int _systemID;
        private readonly IRadioRepository _radioRepo;
        private readonly IRadioHistoryService _radioHistoryService;
        private readonly ITempService<TempRadio, Radio> _tempRadioService;
        private readonly ITempService<TempRadioHistory, RadioHistory> _tempRadioHistoryService;
        private readonly IMergeService _mergeService;

        public RadioService(int systemID, IRadioRepository radioRepository, IRadioHistoryService radioHistoryService, ITempService<TempRadio, Radio> tempRadioService,
            ITempService<TempRadioHistory, RadioHistory> tempRadioHistoryService, IMergeService mergeService)
        {
            _systemID = systemID;
            _radioRepo = radioRepository;
            _radioHistoryService = radioHistoryService;
            _tempRadioService = tempRadioService;
            _tempRadioHistoryService = tempRadioHistoryService;
            _mergeService = mergeService;
        }

        public IEnumerable<Radio> GetForSystem(int systemID)
        {
            return _radioRepo.GetListForSystem(systemID);
        }

        public async Task<int> ProcessRecordsAsync(IEnumerable<IRecord> records, Action<string, string, int, int> updateProgress, Action<bool> completedTasks, bool showCompleted)
        {
            var recordCount = 0;

            await Task.Run(() =>
            {
                var radioCount = 0;
                var historyCount = 0;
                var recordList = (IList<Radio>)records;

                updateProgress("Loading radios", $"Loading radios ({_radioRepo.GetCountForSystem(_systemID):#,##0})", radioCount, radioCount);
                var radios = _radioRepo.GetListForSystem(_systemID).ToList();
                updateProgress("Loading radio history", "Loading radio history", historyCount, historyCount);
                var currentRadioHistory = _radioHistoryService.GetForSystem(_systemID).ToList();

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
                    WriteTempData(_tempRadioService, radios, updateProgress);
                    WriteTempData(_tempRadioHistoryService, currentRadioHistory, updateProgress);
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

        public Radio Create(int systemID, int radioID, string description, DateTime timeStamp)
        {
            var radio = Create<Radio>();

            radio.SystemID = systemID;
            radio.RadioID = radioID;
            radio.Description = description.IsNullOrWhiteSpace() ? $"<Unknown> ({radioID:0})" : description;
            radio.FirstSeen = timeStamp;
            radio.LastSeen = timeStamp;

            return radio;
        }
    }
}
