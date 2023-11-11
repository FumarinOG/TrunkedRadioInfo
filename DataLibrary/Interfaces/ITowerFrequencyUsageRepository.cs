using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLibrary.Interfaces
{
    public interface ITowerFrequencyUsageRepository
    {
        Task<IEnumerable<TowerFrequencyUsage>> GetFrequenciesForTowerAsync(int systemID, int towerNumber);
        Task<IEnumerable<TowerFrequencyUsage>> GetFrequenciesForTowerAsync(int systemID, int towerNumber, DateTime date);
    }
}
