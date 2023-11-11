using System;
using Xunit;

namespace TowerNeighborService.Tests.Core
{
    public class TowerNeighborViewModelTests
    {
        [Fact]
        public void ConstructorAssignsValuesProperly()
        {
            var firstSeen = DateTime.Now.AddYears(-1);
            var lastSeen = DateTime.Now;

            var towerNeighborViewModel = new TowerNeighborViewModel(1034, "Ottawa (LaSalle)", "851.83750", firstSeen, lastSeen);

            Assert.Equal(1034, towerNeighborViewModel.NeighborTowerNumber);
            Assert.Equal("Ottawa (LaSalle)", towerNeighborViewModel.NeighborTowerName);
            Assert.Equal("851.83750", towerNeighborViewModel.NeighborControlChannel);
            Assert.Equal(firstSeen, towerNeighborViewModel.FirstSeen);
            Assert.Equal(lastSeen, towerNeighborViewModel.LastSeen);
        }
    }
}
