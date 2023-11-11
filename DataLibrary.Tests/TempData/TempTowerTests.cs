using DataLibrary.TempData;
using ObjectLibrary;
using System;
using Xunit;

namespace DataLibrary.Tests.TempData
{
    public class TempTowerTests
    {
        private readonly Tower _tower = new Tower()
        {
            SystemID = 1,
            TowerNumber = 5017,
            TowerNumberHex = "T0511",
            Description = "26th St & California St (Cook)",
            HitCount = 59421,
            WACN = "BEE00",
            ControlCapabilities = "Data,Voice,Registration",
            Flavor = "Phase 2",
            CallSigns = "WPTZ798",
            TimeStamp = DateTime.Parse("07-25-2018 08:25:25"),
            FirstSeen = DateTime.Now.AddYears(-1),
            LastSeen = DateTime.Now
        };
        private readonly Guid _sessionID = Guid.NewGuid();

        [Fact]
        public void CopyFromCopiesProperly()
        {
            var tempTower = new TempTower();

            tempTower.CopyFrom(_sessionID, _tower);

            Assert.Equal(_tower.SystemID, tempTower.SystemID);
            Assert.Equal(_tower.TowerNumber, tempTower.TowerNumber);
            Assert.Equal(_tower.TowerNumberHex, tempTower.TowerNumberHex);
            Assert.Equal(_tower.Description, tempTower.Description);
            Assert.Equal(_tower.HitCount, tempTower.HitCount);
            Assert.Equal(_tower.WACN, tempTower.WACN);
            Assert.Equal(_tower.ControlCapabilities, tempTower.ControlCapabilities);
            Assert.Equal(_tower.Flavor, tempTower.Flavor);
            Assert.Equal(_tower.CallSigns, tempTower.CallSigns);
            Assert.Equal(_tower.TimeStamp, tempTower.TimeStamp);
            Assert.Equal(_tower.FirstSeen, tempTower.FirstSeen);
            Assert.Equal(_tower.LastSeen, tempTower.LastSeen);
            Assert.Equal("TempTowers", tempTower.TableName);
        }

        [Fact]
        public void ToStringWorks()
        {
            var tempTower = new TempTower();

            tempTower.CopyFrom(_sessionID, _tower);

            Assert.Equal($"Temp - System ID {_tower.SystemID}, Tower # {_tower.TowerNumber} ({_tower.Description})", tempTower.ToString());
        }
    }
}
