using ObjectLibrary.Abstracts;
using System;

namespace ObjectLibrary
{
    public class TowerFrequencyUsage : CounterRecordBase
    {
        public int SystemID
        {
            get => _systemID;
            set => SetProperty(ref _systemID, value);
        }

        public int TowerID
        {
            get => _towerID;
            set => SetProperty(ref _towerID, value);
        }

        public string Channel
        {
            get => _channel;
            set => SetProperty(ref _channel, value);
        }

        public string Frequency
        {
            get => _frequency;
            set => SetProperty(ref _frequency, value);
        }

        private int _systemID;
        private int _towerID;
        private string _channel;
        private string _frequency;

        public override string ToString() => 
            $"Frequency {_frequency}, Date {Date:MM-dd-yyyy}, Affiliations {AffiliationCount:#,##0}, VoiceGrants {VoiceGrantsTotal():#,##0}, Encrypted {EncryptedVoiceGrantCount:#,##0}, Data {DataGrantsTotal():#,##0}";

        private int VoiceGrantsTotal()
        {
            return (VoiceGrantCount + EmergencyVoiceGrantCount);
        }

        private int DataGrantsTotal()
        {
            return (DataCount + PrivateDataCount);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TowerFrequencyUsage compareObject))
            {
                return false;
            }

            return compareObject != null && (compareObject.SystemID == SystemID &&
                                             compareObject.TowerID == TowerID &&
                                             compareObject.Channel.Equals(Channel, StringComparison.OrdinalIgnoreCase) &&
                                             compareObject.Frequency.Equals(Frequency, StringComparison.OrdinalIgnoreCase) &&
                                             compareObject.HitCount == HitCount &&
                                             compareObject.VoiceGrantCount == VoiceGrantCount &&
                                             compareObject.EmergencyVoiceGrantCount == EmergencyVoiceGrantCount &&
                                             compareObject.EncryptedVoiceGrantCount == EncryptedVoiceGrantCount &&
                                             compareObject.DataCount == DataCount &&
                                             compareObject.PrivateDataCount == PrivateDataCount &&
                                             compareObject.CWIDCount == CWIDCount &&
                                             compareObject.FirstSeen == FirstSeen &&
                                             compareObject.LastSeen == LastSeen);
        }

        public override int GetHashCode()
        {
            return SystemID.GetHashCode() ^
                   TowerID.GetHashCode() ^
                   (Channel.IsNullOrWhiteSpace() ? 0 : Channel.GetHashCode()) ^
                   Frequency.GetHashCode() ^
                   HitCount.GetHashCode() ^
                   VoiceGrantCount.GetHashCode() ^
                   EmergencyVoiceGrantCount.GetHashCode() ^
                   EncryptedVoiceGrantCount.GetHashCode() ^
                   DataCount.GetHashCode() ^
                   PrivateDataCount.GetHashCode() ^
                   CWIDCount.GetHashCode() ^
                   FirstSeen.GetHashCode() ^
                   LastSeen.GetHashCode();
        }
    }
}
