using System;
using Xunit;

namespace TalkgroupHistoryService.Tests.Core
{
    public class TalkgroupHistoryViewModelTests
    {
        [Fact]
        public void ConstructorAssignsValuesProperly()
        {
            var firstSeen = DateTime.Now.AddYears(-1);
            var lastSeen = DateTime.Now;

            var talkgroupHistoryViewModel = new TalkgroupHistoryViewModel("ISP 17-A - LaSalle Primary", firstSeen, lastSeen);

            Assert.Equal("ISP 17-A - LaSalle Primary", talkgroupHistoryViewModel.TalkgroupName);
            Assert.Equal(firstSeen, talkgroupHistoryViewModel.FirstSeen);
            Assert.Equal(lastSeen, talkgroupHistoryViewModel.LastSeen);
        }
    }
}
