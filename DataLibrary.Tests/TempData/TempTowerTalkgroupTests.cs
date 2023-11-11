using DataLibrary.TempData;
using ObjectLibrary;
using System;
using Xunit;

namespace DataLibrary.Tests.TempData
{
    public class TempTowerTalkgroupTests
    {
        private readonly TowerTalkgroup _towerTalkgroup = new TowerTalkgroup()
        {
            SystemID = 1,
            TowerNumber = 1031,
            TalkgroupID = 4501,
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
            var tempTowerTalkgroup = new TempTowerTalkgroup();

            tempTowerTalkgroup.CopyFrom(_sessionID, _towerTalkgroup);

            Assert.Equal(_sessionID, tempTowerTalkgroup.SessionID);
            Assert.Equal(_towerTalkgroup.SystemID, tempTowerTalkgroup.SystemID);
            Assert.Equal(_towerTalkgroup.TowerNumber, tempTowerTalkgroup.TowerID);
            Assert.Equal(_towerTalkgroup.TalkgroupID, tempTowerTalkgroup.TalkgroupID);
            Assert.Equal(_towerTalkgroup.Date, tempTowerTalkgroup.Date);
            Assert.Equal(_towerTalkgroup.AffiliationCount, tempTowerTalkgroup.AffiliationCount);
            Assert.Equal(_towerTalkgroup.DeniedCount, tempTowerTalkgroup.DeniedCount);
            Assert.Equal(_towerTalkgroup.VoiceGrantCount, tempTowerTalkgroup.VoiceGrantCount);
            Assert.Equal(_towerTalkgroup.EmergencyVoiceGrantCount, tempTowerTalkgroup.EmergencyVoiceGrantCount);
            Assert.Equal(_towerTalkgroup.EncryptedVoiceGrantCount, tempTowerTalkgroup.EncryptedVoiceGrantCount);
            Assert.Equal(_towerTalkgroup.DataCount, tempTowerTalkgroup.DataCount);
            Assert.Equal(_towerTalkgroup.PrivateDataCount, tempTowerTalkgroup.PrivateDataCount);
            Assert.Equal(_towerTalkgroup.AlertCount, tempTowerTalkgroup.AlertCount);
            Assert.Equal(_towerTalkgroup.FirstSeen, tempTowerTalkgroup.FirstSeen);
            Assert.Equal(_towerTalkgroup.LastSeen, tempTowerTalkgroup.LastSeen);
            Assert.Equal("TempTowerTalkgroups", tempTowerTalkgroup.TableName);
        }

        [Fact]
        public void ToStringWorks()
        {
            var tempTowerTalkgroup = new TempTowerTalkgroup();

            tempTowerTalkgroup.CopyFrom(_sessionID, _towerTalkgroup);

            Assert.Equal($"Temp - System ID {_towerTalkgroup.SystemID}, Tower # {_towerTalkgroup.TowerNumber}, Talkgroup ID {_towerTalkgroup.TalkgroupID}",
                tempTowerTalkgroup.ToString());
        }
    }
}
