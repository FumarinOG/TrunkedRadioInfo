using DataLibrary.Interfaces;
using ObjectLibrary;
using System;

namespace DataLibrary.TempData
{
    public class TempTalkgroup : ITempRecord<Talkgroup>
    {
        public Guid SessionID { get; private set; }
        public int SystemID { get; private set; }
        public int TalkgroupID { get; private set; }
        public int? Priority { get; private set; }
        public string Description { get; private set; }
        public DateTime LastSeen { get; private set; }
        public DateTime? LastSeenProgram { get; private set; }
        public long? LastSeenProgramUnix { get; private set; }
        public DateTime FirstSeen { get; private set; }
        public DateTime? FirstSeenProgram { get; private set; }
        public long? FirstSeenProgramUnix { get; private set; }
        public string FGColor { get; private set; }
        public string BGColor { get; private set; }
        public bool? EncryptionSeen { get; private set; }
        public bool? IgnoreEmergencySignal { get; private set; }
        public int HitCount { get; private set; }
        public int HitCountProgram { get; private set; }
        public bool? PhaseIISeen { get; private set; }

        [DataTableSkip]
        public string TableName => "TempTalkgroups";

        public void CopyFrom(Guid sessionID, Talkgroup talkgroup)
        {
            SessionID = sessionID;
            SystemID = talkgroup.SystemID;
            TalkgroupID = talkgroup.TalkgroupID;
            Priority = talkgroup.Priority;
            Description = talkgroup.Description;
            LastSeen = talkgroup.LastSeen;
            LastSeenProgram = talkgroup.LastSeenProgram;
            LastSeenProgramUnix = talkgroup.LastSeenProgramUnix;
            FirstSeen = talkgroup.FirstSeen;
            FirstSeenProgram = talkgroup.FirstSeenProgram;
            FirstSeenProgramUnix = talkgroup.FirstSeenProgramUnix;
            FGColor = talkgroup.FGColor;
            BGColor = talkgroup.BGColor;
            EncryptionSeen = talkgroup.EncryptionSeen;
            IgnoreEmergencySignal = talkgroup.IgnoreEmergencySignal;
            HitCount = talkgroup.HitCount;
            HitCountProgram = talkgroup.HitCountProgram;
            PhaseIISeen = talkgroup.PhaseIISeen;
        }

        public override string ToString() => $"Temp - System ID {SystemID}, Talkgroup ID {TalkgroupID} ({Description})";
    }
}
