using ObjectLibrary;
using ServiceCommon;

namespace RadioService
{
    public static class Factory
    {
        public static RadioDataViewModel CreateRadioDataModel(SystemInfo systemInfo, int radioID, RadioViewModel radioViewModel, SearchDataViewModel searchData)
        {
            return new RadioDataViewModel(systemInfo.SystemID, systemInfo.Description, radioID, radioViewModel, searchData);
        }
    }
}
