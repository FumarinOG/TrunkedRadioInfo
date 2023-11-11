namespace TowerService
{
    public sealed class TowerDetailViewModel
    {
        public string SystemID { get; private set; }
        public int TowerNumber { get; private set; }

        public TowerDetailViewModel(string systemID, int towerNumber) => (SystemID, TowerNumber) = (systemID, towerNumber);
    }
}
