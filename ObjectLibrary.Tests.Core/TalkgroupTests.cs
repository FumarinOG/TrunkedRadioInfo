using System;
using Xunit;

namespace ObjectLibrary.Tests.Core
{
    public class TalkgroupTests
    {
        [Fact]
        public void PropertiesAssignProperly()
        {
            var talkgroup = new Talkgroup
            {
                SystemID = 1,
                TalkgroupID = 36531,
                Priority = 50,
                Description = "Superior Air Med",
                LastSeenProgram = new DateTime(2018, 6, 11, 16, 12, 0),
                LastSeenProgramUnix = 1528751571,
                FirstSeenProgram = new DateTime(2018, 4, 20, 18, 35, 0),
                FirstSeenProgramUnix = 1524267332,
                FGColor = "#FFFFFF",
                BGColor = "#000000",
                EncryptionSeen = false,
                IgnoreEmergencySignal = false,
                HitCountProgram = 14,
                PhaseIISeen = false,
                PatchCount = 0
            };

            Assert.Equal(1, talkgroup.SystemID);
            Assert.Equal(36531, talkgroup.TalkgroupID);
            Assert.Equal(50, talkgroup.Priority);
            Assert.Equal("Superior Air Med", talkgroup.Description);
            Assert.Equal(new DateTime(2018, 6, 11, 16, 12, 0), talkgroup.LastSeenProgram);
            Assert.Equal(1528751571, talkgroup.LastSeenProgramUnix);
            Assert.Equal(new DateTime(2018, 4, 20, 18, 35, 0), talkgroup.FirstSeenProgram);
            Assert.Equal(1524267332, talkgroup.FirstSeenProgramUnix);
            Assert.Equal("#FFFFFF", talkgroup.FGColor);
            Assert.Equal("#000000", talkgroup.BGColor);
            Assert.False(talkgroup.EncryptionSeen);
            Assert.False(talkgroup.IgnoreEmergencySignal);
            Assert.Equal(14, talkgroup.HitCountProgram);
            Assert.False(talkgroup.PhaseIISeen);
            Assert.Equal(0, talkgroup.PatchCount);
            Assert.True(talkgroup.IsDirty);
        }

        [Fact]
        public void ToStringWorks()
        {
            var talkgroup = new Talkgroup
            {
                SystemID = 1,
                TalkgroupID = 36531,
                Description = "Superior Air Med"
            };

            Assert.Equal("System ID 1, Talkgroup ID 36531 (Superior Air Med)", talkgroup.ToString());
        }

        [Fact]
        public void AssignSetsValuesCorrectly()
        {
            var lastSeen = DateTime.Now.AddDays(-10);
            var firstSeen = DateTime.Now.AddYears(-1);
            var lastModified = DateTime.Now;
            var talkgroupSource = new Talkgroup
            {
                ID = 36531,
                LastSeen = lastSeen,
                FirstSeen = firstSeen,
                LastModified = lastModified
            };
            var talkgroupDestination = new Talkgroup();

            talkgroupDestination.Assign(talkgroupSource);

            Assert.Equal(talkgroupSource.ID, talkgroupDestination.ID);
            Assert.Equal(talkgroupSource.LastSeen, talkgroupDestination.LastSeen);
            Assert.Equal(talkgroupSource.FirstSeen, talkgroupDestination.FirstSeen);
            Assert.Equal(talkgroupSource.LastModified, talkgroupDestination.LastModified);
        }

        [Fact]
        public void EqualsReturnsTrueForMatchingObjects()
        {
            var talkgroup1 = new Talkgroup
            {
                SystemID = 1,
                TalkgroupID = 36531,
                Priority = 50,
                Description = "Superior Air Med",
                LastSeenProgram = new DateTime(2018, 6, 11, 16, 12, 0),
                LastSeenProgramUnix = 1528751571,
                FirstSeenProgram = new DateTime(2018, 4, 20, 18, 35, 0),
                FirstSeenProgramUnix = 1524267332,
                FGColor = "#FFFFFF",
                BGColor = "#000000",
                EncryptionSeen = false,
                IgnoreEmergencySignal = false,
                HitCount = 253,
                HitCountProgram = 14,
                PhaseIISeen = false,
                PatchCount = 0
            };
            var talkgroup2 = new Talkgroup
            {
                SystemID = 1,
                TalkgroupID = 36531,
                Priority = 50,
                Description = "Superior Air Med",
                LastSeenProgram = new DateTime(2018, 6, 11, 16, 12, 0),
                LastSeenProgramUnix = 1528751571,
                FirstSeenProgram = new DateTime(2018, 4, 20, 18, 35, 0),
                FirstSeenProgramUnix = 1524267332,
                FGColor = "#FFFFFF",
                BGColor = "#000000",
                EncryptionSeen = false,
                IgnoreEmergencySignal = false,
                HitCount = 253,
                HitCountProgram = 14,
                PhaseIISeen = false,
                PatchCount = 0
            };

            Assert.True(talkgroup1.Equals(talkgroup2));
            Assert.True(talkgroup1.GetHashCode() == talkgroup2.GetHashCode());
        }

        [Fact]
        public void EqualsReturnsFalseForNonMatchingObjects()
        {
            var talkgroup1 = new Talkgroup
            {
                SystemID = 1,
                TalkgroupID = 36531,
                Priority = 50,
                Description = "Superior Air Med",
                LastSeenProgram = new DateTime(2018, 6, 11, 16, 12, 0),
                LastSeenProgramUnix = 1528751571,
                FirstSeenProgram = new DateTime(2018, 4, 20, 18, 35, 0),
                FirstSeenProgramUnix = 1524267332,
                FGColor = "#FFFFFF",
                BGColor = "#000000",
                EncryptionSeen = false,
                IgnoreEmergencySignal = false,
                HitCount = 253,
                HitCountProgram = 14,
                PhaseIISeen = false,
                PatchCount = 0
            };
            var talkgroup2 = new Talkgroup
            {
                SystemID = 1,
                TalkgroupID = 36531,
                Priority = 50,
                Description = "Superior Air Med",
                LastSeenProgram = new DateTime(2018, 6, 11, 16, 12, 0),
                LastSeenProgramUnix = 1528751571,
                FirstSeenProgram = new DateTime(2018, 4, 20, 18, 35, 0),
                FirstSeenProgramUnix = 1524267332,
                FGColor = "#FFFFFF",
                BGColor = "#00000A",
                EncryptionSeen = false,
                IgnoreEmergencySignal = false,
                HitCount = 253,
                HitCountProgram = 14,
                PhaseIISeen = false,
                PatchCount = 0
            };

            Assert.False(talkgroup1.Equals(talkgroup2));
            Assert.False(talkgroup1.GetHashCode() == talkgroup2.GetHashCode());
        }

        [Fact]
        public void EqualsReturnsFalseForDifferingObjectTypes()
        {
            var talkgroup1 = new Talkgroup
            {
                SystemID = 1,
                TalkgroupID = 36531,
                Priority = 50,
                Description = "Superior Air Med",
                LastSeenProgram = new DateTime(2018, 6, 11, 16, 12, 0),
                LastSeenProgramUnix = 1528751571,
                FirstSeenProgram = new DateTime(2018, 4, 20, 18, 35, 0),
                FirstSeenProgramUnix = 1524267332,
                FGColor = "#FFFFFF",
                BGColor = "#000000",
                EncryptionSeen = false,
                IgnoreEmergencySignal = false,
                HitCount = 253,
                HitCountProgram = 14,
                PhaseIISeen = false,
                PatchCount = 0
            };
            var talkgroup2 = talkgroup1.ToString();

            Assert.False(talkgroup1.Equals(talkgroup2));
            Assert.False(talkgroup1.GetHashCode() == talkgroup2.GetHashCode());
        }
    }
}
