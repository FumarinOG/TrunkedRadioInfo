using ObjectLibrary;
using ObjectLibrary.Interfaces;
using ServiceCommon;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RadioService
{
    public interface IRadioService
    {
        Task<RadioViewModel> GetDetailAsync(int systemID, int radioID);
        Task<RadioViewModel> GetDetailAsync(int systemID, int radioID, FilterDataModel filters);
        Task<RadioDataViewModel> GetDetailAsync(SystemInfo systemInfo, int radioID, SearchDataViewModel searchData);
        Task<IEnumerable<Radio>> GetDetailForSystemAsync(int systemID);
        Task<IEnumerable<Radio>> GetDetailForSystemAsync(int systemID, string sortField, string sortDirection, int pageNumber, int pageSize);
        Task<IEnumerable<RadioViewModel>> GetViewForSystemAsync(int systemID);
        Task<(IEnumerable<RadioViewModel> radios, int recordCount)> GetViewForSystemAsync(string systemID, bool activeOnly, FilterDataModel filterData);
        Task<int> GetCountForSystemAsync(int systemID);
        Task<IEnumerable<Radio>> GetForSystemAsync(int systemID);
        Task<(IEnumerable<RadioNameViewModel> names, int recordCount)> GetNamesAsync(string systemID, FilterDataModel filterData);
        string ProcessRecord(int systemID, int radioID, string description, DateTime timeStamp, string action, ICollection<Radio> radios,
            Action<IRecord, ActionTypes> updateHitCounts);
        Radio CreateRadio(int systemID, int radioID, string description, DateTime timeStamp);
    }
}
