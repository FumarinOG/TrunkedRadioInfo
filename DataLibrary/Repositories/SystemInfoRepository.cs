using DataAccessLibrary;
using DataLibrary.Interfaces;
using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLibrary.Repositories
{
    public class SystemInfoRepository : RepositoryBase, ISystemInfoRepository
    {
        public async Task<SystemInfo> GetAsync(int id)
        {
            SystemInfo systemInfo = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var systemInfoData = dataEntities.SystemsGet(id).SingleOrDefault();

                        if (systemInfoData != null)
                        {
                            systemInfo = _mapperConfig.Map<SystemInfo>(systemInfoData);
                        }
                        else
                        {
                            systemInfo = null;
                        }
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting system info");
                    throw;
                }
            });

            return systemInfo;
        }

        public async Task<SystemInfo> GetAsync(string systemID)
        {
            SystemInfo systemInfo = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var systemInfoData = dataEntities.SystemsGetForSystem(systemID).SingleOrDefault();

                        if (systemInfoData != null)
                        {
                            systemInfo = _mapperConfig.Map<SystemInfo>(systemInfoData);
                        }
                        else
                        {
                            systemInfo = null;
                        }
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting system info");
                    throw;
                }
            });

            return systemInfo;
        }

        public async Task<int> GetSystemIDAsync(string systemIDString)
        {
            int systemID = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var systemIDData = dataEntities.SystemsGetID(systemIDString).SingleOrDefault();

                        if (systemIDData != null)
                        {
                            systemID = (int)systemIDData;
                        }
                        else
                        {
                            throw new ArgumentException("Invalid system ID", nameof(systemID));
                        }
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting system ID");
                    throw;
                }
            });

            return systemID;
        }

        public async Task<IEnumerable<SystemInfo>> GetListAsync()
        {
            var systems = CreateList<SystemInfo>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        systems.AddRange(dataEntities.SystemsGetList().Select(s => _mapperConfig.Map<SystemInfo>(s)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting system list");
                    throw;
                }
            });

            return systems;
        }

        public async Task<IEnumerable<SystemInfo>> GetListAsync(FilterData filterData)
        {
            var systems = CreateList<SystemInfo>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        systems.AddRange(dataEntities
                            .SystemsGetListFilters(filterData.SearchText, filterData.FirstSeenFrom, filterData.FirstSeenTo, filterData.LastSeenFrom,
                                filterData.LastSeenTo, filterData.SortField, filterData.SortDirection)
                            .Select(s => _mapperConfig.Map<SystemInfo>(s)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting system list");
                    throw;
                }
            });

            return systems;
        }

        public async Task<int> GetCountAsync()
        {
            int systemCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        systemCount = dataEntities.SystemsGetCount().SingleOrDefault() ?? 0;
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting system list");
                    throw;
                }
            });

            return systemCount;
        }

        public async Task WriteAsync(SystemInfo system)
        {
            if (system.IsNew)
            {
                await AddRecordAsync(system);
            }
            else
            {
                await UpdateRecordAsync(system);
            }
        }

        private async Task AddRecordAsync(SystemInfo system)
        {
            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var newRecord = dataEntities.Systems.Create();

                        EditRecord(newRecord, system);
                        dataEntities.Systems.Add(newRecord);
                        dataEntities.SaveChanges();

                        system.ID = newRecord.ID;
                        system.IsNew = false;
                        system.IsDirty = false;
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error adding system");
                    throw;
                }
            });
        }

        private async Task UpdateRecordAsync(SystemInfo system)
        {
            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var selectedRecord = dataEntities.SystemsGet(system.ID).SingleOrDefault();

                        if (selectedRecord == null)
                        {
                            throw new ArgumentException(@"Invalid system record", nameof(system));
                        }

                        dataEntities.SaveChanges();

                        EditRecord(selectedRecord, system);
                        system.IsNew = false;
                        system.IsDirty = false;
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error updating system");
                    throw;
                }
            });
        }

        public void EditRecord(Systems databaseRecord, SystemInfo systemInfo)
        {
            databaseRecord.SystemID = systemInfo.SystemID;
            databaseRecord.SystemIDDecimal = systemInfo.SystemIDDecimal;
            databaseRecord.Description = systemInfo.Description;
            databaseRecord.WACN = systemInfo.WACN;

            if (systemInfo.FirstSeen == DateTime.MinValue)
            {
                databaseRecord.FirstSeen = DateTime.Now;
            }
            else
            {
                databaseRecord.FirstSeen = systemInfo.FirstSeen;
            }

            if (systemInfo.LastSeen == DateTime.MinValue)
            {
                databaseRecord.LastSeen = DateTime.Now;
            }    
            else
            {
                databaseRecord.LastSeen = systemInfo.LastSeen;
            }

            databaseRecord.LastModified = DateTime.Now;
            
            //databaseRecord.ValidFrom = DateTime.Now;
            //databaseRecord.ValidTo = DateTime.Now;
        }
    }
}
