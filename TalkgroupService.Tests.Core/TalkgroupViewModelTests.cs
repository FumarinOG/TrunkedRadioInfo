using System;
using Xunit;

namespace TalkgroupService.Tests.Core
{
    public class TalkgroupViewModelTests
    {
        [Fact]
        public void ConstructorAssignsValuesProperly()
        {
            var firstSeen = DateTime.Now.AddYears(-1);
            var lastSeen = DateTime.Now;

            var talkgroupViewModel = new TalkgroupViewModel(9019, "ISP 17-A - LaSalle Primary", 100250, "1031, 1034", 450, 550, 650, 750, 850,
                true, firstSeen, lastSeen);

            Assert.Equal(9019, talkgroupViewModel.TalkgroupID);
            Assert.Equal("ISP 17-A - LaSalle Primary", talkgroupViewModel.TalkgroupName);
            Assert.Equal(100250, talkgroupViewModel.PatchCount);
            Assert.Equal("100,250", talkgroupViewModel.PatchCountText);
            Assert.Equal("1031, 1034", talkgroupViewModel.Towers);
            Assert.Equal(450, talkgroupViewModel.AffiliationCount);
            Assert.Equal(550, talkgroupViewModel.DeniedCount);
            Assert.Equal(650, talkgroupViewModel.VoiceCount);
            Assert.Equal(750, talkgroupViewModel.EmergencyCount);
            Assert.Equal(850, talkgroupViewModel.EncryptedCount);
            Assert.True(talkgroupViewModel.PhaseIISeen);
            Assert.Equal(firstSeen, talkgroupViewModel.FirstSeen);
            Assert.Equal(lastSeen, talkgroupViewModel.LastSeen);
        }
    }
}
