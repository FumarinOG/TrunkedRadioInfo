using ObjectLibrary;
using ServiceCommon;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TowerService
{
    public interface ITowerService
    {
        Task<Tower> GetForTowerAsync(int systemID, int towerNumber);

        Task<TowerViewModel> GetDetailAsync(int systemID, int towerNumber);
        Task<TowerDataViewModel> GetDetailAsync(SystemInfo systemInfo, int towerNumber, SearchDataViewModel searchData);
        Task<IEnumerable<Tower>> GetForSystemAsync(int systemID);
        Task<IEnumerable<TowerViewModel>> GetViewForSystemAsync(int systemID);
        Task<(IEnumerable<TowerViewModel> towers, int recordCount)> GetViewForSystemAsync(string systemID, bool activeOnly, FilterDataModel filterData);
        Task<int> GetCountForSystemAsync(int systemID);
        void ProcessRecord(Tower tower, DateTime timeStamp);
    }
}
