using Xunit;

namespace TowerFrequencyService.Tests.Core
{
    public class TowerFrequencyDetailViewModelTests
    {
        [Fact]
        public void ConstructorAssignsValuesProperlyWith3Parameters()
        {
            var towerFrequencyDetailViewModel = new TowerFrequencyDetailViewModel("140", 1031, "851.95000");

            Assert.Equal("140", towerFrequencyDetailViewModel.SystemID);
            Assert.Equal(1031, towerFrequencyDetailViewModel.TowerNumber);
            Assert.Equal("851.95000", towerFrequencyDetailViewModel.Frequency);
        }

        [Fact]
        public void ConstructorAssignsValuesProperlyWith4Parameters()
        {
            var towerFrequencyDetailViewModel = new TowerFrequencyDetailViewModel("140", 1031, "LaSalle (LaSalle)", "851.95000");

            Assert.Equal("140", towerFrequencyDetailViewModel.SystemID);
            Assert.Equal(1031, towerFrequencyDetailViewModel.TowerNumber);
            Assert.Equal("LaSalle (LaSalle)", towerFrequencyDetailViewModel.TowerName);
            Assert.Equal("851.95000", towerFrequencyDetailViewModel.Frequency);
        }
    }
}
