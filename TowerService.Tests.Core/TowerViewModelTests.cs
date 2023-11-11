using System;
using Xunit;

namespace TowerService.Tests.Core
{
    public class TowerViewModelTests
    {
        [Fact]
        public void ConstructorAssignsValuesProperly()
        {
            var firstSeen = DateTime.Now.AddYears(-1);
            var lastSeen = DateTime.Now;

            var towerViewModel = new TowerViewModel(1031, "LaSalle (LaSalle)", firstSeen, lastSeen, 15000);

            Assert.Equal(1031, towerViewModel.TowerNumber);
            Assert.Equal("LaSalle (LaSalle)", towerViewModel.TowerName);
            Assert.Equal("1031 (LaSalle (LaSalle))", towerViewModel.Title);
            Assert.Equal(firstSeen, towerViewModel.FirstSeen);
            Assert.Equal(lastSeen, towerViewModel.LastSeen);
        }
    }
}
