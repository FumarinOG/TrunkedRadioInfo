using AutoMapper;
using DataLibrary;
using DataLibrary.Interfaces;
using ObjectLibrary;
using ServiceCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ServiceCommon.Common;
using static ServiceCommon.Factory;

namespace TalkgroupHistoryService
{
    public sealed class TalkgroupHistoryService : ServiceBase, ITalkgroupHistoryService
    {
        private readonly ITalkgroupHistoryRepository _talkgroupHistoryRepo;

        public TalkgroupHistoryService(ITalkgroupHistoryRepository talkgroupHistoryRepository) : base((mc) => CreateMapping(mc)) =>
            _talkgroupHistoryRepo = talkgroupHistoryRepository;

        public static void CreateMapping(IMapperConfigurationExpression config) =>
            config.CreateMap<TalkgroupHistory, TalkgroupHistoryViewModel>()
                .ForMember(dest => dest.TalkgroupName, opt => opt.MapFrom(src => src.Description));

        public async Task<IEnumerable<TalkgroupHistoryViewModel>> GetForTalkgroupAsync(int systemID, int talkgroupID) =>
            (await _talkgroupHistoryRepo.GetForTalkgroupAsync(systemID, talkgroupID)).Select(tgh => _mapper.Map<TalkgroupHistoryViewModel>(tgh));

        public async Task<int> GetForTalkgroupCountAsync(int systemID, int talkgroupID) =>
            await _talkgroupHistoryRepo.GetForTalkgroupCountAsync(systemID, talkgroupID);

        public async Task<(IEnumerable<TalkgroupHistoryViewModel> talkgroupHistory, int recordCount)> GetForTalkgroupAsync(string systemID,
            int talkgroupID, FilterDataModel filterData)
        {
            CheckFilterRanges(filterData, Talkgroup.MIN_TALKGROUP_ID, Talkgroup.MAX_TALKGROUP_ID);

            var (talkgroupHistory, recordCount) = await _talkgroupHistoryRepo.GetForTalkgroupAsync(systemID, talkgroupID, _mapper.Map<FilterData>(filterData));

            return (talkgroupHistory.Select(tgh => _mapper.Map<TalkgroupHistoryViewModel>(tgh)), recordCount);
        }

        public async Task<int> GetForTalkgroupCountAsync(int systemID, int talkgroupID, string searchText) =>
            await _talkgroupHistoryRepo.GetForTalkgroupCountAsync(systemID, talkgroupID);

        public async Task<IEnumerable<TalkgroupHistory>> GetForSystemAsync(int systemID) => await _talkgroupHistoryRepo.GetForSystemAsync(systemID);

        public void ProcessRecord(int systemID, int talkgroupID, string description, DateTime timeStamp, ICollection<Talkgroup> talkgroups,
            ICollection<TalkgroupHistory> history)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                return;
            }

            var differentTalkgroupDescription = talkgroups.SingleOrDefault(tg => tg.TalkgroupID == talkgroupID
                && tg.Description.Equals(description, StringComparison.OrdinalIgnoreCase));

            if (differentTalkgroupDescription == null)
            {
                var newTalkgroupHistory = CreateTalkgroupHistory(systemID, talkgroupID, description, timeStamp);

                var talkgroupHistory = history.SingleOrDefault(tgh => tgh.TalkgroupID == talkgroupID
                    && tgh.Description.Equals(description, StringComparison.OrdinalIgnoreCase));

                if (talkgroupHistory == null)
                {
                    history.Add(newTalkgroupHistory);
                }
                else
                {
                    CheckDates(talkgroupHistory, timeStamp);
                }
            }
            else
            {
                var talkgroupHistory = history.SingleOrDefault(tgh => tgh.TalkgroupID == talkgroupID
                    && tgh.Description.Equals(description, StringComparison.OrdinalIgnoreCase));

                if (talkgroupHistory != null)
                {
                    CheckDates(talkgroupHistory, timeStamp);
                }
                else
                {
                    history.Add(CreateTalkgroupHistory(systemID, talkgroupID, description, timeStamp));
                }
            }
        }

        public TalkgroupHistory CreateTalkgroupHistory(int systemID, int talkgroupID, string description, DateTime timeStamp)
        {
            var talkgroupHistory = Create<TalkgroupHistory>();

            talkgroupHistory.SystemID = systemID;
            talkgroupHistory.TalkgroupID = talkgroupID;

            if (string.IsNullOrWhiteSpace(description))
            {
                talkgroupHistory.Description = $"<Unknown> ({talkgroupID:0})";
            }
            else
            {
                talkgroupHistory.Description = description;
            }

            talkgroupHistory.FirstSeen = timeStamp;
            talkgroupHistory.LastSeen = timeStamp;

            return talkgroupHistory;
        }
    }
}
