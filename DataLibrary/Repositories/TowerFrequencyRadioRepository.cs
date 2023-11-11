using DataLibrary.Interfaces;
using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLibrary.Repositories
{
    public class TowerFrequencyRadioRepository : RepositoryBase, ITowerFrequencyRadioRepository
    {
        public async Task<IEnumerable<TowerFrequencyRadio>> GetForTowerAsync(int systemID, int towerNumber, DateTime date)
        {
            var towerFrequencyRadios = CreateList<TowerFrequencyRadio>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towerFrequencyRadios.AddRange(dataEntities
                            .TowerFrequencyRadiosGetForTowerDate(systemID, towerNumber, date)
                            .Select(tfr => _mapperConfig.Map<TowerFrequencyRadio>(tfr)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting tower frequency radio list");
                    throw;
                }
            });

            return towerFrequencyRadios;
        }

        public async Task<(IEnumerable<TowerFrequencyRadio> towerFrequencyRadios, int recordCount)> GetRadiosForTowerFrequencyAsync(string systemID,
            int towerNumber, string frequency, FilterData filterData)
        {
            var towerFrequencyRadios = CreateList<TowerFrequencyRadio>();
            int recordCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var results = dataEntities.TowerFrequencyRadiosGetRadiosForFrequenciesWithPaging(systemID, towerNumber, frequency, filterData.SearchText,
                                filterData.DateFrom, filterData.DateTo, filterData.IDFrom, filterData.IDTo, filterData.FirstSeenFrom, filterData.FirstSeenTo,
                                filterData.LastSeenFrom, filterData.LastSeenTo, filterData.SortField, filterData.SortDirection, filterData.PageNumber,
                                filterData.PageSize)
                            .ToList();

                        recordCount = results.FirstOrDefault()?.RecordCount ?? 0;
                        towerFrequencyRadios.AddRange(results.Select(tftg =>
                            _mapperConfig.Map<TowerFrequencyRadio>(tftg)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting talkgroups for tower frequency");
                    throw;
                }
            });

            return (towerFrequencyRadios, recordCount);
        }
    }
}
