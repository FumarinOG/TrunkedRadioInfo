using ObjectLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLibrary.Interfaces
{
    public interface ITowerTableRepository
    {
        Task<TowerTable> GetAsync(int systemID, int towerNumber, int tableID);
        Task<IEnumerable<TowerTable>> GetListForTowerAsync(int systemID, int towerNumber);
        Task WriteAsync(TowerTable towerTable);
        Task DeleteForTowerAsync(int systemID, int towerNumber);
    }
}
