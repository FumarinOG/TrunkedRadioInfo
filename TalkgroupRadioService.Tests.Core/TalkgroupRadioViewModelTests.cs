using System;
using Xunit;

namespace TalkgroupRadioService.Tests.Core
{
    public class TalkgroupRadioViewModelTests
    {
        [Fact]
        public void ConstructorAssignsValuesProperly()
        {
            var firstSeen = DateTime.Now.AddYears(-1);
            var lastSeen = DateTime.Now;

            var talkgroupRadioViewModel = new TalkgroupRadioViewModel(1917101, "ISP Dispatch (LaSalle) (17-A)", firstSeen, lastSeen, 1500, 2500, 3500, 4500);

            Assert.Equal(1917101, talkgroupRadioViewModel.RadioID);
            Assert.Equal("ISP Dispatch (LaSalle) (17-A)", talkgroupRadioViewModel.RadioName);
            Assert.Equal(firstSeen, talkgroupRadioViewModel.FirstSeen);
            Assert.Equal(lastSeen, talkgroupRadioViewModel.LastSeen);
            Assert.Equal(1500, talkgroupRadioViewModel.AffiliationCount);
            Assert.Equal(2500, talkgroupRadioViewModel.DeniedCount);
            Assert.Equal(3500, talkgroupRadioViewModel.VoiceCount);
            Assert.Equal(4500, talkgroupRadioViewModel.EncryptedCount);
        }
    }
}
