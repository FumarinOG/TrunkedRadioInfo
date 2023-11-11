using Xunit;

namespace ObjectLibrary.Tests.Core
{
    public class TowerTableTests
    {
        [Fact]
        public void PropertiesAssignedProperly()
        {
            var towerTable = new TowerTable
            {
                SystemID = 1,
                TowerID = 42,
                TableID = 2,
                BaseFrequency = "851.01250",
                Spacing = "0.01250",
                InputOffset = "-45.00000",
                AssumedConfirmed = "Confirmed",
                Bandwidth = "0.01250",
                Slots = 2
            };

            Assert.Equal(1, towerTable.SystemID);
            Assert.Equal(42, towerTable.TowerID);
            Assert.Equal(2, towerTable.TableID);
            Assert.Equal("851.01250", towerTable.BaseFrequency);
            Assert.Equal("0.01250", towerTable.Spacing);
            Assert.Equal("-45.00000", towerTable.InputOffset);
            Assert.Equal("Confirmed", towerTable.AssumedConfirmed);
            Assert.Equal("0.01250", towerTable.Bandwidth);
            Assert.Equal(2, towerTable.Slots);
            Assert.True(towerTable.IsDirty);
        }

        [Fact]
        public void ToStringWorks()
        {
            var towerTable = new TowerTable
            {
                SystemID = 1,
                TowerID = 42,
                TableID = 2
            };

            Assert.Equal("System ID 1, Tower # 42, Table ID 2", towerTable.ToString());
        }
    }
}
