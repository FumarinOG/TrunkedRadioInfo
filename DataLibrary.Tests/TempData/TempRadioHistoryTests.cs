using DataLibrary.TempData;
using ObjectLibrary;
using System;
using Xunit;

namespace DataLibrary.Tests.TempData
{
    public class TempRadioHistoryTests
    {
        private readonly RadioHistory _radioHistory = new RadioHistory()
        {
            SystemID = 1,
            RadioID = 2908101,
            Description = "ISP Dispatch (Peoria) (08-A)",
            FirstSeen = DateTime.Now.AddYears(-1),
            LastSeen = DateTime.Now
        };
        private readonly Guid _sessionID = Guid.NewGuid();

        [Fact]
        public void CopyFromCopiesProperly()
        {
            var tempRadioHistory = new TempRadioHistory();

            tempRadioHistory.CopyFrom(_sessionID, _radioHistory);

            Assert.Equal(_sessionID, tempRadioHistory.SessionID);
            Assert.Equal(_radioHistory.SystemID, tempRadioHistory.SystemID);
            Assert.Equal(_radioHistory.RadioID, tempRadioHistory.RadioID);
            Assert.Equal(_radioHistory.Description, tempRadioHistory.Description);
            Assert.Equal(_radioHistory.FirstSeen, tempRadioHistory.FirstSeen);
            Assert.Equal(_radioHistory.LastSeen, tempRadioHistory.LastSeen);
            Assert.Equal("TempRadioHistory", tempRadioHistory.TableName);
        }

        [Fact]
        public void ToStringWorks()
        {
            var tempRadioHistory = new TempRadioHistory();

            tempRadioHistory.CopyFrom(_sessionID, _radioHistory);

            Assert.Equal($"Temp - System ID {_radioHistory.SystemID}, Radio ID {_radioHistory.RadioID} ({_radioHistory.Description})",
                tempRadioHistory.ToString());
        }
    }
}
