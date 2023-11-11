using DataLibrary.Interfaces;
using ObjectLibrary;
using System;

namespace DataLibrary.TempData
{
    public class TempTowerTalkgroupRadio : ITempRecord<TowerTalkgroupRadio>
    {
        public Guid SessionID { get; private set; }
        public int SystemID { get; private set; }
        public int TowerID { get; private set; }
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
        public string TableName => "TempTowerTalkgroupRadios";

        public void CopyFrom(Guid sessionID, TowerTalkgroupRadio towerTalkgroupRadio)
        {
            SessionID = sessionID;
            SystemID = towerTalkgroupRadio.SystemID;
            TowerID = towerTalkgroupRadio.TowerNumber;
            TalkgroupID = towerTalkgroupRadio.TalkgroupID;
            RadioID = towerTalkgroupRadio.RadioID;
            Date = towerTalkgroupRadio.Date;
            AffiliationCount = towerTalkgroupRadio.AffiliationCount;
            DeniedCount = towerTalkgroupRadio.DeniedCount;
            VoiceGrantCount = towerTalkgroupRadio.VoiceGrantCount;
            EmergencyVoiceGrantCount = towerTalkgroupRadio.EmergencyVoiceGrantCount;
            EncryptedVoiceGrantCount = towerTalkgroupRadio.EncryptedVoiceGrantCount;
            DataCount = towerTalkgroupRadio.DataCount;
            PrivateDataCount = towerTalkgroupRadio.PrivateDataCount;
            AlertCount = towerTalkgroupRadio.AlertCount;
            FirstSeen = towerTalkgroupRadio.FirstSeen;
            LastSeen = towerTalkgroupRadio.LastSeen;
        }

        public override string ToString() => $"Temp - System ID {SystemID}, Tower # {TowerID}, Talkgroup ID {TalkgroupID}, Radio ID {RadioID}";
    }
}
