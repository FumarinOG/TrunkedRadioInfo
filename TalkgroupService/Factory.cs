using ObjectLibrary;
using ServiceCommon;

namespace TalkgroupService
{
    public static class Factory
    {
        public static TalkgroupDataViewModel GetTalkgroupDataModel(SystemInfo systemInfo, int talkgroupID, TalkgroupViewModel talkgroupViewModel,
            SearchDataViewModel searchData) =>

            new TalkgroupDataViewModel(systemInfo.SystemID, systemInfo.Description, talkgroupID, talkgroupViewModel, searchData);
    }
}
