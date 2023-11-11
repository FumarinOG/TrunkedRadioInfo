using ServiceCommon;
using Xunit;

namespace TowerService.Tests.Core
{
    public class TowerDataViewModelTests
    {
        [Fact]
        public void ConstructorAssignsValuesProperlyWith5Properties()
        {
            var towerDataViewModel = new TowerDataViewModel("140", "StarCom 21", 1031, new TowerViewModel(), new SearchDataViewModel());

            Assert.Equal("140", towerDataViewModel.SystemID);
            Assert.Equal("StarCom 21", towerDataViewModel.SystemName);
            Assert.Equal(1031, towerDataViewModel.TowerNumber);
            Assert.IsAssignableFrom<TowerViewModel>(towerDataViewModel.TowerData);
            Assert.IsAssignableFrom<SearchDataViewModel>(towerDataViewModel.SearchData);
        }

        [Fact]
        public void ConstructorAssignsValuesProperlyWith6Properties()
        {
            var towerDataViewModel = new TowerDataViewModel("140", "StarCom 21", 1031, "LaSalle (LaSalle)", new TowerViewModel(), new SearchDataViewModel());

            Assert.Equal("140", towerDataViewModel.SystemID);
            Assert.Equal("StarCom 21", towerDataViewModel.SystemName);
            Assert.Equal(1031, towerDataViewModel.TowerNumber);
            Assert.Equal("LaSalle (LaSalle)", towerDataViewModel.TowerName);
            Assert.IsAssignableFrom<TowerViewModel>(towerDataViewModel.TowerData);
            Assert.IsAssignableFrom<SearchDataViewModel>(towerDataViewModel.SearchData);
        }
    }
}
