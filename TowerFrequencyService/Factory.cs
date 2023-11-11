using ObjectLibrary;
using ServiceCommon;
using TowerService;

namespace TowerFrequencyService
{
    public static class Factory
    {
        public static TowerFrequencyDataViewModel GetTowerFrequencyDataModel(SystemInfo systemInfo, TowerViewModel tower, string frequency,
            TowerFrequencyViewModel towerFrequencyViewModel, SearchDataViewModel searchData) =>
            
            new TowerFrequencyDataViewModel(systemInfo.SystemID, systemInfo.Description, tower.TowerNumber, tower.TowerName, towerFrequencyViewModel, searchData);
    }
}
