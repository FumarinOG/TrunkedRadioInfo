using DataLibrary.TempData;
using ObjectLibrary;
using System;
using Xunit;

namespace DataLibrary.Tests.TempData
{
    public class TempTalkgroupHistoryTests
    {
        private readonly TalkgroupHistory _talkgroupHistory = new TalkgroupHistory()
        {
            SystemID = 1,
            TalkgroupID = 33813,
            Description = "IDoT D1 I-55 Dispatch",
            FirstSeen = DateTime.Now.AddYears(-1),
            LastSeen = DateTime.Now
        };
        private readonly Guid _sessionID = Guid.NewGuid();

        [Fact]
        public void CopyFromCopiesProperly()
        {
            var tempTalkgroupHistory = new TempTalkgroupHistory();

            tempTalkgroupHistory.CopyFrom(_sessionID, _talkgroupHistory);

            Assert.Equal(_sessionID, tempTalkgroupHistory.SessionID);
            Assert.Equal(_talkgroupHistory.SystemID, tempTalkgroupHistory.SystemID);
            Assert.Equal(_talkgroupHistory.TalkgroupID, tempTalkgroupHistory.TalkgroupID);
            Assert.Equal(_talkgroupHistory.Description, tempTalkgroupHistory.Description);
            Assert.Equal(_talkgroupHistory.FirstSeen, tempTalkgroupHistory.FirstSeen);
            Assert.Equal(_talkgroupHistory.LastSeen, tempTalkgroupHistory.LastSeen);
            Assert.Equal("TempTalkgroupHistory", tempTalkgroupHistory.TableName);
        }

        [Fact]
        public void ToStringWorks()
        {
            var tempTalkgroupHistory = new TempTalkgroupHistory();

            tempTalkgroupHistory.CopyFrom(_sessionID, _talkgroupHistory);

            Assert.Equal($"Temp - System ID {_talkgroupHistory.SystemID}, Talkgroup ID {_talkgroupHistory.TalkgroupID} ({_talkgroupHistory.Description})",
                tempTalkgroupHistory.ToString());
        }
    }
}
