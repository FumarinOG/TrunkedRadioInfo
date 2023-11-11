using System;

namespace PatchService
{
    public sealed class PatchDetailViewModel : PatchViewModel
    {
        public int TowerNumber { get; private set; }
        public string TowerName { get; private set; }

        public PatchDetailViewModel(int towerNumber, string towerName, string systemID, string systemName, int fromTalkgroupID, string fromTalkgroupName,
            int toTalkgroupID, string toTalkgroupName, DateTime firstSeen, DateTime lastSeen, int hitCount) : base(systemID, systemName, fromTalkgroupID,
                fromTalkgroupName, toTalkgroupID, toTalkgroupName, firstSeen, lastSeen, hitCount) =>

            (TowerNumber, TowerName) = (towerNumber, towerName);
    }
}
