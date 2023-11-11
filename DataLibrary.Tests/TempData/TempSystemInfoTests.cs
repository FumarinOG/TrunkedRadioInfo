using DataLibrary.TempData;
using ObjectLibrary;
using System;
using Xunit;

namespace DataLibrary.Tests.TempData
{
    public class TempSystemInfoTests
    {
        private readonly SystemInfo _systemInfo = new SystemInfo()
        {
            SystemID = "140",
            SystemIDDecimal = 320,
            Description = "StarCom 21",
            WACN = "BEE00",
            FirstSeen = DateTime.Now.AddYears(-10),
            LastSeen = DateTime.Now
        };
        private readonly Guid _sessionID = Guid.NewGuid();

        [Fact]
        public void CopyFromCopiesProperly()
        {
            var tempSystemInfo = new TempSystemInfo();

            tempSystemInfo.CopyFrom(_sessionID, _systemInfo);

            Assert.Equal(_sessionID, tempSystemInfo.SessionID);
            Assert.Equal(_systemInfo.SystemID, tempSystemInfo.SystemID);
            Assert.Equal(_systemInfo.SystemIDDecimal, tempSystemInfo.SystemIDDecimal);
            Assert.Equal(_systemInfo.Description, tempSystemInfo.Description);
            Assert.Equal(_systemInfo.WACN, tempSystemInfo.WACN);
            Assert.Equal(_systemInfo.FirstSeen, tempSystemInfo.FirstSeen);
            Assert.Equal(_systemInfo.LastSeen, tempSystemInfo.LastSeen);
            Assert.Equal("TempSystems", tempSystemInfo.TableName);
        }

        [Fact]
        public void ToStringWorks()
        {
            var tempSystemInfo = new TempSystemInfo();

            tempSystemInfo.CopyFrom(_sessionID, _systemInfo);

            Assert.Equal($"Temp - System ID {_systemInfo.SystemID} ({_systemInfo.Description})", tempSystemInfo.ToString());
        }
    }
}
