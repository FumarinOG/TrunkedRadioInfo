using System;
using Xunit;

namespace TowerFrequencyRadioService.Tests.Core
{
    public class TowerFrequencyRadioViewModelTests
    {
        [Fact]
        public void ConstructorAssignsValuesProperly()
        {
            var firstSeen = DateTime.Now.AddYears(-1);
            var lastSeen = DateTime.Now;

            var towerFrequencyRadioViewModel = new TowerFrequencyRadioViewModel(1917101, "ISP Dispatch (LaSalle) (17-A)", firstSeen, lastSeen, 12500,
                13500, 14500, 15500, 16500, 17500, 18500);

            Assert.Equal(1917101, towerFrequencyRadioViewModel.RadioID);
            Assert.Equal("ISP Dispatch (LaSalle) (17-A)", towerFrequencyRadioViewModel.RadioName);
            Assert.Equal(firstSeen, towerFrequencyRadioViewModel.FirstSeen);
            Assert.Equal(lastSeen, towerFrequencyRadioViewModel.LastSeen);
            Assert.Equal(12500, towerFrequencyRadioViewModel.AffiliationCount);
            Assert.Equal(13500, towerFrequencyRadioViewModel.DeniedCount);
            Assert.Equal(14500, towerFrequencyRadioViewModel.VoiceCount);
            Assert.Equal(15500, towerFrequencyRadioViewModel.EncryptedCount);
            Assert.Equal(16500, towerFrequencyRadioViewModel.EmergencyCount);
            Assert.Equal(17500, towerFrequencyRadioViewModel.AlertCount);
            Assert.Equal(18500, towerFrequencyRadioViewModel.DataCount);
        }
    }
}
