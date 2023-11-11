using DataLibrary.Interfaces;
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
    public class PatchLogService : FileServiceBase, IService
    {
        private readonly ITempService<TempPatch, Patch> _tempPatchService;

        private readonly ICollection<Patch> _patches = CreatList<Patch>();
        private readonly ICollection<Patch> _unprocessedPatches = CreatList<Patch>();
        private readonly ICollection<Patch> _currentPatches;

        private const string TEXT_ADDED_PATCH = "Added Patch:";
        private const string TEXT_REMOVED_PATCH = "Removed Patch:";
        private const string TEXT_TO = " --> ";
        private const string TEXT_SPACE = " ";
        private const string TEXT_OPEN_PARENTHESIS = "(";

        public PatchLogService(int systemID, int towerNumber, UploadFileModel uploadFile, IPatchRepository patchRepository,
           ITempService<TempPatch, Patch> tempPatchService, IRadioService radioService, IRadioHistoryService radioHistoryService,
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
            _tempPatchService = tempPatchService;
            _currentPatches = patchRepository.GetForSystem(_systemInfo.ID).ToList();
            _processRadios = false;
            _processTowerFrequencies = false;
        }

        public async Task<int> ProcessRecordsAsync(IEnumerable<IRecord> patchLogs, Action<string, string, int, int> updateProgress, Action<bool> completedTasks, bool showCompleted)
        {
            var count = 0;
            var recordList = (IList<PatchLog>)patchLogs;

            await Task.Run(() =>
            {
                foreach (var patchLog in recordList)
                {
                    var patch = Create<Patch>();

                    patch.SystemID = _systemInfo.ID;
                    patch.TowerNumber = patchLog.TowerNumber;
                    patch.Date = patchLog.TimeStamp.Date;
                    patch.FirstSeen = patchLog.TimeStamp;
                    patch.LastSeen = patchLog.TimeStamp;
                    ParseDescription(patchLog, patch);

                    if (patchLog.Action == PatchLog.Actions.Added)
                    {
                        _unprocessedPatches.Add(patch);
                    }

                    count++;
                    updateProgress("Processing patch log", $"{count:#,##0} records processed", count, recordList.Count);
                }

                ProcessPatches();
                Write(updateProgress);
            });

            completedTasks(showCompleted);
            return count;
        }

        private static void ParseDescription(PatchLog patchLog, Patch patch)
        {
            if (patchLog.Description.Substring(0, TEXT_ADDED_PATCH.Length).Equals(TEXT_ADDED_PATCH, StringComparison.OrdinalIgnoreCase))
            {
                patchLog.Action = PatchLog.Actions.Added;
            }
            else if (patchLog.Description.Substring(0, TEXT_REMOVED_PATCH.Length).Equals(TEXT_REMOVED_PATCH, StringComparison.OrdinalIgnoreCase))
            {
                patchLog.Action = PatchLog.Actions.Removed;
            }
            else
            {
                throw new Exception("Patch action not valid");
            }

            GetTalkgroupIDs(patchLog, patch);
        }

        private static void GetTalkgroupIDs(PatchLog patchLog, Patch patch)
        {

            switch (patchLog.Action)
            {
                case PatchLog.Actions.Added:
                    ParseTalkgroupID(patchLog.Description, TEXT_ADDED_PATCH, patch);
                    break;

                case PatchLog.Actions.Removed:
                    ParseTalkgroupID(patchLog.Description, TEXT_REMOVED_PATCH, patch);
                    break;
            }
        }

        private static void ParseTalkgroupID(string value, string staticText, Patch patch)
        {
            var trimmed = value.Substring(staticText.Length).Trim();
            var fromTalkgroupText = trimmed.Substring(0, trimmed.IndexOf(" ", StringComparison.OrdinalIgnoreCase));
            var fromTalkgroupDescription = trimmed.Substring(trimmed.IndexOf(TEXT_OPEN_PARENTHESIS, StringComparison.OrdinalIgnoreCase) + 1,
                trimmed.IndexOf(TEXT_TO, StringComparison.OrdinalIgnoreCase) - trimmed.IndexOf(TEXT_OPEN_PARENTHESIS, StringComparison.OrdinalIgnoreCase) - 2);

            trimmed = trimmed.Substring(trimmed.IndexOf(TEXT_TO, StringComparison.OrdinalIgnoreCase) + TEXT_TO.Length).Trim();

            var toTalkgroupText = trimmed.Substring(0, trimmed.IndexOf(TEXT_SPACE, StringComparison.OrdinalIgnoreCase));
            var toTalkgroupDescription = trimmed.Substring(trimmed.IndexOf(TEXT_OPEN_PARENTHESIS, StringComparison.OrdinalIgnoreCase) + 1,
                trimmed.Length - trimmed.IndexOf(TEXT_OPEN_PARENTHESIS, StringComparison.OrdinalIgnoreCase) - 2);

            if (int.TryParse(fromTalkgroupText, out int fromTalkgroupID))
            {
                patch.FromTalkgroupID = fromTalkgroupID;
                patch.FromTalkgroupName = fromTalkgroupDescription;
            }
            else
            {
                throw new Exception("Invalid patch From Talkgroup ID");
            }

            if (int.TryParse(toTalkgroupText, out int toTalkgroupID))
            {
                patch.ToTalkgroupID = toTalkgroupID;
                patch.ToTalkgroupName = toTalkgroupDescription;
            }
            else
            {
                throw new Exception("Invalid patch From Talkgroup ID");
            }
        }

        private void ProcessPatches()
        {
            var patches = _unprocessedPatches.GroupBy(up => new { up.SystemID, up.TowerNumber, up.FromTalkgroupID, up.ToTalkgroupID, up.Date })
                .Select(p => new
                             {
                                p.Key.SystemID,
                                p.Key.FromTalkgroupID,
                                p.Key.ToTalkgroupID,
                                p.Key.TowerNumber,
                                p.Key.Date,
                                FirstSeen = p.Select(pl => pl.FirstSeen).FirstOrDefault(),
                                LastSeen = p.Select(pl => pl.LastSeen).OrderByDescending(pld => pld).FirstOrDefault(),
                                HitCount = p.Count()
                             }).ToList();

            foreach (var unprocessedPatch in patches)
            {
                var patch = _currentPatches.SingleOrDefault(p => p.TowerNumber == unprocessedPatch.TowerNumber && p.FromTalkgroupID == unprocessedPatch.FromTalkgroupID &&
                    p.ToTalkgroupID == unprocessedPatch.ToTalkgroupID && p.Date == unprocessedPatch.Date);

                if (patch == null)
                {
                    patch = Create<Patch>();

                    patch.SystemID = unprocessedPatch.SystemID;
                    patch.TowerNumber = unprocessedPatch.TowerNumber;
                    patch.Date = unprocessedPatch.Date;
                    patch.FromTalkgroupID = unprocessedPatch.FromTalkgroupID;
                    patch.ToTalkgroupID = unprocessedPatch.ToTalkgroupID;
                }
                else
                {
                    patch.IsNew = false;
                }

                patch.FirstSeen = unprocessedPatch.FirstSeen;
                patch.LastSeen = unprocessedPatch.LastSeen;
                patch.HitCount += unprocessedPatch.HitCount;

                _patches.Add(patch);
                ProcessTalkgroup(patch.FromTalkgroupID, patch.FromTalkgroupName, patch.ToTalkgroupID, patch.ToTalkgroupName, patch.Date, string.Empty, UpdateCounters,
                    UpdateHitCounts);
            }
        }

        public int GetCountForSystem()
        {
            throw new NotImplementedException();
        }

        private void Write(Action<string, string, int, int> updateProgress)
        {
            try
            {
                var changeCount = _patches.Count(p => p.IsNew || p.IsDirty);

                updateProgress("Writing temporary patch log", GetWrittenText(changeCount), changeCount, changeCount);
                WriteTempData(_tempTalkgroupService, _talkgroups, updateProgress);
                WriteTempData(_tempTalkgroupHistoryService, _talkgroupHistory, updateProgress);
                WriteTempData(_tempPatchService, _patches, updateProgress);
            }
            catch
            {
                _mergeService.DeleteTempTables(_sessionID, updateProgress);
                throw;
            }

            _mergeService.MergeRecords(_sessionID, updateProgress);
        }

        private static void UpdateCounters(ICounterRecord record, ActionTypes action)
        {
            // Nothing to count
        }

        private static void UpdateHitCounts(IRecord record, ActionTypes action)
        {
            // Nothing to count
        }
    }
}
