using DataLibrary.TempData;
using ObjectLibrary;
using System;
using Xunit;

namespace DataLibrary.Tests.TempData
{
    public class TempTalkgroupTests
    {
        private readonly Talkgroup _talkgroup = new Talkgroup()
        {
            SystemID = 1,
            TalkgroupID = 36531,
            Priority = 50,
            Description = "Superior Air Med",
            LastSeen = DateTime.Parse("07-15-2018 23:28"),
            LastSeenProgram = DateTime.Parse("07-13-2018 19:25"),
            LastSeenProgramUnix = 1531527947,
            FirstSeen = DateTime.Parse("04-01-2018 07:12"),
            FirstSeenProgram = DateTime.Parse("04-20-2018 18:35"),
            FirstSeenProgramUnix = 1524267332,
            FGColor = "#000000",
            BGColor = "#F2F3F4",
            EncryptionSeen = false,
            IgnoreEmergencySignal = true,
            HitCount = 325,
            HitCountProgram = 21,
            PhaseIISeen = false
        };
        private readonly Guid _sessionID = Guid.NewGuid();

        [Fact]
        public void CopyFromCopiesProperly()
        {
            var tempTalkgroup = new TempTalkgroup();

            tempTalkgroup.CopyFrom(_sessionID, _talkgroup);

            Assert.Equal(_sessionID, tempTalkgroup.SessionID);
            Assert.Equal(_talkgroup.SystemID, tempTalkgroup.SystemID);
            Assert.Equal(_talkgroup.TalkgroupID, tempTalkgroup.TalkgroupID);
            Assert.Equal(_talkgroup.Priority, tempTalkgroup.Priority);
            Assert.Equal(_talkgroup.Description, tempTalkgroup.Description);
            Assert.Equal(_talkgroup.LastSeen, tempTalkgroup.LastSeen);
            Assert.Equal(_talkgroup.LastSeenProgram, tempTalkgroup.LastSeenProgram);
            Assert.Equal(_talkgroup.LastSeenProgramUnix, tempTalkgroup.LastSeenProgramUnix);
            Assert.Equal(_talkgroup.FirstSeen, tempTalkgroup.FirstSeen);
            Assert.Equal(_talkgroup.FirstSeenProgram, tempTalkgroup.FirstSeenProgram);
            Assert.Equal(_talkgroup.FirstSeenProgramUnix, tempTalkgroup.FirstSeenProgramUnix);
            Assert.Equal(_talkgroup.FGColor, tempTalkgroup.FGColor);
            Assert.Equal(_talkgroup.BGColor, tempTalkgroup.BGColor);
            Assert.Equal(_talkgroup.EncryptionSeen, tempTalkgroup.EncryptionSeen);
            Assert.Equal(_talkgroup.IgnoreEmergencySignal, tempTalkgroup.IgnoreEmergencySignal);
            Assert.Equal(_talkgroup.HitCount, tempTalkgroup.HitCount);
            Assert.Equal(_talkgroup.HitCountProgram, tempTalkgroup.HitCountProgram);
            Assert.Equal(_talkgroup.PhaseIISeen, tempTalkgroup.PhaseIISeen);
            Assert.Equal("TempTalkgroups", tempTalkgroup.TableName);
        }

        [Fact]
        public void ToStringWorks()
        {
            var tempTalkgroup = new TempTalkgroup();

            tempTalkgroup.CopyFrom(_sessionID, _talkgroup);

            Assert.Equal($"Temp - System ID {_talkgroup.SystemID}, Talkgroup ID {_talkgroup.TalkgroupID} ({_talkgroup.Description})", tempTalkgroup.ToString());
        }
    }
}
