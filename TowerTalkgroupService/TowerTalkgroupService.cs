using AutoMapper;
using DataLibrary;
using DataLibrary.Interfaces;
using ObjectLibrary;
using ObjectLibrary.Interfaces;
using ServiceCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TowerService;
using static ServiceCommon.Common;
using static ServiceCommon.Factory;

namespace TowerTalkgroupService
{
    public sealed class TowerTalkgroupService : ServiceBase, ITowerTalkgroupService
    {
        private readonly ITowerTalkgroupRepository _towerTalkgroupRepo;

        public TowerTalkgroupService(ITowerTalkgroupRepository towerTalkgroupRepository) : base((mc) => CreateMapping(mc)) =>
            _towerTalkgroupRepo = towerTalkgroupRepository;

        public static void CreateMapping(IMapperConfigurationExpression config)
        {
            config.CreateMap<TowerTalkgroup, TalkgroupTowerViewModel>()
                .ForMember(dest => dest.VoiceCount, opt => opt.MapFrom(src => src.VoiceGrantCount))
                .ForMember(dest => dest.EncryptedCount, opt => opt.MapFrom(src => src.EncryptedVoiceGrantCount));
            config.CreateMap<TowerTalkgroup, TowerTalkgroupViewModel>()
                .ForMember(dest => dest.VoiceCount, opt => opt.MapFrom(src => src.VoiceGrantCount))
                .ForMember(dest => dest.EncryptedCount, opt => opt.MapFrom(src => src.EncryptedVoiceGrantCount));
        }

        public async Task<IEnumerable<TowerTalkgroup>> GetForTowerAsync(int systemID, int towerNumber) =>
            await _towerTalkgroupRepo.GetForTowerAsync(systemID, towerNumber);

        public async Task<IEnumerable<TowerTalkgroup>> GetForTowerImportAsync(int systemID, int towerNumber) =>
            await _towerTalkgroupRepo.GetForTowerImportAsync(systemID, towerNumber);

        public async Task<IEnumerable<TowerTalkgroup>> GetForTowerImportAsync(int systemID, int towerNumber, DateTime date) =>
            await _towerTalkgroupRepo.GetForTowerImportAsync(systemID, towerNumber, date);

        public async Task<IEnumerable<TowerTalkgroup>> GetTalkgroupsForTowerAsync(int systemID, int towerNumber) =>
            await _towerTalkgroupRepo.GetTalkgroupsForTowerAsync(systemID, towerNumber);

        public async Task<int> GetTalkgroupsForTowerCountAsync(int systemID, int towerNumber) =>
            await _towerTalkgroupRepo.GetTalkgroupsForTowerCountAsync(systemID, towerNumber);

        public async Task<(IEnumerable<TowerTalkgroupViewModel> towerTalkgroups, int recordCount)> GetTalkgroupsForTowerAsync(string systemID, int towerNumber,
            FilterDataModel filterData)
        {
            CheckFilterRanges(filterData, Talkgroup.MIN_TALKGROUP_ID, Talkgroup.MAX_TALKGROUP_ID);

            var (towerTalkgroups, recordCount) = await _towerTalkgroupRepo.GetTalkgroupsForTowerAsync(systemID, towerNumber, _mapper.Map<FilterData>(filterData));

            return (towerTalkgroups.Select(ttg => _mapper.Map<TowerTalkgroupViewModel>(ttg)), recordCount);
        }

        public async Task<IEnumerable<TowerTalkgroup>> GetTalkgroupsForTowerByDateAsync(int systemID, int towerNumber) =>
            await _towerTalkgroupRepo.GetTalkgroupsForTowerByDateAsync(systemID, towerNumber);

        public async Task<IEnumerable<TowerTalkgroup>> GetTowersForTalkgroupAsync(int systemID, int talkgroupID) =>
            await _towerTalkgroupRepo.GetTowersForTalkgroupAsync(systemID, talkgroupID);

        public async Task<int> GetTowersForTalkgroupCountAsync(int systemID, int talkgroupID) =>
            await _towerTalkgroupRepo.GetTowersForTalkgroupCountAsync(systemID, talkgroupID);

        public async Task<(IEnumerable<TalkgroupTowerViewModel> talkgroupTowers, int recordCount)> GetTowersForTalkgroupViewAsync(string systemID,
            int talkgroupID, FilterDataModel filterData)
        {
            CheckFilterRanges(filterData, Tower.MIN_TOWER_NUMBER, Tower.MAX_TOWER_NUMBER);

            var (towerTalkgroups, recordCount) = await _towerTalkgroupRepo.GetTowersForTalkgroupsAsync(systemID, talkgroupID,
                _mapper.Map<FilterData>(filterData));

            return (towerTalkgroups.Select(ttg => _mapper.Map<TalkgroupTowerViewModel>(ttg)), recordCount);
        }

        public async Task<IEnumerable<TowerViewModel>> GetTowerListForTalkgroupAsync(string systemID, int talkgroupID, FilterDataModel filterData)
        {
            CheckFilterRanges(filterData, Talkgroup.MIN_TALKGROUP_ID, Talkgroup.MAX_TALKGROUP_ID);

            return (await _towerTalkgroupRepo.GetTowerListForTalkgroupAsync(systemID, talkgroupID, _mapper.Map<FilterData>(filterData)))
                .Select(ttg => _mapper.Map<TowerViewModel>(ttg));
        }

        public async Task<IEnumerable<DateModel>> GetDateListForTowerTalkgroupAsync(string systemID, int talkgroupID, int towerNumber, FilterDataModel filterData)
        {
            CheckFilterRanges(filterData, Talkgroup.MIN_TALKGROUP_ID, Talkgroup.MAX_TALKGROUP_ID);

            var dates = (await _towerTalkgroupRepo.GetDateListForTowerTalkgroupAsync(systemID, talkgroupID, towerNumber, _mapper.Map<FilterData>(filterData)))
                .Select(ttg => CreateDateModel(ttg));

            return dates;
        }

        public void ProcessRecord(int systemID, int towerID, int talkgroupID, DateTime timeStamp, string action, ICollection<TowerTalkgroup> towerTalkgroups,
            Action<ICounterRecord, ActionTypes> updateCounters)
        {
            var towerTalkgroup = towerTalkgroups.SingleOrDefault(ttg => ttg.TalkgroupID == talkgroupID && ttg.Date == timeStamp.Date);

            if (towerTalkgroup == null)
            {
                towerTalkgroup = CreateTalkgroup(systemID, towerID, talkgroupID, timeStamp);
                towerTalkgroups.Add(towerTalkgroup);
            }

            updateCounters(towerTalkgroup, action.GetEnumFromDescription<ActionTypes>());
            CheckDates(towerTalkgroup, timeStamp);
        }

        public TowerTalkgroup CreateTalkgroup(int systemID, int towerNumber, int talkgroupID, DateTime timeStamp)
        {
            var towerTalkgroup = Create<TowerTalkgroup>();

            towerTalkgroup.SystemID = systemID;
            towerTalkgroup.TowerNumber = towerNumber;
            towerTalkgroup.TalkgroupID = talkgroupID;
            towerTalkgroup.Date = timeStamp.Date;
            towerTalkgroup.AffiliationCount = 0;
            towerTalkgroup.DeniedCount = 0;
            towerTalkgroup.VoiceGrantCount = 0;
            towerTalkgroup.FirstSeen = timeStamp;
            towerTalkgroup.LastSeen = timeStamp;

            return towerTalkgroup;
        }
    }
}
