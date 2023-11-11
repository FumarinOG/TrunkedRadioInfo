using DataLibrary.Interfaces;
using ObjectLibrary;
using System;

namespace DataLibrary.TempData
{
    public class TempTalkgroupHistory : ITempRecord<TalkgroupHistory>
    {
        public Guid SessionID { get; private set; }
        public int SystemID { get; private set; }
        public int TalkgroupID { get; private set; }
        public string Description { get; private set; }
        public DateTime FirstSeen { get; private set; }
        public DateTime LastSeen { get; private set; }

        [DataTableSkip]
        public string TableName => "TempTalkgroupHistory";

        public void CopyFrom(Guid sessionID, TalkgroupHistory talkgroupHistory)
        {
            SessionID = sessionID;
            SystemID = talkgroupHistory.SystemID;
            TalkgroupID = talkgroupHistory.TalkgroupID;
            Description = talkgroupHistory.Description;
            FirstSeen = talkgroupHistory.FirstSeen;
            LastSeen = talkgroupHistory.LastSeen;
        }

        public override string ToString() => $"Temp - System ID {SystemID}, Talkgroup ID {TalkgroupID} ({Description})";
    }
}
