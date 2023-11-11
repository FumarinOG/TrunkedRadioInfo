using Xunit;

namespace ObjectLibrary.Tests.Core
{
    public class RadioHistoryTests
    {
        [Fact]
        public void PropertiesAssignedProperly()
        {
            var radioHistory = new RadioHistory
            {
                SystemID = 1,
                RadioID = 1917101,
                RadioIDKey = 42,
                Description = "ISP Dispatch (LaSalle) (17-A)"
            };

            Assert.Equal(1, radioHistory.SystemID);
            Assert.Equal(1917101, radioHistory.RadioID);
            Assert.Equal(42, radioHistory.RadioIDKey);
            Assert.Equal("ISP Dispatch (LaSalle) (17-A)", radioHistory.Description);
            Assert.True(radioHistory.IsDirty);
        }

        [Fact]
        public void ToStringWorks()
        {
            var radioHistory = new RadioHistory
            {
                SystemID = 1,
                RadioID = 1917101,
                Description = "ISP Dispatch (LaSalle) (17-A)"
            };

            Assert.Equal("System ID 1, Radio ID 1917101 (ISP Dispatch (LaSalle) (17-A))", radioHistory.ToString());
        }
    }
}
