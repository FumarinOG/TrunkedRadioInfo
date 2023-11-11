using ObjectLibrary;
using ServiceLibrary.Interfaces.Parsers;
using System.Collections.Generic;
using static ServiceLibrary.Factory;

namespace ServiceLibrary.Parsers
{
    public class ParseAffiliationsV1 : IAffiliationParser
    {
        private const int COL_TIMESTAMP = 0;
        private const int COL_FUNCTION = 1;
        private const int COL_TALKGROUP_ID = 2;
        private const int COL_RADIO_ID = 3;
        private const int COL_RADIO_DESCRIPTION = 4;

        private readonly int _systemID;
        private readonly int _towerNumber;
        private readonly int _year;

        public ParseAffiliationsV1(int sysemID, int towerNumber, int year)
        {
            _systemID = sysemID;
            _towerNumber = towerNumber;
            _year = year;
        }

        public IEnumerable<Affiliation> ParseAffiliation(string[] row)
        {
            var affiliation = Create<Affiliation>();

            affiliation.SystemID = _systemID;
            affiliation.TowerNumber = _towerNumber;
            affiliation.TimeStamp = Common.FixDate(row[COL_TIMESTAMP], _year);
            affiliation.Function = row[COL_FUNCTION];
            affiliation.TalkgroupID = Common.ParseID(row[COL_TALKGROUP_ID]);
            affiliation.RadioID = Common.ParseID(row[COL_RADIO_ID]);
            affiliation.RadioDescription = row[COL_RADIO_DESCRIPTION];

            yield return affiliation;
        }
    }
}
