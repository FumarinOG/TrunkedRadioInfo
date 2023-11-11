using DataLibrary.TempData;
using FileService.Interfaces;
using ObjectLibrary;
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
    public abstract class FileServiceBase : ServiceBase
    {
        protected SystemInfo _systemInfo;
        protected Tower _tower;
        protected DateTime _fileDate;
        protected bool _processRadios = true;
        protected bool _processTowerFrequencies = true;

        protected readonly int _systemID;
        protected readonly int _towerNumber;
        protected readonly UploadFileModel _uploadFile;

        protected readonly IRadioService _radioService;
        protected readonly IRadioHistoryService _radioHistoryService;
        protected readonly ISystemInfoService _systemInfoService;
        protected readonly ITalkgroupService _talkgroupService;
        protected readonly ITalkgroupHistoryService _talkgroupHistoryService;
        protected readonly ITalkgroupRadioService _talkgroupRadioService;
        protected readonly ITowerService _towerService;
        protected readonly ITowerFrequencyRadioService _towerFrequencyRadioService;
        protected readonly ITowerFrequencyTalkgroupService _towerFrequencyTalkgroupService;
        protected readonly ITowerFrequencyUsageService _towerFrequencyUsageService;
        protected readonly ITowerRadioService _towerRadioService;
        protected readonly ITowerTalkgroupService _towerTalkgroupService;
        protected readonly ITowerTalkgroupRadioService _towerTalkgroupRadioService;
        protected readonly IMergeService _mergeService;

        protected readonly ITempService<TempSystemInfo, SystemInfo> _tempSystemInfoService;
        protected readonly ITempService<TempRadio, Radio> _tempRadioService;
        protected readonly ITempService<TempRadioHistory, RadioHistory> _tempRadioHistoryService;
        protected readonly ITempService<TempTalkgroup, Talkgroup> _tempTalkgroupService;
        protected readonly ITempService<TempTalkgroupHistory, TalkgroupHistory> _tempTalkgroupHistoryService;
        protected readonly ITempService<TempTalkgroupRadio, TalkgroupRadio> _tempTalkgroupRadioService;
        protected readonly ITempService<TempTower, Tower> _tempTowerService;
        protected readonly ITempService<TempTowerFrequencyRadio, TowerFrequencyRadio> _tempTowerFrequencyRadioService;
        protected readonly ITempService<TempTowerFrequencyTalkgroup, TowerFrequencyTalkgroup> _tempTowerFrequencyTalkgroupService;
        protected readonly ITempService<TempTowerFrequencyUsage, TowerFrequencyUsage> _tempTowerFrequencyUsageService;
        protected readonly ITempService<TempTowerRadio, TowerRadio> _tempTowerRadioService;
        protected readonly ITempService<TempTowerTalkgroup, TowerTalkgroup> _tempTowerTalkgroupService;
        protected readonly ITempService<TempTowerTalkgroupRadio, TowerTalkgroupRadio> _tempTowerTalkgroupRadioService;

        protected ICollection<Radio> _radios;
        protected ICollection<Talkgroup> _talkgroups;
        protected ICollection<TalkgroupRadio> _talkgroupRadios;
        protected ICollection<TowerFrequencyRadio> _towerFrequencyRadios;
        protected ICollection<TowerFrequencyTalkgroup> _towerFrequencyTalkgroups;
        protected ICollection<TowerFrequencyUsage> _towerFrequencyUsage;
        protected ICollection<TowerRadio> _towerRadios;
        protected ICollection<TowerTalkgroup> _towerTalkgroups;
        protected ICollection<TowerTalkgroupRadio> _towerTalkgroupRadios;
        protected ICollection<RadioHistory> _radioHistory;
        protected ICollection<TalkgroupHistory> _talkgroupHistory;

        protected FileServiceBase(int systemID, int towerNumber, UploadFileModel uploadFile, IRadioService radioService,
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
            ITempService<TempTowerRadio, TowerRadio> tempTowerRadioService,
            ITempService<TempTowerTalkgroup, TowerTalkgroup> tempTowerTalkgroupService,
            ITempService<TempTowerTalkgroupRadio, TowerTalkgroupRadio> tempTowerTalkgroupRadioService)
        {
            _systemID = systemID;
            _towerNumber = towerNumber;
            _uploadFile = uploadFile;
            _radioService = radioService;
            _radioHistoryService = radioHistoryService;
            _systemInfoService = systemInfoService;
            _talkgroupService = talkgroupService;
            _talkgroupHistoryService = talkgroupHistoryService;
            _talkgroupRadioService = talkgroupRadioService;
            _towerService = towerService;
            _towerFrequencyRadioService = towerFrequencyRadioService;
            _towerFrequencyTalkgroupService = towerFrequencyTalkgroupService;
            _towerFrequencyUsageService = towerFrequencyUsageService;
            _towerRadioService = towerRadioService;
            _towerTalkgroupService = towerTalkgroupService;
            _towerTalkgroupRadioService = towerTalkgroupRadioService;
            _mergeService = mergeService;

            _tempSystemInfoService = tempSystemInfoService;
            _tempRadioService = tempRadioService;
            _tempRadioHistoryService = tempRadioHistoryService;
            _tempTalkgroupService = tempTalkgroupService;
            _tempTalkgroupHistoryService = tempTalkgroupHistoryService;
            _tempTalkgroupRadioService = tempTalkgroupRadioService;
            _tempTowerService = tempTowerService;
            _tempTowerFrequencyUsageService = tempTowerFrequencyUsageService;
            _tempTowerFrequencyRadioService = tempTowerFrequencyRadioService;
            _tempTowerFrequencyTalkgroupService = tempTowerFrequencyTalkgroupService;
            _tempTowerRadioService = tempTowerRadioService;
            _tempTowerTalkgroupService = tempTowerTalkgroupService;
            _tempTowerTalkgroupRadioService = tempTowerTalkgroupRadioService;

            _fileDate = ProcessedFileService.Factory.GetFileDate(uploadFile);
        }

        public async Task FillDataAsync()
        {
            _uploadFile.Status = FileStatus.LoadingSystemInfo;
            _systemInfo = await _systemInfoService.GetAsync(_systemID);
            _uploadFile.Status = FileStatus.LoadingTower;
            _tower = await _towerService.GetForTowerAsync(_systemID, _towerNumber);
            _uploadFile.Status = FileStatus.LoadingTalkgroups;
            _talkgroups = (await _talkgroupService.GetForSystemAsync(_systemID)).ToList();
            _uploadFile.Status = FileStatus.LoadingTalkgroupHistory;
            _talkgroupHistory = (await _talkgroupHistoryService.GetForSystemAsync(_systemID)).ToList();
            _uploadFile.Status = FileStatus.LoadingTowerTalkgroups;
            _towerTalkgroups = (await _towerTalkgroupService.GetForTowerImportAsync(_systemID, _towerNumber, _fileDate)).ToList();

            if (_processRadios)
            {
                _uploadFile.Status = FileStatus.LoadingRadios;
                _radios = (await _radioService.GetForSystemAsync(_systemID)).ToList();
                _uploadFile.Status = FileStatus.LoadingRadioHistory;
                _radioHistory = (await _radioHistoryService.GetForSystemAsync(_systemID)).ToList();
                _uploadFile.Status = FileStatus.LoadingTalkgroupRadios;
                _talkgroupRadios = (await _talkgroupRadioService.GetForSystemAsync(_systemID, _fileDate)).ToList();
                _uploadFile.Status = FileStatus.LoadingTowerRadios;
                _towerRadios = (await _towerRadioService.GetForTowerAsync(_systemID, _towerNumber, _fileDate)).ToList();
                _uploadFile.Status = FileStatus.LoadingTowerTalkgroupRadios;
                _towerTalkgroupRadios = (await _towerTalkgroupRadioService.GetForTowerAsync(_systemID, _towerNumber, _fileDate)).ToList();
            }

            if (_processTowerFrequencies)
            {
                _uploadFile.Status = FileStatus.LoadingTowerFrequencies;
                _towerFrequencyUsage = (await GetFrequenciesForTowerAsync(_systemID, _towerNumber, _fileDate)).ToList();
                _uploadFile.Status = FileStatus.LoadingTowerFrequencyRadios;
                _towerFrequencyRadios = (await _towerFrequencyRadioService.GetForTowerAsync(_systemID, _towerNumber, _fileDate)).ToList();
                _uploadFile.Status = FileStatus.LoadingTowerFrequencyTalkgroups;
                _towerFrequencyTalkgroups = (await _towerFrequencyTalkgroupService.GetForTowerAsync(_systemID, _towerNumber, _fileDate)).ToList();
            }
        }

        protected async Task<IEnumerable<TowerFrequencyUsage>> GetFrequenciesForTowerAsync(int systemID, int towerNumber, DateTime date)
        {
            if (_towerFrequencyUsageService == null)
            {
                return CreatList<TowerFrequencyUsage>();
            }

            return await _towerFrequencyUsageService.GetFrequenciesForTowerAsync(systemID, towerNumber, date);
        }

        protected async Task WriteDataAsync(Action<string, string, int, int> updateProgress)
        {
            try
            {
                await WriteTempDataAsync(_tempSystemInfoService, new List<SystemInfo> { _systemInfo }, updateProgress);

                if (_processRadios)
                {
                    await WriteTempDataAsync(_tempRadioService, _radios, updateProgress);
                    await WriteTempDataAsync(_tempRadioHistoryService, _radioHistory, updateProgress);
                    await WriteTempDataAsync(_tempTalkgroupRadioService, _talkgroupRadios, updateProgress);
                    await WriteTempDataAsync(_tempTowerTalkgroupRadioService, _towerTalkgroupRadios, updateProgress);
                    await WriteTempDataAsync(_tempTowerRadioService, _towerRadios, updateProgress);
                }

                if (_processTowerFrequencies)
                {
                    await WriteTempDataAsync(_tempTowerFrequencyRadioService, _towerFrequencyRadios, updateProgress);
                    await WriteTempDataAsync(_tempTowerFrequencyTalkgroupService, _towerFrequencyTalkgroups, updateProgress);
                    await WriteTempDataAsync(_tempTowerFrequencyUsageService, _towerFrequencyUsage, updateProgress);
                }

                await WriteTempDataAsync(_tempTalkgroupService, _talkgroups, updateProgress);
                await WriteTempDataAsync(_tempTalkgroupHistoryService, _talkgroupHistory, updateProgress);
                await WriteTempDataAsync(_tempTowerTalkgroupService, _towerTalkgroups, updateProgress);
                await WriteTempDataAsync(_tempTowerService, new List<Tower> { _tower }, updateProgress);
            }
            catch (Exception exception)
            {
                _logger.Error(exception, "Error writing temp tables");
                await _mergeService.DeleteTempTablesAsync(_sessionID, updateProgress);
                throw;
            }

            await _mergeService.MergeRecordsAsync(_sessionID, updateProgress);
        }
    }
}
