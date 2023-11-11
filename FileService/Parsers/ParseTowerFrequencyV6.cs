using FileService.Interfaces;
using ObjectLibrary;
using System;
using static FileService.Factory;

namespace FileService.Parsers
{
    public class ParseTowerFrequencyV6 : ITowerFrequencyParser
    {
        private const int COL_CHANNEL = 0;
        private const int COL_USAGE = 1;
        private const int COL_FREQUENCY = 2;
        private const int COL_INPUT_FREQUENCY = 3;
        private const int COL_HIT_COUNT = 4;

        private readonly int _systemID;

        public ParseTowerFrequencyV6(int systemID)
        {
            _systemID = systemID;
        }

        public TowerFrequency ParseTowerFrequency(string[] row, Tower tower)
        {
            var newFrequency = Create<TowerFrequency>();

            newFrequency.SystemID = _systemID;
            newFrequency.TowerID = tower.ID;
            newFrequency.Channel = row[COL_CHANNEL];
            newFrequency.Usage = row[COL_USAGE];
            newFrequency.Frequency = row[COL_FREQUENCY];
            newFrequency.InputFrequency = row[COL_INPUT_FREQUENCY];
            newFrequency.HitCount = int.Parse(row[COL_HIT_COUNT]);
            newFrequency.FirstSeen = tower.TimeStamp ?? DateTime.Now;
            newFrequency.LastSeen = tower.TimeStamp ?? DateTime.Now;

            return newFrequency;
        }
    }
}
