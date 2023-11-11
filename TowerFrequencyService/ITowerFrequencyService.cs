using ObjectLibrary;
using ServiceCommon;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TowerService;

namespace TowerFrequencyService
{
    public interface ITowerFrequencyService
    {
        Task<TowerFrequency> GetForFrequencyAsync(int systemID, int towerNumber, string frequency);
        Task<TowerFrequencyViewModel> GetSummaryAsync(string systemID, int towerNumber, string frequency);
        Task<TowerFrequencyDataViewModel> GetSummaryAsync(SystemInfo systemInfo, TowerViewModel tower, string frequency, SearchDataViewModel searchData);
        Task<IEnumerable<TowerFrequency>> GetForTowerAsync(int systemID, int towerNumber);
        Task<IEnumerable<TowerFrequency>> GetFrequenciesForTowerAsync(int systemID, int towerNumber);
        Task<int> GetFrequenciesForTowerCountAsync(int systemID, int towerNumber);
        Task<(IEnumerable<TowerFrequencyViewModel> towerFrequencies, int recordCount)> GetFrequenciesForTowerAsync(string systemID, int towerNumber,
            string frequencyType, FilterDataModel filterData);
        Task<IEnumerable<TowerFrequency>> GetFrequenciesForTowerAllAsync(int systemID, int towerNumber);
        Task<int> GetFrequenciesForTowerAllCountAsync(int systemID, int towerNumber);
        Task<IEnumerable<TowerFrequency>> GetFrequenciesForTowerNotCurrentAsync(int systemID, int towerNumber);
        Task<int> GetFrequenciesForTowerNotCurrentCountAsync(int systemID, int towerNumber);
        Task WriteAsync(TowerFrequency towerFrequencyUsage);
        Task DeleteAsync(int id);
        TowerFrequency CreateTowerFrequency(int systemID, int towerID, string usage, string frequency, DateTime timeStamp);
    }
}
