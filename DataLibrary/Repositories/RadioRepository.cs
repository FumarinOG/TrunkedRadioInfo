using DataLibrary.Interfaces;
using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLibrary.Repositories
{
    public class RadioRepository : RepositoryBase, IRadioRepository
    {
        public async Task<IEnumerable<Radio>> GetListForSystemAsync(int systemID)
        {
            var radios = CreateList<Radio>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        radios.AddRange(dataEntities.RadiosGetForSystem(systemID).Select(r => _mapperConfig.Map<Radio>(r)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting radios for system");
                    throw;
                }
            });

            return radios;
        }

        public async Task<IEnumerable<Radio>> GetDetailForSystemAsync(int systemID)
        {
            var radios = CreateList<Radio>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        radios.AddRange(dataEntities.RadiosGetDetailForSystem(systemID)
                            .Select(r => _mapperConfig.Map<Radio>(r)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting radio details for system");
                    throw;
                }
            });

            return radios;
        }

        public async Task<Radio> GetDetailAsync(int systemID, int radioID)
        {
            Radio radio = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var radioData = dataEntities.RadiosGetDetail(systemID, radioID).SingleOrDefault();

                        if (radioData == null)
                        {
                            throw new ArgumentException("Invalid radio", nameof(radioID));
                        }

                        radio = _mapperConfig.Map<Radio>(radioData);
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting detail for radio");
                    throw;
                }
            });

            return radio;
        }

        public async Task<Radio> GetDetailAsync(int systemID, int radioID, DateTime? dateFrom, DateTime? dateTo)
        {
            Radio radio = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var radioData = dataEntities.RadiosGetDetailFilters(systemID, radioID, dateFrom, dateTo)
                            .SingleOrDefault();

                        if (radioData == null)
                        {
                            throw new ArgumentException("Invalid radio", nameof(radioID));
                        }

                        radio = _mapperConfig.Map<Radio>(radioData);
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting detail for radio date range");
                    throw;
                }
            });

            return radio;
        }

        public async Task<(IEnumerable<Radio> radios, int recordCount)> GetDetailForSystemAsync(string systemID, bool activeOnly, string sortField,
            string sortDirection, int pageNumber, int pageSize)
        {
            var radios = CreateList<Radio>();
            int recordCount = default;

            await Task.Run(() =>
            {
                try
                {
                    var dataEntities = CreateEntities();

                    if (activeOnly)
                    {
                        var results = dataEntities.RadiosGetDetailForSystemActiveWithPaging(systemID, sortField, sortDirection, pageNumber, pageSize).ToList();

                        recordCount = results.FirstOrDefault()?.RecordCount ?? 0;
                        radios.AddRange(results.Select(r => _mapperConfig.Map<Radio>(r)));
                    }
                    else
                    {
                        var results = dataEntities.RadiosGetDetailForSystemWithPaging(systemID, sortField, sortDirection, pageNumber, pageSize).ToList();

                        recordCount = results.FirstOrDefault()?.RecordCount ?? 0;
                        radios.AddRange(results.Select(r => _mapperConfig.Map<Radio>(r)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error get radio details for system");
                    throw;
                }
            });

            return (radios, recordCount);
        }

        public async Task<(IEnumerable<Radio> radios, int recordCount)> GetDetailForSystemAsync(string systemID, bool activeOnly, FilterData filterData)
        {
            var radios = CreateList<Radio>();
            int radioCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        if (activeOnly)
                        {
                            var results = dataEntities.RadiosGetDetailForSystemActiveFiltersWithPaging(systemID, filterData.SearchText, filterData.DateFrom,
                                    filterData.DateTo, filterData.IDFrom, filterData.IDTo, filterData.FirstSeenFrom, filterData.FirstSeenTo, filterData.LastSeenFrom,
                                    filterData.LastSeenTo, filterData.SortField, filterData.SortDirection, filterData.PageNumber, filterData.PageSize)
                                .ToList();

                            radioCount = results.FirstOrDefault()?.RecordCount ?? 0;
                            radios.AddRange(results.Select(r => _mapperConfig.Map<Radio>(r)));
                        }
                        else
                        {
                            var results = dataEntities.RadiosGetDetailForSystemFiltersWithPaging(systemID, filterData.SearchText, filterData.DateFrom, filterData.DateTo,
                                    filterData.IDFrom, filterData.IDTo, filterData.FirstSeenFrom, filterData.FirstSeenTo, filterData.LastSeenFrom, filterData.LastSeenTo,
                                    filterData.SortField, filterData.SortDirection, filterData.PageNumber, filterData.PageSize)
                                .ToList();

                            radioCount = results.FirstOrDefault()?.RecordCount ?? 0;
                            radios.AddRange(results.Select(r => _mapperConfig.Map<Radio>(r)));
                        }
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting radio details for system");
                    throw;
                }
            });

            return (radios, radioCount);
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
                        recordCount = dataEntities.RadiosGetCountForSystem(systemID).SingleOrDefault() ?? 0;
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting radio count for system");
                    throw;
                }
            });

            return recordCount;
        }

        public async Task<(IEnumerable<(int radioID, string name)> names, int recordCount)> GetNamesAsync(string systemID, FilterData filterData)
        {
            var names = CreateList<(int radioID, string name)>();
            int recordCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var results = dataEntities.RadiosGetSystemNames(systemID, filterData.SearchText, filterData.IDFrom, filterData.IDTo,
                                filterData.SortField, filterData.SortDirection, filterData.PageNumber, filterData.PageSize)
                            .ToList();

                        recordCount = results.FirstOrDefault()?.RecordCount ?? 0;
                        names.AddRange(results.Select(rn => _mapperConfig.Map<(int radioID, string name)>(rn)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting radio names");
                    throw;
                }
            });

            return (names, recordCount);
        }
    }
}
