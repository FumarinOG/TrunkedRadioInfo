using ServiceCommon;
using System;

namespace PatchService
{
    public class PatchViewModel : IViewModel
    {
        public string SystemID { get; private set; }
        public string SystemName { get; private set; }
        public int FromTalkgroupID { get; private set; }
        public string FromTalkgroupName { get; private set; }
        public int ToTalkgroupID { get; private set; }
        public string ToTalkgroupName { get; private set; }
        public DateTime FirstSeen { get; private set; }
        public string FirstSeenText => $"{FirstSeen:MM-dd-yyyy HH:mm}";
        public DateTime LastSeen { get; private set; }
        public string LastSeenText => $"{LastSeen:MM-dd-yyyy HH:mm}";
        public int HitCount { get; private set; }
        public string HitCountText => $"{HitCount:#,##0}";

        public PatchViewModel()
        {
        }

        public PatchViewModel(string systemID, string systemName, int fromTalkgroupID, string fromTalkgroupName, int toTalkgroupID, string toTalkgroupName,
            int hitCount) : this(systemID, systemName, fromTalkgroupID, fromTalkgroupName, toTalkgroupID, toTalkgroupName, DateTime.Now, DateTime.Now, hitCount)
        {
        }

        public PatchViewModel(string systemID, string systemName, int fromTalkgroupID, string fromTalkgroupName, int toTalkgroupID, string toTalkgroupName,
            DateTime firstSeen, DateTime lastSeen, int hitCount) =>

            (SystemID, SystemName, FromTalkgroupID, FromTalkgroupName, ToTalkgroupID, ToTalkgroupName, FirstSeen, LastSeen, HitCount) =
            (systemID, systemName, fromTalkgroupID, fromTalkgroupName, toTalkgroupID, toTalkgroupName, firstSeen, lastSeen, hitCount);
    }
}
