using DataLibrary.Interfaces;
using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLibrary.Repositories
{
    public class TalkgroupRepository : RepositoryBase, ITalkgroupRepository
    {
        public async Task<IEnumerable<Talkgroup>> GetForSystemAsync(int systemID)
        {
            var talkgroups = CreateList<Talkgroup>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        talkgroups.AddRange(dataEntities.TalkgroupsGetForSystem(systemID)
                            .Select(tg => _mapperConfig.Map<Talkgroup>(tg)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting talkgroups for system");
                    throw;
                }
            });

            return talkgroups;
        }

        public async Task<Talkgroup> GetDetailAsync(int systemID, int talkgroupID)
        {
            Talkgroup talkgroup = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var talkgroupData = dataEntities.TalkgroupsGetDetail(systemID, talkgroupID).SingleOrDefault();

                        if (talkgroupData != null)
                        {
                            talkgroup = _mapperConfig.Map<Talkgroup>(talkgroupData);
                        }
                        else
                        {
                            throw new ArgumentException("Invalid talkgroup", nameof(talkgroupID));
                        }
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting detail for talkgroup");
                    throw;
                }
            });

            return talkgroup;
        }

        public async Task<Talkgroup> GetDetailAsync(int systemID, int talkgroupID, DateTime? dateFrom, DateTime? dateTo)
        {
            Talkgroup talkgroup = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var talkgroupData = dataEntities.TalkgroupsGetDetailFilters(systemID, talkgroupID, dateFrom,
                            dateTo).SingleOrDefault();

                        if (talkgroupData != null)
                        {
                            talkgroup = _mapperConfig.Map<Talkgroup>(talkgroupData);
                        }
                        else
                        {
                            throw new ArgumentException("Invalid talkgroup", nameof(talkgroupID));
                        }
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting detail for talkgroup date range");
                    throw;
                }
            });

            return talkgroup;
        }

        public async Task<IEnumerable<Talkgroup>> GetDetailForSystemAsync(int systemID)
        {
            var talkgroups = CreateList<Talkgroup>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        talkgroups.AddRange(dataEntities.TalkgroupsGetDetailForSystem(systemID).Select(tg =>
                            _mapperConfig.Map<Talkgroup>(tg)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting talkgroup detail for system");
                    throw;
                }
            });

            return talkgroups;
        }

        public async Task<(IEnumerable<Talkgroup> talkgroups, int recordCount)> GetDetailForSystemAsync(string systemID, bool activeOnly,
            FilterData filterData)
        {
            var talkgroups = CreateList<Talkgroup>();
            int recordCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        if (activeOnly)
                        {
                            var results = dataEntities.TalkgroupsGetDetailForSystemActiveFiltersWithPaging(systemID,
                                    filterData.SearchText, filterData.DateFrom, filterData.DateTo, filterData.IDFrom,
                                    filterData.IDTo, filterData.FirstSeenFrom, filterData.FirstSeenTo,
                                    filterData.LastSeenFrom, filterData.LastSeenTo, filterData.SortField,
                                    filterData.SortDirection, filterData.PageNumber, filterData.PageSize)
                                .ToList();

                            recordCount = results.FirstOrDefault()?.RecordCount ?? 0;
                            talkgroups.AddRange(results.Select(tg => _mapperConfig.Map<Talkgroup>(tg)));
                        }
                        else
                        {
                            var results = dataEntities.TalkgroupsGetDetailForSystemFiltersWithPaging(systemID,
                                    filterData.SearchText, filterData.DateFrom, filterData.DateTo, filterData.IDFrom,
                                    filterData.IDTo, filterData.FirstSeenFrom, filterData.FirstSeenTo,
                                    filterData.LastSeenFrom, filterData.LastSeenTo, filterData.SortField,
                                    filterData.SortDirection, filterData.PageNumber, filterData.PageSize)
                                .ToList();

                            recordCount = results.FirstOrDefault()?.RecordCount ?? 0;
                            talkgroups.AddRange(results.Select(tg => _mapperConfig.Map<Talkgroup>(tg)));
                        }
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting talkgroup detail for system");
                    throw;
                }
            });

            return (talkgroups, recordCount);
        }

        public async Task<(IEnumerable<Talkgroup> talkgroups, int recordCount)> GetDetailForSystemUnknownAsync(string systemID, FilterData filterData)
        {
            var talkgroups = CreateList<Talkgroup>();
            int recordCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var results = dataEntities.TalkgroupsGetDetailForSystemUnknownFiltersWithPaging(systemID,
                                filterData.SearchText, filterData.DateFrom, filterData.DateTo, filterData.IDFrom, 
                                filterData.IDTo, filterData.FirstSeenFrom, filterData.FirstSeenTo,
                                filterData.LastSeenFrom, filterData.LastSeenTo, filterData.SortField,
                                filterData.SortDirection, filterData.PageNumber, filterData.PageSize)
                            .ToList();

                        recordCount = results.FirstOrDefault()?.RecordCount ?? 0;
                        talkgroups.AddRange(results.Select(tg => _mapperConfig.Map<Talkgroup>(tg)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting talkgroup detail for system");
                    throw;
                }
            });

            return (talkgroups, recordCount);
        }

        public async Task<int> GetCountForSystemAsync(int systemID)
        {
            int recordCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        recordCount = dataEntities.TalkgroupsGetCountForSystem(systemID).SingleOrDefault() ?? 0;
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting talkgroups for system count");
                    throw;
                }
            });

            return recordCount;
        }
    }
}
