using ObjectLibrary;
using ObjectLibrary.Interfaces;
using ServiceCommon;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TowerFrequencyTalkgroupService
{
    public interface ITowerFrequencyTalkgroupService
    {
        Task<IEnumerable<TowerFrequencyTalkgroup>> GetForTowerAsync(int systemID, int towerNumber, DateTime date);
        Task<(IEnumerable<TowerFrequencyTalkgroupViewModel> towerFrequencyTalkgroups, int recordCount)> GetTalkgroupsForTowerFrequencyAsync(string systemID,
            int towerNumber, string frequency, FilterDataModel filterData);
        void ProcessRecord(int systemID, int towerID, string frequency, int talkgroupID, ICollection<Talkgroup> talkgroups,
            ICollection<TowerFrequencyTalkgroup> towerFrequencyTalkgroups, DateTime timeStamp, string action, Action<ICounterRecord, ActionTypes> updateCounters);
        TowerFrequencyTalkgroup CreateTowerFrequencyTalkgroup(int systemID, int towerNumber, string frequency, int talkgroupID, DateTime timeStamp);
    }
}
