using System;

namespace SystemInfoService
{
    public sealed class SystemInfoViewModel
    {
        public int ID { get; private set; }
        public string SystemID { get; private set; }
        public string Description { get; private set; }
        public DateTime FirstSeen { get; private set; }
        public DateTime LastSeen { get; private set; }
        public int TalkgroupCount { get; private set; }
        public int RadioCount { get; private set; }
        public int TowerCount { get; private set; }
        public int RowCount { get; private set; }

        public string SystemIDDescription => $"{SystemID} - {Description}";

        public SystemInfoViewModel(int id, string systemID, string description, DateTime firstSeen, DateTime lastSeen, int talkgroupCount,
            int radioCount, int towerCount, int rowCount) =>

            (ID, SystemID, Description, FirstSeen, LastSeen, TalkgroupCount, RadioCount, TowerCount, RowCount) =
            (id, systemID, description, firstSeen, lastSeen, talkgroupCount, radioCount, towerCount, rowCount);
    }
}
