using DataLibrary.TempData;
using ObjectLibrary;
using System;
using Xunit;

namespace DataLibrary.Tests.TempData
{
    public class TempPatchTests
    {
        private readonly Patch _patch = new Patch()
        {
            SystemID = 1,
            TowerNumber = 1031,
            FromTalkgroupID = 9019,
            ToTalkgroupID = 9020,
            Date = DateTime.Now,
            FirstSeen = DateTime.Now.AddYears(-1),
            LastSeen = DateTime.Now,
            HitCount = 150
        };
        private readonly Guid _sessionID = Guid.NewGuid();

        [Fact]
        public void CopyFromCopiesProperly()
        {
            var tempPatch = new TempPatch();

            tempPatch.CopyFrom(_sessionID, _patch);

            Assert.Equal(_sessionID, tempPatch.SessionID);
            Assert.Equal(_patch.SystemID, tempPatch.SystemID);
            Assert.Equal(_patch.TowerNumber, tempPatch.TowerID);
            Assert.Equal(_patch.FromTalkgroupID, tempPatch.FromTalkgroupID);
            Assert.Equal(_patch.ToTalkgroupID, tempPatch.ToTalkgroupID);
            Assert.Equal(_patch.Date, tempPatch.Date);
            Assert.Equal(_patch.FirstSeen, tempPatch.FirstSeen);
            Assert.Equal(_patch.LastSeen, tempPatch.LastSeen);
            Assert.Equal(_patch.HitCount, tempPatch.HitCount);
            Assert.Equal("TempPatches", tempPatch.TableName);
        }

        [Fact]
        public void ToStringWorks()
        {
            var tempPatch = new TempPatch();

            tempPatch.CopyFrom(_sessionID, _patch);

            Assert.Equal($"Temp - TowerID {_patch.TowerNumber}, From {_patch.FromTalkgroupID} to {_patch.ToTalkgroupID}", tempPatch.ToString());
        }
    }
}
