using DataLibrary.Interfaces;
using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLibrary.Repositories
{
    public class TowerTalkgroupRepository : RepositoryBase, ITowerTalkgroupRepository
    {
        public async Task<IEnumerable<TowerTalkgroup>> GetForTowerAsync(int systemID, int towerNumber)
        {
            var towerTalkgroups = CreateList<TowerTalkgroup>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towerTalkgroups.AddRange(dataEntities
                            .TowerTalkgroupsGetForTower(systemID, towerNumber)
                            .Select(ttg => _mapperConfig.Map<TowerTalkgroup>(ttg)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting tower talkgroups for tower");
                    throw;
                }
            });

            return towerTalkgroups;
        }

        public async Task<IEnumerable<TowerTalkgroup>> GetForTowerImportAsync(int systemID, int towerNumber)
        {
            var towerTalkgroups = CreateList<TowerTalkgroup>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towerTalkgroups.AddRange(dataEntities.TowerTalkgroupsGetForTowerImport(systemID, towerNumber)
                            .Select(ttg => _mapperConfig.Map<TowerTalkgroup>(ttg)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting tower talkgroups for tower import");
                    throw;
                }
            });

            return towerTalkgroups;
        }

        public async Task<IEnumerable<TowerTalkgroup>> GetForTowerImportAsync(int systemID, int towerNumber, DateTime date)
        {
            var towerTalkgroups = CreateList<TowerTalkgroup>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towerTalkgroups.AddRange(dataEntities.TowerTalkgroupsGetForTowerImportDateRange(systemID, towerNumber, date)
                            .Select(ttg => _mapperConfig.Map<TowerTalkgroup>(ttg)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting tower talkgroups for tower import");
                    throw;
                }
            });

            return towerTalkgroups;
        }

        public async Task<IEnumerable<TowerTalkgroup>> GetTalkgroupsForTowerAsync(int systemID, int towerNumber)
        {
            var towerTalkgroups = CreateList<TowerTalkgroup>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towerTalkgroups.AddRange(dataEntities.TowerTalkgroupsGetTalkgroupsForTower(systemID, towerNumber)
                            .Select(tt => _mapperConfig.Map<TowerTalkgroup>(tt)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting talkgroups for tower");
                    throw;
                }
            });

            return towerTalkgroups;
        }

        public async Task<(IEnumerable<TowerTalkgroup> towerTalkgroups, int recordCount)> GetTalkgroupsForTowerAsync(string systemID, int towerNumber,
            FilterData filterData)
        {
            var towerTalkgroups = CreateList<TowerTalkgroup>();
            int recordCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var results = dataEntities.TowerTalkgroupsGetTalkgroupsForTowerFiltersWithPaging(systemID, towerNumber, filterData.SearchText,
                                filterData.DateFrom, filterData.DateTo, filterData.IDFrom, filterData.IDTo, filterData.FirstSeenFrom, filterData.FirstSeenTo,
                                filterData.LastSeenFrom, filterData.LastSeenTo, filterData.SortField, filterData.SortDirection, filterData.PageNumber,
                                filterData.PageSize)
                            .ToList();

                        recordCount = results.FirstOrDefault()?.RecordCount ?? 0;
                        towerTalkgroups.AddRange(results.Select(ttg => _mapperConfig.Map<TowerTalkgroup>(ttg)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting talkgroups for tower");
                    throw;
                }
            });

            return (towerTalkgroups, recordCount);
        }

        public async Task<int> GetTalkgroupsForTowerCountAsync(int systemID, int towerNumber)
        {
            int towerTalkgroupCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towerTalkgroupCount = dataEntities.TowerTalkgroupsGetTalkgroupsForTowerCount(systemID,
                            towerNumber).SingleOrDefault() ?? 0;
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting talkgroups for tower count");
                    throw;
                }
            });

            return towerTalkgroupCount;
        }

        public async Task<IEnumerable<TowerTalkgroup>> GetTalkgroupsForTowerByDateAsync(int systemID, int towerNumber)
        {
            var towerTalkgroups = CreateList<TowerTalkgroup>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towerTalkgroups.AddRange(dataEntities.TowerTalkgroupsGetTalkgroupsForTowerByDate(systemID, towerNumber)
                            .Select(ttg => _mapperConfig.Map<TowerTalkgroup>(ttg)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting talkgroups for tower by date");
                    throw;
                }
            });

            return towerTalkgroups;
        }

        public async Task<IEnumerable<TowerTalkgroup>> GetTowersForTalkgroupAsync(int systemID, int talkgroupID)
        {
            var towerTalkgroups = CreateList<TowerTalkgroup>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towerTalkgroups.AddRange(dataEntities.TowerTalkgroupsGetTowersForTalkgroup(systemID, talkgroupID)
                            .Select(ttg => _mapperConfig.Map<TowerTalkgroup>(ttg)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting towers for talkgroup");
                    throw;
                }
            });

            return towerTalkgroups;
        }

        public async Task<int> GetTowersForTalkgroupCountAsync(int systemID, int talkgroupID)
        {
            int towerTalkgroupCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towerTalkgroupCount = dataEntities.TowerTalkgroupsGetTowersForTalkgroupCount(systemID,
                            talkgroupID).SingleOrDefault() ?? 0;
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting towers for talkgroup count");
                    throw;
                }
            });

            return towerTalkgroupCount;
        }

        public async Task<(IEnumerable<TowerTalkgroup> towerTalkgroups, int recordCount)> GetTowersForTalkgroupsAsync(string systemID, int talkgroupID,
            FilterData filterData)
        {
            var towerTalkgroups = CreateList<TowerTalkgroup>();
            int recordCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var results = dataEntities.TowerTalkgroupsGetTowersForTalkgroupFiltersWithPaging(systemID,
                                talkgroupID, filterData.SearchText, filterData.DateFrom, filterData.DateTo,
                                filterData.IDFrom, filterData.IDTo, filterData.FirstSeenFrom, filterData.FirstSeenTo,
                                filterData.LastSeenFrom, filterData.LastSeenTo, filterData.SortField, filterData.SortDirection, filterData.PageNumber,
                                filterData.PageSize)
                            .ToList();

                        recordCount = results.FirstOrDefault()?.RecordCount ?? 0;
                        towerTalkgroups.AddRange(results.Select(ttg => _mapperConfig.Map<TowerTalkgroup>(ttg)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting towers for talkgroups");
                    throw;
                }
            });

            return (towerTalkgroups, recordCount);
        }

        public async Task<IEnumerable<TowerTalkgroup>> GetTowerListForTalkgroupAsync(string systemID, int talkgroupID, FilterData filterData)
        {
            var towers = CreateList<TowerTalkgroup>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towers.AddRange(dataEntities.TowerTalkgroupsGetTowerListForTalkgroup(systemID, talkgroupID, filterData.DateFrom, filterData.DateTo)
                            .Select(ttg => _mapperConfig.Map<TowerTalkgroup>(ttg)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting tower list for talkgroup");
                    throw;
                }
            });

            return towers;
        }

        public async Task<IEnumerable<DateTime>> GetDateListForTowerTalkgroupAsync(string systemID, int talkgroupID, int towerNumber, FilterData filterData)
        {
            var dates = CreateList<DateTime>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var results = dataEntities.TowerTalkgroupsGetDateListForTowerTalkgroup(systemID, talkgroupID,
                            towerNumber, filterData.DateFrom, filterData.DateTo).ToList();

                        dates.AddRange(results.Select(d => Convert.ToDateTime(d)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting date list for tower talkgroup");
                    throw;
                }
            });

            return dates;
        }
    }
}
