using System;
using Xunit;

namespace TowerTalkgroupService.Tests.Core
{
    public class TalkgroupTowerViewModelTests
    {
        [Fact]
        public void ConstructorAssignsProperValues()
        {
            var firstSeen = DateTime.Now.AddYears(-1);
            var lastSeen = DateTime.Now;

            var talkgroupTowerViewModel = new TalkgroupTowerViewModel(1031, "LaSalle (LaSalle)", firstSeen, lastSeen, 500, 1000, 1500, 2000);

            Assert.Equal(1031, talkgroupTowerViewModel.TowerNumber);
            Assert.Equal("LaSalle (LaSalle)", talkgroupTowerViewModel.TowerName);
            Assert.Equal(firstSeen, talkgroupTowerViewModel.FirstSeen);
            Assert.Equal(lastSeen, talkgroupTowerViewModel.LastSeen);
            Assert.Equal(500, talkgroupTowerViewModel.AffiliationCount);
            Assert.Equal(1000, talkgroupTowerViewModel.DeniedCount);
            Assert.Equal(1500, talkgroupTowerViewModel.VoiceCount);
            Assert.Equal(2000, talkgroupTowerViewModel.EncryptedCount);
        }
    }
}
