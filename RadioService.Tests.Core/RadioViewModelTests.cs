using System;
using Xunit;

namespace RadioService.Tests.Core
{
    public class RadioViewModelTests
    {
        [Fact]
        public void ConstructorAssignsValuesProperly()
        {
            var firstSeen = DateTime.Now.AddYears(-1);
            var lastSeen = DateTime.Now;

            var radioViewModel = new RadioViewModel(100, 1000, 10000, 100000, 1000000, true, firstSeen, lastSeen);

            Assert.Equal(100, radioViewModel.AffiliationCount);
            Assert.Equal(1000, radioViewModel.DeniedCount);
            Assert.Equal(10000, radioViewModel.VoiceCount);
            Assert.Equal(100000, radioViewModel.EmergencyCount);
            Assert.Equal(1000000, radioViewModel.EncryptedCount);
            Assert.True(radioViewModel.PhaseIISeen);
            Assert.Equal(firstSeen, radioViewModel.FirstSeen);
            Assert.Equal(lastSeen, radioViewModel.LastSeen);
        }
    }
}
