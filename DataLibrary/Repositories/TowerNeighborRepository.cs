using DataAccessLibrary;
using DataLibrary.Interfaces;
using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLibrary.Repositories
{
    public class TowerNeighborRepository : RepositoryBase, ITowerNeighborRepository
    {
        public async Task<TowerNeighbor> GetAsync(int systemID, int towerID, int neighborSystemID, int neighborTowerNumber)
        {
            TowerNeighbor towerNeighbor = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var towerNeighborData = dataEntities
                            .TowerNeighborsGetForSystemTower(systemID, towerID, neighborSystemID, neighborTowerNumber)
                            .SingleOrDefault();

                        if (towerNeighborData != null)
                        {
                            towerNeighbor = _mapperConfig.Map<TowerNeighbor>(towerNeighborData);
                        }
                        else
                        {
                            towerNeighbor = null;
                        }
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting tower number");
                    throw;
                }
            });

            return towerNeighbor;
        }

        public async Task<TowerNeighbor> GetForSystemTowerNumberAsync(int systemID, int towerNumber, int neighborSystemID, int neighborTowerNumber)
        {
            TowerNeighbor towerNeighbor = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var towerNeighborData = dataEntities.TowerNeighborsGetForSystemTowerNumber(systemID, towerNumber, neighborSystemID, neighborTowerNumber)
                            .SingleOrDefault();

                        if (towerNeighborData != null)
                        {
                            towerNeighbor = _mapperConfig.Map<TowerNeighbor>(towerNeighborData);
                        }
                        else
                        {
                            towerNeighbor = null;
                        }
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting tower neighbor");
                    throw;
                }
            });

            return towerNeighbor;
        }

        public async Task<IEnumerable<TowerNeighbor>> GetNeighborsForTowerAsync(int systemID, int towerNumber)
        {
            var towerNeighbors = CreateList<TowerNeighbor>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towerNeighbors.AddRange(dataEntities.TowerNeighborsGetNeighborsForTower(systemID, towerNumber)
                            .Select(tn => _mapperConfig.Map<TowerNeighbor>(tn)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting tower neighbors");
                    throw;
                }
            });

            return towerNeighbors;
        }

        public async Task<(IEnumerable<TowerNeighbor> towerNeighbors, int recordCount)> GetNeighborsForTowerAsync(string systemID, int towerNumber,
            FilterData filterData)
        {
            var towerNeighbors = CreateList<TowerNeighbor>();
            int recordCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var results = dataEntities.TowerNeighborsGetNeighborsForSystemTowerFiltersWithPaging(systemID, towerNumber, filterData.SearchText,
                                filterData.IDFrom, filterData.IDTo, filterData.FirstSeenFrom, filterData.FirstSeenTo, filterData.LastSeenFrom,
                                filterData.LastSeenTo, filterData.SortField, filterData.SortDirection, filterData.PageNumber, filterData.PageSize)
                            .ToList();

                        recordCount = results.FirstOrDefault()?.RecordCount ?? 0;
                        towerNeighbors.AddRange(results.Select(tn => _mapperConfig.Map<TowerNeighbor>(tn)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting neighbors for tower");
                    throw;
                }
            });

            return (towerNeighbors, recordCount);
        }

        public async Task WriteAsync(TowerNeighbor towerNeighbor)
        {
            if (towerNeighbor.IsNew)
            {
                await AddRecordAsync(towerNeighbor);
            }
            else
            {
                await UpdateRecordAsync(towerNeighbor);
            }
        }

        private async Task AddRecordAsync(TowerNeighbor towerNeighbor)
        {
            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var newRecord = dataEntities.TowerNeighbors.Create();

                        EditRecord(newRecord, towerNeighbor);
                        dataEntities.TowerNeighbors.Add(newRecord);
                        dataEntities.SaveChanges();

                        towerNeighbor.ID = newRecord.ID;
                        towerNeighbor.IsNew = false;
                        towerNeighbor.IsDirty = false;
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error adding tower neighbor");
                    throw;
                }
            });
        }

        private async Task UpdateRecordAsync(TowerNeighbor towerNeighbor)
        {
            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var selectedRecord = dataEntities.TowerNeighborsGet(towerNeighbor.ID).SingleOrDefault();

                        if (selectedRecord == null)
                        {
                            throw new ArgumentException(@"Invalid tower number specified", nameof(towerNeighbor));
                        }

                        EditRecord(selectedRecord, towerNeighbor);
                        dataEntities.SaveChanges();

                        towerNeighbor.IsNew = false;
                        towerNeighbor.IsDirty = false;
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error updating tower neighbors");
                    throw;
                }
            });
        }

        public void EditRecord(TowerNeighbors databaseRecord, TowerNeighbor towerNeighbor)
        {
            databaseRecord.SystemID = towerNeighbor.SystemID;
            databaseRecord.TowerID = towerNeighbor.TowerNumber;
            databaseRecord.NeighborSystemID = towerNeighbor.NeighborSystemID;
            databaseRecord.NeighborTowerID = towerNeighbor.NeighborTowerID;
            databaseRecord.NeighborTowerNumberHex = towerNeighbor.NeighborTowerNumberHex;
            databaseRecord.NeighborChannel = towerNeighbor.NeighborChannel;
            databaseRecord.NeighborFrequency = towerNeighbor.NeighborFrequency;
            databaseRecord.NeighborTowerName = towerNeighbor.NeighborTowerName;
            databaseRecord.LastModified = DateTime.Now;
            databaseRecord.FirstSeen = (towerNeighbor.FirstSeen == DateTime.MinValue ? new DateTime(2000, 1, 1) : towerNeighbor.FirstSeen);
            databaseRecord.LastSeen = (towerNeighbor.LastSeen == DateTime.MinValue ? new DateTime(2000, 1, 1) : towerNeighbor.LastSeen);
        }

        public async Task DeleteForTowerAsync(int systemID, int towerNumber)
        {
            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        dataEntities.TowerNeighborsDeleteForTower(systemID, towerNumber);
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error deleting neighbors for tower");
                    throw;
                }
            });
        }
    }
}
