using DataAccessLibrary;
using DataLibrary.Interfaces;
using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLibrary.Repositories
{
    public class TowerFrequencyRepository : RepositoryBase, ITowerFrequencyRepository
    {
        public async Task<TowerFrequency> GetForFrequencyAsync(int systemID, int towerNumber, string frequency)
        {
            TowerFrequency towerFrequency = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var towerFrequencyData = dataEntities
                            .TowerFrequenciesGetForFrequency(systemID, towerNumber, frequency).SingleOrDefault();

                        if (towerFrequencyData != null)
                        {
                            towerFrequency = _mapperConfig.Map<TowerFrequency>(towerFrequencyData);
                        }
                        else
                        {
                            towerFrequency = null;
                        }
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting frequency");
                    throw;
                }
            });

            return towerFrequency;
        }

        public async Task<TowerFrequency> GetSummaryAsync(string systemID, int towerNumber, string frequency)
        {
            TowerFrequency towerFrequencySummary = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var towerFrequencySummaryData = dataEntities
                            .TowerFrequencyGetSummaryForFrequency(systemID, towerNumber, frequency).SingleOrDefault();

                        if (towerFrequencySummaryData != null)
                        {
                            towerFrequencySummary = _mapperConfig.Map<TowerFrequency>(towerFrequencySummaryData);
                        }
                        else
                        {
                            towerFrequencySummary = null;
                        }
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting frequency summary");
                    throw;
                }
            });

            return towerFrequencySummary;
        }

        public async Task<IEnumerable<TowerFrequency>> GetForTowerAsync(int systemID, int towerNumber)
        {
            var towerFrequencies = CreateList<TowerFrequency>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towerFrequencies.AddRange(dataEntities.TowerFrequenciesGetForTower(systemID, towerNumber)
                            .Select(tf => _mapperConfig.Map<TowerFrequency>(tf)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting frequencies for tower");
                    throw;
                }
            });

            return towerFrequencies;
        }

        public async Task<int> GetFrequenciesForTowerCountAsync(int systemID, int towerNumber)
        {
            int towerFrequencyCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towerFrequencyCount = dataEntities.TowerFrequenciesGetFrequenciesForTowerCount(systemID,
                            towerNumber).SingleOrDefault() ?? 0;
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting frequencies for tower count");
                    throw;
                }
            });

            return towerFrequencyCount;
        }

        public async Task<IEnumerable<TowerFrequency>> GetFrequenciesForTowerAsync(int systemID, int towerNumber)
        {
            var towerFrequencies = CreateList<TowerFrequency>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towerFrequencies.AddRange(dataEntities.TowerFrequenciesGetFrequenciesForTower(systemID, towerNumber)
                            .Select(tf => _mapperConfig.Map<TowerFrequency>(tf)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting frequencies for tower");
                    throw;
                }
            });

            return towerFrequencies;
        }

        public async Task<(IEnumerable<TowerFrequency> towerFrequencies, int recordCount)> GetFrequenciesForTowerAsync(string systemID, int towerNumber,
            FilterData filterData)
        {
            var towerFrequencies = CreateList<TowerFrequency>();
            int recordCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var results = dataEntities.TowerFrequenciesGetFrequenciesForTowerFiltersWithPaging(systemID, towerNumber, 
                                filterData.SearchText, filterData.DateFrom, filterData.DateTo, filterData.FirstSeenFrom, 
                                filterData.FirstSeenTo, filterData.LastSeenFrom, filterData.LastSeenTo, filterData.SortField, 
                                filterData.SortDirection, filterData.PageNumber, filterData.PageSize)
                            .ToList();

                        recordCount = results.FirstOrDefault()?.RecordCount ?? 0;
                        towerFrequencies.AddRange(results.Select(tf => _mapperConfig.Map<TowerFrequency>(tf)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting frequencies for tower");
                    throw;
                }
            });

            return (towerFrequencies, recordCount);
        }

        public async Task<int> GetFrequenciesForTowerAllCountAsync(int systemID, int towerNumber)
        {
            int towerFrequencyCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towerFrequencyCount = dataEntities.TowerFrequenciesGetFrequenciesForTowerAllCount(systemID,
                            towerNumber).SingleOrDefault() ?? 0;
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting all frequencies for tower count");
                    throw;
                }
            });

            return towerFrequencyCount;
        }

        public async Task<IEnumerable<TowerFrequency>> GetFrequenciesForTowerAllAsync(int systemID, int towerNumber)
        {
            var towerFrequencies = CreateList<TowerFrequency>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towerFrequencies.AddRange(dataEntities
                            .TowerFrequenciesGetFrequenciesForTowerAll(systemID, towerNumber)
                            .Select(tf => _mapperConfig.Map<TowerFrequency>(tf)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting all frequencies for tower");
                    throw;
                }
            });

            return towerFrequencies;
        }

        public async Task<(IEnumerable<TowerFrequency> towerFrequencies, int recordCount)> GetFrequenciesForTowerAllAsync(string systemID,
            int towerNumber, FilterData filterData)
        {
            var towerFrequencies = CreateList<TowerFrequency>();
            int recordCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var results = dataEntities.TowerFrequenciesGetFrequenciesForTowerAllFiltersWithPaging(systemID,
                                towerNumber, filterData.SearchText, filterData.DateFrom, filterData.DateTo, 
                                filterData.FirstSeenFrom, filterData.FirstSeenTo, filterData.LastSeenFrom,
                                filterData.LastSeenTo, filterData.SortField, filterData.SortDirection, 
                                filterData.PageNumber, filterData.PageSize)
                            .ToList();

                        recordCount = results.FirstOrDefault()?.RecordCount ?? 0;
                        towerFrequencies.AddRange(results.Select(tf => _mapperConfig.Map<TowerFrequency>(tf)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception);
                    throw;
                }
            });

            return (towerFrequencies, recordCount);
        }

        public async Task<int> GetFrequenciesForTowerNotCurrentCountAsync(int systemID, int towerNumber)
        {
            int towerFrequencyCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towerFrequencyCount = dataEntities.TowerFrequenciesGetFrequenciesForTowerNotCurrentCount(systemID, towerNumber)
                            .SingleOrDefault() ?? 0;
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting not current frequencies for tower count");
                    throw;
                }
            });

            return towerFrequencyCount;
        }

        public async Task<IEnumerable<TowerFrequency>> GetFrequenciesForTowerNotCurrentAsync(int systemID, int towerNumber)
        {
            var towerFrequencies = CreateList<TowerFrequency>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towerFrequencies.AddRange(dataEntities.TowerFrequenciesGetFrequenciesForTowerNotCurrent(systemID, towerNumber)
                            .Select(tf => _mapperConfig.Map<TowerFrequency>(tf)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting not current frequencies for tower");
                    throw;
                }
            });

            return towerFrequencies;
        }

        public async Task<(IEnumerable<TowerFrequency> towerFrequencies, int recordCount)> GetFrequenciesForTowerNotCurrentAsync(string systemID,
            int towerNumber, FilterData filterData)
        {
            var towerFrequencies = CreateList<TowerFrequency>();
            int recordCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var results = dataEntities.TowerFrequenciesGetFrequenciesForTowerNotCurrentFiltersWithPaging(systemID, towerNumber,
                                filterData.SearchText, filterData.DateFrom, filterData.DateTo, filterData.FirstSeenFrom, filterData.FirstSeenTo,
                                filterData.LastSeenFrom, filterData.LastSeenTo, filterData.SortField, filterData.SortDirection, filterData.PageNumber,
                                filterData.PageSize)
                            .ToList();

                        recordCount = results.FirstOrDefault()?.RecordCount ?? 0;
                        towerFrequencies.AddRange(results.Select(tf => _mapperConfig.Map<TowerFrequency>(tf)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting not current frequencies for tower");
                    throw;
                }
            });

            return (towerFrequencies, recordCount);
        }

        public async Task WriteAsync(TowerFrequency towerFrequency)
        {
            if (towerFrequency.IsNew)
            {
                await AddRecordAsync(towerFrequency);
            }
            else
            {
                await UpdateRecordAsync(towerFrequency);
            }
        }

        private async Task AddRecordAsync(TowerFrequency towerFrequency)
        {
            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var newRecord = dataEntities.TowerFrequencies.Create();

                        EditRecord(newRecord, towerFrequency);
                        dataEntities.TowerFrequencies.Add(newRecord);
                        dataEntities.SaveChanges();

                        towerFrequency.ID = newRecord.ID;
                        towerFrequency.IsNew = false;
                        towerFrequency.IsDirty = false;
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error adding tower frequency");
                    throw;
                }
            });
        }

        private async Task UpdateRecordAsync(TowerFrequency towerFrequency)
        {
            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var selectedRecord = dataEntities.TowerFrequenciesGet(towerFrequency.ID).SingleOrDefault();

                        if (selectedRecord == null)
                        {
                            throw new ArgumentException("Invalid tower Frequency", nameof(towerFrequency));
                        }

                        EditRecord(selectedRecord, towerFrequency);
                        dataEntities.SaveChanges();

                        towerFrequency.IsNew = false;
                        towerFrequency.IsDirty = false;
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error updating tower frequency");
                    throw;
                }
            });
        }

        public void EditRecord(TowerFrequencies databaseRecord, TowerFrequency towerFrequency)
        {
            databaseRecord.SystemID = towerFrequency.SystemID;
            databaseRecord.TowerID = towerFrequency.TowerID;
            databaseRecord.Channel = towerFrequency.Channel;
            databaseRecord.Usage = towerFrequency.Usage;
            databaseRecord.Frequency = towerFrequency.Frequency;
            databaseRecord.InputChannel = towerFrequency.InputChannel;
            databaseRecord.InputFrequency = towerFrequency.InputFrequency;
            databaseRecord.InputExplicit = towerFrequency.InputExplicit;
            databaseRecord.HitCount = towerFrequency.HitCount;
            databaseRecord.FirstSeen = towerFrequency.FirstSeen;
            databaseRecord.LastSeen = towerFrequency.LastSeen;
        }

        public async Task DeleteAsync(int id)
        {
            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        dataEntities.TowerFrequenciesDelete(id);
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error deleting record");
                    throw;
                }
            });
        }
    }
}
