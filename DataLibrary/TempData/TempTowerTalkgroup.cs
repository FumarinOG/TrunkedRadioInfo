using DataLibrary.Interfaces;
using ObjectLibrary;
using System;

namespace DataLibrary.TempData
{
    public class TempTowerTalkgroup : ITempRecord<TowerTalkgroup>
    {
        public Guid SessionID { get; private set; }
        public int SystemID { get; private set; }
        public int TowerID { get; private set; }
        public int TalkgroupID { get; private set; }
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
        public string TableName => "TempTowerTalkgroups";

        public void CopyFrom(Guid sessionID, TowerTalkgroup towerTalkgroup)
        {
            SessionID = sessionID;
            SystemID = towerTalkgroup.SystemID;
            TowerID = towerTalkgroup.TowerNumber;
            TalkgroupID = towerTalkgroup.TalkgroupID;
            Date = towerTalkgroup.Date;
            AffiliationCount = towerTalkgroup.AffiliationCount;
            DeniedCount = towerTalkgroup.DeniedCount;
            VoiceGrantCount = towerTalkgroup.VoiceGrantCount;
            EmergencyVoiceGrantCount = towerTalkgroup.EmergencyVoiceGrantCount;
            EncryptedVoiceGrantCount = towerTalkgroup.EncryptedVoiceGrantCount;
            DataCount = towerTalkgroup.DataCount;
            PrivateDataCount = towerTalkgroup.PrivateDataCount;
            AlertCount = towerTalkgroup.AlertCount;
            FirstSeen = towerTalkgroup.FirstSeen;
            LastSeen = towerTalkgroup.LastSeen;
        }

        public override string ToString() => $"Temp - System ID {SystemID}, Tower # {TowerID}, Talkgroup ID {TalkgroupID}";
    }
}
