using ServiceCommon;
using System;

namespace TalkgroupRadioService
{
    public sealed class RadioTalkgroupViewModel : IViewModel
    {
        public int TalkgroupID { get; private set; }
        public string TalkgroupName { get; private set; }
        public DateTime FirstSeen { get; private set; }
        public DateTime LastSeen { get; private set; }
        public int AffiliationCount { get; private set; }
        public int DeniedCount { get; private set; }
        public int VoiceCount { get; private set; }
        public int EncryptedCount { get; private set; }

        public RadioTalkgroupViewModel()
        {
        }

        public RadioTalkgroupViewModel(int talkgroupID, string talkgroupName, DateTime firstSeen, DateTime lastSeen, int affiliationCount, int deniedCount,
            int voiceCount, int encryptedCount) =>

            (TalkgroupID, TalkgroupName, FirstSeen, LastSeen, AffiliationCount, DeniedCount, VoiceCount, EncryptedCount) =
            (talkgroupID, talkgroupName, firstSeen, lastSeen, affiliationCount, deniedCount, voiceCount, encryptedCount);
    }
}
