using DataLibrary.Interfaces;
using ObjectLibrary;
using System;

namespace DataLibrary.TempData
{
    public class TempTowerFrequencyRadio : ITempRecord<TowerFrequencyRadio>
    {
        public Guid SessionID { get; private set; }
        public int SystemID { get; private set; }
        public int TowerID { get; private set; }
        public string Frequency { get; private set; }
        public int RadioID { get; private set; }
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
        public string TableName => "TempTowerFrequencyRadios";

        public void CopyFrom(Guid sessionID, TowerFrequencyRadio towerFrequencyRadio)
        {
            SessionID = sessionID;
            SystemID = towerFrequencyRadio.SystemID;
            TowerID = towerFrequencyRadio.TowerID;
            Frequency = towerFrequencyRadio.Frequency;
            RadioID = towerFrequencyRadio.RadioID;
            Date = towerFrequencyRadio.Date;
            AffiliationCount = towerFrequencyRadio.AffiliationCount;
            DeniedCount = towerFrequencyRadio.DeniedCount;
            VoiceGrantCount = towerFrequencyRadio.VoiceGrantCount;
            EmergencyVoiceGrantCount = towerFrequencyRadio.EmergencyVoiceGrantCount;
            EncryptedVoiceGrantCount = towerFrequencyRadio.EncryptedVoiceGrantCount;
            DataCount = towerFrequencyRadio.DataCount;
            PrivateDataCount = towerFrequencyRadio.PrivateDataCount;
            AlertCount = towerFrequencyRadio.AlertCount;
            FirstSeen = towerFrequencyRadio.FirstSeen;
            LastSeen = towerFrequencyRadio.LastSeen;
        }

        public override string ToString() => $"Temp - Tower {TowerID}, Frequency {Frequency}, Radio {RadioID}";
    }
}
