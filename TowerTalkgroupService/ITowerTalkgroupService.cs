using ObjectLibrary;
using ObjectLibrary.Interfaces;
using ServiceCommon;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TowerService;

namespace TowerTalkgroupService
{
    public interface ITowerTalkgroupService
    {
        Task<IEnumerable<TowerTalkgroup>> GetForTowerAsync(int systemID, int towerNumber);
        Task<IEnumerable<TowerTalkgroup>> GetForTowerImportAsync(int systemID, int towerNumber);
        Task<IEnumerable<TowerTalkgroup>> GetForTowerImportAsync(int systemID, int towerNumber, DateTime date);
        Task<IEnumerable<TowerTalkgroup>> GetTalkgroupsForTowerAsync(int systemID, int towerNumber);
        Task<int> GetTalkgroupsForTowerCountAsync(int systemID, int towerNumber);
        Task<(IEnumerable<TowerTalkgroupViewModel> towerTalkgroups, int recordCount)> GetTalkgroupsForTowerAsync(string systemID, int towerNumber,
            FilterDataModel filterData);
        Task<IEnumerable<TowerTalkgroup>> GetTalkgroupsForTowerByDateAsync(int systemID, int towerNumber);
        Task<IEnumerable<TowerTalkgroup>> GetTowersForTalkgroupAsync(int systemID, int talkgroupID);
        Task<int> GetTowersForTalkgroupCountAsync(int systemID, int talkgroupID);
        Task<(IEnumerable<TalkgroupTowerViewModel> talkgroupTowers, int recordCount)> GetTowersForTalkgroupViewAsync(string systemID, int talkgroupID,
            FilterDataModel filterData);
        Task<IEnumerable<TowerViewModel>> GetTowerListForTalkgroupAsync(string systemID, int talkgroupID, FilterDataModel filterData);
        Task<IEnumerable<DateModel>> GetDateListForTowerTalkgroupAsync(string systemID, int talkgroupID, int towerNumber, FilterDataModel filterData);
        void ProcessRecord(int systemID, int towerID, int talkgroupID, DateTime timeStamp, string action, ICollection<TowerTalkgroup> towerTalkgroups,
            Action<ICounterRecord, ActionTypes> updateCounters);
        TowerTalkgroup CreateTalkgroup(int systemID, int towerNumber, int talkgroupID, DateTime timeStamp);
    }
}
