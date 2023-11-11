using DataLibrary.TempData;
using FileService.Interfaces;
using ObjectLibrary;
using ObjectLibrary.Interfaces;
using ProcessedFileService;
using RadioHistoryService;
using RadioService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemInfoService;
using TalkgroupHistoryService;
using TalkgroupRadioService;
using TalkgroupService;
using TowerFrequencyRadioService;
using TowerFrequencyTalkgroupService;
using TowerFrequencyUsageService;
using TowerRadioService;
using TowerService;
using TowerTalkgroupRadioService;
using TowerTalkgroupService;

namespace FileService
{
    public sealed class AffiliationLogService : FileServiceBase, IService
    {
        public AffiliationLogService(int systemID, int towerNumber, UploadFileModel uploadFile, IRadioService radioService,
            IRadioHistoryService radioHistoryService, ISystemInfoService systemInfoService, ITalkgroupService talkgroupService,
            ITalkgroupHistoryService talkgroupHistoryService, ITalkgroupRadioService talkgroupRadioService, ITowerService towerService,
            ITowerFrequencyRadioService towerFrequencyRadioService, ITowerFrequencyTalkgroupService towerFrequencyTalkgroupService,
            ITowerFrequencyUsageService towerFrequencyUsageService, ITowerRadioService towerRadioService, ITowerTalkgroupService towerTalkgroupService,
            ITowerTalkgroupRadioService towerTalkgroupRadioService, IMergeService mergeService, ITempService<TempRadio, Radio> tempRadioService,
            ITempService<TempRadioHistory, RadioHistory> tempRadioHistoryService, ITempService<TempSystemInfo, SystemInfo> tempSystemInfoService,
            ITempService<TempTalkgroup, Talkgroup> tempTalkgroupService, ITempService<TempTalkgroupHistory, TalkgroupHistory> tempTalkgroupHistoryService,
            ITempService<TempTalkgroupRadio, TalkgroupRadio> tempTalkgroupRadioService, ITempService<TempTower, Tower> tempTowerService,
            ITempService<TempTowerFrequencyRadio, TowerFrequencyRadio> tempTowerFrequencyRadioService,
            ITempService<TempTowerFrequencyTalkgroup, TowerFrequencyTalkgroup> tempTowerFrequencyTalkgroupService,
            ITempService<TempTowerFrequencyUsage, TowerFrequencyUsage> tempTowerFrequencyUsageService,
            ITempService<TempTowerRadio, TowerRadio> tempTowerRadioService, ITempService<TempTowerTalkgroup, TowerTalkgroup> tempTowerTalkgroupService,
            ITempService<TempTowerTalkgroupRadio, TowerTalkgroupRadio> tempTowerTalkgroupRadioService)
            : base(
                systemID, towerNumber, uploadFile, radioService, radioHistoryService, systemInfoService, talkgroupService, talkgroupHistoryService,
                talkgroupRadioService, towerService, towerFrequencyRadioService, towerFrequencyTalkgroupService, towerFrequencyUsageService,
                towerRadioService, towerTalkgroupService, towerTalkgroupRadioService, mergeService, tempRadioService, tempRadioHistoryService,
                tempSystemInfoService, tempTalkgroupService, tempTalkgroupHistoryService, tempTalkgroupRadioService, tempTowerService,
                tempTowerFrequencyRadioService, tempTowerFrequencyTalkgroupService, tempTowerFrequencyUsageService, tempTowerRadioService,
                tempTowerTalkgroupService, tempTowerTalkgroupRadioService)
        {
            _processTowerFrequencies = false;
        }

        public async Task<int> ProcessRecordsAsync(IEnumerable<IRecord> affiliations, Action<string, string, int, int> updateProgress, Action<bool> completedTasks,
            bool showCompleted)
        {
            var count = 0;
            var recordList = (IList<Affiliation>)affiliations;
            var recordCount = recordList.Count;

            await Task.Run(async () =>
            {
                var description = string.Empty;

                await FillDataAsync();

                foreach (var affiliation in recordList)
                {
                    count++;
                    var recordsProcessed = $"{count:#,##0} records processed";

                    updateProgress("Processing system", recordsProcessed, count, recordCount);
                    _systemInfoService.ProcessRecord(_systemInfo, affiliation.TimeStamp);

                    updateProgress("Processing radio", recordsProcessed, count, recordCount);
                    description = _radioService.ProcessRecord(_systemInfo.ID, affiliation.RadioID, affiliation.RadioDescription, affiliation.TimeStamp,
                        affiliation.Function, _radios, UpdateHitCounts);

                    updateProgress("Processing radio history", recordsProcessed, count, recordCount);
                    _radioHistoryService.ProcessRecord(_systemInfo.ID, affiliation.RadioID, description, affiliation.TimeStamp, _radios, _radioHistory);

                    updateProgress("Processing tower radio", recordsProcessed, count, recordCount);
                    _towerRadioService.ProcessRecord(_systemInfo.ID, _tower.ID, affiliation.RadioID, affiliation.TimeStamp, affiliation.Function,
                        _towerRadios, UpdateCounters);

                    updateProgress("Processing talkgroup", recordsProcessed, count, recordCount);
                    description = _talkgroupService.ProcessRecord(_systemInfo.ID, affiliation.TalkgroupID, affiliation.TalkgroupDescription, _talkgroups,
                        affiliation.TimeStamp, affiliation.Function, UpdateHitCounts);

                    if (!string.IsNullOrWhiteSpace(description))
                    {
                        updateProgress("Processing talkgroup history", recordsProcessed, count, recordCount);
                        _talkgroupHistoryService.ProcessRecord(_systemInfo.ID, affiliation.TalkgroupID, description, affiliation.TimeStamp, _talkgroups,
                            _talkgroupHistory);
                    }

                    updateProgress("Processing tower talkgroup", recordsProcessed, count, recordCount);
                    _towerTalkgroupService.ProcessRecord(_systemInfo.ID, _tower.ID, affiliation.TalkgroupID, affiliation.TimeStamp, affiliation.Function,
                        _towerTalkgroups, UpdateCounters);

                    updateProgress("Processing talkgroup radio", recordsProcessed, count, recordCount);
                    _talkgroupRadioService.ProcessRecord(_systemInfo.ID, affiliation.TalkgroupID, affiliation.RadioID, _talkgroupRadios, affiliation.TimeStamp,
                        affiliation.Function, UpdateCounters);

                    updateProgress("Processing tower talkgroup radio", recordsProcessed, count, recordCount);
                    _towerTalkgroupRadioService.ProcessRecord(_systemInfo.ID, _tower.ID, affiliation.TalkgroupID, affiliation.RadioID, _towerTalkgroupRadios,
                        affiliation.TimeStamp, affiliation.Function, UpdateCounters);

                    updateProgress("Processing tower", recordsProcessed, count, recordCount);
                    _towerService.ProcessRecord(_tower, affiliation.TimeStamp);
                }

                await WriteDataAsync(updateProgress);
            });

            completedTasks(showCompleted);
            return count;
        }

        private static void UpdateCounters(ICounterRecord record, ActionTypes action)
        {
            switch (action)
            {
                case ActionTypes.Affiliate:
                case ActionTypes.Forced:
                    record.AffiliationCount++;
                    break;

                case ActionTypes.Denied:
                case ActionTypes.Refused:
                    record.DeniedCount++;
                    break;

                case ActionTypes.Unaffiliate:
                    // Don't log these types
                    break;

                case ActionTypes.Alert:
                case ActionTypes.CWID:
                case ActionTypes.StationID:
                case ActionTypes.Data:
                case ActionTypes.Group:
                case ActionTypes.GroupEmergency:
                case ActionTypes.GroupEncrypted:
                case ActionTypes.GroupData:
                case ActionTypes.PrivateData:
                case ActionTypes.Queued:
                case ActionTypes.QueuedDataGrant:
                    // These action types don't exist in affiliatios
                    break;

                default:
                    throw new Exception("Unknown Action!");
            }
        }

        private static void UpdateHitCounts(IRecord record, ActionTypes action)
        {
            switch (action)
            {
                case ActionTypes.Affiliate:
                case ActionTypes.Unaffiliate:
                case ActionTypes.Denied:
                case ActionTypes.Forced:
                case ActionTypes.Refused:
                    // Don't log these types
                    break;

                case ActionTypes.Alert:
                case ActionTypes.CWID:
                case ActionTypes.StationID:
                case ActionTypes.Data:
                case ActionTypes.Group:
                case ActionTypes.GroupEmergency:
                case ActionTypes.GroupEncrypted:
                case ActionTypes.GroupData:
                case ActionTypes.PrivateData:
                case ActionTypes.Queued:
                case ActionTypes.QueuedDataGrant:
                    // These action types don't exist in affiliatios
                    break;

                default:
                    throw new Exception("Unknown Action!");
            }
        }
    }
}
