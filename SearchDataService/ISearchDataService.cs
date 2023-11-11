using ServiceCommon;

namespace SearchDataService
{
    public interface ISearchDataService
    {
        FilterDataModel Create(string data);
        SearchDataViewModel GetView(string data);
    }
}
