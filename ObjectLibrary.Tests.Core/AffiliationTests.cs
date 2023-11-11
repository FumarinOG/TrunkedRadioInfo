using System;
using Xunit;

namespace ObjectLibrary.Tests.Core
{
    public class AffiliationTests
    {
        [Fact]
        public void PropertiesAssignedProperly()
        {
            var date = DateTime.Now;

            var affiliation = new Affiliation
            {
                SystemID = 1,
                TowerNumber = 1031,
                TimeStamp = date,
                Function = "function",
                TalkgroupID = 9019,
                TalkgroupDescription = "talkgroup description",
                RadioID = 15001,
                RadioDescription = "radio description"
            };

            Assert.Equal(1, affiliation.SystemID);
            Assert.Equal(1031, affiliation.TowerNumber);
            Assert.Equal(date, affiliation.TimeStamp);
            Assert.Equal("function", affiliation.Function);
            Assert.Equal(9019, affiliation.TalkgroupID);
            Assert.Equal("talkgroup description", affiliation.TalkgroupDescription);
            Assert.Equal(15001, affiliation.RadioID);
            Assert.Equal("radio description", affiliation.RadioDescription);
            Assert.True(affiliation.IsDirty);
        }

        [Fact]
        public void ToStringWorks()
        {
            var affiliation = new Affiliation
            {
                TowerNumber = 1031,
                TalkgroupID = 9019,
                RadioID = 1104391,
                Function = "Affiliate"
            };

            Assert.Equal("Tower Number 1031, Talkgroup ID 9019, Radio ID 1104391, Function Affiliate", affiliation.ToString());
        }
    }
}
