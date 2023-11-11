using ServiceCommon;
using System;

namespace TalkgroupService
{
    public sealed class TalkgroupViewModel : DataViewModelBase
    {
        public int TalkgroupID { get; private set; }
        public string TalkgroupName { get; private set; }
        public int PatchCount { get; private set; }

        public string PatchCountText
        {
            get => $"{PatchCount:#,##0}";
        }

        public string Towers { get; private set; }

        public TalkgroupViewModel()
        {
        }

        public TalkgroupViewModel(int talkgroupID, string talkgroupName, int patchCount, string towers, int affiliationCount, int deniedCount, int voiceCount,
            int emergencyCount, int encryptedCount, bool phaseIISeen, DateTime firstSeen, DateTime lastSeen) : base(affiliationCount, deniedCount, voiceCount,
                emergencyCount, encryptedCount, phaseIISeen, firstSeen, lastSeen) =>

            (TalkgroupID, TalkgroupName, PatchCount, Towers) = (talkgroupID, talkgroupName, patchCount, towers);
    }
}
