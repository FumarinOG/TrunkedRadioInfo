using DataAccessLibrary;
using DataLibrary.Interfaces;
using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLibrary.Repositories
{
    public class TowerRepository : RepositoryBase, ITowerRepository
    {
        public async Task<Tower> GetAsync(int id)
        {
            Tower tower = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var towerData = dataEntities.TowersGet(id).SingleOrDefault();

                        if (towerData != null)
                        {
                            tower = _mapperConfig.Map<Tower>(towerData);
                        }
                        else
                        {
                            tower = null;
                        }
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting tower");
                    throw;
                }
            });

            return tower;
        }

        public async Task<Tower> GetAsync(int systemID, int towerNumber)
        {
            Tower tower = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var towerData = dataEntities.TowersGetForSystemTower(systemID, towerNumber).SingleOrDefault();

                        if (towerData != null)
                        {
                            tower = _mapperConfig.Map<Tower>(towerData);
                        }
                        else
                        {
                            tower = null;
                        }
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting tower");
                    throw;
                }
            });

            return tower;
        }

        public async Task<IEnumerable<Tower>> GetForSystemAsync(int systemID)
        {
            var towers = CreateList<Tower>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towers.AddRange(dataEntities.TowersGetForSystem(systemID)
                            .Select(t => _mapperConfig.Map<Tower>(t)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting towers for system");
                    throw;
                }
            });

            return towers;
        }

        public async Task<(IEnumerable<Tower> towers, int recordCount)> GetForSystemAsync(string systemID, bool activeOnly, FilterData filterData)
        {
            var towers = CreateList<Tower>();
            int recordCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        if (activeOnly)
                        {
                            var results = dataEntities.TowersGetForSystemActiveFiltersWithPaging(systemID,
                                    filterData.SearchText, filterData.DateFrom, filterData.DateTo,
                                    filterData.IDFrom, filterData.IDTo, filterData.FirstSeenFrom,
                                    filterData.FirstSeenTo, filterData.LastSeenFrom, filterData.LastSeenTo,
                                    filterData.SortField, filterData.SortDirection, filterData.PageNumber,
                                    filterData.PageSize)
                                .ToList();

                            recordCount = results.FirstOrDefault()?.RecordCount ?? 0;
                            towers.AddRange(results.Select(t => _mapperConfig.Map<Tower>(t)));
                        }
                        else
                        {
                            var results = dataEntities.TowersGetForSystemFiltersWithPaging(systemID,
                                    filterData.SearchText, filterData.DateFrom, filterData.DateTo, filterData.IDFrom,
                                    filterData.IDTo, filterData.FirstSeenFrom, filterData.FirstSeenTo, filterData.LastSeenFrom, filterData.LastSeenTo, filterData.SortField,
                                    filterData.SortDirection, filterData.PageNumber, filterData.PageSize)
                                .ToList();

                            recordCount = results.FirstOrDefault()?.RecordCount ?? 0;
                            towers.AddRange(results.Select(t => _mapperConfig.Map<Tower>(t)));
                        }
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting towers for system");
                    throw;
                }
            });

            return (towers, recordCount);
        }

        public async Task<int> GetCountForSystemAsync(int systemID)
        {
            int towerCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towerCount = dataEntities.TowersGetCountForSystem(systemID).SingleOrDefault() ?? 0;
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting tower count for system");
                    throw;
                }
            });

            return towerCount;
        }

        public async Task WriteAsync(Tower tower)
        {
            if (tower.IsNew)
            {
                await AddRecordAsync(tower);
            }
            else
            {
                await UpdateRecordAsync(tower);
            }
        }

        private async Task AddRecordAsync(Tower tower)
        {
            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var newRecord = dataEntities.Towers.Create();

                        EditRecord(newRecord, tower);
                        dataEntities.Towers.Add(newRecord);
                        dataEntities.SaveChanges();

                        tower.ID = newRecord.ID;
                        tower.IsNew = false;
                        tower.IsDirty = false;
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error adding tower");
                    throw;
                }
            });
        }

        private async Task UpdateRecordAsync(Tower tower)
        {
            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var selectedRecord = dataEntities.TowersGet(tower.ID).SingleOrDefault();

                        if (selectedRecord == null)
                        {
                            throw new ArgumentException(@"Invalid tower record", nameof(tower));
                        }

                        EditRecord(selectedRecord, tower);
                        dataEntities.SaveChanges();

                        tower.IsNew = false;
                        tower.IsDirty = false;
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error updating tower");
                    throw;
                }
            });
        }

        public void EditRecord(Towers databaseRecord, Tower tower)
        {
            databaseRecord.SystemID = tower.SystemID;
            databaseRecord.TowerNumber = tower.TowerNumber;
            databaseRecord.TowerNumberHex = tower.TowerNumberHex;
            databaseRecord.Description = tower.Description;
            databaseRecord.HitCount = tower.HitCount;
            databaseRecord.WACN = tower.WACN;
            databaseRecord.ControlCapabilities = tower.ControlCapabilities;
            databaseRecord.Flavor = tower.Flavor;
            databaseRecord.CallSigns = tower.CallSigns;
            databaseRecord.TimeStamp = tower.TimeStamp;
            databaseRecord.FirstSeen = (tower.FirstSeen == DateTime.MinValue ? new DateTime(2000, 1, 1) : tower.FirstSeen);
            databaseRecord.LastSeen = (tower.LastSeen == DateTime.MinValue ? new DateTime(2000, 1, 1) : tower.LastSeen);
            databaseRecord.LastModified = DateTime.Now;
        }
    }
}
