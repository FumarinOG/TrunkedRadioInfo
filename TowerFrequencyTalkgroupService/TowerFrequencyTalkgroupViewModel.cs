using System;

namespace TowerFrequencyTalkgroupService
{
    public sealed class TowerFrequencyTalkgroupViewModel
    {
        public int TalkgroupID { get; private set; }
        public string TalkgroupName { get; private set; }
        public DateTime FirstSeen { get; private set; }
        public DateTime LastSeen { get; private set; }
        public int AffiliationCount { get; private set; }
        public int DeniedCount { get; private set; }
        public int VoiceCount { get; private set; }
        public int EncryptedCount { get; private set; }
        public int EmergencyCount { get; private set; }
        public int AlertCount { get; private set; }

        public TowerFrequencyTalkgroupViewModel()
        {

        }

        public TowerFrequencyTalkgroupViewModel(int talkgroupID, string talkgroupName, DateTime firstSeen, DateTime lastSeen, int affiliationCount,
            int deniedCount, int voiceCount, int encryptedCount, int emergencyCount, int alertCount) =>

            (TalkgroupID, TalkgroupName, FirstSeen, LastSeen, AffiliationCount, DeniedCount, VoiceCount, EncryptedCount, EmergencyCount, AlertCount) =
            (talkgroupID, talkgroupName, firstSeen, lastSeen, affiliationCount, deniedCount, voiceCount, encryptedCount, emergencyCount, alertCount);
    }
}
