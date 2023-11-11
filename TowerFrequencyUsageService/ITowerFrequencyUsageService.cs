using ObjectLibrary;
using ObjectLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TowerFrequencyUsageService
{
    public interface ITowerFrequencyUsageService
    {
        Task<IEnumerable<TowerFrequencyUsage>> GetFrequenciesForTowerAsync(int systemID, int towerNumber);
        Task<IEnumerable<TowerFrequencyUsage>> GetFrequenciesForTowerAsync(int systemID, int towerNumber, DateTime date);
        void ProcessRecord(int systemID, int towerNumber, string frequency, string channel, int talkgroupID, int radioID,
            ICollection<TowerFrequencyUsage> towerFrequencyUsages, DateTime timeStamp, string action, Action<ICounterRecord, ActionTypes> updateCounters);
        TowerFrequencyUsage CreateTowerFrequencyUsage(int systemID, int towerNumber, string frequency, string channel, DateTime timeStamp);
    }
}
