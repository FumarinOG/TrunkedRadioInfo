using ServiceCommon;
using System;

namespace TalkgroupRadioService
{
    public sealed class TalkgroupRadioViewModel : IViewModel
    {
        public int RadioID { get; private set; }
        public string RadioName { get; private set; }
        public DateTime FirstSeen { get; private set; }
        public DateTime LastSeen { get; private set; }
        public int AffiliationCount { get; private set; }
        public int DeniedCount { get; private set; }
        public int VoiceCount { get; private set; }
        public int EncryptedCount { get; private set; }

        public TalkgroupRadioViewModel()
        {
        }

        public TalkgroupRadioViewModel(int radioID, string radioName, DateTime firstSeen, DateTime lastSeen, int affiliationCount, int deniedCount,
            int voiceCount, int encryptedCount) =>

            (RadioID, RadioName, FirstSeen, LastSeen, AffiliationCount, DeniedCount, VoiceCount, EncryptedCount) =
            (radioID, radioName, firstSeen, lastSeen, affiliationCount, deniedCount, voiceCount, encryptedCount);
    }
}
