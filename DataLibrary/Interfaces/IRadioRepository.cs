using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLibrary.Interfaces
{
    public interface IRadioRepository
    {
        Task<Radio> GetDetailAsync(int systemID, int radioID);
        Task<Radio> GetDetailAsync(int systemID, int radioID, DateTime? dateFrom, DateTime? dateTo);
        Task<IEnumerable<Radio>> GetListForSystemAsync(int systemID);
        Task<IEnumerable<Radio>> GetDetailForSystemAsync(int systemID);
        Task<(IEnumerable<Radio> radios, int recordCount)> GetDetailForSystemAsync(string systemID, bool activeOnly, string sortField, string sortDirection, int pageNumber, int pageSize);
        Task<(IEnumerable<Radio> radios, int recordCount)> GetDetailForSystemAsync(string systemID, bool activeOnly, FilterData filterData);
        Task<int> GetCountForSystemAsync(int systemID);
        Task<(IEnumerable<(int radioID, string name)> names, int recordCount)> GetNamesAsync(string systemID, FilterData filterData);
    }
}
