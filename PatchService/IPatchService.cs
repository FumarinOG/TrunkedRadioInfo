using ObjectLibrary;
using ServiceCommon;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PatchService
{
    public interface IPatchService
    {
        Task<IEnumerable<Patch>> GetSummaryForSystemAsync(int systemID);
        Task<PatchViewModel> GetSummaryAsync(SystemInfo systemInfo, int fromTalkgroupID, int toTalkgroupID);
        Task<(IEnumerable<PatchViewModel> patches, int recordCount)> GetSummaryAsync(string systemID, int fromTalkgroupID, int toTalkgroupID, FilterDataModel filterData);
        Task<(IEnumerable<PatchViewModel> patches, int recordCount)> GetSummaryForSystemAsync(string systemID, FilterDataModel filterData);
        Task<IEnumerable<Patch>> GetForPatchByDateAsync(int systemID, int fromTalkgroupID, int toTalkgroupID);
        Task<IEnumerable<PatchDatesViewModel>> GetForPatchByDateAsync(int systemID, int fromTalkgroupID, int toTalkgroupID, int pageNumber, int pageSize);
        Task<int> GetForPatchByDateCountAsync(int systemID, int fromTalkgroupID, int toTalkgropupID);
        Task<IEnumerable<Patch>> GetForSystemTalkgroupAsync(int systemID, int talkgroupID);
        Task<int> GetForSystemTalkgroupCountAsync(int systemID, int talkgroupID);
        Task<(IEnumerable<Patch> patches, int recordCount)> GetForSystemTalkgroupAsync(string systemID, int talkgroupID, FilterDataModel filterData);
        Task<int> GetCountForSystemAsync(int systemID, string searchText);
        Task<(IEnumerable<Patch> patches, int recordCount)> GetForSystemTowerAsync(string systemID, int talkgroupID, FilterDataModel filterData);
    }
}
