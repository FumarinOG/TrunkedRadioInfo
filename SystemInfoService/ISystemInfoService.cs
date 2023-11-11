using ObjectLibrary;
using ObjectLibrary.Interfaces;
using ServiceCommon;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SystemInfoService
{
    public interface ISystemInfoService
    {
        Task<SystemInfo> GetAsync(int id);
        Task<SystemInfo> GetAsync(string systemID);
        Task<SystemViewModel> GetSystemAsync(string systemID);
        Task<int> GetSystemIDAsync(string systemID);
        Task<string> GetNameAsync(string systemID);
        Task<IEnumerable<SystemInfoViewModel>> GetListAsync();
        Task<IEnumerable<SystemInfoViewModel>> GetListAsync(FilterDataModel filterData);
        Task WriteAsync(SystemInfo systemInfo);
        Task WriteAsync(IEnumerable<IRecord> systemInfo);
        void ProcessRecord(SystemInfo systemInfo, DateTime timeStamp);
    }
}
