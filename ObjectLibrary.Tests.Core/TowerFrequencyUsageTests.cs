using System;
using Xunit;

namespace ObjectLibrary.Tests.Core
{
    public class TowerFrequencyUsageTests
    {
        [Fact]
        public void PropertiesAssignedProperly()
        {
            var towerFrequency = new TowerFrequency
            {
                SystemID = 1,
                TowerID = 5,
                Channel = "00-0067",
                Frequency = "851.42500"
            };

            Assert.Equal(1, towerFrequency.SystemID);
            Assert.Equal(5, towerFrequency.TowerID);
            Assert.Equal("00-0067", towerFrequency.Channel);
            Assert.Equal("851.42500", towerFrequency.Frequency);
            Assert.True(towerFrequency.IsDirty);
        }

        [Fact]
        public void ToStringWorks()
        {
            var date = DateTime.Now;
            var towerFrequencyUsage = new TowerFrequencyUsage
            {
                SystemID = 1,
                TowerID = 5,
                Channel = "00-0067",
                Frequency = "851.42500",
                Date = date,
                AffiliationCount = 1,
                VoiceGrantCount = 10,
                EmergencyVoiceGrantCount = 20,
                EncryptedVoiceGrantCount = 30,
                DataCount = 40,
                PrivateDataCount = 50
            };

            Assert.Equal($"Frequency 851.42500, Date {date:MM-dd-yyyy}, Affiliations 1, VoiceGrants 30, Encrypted 30, Data 90", towerFrequencyUsage.ToString());
        }
    }
}
