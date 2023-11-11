using DataAccessLibrary;
using DataLibrary.Interfaces;
using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLibrary.Repositories
{
    public class TowerTableRepository : RepositoryBase, ITowerTableRepository
    {
        public async Task<TowerTable> GetAsync(int systemID, int towerNumber, int tableID)
        {
            TowerTable towerTable = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var towerTableData = dataEntities
                            .TowerTablesGetForTowerTable(systemID, towerNumber, tableID).SingleOrDefault();

                        if (towerTableData != null)
                        {
                            towerTable = _mapperConfig.Map<TowerTable>(towerTableData);
                        }
                        else
                        {
                            towerTable = null;
                        }
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting tower table");
                    throw;
                }
            });

            return towerTable;
        }

        public async Task<IEnumerable<TowerTable>> GetListForTowerAsync(int systemID, int towerNumber)
        {
            var towerTables = CreateList<TowerTable>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towerTables.AddRange(dataEntities.TowerTablesGetForTower(systemID, towerNumber)
                            .Select(tt => _mapperConfig.Map<TowerTable>(tt)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting tower tables for tower");
                    throw;
                }
            });

            return towerTables;
        }

        public async Task WriteAsync(TowerTable towerTable)
        {
            if (towerTable.IsNew)
            {
                await AddRecordAsync(towerTable);
            }
            else
            {
                await UpdateRecordAsync(towerTable);
            }
        }

        private async Task AddRecordAsync(TowerTable towerTable)
        {
            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var newRecord = dataEntities.TowerTables.Create();

                        EditRecord(newRecord, towerTable);
                        dataEntities.TowerTables.Add(newRecord);
                        dataEntities.SaveChanges();

                        towerTable.ID = newRecord.ID;
                        towerTable.IsNew = false;
                        towerTable.IsDirty = false;
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error adding tower table");
                    throw;
                }
            });
        }

        private async Task UpdateRecordAsync(TowerTable towerTable)
        {
            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var selectedRecord = dataEntities.TowerTablesGet(towerTable.ID).SingleOrDefault();

                        if (selectedRecord == null)
                        {
                            throw new ArgumentException(@"Invalid tower table record", nameof(towerTable));
                        }

                        EditRecord(selectedRecord, towerTable);
                        dataEntities.SaveChanges();
                        towerTable.IsNew = false;
                        towerTable.IsDirty = false;
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error updating tower table");
                    throw;
                }
            });
        }

        public void EditRecord(TowerTables databaseRecord, TowerTable towerTable)
        {
            databaseRecord.SystemID = towerTable.SystemID;
            databaseRecord.TowerID = towerTable.TowerID;
            databaseRecord.TableID = towerTable.TableID;
            databaseRecord.BaseFrequency = towerTable.BaseFrequency;
            databaseRecord.Spacing = towerTable.Spacing;
            databaseRecord.InputOffset = towerTable.InputOffset;
            databaseRecord.AssumedConfirmed = towerTable.AssumedConfirmed;
            databaseRecord.Bandwidth = towerTable.Bandwidth;
            databaseRecord.Slots = towerTable.Slots;
        }

        public async Task DeleteForTowerAsync(int systemID, int towerNumber)
        {
            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        dataEntities.TowerTablesDeleteForTower(systemID, towerNumber);
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error deleting tower tables for tower");
                    throw;
                }
            });
        }
    }
}
