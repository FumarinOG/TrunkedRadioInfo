using Xunit;

namespace TalkgroupService.Tests.Core
{
    public class TalkgroupDetailViewModelTests
    {
        [Fact]
        public void ConstructorAssignsValuesProperlyFor2Parameters()
        {
            var talkgroupDetailViewModel = new TalkgroupDetailViewModel("140", 9019);

            Assert.Equal("140", talkgroupDetailViewModel.SystemID);
            Assert.Equal(9019, talkgroupDetailViewModel.TalkgroupID);
        }

        [Fact]
        public void ConstructorAssignsValuesProperlyFor3Parameters()
        {
            var talkgroupDetailViewModel = new TalkgroupDetailViewModel("140", 9019, 1031);

            Assert.Equal("140", talkgroupDetailViewModel.SystemID);
            Assert.Equal(9019, talkgroupDetailViewModel.TalkgroupID);
            Assert.Equal(1031, talkgroupDetailViewModel.TowerNumber);
        }
    }
}
