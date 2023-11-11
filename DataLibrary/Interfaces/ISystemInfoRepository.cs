using DataAccessLibrary;
using ObjectLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLibrary.Interfaces
{
    public interface ISystemInfoRepository
    {
        Task<SystemInfo> GetAsync(int id);
        Task<SystemInfo> GetAsync(string systemID);
        Task<int> GetSystemIDAsync(string systemID);
        Task<IEnumerable<SystemInfo>> GetListAsync();
        Task<IEnumerable<SystemInfo>> GetListAsync(FilterData filterData);
        Task<int> GetCountAsync();
        Task WriteAsync(SystemInfo systemInfo);
        void EditRecord(Systems databaseRecord, SystemInfo systemInfo);
    }
}
