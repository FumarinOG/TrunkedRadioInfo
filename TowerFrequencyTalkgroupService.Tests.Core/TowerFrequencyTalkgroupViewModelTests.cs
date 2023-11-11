using System;
using Xunit;

namespace TowerFrequencyTalkgroupService.Tests.Core
{
    public class TowerFrequencyTalkgroupViewModelTests
    {
        [Fact]
        public void ConstructorAssignsValuesProperly()
        {
            var firstSeen = DateTime.Now.AddYears(-1);
            var lastSeen = DateTime.Now;

            var towerFrequencyTalkgroupViewModel = new TowerFrequencyTalkgroupViewModel(9019, "ISP 17-A - LaSalle Primary", firstSeen, lastSeen,
                15000, 16000, 17000, 18000, 19000, 20000);

            Assert.Equal(9019, towerFrequencyTalkgroupViewModel.TalkgroupID);
            Assert.Equal("ISP 17-A - LaSalle Primary", towerFrequencyTalkgroupViewModel.TalkgroupName);
            Assert.Equal(firstSeen, towerFrequencyTalkgroupViewModel.FirstSeen);
            Assert.Equal(lastSeen, towerFrequencyTalkgroupViewModel.LastSeen);
            Assert.Equal(15000, towerFrequencyTalkgroupViewModel.AffiliationCount);
            Assert.Equal(16000, towerFrequencyTalkgroupViewModel.DeniedCount);
            Assert.Equal(17000, towerFrequencyTalkgroupViewModel.VoiceCount);
            Assert.Equal(18000, towerFrequencyTalkgroupViewModel.EncryptedCount);
            Assert.Equal(19000, towerFrequencyTalkgroupViewModel.EmergencyCount);
            Assert.Equal(20000, towerFrequencyTalkgroupViewModel.AlertCount);
        }
    }
}
