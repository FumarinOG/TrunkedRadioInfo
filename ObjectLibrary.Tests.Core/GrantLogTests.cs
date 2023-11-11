using System;
using Xunit;

namespace ObjectLibrary.Tests.Core
{
    public class GrantLogTests
    {
        [Fact]
        public void PropertiesAssignedProperly()
        {
            var date = DateTime.Now;
            var grantLog = new GrantLog
            {
                TowerNumber = 1,
                TimeStamp = date,
                Type = "Type",
                Channel = "Channel",
                Frequency = "851.9500",
                TalkgroupID = 9000,
                TalkgroupDescription = "talkgroup description",
                RadioID = 19170101,
                RadioDescription = "radio description"
            };

            Assert.Equal(1, grantLog.TowerNumber);
            Assert.Equal(date, grantLog.TimeStamp);
            Assert.Equal("Type", grantLog.Type);
            Assert.Equal("Channel", grantLog.Channel);
            Assert.Equal("851.9500", grantLog.Frequency);
            Assert.Equal(9000, grantLog.TalkgroupID);
            Assert.Equal("talkgroup description", grantLog.TalkgroupDescription);
            Assert.Equal(19170101, grantLog.RadioID);
            Assert.Equal("radio description", grantLog.RadioDescription);
            Assert.True(grantLog.IsDirty);
        }

        [Fact]
        public void ToStringWorks()
        {
            var grantLog = new GrantLog
            {
                TalkgroupID = 20005,
                RadioID = 121648,
                Type = "Group"
            };

            Assert.Equal("Talkgroup ID 20005, Radio ID 121648, Type Group", grantLog.ToString());
        }
    }
}
