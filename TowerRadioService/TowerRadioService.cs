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

namespace TowerRadioService
{
    public sealed class TowerRadioService : ServiceBase, ITowerRadioService
    {
        private readonly ITowerRadioRepository _towerRadioRepo;

        public TowerRadioService(ITowerRadioRepository towerRadioRepository) : base((mc) => CreateMapping(mc)) => _towerRadioRepo = towerRadioRepository;

        public static void CreateMapping(IMapperConfigurationExpression config)
        {
            config.CreateMap<TowerRadio, RadioTowerViewModel>()
                .ForMember(dest => dest.VoiceCount, opt => opt.MapFrom(src => src.VoiceGrantCount))
                .ForMember(dest => dest.EncryptedCount, opt => opt.MapFrom(src => src.EncryptedVoiceGrantCount));
            config.CreateMap<TowerRadio, TowerRadioViewModel>()
                .ForMember(dest => dest.VoiceCount, opt => opt.MapFrom(src => src.VoiceGrantCount))
                .ForMember(dest => dest.EncryptedCount, opt => opt.MapFrom(src => src.EncryptedVoiceGrantCount))
                .ForMember(dest => dest.FirstSeen, opt => opt.Ignore())
                .ForMember(dest => dest.LastSeen, opt => opt.Ignore());
        }

        public async Task<IEnumerable<TowerRadio>> GetForTowerAsync(int systemID, int towerNumber, DateTime date) =>
            await _towerRadioRepo.GetForTowerAsync(systemID, towerNumber, date);

        public async Task<IEnumerable<TowerRadio>> GetRadiosForTowerAsync(int systemID, int towerNumber) =>
            await _towerRadioRepo.GetRadiosForTowerAsync(systemID, towerNumber);

        public async Task<(IEnumerable<TowerRadioViewModel> towerRadios, int recordCount)> GetRadiosForTowerAsync(string systemID, int towerNumber,
            FilterDataModel filterData)
        {
            CheckFilterRanges(filterData, Radio.MIN_RADIO_ID, Radio.MAX_RADIO_ID);

            var (towerRadios, recordCount) = await _towerRadioRepo.GetRadiosForTowerAsync(systemID, towerNumber, _mapper.Map<FilterData>(filterData));

            return (towerRadios.Select(tr => _mapper.Map<TowerRadioViewModel>(tr)), recordCount);
        }

        public async Task<int> GetRadiosForTowerCountAsync(int systemID, int towerNumber) => await _towerRadioRepo.GetRadiosForTowerCountAsync(systemID, towerNumber);

        public async Task<IEnumerable<TowerRadio>> GetRadiosForTowerByDateAsync(int systemID, int towerNumber) =>
            await _towerRadioRepo.GetRadiosForTowerByDateAsync(systemID, towerNumber);

        public async Task<IEnumerable<TowerRadio>> GetTowersForRadioAsync(int systemID, int radioID) => await _towerRadioRepo.GetTowersForRadioAsync(systemID, radioID);

        public async Task<int> GetTowersForRadioCountAsync(int systemID, int radioID) => await _towerRadioRepo.GetTowersForRadioCountAsync(systemID, radioID);

        public async Task<(IEnumerable<RadioTowerViewModel> radioTowers, int recordCount)> GetTowersForRadioAsync(string systemID, int radioID,
            FilterDataModel filterData)
        {
            CheckFilterRanges(filterData, Tower.MIN_TOWER_NUMBER, Tower.MAX_TOWER_NUMBER);

            var (towerRadios, recordCount) = await _towerRadioRepo.GetTowersForRadioAsync(systemID, radioID, _mapper.Map<FilterData>(filterData));

            return (towerRadios.Select(tr => _mapper.Map<RadioTowerViewModel>(tr)), recordCount);
        }

        public async Task<IEnumerable<TowerViewModel>> GetTowerListForRadioAsync(string systemID, int radioID, FilterDataModel filterData)
        {
            CheckFilterRanges(filterData, Radio.MIN_RADIO_ID, Radio.MAX_RADIO_ID);

            return (await _towerRadioRepo.GetTowerListForRadioAsync(systemID, radioID, _mapper.Map<FilterData>(filterData)))
                .Select(tr => _mapper.Map<TowerViewModel>(tr));
        }

        public async Task<IEnumerable<DateModel>> GetDateListForTowerRadioAsync(string systemID, int radioID, int towerNumber, FilterDataModel filterData)
        {
            CheckFilterRanges(filterData, Radio.MIN_RADIO_ID, Radio.MAX_RADIO_ID);

            var dates = (await _towerRadioRepo.GetDateListForTowerRadioAsync(systemID, radioID, towerNumber, _mapper.Map<FilterData>(filterData)))
                .Select(tr => CreateDateModel(tr));

            return dates;
        }

        public void ProcessRecord(int systemID, int towerID, int radioID, DateTime timeStamp, string action, ICollection<TowerRadio> towerRadios,
            Action<ICounterRecord, ActionTypes> updateCounters)
        {
            var towerRadio = towerRadios.SingleOrDefault(tr => tr.RadioID == radioID && tr.Date == timeStamp.Date);

            if (towerRadio == null)
            {
                towerRadio = CreateRadio(systemID, towerID, radioID, timeStamp);
                towerRadios.Add(towerRadio);
            }

            updateCounters(towerRadio, action.GetEnumFromDescription<ActionTypes>());
            CheckDates(towerRadio, timeStamp);
        }

        public TowerRadio CreateRadio(int systemID, int towerNumber, int radioID, DateTime timeStamp)
        {
            var towerRadio = Create<TowerRadio>();

            towerRadio.SystemID = systemID;
            towerRadio.TowerNumber = towerNumber;
            towerRadio.RadioID = radioID;
            towerRadio.Date = timeStamp.Date;
            towerRadio.AffiliationCount = 0;
            towerRadio.DeniedCount = 0;
            towerRadio.VoiceGrantCount = 0;
            towerRadio.FirstSeen = timeStamp;
            towerRadio.LastSeen = timeStamp;

            return towerRadio;
        }
    }
}
