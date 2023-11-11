using DataLibrary.TempData;
using ObjectLibrary;
using System;
using Xunit;

namespace DataLibrary.Tests.TempData
{
    public class TempTalkgroupRadioTests
    {
        private readonly TalkgroupRadio _talkgroupRadio = new TalkgroupRadio()
        {
            SystemID = 1,
            TalkgroupID = 9019,
            RadioID = 1902101,
            Date = DateTime.Now,
            AffiliationCount = 1000,
            DeniedCount = 5000,
            VoiceGrantCount = 10000,
            EmergencyVoiceGrantCount = 15000,
            EncryptedVoiceGrantCount = 100000,
            DataCount = 150000,
            PrivateDataCount = 1000000,
            AlertCount = 1500000,
            FirstSeen = DateTime.Now.AddYears(-10),
            LastSeen = DateTime.Now
        };
        private readonly Guid _sessionID = Guid.NewGuid();

        [Fact]
        public void CopyFromCopiesProperly()
        {
            var tempTalkgroupRadio = new TempTalkgroupRadio();

            tempTalkgroupRadio.CopyFrom(_sessionID, _talkgroupRadio);

            Assert.Equal(_sessionID, tempTalkgroupRadio.SessionID);
            Assert.Equal(_talkgroupRadio.SystemID, tempTalkgroupRadio.SystemID);
            Assert.Equal(_talkgroupRadio.TalkgroupID, tempTalkgroupRadio.TalkgroupID);
            Assert.Equal(_talkgroupRadio.RadioID, tempTalkgroupRadio.RadioID);
            Assert.Equal(_talkgroupRadio.Date, tempTalkgroupRadio.Date);
            Assert.Equal(_talkgroupRadio.AffiliationCount, tempTalkgroupRadio.AffiliationCount);
            Assert.Equal(_talkgroupRadio.DeniedCount, tempTalkgroupRadio.DeniedCount);
            Assert.Equal(_talkgroupRadio.VoiceGrantCount, tempTalkgroupRadio.VoiceGrantCount);
            Assert.Equal(_talkgroupRadio.EmergencyVoiceGrantCount, tempTalkgroupRadio.EmergencyVoiceGrantCount);
            Assert.Equal(_talkgroupRadio.EncryptedVoiceGrantCount, tempTalkgroupRadio.EncryptedVoiceGrantCount);
            Assert.Equal(_talkgroupRadio.DataCount, tempTalkgroupRadio.DataCount);
            Assert.Equal(_talkgroupRadio.PrivateDataCount, tempTalkgroupRadio.PrivateDataCount);
            Assert.Equal(_talkgroupRadio.AlertCount, tempTalkgroupRadio.AlertCount);
            Assert.Equal(_talkgroupRadio.FirstSeen, tempTalkgroupRadio.FirstSeen);
            Assert.Equal(_talkgroupRadio.LastSeen, tempTalkgroupRadio.LastSeen);
            Assert.Equal("TempTalkgroupRadios", tempTalkgroupRadio.TableName);
        }

        [Fact]
        public void ToStringWorks()
        {
            var tempTalkgroupRadio = new TempTalkgroupRadio();

            tempTalkgroupRadio.CopyFrom(_sessionID, _talkgroupRadio);

            Assert.Equal($"Temp - System ID {_talkgroupRadio.SystemID}, Talkgroup ID {_talkgroupRadio.TalkgroupID}, Radio ID {_talkgroupRadio.RadioID}",
                tempTalkgroupRadio.ToString());
        }
    }
}
