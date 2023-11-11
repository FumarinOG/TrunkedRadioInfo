using ServiceCommon;
using Xunit;

namespace TalkgroupService.Tests.Core
{
    public class TalkgroupDataViewModelTests
    {
        [Fact]
        public void ConstructorAssignesValuesProperly()
        {
            var talkgroupDataViewModel = new TalkgroupDataViewModel("140", "StarCom 21", 9019, new TalkgroupViewModel(), new SearchDataViewModel());

            Assert.Equal("140", talkgroupDataViewModel.SystemID);
            Assert.Equal("StarCom 21", talkgroupDataViewModel.SystemName);
            Assert.Equal(9019, talkgroupDataViewModel.TalkgroupID);
            Assert.IsAssignableFrom<TalkgroupViewModel>(talkgroupDataViewModel.TalkgroupData);
            Assert.IsAssignableFrom<SearchDataViewModel>(talkgroupDataViewModel.SearchData);
        }
    }
}
