namespace TalkgroupService
{
    public sealed class TalkgroupDetailViewModel
    {
        public string SystemID { get; private set; }
        public int TalkgroupID { get; private set; }
        public int TowerNumber { get; private set; }

        public TalkgroupDetailViewModel(string systemID, int talkgroupID) => (SystemID, TalkgroupID) = (systemID, talkgroupID);

        public TalkgroupDetailViewModel(string systemID, int talkgroupID, int towerNumber) =>
            (SystemID, TalkgroupID, TowerNumber) = (systemID, talkgroupID, towerNumber);
    }
}
