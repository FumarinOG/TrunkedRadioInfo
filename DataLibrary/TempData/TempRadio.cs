using DataLibrary.Interfaces;
using ObjectLibrary;
using System;

namespace DataLibrary.TempData
{
    public sealed class TempRadio : ITempRecord<Radio>
    {
        public Guid SessionID { get; private set; }
        public int SystemID { get; private set; }
        public int RadioID { get; private set; }
        public string Description { get; private set; }
        public DateTime LastSeen { get; private set; }
        public DateTime? LastSeenProgram { get; private set; }
        public long? LastSeenProgramUnix { get; private set; }
        public DateTime FirstSeen { get; private set; }
        public string FGColor { get; private set; }
        public string BGColor { get; private set; }
        public int HitCount { get; private set; }
        public bool? PhaseIISeen { get; private set; }

        [DataTableSkip]
        public string TableName => "TempRadios";

        public void CopyFrom(Guid sessionID, Radio radio)
        {
            SessionID = sessionID;
            SystemID = radio.SystemID;
            RadioID = radio.RadioID;
            Description = radio.Description;
            LastSeen = radio.LastSeen;
            LastSeenProgram = radio.LastSeenProgram;
            LastSeenProgramUnix = radio.LastSeenProgramUnix;
            FirstSeen = radio.FirstSeen;
            FGColor = radio.FGColor;
            BGColor = radio.BGColor;
            HitCount = radio.HitCount;
            PhaseIISeen = radio.PhaseIISeen;
        }

        public override string ToString() => $"Temp - System ID {SystemID}, Radio ID {RadioID} ({Description})";
    }
}
