using ObjectLibrary;

namespace FileService.Interfaces
{
    public interface ITowerTableParser
    {
        TowerTable ParseTowerTable(string[] row, Tower tower);
    }
}
