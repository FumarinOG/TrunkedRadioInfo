using Xunit;

namespace ObjectLibrary.Tests.Core
{
    public class TowerTalkgroupRadioTests
    {
        [Fact]
        public void PropertiesAssignedProperly()
        {
            var towerTalkgroupRadio = new TowerTalkgroupRadio
            {
                SystemID = 1,
                TowerNumber = 42,
                TalkgroupID = 9019,
                RadioID = 1917101
            };

            Assert.Equal(1, towerTalkgroupRadio.SystemID);
            Assert.Equal(42, towerTalkgroupRadio.TowerNumber);
            Assert.Equal(9019, towerTalkgroupRadio.TalkgroupID);
            Assert.Equal(1917101, towerTalkgroupRadio.RadioID);
            Assert.True(towerTalkgroupRadio.IsDirty);
        }

        [Fact]
        public void ToStringWorks()
        {
            var towerTalkgroupRadio = new TowerTalkgroupRadio
            {
                SystemID = 1,
                TowerNumber = 42,
                TalkgroupID = 9019,
                RadioID = 1917101
            };

            Assert.Equal("System ID 1, Tower # 42, Talkgroup ID 9019, Radio ID 1917101", towerTalkgroupRadio.ToString());
        }
    }
}
