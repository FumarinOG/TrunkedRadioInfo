using DataLibrary.Interfaces;
using ObjectLibrary;
using System;

namespace DataLibrary.TempData
{
    public class TempTowerFrequencyUsage : ITempRecord<TowerFrequencyUsage>
    {
        public Guid SessionID { get; private set; }
        public int SystemID { get; private set; }
        public int TowerID { get; private set; }
        public string Channel { get; private set; }
        public string Frequency { get; private set; }
        public DateTime Date { get; private set; }
        public int? AffiliationCount { get; private set; }
        public int? DeniedCount { get; private set; }
        public int? VoiceGrantCount { get; private set; }
        public int? EmergencyVoiceGrantCount { get; private set; }
        public int? EncryptedVoiceGrantCount { get; private set; }
        public int? DataCount { get; private set; }
        public int? PrivateDataCount { get; private set; }
        public int? CWIDCount { get; private set; }
        public int? AlertCount { get; private set; }
        public DateTime FirstSeen { get; private set; }
        public DateTime LastSeen { get; private set; }

        [DataTableSkip]
        public string TableName => "TempTowerFrequencyUsage";

        public void CopyFrom(Guid sessionID, TowerFrequencyUsage towerFrequencyUsage)
        {
            SessionID = sessionID;
            SystemID = towerFrequencyUsage.SystemID;
            TowerID = towerFrequencyUsage.TowerID;
            Channel = towerFrequencyUsage.Channel;
            Frequency = towerFrequencyUsage.Frequency;
            Date = towerFrequencyUsage.Date;
            AffiliationCount = towerFrequencyUsage.AffiliationCount;
            DeniedCount = towerFrequencyUsage.DeniedCount;
            VoiceGrantCount = towerFrequencyUsage.VoiceGrantCount;
            EmergencyVoiceGrantCount = towerFrequencyUsage.EmergencyVoiceGrantCount;
            EncryptedVoiceGrantCount = towerFrequencyUsage.EncryptedVoiceGrantCount;
            DataCount = towerFrequencyUsage.DataCount;
            PrivateDataCount = towerFrequencyUsage.PrivateDataCount;
            CWIDCount = towerFrequencyUsage.CWIDCount;
            AlertCount = towerFrequencyUsage.AlertCount;
            FirstSeen = towerFrequencyUsage.FirstSeen;
            LastSeen = towerFrequencyUsage.LastSeen;
        }

        public override string ToString() => $"Temp - System {SystemID}, Tower {TowerID}, Frequency {Frequency}, Date {Date:MM-dd-yyyy}";

        private int VoiceGrantsTotal()
        {
            return ((VoiceGrantCount ?? 0) + (EmergencyVoiceGrantCount ?? 0));
        }

        private int DataGrantsTotal()
        {
            return ((DataCount ?? 0) + (PrivateDataCount ?? 0));
        }
    }
}
