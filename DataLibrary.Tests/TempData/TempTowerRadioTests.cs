using DataLibrary.TempData;
using ObjectLibrary;
using System;
using Xunit;

namespace DataLibrary.Tests.TempData
{
    public class TempTowerRadioTests
    {
        private readonly TowerRadio _towerRadio = new TowerRadio()
        {
            SystemID = 1,
            TowerNumber = 1031,
            RadioID = 447004,
            Date = DateTime.Now,
            AffiliationCount = 100,
            DeniedCount = 200,
            VoiceGrantCount = 300,
            EmergencyVoiceGrantCount = 400,
            EncryptedVoiceGrantCount = 500,
            DataCount = 600,
            PrivateDataCount = 700,
            AlertCount = 800,
            FirstSeen = DateTime.Now.AddYears(-1),
            LastSeen = DateTime.Now
        };
        private readonly Guid _sessionID = Guid.NewGuid();

        [Fact]
        public void CopyFromCopiesProperly()
        {
            var tempTowerRadio = new TempTowerRadio();

            tempTowerRadio.CopyFrom(_sessionID, _towerRadio);

            Assert.Equal(_sessionID, tempTowerRadio.SessionID);
            Assert.Equal(_towerRadio.SystemID, tempTowerRadio.SystemID);
            Assert.Equal(_towerRadio.TowerNumber, tempTowerRadio.TowerID);
            Assert.Equal(_towerRadio.RadioID, tempTowerRadio.RadioID);
            Assert.Equal(_towerRadio.Date, tempTowerRadio.Date);
            Assert.Equal(_towerRadio.AffiliationCount, tempTowerRadio.AffiliationCount);
            Assert.Equal(_towerRadio.DeniedCount, tempTowerRadio.DeniedCount);
            Assert.Equal(_towerRadio.VoiceGrantCount, tempTowerRadio.VoiceGrantCount);
            Assert.Equal(_towerRadio.EmergencyVoiceGrantCount, tempTowerRadio.EmergencyVoiceGrantCount);
            Assert.Equal(_towerRadio.EncryptedVoiceGrantCount, tempTowerRadio.EncryptedVoiceGrantCount);
            Assert.Equal(_towerRadio.DataCount, tempTowerRadio.DataCount);
            Assert.Equal(_towerRadio.PrivateDataCount, tempTowerRadio.PrivateDataCount);
            Assert.Equal(_towerRadio.AlertCount, tempTowerRadio.AlertCount);
            Assert.Equal(_towerRadio.FirstSeen, tempTowerRadio.FirstSeen);
            Assert.Equal(_towerRadio.LastSeen, tempTowerRadio.LastSeen);
            Assert.Equal("TempTowerRadios", tempTowerRadio.TableName);
        }

        [Fact]
        public void ToStringWorks()
        {
            var tempTowerRadio = new TempTowerRadio();

            tempTowerRadio.CopyFrom(_sessionID, _towerRadio);

            Assert.Equal($"Temp - System ID {_towerRadio.SystemID}, Tower # {_towerRadio.TowerNumber}, Radio ID {_towerRadio.RadioID}",
                tempTowerRadio.ToString());
        }
    }
}
