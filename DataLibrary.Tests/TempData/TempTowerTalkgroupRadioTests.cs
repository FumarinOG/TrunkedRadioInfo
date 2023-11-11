using DataLibrary.TempData;
using ObjectLibrary;
using System;
using Xunit;

namespace DataLibrary.Tests.TempData
{
    public class TempTowerTalkgroupRadioTests
    {
        private readonly TowerTalkgroupRadio _towerTalkgroupRadio = new TowerTalkgroupRadio()
        {
            SystemID = 1,
            TowerNumber = 1031,
            TalkgroupID = 4501,
            RadioID = 779555,
            Date = DateTime.Now,
            AffiliationCount = 1000,
            DeniedCount = 2000,
            VoiceGrantCount = 3000,
            EmergencyVoiceGrantCount = 4000,
            EncryptedVoiceGrantCount = 5000,
            DataCount = 6000,
            PrivateDataCount = 7000,
            AlertCount = 8000,
            FirstSeen = DateTime.Now.AddYears(-5),
            LastSeen = DateTime.Now
        };
        private readonly Guid _sessionID = Guid.NewGuid();

        [Fact]
        public void CopyFromCopiesProperly()
        {
            var tempTowerTalkgroupRadio = new TempTowerTalkgroupRadio();

            tempTowerTalkgroupRadio.CopyFrom(_sessionID, _towerTalkgroupRadio);

            Assert.Equal(_sessionID, tempTowerTalkgroupRadio.SessionID);
            Assert.Equal(_towerTalkgroupRadio.SystemID, tempTowerTalkgroupRadio.SystemID);
            Assert.Equal(_towerTalkgroupRadio.TowerNumber, tempTowerTalkgroupRadio.TowerID);
            Assert.Equal(_towerTalkgroupRadio.TalkgroupID, tempTowerTalkgroupRadio.TalkgroupID);
            Assert.Equal(_towerTalkgroupRadio.RadioID, tempTowerTalkgroupRadio.RadioID);
            Assert.Equal(_towerTalkgroupRadio.Date, tempTowerTalkgroupRadio.Date);
            Assert.Equal(_towerTalkgroupRadio.AffiliationCount, tempTowerTalkgroupRadio.AffiliationCount);
            Assert.Equal(_towerTalkgroupRadio.DeniedCount, tempTowerTalkgroupRadio.DeniedCount);
            Assert.Equal(_towerTalkgroupRadio.VoiceGrantCount, tempTowerTalkgroupRadio.VoiceGrantCount);
            Assert.Equal(_towerTalkgroupRadio.EmergencyVoiceGrantCount, tempTowerTalkgroupRadio.EmergencyVoiceGrantCount);
            Assert.Equal(_towerTalkgroupRadio.EncryptedVoiceGrantCount, tempTowerTalkgroupRadio.EncryptedVoiceGrantCount);
            Assert.Equal(_towerTalkgroupRadio.DataCount, tempTowerTalkgroupRadio.DataCount);
            Assert.Equal(_towerTalkgroupRadio.PrivateDataCount, tempTowerTalkgroupRadio.PrivateDataCount);
            Assert.Equal(_towerTalkgroupRadio.AlertCount, tempTowerTalkgroupRadio.AlertCount);
            Assert.Equal(_towerTalkgroupRadio.FirstSeen, tempTowerTalkgroupRadio.FirstSeen);
            Assert.Equal(_towerTalkgroupRadio.LastSeen, tempTowerTalkgroupRadio.LastSeen);
            Assert.Equal("TempTowerTalkgroupRadios", tempTowerTalkgroupRadio.TableName);
        }

        [Fact]
        public void ToStringWorks()
        {
            var tempTowerTalkgroupRadio = new TempTowerTalkgroupRadio();

            tempTowerTalkgroupRadio.CopyFrom(_sessionID, _towerTalkgroupRadio);

            Assert.Equal
                ($"Temp - System ID {_towerTalkgroupRadio.SystemID}, Tower # {_towerTalkgroupRadio.TowerNumber}, Talkgroup ID {_towerTalkgroupRadio.TalkgroupID}, Radio ID {_towerTalkgroupRadio.RadioID}",
                tempTowerTalkgroupRadio.ToString());
        }
    }
}
