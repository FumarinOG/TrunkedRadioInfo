using DataLibrary.Interfaces;
using ObjectLibrary;
using ServiceCommon;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ServiceCommon.Factory;

namespace TowerTableService
{
    public sealed class TowerTableService : ServiceBase, ITowerTableService
    {
        private readonly ITowerTableRepository _towerTableRepository;

        public TowerTableService(ITowerTableRepository towerTableRepository) : base(null) => _towerTableRepository = towerTableRepository;

        public async Task<TowerTable> GetAsync(int systemID, int towerNumber, int tableID) => await _towerTableRepository.GetAsync(systemID, towerNumber, tableID);

        public async Task<IEnumerable<TowerTable>> GetListForTowerAsync(int systemID, int towerNumber) =>
            await _towerTableRepository.GetListForTowerAsync(systemID, towerNumber);

        public async Task WriteAsync(TowerTable towerTable) => await _towerTableRepository.WriteAsync(towerTable);

        public async Task DeleteForTowerAsync(int systemID, int towerNumber) => await _towerTableRepository.DeleteForTowerAsync(systemID, towerNumber);

        public TowerTable Create(int systemID, int towerNumber, int tableID)
        {
            var towerTable = Create<TowerTable>();

            towerTable.SystemID = systemID;
            towerTable.TowerID = towerNumber;
            towerTable.TableID = tableID;

            return towerTable;
        }

        public void Fill(TowerTable table, TowerTable currentTable)
        {
            currentTable.BaseFrequency = table.BaseFrequency;
            currentTable.Spacing = table.Spacing;
            currentTable.InputOffset = table.InputOffset;
            currentTable.AssumedConfirmed = table.AssumedConfirmed;
            currentTable.Bandwidth = table.Bandwidth;
            currentTable.Slots = table.Slots;
        }
    }
}
