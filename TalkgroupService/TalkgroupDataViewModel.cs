using ServiceCommon;

namespace TalkgroupService
{
    public sealed class TalkgroupDataViewModel
    {
        public string SystemID { get; private set; }
        public string SystemName { get; private set; }
        public int TalkgroupID { get; private set; }
        public TalkgroupViewModel TalkgroupData { get; private set; }
        public SearchDataViewModel SearchData { get; private set; }

        public TalkgroupDataViewModel(string systemID, string systemName, int talkgroupID, TalkgroupViewModel talkgroupData, SearchDataViewModel searchData) =>
            (SystemID, SystemName, TalkgroupID, TalkgroupData, SearchData) = (systemID, systemName, talkgroupID, talkgroupData, searchData);
    }
}
