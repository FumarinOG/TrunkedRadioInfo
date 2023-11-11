using FileService.Interfaces;
using ObjectLibrary;
using static FileService.Factory;

namespace FileService.Parsers
{
    public class ParseTowerTableV7 : ITowerTableParser
    {
        private const int COL_TABLE_ID = 0;
        private const int COL_BASE_FREQUENCY = 1;
        private const int COL_SPACING = 2;
        private const int COL_INPUT_OFFSET = 3;
        private const int COL_ASSUMED_CONFIRMED = 4;
        private const int COL_BANDWIDTH = 5;
        private const int COL_SLOTS = 6;

        private readonly int _systemID;

        public ParseTowerTableV7(int systemID)
        {
            _systemID = systemID;
        }

        public TowerTable ParseTowerTable(string[] row, Tower tower)
        {
            var towerTable = Create<TowerTable>();

            towerTable.SystemID = _systemID;
            towerTable.TowerID = tower.TowerNumber;
            towerTable.TableID = int.Parse(row[COL_TABLE_ID]);
            towerTable.BaseFrequency = row[COL_BASE_FREQUENCY];
            towerTable.Spacing = row[COL_SPACING];
            towerTable.InputOffset = row[COL_INPUT_OFFSET];
            towerTable.AssumedConfirmed = row[COL_ASSUMED_CONFIRMED];
            towerTable.Bandwidth = row[COL_BANDWIDTH];
            towerTable.Slots = int.Parse(row[COL_SLOTS]);

            return towerTable;
        }
    }
}
