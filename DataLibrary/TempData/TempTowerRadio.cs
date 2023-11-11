using DataLibrary.Interfaces;
using ObjectLibrary;
using System;

namespace DataLibrary.TempData
{
    public class TempTowerRadio : ITempRecord<TowerRadio>
    {
        public Guid SessionID { get; private set; }
        public int SystemID { get; private set; }
        public int TowerID { get; private set; }
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
        public string TableName => "TempTowerRadios";

        public void CopyFrom(Guid sessionID, TowerRadio towerRadio)
        {
            SessionID = sessionID;
            SystemID = towerRadio.SystemID;
            TowerID = towerRadio.TowerNumber;
            RadioID = towerRadio.RadioID;
            Date = towerRadio.Date;
            AffiliationCount = towerRadio.AffiliationCount;
            DeniedCount = towerRadio.DeniedCount;
            VoiceGrantCount = towerRadio.VoiceGrantCount;
            EmergencyVoiceGrantCount = towerRadio.EmergencyVoiceGrantCount;
            EncryptedVoiceGrantCount = towerRadio.EncryptedVoiceGrantCount;
            DataCount = towerRadio.DataCount;
            PrivateDataCount = towerRadio.PrivateDataCount;
            AlertCount = towerRadio.AlertCount;
            FirstSeen = towerRadio.FirstSeen;
            LastSeen = towerRadio.LastSeen;
        }

        public override string ToString() => $"Temp - System ID {SystemID}, Tower # {TowerID}, Radio ID {RadioID}";
    }
}
