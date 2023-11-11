using ServiceCommon;
using System;

namespace TowerFrequencyService
{
    public class TowerFrequencyViewModel : DataViewModelBase
    {
        public string Frequency { get; private set; }
        public string Channel { get; private set; }
        public string InputFrequency { get; private set; }
        public string InputChannel { get; private set; }
        public string Usage { get; private set; }
        public int DataCount { get; private set; }
        public string DataCountText => $"{DataCount:#,##0}";
        public int CWIDCount { get; private set; }
        public int AlertCount { get; private set; }

        public TowerFrequencyViewModel()
        {
        }

        public TowerFrequencyViewModel(int affiliationCount, int deniedCount, int voiceCount, int emergencyCount, int encryptedCount, bool phaseIISeen,
            DateTime firstSeen, DateTime lastSeen) : base(affiliationCount, deniedCount, voiceCount, emergencyCount, encryptedCount, phaseIISeen,
                firstSeen, lastSeen)
        {
        }
    }
}
