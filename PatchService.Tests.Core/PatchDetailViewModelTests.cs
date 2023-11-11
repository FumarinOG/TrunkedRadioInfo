using System;
using Xunit;

namespace PatchService.Tests.Core
{
    public class PatchDetailViewModelTests
    {
        [Fact]
        public void ConstructorAssignsProperValuesAndTextIsFormattedProperly()
        {
            var firstSeen = DateTime.Now.AddYears(-1);
            var lastSeen = DateTime.Now;

            var patchDetailViewModel = new PatchDetailViewModel(1031, "LaSalle (LaSalle)", "140", "Starcom 21", 9019, "ISP 17-A - LaSalle Primary",
                9000, "ISP 01-A - Sterling Primary", firstSeen, lastSeen, 1000000);

            Assert.Equal(1031, patchDetailViewModel.TowerNumber);
            Assert.Equal("LaSalle (LaSalle)", patchDetailViewModel.TowerName);
            Assert.Equal("140", patchDetailViewModel.SystemID);
            Assert.Equal("Starcom 21", patchDetailViewModel.SystemName);
            Assert.Equal(9019, patchDetailViewModel.FromTalkgroupID);
            Assert.Equal("ISP 17-A - LaSalle Primary", patchDetailViewModel.FromTalkgroupName);
            Assert.Equal(9000, patchDetailViewModel.ToTalkgroupID);
            Assert.Equal("ISP 01-A - Sterling Primary", patchDetailViewModel.ToTalkgroupName);
            Assert.Equal(firstSeen, patchDetailViewModel.FirstSeen);
            Assert.Equal($"{firstSeen:MM-dd-yyyy HH:mm}", patchDetailViewModel.FirstSeenText);
            Assert.Equal(lastSeen, patchDetailViewModel.LastSeen);
            Assert.Equal($"{lastSeen:MM-dd-yyyy HH:mm}", patchDetailViewModel.LastSeenText);
            Assert.Equal(1000000, patchDetailViewModel.HitCount);
            Assert.Equal("1,000,000", patchDetailViewModel.HitCountText);
        }
    }
}
