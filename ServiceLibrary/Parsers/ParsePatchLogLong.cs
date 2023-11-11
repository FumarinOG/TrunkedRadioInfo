using ObjectLibrary;
using ServiceLibrary.Interfaces.Parsers;
using System.Collections.Generic;
using static ServiceLibrary.Factory;

namespace ServiceLibrary.Parsers
{
    public class ParsePatchLogLong : IPatchLogParser
    {
        private const int COL_TIMESTAMP = 0;
        private const int COL_DESCRIPTION = 1;

        private const int ROW_LENGTH = 2;

        private readonly int _systemID;
        private readonly int _towerNumber;
        private readonly int _year;

        public ParsePatchLogLong(int systemID, int towerNumber, int year)
        {
            _systemID = systemID;
            _towerNumber = towerNumber;
            _year = year;
        }

        public IEnumerable<PatchLog> ParsePatchLog(string[] row)
        {
            var columnOffset = 0;
            var canContinue = true;

            while (canContinue)
            {
                var patch = Create<PatchLog>();

                patch.SystemID = _systemID;
                patch.TowerNumber = _towerNumber;
                patch.TimeStamp = Common.FixDate(row[COL_TIMESTAMP + columnOffset], _year);
                patch.Description = row[COL_DESCRIPTION + columnOffset];

                columnOffset += ROW_LENGTH;
                canContinue = !(columnOffset >= row.Length);

                yield return patch;
            }
        }
    }
}
