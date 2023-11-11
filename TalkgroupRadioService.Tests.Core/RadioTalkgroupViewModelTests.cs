using System;
using Xunit;

namespace TalkgroupRadioService.Tests.Core
{
    public class RadioTalkgroupViewModelTests
    {
        [Fact]
        public void ConstructorAssignsValuesProperly()
        {
            var firstSeen = DateTime.Now.AddYears(-1);
            var lastSeen = DateTime.Now;

            var radioTalkgroupViewModel = new RadioTalkgroupViewModel(9019, "ISP 17-A - LaSalle Primary", firstSeen, lastSeen, 1000, 2000, 3000, 4000);

            Assert.Equal(9019, radioTalkgroupViewModel.TalkgroupID);
            Assert.Equal("ISP 17-A - LaSalle Primary", radioTalkgroupViewModel.TalkgroupName);
            Assert.Equal(firstSeen, radioTalkgroupViewModel.FirstSeen);
            Assert.Equal(lastSeen, radioTalkgroupViewModel.LastSeen);
            Assert.Equal(1000, radioTalkgroupViewModel.AffiliationCount);
            Assert.Equal(2000, radioTalkgroupViewModel.DeniedCount);
            Assert.Equal(3000, radioTalkgroupViewModel.VoiceCount);
            Assert.Equal(4000, radioTalkgroupViewModel.EncryptedCount);
        }
    }
}
