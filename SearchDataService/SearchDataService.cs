using Newtonsoft.Json;
using ServiceCommon;
using static SearchDataService.Factory;

namespace SearchDataService
{
    public sealed class SearchDataService : ISearchDataService
    {
        public FilterDataModel Create(string data)
        {
            if (data.IsNullOrWhiteSpace())
            {
                return CreateFilterDataModel();
            }

            return JsonConvert.DeserializeObject<FilterDataModel>(data);
        }

        public SearchDataViewModel GetView(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return new SearchDataViewModel();
            }

            return CreateSearchData(JsonConvert.DeserializeObject<FilterDataModel>(data), data);
        }
    }
}
