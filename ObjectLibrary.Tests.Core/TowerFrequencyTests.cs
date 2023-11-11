using System;
using Xunit;

namespace ObjectLibrary.Tests.Core
{
    public class TowerFrequencyTests
    {
        [Fact]
        public void PropertiesAssignedProperly()
        {
            var towerFrequency = new TowerFrequency
            {
                SystemID = 1,
                TowerID = 5,
                Channel = "00-0067",
                Usage = "vdi",
                Frequency = "851.42500",
                InputChannel = "00-0067",
                InputFrequency = "806.42500",
                InputExplicit = 0
            };

            Assert.Equal(1, towerFrequency.SystemID);
            Assert.Equal(5, towerFrequency.TowerID);
            Assert.Equal("00-0067", towerFrequency.Channel);
            Assert.Equal("vdi", towerFrequency.Usage);
            Assert.Equal("851.42500", towerFrequency.Frequency);
            Assert.Equal("00-0067", towerFrequency.InputChannel);
            Assert.Equal("806.42500", towerFrequency.InputFrequency);
            Assert.Equal(0, towerFrequency.InputExplicit);
            Assert.True(towerFrequency.IsDirty);
        }

        [Fact]
        public void ToStringWorks()
        {
            var towerFrequency = new TowerFrequency
            {
                SystemID = 1,
                TowerID = 5,
                Frequency = "851.42500"
            };

            Assert.Equal("System ID 1, Tower # 5, Frequency 851.42500", towerFrequency.ToString());
        }

        [Fact]
        public void EqualsReturnsTrueForMatchingObjects()
        {
            var firstSeen = DateTime.Now.AddYears(-1);
            var lastSeen = DateTime.Now;
            var towerFrequency1 = new TowerFrequency
            {
                SystemID = 1,
                TowerID = 5,
                Channel = "00-0067",
                Usage = "vdi",
                Frequency = "851.42500",
                InputChannel = "00-0067",
                InputFrequency = "806.42500",
                InputExplicit = 0,
                FirstSeen = firstSeen,
                LastSeen = lastSeen
            };
            var towerFrequency2 = new TowerFrequency
            {
                SystemID = 1,
                TowerID = 5,
                Channel = "00-0067",
                Usage = "vdi",
                Frequency = "851.42500",
                InputChannel = "00-0067",
                InputFrequency = "806.42500",
                InputExplicit = 0,
                FirstSeen = firstSeen,
                LastSeen = lastSeen
            };

            Assert.True(towerFrequency1.Equals(towerFrequency2));
            Assert.True(towerFrequency1.GetHashCode() == towerFrequency2.GetHashCode());
        }

        [Fact]
        public void EqualsReturnsFalseForNonMatchingObjects()
        {
            var firstSeen = DateTime.Now.AddYears(-1);
            var lastSeen = DateTime.Now;
            var towerFrequency1 = new TowerFrequency
            {
                SystemID = 1,
                TowerID = 5,
                Channel = "00-0067",
                Usage = "vdi",
                Frequency = "851.42500",
                InputChannel = "00-0067",
                InputFrequency = "806.42500",
                InputExplicit = 0,
                FirstSeen = firstSeen,
                LastSeen = lastSeen
            };
            var towerFrequency2 = new TowerFrequency
            {
                SystemID = 1,
                TowerID = 5,
                Channel = "00-0067",
                Usage = "vdi",
                Frequency = "851.95000",
                InputChannel = "00-0067",
                InputFrequency = "806.42500",
                InputExplicit = 0,
                FirstSeen = firstSeen,
                LastSeen = lastSeen
            };

            Assert.False(towerFrequency1.Equals(towerFrequency2));
            Assert.False(towerFrequency1.GetHashCode() == towerFrequency2.GetHashCode());
        }

        [Fact]
        public void EqualsReturnsFalseForDifferingObjectTypes()
        {
            var firstSeen = DateTime.Now.AddYears(-1);
            var lastSeen = DateTime.Now;
            var towerFrequency1 = new TowerFrequency
            {
                SystemID = 1,
                TowerID = 5,
                Channel = "00-0067",
                Usage = "vdi",
                Frequency = "851.42500",
                InputChannel = "00-0067",
                InputFrequency = "806.42500",
                InputExplicit = 0,
                FirstSeen = firstSeen,
                LastSeen = lastSeen
            };
            var towerFrequency2 = towerFrequency1.ToString();

            Assert.False(towerFrequency1.Equals(towerFrequency2));
            Assert.False(towerFrequency1.GetHashCode() == towerFrequency2.GetHashCode());
        }
    }
}
