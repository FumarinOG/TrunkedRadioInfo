using Xunit;

namespace TowerService.Tests.Core
{
    public class TowerDetailViewModelTests
    {
        [Fact]
        public void ConstructorAssignsValuesProperly()
        {
            var towerDetailViewModel = new TowerDetailViewModel("140", 1031);

            Assert.Equal("140", towerDetailViewModel.SystemID);
            Assert.Equal(1031, towerDetailViewModel.TowerNumber);
        }
    }
}
