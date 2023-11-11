using System;
using Xunit;

namespace PatchService.Tests.Core
{
    public class PatchViewModelTests
    {
        [Fact]
        public void ConstructorAssignsProperValuesAndTextIsFormattedProperly()
        {
            var firstSeen = DateTime.Now.AddYears(-1);
            var lastSeen = DateTime.Now;

            var patchViewModel = new PatchViewModel("140", "Starcom 21", 9019, "ISP 17-A - LaSalle Primary", 9000, "ISP 01-A - Sterling Primary", 
                firstSeen, lastSeen, 1000000);

            Assert.Equal("140", patchViewModel.SystemID);
            Assert.Equal("Starcom 21", patchViewModel.SystemName);
            Assert.Equal(9019, patchViewModel.FromTalkgroupID);
            Assert.Equal("ISP 17-A - LaSalle Primary", patchViewModel.FromTalkgroupName);
            Assert.Equal(9000, patchViewModel.ToTalkgroupID);
            Assert.Equal("ISP 01-A - Sterling Primary", patchViewModel.ToTalkgroupName);
            Assert.Equal(firstSeen, patchViewModel.FirstSeen);
            Assert.Equal($"{firstSeen:MM-dd-yyyy HH:mm}", patchViewModel.FirstSeenText);
            Assert.Equal(lastSeen, patchViewModel.LastSeen);
            Assert.Equal($"{lastSeen:MM-dd-yyyy HH:mm}", patchViewModel.LastSeenText);
            Assert.Equal(1000000, patchViewModel.HitCount);
            Assert.Equal("1,000,000", patchViewModel.HitCountText);
        }
    }
}
