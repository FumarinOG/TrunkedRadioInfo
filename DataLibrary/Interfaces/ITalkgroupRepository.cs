using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLibrary.Interfaces
{
    public interface ITalkgroupRepository
    {
        Task<IEnumerable<Talkgroup>> GetForSystemAsync(int systemID);
        Task<Talkgroup> GetDetailAsync(int systemID, int talkgroupID);
        Task<Talkgroup> GetDetailAsync(int systemID, int talkgroupID, DateTime? dateFrom, DateTime? dateTo);
        Task<IEnumerable<Talkgroup>> GetDetailForSystemAsync(int systemID);
        Task<(IEnumerable<Talkgroup> talkgroups, int recordCount)> GetDetailForSystemAsync(string systemID, bool activeOnly, FilterData filterData);
        Task<(IEnumerable<Talkgroup> talkgroups, int recordCount)> GetDetailForSystemUnknownAsync(string systemID, FilterData filterData);
        Task<int> GetCountForSystemAsync(int systemID);
    }
}
