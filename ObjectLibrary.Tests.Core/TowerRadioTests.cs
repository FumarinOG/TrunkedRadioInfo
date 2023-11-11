using Xunit;

namespace ObjectLibrary.Tests.Core
{
    public class TowerRadioTests
    {
        [Fact]
        public void PropertiesAssignedProperly()
        {
            var towerRadio = new TowerRadio
            {
                SystemID = 1,
                TowerNumber = 42,
                RadioID = 1917101
            };

            Assert.Equal(1, towerRadio.SystemID);
            Assert.Equal(42, towerRadio.TowerNumber);
            Assert.Equal(1917101, towerRadio.RadioID);
            Assert.True(towerRadio.IsDirty);
        }

        [Fact]
        public void ToStringWorks()
        {
            var towerRadio = new TowerRadio
            {
                SystemID = 1,
                TowerNumber = 42,
                RadioID = 1917101
            };

            Assert.Equal("System ID 1, Tower # 42, Radio ID 1917101", towerRadio.ToString());
        }
    }
}
