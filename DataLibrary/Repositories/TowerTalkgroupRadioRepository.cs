using DataLibrary.Interfaces;
using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLibrary.Repositories
{
    public class TowerTalkgroupRadioRepository : RepositoryBase, ITowerTalkgroupRadioRepository
    {
        public async Task<IEnumerable<TowerTalkgroupRadio>> GetForTowerAsync(int systemID, int towerNumber)
        {
            var towerTalkgroupRadios = CreateList<TowerTalkgroupRadio>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towerTalkgroupRadios.AddRange(dataEntities
                            .TowerTalkgroupRadiosGetForTower(systemID, towerNumber)
                            .Select(ttgr => _mapperConfig.Map<TowerTalkgroupRadio>(ttgr)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting talkgroup radios for tower");
                    throw;
                }
            });

            return towerTalkgroupRadios;
        }

        public async Task<IEnumerable<TowerTalkgroupRadio>> GetForTowerAsync(int systemID, int towerNumber, DateTime date)
        {
            var towerTalkgroupRadios = CreateList<TowerTalkgroupRadio>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towerTalkgroupRadios.AddRange(dataEntities.TowerTalkgroupRadiosGetForTowerDateRange(systemID, towerNumber, date)
                            .Select(ttgr => _mapperConfig.Map<TowerTalkgroupRadio>(ttgr)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting talkgroup radios for tower");
                    throw;
                }
            });

            return towerTalkgroupRadios;
        }

        public async Task<IEnumerable<TowerTalkgroupRadio>> GetTowersForTalkgroupRadioAsync(int systemID, int talkgroupID, int radioID)
        {
            var towerTalkgroupRadios = CreateList<TowerTalkgroupRadio>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towerTalkgroupRadios.AddRange(dataEntities
                            .TowerTalkgroupRadiosGetTowersForTalkgroupRadio(systemID, talkgroupID, radioID)
                            .Select(ttr => _mapperConfig.Map<TowerTalkgroupRadio>(ttr)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting towers for talkgroup radio");
                    throw;
                }
            });

            return towerTalkgroupRadios;
        }

        public async Task<IEnumerable<TowerTalkgroupRadio>> GetRadiosForTalkgroupWithDatesAsync(int systemID, int talkgroupID)
        {
            var towerTalkgroupRadios = CreateList<TowerTalkgroupRadio>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towerTalkgroupRadios.AddRange(dataEntities.TowerTalkgroupRadiosGetRadiosForTalkgroupWithDates(systemID, talkgroupID)
                            .Select(ttr => _mapperConfig.Map<TowerTalkgroupRadio>(ttr)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting tower radios for talkgroup date");
                    throw;
                }
            });

            return towerTalkgroupRadios;
        }

        public async Task<IEnumerable<TowerTalkgroupRadio>> GetTalkgroupsForRadioWithDatesAsync(int systemID, int radioID)
        {
            var towerTalkgroupRadios = CreateList<TowerTalkgroupRadio>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towerTalkgroupRadios.AddRange(dataEntities.TowerTalkgroupRadiosGetTalkgroupsForRadioWithDates(systemID, radioID)
                            .Select(ttgr => _mapperConfig.Map<TowerTalkgroupRadio>(ttgr)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting tower talkgroups for radio dates");
                    throw;
                }

            });
            return towerTalkgroupRadios;
        }

        public async Task<(IEnumerable<TowerTalkgroupRadio> towerRadios, int recordCount)> GetRadiosForTalkgroupTowerAsync(string systemID, int talkgroupID,
            int towerNumber, FilterData filterData)
        {
            var towerRadios = CreateList<TowerTalkgroupRadio>();
            int recordCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var results = dataEntities.TowerTalkgroupRadiosGetRadiosForTowerTalkgroupFiltersWithPaging(systemID,
                                talkgroupID, towerNumber, filterData.SearchText, filterData.DateFrom, filterData.DateTo,
                                filterData.IDFrom, filterData.IDTo, filterData.FirstSeenFrom, filterData.FirstSeenTo,
                                filterData.LastSeenFrom, filterData.LastSeenTo, filterData.SortField, filterData.SortDirection,
                                filterData.PageNumber, filterData.PageSize)
                            .ToList();

                        recordCount = results.FirstOrDefault()?.RecordCount ?? 0;
                        towerRadios.AddRange(results.Select(ttgr => _mapperConfig.Map<TowerTalkgroupRadio>(ttgr)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting radios for talkgroup tower");
                    throw;
                }
            });

            return (towerRadios, recordCount);
        }

        public async Task<(IEnumerable<TowerTalkgroupRadio> towerTalkgroups, int recordCount)> GetTalkgroupsForTowerRadioAsync(string systemID, int radioID,
            int towerNumber, FilterData filterData)
        {
            var towerTalkgroups = CreateList<TowerTalkgroupRadio>();
            int recordCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var results = dataEntities.TowerTalkgroupRadiosGetTalkgroupsForTowerRadioFiltersWithPaging(systemID, radioID, towerNumber,
                                filterData.SearchText, filterData.DateFrom, filterData.DateTo, filterData.IDFrom, filterData.IDTo, filterData.FirstSeenFrom,
                                filterData.FirstSeenTo, filterData.LastSeenFrom, filterData.LastSeenTo, filterData.SortField, filterData.SortDirection,
                                filterData.PageNumber, filterData.PageSize)
                            .ToList();

                        recordCount = results.FirstOrDefault()?.RecordCount ?? 0;
                        towerTalkgroups.AddRange(results.Select(ttgr => _mapperConfig.Map<TowerTalkgroupRadio>(ttgr)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting tower talkgroups for tower radio");
                    throw;
                }
            });

            return (towerTalkgroups, recordCount);
        }
    }
}
