using ServiceCommon;

namespace TowerService
{
    public sealed class TowerDataViewModel
    {
        public string SystemID { get; private set; }
        public string SystemName { get; private set; }
        public int TowerNumber { get; private set; }
        public string TowerName { get; private set; }
        public TowerViewModel TowerData { get; private set; }
        public SearchDataViewModel SearchData { get; private set; }

        public TowerDataViewModel(string systemID, string systemName, int towerNumber, TowerViewModel towerData, SearchDataViewModel searchData) :
            this(systemID, systemName, towerNumber, string.Empty, towerData, searchData)
        {
        }

        public TowerDataViewModel(string systemID, string systemName, int towerNumber, string towerName, TowerViewModel towerData,
            SearchDataViewModel searchData) =>

            (SystemID, SystemName, TowerNumber, TowerName, TowerData, SearchData) = (systemID, systemName, towerNumber, towerName, towerData, searchData);
    }
}
