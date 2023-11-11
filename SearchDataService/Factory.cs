using ServiceCommon;

namespace SearchDataService
{
    public static class Factory
    {
        public static SearchDataViewModel CreateSearchData(FilterDataModel searchData, string searchDataEncoded)
        {
            var searchDataViewModel = new SearchDataViewModel(searchData, searchDataEncoded);

            return searchDataViewModel;
        }

        public static FilterDataModel CreateFilterDataModel()
        {
            return new FilterDataModel();
        }
    }
}
