using ServiceCommon;
using Xunit;

namespace TowerFrequencyService.Tests.Core
{
    public class TowerFrequencyDataViewModelTests
    {
        [Fact]
        public void ConstructorsAssignsValuesProperly()
        {
            var towerFrequencyDataViewModel = new TowerFrequencyDataViewModel("140", "StarCom 21", 1031, "LaSalle (LaSalle)",
                new TowerFrequencyViewModel(), new SearchDataViewModel());

            Assert.Equal("140", towerFrequencyDataViewModel.SystemID);
            Assert.Equal("StarCom 21", towerFrequencyDataViewModel.SystemName);
            Assert.Equal(1031, towerFrequencyDataViewModel.TowerNumber);
            Assert.Equal("LaSalle (LaSalle)", towerFrequencyDataViewModel.TowerName);
            Assert.IsAssignableFrom<TowerFrequencyViewModel>(towerFrequencyDataViewModel.TowerFrequencyData);
            Assert.IsAssignableFrom<SearchDataViewModel>(towerFrequencyDataViewModel.SearchData);
        }
    }
}
