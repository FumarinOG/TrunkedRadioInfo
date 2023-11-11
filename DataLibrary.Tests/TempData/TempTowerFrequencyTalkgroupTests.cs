using DataLibrary.TempData;
using ObjectLibrary;
using System;
using Xunit;

namespace DataLibrary.Tests.TempData
{
    public class TempTowerFrequencyTalkgroupTests
    {
        private readonly TowerFrequencyTalkgroup _towerFrequencyTalkgroup = new TowerFrequencyTalkgroup()
        {
            SystemID = 1,
            TowerID = 1031,
            Frequency = "855.1625",
            TalkgroupID = 9015,
            Date = DateTime.Now,
            AffiliationCount = 1,
            DeniedCount = 10,
            VoiceGrantCount = 100,
            EmergencyVoiceGrantCount = 1000,
            EncryptedVoiceGrantCount = 10000,
            DataCount = 100000,
            PrivateDataCount = 10000000,
            AlertCount = 100000000,
            FirstSeen = DateTime.Now.AddYears(-5),
            LastSeen = DateTime.Now
        };
        private readonly Guid _sessionID = Guid.NewGuid();

        [Fact]
        public void CopyFromCopiesProperly()
        {
            var tempTowerFrequencyTalkgroup = new TempTowerFrequencyTalkgroup();

            tempTowerFrequencyTalkgroup.CopyFrom(_sessionID, _towerFrequencyTalkgroup);

            Assert.Equal(_sessionID, tempTowerFrequencyTalkgroup.SessionID);
            Assert.Equal(_towerFrequencyTalkgroup.SystemID, tempTowerFrequencyTalkgroup.SystemID);
            Assert.Equal(_towerFrequencyTalkgroup.TowerID, tempTowerFrequencyTalkgroup.TowerID);
            Assert.Equal(_towerFrequencyTalkgroup.Frequency, tempTowerFrequencyTalkgroup.Frequency);
            Assert.Equal(_towerFrequencyTalkgroup.TalkgroupID, tempTowerFrequencyTalkgroup.TalkgroupID);
            Assert.Equal(_towerFrequencyTalkgroup.Date, tempTowerFrequencyTalkgroup.Date);
            Assert.Equal(_towerFrequencyTalkgroup.AffiliationCount, tempTowerFrequencyTalkgroup.AffiliationCount);
            Assert.Equal(_towerFrequencyTalkgroup.DeniedCount, tempTowerFrequencyTalkgroup.DeniedCount);
            Assert.Equal(_towerFrequencyTalkgroup.VoiceGrantCount, tempTowerFrequencyTalkgroup.VoiceGrantCount);
            Assert.Equal(_towerFrequencyTalkgroup.EmergencyVoiceGrantCount, tempTowerFrequencyTalkgroup.EmergencyVoiceGrantCount);
            Assert.Equal(_towerFrequencyTalkgroup.EncryptedVoiceGrantCount, tempTowerFrequencyTalkgroup.EncryptedVoiceGrantCount);
            Assert.Equal(_towerFrequencyTalkgroup.DataCount, tempTowerFrequencyTalkgroup.DataCount);
            Assert.Equal(_towerFrequencyTalkgroup.PrivateDataCount, tempTowerFrequencyTalkgroup.PrivateDataCount);
            Assert.Equal(_towerFrequencyTalkgroup.AlertCount, tempTowerFrequencyTalkgroup.AlertCount);
            Assert.Equal(_towerFrequencyTalkgroup.FirstSeen, tempTowerFrequencyTalkgroup.FirstSeen);
            Assert.Equal(_towerFrequencyTalkgroup.LastSeen, tempTowerFrequencyTalkgroup.LastSeen);
            Assert.Equal("TempTowerFrequencyTalkgroups", tempTowerFrequencyTalkgroup.TableName);
        }

        [Fact]
        public void ToStringWorks()
        {
            var tempTowerFrequencyTalkgroup = new TempTowerFrequencyTalkgroup();

            tempTowerFrequencyTalkgroup.CopyFrom(_sessionID, _towerFrequencyTalkgroup);

            Assert.Equal(
                $"Temp - Tower {_towerFrequencyTalkgroup.TowerID}, Frequency {_towerFrequencyTalkgroup.Frequency}, Talkgroup {_towerFrequencyTalkgroup.TalkgroupID}",
                tempTowerFrequencyTalkgroup.ToString());
        }
    }
}
