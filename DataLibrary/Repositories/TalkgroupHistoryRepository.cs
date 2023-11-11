using DataLibrary.Interfaces;
using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLibrary.Repositories
{
    public class TalkgroupHistoryRepository : RepositoryBase, ITalkgroupHistoryRepository
    {
        public async Task<IEnumerable<TalkgroupHistory>> GetForTalkgroupAsync(int systemID, int talkgroupID)
        {
            var talkgroupHistories = CreateList<TalkgroupHistory>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        talkgroupHistories.AddRange(dataEntities.TalkgroupHistoryGetForTalkgroup(systemID, talkgroupID)
                            .Select(tgh => _mapperConfig.Map<TalkgroupHistory>(tgh)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting talkgroup history");
                    throw;
                }
            });

            return talkgroupHistories;
        }

        public async Task<(IEnumerable<TalkgroupHistory> talkgroupHistory, int recordCount)> GetForTalkgroupAsync(string systemID, int talkgroupID,
            FilterData filterData)
        {
            var talkgroupHistory = CreateList<TalkgroupHistory>();
            int recordCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var results = dataEntities.TalkgroupHistoryGetForTalkgroupFiltersWithPaging(systemID, talkgroupID, filterData.SearchText,
                                filterData.DateFrom, filterData.DateTo, filterData.IDFrom, filterData.IDTo, filterData.FirstSeenFrom,
                                filterData.FirstSeenTo, filterData.LastSeenFrom, filterData.LastSeenTo, filterData.SortField, filterData.SortDirection,
                                filterData.PageNumber, filterData.PageSize)
                            .ToList();

                        recordCount = results.FirstOrDefault()?.RecordCount ?? 0;
                        talkgroupHistory.AddRange(results.Select(tgh => _mapperConfig.Map<TalkgroupHistory>(tgh)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting talkgroup history");
                    throw;
                }
            });

            return (talkgroupHistory, recordCount);
        }

        public async Task<int> GetForTalkgroupCountAsync(int systemID, int talkgroupID)
        {
            int talkgroupHistoryCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        talkgroupHistoryCount = dataEntities
                            .TalkgroupHistoryGetForTalkgroupCount(systemID, talkgroupID).SingleOrDefault() ?? 0;
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting talkgroup history count");
                    throw;
                }
            });

            return talkgroupHistoryCount;
        }

        public async Task<IEnumerable<TalkgroupHistory>> GetForSystemAsync(int systemID)
        {
            var talkgroupHistories = CreateList<TalkgroupHistory>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        talkgroupHistories.AddRange(dataEntities.TalkgroupHistoryGetForSystem(systemID)
                            .Select(tgh => _mapperConfig.Map<TalkgroupHistory>(tgh)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting talkgroup history for system");
                    throw;
                }
            });

            return talkgroupHistories;
        }
    }
}
