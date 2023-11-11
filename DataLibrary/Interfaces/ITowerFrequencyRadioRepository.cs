using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLibrary.Interfaces
{
    public interface ITowerFrequencyRadioRepository
    {
        Task<IEnumerable<TowerFrequencyRadio>> GetForTowerAsync(int systemID, int towerID, DateTime date);
        Task<(IEnumerable<TowerFrequencyRadio> towerFrequencyRadios, int recordCount)> GetRadiosForTowerFrequencyAsync(string systemID, int towerNumber,
            string frequency, FilterData filterData);
    }
}
