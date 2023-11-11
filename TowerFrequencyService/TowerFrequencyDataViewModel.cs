using ServiceCommon;

namespace TowerFrequencyService
{
    public sealed class TowerFrequencyDataViewModel
    {
        public string SystemID { get; private set; }
        public string SystemName { get; private set; }
        public int TowerNumber { get; private set; }
        public string TowerName { get; private set; }
        public TowerFrequencyViewModel TowerFrequencyData { get; private set; }
        public SearchDataViewModel SearchData { get; private set; }

        public TowerFrequencyDataViewModel()
        {
        }
        public TowerFrequencyDataViewModel(string systemID, string systemName, int towerNumber, string towerName, TowerFrequencyViewModel towerFrequencyData,
            SearchDataViewModel searchData) =>

            (SystemID, SystemName, TowerNumber, TowerName, TowerFrequencyData, SearchData) = 
            (systemID, systemName, towerNumber, towerName, towerFrequencyData, searchData);
    }
}
