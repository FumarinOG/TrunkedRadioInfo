using DataLibrary.Interfaces;
using ObjectLibrary;
using System;

namespace DataLibrary.TempData
{
    public class TempTalkgroupRadio : ITempRecord<TalkgroupRadio>
    {
        public Guid SessionID { get; private set; }
        public int SystemID { get; private set; }
        public int TalkgroupID { get; private set; }
        public int RadioID { get; private set; }
        public DateTime Date { get; private set; }
        public int AffiliationCount { get; private set; }
        public int DeniedCount { get; private set; }
        public int VoiceGrantCount { get; private set; }
        public int EmergencyVoiceGrantCount { get; private set; }
        public int EncryptedVoiceGrantCount { get; private set; }
        public int DataCount { get; private set; }
        public int PrivateDataCount { get; private set; }
        public int AlertCount { get; private set; }
        public DateTime FirstSeen { get; private set; }
        public DateTime LastSeen { get; private set; }

        [DataTableSkip]
        public string TableName => "TempTalkgroupRadios";

        public void CopyFrom(Guid sessionID, TalkgroupRadio talkgroupRadio)
        {
            SessionID = sessionID;
            SystemID = talkgroupRadio.SystemID;
            TalkgroupID = talkgroupRadio.TalkgroupID;
            RadioID = talkgroupRadio.RadioID;
            Date = talkgroupRadio.Date;
            AffiliationCount = talkgroupRadio.AffiliationCount;
            DeniedCount = talkgroupRadio.DeniedCount;
            VoiceGrantCount = talkgroupRadio.VoiceGrantCount;
            EmergencyVoiceGrantCount = talkgroupRadio.EmergencyVoiceGrantCount;
            EncryptedVoiceGrantCount = talkgroupRadio.EncryptedVoiceGrantCount;
            DataCount = talkgroupRadio.DataCount;
            PrivateDataCount = talkgroupRadio.PrivateDataCount;
            AlertCount = talkgroupRadio.AlertCount;
            FirstSeen = talkgroupRadio.FirstSeen;
            LastSeen = talkgroupRadio.LastSeen;
        }

        public override string ToString() => $"Temp - System ID {SystemID}, Talkgroup ID {TalkgroupID}, Radio ID {RadioID}";
    }
}
