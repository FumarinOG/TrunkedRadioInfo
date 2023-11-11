using ObjectLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLibrary.Interfaces
{
    public interface IPatchRepository
    {
        Task<IEnumerable<Patch>> GetForSystemAsync(int systemID);
        Task<Patch> GetSummaryAsync(int systemID, int fromTalkgroupID, int toTalkgroupID);
        Task<(IEnumerable<Patch> patches, int recordCount)> GetSummaryAsync(string systemID, int fromTalkgroupID, int toTalkgroupID, FilterData filterData);
        Task<IEnumerable<Patch>> GetSummaryForSystemAsync(int systemID);
        Task<(IEnumerable<Patch> patches, int recordCount)> GetSummaryForSystemAsync(string systemID, FilterData filterData);
        Task<IEnumerable<Patch>> GetForPatchByDateAsync(int systemID, int fromTalkgroupID, int toTalkgroupID);
        Task<IEnumerable<Patch>> GetForPatchByDateAsync(int systemID, int fromTalkgroupID, int toTalkgroupID, int pageNumber, int pageSize);
        Task<int> GetForPatchByDateCountAsync(int systemID, int fromTalkgroupID, int toTalkgropupID);
        Task<IEnumerable<Patch>> GetForSystemFromTalkgroupAsync(int systemID, int fromTalkgroupID);
        Task<IEnumerable<Patch>> GetForSystemToTalkgroupAsync(int systemID, int toTalkgroupID);
        Task<IEnumerable<Patch>> GetForSystemTalkgroupAsync(int systemID, int talkgroupID);
        Task<int> GetForSystemTalkgroupCountAsync(int systemID, int talkgroupID);
        Task<(IEnumerable<Patch> patches, int recordCount)> GetForSystemTalkgroupAsync(string systemID, int talkgroupID, FilterData filterData);
        Task<IEnumerable<Patch>> GetForSystemTowerAsync(int systemID, int towerNumber);
        Task<(IEnumerable<Patch> patches, int recordCount)> GetForSystemTowerAsync(string systemID, int towerNumber, FilterData filterData);
        Task<int> GetCountForSystemAsync(int systemID);
        Task<int> GetCountForSystemAsync(int systemID, string searchText);
    }
}
