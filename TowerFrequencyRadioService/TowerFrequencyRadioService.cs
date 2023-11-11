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
using static ServiceCommon.Common;
using static ServiceCommon.Factory;

namespace TowerFrequencyRadioService
{
    public sealed class TowerFrequencyRadioService : ServiceBase, ITowerFrequencyRadioService
    {
        private readonly ITowerFrequencyRadioRepository _towerFrequencyRadioRepository;

        public TowerFrequencyRadioService(ITowerFrequencyRadioRepository towerFrequencyRadioRepo) : base((mc) => CreateMapping(mc)) =>
            _towerFrequencyRadioRepository = towerFrequencyRadioRepo;

        public static void CreateMapping(IMapperConfigurationExpression config) =>
            config.CreateMap<TowerFrequencyRadio, TowerFrequencyRadioViewModel>()
                .ForMember(dest => dest.VoiceCount, opt => opt.MapFrom(src => src.VoiceGrantCount))
                .ForMember(dest => dest.EncryptedCount, opt => opt.MapFrom(src => src.EncryptedVoiceGrantCount))
                .ForMember(dest => dest.EmergencyCount, opt => opt.MapFrom(src => src.EmergencyVoiceGrantCount))
                .ForMember(dest => dest.DataCount, opt => opt.MapFrom(src => src.DataCount + src.PrivateDataCount));

        public async Task<IEnumerable<TowerFrequencyRadio>> GetForTowerAsync(int systemID, int towerNumber, DateTime date) =>
            await _towerFrequencyRadioRepository.GetForTowerAsync(systemID, towerNumber, date);

        public async Task<(IEnumerable<TowerFrequencyRadioViewModel> towerFrequencyRadios, int recordCount)> GetRadiosForTowerFrequencyAsync(string systemID,
            int towerNumber, string frequency, FilterDataModel filterData)
        {
            CheckFilterRanges(filterData, Talkgroup.MIN_TALKGROUP_ID, Talkgroup.MAX_TALKGROUP_ID);

            var (towerFrequencyRadios, recordCount) = await _towerFrequencyRadioRepository.GetRadiosForTowerFrequencyAsync(systemID, towerNumber, frequency,
                _mapper.Map<FilterData>(filterData));

            return (towerFrequencyRadios.Select(tfr => _mapper.Map<TowerFrequencyRadioViewModel>(tfr)), recordCount);

        }

        public void ProcessRecord(int systemID, int towerID, string frequency, int radioID, ICollection<Radio> radios,
            ICollection<TowerFrequencyRadio> towerFrequencyRadios, DateTime timeStamp, string action, Action<ICounterRecord, ActionTypes> updateCounters)
        {
            if (string.IsNullOrWhiteSpace(frequency) || (radioID == 0))
            {
                return;
            }

            frequency = FixFrequency(frequency);

            var towerFrequencyRadio = towerFrequencyRadios.SingleOrDefault(tfr => tfr.Frequency.Equals(frequency, StringComparison.OrdinalIgnoreCase) &&
                tfr.RadioID == radioID && tfr.Date == timeStamp.Date);

            if (towerFrequencyRadio == null)
            {
                towerFrequencyRadio = CreateTowerFrequencyRadio(systemID, towerID, frequency, radioID, timeStamp);
                towerFrequencyRadios.Add(towerFrequencyRadio);
            }

            if (towerFrequencyRadio != null)
            {
                updateCounters(towerFrequencyRadio, action.GetEnumFromDescription<ActionTypes>());
                CheckDates(towerFrequencyRadio, timeStamp);
            }

            CheckRadioPhaseII(radioID, frequency, radios);
        }

        public void CheckRadioPhaseII(int radioID, string frequency, ICollection<Radio> radios) 
        {
            if (IsPhaseIIFrequency(frequency))
            {
                var radio = radios.Where(r => r.RadioID == radioID).SingleOrDefault();

                if (radio != null)
                {
                    radio.PhaseIISeen = true;
                }
            }
        }

        public TowerFrequencyRadio CreateTowerFrequencyRadio(int systemID, int towerNumber, string frequency, int radioID, DateTime timeStamp)
        {
            var towerFrequencyRadio = Create<TowerFrequencyRadio>();

            towerFrequencyRadio.SystemID = systemID;
            towerFrequencyRadio.TowerID = towerNumber;
            towerFrequencyRadio.Frequency = frequency;
            towerFrequencyRadio.RadioID = radioID;
            towerFrequencyRadio.Date = timeStamp.Date;

            return towerFrequencyRadio;
        }
    }
}
