using ServiceCommon;
using System;

namespace TalkgroupHistoryService
{
    public sealed class TalkgroupHistoryViewModel : IViewModel
    {
        public string TalkgroupName { get; private set; }
        public DateTime FirstSeen { get; private set; }
        public DateTime LastSeen { get; private set; }

        public TalkgroupHistoryViewModel()
        {
        }

        public TalkgroupHistoryViewModel(string talkgroupName, DateTime firstSeen, DateTime lastSeen) =>
            (TalkgroupName, FirstSeen, LastSeen) = (talkgroupName, firstSeen, lastSeen);
    }
}
