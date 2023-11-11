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
using static TalkgroupService.Factory;

namespace TalkgroupService
{
    public sealed class TalkgroupService : ServiceBase, ITalkgroupService
    {
        private readonly ITalkgroupRepository _talkgroupRepo;

        public TalkgroupService(ITalkgroupRepository talkgroupRepo) : base((mc) => CreateMapping(mc)) => _talkgroupRepo = talkgroupRepo;

        public static void CreateMapping(IMapperConfigurationExpression config) => 
            config.CreateMap<Talkgroup, TalkgroupViewModel>()
               .ForMember(dest => dest.TalkgroupName, opt => opt.MapFrom(src => src.Description))
               .ForMember(dest => dest.VoiceCount, opt => opt.MapFrom(src => src.VoiceGrantCount))
               .ForMember(dest => dest.EmergencyCount, opt => opt.MapFrom(src => src.EmergencyVoiceGrantCount))
               .ForMember(dest => dest.EncryptedCount, opt => opt.MapFrom(src => src.EncryptedVoiceGrantCount));

        public async Task<TalkgroupViewModel> GetDetailAsync(int systemID, int talkgroupID) => await GetDetailAsync(systemID, talkgroupID, null);

        public async Task<TalkgroupViewModel> GetDetailAsync(int systemID, int talkgroupID, FilterDataModel filterData)
        {
            if ((filterData != null) && ((filterData.DateFrom != null) || (filterData.DateTo != null)))
            {
                return _mapper.Map<TalkgroupViewModel>(await _talkgroupRepo.GetDetailAsync(systemID, talkgroupID, filterData.DateFrom, filterData.DateTo));
            }

            return _mapper.Map<TalkgroupViewModel>(await _talkgroupRepo.GetDetailAsync(systemID, talkgroupID));
        }

        public async Task<TalkgroupDataViewModel> GetDetailAsync(SystemInfo systemInfo, int talkgroupID, SearchDataViewModel searchData) =>
            GetTalkgroupDataModel(systemInfo, talkgroupID, await GetDetailAsync(systemInfo.ID, talkgroupID, searchData.SearchData), searchData);

        public async Task<IEnumerable<Talkgroup>> GetDetailForSystemAsync(int systemID) => await _talkgroupRepo.GetDetailForSystemAsync(systemID);

        public async Task<IEnumerable<Talkgroup>> GetDetailForSystemAsync(int systemID, string sortField, string sortDirection, int pageNumber, int pageSize) => null;

        public async Task<IEnumerable<TalkgroupViewModel>> GetViewForSystemAsync(int systemID) =>
            (await _talkgroupRepo.GetDetailForSystemAsync(systemID)).Select(tg => _mapper.Map<TalkgroupViewModel>(tg));

        public async Task<(IEnumerable<TalkgroupViewModel> talkgroups, int recordCount)> GetViewForSystemAsync(string systemID, bool activeOnly,
            FilterDataModel filterData)
        {
            CheckFilterRanges(filterData, Talkgroup.MIN_TALKGROUP_ID, Talkgroup.MAX_TALKGROUP_ID);

            var (talkgroups, recordCount) = await _talkgroupRepo.GetDetailForSystemAsync(systemID, activeOnly, _mapper.Map<FilterData>(filterData));

            return (talkgroups.Select(tg => _mapper.Map<TalkgroupViewModel>(tg)), recordCount);
        }

        public async Task<(IEnumerable<TalkgroupViewModel> talkgroups, int recordCount)> GetViewForSystemUnknownAsync(string systemID, FilterDataModel filterData)
        {
            CheckFilterRanges(filterData, Talkgroup.MIN_TALKGROUP_ID, Talkgroup.MAX_TALKGROUP_ID);

            var (talkgroups, recordCount) = await _talkgroupRepo.GetDetailForSystemUnknownAsync(systemID, _mapper.Map<FilterData>(filterData));

            return (talkgroups.Select(tg => _mapper.Map<TalkgroupViewModel>(tg)), recordCount);
        }

        public async Task<IEnumerable<Talkgroup>> GetForSystemAsync(int systemID) => await _talkgroupRepo.GetForSystemAsync(systemID);

        public async Task<int> GetCountForSystemAsync(int systemID) => await _talkgroupRepo.GetCountForSystemAsync(systemID);

        public string ProcessRecord(int systemID, int talkgroupID, string description, ICollection<Talkgroup> talkgroups, DateTime timeStamp, string action,
            Action<IRecord, ActionTypes> updateHitCounts)
        {
            if ((talkgroupID == 0) || (talkgroupID == -1) || (talkgroupID >= Talkgroup.MAX_TALKGROUP_ID))
            // Talkgroup ID -1 is a Group Data event. Talkgroups greater than the max talkgroup ID appear due to
            // a bug in Pro96Com involving queued voice grants when the next successful grant is granted
            {
                return null;
            }

            var talkgroup = talkgroups.SingleOrDefault(tg => tg.TalkgroupID == talkgroupID);

            if (talkgroup == null)
            {
                talkgroup = CreateTalkgroup(systemID, talkgroupID, description, timeStamp);
                talkgroups.Add(talkgroup);

                // If the talkgroup description is empty, a name is created so store the newly created name
                description = talkgroup.Description;
            }

            updateHitCounts(talkgroup, action.GetEnumFromDescription<ActionTypes>());
            CheckDates(talkgroup, timeStamp);
            return description;
        }

        public Talkgroup CreateTalkgroup(int systemID, int talkgroupID, string description, DateTime timeStamp)
        {
            var talkgroup = Create<Talkgroup>();

            talkgroup.SystemID = systemID;
            talkgroup.TalkgroupID = talkgroupID;
            talkgroup.Description = string.IsNullOrWhiteSpace(description) ? $"<Unknown> ({talkgroupID:0})" : description;
            talkgroup.FirstSeen = timeStamp;
            talkgroup.LastSeen = timeStamp;

            return talkgroup;

        }
    }
}
