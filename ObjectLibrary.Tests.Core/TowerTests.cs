using System;
using Xunit;

namespace ObjectLibrary.Tests.Core
{
    public class TowerTests
    {
        [Fact]
        public void PropertiesAssignProperly()
        {
            var tower = new Tower
            {
                SystemID = 1,
                TowerNumber = 1031,
                TowerNumberHex = "T011F",
                Description = "LaSalle (LaSalle)",
                WACN = "BEE00",
                ControlCapabilities = "Data,Voice,Registration",
                Flavor = "Phase 2",
                CallSigns = "WQCZ742 WQIU454",
                TimeStamp = new DateTime(2018, 6, 2, 13, 53, 30)
            };

            Assert.Equal(1, tower.SystemID);
            Assert.Equal(1031, tower.TowerNumber);
            Assert.Equal("T011F", tower.TowerNumberHex);
            Assert.Equal("LaSalle (LaSalle)", tower.Description);
            Assert.Equal("BEE00", tower.WACN);
            Assert.Equal("Data,Voice,Registration", tower.ControlCapabilities);
            Assert.Equal("Phase 2", tower.Flavor);
            Assert.Equal("WQCZ742 WQIU454", tower.CallSigns);
            Assert.Equal(new DateTime(2018, 6, 2, 13, 53, 30), tower.TimeStamp);
            Assert.True(tower.IsDirty);
        }
    }
}
