using DataLibrary.Interfaces;
using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLibrary.Repositories
{
    public class TowerRadioRepository : RepositoryBase, ITowerRadioRepository
    {
        public async Task<IEnumerable<TowerRadio>> GetForTowerAsync(int systemID, int towerNumber)
        {
            var towerRadios = CreateList<TowerRadio>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towerRadios.AddRange(dataEntities.TowerRadiosGetForTower(systemID, towerNumber)
                            .Select(tr => _mapperConfig.Map<TowerRadio>(tr)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting tower radios for tower");
                    throw;
                }
            });

            return towerRadios;
        }

        public async Task<IEnumerable<TowerRadio>> GetForTowerAsync(int systemID, int towerNumber, DateTime date)
        {
            var towerRadios = CreateList<TowerRadio>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towerRadios.AddRange(dataEntities.TowerRadiosGetForTowerDateRange(systemID, towerNumber, date)
                            .Select(tr => _mapperConfig.Map<TowerRadio>(tr)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting tower radios for tower");
                    throw;
                }
            });

            return towerRadios;
        }

        public async Task<IEnumerable<TowerRadio>> GetRadiosForTowerAsync(int systemID, int towerNumber)
        {
            var towerRadios = CreateList<TowerRadio>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towerRadios.AddRange(dataEntities.TowerRadiosGetRadiosForTower(systemID, towerNumber)
                            .Select(tr => _mapperConfig.Map<TowerRadio>(tr)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting radios for tower");
                    throw;
                }
            });

            return towerRadios;
        }

        public async Task<(IEnumerable<TowerRadio> towerRadios, int recordCount)> GetRadiosForTowerAsync(string systemID, int towerNumber, FilterData filterData)
        {
            var towerRadios = CreateList<TowerRadio>();
            int recordCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var results = dataEntities.TowerRadiosGetRadiosForTowerFiltersWithPaging(systemID,
                                towerNumber, filterData.SearchText, filterData.DateFrom, filterData.DateTo,
                                filterData.IDFrom, filterData.IDTo, filterData.FirstSeenFrom, filterData.FirstSeenTo,
                                filterData.LastSeenFrom, filterData.LastSeenTo, filterData.SortField, filterData.SortDirection,
                                filterData.PageNumber, filterData.PageSize)
                            .ToList();

                        recordCount = results.FirstOrDefault()?.RecordCount ?? 0;
                        towerRadios.AddRange(results.Select(tr => _mapperConfig.Map<TowerRadio>(tr)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting radios for tower");
                    throw;
                }
            });

            return (towerRadios, recordCount);
        }

        public async Task<int> GetRadiosForTowerCountAsync(int systemID, int towerNumber)
        {
            int towerRadioCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towerRadioCount = dataEntities.TowerRadiosGetRadiosForTowerCount(systemID, towerNumber)
                            .SingleOrDefault() ?? 0;
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting radios for tower count");
                    throw;
                }
            });

            return towerRadioCount;
        }

        public async Task<IEnumerable<TowerRadio>> GetRadiosForTowerByDateAsync(int systemID, int towerNumber)
        {
            var towerRadios = CreateList<TowerRadio>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towerRadios.AddRange(dataEntities.TowerRadiosGetRadiosForTowerByDate(systemID, towerNumber)
                            .Select(tr => _mapperConfig.Map<TowerRadio>(tr)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting radios for tower by date");
                    throw;
                }
            });

            return towerRadios;
        }

        public async Task<IEnumerable<TowerRadio>> GetTowersForRadioAsync(int systemID, int radioID)
        {
            var towerRadios = CreateList<TowerRadio>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towerRadios.AddRange(dataEntities.TowerRadiosGetTowersForRadio(systemID, radioID)
                            .Select(tr => _mapperConfig.Map<TowerRadio>(tr)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting towers for radio");
                    throw;
                }
            });

            return towerRadios;
        }

        public async Task<int> GetTowersForRadioCountAsync(int systemID, int radioID)
        {
            int towerRadioCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towerRadioCount = dataEntities.TowerRadiosGetTowersForRadioCount(systemID, radioID)
                            .SingleOrDefault() ?? 0;
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting towers for radio count");
                    throw;
                }
            });

            return towerRadioCount;
        }

        public async Task<(IEnumerable<TowerRadio> towerRadios, int recordCount)> GetTowersForRadioAsync(string systemID, int radioID, FilterData filterData)
        {
            var towerRadios = CreateList<TowerRadio>();
            int recordCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var results = dataEntities.TowerRadiosGetTowersForRadioFiltersWithPaging(systemID, radioID,
                                filterData.SearchText, filterData.DateFrom, filterData.DateTo, filterData.IDFrom,
                                filterData.IDTo, filterData.FirstSeenFrom, filterData.FirstSeenTo, filterData.LastSeenFrom,
                                filterData.LastSeenTo, filterData.SortField, filterData.SortDirection, filterData.PageNumber, filterData.PageSize)
                            .ToList();

                        recordCount = results.FirstOrDefault()?.RecordCount ?? 0;
                        towerRadios.AddRange(results.Select(tr => _mapperConfig.Map<TowerRadio>(tr)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting towers for radio");
                    throw;
                }
            });

            return (towerRadios, recordCount);
        }

        public async Task<IEnumerable<TowerRadio>> GetTowerListForRadioAsync(string systemID, int radioID, FilterData filterData)
        {
            var towers = CreateList<TowerRadio>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var results = dataEntities.TowerRadiosGetTowerListForRadio(systemID, radioID, filterData.DateFrom, filterData.DateTo)
                            .ToList();

                        towers.AddRange(results.Select(tr => _mapperConfig.Map<TowerRadio>(tr)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting tower list for radio");
                    throw;
                }
            });

            return towers;
        }

        public async Task<IEnumerable<DateTime>> GetDateListForTowerRadioAsync(string systemID, int radioID, int towerNumber, FilterData filterData)
        {
            var dates = CreateList<DateTime>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var results = dataEntities.TowerRadiosGetDateListForTowerRadio(systemID, radioID, towerNumber, filterData.DateFrom, filterData.DateTo)
                            .ToList();

                        dates.AddRange(results.Select(d => Convert.ToDateTime(d)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting date list for tower radio");
                    throw;
                }
            });

            return dates;
        }
    }
}
