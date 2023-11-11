using FileService.Interfaces;
using ObjectLibrary;
using System.Collections.Generic;
using static FileService.Factory;

namespace FileService.Parsers
{
    public class ParseAffiliationsV2 : IAffiliationParser
    {
        private const int COL_TIMESTAMP = 0;
        private const int COL_FUNCTION = 1;
        private const int COL_TALKGROUP_ID = 2;
        private const int COL_TALKGROUP_DESCRIPTION = 3;
        private const int COL_RADIO_ID = 4;
        private const int COL_RADIO_DESCRIPTION = 5;

        private readonly int _systemID;
        private readonly int _towerNumber;
        private readonly int _year;

        public ParseAffiliationsV2(int systemID, int towerNumber, int year)
        {
            _systemID = systemID;
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
            affiliation.TalkgroupDescription = row[COL_TALKGROUP_DESCRIPTION];
            affiliation.RadioID = Common.ParseID(row[COL_RADIO_ID]);
            affiliation.RadioDescription = row[COL_RADIO_DESCRIPTION];

            yield return affiliation;
        }
    }
}
