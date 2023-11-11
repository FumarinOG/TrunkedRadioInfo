using DataLibrary.TempData;
using ObjectLibrary;
using ObjectLibrary.Interfaces;
using ProcessedFileService;
using RadioHistoryService;
using ServiceLibrary.Abstracts;
using ServiceLibrary.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemInfoService;
using TalkgroupHistoryService;
using TalkgroupRadioService;
using TowerFrequencyRadioService;
using TowerFrequencyTalkgroupService;
using TowerFrequencyUsageService;
using TowerRadioService;
using TowerTalkgroupRadioService;
using TowerTalkgroupService;

namespace ServiceLibrary.Services
{
    public class AffiliationLogService : FileServiceBase, IService
    {
        public AffiliationLogService(int systemID, int towerNumber, UploadFileModel uploadFile, IRadioService radioService, IRadioHistoryService radioHistoryService,
            ISystemInfoService systemInfoService, ITalkgroupService talkgroupService, ITalkgroupHistoryService talkgroupHistoryService,
            ITalkgroupRadioService talkgroupRadioService, ITowerService towerService, ITowerFrequencyRadioService towerFrequencyRadioService,
            ITowerFrequencyTalkgroupService towerFrequencyTalkgroupService, ITowerFrequencyUsageService towerFrequencyUsageService,
            ITowerRadioService towerRadioService, ITowerTalkgroupService towerTalkgroupService, ITowerTalkgroupRadioService towerTalkgroupRadioService,
            IMergeService mergeService, ITempService<TempRadio, Radio> tempRadioService, ITempService<TempRadioHistory, RadioHistory> tempRadioHistoryService,
            ITempService<TempSystemInfo, SystemInfo> tempSystemInfoService, ITempService<TempTalkgroup, Talkgroup> tempTalkgroupService,
            ITempService<TempTalkgroupHistory, TalkgroupHistory> tempTalkgroupHistoryService, ITempService<TempTalkgroupRadio, TalkgroupRadio> tempTalkgroupRadioService,
            ITempService<TempTower, Tower> tempTowerService, ITempService<TempTowerFrequencyRadio, TowerFrequencyRadio> tempTowerFrequencyRadioService,
            ITempService<TempTowerFrequencyTalkgroup, TowerFrequencyTalkgroup> tempTowerFrequencyTalkgroupService,
            ITempService<TempTowerFrequencyUsage, TowerFrequencyUsage> tempTowerFrequencyUsageService, ITempService<TempTowerRadio, TowerRadio> tempTowerRadioService,
            ITempService<TempTowerTalkgroup, TowerTalkgroup> tempTowerTalkgroupService, ITempService<TempTowerTalkgroupRadio, TowerTalkgroupRadio> tempTowerTalkgroupRadioService)
            : base(
                systemID, towerNumber, uploadFile, radioService, radioHistoryService, systemInfoService, talkgroupService, talkgroupHistoryService, talkgroupRadioService,
                towerService, towerFrequencyRadioService, towerFrequencyTalkgroupService, towerFrequencyUsageService, towerRadioService, towerTalkgroupService,
                towerTalkgroupRadioService, mergeService, tempRadioService, tempRadioHistoryService, tempSystemInfoService, tempTalkgroupService, tempTalkgroupHistoryService,
                tempTalkgroupRadioService, tempTowerService, tempTowerFrequencyRadioService, tempTowerFrequencyTalkgroupService, tempTowerFrequencyUsageService,
                tempTowerRadioService, tempTowerTalkgroupService, tempTowerTalkgroupRadioService)
        {
            _processTowerFrequencies = false;
        }

        public async Task<int> ProcessRecordsAsync(IEnumerable<IRecord> affiliations, Action<string, string, int, int> updateProgress, Action<bool> completedTasks, bool showCompleted)
        {
            var count = 0;
            var recordList = (IList<Affiliation>)affiliations;

            await Task.Run(() =>
            {
                foreach (var affiliation in recordList)
                {
                    count++;
                    var recordsProcessed = $"{count:#,##0} records processed";

                    updateProgress("Processing system", recordsProcessed, count, recordList.Count);
                    ProcessSystem(affiliation.TimeStamp);

                    updateProgress("Processing radio", recordsProcessed, count, recordList.Count);
                    ProcessRadio(affiliation.RadioID, affiliation.RadioDescription, affiliation.TimeStamp, affiliation.Function, UpdateCounters, UpdateHitCounts);

                    updateProgress("Processing talkgroup", recordsProcessed, count, recordList.Count);
                    ProcessTalkgroup(affiliation.TalkgroupID, affiliation.TalkgroupDescription, affiliation.TimeStamp, affiliation.Function, UpdateCounters,
                        UpdateHitCounts);

                    updateProgress("Processing talkgroup radio", recordsProcessed, count, recordList.Count);
                    ProcessTalkgroupRadio(affiliation.TalkgroupID, affiliation.RadioID, affiliation.TimeStamp, affiliation.Function, UpdateCounters);

                    updateProgress("Processing tower talkgroup radio", recordsProcessed, count, recordList.Count);
                    ProcessTowerTalkgroupRadio(affiliation.TalkgroupID, affiliation.RadioID, affiliation.TimeStamp, affiliation.Function, UpdateCounters);

                    updateProgress("Processing tower", recordsProcessed, count, recordList.Count);
                    ProcessTower(affiliation.TimeStamp);
                }

                WriteData(updateProgress);
            });

            completedTasks(showCompleted);
            return count;
        }

        public int GetCountForSystem()
        {
            throw new NotImplementedException();
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
