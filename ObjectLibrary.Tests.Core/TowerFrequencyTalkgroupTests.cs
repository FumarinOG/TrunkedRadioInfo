using Xunit;

namespace ObjectLibrary.Tests.Core
{
    public class TowerFrequencyTalkgroupTests
    {
        [Fact]
        public void PropertiesAssignedProperly()
        {
            var towerFrequencyTalkgroup = new TowerFrequencyTalkgroup
            {
                SystemID = 1,
                TowerID = 42,
                Frequency = "851.42500",
                TalkgroupID = 9019
            };

            Assert.Equal(1, towerFrequencyTalkgroup.SystemID);
            Assert.Equal(42, towerFrequencyTalkgroup.TowerID);
            Assert.Equal("851.42500", towerFrequencyTalkgroup.Frequency);
            Assert.Equal(9019, towerFrequencyTalkgroup.TalkgroupID);
            Assert.True(towerFrequencyTalkgroup.IsDirty);
        }

        [Fact]
        public void ToStringWorks()
        {
            var towerFrequencyTalkgroup = new TowerFrequencyTalkgroup
            {
                SystemID = 1,
                TowerID = 42,
                Frequency = "851.42500",
                TalkgroupID = 9019
            };

            Assert.Equal("System ID 1, Tower # 42, Frequency 851.42500, Talkgroup ID 9019", towerFrequencyTalkgroup.ToString());
        }
    }
}
