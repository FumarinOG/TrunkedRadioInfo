using System;

namespace PatchService
{
    public sealed class PatchDatesViewModel
    {
        public int TowerNumber { get; private set; }
        public string TowerName { get; private set; }
        public DateTime Date { get; private set; }
        public int HitCount { get; private set; }

        public PatchDatesViewModel(int towerNumber, string towerName, DateTime date, int hitCount) =>
            (TowerNumber, TowerName, Date, HitCount) = (towerNumber, towerName, date, hitCount);
    }
}
