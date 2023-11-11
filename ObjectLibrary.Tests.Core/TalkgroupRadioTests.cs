using Xunit;

namespace ObjectLibrary.Tests.Core
{
    public class TalkgroupRadioTests
    {
        [Fact]
        public void PropertiesAssignedProperly()
        {
            var talkgroupRadio = new TalkgroupRadio
            {
                SystemID = 1,
                TalkgroupID = 9019,
                RadioID = 1917101
            };

            Assert.Equal(1, talkgroupRadio.SystemID);
            Assert.Equal(9019, talkgroupRadio.TalkgroupID);
            Assert.Equal(1917101, talkgroupRadio.RadioID);
            Assert.True(talkgroupRadio.IsDirty);
        }

        [Fact]
        public void ToStringWorks()
        {
            var talkgroupRadio = new TalkgroupRadio
            {
                SystemID = 1,
                TalkgroupID = 9019,
                TalkgroupName = "ISP 17-A - LaSalle Primary",
                RadioID = 1917101,
                RadioName = "ISP Dispatch (LaSalle) (17-A)"
            };

            Assert.Equal("System ID 1, Talkgroup ID 9019 (ISP 17-A - LaSalle Primary), Radio ID 1917101 (ISP Dispatch (LaSalle) (17-A))", talkgroupRadio.ToString());
        }
    }
}
