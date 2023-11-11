using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLibrary.Interfaces
{
    public interface ITowerFrequencyTalkgroupRepository
    {
        Task<IEnumerable<TowerFrequencyTalkgroup>> GetForTowerAsync(int systemID, int towerID, DateTime date);
        Task<(IEnumerable<TowerFrequencyTalkgroup> towerFrequencyTalkgroups, int recordCount)> GetTalkgroupsForTowerFrequencyAsync(string systemID,
            int towerNumber, string frequency, FilterData filterData);
    }
}
