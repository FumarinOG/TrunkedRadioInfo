using System;
using Xunit;

namespace PatchService.Tests.Core
{
    public class PatchDatesViewModelTests
    {
        [Fact]
        public void ConstructorSetsProperValues()
        {
            var date = DateTime.Now;
            var random = new Random(DateTime.Now.Millisecond);
            var hitCount = random.Next(1, 99999999);

            var patchDatesViewModel = new PatchDatesViewModel(1031, "LaSalle (LaSalle)", date, hitCount);

            Assert.Equal(1031, patchDatesViewModel.TowerNumber);
            Assert.Equal("LaSalle (LaSalle)", patchDatesViewModel.TowerName);
            Assert.Equal(date, patchDatesViewModel.Date);
            Assert.Equal(hitCount, patchDatesViewModel.HitCount);
        }
    }
}
