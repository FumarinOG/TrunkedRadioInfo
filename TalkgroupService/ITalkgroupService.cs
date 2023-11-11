using ObjectLibrary;
using ObjectLibrary.Interfaces;
using ServiceCommon;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TalkgroupService
{
    public interface ITalkgroupService
    {
        Task<TalkgroupViewModel> GetDetailAsync(int systemID, int talkgroupID);
        Task<TalkgroupViewModel> GetDetailAsync(int systemID, int talkgroupID, FilterDataModel filters);
        Task<TalkgroupDataViewModel> GetDetailAsync(SystemInfo systemInfo, int talkgroupID, SearchDataViewModel searchData);
        Task<IEnumerable<Talkgroup>> GetDetailForSystemAsync(int systemID);
        Task<IEnumerable<Talkgroup>> GetDetailForSystemAsync(int systemID, string sortField, string sortDirection, int pageNumber, int pageSize);
        Task<IEnumerable<TalkgroupViewModel>> GetViewForSystemAsync(int systemID);
        Task<(IEnumerable<TalkgroupViewModel> talkgroups, int recordCount)> GetViewForSystemAsync(string systemID, bool activeOnly, FilterDataModel filterData);
        Task<(IEnumerable<TalkgroupViewModel> talkgroups, int recordCount)> GetViewForSystemUnknownAsync(string systemID, FilterDataModel filterData);
        Task<IEnumerable<Talkgroup>> GetForSystemAsync(int systemID);
        Task<int> GetCountForSystemAsync(int systemID);
        string ProcessRecord(int systemID, int talkgroupID, string description, ICollection<Talkgroup> talkgroups, DateTime timeStamp, string action,
            Action<IRecord, ActionTypes> updateHitCounts);
        Talkgroup CreateTalkgroup(int systemID, int talkgroupID, string description, DateTime timeStamp);
    }
}
