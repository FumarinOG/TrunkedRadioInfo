using DataLibrary.TempData;
using ObjectLibrary;
using ObjectLibrary.Interfaces;
using ProcessedFileService;
using RadioHistoryService;
using ServiceLibrary.Abstracts;
using ServiceLibrary.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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
using static ServiceLibrary.Factory;

namespace ServiceLibrary.Services
{
    public class GrantLogService : FileServiceBase, IService
    {
        private readonly int _systemID;
        private readonly int _towerNumber;

        public GrantLogService(int systemID, int towerNumber, UploadFileModel uploadFile, IRadioService radioService, IRadioHistoryService radioHistoryService,
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
            _systemID = systemID;
            _towerNumber = towerNumber;
        }

        public async Task<int> ProcessRecordsAsync(IEnumerable<IRecord> grantLogs, Action<string, string, int, int> updateProgress, Action<bool> completedTasks, bool showCompleted)
        {
            var recordCount = 0;

            await Task.Run(() =>
            {
                var alertCount = 0;
                var grantLogList = ((IEnumerable<GrantLog>)grantLogs).ToList();
                var alertItemsToAdd = CreatList<GrantLog>();
                var alertItemsToRemove = CreatList<GrantLog>();

                foreach (var grantLog in grantLogList.Where(gl => gl.Type.Equals("Alert", StringComparison.OrdinalIgnoreCase)))
                {
                    alertCount++;
                    updateProgress("Processing alerts", $"{alertCount:#,##0} records processed", alertCount, grantLogList.Count);
                    alertItemsToAdd.Add(CreateGrantLog(grantLog.TimeStamp, grantLog.Type, grantLog.Channel, grantLog.Frequency, grantLog.TalkgroupID, grantLog.TalkgroupDescription));
                    alertItemsToAdd.Add(CreateGrantLog(grantLog.TimeStamp, grantLog.Type, grantLog.Channel, grantLog.Frequency, grantLog.RadioID, grantLog.RadioDescription));
                    alertItemsToRemove.Add(grantLog);
                }

                foreach (var grantLog in alertItemsToRemove)
                {
                    grantLogList.Remove(grantLog);
                }

                grantLogList.AddRange(alertItemsToAdd);

                foreach (var grantLog in grantLogList)
                {
                    recordCount++;
                    var recordsProcessed = $"{recordCount:#,##0} records processed";

                    updateProgress("Processing system", recordsProcessed, recordCount, grantLogList.Count);
                    ProcessSystem(grantLog.TimeStamp);

                    updateProgress("Processing radio", recordsProcessed, recordCount, grantLogList.Count);
                    ProcessRadio(grantLog.RadioID, grantLog.RadioDescription, grantLog.TimeStamp, grantLog.Type, UpdateCounters, UpdateHitCounts);

                    updateProgress("Processing talkgroup", recordsProcessed, recordCount, grantLogList.Count);
                    ProcessTalkgroup(grantLog.TalkgroupID, grantLog.TalkgroupDescription, grantLog.TimeStamp, grantLog.Type, UpdateCounters, UpdateHitCounts);

                    updateProgress("Processing talkgroup radio", recordsProcessed, recordCount, grantLogList.Count);
                    ProcessTalkgroupRadio(grantLog.TalkgroupID, grantLog.RadioID, grantLog.TimeStamp, grantLog.Type, UpdateCounters);

                    updateProgress("Processing tower", recordsProcessed, recordCount, grantLogList.Count);
                    ProcessTower(grantLog.TimeStamp);

                    updateProgress("Processing tower talkgroup radio", recordsProcessed, recordCount, grantLogList.Count);
                    ProcessTowerTalkgroupRadio(grantLog.TalkgroupID, grantLog.RadioID, grantLog.TimeStamp, grantLog.Type, UpdateCounters);

                    updateProgress("Processing tower frequency", recordsProcessed, recordCount, grantLogList.Count);
                    ProcessTowerFrequency(_systemID, _towerNumber, grantLog.Frequency, grantLog.Channel, grantLog.TalkgroupID, grantLog.RadioID, grantLog.TimeStamp,
                        grantLog.Type, UpdateCounters);

                    updateProgress("Processing tower frequency radio", recordsProcessed, recordCount, grantLogList.Count);
                    ProcessTowerFrequencyRadio(_systemID, grantLog.Frequency, grantLog.RadioID, grantLog.TimeStamp, grantLog.Type, UpdateCounters);

                    updateProgress("Processing tower frequency talkgroup", recordsProcessed, recordCount, grantLogList.Count);
                    ProcessTowerFrequencyTalkgroup(_systemID, grantLog.Frequency, grantLog.TalkgroupID, grantLog.TimeStamp, grantLog.Type, UpdateCounters);
                }

                WriteData(updateProgress);
            });

            completedTasks(showCompleted);
            return recordCount;
        }

        public int GetCountForSystem()
        {
            throw new NotImplementedException();
        }

        private static GrantLog CreateGrantLog(DateTime timeStamp, string type, string channel, string frequency, int radioID, string radioDescription)
        {
            var grantLog = Create<GrantLog>();

            grantLog.TimeStamp = timeStamp;
            grantLog.Type = type;
            grantLog.Channel = channel;
            grantLog.Frequency = frequency;
            grantLog.TalkgroupID = 0;
            grantLog.TalkgroupDescription = string.Empty;
            grantLog.RadioID = radioID;
            grantLog.RadioDescription = radioDescription;

            return grantLog;
        }

        private static void UpdateCounters(ICounterRecord record, ActionTypes action)
        {
            switch (action)
            {
                case ActionTypes.Alert:
                    record.AlertCount++;
                    break;

                case ActionTypes.Data:
                    record.DataCount++;
                    break;

                case ActionTypes.Group:
                    record.VoiceGrantCount++;
                    break;

                case ActionTypes.GroupEmergency:
                    record.EmergencyVoiceGrantCount++;
                    break;

                case ActionTypes.GroupEncrypted:
                    record.EncryptedVoiceGrantCount++;
                    break;

                case ActionTypes.PrivateData:
                    record.PrivateDataCount++;
                    break;

                case ActionTypes.CWID:
                case ActionTypes.StationID:
                    record.CWIDCount++;
                    break;

                case ActionTypes.GroupData:
                case ActionTypes.Queued:
                case ActionTypes.QueuedDataGrant:
                    // Don't log these types
                    break;

                case ActionTypes.Affiliate:
                case ActionTypes.Unaffiliate:
                case ActionTypes.Denied:
                case ActionTypes.Forced:
                case ActionTypes.Refused:
                    // These types aren't used by grant logs
                    break;

                default:
                    throw new Exception("Unknown Action!");
            }
        }

        private static void UpdateHitCounts(IRecord record, ActionTypes action)
        {
            switch (action)
            {
                case ActionTypes.Group:
                case ActionTypes.GroupEmergency:
                case ActionTypes.GroupEncrypted:
                    record.HitCount++;
                    break;

                case ActionTypes.Alert:
                case ActionTypes.CWID:
                case ActionTypes.Data:
                case ActionTypes.GroupData:
                case ActionTypes.PrivateData:
                case ActionTypes.Queued:
                case ActionTypes.QueuedDataGrant:
                case ActionTypes.StationID:
                    // Don't log these types
                    break;

                case ActionTypes.Affiliate:
                case ActionTypes.Unaffiliate:
                case ActionTypes.Denied:
                case ActionTypes.Forced:
                case ActionTypes.Refused:
                    // These actions don't exist in grant logs
                    break;

                default:
                    throw new Exception("Unknown Action!");
            }
        }
    }
}
