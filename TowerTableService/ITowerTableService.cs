using ObjectLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TowerTableService
{
    public interface ITowerTableService
    {
        Task<TowerTable> GetAsync(int systemID, int towerID, int tableID);
        Task<IEnumerable<TowerTable>> GetListForTowerAsync(int systemID, int towerNumber);
        Task WriteAsync(TowerTable towerTable);
        Task DeleteForTowerAsync(int systemID, int towerNumber);
        TowerTable Create(int systemID, int towerNumber, int tableID);
        void Fill(TowerTable table, TowerTable currentTable);
    }
}
