using System;
using Xunit;

namespace ObjectLibrary.Tests.Core
{
    public class PatchTests
    {
        [Fact]
        public void PropertiesAssignedProperly()
        {
            var date = DateTime.Now;

            var patch = new Patch
            {
                SystemID = 1,
                TowerNumber = 1031,
                Date = date,
                FromTalkgroupID = 9000,
                ToTalkgroupID = 9019
            };

            Assert.Equal(1, patch.SystemID);
            Assert.Equal(1031, patch.TowerNumber);
            Assert.Equal(date, patch.Date);
            Assert.Equal(9000, patch.FromTalkgroupID);
            Assert.Equal(9019, patch.ToTalkgroupID);
            Assert.True(patch.IsDirty);
        }

        [Fact]
        public void ToStringWorks()
        {
            var patch = new Patch
            {
                SystemID = 1,
                FromTalkgroupID = 9000,
                ToTalkgroupID = 9019
            };

            Assert.Equal("System ID 1, From 9000 to 9019", patch.ToString());
        }

        [Fact]
        public void EqualsReturnsTrueForMatchingObjects()
        {
            var firstSeen = DateTime.Now.AddYears(-1);
            var lastSeen = DateTime.Now;
            var patch1 = new Patch
            {
                SystemID = 1,
                FromTalkgroupID = 9019,
                ToTalkgroupID = 9000,
                LastSeen = lastSeen,
                FirstSeen = firstSeen,
                HitCount = 100
            };
            var patch2 = new Patch
            {
                SystemID = 1,
                FromTalkgroupID = 9019,
                ToTalkgroupID = 9000,
                LastSeen = lastSeen,
                FirstSeen = firstSeen,
                HitCount = 100
            };

            Assert.True(patch1.Equals(patch2));
            Assert.True(patch1.GetHashCode() == patch2.GetHashCode());
        }

        [Fact]
        public void EqualsReturnsFalseForNonMatchingObjects()
        {
            var firstSeen = DateTime.Now.AddYears(-1);
            var lastSeen = DateTime.Now;
            var patch1 = new Patch
            {
                SystemID = 1,
                FromTalkgroupID = 9019,
                ToTalkgroupID = 9000,
                LastSeen = lastSeen,
                FirstSeen = firstSeen,
                HitCount = 100
            };
            var patch2 = new Patch
            {
                SystemID = 1,
                FromTalkgroupID = 9002,
                ToTalkgroupID = 9003,
                LastSeen = lastSeen,
                FirstSeen = firstSeen,
                HitCount = 100
            };

            Assert.False(patch1.Equals(patch2));
            Assert.False(patch1.GetHashCode() == patch2.GetHashCode());
        }

        [Fact]
        public void EqualsReturnsFalseForDifferingObjectTypes()
        {
            var firstSeen = DateTime.Now.AddYears(-1);
            var lastSeen = DateTime.Now;
            var patch1 = new Patch
            {
                SystemID = 1,
                FromTalkgroupID = 9019,
                ToTalkgroupID = 9000,
                LastSeen = lastSeen,
                FirstSeen = firstSeen,
                HitCount = 100
            };
            var patch2 = patch1.ToString();

            Assert.False(patch1.Equals(patch2));
            Assert.False(patch1.GetHashCode() == patch2.GetHashCode());
        }
    }
}
