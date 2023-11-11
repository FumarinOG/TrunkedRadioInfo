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
using TowerRadioService;
using TowerTalkgroupService;
using static ServiceCommon.Common;
using static ServiceCommon.Factory;

namespace TowerTalkgroupRadioService
{
    public sealed class TowerTalkgroupRadioService : ServiceBase, ITowerTalkgroupRadioService
    {
        private readonly ITowerTalkgroupRadioRepository _towerTalkgroupRadioRepo;

        public TowerTalkgroupRadioService(ITowerTalkgroupRadioRepository towerTalkgroupRadioRepository) : base((mc) => CreateMapping(mc)) =>
            _towerTalkgroupRadioRepo = towerTalkgroupRadioRepository;

        public static void CreateMapping(IMapperConfigurationExpression config)
        {
            config.CreateMap<TowerTalkgroupRadio, TowerRadioViewModel>()
                .ForMember(dest => dest.VoiceCount, opt => opt.MapFrom(src => (src.VoiceGrantCount + src.EmergencyVoiceGrantCount)))
                .ForMember(dest => dest.EncryptedCount, opt => opt.MapFrom(src => src.EncryptedVoiceGrantCount))
                .ForMember(dest => dest.DataCount, opt => opt.MapFrom(src => (src.DataCount + src.PrivateDataCount)));
            config.CreateMap<TowerTalkgroupRadio, TowerTalkgroupViewModel>()
                .ForMember(dest => dest.VoiceCount, opt => opt.MapFrom(src => (src.VoiceGrantCount + src.EmergencyVoiceGrantCount)))
                .ForMember(dest => dest.EncryptedCount, opt => opt.MapFrom(src => src.EncryptedVoiceGrantCount));
        }

        public async Task<IEnumerable<TowerTalkgroupRadio>> GetForTowerAsync(int systemID, int towerNumber) =>
            await _towerTalkgroupRadioRepo.GetForTowerAsync(systemID, towerNumber);

        public async Task<IEnumerable<TowerTalkgroupRadio>> GetForTowerAsync(int systemID, int towerNumber, DateTime date) =>
            await _towerTalkgroupRadioRepo.GetForTowerAsync(systemID, towerNumber, date);

        public async Task<IEnumerable<TowerTalkgroupRadio>> GetTowersForTalkgroupRadioAsync(int systemID, int talkgroupID, int radioID) =>
            await _towerTalkgroupRadioRepo.GetTowersForTalkgroupRadioAsync(systemID, talkgroupID, radioID);

        public async Task<IEnumerable<TowerTalkgroupRadio>> GetTalkgroupsForRadioWithDatesAsync(int systemID, int radioID) =>
            await _towerTalkgroupRadioRepo.GetTalkgroupsForRadioWithDatesAsync(systemID, radioID);

        public async Task<IEnumerable<TowerTalkgroupRadio>> GetRadiosForTalkgroupWithDatesAsync(int systemID, int talkgroupID) =>
            await _towerTalkgroupRadioRepo.GetRadiosForTalkgroupWithDatesAsync(systemID, talkgroupID);

        public async Task<(IEnumerable<TowerRadioViewModel> towerRadios, int recordCount)> GetRadiosForTowerTalkgroupAsync(string systemID, int talkgroupID,
            int towerNumber, DateTime? date, FilterDataModel filterData)
        {
            CheckFilterRanges(filterData, Radio.MIN_RADIO_ID, Radio.MAX_RADIO_ID);

            if (date != null)       // If a date is selected, override whatever date filter is selected with the
            {                       // date specified
                filterData.DateFrom = date;
                filterData.DateTo = date;
            }

            var (towerRadios, recordCount) = await _towerTalkgroupRadioRepo.GetRadiosForTalkgroupTowerAsync(systemID, talkgroupID, towerNumber,
                _mapper.Map<FilterData>(filterData));

            return (towerRadios.Select(tr => _mapper.Map<TowerRadioViewModel>(tr)), recordCount);
        }

        public async Task<(IEnumerable<TowerTalkgroupViewModel> towerTalkgroups, int recordCount)> GetTalkgroupsForTowerRadioAsync(string system, int radioID,
            int towerNumber, DateTime? date, FilterDataModel filterData)
        {
            CheckFilterRanges(filterData, Talkgroup.MIN_TALKGROUP_ID, Talkgroup.MAX_TALKGROUP_ID);

            if (date != null)       // If a date is selected, override whatever date filter is selected with the
            {                       // date specified
                filterData.DateFrom = date;
                filterData.DateTo = date;
            }

            var (towerTalkgroups, recordCount) = await _towerTalkgroupRadioRepo.GetTalkgroupsForTowerRadioAsync(system, radioID, towerNumber,
                _mapper.Map<FilterData>(filterData));

            return (towerTalkgroups.Select(ttg => _mapper.Map<TowerTalkgroupViewModel>(ttg)), recordCount);
        }

        public void ProcessRecord(int systemID, int towerID, int talkgroupID, int radioID, ICollection<TowerTalkgroupRadio> towerTalkgroupRadios,
            DateTime timeStamp, string action, Action<ICounterRecord, ActionTypes> updateCounters)
        {
            if ((talkgroupID == 0) || (talkgroupID == -1))    // Talkgroup ID -1 is a Group Data action
            {
                return;
            }

            var towerTalkgroupRadio = towerTalkgroupRadios.SingleOrDefault(ttgr => ttgr.TalkgroupID == talkgroupID && ttgr.RadioID == radioID &&
                ttgr.Date == timeStamp.Date);

            if (towerTalkgroupRadio == null)
            {
                towerTalkgroupRadio = CreateTowerTalkgroupRadio(systemID, towerID, talkgroupID, radioID, timeStamp);
                towerTalkgroupRadios.Add(towerTalkgroupRadio);
            }

            updateCounters(towerTalkgroupRadio, action.GetEnumFromDescription<ActionTypes>());
            CheckDates(towerTalkgroupRadio, timeStamp);
        }

        public TowerTalkgroupRadio CreateTowerTalkgroupRadio(int systemID, int towerNumber, int talkgroupID, int radioID, DateTime timeStamp)
        {
            var towerTalkgroupRadio = Create<TowerTalkgroupRadio>();

            towerTalkgroupRadio.SystemID = systemID;
            towerTalkgroupRadio.TowerNumber = towerNumber;
            towerTalkgroupRadio.TalkgroupID = talkgroupID;
            towerTalkgroupRadio.RadioID = radioID;
            towerTalkgroupRadio.Date = timeStamp.Date;
            towerTalkgroupRadio.AffiliationCount = 0;
            towerTalkgroupRadio.DeniedCount = 0;
            towerTalkgroupRadio.VoiceGrantCount = 0;
            towerTalkgroupRadio.FirstSeen = timeStamp;
            towerTalkgroupRadio.LastSeen = timeStamp;

            return towerTalkgroupRadio;
        }
    }
}
