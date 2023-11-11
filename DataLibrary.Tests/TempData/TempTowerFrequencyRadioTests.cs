using DataLibrary.TempData;
using ObjectLibrary;
using System;
using Xunit;

namespace DataLibrary.Tests.TempData
{
    public class TempTowerFrequencyRadioTests
    {
        private readonly TowerFrequencyRadio _towerFrequencyRadio = new TowerFrequencyRadio()
        {
            SystemID = 1,
            TowerID = 1031,
            Frequency = "853.31250",
            RadioID = 1904385,
            Date = DateTime.Now,
            AffiliationCount = 250,
            DeniedCount = 500,
            VoiceGrantCount = 750,
            EmergencyVoiceGrantCount = 1000,
            EncryptedVoiceGrantCount = 1250,
            DataCount = 1500,
            PrivateDataCount = 1750,
            AlertCount = 2000,
            FirstSeen = DateTime.Now.AddYears(-1),
            LastSeen = DateTime.Now
        };
        private readonly Guid _sessionID = Guid.NewGuid();

        [Fact]
        public void CopyFromCopiesProperly()
        {
            var tempTowerFrequencyRadio = new TempTowerFrequencyRadio();

            tempTowerFrequencyRadio.CopyFrom(_sessionID, _towerFrequencyRadio);

            Assert.Equal(_sessionID, tempTowerFrequencyRadio.SessionID);
            Assert.Equal(_towerFrequencyRadio.SystemID, tempTowerFrequencyRadio.SystemID);
            Assert.Equal(_towerFrequencyRadio.TowerID, tempTowerFrequencyRadio.TowerID);
            Assert.Equal(_towerFrequencyRadio.Frequency, tempTowerFrequencyRadio.Frequency);
            Assert.Equal(_towerFrequencyRadio.RadioID, tempTowerFrequencyRadio.RadioID);
            Assert.Equal(_towerFrequencyRadio.Date, tempTowerFrequencyRadio.Date);
            Assert.Equal(_towerFrequencyRadio.AffiliationCount, tempTowerFrequencyRadio.AffiliationCount);
            Assert.Equal(_towerFrequencyRadio.DeniedCount, tempTowerFrequencyRadio.DeniedCount);
            Assert.Equal(_towerFrequencyRadio.VoiceGrantCount, tempTowerFrequencyRadio.VoiceGrantCount);
            Assert.Equal(_towerFrequencyRadio.EmergencyVoiceGrantCount, tempTowerFrequencyRadio.EmergencyVoiceGrantCount);
            Assert.Equal(_towerFrequencyRadio.EncryptedVoiceGrantCount, tempTowerFrequencyRadio.EncryptedVoiceGrantCount);
            Assert.Equal(_towerFrequencyRadio.DataCount, tempTowerFrequencyRadio.DataCount);
            Assert.Equal(_towerFrequencyRadio.PrivateDataCount, tempTowerFrequencyRadio.PrivateDataCount);
            Assert.Equal(_towerFrequencyRadio.AlertCount, tempTowerFrequencyRadio.AlertCount);
            Assert.Equal(_towerFrequencyRadio.FirstSeen, tempTowerFrequencyRadio.FirstSeen);
            Assert.Equal(_towerFrequencyRadio.LastSeen, tempTowerFrequencyRadio.LastSeen);
            Assert.Equal("TempTowerFrequencyRadios", tempTowerFrequencyRadio.TableName);
        }

        [Fact]
        public void ToStringWorks()
        {
            var tempTowerFrequencyRadio = new TempTowerFrequencyRadio();

            tempTowerFrequencyRadio.CopyFrom(_sessionID, _towerFrequencyRadio);

            Assert.Equal($"Temp - Tower {_towerFrequencyRadio.TowerID}, Frequency {_towerFrequencyRadio.Frequency}, Radio {_towerFrequencyRadio.RadioID}",
                tempTowerFrequencyRadio.ToString());
        }
    }
}
