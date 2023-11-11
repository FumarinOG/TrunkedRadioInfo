using ServiceCommon;
using System;

namespace TowerService
{
    public sealed class TowerViewModel : IViewModel
    {
        public int TowerNumber { get; private set; }
        public string TowerName { get; private set; }
        public string Title => $"{TowerNumber} ({TowerName})";
        public DateTime FirstSeen { get; private set; }
        public DateTime LastSeen { get; private set; }
        public int HitCount { get; private set; }

        public TowerViewModel()
        {
        }

        public TowerViewModel(int towerNumber, string towerName, DateTime firstSeen, DateTime lastSeen, int hitCount) =>
            (TowerNumber, TowerName, FirstSeen, LastSeen, HitCount) = (towerNumber, towerName, firstSeen, lastSeen, hitCount);
    }
}
