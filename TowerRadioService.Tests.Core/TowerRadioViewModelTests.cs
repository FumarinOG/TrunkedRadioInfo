using System;
using Xunit;

namespace TowerRadioService.Tests.Core
{
    public class TowerRadioViewModelTests
    {
        [Fact]
        public void ConstructorAssignsProperValues()
        {
            var firstSeen = DateTime.Now.AddYears(-1);
            var lastSeen = DateTime.Now;

            var towerRadioViewModel = new TowerRadioViewModel(1917101, "ISP Dispatch (LaSalle) (17-A)", firstSeen, lastSeen, 2750, 3250, 3750, 4250, 4750);

            Assert.Equal(1917101, towerRadioViewModel.RadioID);
            Assert.Equal("ISP Dispatch (LaSalle) (17-A)", towerRadioViewModel.RadioName);
            Assert.Equal(firstSeen, towerRadioViewModel.FirstSeen);
            Assert.Equal(lastSeen, towerRadioViewModel.LastSeen);
            Assert.Equal(2750, towerRadioViewModel.AffiliationCount);
            Assert.Equal(3250, towerRadioViewModel.DeniedCount);
            Assert.Equal(3750, towerRadioViewModel.VoiceCount);
            Assert.Equal(4250, towerRadioViewModel.EncryptedCount);
            Assert.Equal(4750, towerRadioViewModel.DataCount);
        }
    }
}
