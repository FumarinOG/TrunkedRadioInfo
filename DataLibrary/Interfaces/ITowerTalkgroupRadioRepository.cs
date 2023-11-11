using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLibrary.Interfaces
{
    public interface ITowerTalkgroupRadioRepository
    {
        Task<IEnumerable<TowerTalkgroupRadio>> GetForTowerAsync(int systemID, int towerNumber);
        Task<IEnumerable<TowerTalkgroupRadio>> GetForTowerAsync(int systemID, int towerNumber, DateTime date);
        Task<IEnumerable<TowerTalkgroupRadio>> GetTowersForTalkgroupRadioAsync(int systemID, int talkgroupID, int radioID);
        Task<IEnumerable<TowerTalkgroupRadio>> GetTalkgroupsForRadioWithDatesAsync(int systemID, int radioID);
        Task<IEnumerable<TowerTalkgroupRadio>> GetRadiosForTalkgroupWithDatesAsync(int systemID, int talkgroupID);
        Task<(IEnumerable<TowerTalkgroupRadio> towerRadios, int recordCount)> GetRadiosForTalkgroupTowerAsync(string systemID, int talkgroupID, int towerNumber,
            FilterData filterData);
        Task<(IEnumerable<TowerTalkgroupRadio> towerTalkgroups, int recordCount)> GetTalkgroupsForTowerRadioAsync(string systemID, int radioID, int towerNumber,
            FilterData filterData);
    }
}
