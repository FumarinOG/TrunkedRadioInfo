using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLibrary.Interfaces
{
    public interface ITowerRadioRepository
    {
        Task<IEnumerable<TowerRadio>> GetForTowerAsync(int systemID, int towerNumber);
        Task<IEnumerable<TowerRadio>> GetForTowerAsync(int systemID, int towerNumber, DateTime date);
        Task<IEnumerable<TowerRadio>> GetRadiosForTowerAsync(int systemID, int towerNumber);
        Task<(IEnumerable<TowerRadio> towerRadios, int recordCount)> GetRadiosForTowerAsync(string systemID, int towerNumber, FilterData filterData);
        Task<int> GetRadiosForTowerCountAsync(int systemID, int towerNumber);
        Task<IEnumerable<TowerRadio>> GetRadiosForTowerByDateAsync(int systemID, int towerNumber);
        Task<IEnumerable<TowerRadio>> GetTowersForRadioAsync(int systemID, int radioID);
        Task<int> GetTowersForRadioCountAsync(int systemID, int radioID);
        Task<(IEnumerable<TowerRadio> towerRadios, int recordCount)> GetTowersForRadioAsync(string systemID, int radioID, FilterData filterData);
        Task<IEnumerable<TowerRadio>> GetTowerListForRadioAsync(string systemID, int radioID, FilterData filterData);
        Task<IEnumerable<DateTime>> GetDateListForTowerRadioAsync(string systemID, int radioID, int towerNumber, FilterData filterData);
    }
}
