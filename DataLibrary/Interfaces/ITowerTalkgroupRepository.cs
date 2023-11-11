using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLibrary.Interfaces
{
    public interface ITowerTalkgroupRepository
    {
        Task<IEnumerable<TowerTalkgroup>> GetForTowerAsync(int systemID, int towerNumber);
        Task<IEnumerable<TowerTalkgroup>> GetForTowerImportAsync(int systemID, int towerNumber);
        Task<IEnumerable<TowerTalkgroup>> GetForTowerImportAsync(int systemID, int towerNumber, DateTime date);
        Task<IEnumerable<TowerTalkgroup>> GetTalkgroupsForTowerAsync(int systemID, int talkgroupID);
        Task<(IEnumerable<TowerTalkgroup> towerTalkgroups, int recordCount)> GetTalkgroupsForTowerAsync(string systemID, int towerNumber, FilterData filterData);
        Task<int> GetTalkgroupsForTowerCountAsync(int systemID, int towerNumber);
        Task<IEnumerable<TowerTalkgroup>> GetTalkgroupsForTowerByDateAsync(int systemID, int towerNumber);
        Task<IEnumerable<TowerTalkgroup>> GetTowersForTalkgroupAsync(int systemID, int talkgroupID);
        Task<int> GetTowersForTalkgroupCountAsync(int systemID, int talkgroupID);
        Task<(IEnumerable<TowerTalkgroup> towerTalkgroups, int recordCount)> GetTowersForTalkgroupsAsync(string systemID, int talkgroupID, FilterData filterData);
        Task<IEnumerable<TowerTalkgroup>> GetTowerListForTalkgroupAsync(string systemID, int talkgroupID, FilterData filterData);
        Task<IEnumerable<DateTime>> GetDateListForTowerTalkgroupAsync(string systemID, int talkgroupID, int towerNumber, FilterData filterData);
    }
}
