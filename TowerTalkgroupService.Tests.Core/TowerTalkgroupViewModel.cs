using System;
using Xunit;

namespace TowerTalkgroupService.Tests.Core
{
    public class TowerTalkgroupViewModelTests
    {
        [Fact]
        public void ConstructorAssignsProperValues()
        {
            var firstSeen = DateTime.Now.AddYears(-1);
            var lastSeen = DateTime.Now;

            var towerTalkgroupViewModel = new TowerTalkgroupViewModel(9019, "ISP 17-A - LaSalle Primary", firstSeen, lastSeen, 500, 1000, 1500, 2000);

            Assert.Equal(9019, towerTalkgroupViewModel.TalkgroupID);
            Assert.Equal("ISP 17-A - LaSalle Primary", towerTalkgroupViewModel.TalkgroupName);
            Assert.Equal(firstSeen, towerTalkgroupViewModel.FirstSeen);
            Assert.Equal(lastSeen, towerTalkgroupViewModel.LastSeen);
            Assert.Equal(500, towerTalkgroupViewModel.AffiliationCount);
            Assert.Equal(1000, towerTalkgroupViewModel.DeniedCount);
            Assert.Equal(1500, towerTalkgroupViewModel.VoiceCount);
            Assert.Equal(2000, towerTalkgroupViewModel.EncryptedCount);
        }
    }
}
