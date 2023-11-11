using ObjectLibrary;
using ServiceLibrary.Interfaces.Parsers;
using System;
using static ServiceLibrary.Factory;

namespace ServiceLibrary.Parsers
{
    public class ParseTowerFrequencyV8 : ITowerFrequencyParser
    {
        private const int COL_CHANNEL = 0;
        private const int COL_USAGE = 1;
        private const int COL_FREQUENCY = 2;
        private const int COL_INPUT_CHANNEL = 3;
        private const int COL_INPUT_FREQUENCY = 4;
        private const int COL_INPUT_EXPLICIT = 5;
        private const int COL_HIT_COUNT = 6;

        private readonly int _systemID;

        public ParseTowerFrequencyV8(int systemID)
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
            newFrequency.InputChannel = row[COL_INPUT_CHANNEL];
            newFrequency.InputFrequency = row[COL_INPUT_FREQUENCY];
            newFrequency.InputExplicit = int.Parse(row[COL_INPUT_EXPLICIT]);
            newFrequency.HitCount = int.Parse(row[COL_HIT_COUNT]);
            newFrequency.FirstSeen = tower.TimeStamp ?? DateTime.Now;
            newFrequency.LastSeen = tower.TimeStamp ?? DateTime.Now;

            return newFrequency;
        }
    }
}
