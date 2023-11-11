using ObjectLibrary;

namespace FileService.Interfaces
{
    public interface ITowerFrequencyParser
    {
        TowerFrequency ParseTowerFrequency(string[] row, Tower tower);
    }
}
