using ObjectLibrary;
using ServiceLibrary.Interfaces.Parsers;
using System.Collections.Generic;
using static ServiceLibrary.Factory;

namespace ServiceLibrary.Parsers
{
    public class ParsePatchLogV1 : IPatchLogParser
    {
        private const int COL_TIMESTAMP = 0;
        private const int COL_DESCRIPTION = 1;

        private readonly int _systemID;
        private readonly int _towerNumber;
        private readonly int _year;

        public ParsePatchLogV1(int systemID, int towerNumber, int year)
        {
            _systemID = systemID;
            _towerNumber = towerNumber;
            _year = year;
        }

        public IEnumerable<PatchLog> ParsePatchLog(string[] row)
        {
            var patch = Create<PatchLog>();

            patch.SystemID = _systemID;
            patch.TowerNumber = _towerNumber;
            patch.TimeStamp = Common.FixDate(row[COL_TIMESTAMP], _year);
            patch.Description = row[COL_DESCRIPTION];

            yield return patch;
        }
    }
}
