using ObjectLibrary;
using ServiceCommon;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TalkgroupHistoryService
{
    public interface ITalkgroupHistoryService
    {
        Task<IEnumerable<TalkgroupHistoryViewModel>> GetForTalkgroupAsync(int systemID, int talkgroupID);
        Task<int> GetForTalkgroupCountAsync(int systemID, int talkgroupID);
        Task<(IEnumerable<TalkgroupHistoryViewModel> talkgroupHistory, int recordCount)> GetForTalkgroupAsync(string systemID, int talkgroupID,
            FilterDataModel filterData);
        Task<int> GetForTalkgroupCountAsync(int systemID, int talkgroupID, string searchText);
        Task<IEnumerable<TalkgroupHistory>> GetForSystemAsync(int systemID);
        void ProcessRecord(int systemID, int talkgroupID, string description, DateTime timeStamp, ICollection<Talkgroup> talkgroups,
            ICollection<TalkgroupHistory> history);
        TalkgroupHistory CreateTalkgroupHistory(int systemID, int talkgroupID, string description, DateTime timeStamp);
    }
}
