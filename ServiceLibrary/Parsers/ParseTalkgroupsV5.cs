using ObjectLibrary;
using ServiceLibrary.Interfaces.Parsers;
using System;
using static ServiceLibrary.Factory;

namespace ServiceLibrary.Parsers
{
    public class ParseTalkgroupsV5 : ITalkgroupParser
    {
        private const int COL_TALKGROUP_ID = 0;
        private const int COL_PRIORITY = 1;
        private const int COL_DESCRIPTION = 2;
        private const int COL_LAST_SEEN_UNIX = 3;
        private const int COL_LAST_SEEN = 4;
        private const int COL_FIRST_SEEN_UNIX = 5;
        private const int COL_FIRST_SEEN = 6;
        private const int COL_FG_COLOR = 7;
        private const int COL_BG_COLOR = 8;

        private readonly int _systemID;

        public ParseTalkgroupsV5(int systemID)
        {
            _systemID = systemID;
        }

        public Talkgroup ParseTalkgroup(string[] row)
        {
            var talkgroup = Create<Talkgroup>();

            talkgroup.SystemID = _systemID;
            talkgroup.TalkgroupID = int.Parse(row[COL_TALKGROUP_ID]);
            talkgroup.Priority = int.Parse(row[COL_PRIORITY]);
            talkgroup.Description = row[COL_DESCRIPTION];
            talkgroup.LastSeen = DateTime.Parse(row[COL_LAST_SEEN].Replace("@", string.Empty));
            talkgroup.LastSeenProgramUnix = long.Parse(row[COL_LAST_SEEN_UNIX]);
            talkgroup.LastSeenProgram = DateTime.Parse(row[COL_LAST_SEEN].Replace("@", string.Empty));
            talkgroup.FirstSeen = DateTime.Parse(row[COL_FIRST_SEEN].Replace("@", string.Empty));
            talkgroup.FirstSeenProgramUnix = long.Parse(row[COL_FIRST_SEEN_UNIX]);
            talkgroup.FirstSeenProgram = DateTime.Parse(row[COL_FIRST_SEEN].Replace("@", string.Empty));
            talkgroup.FGColor = row[COL_FG_COLOR];
            talkgroup.BGColor = row[COL_BG_COLOR];

            return talkgroup;
        }
    }
}
