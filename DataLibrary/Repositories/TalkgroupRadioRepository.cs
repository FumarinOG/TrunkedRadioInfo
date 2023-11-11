using DataLibrary.Interfaces;
using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLibrary.Repositories
{
    public class TalkgroupRadioRepository : RepositoryBase, ITalkgroupRadioRepository
    {
        public async Task<IEnumerable<TalkgroupRadio>> GetForSystemAsync(int systemID)
        {
            var talkgroupRadios = CreateList<TalkgroupRadio>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        talkgroupRadios.AddRange(dataEntities.TalkgroupRadiosGetForSystem(systemID)
                            .Select(tgr => _mapperConfig.Map<TalkgroupRadio>(tgr)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting talkgroup radios for system");
                    throw;
                }
            });

            return talkgroupRadios;
        }

        public async Task<IEnumerable<TalkgroupRadio>> GetForSystemAsync(int systemID, DateTime date)
        {
            var talkgroupRadios = CreateList<TalkgroupRadio>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        talkgroupRadios.AddRange(dataEntities.TalkgroupRadiosGetForSystemDateRange(systemID, date)
                            .Select(tgr => _mapperConfig.Map<TalkgroupRadio>(tgr)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting talkgroup radios for system");
                    throw;
                }
            });

            return talkgroupRadios;
        }

        public async Task<IEnumerable<TalkgroupRadio>> GetTalkgroupsForRadioAsync(int systemID, int radioID)
        {
            var talkgroupRadios = CreateList<TalkgroupRadio>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        talkgroupRadios.AddRange(dataEntities
                            .TalkgroupRadiosGetTalkgroupsForRadio(systemID, radioID)
                            .Select(tgr => _mapperConfig.Map<TalkgroupRadio>(tgr)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting talkgroup radios for system");
                    throw;
                }
            });

            return talkgroupRadios;
        }

        public async Task<(IEnumerable<TalkgroupRadio> talkgroupRadios, int recordCount)> GetTalkgroupsForRadioAsync(string systemID, int radioID,
            FilterData filterData)
        {
            var talkgroupRadios = CreateList<TalkgroupRadio>();
            int recordCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var results = dataEntities.TalkgroupRadiosGetTalkgroupsForRadioFiltersWithPaging(systemID,
                                radioID, filterData.SearchText, filterData.DateFrom, filterData.DateTo, 
                                filterData.IDFrom, filterData.IDTo, filterData.FirstSeenFrom, filterData.FirstSeenTo,
                                filterData.LastSeenFrom, filterData.LastSeenTo, filterData.SortField, 
                                filterData.SortDirection, filterData.PageNumber, filterData.PageSize)
                            .ToList();

                        recordCount = results.FirstOrDefault()?.RecordCount ?? 0;
                        talkgroupRadios.AddRange(results.Select(tgr => _mapperConfig.Map<TalkgroupRadio>(tgr)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting talkgroup for radios");
                    throw;
                }
            });

            return (talkgroupRadios, recordCount);
        }

        public async Task<int> GetTalkgroupsForRadioCountAsync(int systemID, int radioID)
        {
            int talkgroupRadioCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        talkgroupRadioCount = dataEntities.TalkgroupRadiosGetTalkgroupsForRadioCount(systemID,
                            radioID).SingleOrDefault() ?? 0;
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting talkgroups for radio count");
                    throw;
                }
            });

            return talkgroupRadioCount;
        }

        public async Task<IEnumerable<TalkgroupRadio>> GetRadiosForTalkgroupAsync(int systemID, int talkgroupID)
        {
            var talkgroupRadios = CreateList<TalkgroupRadio>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        talkgroupRadios.AddRange(dataEntities.TalkgroupRadiosGetRadiosForTalkgroup(systemID,
                            talkgroupID).Select(tgr => _mapperConfig.Map<TalkgroupRadio>(tgr)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting radios for talkgroup");
                    throw;
                }
            });

            return talkgroupRadios;
        }

        public async Task<int> GetRadiosForTalkgroupCountAsync(int systemID, int talkgroupID)
        {
            int talkgroupRadioCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        talkgroupRadioCount = dataEntities.TalkgroupRadiosGetRadiosForTalkgroupCount(systemID,
                            talkgroupID).SingleOrDefault() ?? 0;
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting radios for talkgroup count");
                    throw;
                }
            });

            return talkgroupRadioCount;
        }

        public async Task<(IEnumerable<TalkgroupRadio> talkgroupRadios, int recordCount)> GetRadiosForTalkgroupAsync(string systemID, int talkgroupID,
            FilterData filterData)
        {
            var talkgroupRadios = CreateList<TalkgroupRadio>();
            int recordCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var results = dataEntities.TalkgroupRadiosGetRadiosForTalkgroupFiltersWithPaging(systemID,
                                talkgroupID, filterData.SearchText, filterData.DateFrom, filterData.DateTo, 
                                filterData.IDFrom, filterData.IDTo, filterData.FirstSeenFrom, filterData.FirstSeenTo,
                                filterData.LastSeenFrom, filterData.LastSeenTo, filterData.SortField,
                                filterData.SortDirection, filterData.PageNumber, filterData.PageSize)
                            .ToList();

                        recordCount = results.FirstOrDefault()?.RecordCount ?? 0;
                        talkgroupRadios.AddRange(results.Select(tgr => _mapperConfig.Map<TalkgroupRadio>(tgr)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting radios for talkgroup");
                    throw;
                }
            });

            return (talkgroupRadios, recordCount);
        }

        public async Task<IEnumerable<TalkgroupRadio>> GetRadiosForTalkgroupWithDatesAsync(int systemID, int talkgroupID)
        {
            var talkgroupRadios = CreateList<TalkgroupRadio>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        talkgroupRadios.AddRange(dataEntities.TalkgroupRadiosGetRadiosForTalkgroupWithDates(systemID,
                            talkgroupID).Select(tgr => _mapperConfig.Map<TalkgroupRadio>(tgr)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting radios for talkgroups with dates");
                    throw;
                }
            });

            return talkgroupRadios;
        }
    }
}
