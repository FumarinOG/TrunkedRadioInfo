using ObjectLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLibrary.Interfaces
{
    public interface ITowerFrequencyRepository
    {
        Task<TowerFrequency> GetForFrequencyAsync(int systemID, int towerNumber, string frequency);
        Task<TowerFrequency> GetSummaryAsync(string systemID, int towerNumber, string frequency);
        Task<IEnumerable<TowerFrequency>> GetForTowerAsync(int systemID, int towerNumber);
        Task<int> GetFrequenciesForTowerCountAsync(int systemID, int towerNumber);
        Task<IEnumerable<TowerFrequency>> GetFrequenciesForTowerAsync(int systemID, int towerNumber);
        Task<(IEnumerable<TowerFrequency> towerFrequencies, int recordCount)> GetFrequenciesForTowerAsync(string systemID, int towerNumber, FilterData filterData);
        Task<int> GetFrequenciesForTowerAllCountAsync(int systemID, int towerNumber);
        Task<IEnumerable<TowerFrequency>> GetFrequenciesForTowerAllAsync(int systemID, int towerNumber);
        Task<(IEnumerable<TowerFrequency> towerFrequencies, int recordCount)> GetFrequenciesForTowerAllAsync(string systemID, int towerNumber,
            FilterData filterData);
        Task<int> GetFrequenciesForTowerNotCurrentCountAsync(int systemID, int towerNumber);
        Task<IEnumerable<TowerFrequency>> GetFrequenciesForTowerNotCurrentAsync(int systemID, int towerNumber);
        Task<(IEnumerable<TowerFrequency> towerFrequencies, int recordCount)> GetFrequenciesForTowerNotCurrentAsync(string systemID, int towerNumber,
            FilterData filterData);
        Task WriteAsync(TowerFrequency towerFrequency);
        Task DeleteAsync(int id);
    }
}
