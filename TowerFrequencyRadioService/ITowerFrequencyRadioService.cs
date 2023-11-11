using ObjectLibrary;
using ObjectLibrary.Interfaces;
using ServiceCommon;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TowerFrequencyRadioService
{
    public interface ITowerFrequencyRadioService
    {
        Task<IEnumerable<TowerFrequencyRadio>> GetForTowerAsync(int systemID, int towerNumber, DateTime date);
        Task<(IEnumerable<TowerFrequencyRadioViewModel> towerFrequencyRadios, int recordCount)> GetRadiosForTowerFrequencyAsync(string systemID,
            int towerNumber, string frequency, FilterDataModel filterData);
        void ProcessRecord(int systemID, int towerID, string frequency, int radioID, ICollection<Radio> radios,
            ICollection<TowerFrequencyRadio> towerFrequencyRadios, DateTime timeStamp, string action, Action<ICounterRecord, ActionTypes> updateCounters);
        TowerFrequencyRadio CreateTowerFrequencyRadio(int systemID, int towerNumber, string frequency, int radioID, DateTime timeStamp);
    }
}
