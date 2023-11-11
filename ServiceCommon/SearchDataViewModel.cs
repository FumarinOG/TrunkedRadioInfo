namespace ServiceCommon
{
    public sealed class SearchDataViewModel
    {
        public FilterDataModel SearchData { get; private set; }
        public string SearchDataEncoded { get; private set; }

        public SearchDataViewModel()
        {
        }

        public SearchDataViewModel(FilterDataModel searchData, string searchDataEncoded) => (SearchData, SearchDataEncoded) = (searchData, searchDataEncoded);
    }
}
