namespace TowerFrequencyService
{
    public sealed class TowerFrequencyDetailViewModel
    {
        public string SystemID { get; private set; }
        public int TowerNumber { get; private set; }
        public string TowerName { get; private set; }
        public string Frequency { get; private set; }

        public TowerFrequencyDetailViewModel(string systemID, int towerNumber, string frequency) :
            this(systemID, towerNumber, string.Empty, frequency)
        {
        }

        public TowerFrequencyDetailViewModel(string systemID, int towerNumber, string towerName, string frequency) =>
            (SystemID, TowerNumber, TowerName, Frequency) = (systemID, towerNumber, towerName, frequency);
    }
}
