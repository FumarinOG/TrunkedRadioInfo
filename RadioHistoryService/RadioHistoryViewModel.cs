using ServiceCommon;
using System;

namespace RadioHistoryService
{
    public sealed class RadioHistoryViewModel : IViewModel
    {
        public string RadioName { get; private set; }
        public DateTime FirstSeen { get; private set; }
        public DateTime LastSeen { get; private set; }

        public RadioHistoryViewModel()
        {
        }

        public RadioHistoryViewModel(string radioName, DateTime firstSeen, DateTime lastSeen) =>
            (RadioName, FirstSeen, LastSeen) = (radioName, firstSeen, lastSeen);
    }
}
