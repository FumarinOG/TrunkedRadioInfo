using DataAccessLibrary;
using ObjectLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLibrary.Interfaces
{
    public interface ITowerNeighborRepository
    {
        Task<TowerNeighbor> GetAsync(int systemID, int towerID, int neighborSystemID, int neighborTowerNumber);
        Task<TowerNeighbor> GetForSystemTowerNumberAsync(int systemID, int towerNumber, int neighborSystemID, int neighborTowerNumber);
        Task<(IEnumerable<TowerNeighbor> towerNeighbors, int recordCount)> GetNeighborsForTowerAsync(string systemID, int towerNumber, FilterData filterData);
        Task<IEnumerable<TowerNeighbor>> GetNeighborsForTowerAsync(int systemID, int towerNumber);
        Task WriteAsync(TowerNeighbor towerNeighbor);
        void EditRecord(TowerNeighbors databaseRecord, TowerNeighbor towerNeighbor);
        Task DeleteForTowerAsync(int systemID, int towerNumber);
    }
}
