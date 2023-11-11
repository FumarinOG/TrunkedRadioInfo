using System;
using Xunit;

namespace ObjectLibrary.Tests.Core
{
    public class RadioTests
    {
        [Fact]
        public void PropertiesAssignedProperly()
        {
            var radio = new Radio
            {
                SystemID = 1,
                RadioID = 1917101,
                Description = "ISP Dispatch (LaSalle) (17-A)",
                LastSeenProgram = new DateTime(2016, 10, 2, 22, 52, 0),
                LastSeenProgramUnix = 1475466750,
                FGColor = "#FFFFFF",
                BGColor = "#000000",
                PhaseIISeen = false
            };

            Assert.Equal(1, radio.SystemID);
            Assert.Equal(1917101, radio.RadioID);
            Assert.Equal("ISP Dispatch (LaSalle) (17-A)", radio.Description);
            Assert.Equal(new DateTime(2016, 10, 2, 22, 52, 0), radio.LastSeenProgram);
            Assert.Equal(1475466750, radio.LastSeenProgramUnix);
            Assert.Equal("#FFFFFF", radio.FGColor);
            Assert.Equal("#000000", radio.BGColor);
            Assert.False(radio.PhaseIISeen);
        }

        [Theory]
        [InlineData(true, "Yes")]
        [InlineData(false, "No")]
        [InlineData(null, "No")]
        public void PhaseIITextReturnsProperText(bool? testValue, string result)
        {
            var radio = new Radio
            {
                PhaseIISeen = testValue
            };

            Assert.Equal(result, radio.PhaseIISeenText);
        }

        [Fact]
        public void AssignSetsValuesCorrectly()
        {
            var firstSeen = DateTime.Now.AddYears(-1);
            var lastSeen = DateTime.Now.AddMinutes(-10);
            var lastModified = DateTime.Now;
            var radioSource = new Radio
            {
                ID = 1,
                FirstSeen = firstSeen,
                LastSeen = lastSeen,
                HitCount = 100,
                LastModified = lastModified
            };
            var radioDestination = new Radio();

            radioDestination.Assign(radioSource);

            Assert.Equal(radioDestination.ID, radioSource.ID);
            Assert.Equal(radioDestination.FirstSeen, radioSource.FirstSeen);
            Assert.Equal(radioDestination.LastSeen, radioSource.LastSeen);
            Assert.Equal(radioDestination.HitCount, radioSource.HitCount);
            Assert.Equal(radioDestination.LastModified, radioSource.LastModified);
        }

        [Fact]
        public void ToStringWorks()
        {
            var radio = new Radio
            {
                SystemID = 1,
                RadioID = 1917101,
                Description = "ISP Dispatch (LaSalle) (17-A)",
                LastSeenProgram = new DateTime(2016, 10, 2, 22, 52, 0),
                LastSeenProgramUnix = 1475466750,
                FGColor = "#FFFFFF",
                BGColor = "#000000",
                PhaseIISeen = false
            };

            Assert.Equal("System ID 1, Radio ID 1917101 (ISP Dispatch (LaSalle) (17-A))", radio.ToString());
        }

        [Fact]
        public void EqualsReturnsTrueForMatchingObjects()
        {
            var lastSeen = DateTime.Now;
            var firstSeen = DateTime.Now.AddYears(-1);
            var radio1 = new Radio
            {
                SystemID = 1,
                RadioID = 1917101,
                Description = "ISP Dispatch (LaSalle) (17-A)",
                LastSeen = lastSeen,
                LastSeenProgram = new DateTime(2016, 10, 2, 22, 52, 0),
                LastSeenProgramUnix = 1475466750,
                FirstSeen = firstSeen,
                FGColor = "#FFFFFF",
                BGColor = "#000000",
                HitCount = 100,
                PhaseIISeen = false
            };
            var radio2 = new Radio
            {
                SystemID = 1,
                RadioID = 1917101,
                Description = "ISP Dispatch (LaSalle) (17-A)",
                LastSeen = lastSeen,
                LastSeenProgram = new DateTime(2016, 10, 2, 22, 52, 0),
                LastSeenProgramUnix = 1475466750,
                FirstSeen = firstSeen,
                FGColor = "#FFFFFF",
                BGColor = "#000000",
                HitCount = 100,
                PhaseIISeen = false
            };

            Assert.True(radio1.Equals(radio2));
            Assert.True(radio1.GetHashCode() == radio2.GetHashCode());
        }

        [Fact]
        public void EqualsReturnsFalseForNonMatchingObjects()
        {
            var lastSeen = DateTime.Now;
            var firstSeen = DateTime.Now.AddYears(-1);
            var radio1 = new Radio
            {
                SystemID = 1,
                RadioID = 1917101,
                Description = "ISP Dispatch (LaSalle) (17-A)",
                LastSeen = lastSeen,
                LastSeenProgram = new DateTime(2016, 10, 2, 22, 52, 0),
                LastSeenProgramUnix = 1475466750,
                FirstSeen = firstSeen,
                FGColor = "#FFFFFF",
                BGColor = "#000000",
                HitCount = 100,
                PhaseIISeen = false
            };
            var radio2 = new Radio
            {
                SystemID = 1,
                RadioID = 1917102,
                Description = "ISP Dispatch (LaSalle) (17-B)",
                LastSeen = lastSeen,
                LastSeenProgram = new DateTime(2016, 10, 2, 22, 52, 0),
                LastSeenProgramUnix = 1475466750,
                FirstSeen = firstSeen,
                FGColor = "#FFFFFF",
                BGColor = "#000000",
                HitCount = 100,
                PhaseIISeen = false
            };

            Assert.False(radio1.Equals(radio2));
            Assert.False(radio1.GetHashCode() == radio2.GetHashCode());
        }

        [Fact]
        public void EqualsReturnsFalseForDifferingObjectTypes()
        {
            var lastSeen = DateTime.Now;
            var firstSeen = DateTime.Now.AddYears(-1);
            var radio1 = new Radio
            {
                SystemID = 1,
                RadioID = 1917101,
                Description = "ISP Dispatch (LaSalle) (17-A)",
                LastSeen = lastSeen,
                LastSeenProgram = new DateTime(2016, 10, 2, 22, 52, 0),
                LastSeenProgramUnix = 1475466750,
                FirstSeen = firstSeen,
                FGColor = "#FFFFFF",
                BGColor = "#000000",
                HitCount = 100,
                PhaseIISeen = false
            };
            var radio2 = radio1.ToString();

            Assert.False(radio1.Equals(radio2));
            Assert.False(radio1.GetHashCode() == radio2.GetHashCode());
        }
    }
}
