using System;
using Xunit;

namespace ObjectLibrary.Tests.Core
{
    public class PatchLogTests
    {
        [Fact]
        public void PropertiesAssignedProperly()
        {
            var date = DateTime.Now;
            var patchLog = new PatchLog
            {
                SystemID = 1,
                TowerNumber = 1031,
                TimeStamp = date,
                Description = "description"
            };

            Assert.Equal(1, patchLog.SystemID);
            Assert.Equal(1031, patchLog.TowerNumber);
            Assert.Equal(date, patchLog.TimeStamp);
            Assert.Equal("description", patchLog.Description);
            Assert.True(patchLog.IsDirty);
        }

        [Fact]
        public void ToStringWorks()
        {
            var patchLog = new PatchLog
            {
                TimeStamp = new DateTime(2018, 6, 11, 23, 54, 0),
                Description = "Added Patch:   9019 (ISP 17-A - LaSalle Primary) --> 9016 (ISP 07-B - Rock Island Alternate)"
            };

            Assert.Equal("Timestamp 06-11-2018 23:54, Description Added Patch:   9019 (ISP 17-A - LaSalle Primary) --> 9016 (ISP 07-B - Rock Island Alternate)",
                patchLog.ToString());
        }
    }
}
