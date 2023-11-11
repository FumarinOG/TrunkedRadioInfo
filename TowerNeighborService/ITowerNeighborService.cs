using ObjectLibrary;
using ServiceCommon;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TowerNeighborService
{
    public interface ITowerNeighborService
    {
        Task<TowerNeighbor> GetAsync(int systemID, int towerNumber, int neighborSystemID, int neighborTowerNumber);
        Task<TowerNeighbor> GetForSystemTowerNumberAsync(int systemID, int towerNumber, int neighborSystemID, int neighborTowerNumber);
        Task<IEnumerable<TowerNeighbor>> GetForTowerAsync(int systemID, int towerNumber);
        Task<(IEnumerable<TowerNeighborViewModel> towerNeighbors, int recordCount)> GetForTowerAsync(string systemID, int towerNumber,
            FilterDataModel filterData);
        Task WriteAsync(TowerNeighbor towerNeighbor);
        Task DeleteForTowerAsync(int systemID, int towerNumber);
        TowerNeighbor Create(int systemID, int towerNumber, int neighborSystemID, int neighborTowerNumber, string neighborTowerNumberHex, string neighborTowerName, string neighborFrequency, string neighborChannel);
        void Fill(TowerNeighbor neighbor, TowerNeighbor currentNeighbor);
    }
}
