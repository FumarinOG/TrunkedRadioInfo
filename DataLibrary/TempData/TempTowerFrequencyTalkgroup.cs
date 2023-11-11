using DataLibrary.Interfaces;
using ObjectLibrary;
using System;

namespace DataLibrary.TempData
{
    public class TempTowerFrequencyTalkgroup : ITempRecord<TowerFrequencyTalkgroup>
    {
        public Guid SessionID { get; private set; }
        public int SystemID { get; private set; }
        public int TowerID { get; private set; }
        public string Frequency { get; private set; }
        public int TalkgroupID { get; private set; }
        public DateTime Date { get; private set; }
        public int? AffiliationCount { get; private set; }
        public int? DeniedCount { get; private set; }
        public int? VoiceGrantCount { get; private set; }
        public int? EmergencyVoiceGrantCount { get; private set; }
        public int? EncryptedVoiceGrantCount { get; private set; }
        public int? DataCount { get; private set; }
        public int? PrivateDataCount { get; private set; }
        public int? AlertCount { get; private set; }
        public DateTime FirstSeen { get; private set; }
        public DateTime LastSeen { get; private set; }

        [DataTableSkip]
        public string TableName => "TempTowerFrequencyTalkgroups";

        public void CopyFrom(Guid sessionID, TowerFrequencyTalkgroup towerFrequencyTalkgroup)
        {
            SessionID = sessionID;
            SystemID = towerFrequencyTalkgroup.SystemID;
            TowerID = towerFrequencyTalkgroup.TowerID;
            Frequency = towerFrequencyTalkgroup.Frequency;
            TalkgroupID = towerFrequencyTalkgroup.TalkgroupID;
            Date = towerFrequencyTalkgroup.Date;
            AffiliationCount = towerFrequencyTalkgroup.AffiliationCount;
            DeniedCount = towerFrequencyTalkgroup.DeniedCount;
            VoiceGrantCount = towerFrequencyTalkgroup.VoiceGrantCount;
            EmergencyVoiceGrantCount = towerFrequencyTalkgroup.EmergencyVoiceGrantCount;
            EncryptedVoiceGrantCount = towerFrequencyTalkgroup.EncryptedVoiceGrantCount;
            DataCount = towerFrequencyTalkgroup.DataCount;
            PrivateDataCount = towerFrequencyTalkgroup.PrivateDataCount;
            AlertCount = towerFrequencyTalkgroup.AlertCount;
            FirstSeen = towerFrequencyTalkgroup.FirstSeen;
            LastSeen = towerFrequencyTalkgroup.LastSeen;
        }

        public override string ToString() => $"Temp - Tower {TowerID}, Frequency {Frequency}, Talkgroup {TalkgroupID}";
    }
}
