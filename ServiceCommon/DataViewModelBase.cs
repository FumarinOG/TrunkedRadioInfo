using System;

namespace ServiceCommon
{
    public abstract class DataViewModelBase : IViewModel
    {
        public int AffiliationCount { get; private set; }

        public string AffiliationCountText => $"{AffiliationCount:#,##0}";

        public int DeniedCount { get; private set; }

        public string DeniedCountText => $"{DeniedCount:#,##0}";

        public int VoiceCount { get; private set; }

        public string VoiceCountText => $"{VoiceCount:#,##0}";

        public int EmergencyCount { get; private set; }

        public string EmergencyCountText => $"{EmergencyCount:#,##0}";

        public int EncryptedCount { get; private set; }

        public string EncryptedCountText => $"{EncryptedCount:#,##0}";

        public string EncryptionSeen => (EncryptedCount > 0) ? "Yes" : "No";

        public bool PhaseIISeen { get; private set; }

        public string PhaseIISeenText => (PhaseIISeen ? "Yes" : "No");

        public DateTime FirstSeen { get; private set; }

        public string FirstSeenText => $"{FirstSeen:MM-dd-yyyy HH:mm}";

        public DateTime LastSeen { get; private set; }

        public string LastSeenText => $"{LastSeen:MM-dd-yyyy HH:mm}";

        public DataViewModelBase()
        {
        }

        public DataViewModelBase(int affiliationCount, int deniedCount, int voiceCount, int emergencyCount, int encryptedCount, bool phaseIISeen,
            DateTime firstSeen, DateTime lastSeen) =>

            (AffiliationCount, DeniedCount, VoiceCount, EmergencyCount, EncryptedCount, PhaseIISeen, FirstSeen, LastSeen) =
            (affiliationCount, deniedCount, voiceCount, emergencyCount, encryptedCount, phaseIISeen, firstSeen, lastSeen);
    }
}
