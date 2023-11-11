using ServiceCommon;
using System;

namespace TowerRadioService
{
    public sealed class TowerRadioViewModel : IViewModel
    {
        public int RadioID { get; private set; }
        public string RadioName { get; private set; }
        public DateTime FirstSeen { get; private set; }
        public DateTime LastSeen { get; private set; }
        public int AffiliationCount { get; private set; }
        public int DeniedCount { get; private set; }
        public int VoiceCount { get; private set; }
        public int EncryptedCount { get; private set; }
        public int DataCount { get; private set; }

        public TowerRadioViewModel()
        {
        }

        public TowerRadioViewModel(int radioID, string radioName, DateTime firstSeen, DateTime lastSeen, int affiliationCount, int deniedCount, int voiceCount,
            int encryptedCount, int dataCount) =>

            (RadioID, RadioName, FirstSeen, LastSeen, AffiliationCount, DeniedCount, VoiceCount, EncryptedCount, DataCount) =
            (radioID, radioName, firstSeen, lastSeen, affiliationCount, deniedCount, voiceCount, encryptedCount, dataCount);
    }
}
