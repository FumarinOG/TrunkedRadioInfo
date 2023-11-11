using ObjectLibrary;
using ObjectLibrary.Interfaces;
using ServiceCommon;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TowerRadioService;
using TowerTalkgroupService;

namespace TowerTalkgroupRadioService
{
    public interface ITowerTalkgroupRadioService
    {
        Task<IEnumerable<TowerTalkgroupRadio>> GetForTowerAsync(int systemID, int towerNumber);
        Task<IEnumerable<TowerTalkgroupRadio>> GetForTowerAsync(int systemID, int towerNumber, DateTime date);
        Task<IEnumerable<TowerTalkgroupRadio>> GetTowersForTalkgroupRadioAsync(int systemID, int talkgroupID, int radioID);
        Task<IEnumerable<TowerTalkgroupRadio>> GetTalkgroupsForRadioWithDatesAsync(int systemID, int radioID);
        Task<IEnumerable<TowerTalkgroupRadio>> GetRadiosForTalkgroupWithDatesAsync(int systemID, int talkgroupID);
        Task<(IEnumerable<TowerRadioViewModel> towerRadios, int recordCount)> GetRadiosForTowerTalkgroupAsync(string systemID, int talkgroupID,
            int towerNumber, DateTime? date, FilterDataModel filterData);
        Task<(IEnumerable<TowerTalkgroupViewModel> towerTalkgroups, int recordCount)> GetTalkgroupsForTowerRadioAsync(string system, int radioID,
            int towerNumber, DateTime? date, FilterDataModel filterData);
        void ProcessRecord(int systemID, int towerID, int talkgroupID, int radioID, ICollection<TowerTalkgroupRadio> towerTalkgroupRadios,
            DateTime timeStamp, string action, Action<ICounterRecord, ActionTypes> updateCounters);
        TowerTalkgroupRadio CreateTowerTalkgroupRadio(int systemID, int towerNumber, int talkgroupID, int radioID, DateTime timeStamp);
    }
}
