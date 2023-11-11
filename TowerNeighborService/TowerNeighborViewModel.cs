using ServiceCommon;
using System;

namespace TowerNeighborService
{
    public sealed class TowerNeighborViewModel : IViewModel
    {
        public int NeighborTowerNumber { get; private set; }
        public string NeighborTowerName { get; private set; }
        public string NeighborControlChannel { get; private set; }
        public DateTime FirstSeen { get; private set; }
        public DateTime LastSeen { get; private set; }

        public TowerNeighborViewModel()
        {
        }

        public TowerNeighborViewModel(int neighborTowerNumber, string neighborTowerName, string neighborControlChannel, DateTime firstSeen, DateTime lastSeen) =>
            (NeighborTowerNumber, NeighborTowerName, NeighborControlChannel, FirstSeen, LastSeen) =
            (neighborTowerNumber, neighborTowerName, neighborControlChannel, firstSeen, lastSeen);
    }
}
