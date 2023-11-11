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

namespace TowerFrequencyTalkgroupService
{
    public sealed class TowerFrequencyTalkgroupService : ServiceBase, ITowerFrequencyTalkgroupService
    {
        private readonly ITowerFrequencyTalkgroupRepository _towerFrequencyTalkgroupRepository;

        public TowerFrequencyTalkgroupService(ITowerFrequencyTalkgroupRepository towerFrequencyTalkgroupRepo) : base((mc) => CreateMapping(mc)) =>
            _towerFrequencyTalkgroupRepository = towerFrequencyTalkgroupRepo;

        public static void CreateMapping(IMapperConfigurationExpression config) =>
            config.CreateMap<TowerFrequencyTalkgroup, TowerFrequencyTalkgroupViewModel>()
                .ForMember(dest => dest.VoiceCount, opt => opt.MapFrom(src => src.VoiceGrantCount))
                .ForMember(dest => dest.EncryptedCount, opt => opt.MapFrom(src => src.EncryptedVoiceGrantCount))
                .ForMember(dest => dest.EmergencyCount, opt => opt.MapFrom(src => src.EmergencyVoiceGrantCount));

        public async Task<IEnumerable<TowerFrequencyTalkgroup>> GetForTowerAsync(int systemID, int towerNumber, DateTime date) =>
            await _towerFrequencyTalkgroupRepository.GetForTowerAsync(systemID, towerNumber, date);

        public async Task<(IEnumerable<TowerFrequencyTalkgroupViewModel> towerFrequencyTalkgroups, int recordCount)> GetTalkgroupsForTowerFrequencyAsync(
            string systemID, int towerNumber, string frequency, FilterDataModel filterData)
        {
            CheckFilterRanges(filterData, Talkgroup.MIN_TALKGROUP_ID, Talkgroup.MAX_TALKGROUP_ID);

            var (towerFrequencyTalkgroups, recordCount) = await _towerFrequencyTalkgroupRepository.GetTalkgroupsForTowerFrequencyAsync(systemID, towerNumber,
                frequency, _mapper.Map<FilterData>(filterData));

            return (towerFrequencyTalkgroups.Select(tftg => _mapper.Map<TowerFrequencyTalkgroupViewModel>(tftg)), recordCount);

        }

        public void ProcessRecord(int systemID, int towerID, string frequency, int talkgroupID, ICollection<Talkgroup> talkgroups,
            ICollection<TowerFrequencyTalkgroup> towerFrequencyTalkgroups, DateTime timeStamp, string action, Action<ICounterRecord, ActionTypes> updateCounters)
        {
            if (string.IsNullOrWhiteSpace(frequency) || (talkgroupID == 0) || (talkgroupID == -1))
            {
                return;
            }

            frequency = FixFrequency(frequency);

            var towerFrequencyTalkgroup = towerFrequencyTalkgroups.SingleOrDefault(tftg => tftg.Frequency.Equals(frequency, StringComparison.OrdinalIgnoreCase) &&
                tftg.TalkgroupID == talkgroupID && tftg.Date == timeStamp.Date);

            if (towerFrequencyTalkgroup == null)
            {
                towerFrequencyTalkgroup = CreateTowerFrequencyTalkgroup(systemID, towerID, frequency, talkgroupID, timeStamp);
                towerFrequencyTalkgroups.Add(towerFrequencyTalkgroup);
            }

            if (towerFrequencyTalkgroup != null)
            {
                updateCounters(towerFrequencyTalkgroup, action.GetEnumFromDescription<ActionTypes>());
                CheckDates(towerFrequencyTalkgroup, timeStamp);
            }

            CheckTalkgroupPhaseII(talkgroupID, frequency, talkgroups);
        }

        private void CheckTalkgroupPhaseII(int talkgroupID, string frequency, ICollection<Talkgroup> talkgroups)
        {
            if (IsPhaseIIFrequency(frequency))
            {
                var talkgroup = talkgroups.Where(tg => tg.TalkgroupID == talkgroupID).SingleOrDefault();

                if (talkgroup != null)
                {
                    talkgroup.PhaseIISeen = true;
                }
            }
        }

        public TowerFrequencyTalkgroup CreateTowerFrequencyTalkgroup(int systemID, int towerNumber, string frequency, int talkgroupID, DateTime timeStamp)
        {
            var towerFrequencyTalkgroup = Create<TowerFrequencyTalkgroup>();

            towerFrequencyTalkgroup.SystemID = systemID;
            towerFrequencyTalkgroup.TowerID = towerNumber;
            towerFrequencyTalkgroup.Frequency = frequency;
            towerFrequencyTalkgroup.TalkgroupID = talkgroupID;
            towerFrequencyTalkgroup.Date = timeStamp.Date;

            return towerFrequencyTalkgroup;
        }
    }
}
