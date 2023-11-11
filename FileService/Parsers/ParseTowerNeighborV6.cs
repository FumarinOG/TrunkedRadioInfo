using FileService.Interfaces;
using ObjectLibrary;
using System.Collections.Generic;
using static FileService.Factory;

namespace FileService.Parsers
{
    public class ParseTowerNeighborV6 : ITowerNeighborParser
    {
        private const int COL_TOWER_ID = 0;
        private const int COL_TOWER_ID_HEX = 1;
        private const int COL_SYSEM_ID = 2;
        private const int COL_NEIGHBOR_CHANNEL = 3;
        private const int COL_NEIGHBOR_FREQUENCY = 4;
        private const int COL_TOWER_NAME = 5;

        private readonly int _systemID;

        public ParseTowerNeighborV6(int systemID)
        {
            _systemID = systemID;
        }

        public TowerNeighbor ParseTowerNeighbor(string[] row, Tower tower, Dictionary<string, int> systems)
        {
            var towerNeighbor = Create<TowerNeighbor>();

            towerNeighbor.SystemID = _systemID;
            towerNeighbor.TowerNumber = tower.TowerNumber;
            towerNeighbor.NeighborSystemID = systems[row[COL_SYSEM_ID].ToUpperInvariant()];
            towerNeighbor.NeighborTowerID = Common.FixTowerNumber(row[COL_TOWER_ID]);
            towerNeighbor.NeighborTowerNumberHex = row[COL_TOWER_ID_HEX];
            towerNeighbor.NeighborChannel = row[COL_NEIGHBOR_CHANNEL];
            towerNeighbor.NeighborFrequency = row[COL_NEIGHBOR_FREQUENCY];
            towerNeighbor.NeighborTowerName = row[COL_TOWER_NAME];

            return towerNeighbor;
        }
    }
}
