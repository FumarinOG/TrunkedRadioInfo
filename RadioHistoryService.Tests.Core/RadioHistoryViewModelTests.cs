using System;
using Xunit;

namespace RadioHistoryService.Tests.Core
{
    public class RadioHistoryViewModelTests
    {
        [Fact]
        public void ConstructorAssignsAppropriateValues()
        {
            var firstSeen = DateTime.Now.AddYears(-1);
            var lastSeen = DateTime.Now;

            var radioHistoryViewModel = new RadioHistoryViewModel("ISP Unit 17-27 (LaSalle)", firstSeen, lastSeen);

            Assert.Equal("ISP Unit 17-27 (LaSalle)", radioHistoryViewModel.RadioName);
            Assert.Equal(firstSeen, radioHistoryViewModel.FirstSeen);
            Assert.Equal(lastSeen, radioHistoryViewModel.LastSeen);
        }
    }
}
