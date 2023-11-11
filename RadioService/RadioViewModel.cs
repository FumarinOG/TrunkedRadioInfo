using ServiceCommon;
using System;

namespace RadioService
{
    public sealed class RadioViewModel : DataViewModelBase
    {
        public int RadioID { get; private set; }
        public string RadioName { get; private set; }
        public int DataCount { get; private set; }

        public string DataCountText => $"{DataCount:#,##0}";

        public RadioViewModel()
        {
        }

        public RadioViewModel(int affiliationCount, int deniedCount, int voiceCount, int emergencyCount, int encryptedCount, bool phaseIISeen, DateTime firstSeen,
            DateTime lastSeen) : base(affiliationCount, deniedCount, voiceCount, emergencyCount, encryptedCount, phaseIISeen, firstSeen, lastSeen)
        {
        }
    }
}
