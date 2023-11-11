using Xunit;

namespace ObjectLibrary.Tests.Core
{
    public class TalkgroupHistoryTests
    {
        [Fact]
        public void PropertiesAssignedProperly()
        {
            var talkgroupHistory = new TalkgroupHistory
            {
                SystemID = 1,
                TalkgroupID = 36421,
                TalkgroupIDKey = 42,
                Description = "Northwest Rescue Helo"
            };

            Assert.Equal(1, talkgroupHistory.SystemID);
            Assert.Equal(36421, talkgroupHistory.TalkgroupID);
            Assert.Equal(42, talkgroupHistory.TalkgroupIDKey);
            Assert.Equal("Northwest Rescue Helo", talkgroupHistory.Description);
            Assert.True(talkgroupHistory.IsDirty);
        }

        [Fact]
        public void ToStringWorks()
        {
            var talkgroupHistory = new TalkgroupHistory
            {
                SystemID = 1,
                TalkgroupID = 36421,
                Description = "Northwest Rescue Helo"
            };

            Assert.Equal("System ID 1, Talkgroup ID 36421 (Northwest Rescue Helo)", talkgroupHistory.ToString());
        }
    }
}
