using DataLibrary.TempData;
using ObjectLibrary;
using System;
using Xunit;

namespace DataLibrary.Tests.TempData
{
    public class TempRadioTests
    {
        private readonly Radio _radio = new Radio()
        {
            SystemID = 1,
            RadioID = 1905001,
            Description = "ISP Dispatch (Joliet) (05-A)",
            LastSeen = DateTime.Now,
            LastSeenProgram = DateTime.Now.AddHours(-3),
            LastSeenProgramUnix = (long)(DateTime.Now.AddHours(-3) - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds,
            FirstSeen = DateTime.Now.AddYears(-1),
            FGColor = "#F3F3F3",
            BGColor = "#111111",
            PhaseIISeen = true,
            HitCount = 1830394
        };
        private readonly Guid _sessionID = Guid.NewGuid();

        [Fact]
        public void CopyFromCopiesProperly()
        {
            var tempRadio = new TempRadio();

            tempRadio.CopyFrom(_sessionID, _radio);

            Assert.Equal(_sessionID, tempRadio.SessionID);
            Assert.Equal(_radio.SystemID, tempRadio.SystemID);
            Assert.Equal(_radio.RadioID, tempRadio.RadioID);
            Assert.Equal(_radio.Description, tempRadio.Description);
            Assert.Equal(_radio.LastSeen, tempRadio.LastSeen);
            Assert.Equal(_radio.LastSeenProgram, tempRadio.LastSeenProgram);
            Assert.Equal(_radio.LastSeenProgramUnix, tempRadio.LastSeenProgramUnix);
            Assert.Equal(_radio.FirstSeen, tempRadio.FirstSeen);
            Assert.Equal(_radio.FGColor, tempRadio.FGColor);
            Assert.Equal(_radio.BGColor, tempRadio.BGColor);
            Assert.Equal(_radio.PhaseIISeen, tempRadio.PhaseIISeen);
            Assert.Equal(_radio.HitCount, tempRadio.HitCount);
        }

        [Fact]
        public void ToStringWorks()
        {
            var tempRadio = new TempRadio();

            tempRadio.CopyFrom(_sessionID, _radio);

            Assert.Equal($"Temp - System ID {_radio.SystemID}, Radio ID {_radio.RadioID} ({_radio.Description})", tempRadio.ToString());
        }
    }
}
