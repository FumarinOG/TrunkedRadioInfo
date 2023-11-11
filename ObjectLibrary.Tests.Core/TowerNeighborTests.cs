using System;
using Xunit;

namespace ObjectLibrary.Tests.Core
{
    public class TowerNeighborTests
    {
        [Fact]
        public void PropertiesAssignedProperly()
        {
            var firstSeen = DateTime.Now.AddYears(-1);
            var lastSeen = DateTime.Now;
            var towerNeighbor = new TowerNeighbor
            {
                SystemID = 1,
                TowerNumber = 42,
                NeighborSystemID = 1,
                NeighborTowerID = 84,
                NeighborTowerNumberHex = "T0122",
                NeighborChannel = "00-0133",
                NeighborFrequency = "851.83750",
                NeighborTowerName = "Ottawa (LaSalle)",
                FirstSeen = firstSeen,
                LastSeen = lastSeen
            };

            Assert.Equal(1, towerNeighbor.SystemID);
            Assert.Equal(42, towerNeighbor.TowerNumber);
            Assert.Equal(1, towerNeighbor.NeighborSystemID);
            Assert.Equal(84, towerNeighbor.NeighborTowerID);
            Assert.Equal("T0122", towerNeighbor.NeighborTowerNumberHex);
            Assert.Equal("00-0133", towerNeighbor.NeighborChannel);
            Assert.Equal("851.83750", towerNeighbor.NeighborFrequency);
            Assert.Equal("Ottawa (LaSalle)", towerNeighbor.NeighborTowerName);
            Assert.Equal(firstSeen, towerNeighbor.FirstSeen);
            Assert.Equal(lastSeen, towerNeighbor.LastSeen);
            Assert.True(towerNeighbor.IsDirty);
        }

        [Fact]
        public void ToStringWorks()
        {
            var towerNeighbor = new TowerNeighbor
            {
                SystemID = 1,
                TowerNumber = 42,
                NeighborTowerID = 84,
                NeighborTowerName = "Ottawa (LaSalle)"
            };

            Assert.Equal("System ID 1, Tower # 42, Neighbor Tower # 84 (Ottawa (LaSalle))", towerNeighbor.ToString());
        }
    }
}
