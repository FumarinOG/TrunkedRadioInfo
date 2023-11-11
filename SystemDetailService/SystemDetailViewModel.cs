namespace SystemDetailService
{
    public sealed class SystemDetailViewModel
    {
        public string SystemID { get; private set; }
        public string SystemName { get; private set; }
        public int TalkgroupCount { get; private set; }
        public int RadioCount { get; private set; }
        public int TowerCount { get; private set; }
        public int PatchCount { get; private set; }
        public int ProcessedFileCount { get; private set; }

        public SystemDetailViewModel(string systemID) => SystemID = systemID;

        public SystemDetailViewModel(string systemID, string systemName, int talkgroupCount, int radioCount, int towerCount, int patchCount,
            int processedFileCount) =>

            (SystemID, SystemName, TalkgroupCount, RadioCount, TowerCount, PatchCount, ProcessedFileCount) =
            (systemID, systemName, talkgroupCount, radioCount, towerCount, patchCount, processedFileCount);
    }
}
