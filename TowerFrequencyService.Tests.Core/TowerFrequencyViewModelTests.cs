using System;
using Xunit;

namespace TowerFrequencyService.Tests.Core
{
    public class TowerFrequencyViewModelTests
    {
        [Fact]
        public void ConstructorAssignsValuesProperly()
        {
            var firstSeen = DateTime.Now.AddYears(-1);
            var lastSeen = DateTime.Now;

            var towerFrequencyViewModel = new TowerFrequencyViewModel(1750, 2750, 3750, 4750, 5750, true, firstSeen, lastSeen);

            Assert.Equal(1750, towerFrequencyViewModel.AffiliationCount);
            Assert.Equal(2750, towerFrequencyViewModel.DeniedCount);
            Assert.Equal(3750, towerFrequencyViewModel.VoiceCount);
            Assert.Equal(4750, towerFrequencyViewModel.EmergencyCount);
            Assert.Equal(5750, towerFrequencyViewModel.EncryptedCount);
            Assert.True(towerFrequencyViewModel.PhaseIISeen);
        }
    }
}
