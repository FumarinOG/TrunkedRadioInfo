using DataLibrary.TempData;
using FileService.Interfaces;
using ObjectLibrary;
using ObjectLibrary.Interfaces;
using ProcessedFileService;
using RadioHistoryService;
using RadioService;
using System;
using System.Collections.Generic;
using System.Linq;
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
using static FileService.Factory;

namespace FileService
{
    public sealed class GrantLogService : FileServiceBase, IService
    {
        public GrantLogService(int systemID, int towerNumber, UploadFileModel uploadFile, IRadioService radioService,
            IRadioHistoryService radioHistoryService, ISystemInfoService systemInfoService, ITalkgroupService talkgroupService, ITowerService towerService,
            ITalkgroupFileService talkgroupFileService, ITalkgroupHistoryService talkgroupHistoryService, ITalkgroupRadioService talkgroupRadioService,
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
        }

        public async Task<int> ProcessRecordsAsync(IEnumerable<IRecord> grantLogs, Action<string, string, int, int> updateProgress, Action<bool> completedTasks, bool showCompleted)
        {
            var count = 0;

            await Task.Run(async () =>
            {
                var alertCount = 0;
                var grantLogList = ((IEnumerable<GrantLog>)grantLogs).ToList();
                var alertItemsToAdd = CreatList<GrantLog>();
                var alertItemsToRemove = CreatList<GrantLog>();

                await FillDataAsync();

                foreach (var grantLog in grantLogList.Where(gl => gl.Type.Equals("Alert", StringComparison.OrdinalIgnoreCase)))
                {
                    alertCount++;
                    updateProgress("Processing alerts", $"{alertCount:#,##0} records processed", alertCount, grantLogList.Count);
                    alertItemsToAdd.Add(CreateAlertGrantLogTalkgroup(grantLog));
                    alertItemsToAdd.Add(CreateAlertGrantLogRadio(grantLog));
                    alertItemsToRemove.Add(grantLog);
                }

                foreach (var grantLog in alertItemsToRemove)
                {
                    grantLogList.Remove(grantLog);
                }

                grantLogList.AddRange(alertItemsToAdd);

                var recordCount = grantLogList.Count;

                foreach (var grantLog in grantLogList)
                {
                    var description = string.Empty;
                    count++;
                    var recordsProcessed = $"{count:#,##0} records processed";

                    updateProgress("Processing system", recordsProcessed, count, recordCount);
                    _systemInfoService.ProcessRecord(_systemInfo, grantLog.TimeStamp);

                    updateProgress("Processing radio", recordsProcessed, count, recordCount);
                    description = _radioService.ProcessRecord(_systemInfo.ID, grantLog.RadioID, grantLog.RadioDescription, grantLog.TimeStamp,
                        grantLog.Type, _radios, UpdateHitCounts);

                    updateProgress("Processing radio history", recordsProcessed, count, recordCount);
                    _radioHistoryService.ProcessRecord(_systemInfo.ID, grantLog.RadioID, description, grantLog.TimeStamp, _radios, _radioHistory);

                    updateProgress("Processing tower radio", recordsProcessed, count, recordCount);
                    _towerRadioService.ProcessRecord(_systemInfo.ID, _tower.ID, grantLog.RadioID, grantLog.TimeStamp, grantLog.Type, _towerRadios,
                        UpdateCounters);

                    updateProgress("Processing talkgroup", recordsProcessed, count, recordCount);
                    description = _talkgroupService.ProcessRecord(_systemInfo.ID, grantLog.TalkgroupID, grantLog.TalkgroupDescription, _talkgroups,
                        grantLog.TimeStamp, grantLog.Type, UpdateHitCounts);

                    if (!string.IsNullOrWhiteSpace(description))
                    {
                        updateProgress("Processing talkgroup history", recordsProcessed, count, recordCount);
                        _talkgroupHistoryService.ProcessRecord(_systemInfo.ID, grantLog.TalkgroupID, description, grantLog.TimeStamp, _talkgroups,
                            _talkgroupHistory);
                    }

                    updateProgress("Processing tower talkgroup", recordsProcessed, count, recordCount);
                    _towerTalkgroupService.ProcessRecord(_systemInfo.ID, _tower.ID, grantLog.TalkgroupID, grantLog.TimeStamp, grantLog.Type,
                        _towerTalkgroups, UpdateCounters);

                    updateProgress("Processing talkgroup radio", recordsProcessed, count, recordCount);
                    _talkgroupRadioService.ProcessRecord(_systemInfo.ID, grantLog.TalkgroupID, grantLog.RadioID, _talkgroupRadios, grantLog.TimeStamp,
                        grantLog.Type, UpdateCounters);

                    updateProgress("Processing tower", recordsProcessed, count, recordCount);
                    _towerService.ProcessRecord(_tower, grantLog.TimeStamp);

                    updateProgress("Processing tower talkgroup radio", recordsProcessed, count, recordCount);
                    _towerTalkgroupRadioService.ProcessRecord(_systemInfo.ID, _tower.ID, grantLog.TalkgroupID, grantLog.RadioID, _towerTalkgroupRadios,
                        grantLog.TimeStamp, grantLog.Type, UpdateCounters);

                    updateProgress("Processing tower frequency", recordsProcessed, count, recordCount);
                    _towerFrequencyUsageService.ProcessRecord(_systemInfo.ID, _towerNumber, grantLog.Frequency, grantLog.Channel, grantLog.TalkgroupID,
                        grantLog.RadioID, _towerFrequencyUsage, grantLog.TimeStamp, grantLog.Type, UpdateCounters);

                    updateProgress("Processing tower frequency radio", recordsProcessed, count, recordCount);
                    _towerFrequencyRadioService.ProcessRecord(_systemInfo.ID, _tower.ID, grantLog.Frequency, grantLog.RadioID, _radios, _towerFrequencyRadios,
                        grantLog.TimeStamp, grantLog.Type, UpdateCounters);

                    updateProgress("Processing tower frequency talkgroup", recordsProcessed, count, recordCount);
                    _towerFrequencyTalkgroupService.ProcessRecord(_systemInfo.ID, _tower.ID, grantLog.Frequency, grantLog.TalkgroupID, _talkgroups,
                        _towerFrequencyTalkgroups, grantLog.TimeStamp, grantLog.Type, UpdateCounters);
                }

                await WriteDataAsync(updateProgress);
            });

            completedTasks(showCompleted);
            return count;
        }

        public static GrantLog CreateAlertGrantLogRadio(GrantLog grantLog)
        {
            var newGrantLog = Create<GrantLog>();

            newGrantLog.TimeStamp = grantLog.TimeStamp;
            newGrantLog.Type = grantLog.Type;
            newGrantLog.Channel = grantLog.Channel;
            newGrantLog.Frequency = grantLog.Frequency;
            newGrantLog.TalkgroupID = 0;
            newGrantLog.TalkgroupDescription = string.Empty;
            newGrantLog.RadioID = grantLog.RadioID;
            newGrantLog.RadioDescription = grantLog.RadioDescription;

            return newGrantLog;
        }

        public static GrantLog CreateAlertGrantLogTalkgroup(GrantLog grantLog)
        {
            var newGrantLog = Create<GrantLog>();

            newGrantLog.TimeStamp = grantLog.TimeStamp;
            newGrantLog.Type = grantLog.Type;
            newGrantLog.Channel = grantLog.Channel;
            newGrantLog.Frequency = grantLog.Frequency;
            newGrantLog.TalkgroupID = 0;
            newGrantLog.TalkgroupDescription = string.Empty;
            newGrantLog.RadioID = grantLog.TalkgroupID;
            newGrantLog.RadioDescription = grantLog.TalkgroupDescription;

            return newGrantLog;
        }

        public static void UpdateCounters(ICounterRecord record, ActionTypes action)
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

        public static void UpdateHitCounts(IRecord record, ActionTypes action)
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
