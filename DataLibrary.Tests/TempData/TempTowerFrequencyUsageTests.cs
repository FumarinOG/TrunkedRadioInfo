using DataLibrary.TempData;
using ObjectLibrary;
using System;
using Xunit;

namespace DataLibrary.Tests.TempData
{
    public class TempTowerFrequencyUsageTests
    {
        private readonly TowerFrequencyUsage _towerFrequencyUsage = new TowerFrequencyUsage()
        {
            SystemID = 1,
            TowerID = 1031,
            Channel = "851.95000",
            Date = DateTime.Now,
            AffiliationCount = 1,
            DeniedCount = 2,
            VoiceGrantCount = 3,
            EmergencyVoiceGrantCount = 4,
            EncryptedVoiceGrantCount = 5,
            DataCount = 6,
            PrivateDataCount = 7,
            CWIDCount = 8,
            AlertCount = 9,
            FirstSeen = DateTime.Now.AddYears(-11),
            LastSeen = DateTime.Now
        };
        private Guid _sessionID = Guid.NewGuid();

        [Fact]
        public void CopyFromCopiesProperly()
        {
            var tempTowerFrequencyUsage = new TempTowerFrequencyUsage();

            tempTowerFrequencyUsage.CopyFrom(_sessionID, _towerFrequencyUsage);

            Assert.Equal(_sessionID, tempTowerFrequencyUsage.SessionID);
            Assert.Equal(_towerFrequencyUsage.SystemID, tempTowerFrequencyUsage.SystemID);
            Assert.Equal(_towerFrequencyUsage.TowerID, tempTowerFrequencyUsage.TowerID);
            Assert.Equal(_towerFrequencyUsage.Channel, tempTowerFrequencyUsage.Channel);
            Assert.Equal(_towerFrequencyUsage.Date, tempTowerFrequencyUsage.Date);
            Assert.Equal(_towerFrequencyUsage.AffiliationCount, tempTowerFrequencyUsage.AffiliationCount);
            Assert.Equal(_towerFrequencyUsage.DeniedCount, tempTowerFrequencyUsage.DeniedCount);
            Assert.Equal(_towerFrequencyUsage.VoiceGrantCount, tempTowerFrequencyUsage.VoiceGrantCount);
            Assert.Equal(_towerFrequencyUsage.EmergencyVoiceGrantCount, tempTowerFrequencyUsage.EmergencyVoiceGrantCount);
            Assert.Equal(_towerFrequencyUsage.EncryptedVoiceGrantCount, tempTowerFrequencyUsage.EncryptedVoiceGrantCount);
            Assert.Equal(_towerFrequencyUsage.DataCount, tempTowerFrequencyUsage.DataCount);
            Assert.Equal(_towerFrequencyUsage.PrivateDataCount, tempTowerFrequencyUsage.PrivateDataCount);
            Assert.Equal(_towerFrequencyUsage.CWIDCount, tempTowerFrequencyUsage.CWIDCount);
            Assert.Equal(_towerFrequencyUsage.AlertCount, tempTowerFrequencyUsage.AlertCount);
            Assert.Equal(_towerFrequencyUsage.FirstSeen, tempTowerFrequencyUsage.FirstSeen);
            Assert.Equal(_towerFrequencyUsage.LastSeen, tempTowerFrequencyUsage.LastSeen);
            Assert.Equal("TempTowerFrequencyUsage", tempTowerFrequencyUsage.TableName);
        }

        [Fact]
        public void ToStringWorks()
        {
            var tempTowerFrequencyUsage = new TempTowerFrequencyUsage();

            tempTowerFrequencyUsage.CopyFrom(_sessionID, _towerFrequencyUsage);

            Assert.Equal(
                $"Temp - System {_towerFrequencyUsage.SystemID}, Tower {_towerFrequencyUsage.TowerID}, Frequency {_towerFrequencyUsage.Frequency}, Date {_towerFrequencyUsage.Date:MM-dd-yyyy}",
                tempTowerFrequencyUsage.ToString());
        }
    }
}
