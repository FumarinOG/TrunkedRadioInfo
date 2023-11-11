using ObjectLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLibrary.Interfaces
{
    public interface ITalkgroupHistoryRepository
    {
        Task<IEnumerable<TalkgroupHistory>> GetForTalkgroupAsync(int systemID, int talkgroupID);
        Task<(IEnumerable<TalkgroupHistory> talkgroupHistory, int recordCount)> GetForTalkgroupAsync(string systemID, int talkgroupID, FilterData filterData);
        Task<int> GetForTalkgroupCountAsync(int systemID, int talkgroupID);
        Task<IEnumerable<TalkgroupHistory>> GetForSystemAsync(int systemID);
    }
}
