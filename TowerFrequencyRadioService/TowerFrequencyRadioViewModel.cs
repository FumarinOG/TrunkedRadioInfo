using System;

namespace TowerFrequencyRadioService
{
    public sealed class TowerFrequencyRadioViewModel
    {
        public int RadioID { get; private set; }
        public string RadioName { get; private set; }
        public DateTime FirstSeen { get; private set; }
        public DateTime LastSeen { get; private set; }
        public int AffiliationCount { get; private set; }
        public int DeniedCount { get; private set; }
        public int VoiceCount { get; private set; }
        public int EncryptedCount { get; private set; }
        public int EmergencyCount { get; private set; }
        public int AlertCount { get; private set; }
        public int DataCount { get; private set; }

        public TowerFrequencyRadioViewModel()
        {
        }

        public TowerFrequencyRadioViewModel(int radioID, string radioName, DateTime firstSeen, DateTime lastSeen, int affiliationCount,
            int deniedCount, int voiceCount, int encryptedCount, int emergencyCount, int alertCount, int dataCount) =>

            (RadioID, RadioName, FirstSeen, LastSeen, AffiliationCount, DeniedCount, VoiceCount, EncryptedCount, EmergencyCount, AlertCount, DataCount) =
            (radioID, radioName, firstSeen, lastSeen, affiliationCount, deniedCount, voiceCount, encryptedCount, emergencyCount, alertCount, dataCount);
    }
}
