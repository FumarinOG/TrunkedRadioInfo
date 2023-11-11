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
using static ObjectLibrary.Factory;
using static ServiceCommon.Common;

namespace TalkgroupRadioService
{
    public sealed class TalkgroupRadioService : ServiceBase, ITalkgroupRadioService
    {
        private readonly ITalkgroupRadioRepository _talkgroupRadioRepo;

        public TalkgroupRadioService(ITalkgroupRadioRepository talkgroupRadioRepository) : base((mc) => CreateMapping(mc)) =>
            _talkgroupRadioRepo = talkgroupRadioRepository;

        public static void CreateMapping(IMapperConfigurationExpression config)
        {
            config.CreateMap<TalkgroupRadio, RadioTalkgroupViewModel>()
               .ForMember(dest => dest.VoiceCount, opt => opt.MapFrom(src => src.VoiceGrantCount))
               .ForMember(dest => dest.EncryptedCount, opt => opt.MapFrom(src => src.EncryptedVoiceGrantCount));
            config.CreateMap<TalkgroupRadio, TalkgroupRadioViewModel>()
                .ForMember(dest => dest.VoiceCount, opt => opt.MapFrom(src => src.VoiceGrantCount))
                .ForMember(dest => dest.EncryptedCount, opt => opt.MapFrom(src => src.EncryptedVoiceGrantCount));
        }

        public async Task<IEnumerable<TalkgroupRadio>> GetForSystemAsync(int systemID) => await _talkgroupRadioRepo.GetForSystemAsync(systemID);

        public async Task<IEnumerable<TalkgroupRadio>> GetForSystemAsync(int systemID, DateTime date) => await _talkgroupRadioRepo.GetForSystemAsync(systemID, date);

        public async Task<IEnumerable<TalkgroupRadio>> GetRadiosForTalkgroupAsync(int systemID, int talkgroupID) =>
            await _talkgroupRadioRepo.GetRadiosForTalkgroupAsync(systemID, talkgroupID);

        public async Task<int> GetRadiosForTalkgroupCountAsync(int systemID, int talkgroupID) =>
            await _talkgroupRadioRepo.GetRadiosForTalkgroupCountAsync(systemID, talkgroupID);

        public async Task<(IEnumerable<TalkgroupRadioViewModel> talkgroupRadios, int recordCount)> GetRadiosForTalkgroupAsync(string systemID,
            int talkgroupID, FilterDataModel filterData)
        {
            CheckFilterRanges(filterData, Radio.MIN_RADIO_ID, Radio.MAX_RADIO_ID);

            var (talkgroupRadios, recordCount) = await _talkgroupRadioRepo.GetRadiosForTalkgroupAsync(systemID, talkgroupID,
                _mapper.Map<FilterData>(filterData));

            return (talkgroupRadios.Select(tgr => _mapper.Map<TalkgroupRadioViewModel>(tgr)), recordCount);
        }

        public async Task<int> GetRadiosForTalkgroupCountAsync(int systemID, int talkgroupID, string searchText, string searchData) =>
            await _talkgroupRadioRepo.GetRadiosForTalkgroupCountAsync(systemID, talkgroupID);

        public async Task<IEnumerable<TalkgroupRadio>> GetRadiosForTalkgroupWithDatesAsync(int systemID, int talkgroupID) =>
            await _talkgroupRadioRepo.GetRadiosForTalkgroupWithDatesAsync(systemID, talkgroupID);

        public async Task<IEnumerable<TalkgroupRadio>> GetTalkgroupsForRadioAsync(int systemID, int radioID) =>
            await _talkgroupRadioRepo.GetTalkgroupsForRadioAsync(systemID, radioID);

        public async Task<(IEnumerable<RadioTalkgroupViewModel> radioTalkgroups, int recordCount)> GetTalkgroupsForRadioAsync(string systemID,
            int radioID, FilterDataModel filterData)
        {
            CheckFilterRanges(filterData, Talkgroup.MIN_TALKGROUP_ID, Talkgroup.MAX_TALKGROUP_ID);

            var (talkgroupRadios, recordCount) = await _talkgroupRadioRepo.GetTalkgroupsForRadioAsync(systemID, radioID, _mapper.Map<FilterData>(filterData));

            return (talkgroupRadios.Select(tgr => _mapper.Map<RadioTalkgroupViewModel>(tgr)), recordCount);
        }

        public async Task<int> GetTalkgroupsForRadioCountAsync(int systemID, int radioID) => await _talkgroupRadioRepo.GetTalkgroupsForRadioCountAsync(systemID, radioID);

        public void ProcessRecord(int systemID, int talkgroupID, int radioID, ICollection<TalkgroupRadio> talkgroupRadios, DateTime timeStamp, string action,
            Action<ICounterRecord, ActionTypes> updateCounters)
        {
            if ((talkgroupID == 0) || (talkgroupID == -1))    // Talkgroup ID -1 is a Group Data action
            {
                return;
            }

            var talkgroupRadio = talkgroupRadios.SingleOrDefault(tgr => tgr.TalkgroupID == talkgroupID && tgr.RadioID == radioID && tgr.Date == timeStamp.Date);

            if (talkgroupRadio == null)
            {
                talkgroupRadio = CreateTalkgroupRadio(systemID, talkgroupID, radioID, timeStamp);
                talkgroupRadios.Add(talkgroupRadio);
            }

            updateCounters(talkgroupRadio, action.GetEnumFromDescription<ActionTypes>());
            CheckDates(talkgroupRadio, timeStamp);
        }

        public TalkgroupRadio CreateTalkgroupRadio(int systemID, int talkgroupID, int radioID, DateTime timeStamp)
        {
            var talkgroupRadio = Create<TalkgroupRadio>();

            talkgroupRadio.SystemID = systemID;
            talkgroupRadio.TalkgroupID = talkgroupID;
            talkgroupRadio.RadioID = radioID;
            talkgroupRadio.Date = timeStamp.Date;
            talkgroupRadio.AffiliationCount = 0;
            talkgroupRadio.DeniedCount = 0;
            talkgroupRadio.VoiceGrantCount = 0;
            talkgroupRadio.FirstSeen = timeStamp;
            talkgroupRadio.LastSeen = timeStamp;

            return talkgroupRadio;
        }
    }
}
