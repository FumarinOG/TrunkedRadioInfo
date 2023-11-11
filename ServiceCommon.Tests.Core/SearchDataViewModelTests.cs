
using Xunit;

namespace ServiceCommon.Tests.Core
{
    public class SearchDataViewModelTests
    {
        [Fact]
        public void ConstructorAssignsValuesProperly()
        {
            var searchDataViewModel = new SearchDataViewModel(new FilterDataModel(), "search string");

            Assert.IsAssignableFrom<FilterDataModel>(searchDataViewModel.SearchData);
            Assert.Equal("search string", searchDataViewModel.SearchDataEncoded);
        }
    }
}
