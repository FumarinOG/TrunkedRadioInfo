using ObjectLibrary;
using ServiceLibrary.Interfaces.Parsers;
using System.Collections.Generic;
using static ServiceLibrary.Factory;

namespace ServiceLibrary.Parsers
{
    public class ParseAffiliationsLong : IAffiliationParser
    {
        private const int COL_TIMESTAMP = 0;
        private const int COL_FUNCTION = 1;
        private const int COL_TALKGROUP_ID = 2;
        private const int COL_TALKGROUP_DESCRIPTION = 3;
        private const int COL_RADIO_ID = 4;
        private const int COL_RADIO_DESCRIPTION = 5;

        private const int ROW_LENGTH = 6;

        private readonly int _systemID;
        private readonly int _towerNumber;
        private readonly int _year;

        public ParseAffiliationsLong(int sysemID, int towerNumber, int year)
        {
            _systemID = sysemID;
            _towerNumber = towerNumber;
            _year = year;
        }

        public IEnumerable<Affiliation> ParseAffiliation(string[] row)
        {
            var columnOffset = 0;
            var canContinue = true;

            while (canContinue)
            {
                var affiliation = Create<Affiliation>();

                affiliation.SystemID = _systemID;
                affiliation.TowerNumber = _towerNumber;
                affiliation.TimeStamp = Common.FixDate(row[COL_TIMESTAMP + columnOffset], _year);
                affiliation.Function = row[COL_FUNCTION + columnOffset];
                affiliation.TalkgroupID = Common.ParseID(row[COL_TALKGROUP_ID + columnOffset]);
                affiliation.TalkgroupDescription = row[COL_TALKGROUP_DESCRIPTION + columnOffset];
                affiliation.RadioID = Common.ParseID(row[COL_RADIO_ID + columnOffset]);
                affiliation.RadioDescription = row[COL_RADIO_DESCRIPTION + columnOffset];

                columnOffset += ROW_LENGTH;
                canContinue = !(columnOffset >= row.Length);

                yield return affiliation;
            }
        }
    }
}
