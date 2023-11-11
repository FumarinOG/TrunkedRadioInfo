using Xunit;

namespace ObjectLibrary.Tests.Core
{
    public class TowerTalkgroupTests
    {
        [Fact]
        public void PropertiesAssignedProperly()
        {
            var towerTalkgroup = new TowerTalkgroup
            {
                SystemID = 1,
                TowerNumber = 42,
                TalkgroupID = 9019
            };

            Assert.Equal(1, towerTalkgroup.SystemID);
            Assert.Equal(42, towerTalkgroup.TowerNumber);
            Assert.Equal(9019, towerTalkgroup.TalkgroupID);
            Assert.True(towerTalkgroup.IsDirty);
        }

        [Fact]
        public void ToStringWorks()
        {
            var towerTalkgroup = new TowerTalkgroup
            {
                SystemID = 1,
                TowerNumber = 42,
                TalkgroupID = 9019
            };

            Assert.Equal("System ID 1, Tower # 42, Talkgroup ID 9019", towerTalkgroup.ToString());
        }
    }
}
