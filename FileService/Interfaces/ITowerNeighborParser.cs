using ObjectLibrary;
using System.Collections.Generic;

namespace FileService.Interfaces
{
    public interface ITowerNeighborParser
    {
        TowerNeighbor ParseTowerNeighbor(string[] row, Tower tower, Dictionary<string, int> systems);
    }
}
