using FileService.Interfaces;
using ObjectLibrary;
using System;
using static FileService.Factory;

namespace FileService.Parsers
{
    public class ParseRadiosV4 : IRadioParser
    {
        private const int COL_RADIO_ID = 0;
        private const int COL_DESCRIPTION = 1;
        private const int COL_LAST_SEEN_UNIX = 2;
        private const int COL_LAST_SEEN = 3;
        private const int COL_TEXT_COLOR = 4;
        private const int COL_BG_COLOR = 5;

        private readonly int _systemID;

        public ParseRadiosV4(int systemID)
        {
            _systemID = systemID;
        }

        public Radio ParseRadio(string[] row)
        {
            var radio = Create<Radio>();

            radio.SystemID = _systemID;
            radio.RadioID = int.Parse(row[COL_RADIO_ID]);
            radio.Description = row[COL_DESCRIPTION];
            radio.LastSeenProgramUnix = long.Parse(row[COL_LAST_SEEN_UNIX]);
            radio.LastSeenProgram = DateTime.Parse(row[COL_LAST_SEEN].Replace("@", string.Empty));
            radio.FirstSeen = DateTime.Parse(row[COL_LAST_SEEN].Replace("@", string.Empty));
            radio.LastSeen = DateTime.Parse(row[COL_LAST_SEEN].Replace("@", string.Empty));
            radio.FGColor = row[COL_TEXT_COLOR];
            radio.BGColor = row[COL_BG_COLOR];

            return radio;
        }
    }
}
