using AutoMapper;
using DataLibrary;
using DataLibrary.Interfaces;
using ObjectLibrary;
using ServiceCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TowerService;
using static ServiceCommon.Factory;

namespace TowerFrequencyService
{
    public sealed class TowerFrequencyService : ServiceBase, ITowerFrequencyService
    {
        private const string EMPTY_FREQUENCY = "0.00000";
        private const string FREQUENCY_TYPE_CURRENT = "Current";
        private const string FREQUENCY_TYPE_ALL = "All";
        private const string FREQUENCY_TYPE_NOT_CURRENT = "Not Current";

        private readonly ITowerFrequencyRepository _towerFrequencyRepository;

        public TowerFrequencyService(ITowerFrequencyRepository towerFrequencyRepository) : base((mc) => CreateMapping(mc)) =>
            _towerFrequencyRepository = towerFrequencyRepository;

        public static void CreateMapping(IMapperConfigurationExpression config) =>
            config.CreateMap<TowerFrequency, TowerFrequencyViewModel>()
                .ForMember(dest => dest.VoiceCount, opt => opt.MapFrom(src => src.VoiceGrantCount))
                .ForMember(dest => dest.EncryptedCount, opt => opt.MapFrom(src => src.EncryptedVoiceGrantCount))
                .ForMember(dest => dest.EmergencyCount, opt => opt.MapFrom(src => src.EmergencyVoiceGrantCount))
                .ForMember(dest => dest.PhaseIISeen, opt => opt.Ignore());

        public async Task<TowerFrequency> GetForFrequencyAsync(int systemID, int towerNumber, string frequency) =>
            await _towerFrequencyRepository.GetForFrequencyAsync(systemID, towerNumber, frequency);

        public async Task<TowerFrequencyViewModel> GetSummaryAsync(string systemID, int towerNumber, string frequency) =>
            _mapper.Map<TowerFrequencyViewModel>(await _towerFrequencyRepository.GetSummaryAsync(systemID, towerNumber, frequency));

        public async Task<TowerFrequencyDataViewModel> GetSummaryAsync(SystemInfo systemInfo, TowerViewModel tower, string frequency,
            SearchDataViewModel searchData) =>
            
            Factory.GetTowerFrequencyDataModel(systemInfo, tower, frequency, await GetSummaryAsync(systemInfo.SystemID, tower.TowerNumber,
                frequency), searchData);

        public async Task<IEnumerable<TowerFrequency>> GetForTowerAsync(int systemID, int towerNumber) =>
            await _towerFrequencyRepository.GetForTowerAsync(systemID, towerNumber);

        public async Task<IEnumerable<TowerFrequency>> GetFrequenciesForTowerAsync(int systemID, int towerNumber) =>
            await _towerFrequencyRepository.GetFrequenciesForTowerAsync(systemID, towerNumber);

        public async Task<int> GetFrequenciesForTowerCountAsync(int systemID, int towerNumber) =>
            await _towerFrequencyRepository.GetFrequenciesForTowerCountAsync(systemID, towerNumber);

        public async Task<(IEnumerable<TowerFrequencyViewModel> towerFrequencies, int recordCount)> GetFrequenciesForTowerAsync(string systemID,
            int towerNumber, string frequencyType, FilterDataModel filterData)
        {
            var towerFrequencies = (default(IEnumerable<TowerFrequency>), default(int));
            var resultsC = (default(IEnumerable<TowerFrequency>), default(int));
            var resultsA = (default(IEnumerable<TowerFrequency>), default(int));
            var resultsNC = (default(IEnumerable<TowerFrequency>), default(int));

            CheckFilterRanges(filterData, Tower.MIN_TOWER_NUMBER, Tower.MAX_TOWER_NUMBER);

            switch (frequencyType)
            {
                case FREQUENCY_TYPE_CURRENT:
                    resultsC = await _towerFrequencyRepository.GetFrequenciesForTowerAsync(systemID, towerNumber, _mapper.Map<FilterData>(filterData));
                    break;

                case FREQUENCY_TYPE_ALL:
                    resultsA = await _towerFrequencyRepository.GetFrequenciesForTowerAllAsync(systemID, towerNumber, _mapper.Map<FilterData>(filterData));
                    break;

                case FREQUENCY_TYPE_NOT_CURRENT:
                    resultsNC = await _towerFrequencyRepository.GetFrequenciesForTowerNotCurrentAsync(systemID, towerNumber,
                        _mapper.Map<FilterData>(filterData));
                    break;
    
                default:
                    throw new ArgumentOutOfRangeException(nameof(frequencyType));
            }

            if (resultsC.Item1.ToList().Count > 0)
            {
                towerFrequencies = resultsC;
            }
            else if (resultsA.Item1.ToList().Count > 0)
            {
                towerFrequencies = resultsA;
            }
            else if (resultsNC.Item1.ToList().Count > 0)
            {
                towerFrequencies = resultsNC;
            }
            
            return (towerFrequencies.Item1.Select(tf => _mapper.Map<TowerFrequencyViewModel>(tf)), towerFrequencies.Item2);
        }

        public async Task<IEnumerable<TowerFrequency>> GetFrequenciesForTowerAllAsync(int systemID, int towerNumber) =>
            await _towerFrequencyRepository.GetFrequenciesForTowerAllAsync(systemID, towerNumber);

        public async Task<int> GetFrequenciesForTowerAllCountAsync(int systemID, int towerNumber) =>
            await _towerFrequencyRepository.GetFrequenciesForTowerAllCountAsync(systemID, towerNumber);

        public async Task<IEnumerable<TowerFrequency>> GetFrequenciesForTowerNotCurrentAsync(int systemID, int towerNumber) =>
            await _towerFrequencyRepository.GetFrequenciesForTowerNotCurrentAsync(systemID, towerNumber);

        public async Task<int> GetFrequenciesForTowerNotCurrentCountAsync(int systemID, int towerNumber) =>
            await _towerFrequencyRepository.GetFrequenciesForTowerNotCurrentCountAsync(systemID, towerNumber);

        public async Task WriteAsync(TowerFrequency towerFrequency) => await _towerFrequencyRepository.WriteAsync(towerFrequency);

        public async Task DeleteAsync(int id) => await _towerFrequencyRepository.DeleteAsync(id);

        public TowerFrequency CreateTowerFrequency(int systemID, int towerID, string usage, string frequency, DateTime timeStamp)
        {
            if (string.IsNullOrWhiteSpace(frequency) || frequency.Equals(EMPTY_FREQUENCY, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            var towerFrequency = Create<TowerFrequency>();

            towerFrequency.SystemID = systemID;
            towerFrequency.TowerID = towerID;
            towerFrequency.Channel = string.Empty;
            towerFrequency.Usage = usage;
            towerFrequency.Frequency = frequency;
            towerFrequency.Date = timeStamp.Date;
            towerFrequency.InputChannel = string.Empty;
            towerFrequency.InputFrequency = string.Empty;
            towerFrequency.InputExplicit = 0;
            towerFrequency.HitCount = 0;
            towerFrequency.FirstSeen = timeStamp;
            towerFrequency.LastSeen = timeStamp;

            return towerFrequency;
        }

        public TowerFrequency CreateTowerFrequency(int systemID, int towerNumber, string frequency, string channel, DateTime timeStamp,
            int talkgroupID = -1, int radioID = -1)
        {
            if (string.IsNullOrWhiteSpace(frequency) || frequency.Equals(EMPTY_FREQUENCY, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            var towerFrequency = Create<TowerFrequency>();

            towerFrequency.SystemID = systemID;
            towerFrequency.TowerID = towerNumber;
            towerFrequency.Channel = channel;
            towerFrequency.Usage = string.Empty;
            towerFrequency.Frequency = frequency;
            towerFrequency.Date = timeStamp.Date;
            towerFrequency.InputChannel = string.Empty;
            towerFrequency.InputFrequency = string.Empty;
            towerFrequency.InputExplicit = 0;
            towerFrequency.HitCount = 0;
            towerFrequency.FirstSeen = timeStamp;
            towerFrequency.LastSeen = timeStamp;

            return towerFrequency;
        }
    }
}
