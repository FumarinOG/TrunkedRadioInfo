using ServiceCommon;
using Xunit;

namespace RadioService.Tests.Core
{
    public class RadioDataViewModelTests
    {
        [Fact]
        public void ConstructorSetsValuesProperly()
        {
            var radioDataViewModel = new RadioDataViewModel("140", "StarCom 21", 1917101, new RadioViewModel(), new SearchDataViewModel());

            Assert.Equal("140", radioDataViewModel.SystemID);
            Assert.Equal("StarCom 21", radioDataViewModel.SystemName);
            Assert.Equal(1917101, radioDataViewModel.RadioID);
            Assert.IsAssignableFrom<RadioViewModel>(radioDataViewModel.RadioData);
            Assert.IsAssignableFrom<SearchDataViewModel>(radioDataViewModel.SearchData);
        }
    }
}
