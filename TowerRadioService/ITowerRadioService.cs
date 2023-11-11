using ObjectLibrary;
using ObjectLibrary.Interfaces;
using ServiceCommon;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TowerService;

namespace TowerRadioService
{
    public interface ITowerRadioService
    {
        Task<IEnumerable<TowerRadio>> GetForTowerAsync(int systemID, int towerNumber, DateTime date);
        Task<IEnumerable<TowerRadio>> GetRadiosForTowerAsync(int systemID, int towerNumber);
        Task<(IEnumerable<TowerRadioViewModel> towerRadios, int recordCount)> GetRadiosForTowerAsync(string systemID, int towerNumber, FilterDataModel filterData);
        Task<int> GetRadiosForTowerCountAsync(int systemID, int towerNumber);
        Task<IEnumerable<TowerRadio>> GetRadiosForTowerByDateAsync(int systemID, int towerNumber);
        Task<IEnumerable<TowerRadio>> GetTowersForRadioAsync(int systemID, int radioID);
        Task<int> GetTowersForRadioCountAsync(int systemID, int radioID);
        Task<(IEnumerable<RadioTowerViewModel> radioTowers, int recordCount)> GetTowersForRadioAsync(string systemID, int radioID, FilterDataModel filterData);
        Task<IEnumerable<TowerViewModel>> GetTowerListForRadioAsync(string systemID, int radioID, FilterDataModel filterData);
        Task<IEnumerable<DateModel>> GetDateListForTowerRadioAsync(string systemID, int radioID, int towerNumber, FilterDataModel filterData);
        void ProcessRecord(int systemID, int towerID, int radioID, DateTime timeStamp, string action, ICollection<TowerRadio> towerRadios,
            Action<ICounterRecord, ActionTypes> updateCounters);
        TowerRadio CreateRadio(int systemID, int towerNumber, int radioID, DateTime timeStamp);
    }
}
