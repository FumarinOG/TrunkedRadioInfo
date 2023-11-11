using DataLibrary.Interfaces;
using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLibrary.Repositories
{
    public class PatchRepository : RepositoryBase, IPatchRepository
    {
        public async Task<Patch> GetAsync(int id)
        {
            Patch patch = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var patchData = dataEntities.PatchesGet(id).SingleOrDefault();

                        if (patchData != null)
                        {
                            patch = _mapperConfig.Map<Patch>(patchData);
                        }
                        else
                        {
                            patch = null;
                        }
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting patch");
                    throw;
                }
            });

            return patch;
        }

        public async Task<IEnumerable<Patch>> GetForSystemAsync(int systemID)
        {
            var patches = CreateList<Patch>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        patches.AddRange(dataEntities
                            .PatchesGetForSystem(systemID).Select(p => _mapperConfig.Map<Patch>(p)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting patch for system");
                    throw;
                }
            });

            return patches;
        }

        public async Task<Patch> GetSummaryAsync(int systemID, int fromTalkgroupID, int toTalkgroupID)
        {
            Patch patch = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        patch = _mapperConfig.Map<Patch>(dataEntities
                            .PatchesGetSummary(systemID, fromTalkgroupID, toTalkgroupID).SingleOrDefault());
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting patch summary");
                    throw;
                }
            });

            return patch;
        }

        public async Task<(IEnumerable<Patch> patches, int recordCount)> GetSummaryAsync(string systemID, int fromTalkgroupID, int toTalkgroupID, FilterData filterData)
        {
            var patches = CreateList<Patch>();
            int recordCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var results = dataEntities.PatchesGetSummaryForSystemFiltersWithPaging(systemID, filterData.SearchText, filterData.DateFrom, filterData.DateTo,
                                fromTalkgroupID, toTalkgroupID, filterData.FirstSeenFrom, filterData.FirstSeenTo, filterData.LastSeenFrom, filterData.LastSeenTo,
                                filterData.SortField, filterData.SortDirection, filterData.PageNumber, filterData.PageSize)
                            .ToList();


                        recordCount = results.FirstOrDefault()?.RecordCount ?? 0;
                        patches.AddRange(results.Select(p => _mapperConfig.Map<Patch>(p)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting patch summary for system from and to talkgroup");
                    throw;
                }
            });

            return (patches, recordCount);
        }

        public async Task<IEnumerable<Patch>> GetSummaryForSystemAsync(int systemID)
        {
            var patches = CreateList<Patch>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        patches.AddRange(dataEntities.PatchesGetSummaryForSystem(systemID)
                            .Select(p => _mapperConfig.Map<Patch>(p)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting patch summary for system");
                    throw;
                }
            });

            return patches;
        }

        public async Task<(IEnumerable<Patch> patches, int recordCount)> GetSummaryForSystemAsync(string systemID, FilterData filterData)
        {
            var patches = CreateList<Patch>();
            int recordCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var results = dataEntities.PatchesGetSummaryForSystemFiltersWithPaging(systemID, filterData.SearchText, filterData.DateFrom,
                                filterData.DateTo, filterData.IDFrom, filterData.IDTo, filterData.FirstSeenFrom, filterData.FirstSeenTo,
                                filterData.LastSeenFrom, filterData.LastSeenTo, filterData.SortField, filterData.SortDirection, filterData.PageNumber,
                                filterData.PageSize)
                            .ToList();

                        recordCount = results.FirstOrDefault()?.RecordCount ?? 0;
                        patches.AddRange(results.Select(p => _mapperConfig.Map<Patch>(p)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting patch summary for system");
                    throw;
                }
            });

            return (patches, recordCount);
        }

        public async Task<IEnumerable<Patch>> GetForPatchByDateAsync(int systemID, int fromTalkgroupID, int toTalkgroupID)
        {
            var patches = CreateList<Patch>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        patches.AddRange(dataEntities.PatchesGetForPatchByDate(systemID, fromTalkgroupID, toTalkgroupID)
                            .Select(p => _mapperConfig.Map<Patch>(p)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting patches by date");
                    throw;
                }
            });

            return patches;
        }

        public async Task<IEnumerable<Patch>> GetForPatchByDateAsync(int systemID, int fromTalkgroupID, int toTalkgroupID, int pageNumber, int pageSize)
        {
            var patches = CreateList<Patch>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        patches.AddRange(dataEntities.PatchesGetForPatchByDateWithPaging(systemID, fromTalkgroupID, toTalkgroupID, pageNumber, pageSize)
                            .Select(p => _mapperConfig.Map<Patch>(p)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting patches by date");
                    throw;
                }
            });

            return patches;
        }

        public async Task<int> GetForPatchByDateCountAsync(int systemID, int fromTalkgroupID, int toTalkgropupID)
        {
            int patchCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntiites = CreateEntities())
                    {
                        return dataEntiites.PatchesGetForPatchByDateCount(systemID, fromTalkgroupID, toTalkgropupID)
                            .SingleOrDefault() ?? 0;
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting patch count by dates");
                    throw;
                }
            });

            return patchCount;
        }

        public async Task<IEnumerable<Patch>> GetForSystemFromTalkgroupAsync(int systemID, int fromTalkgroupID)
        {
            var patches = CreateList<Patch>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        patches.AddRange(dataEntities.PatchesGetForSystemFromTalkgroupID(systemID, fromTalkgroupID)
                            .Select(p => _mapperConfig.Map<Patch>(p)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting patches for system 'from' talkgroup");
                    throw;
                }
            });

            return patches;
        }

        public async Task<IEnumerable<Patch>> GetForSystemToTalkgroupAsync(int systemID, int toTalkgroupID)
        {
            var patches = CreateList<Patch>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        patches.AddRange(dataEntities.PatchesGetForSystemToTalkgroupID(systemID, toTalkgroupID)
                            .Select(p => _mapperConfig.Map<Patch>(p)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting patches for system 'to' talkgroup");
                    throw;
                }
            });

            return patches;
        }

        public async Task<IEnumerable<Patch>> GetForSystemTalkgroupAsync(int systemID, int talkgroupID)
        {
            var patches = CreateList<Patch>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        patches.AddRange(dataEntities.PatchesGetForSystemTalkgroup(systemID, talkgroupID)
                            .Select(p => _mapperConfig.Map<Patch>(p)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting patches for system talkgroup");
                    throw;
                }
            });

            return patches;
        }

        public async Task<int> GetForSystemTalkgroupCountAsync(int systemID, int talkgroupID)
        {
            int patchCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        return dataEntities.PatchesGetForSystemTalkgroupCount(systemID, talkgroupID).SingleOrDefault() ?? 0;
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting patches for system talkgroup count");
                    throw;
                }
            });

            return patchCount;
        }

        public async Task<(IEnumerable<Patch> patches, int recordCount)> GetForSystemTalkgroupAsync(string systemID, int talkgroupID, FilterData filterData)
        {
            var patches = CreateList<Patch>();
            int recordCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var results = dataEntities.PatchesGetForSystemTalkgroupFiltersWithPaging(systemID, talkgroupID,
                                filterData.SearchText, filterData.DateFrom, filterData.DateTo, filterData.IDFrom,
                                filterData.IDTo, filterData.FirstSeenFrom, filterData.FirstSeenTo,
                                filterData.LastSeenFrom, filterData.LastSeenTo, filterData.SortField,
                                filterData.SortDirection, filterData.PageNumber, filterData.PageSize)
                            .ToList();

                        recordCount = results.FirstOrDefault()?.RecordCount ?? 0;
                        patches.AddRange(results.Select(p => _mapperConfig.Map<Patch>(p)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting patches for system talkgroup");
                    throw;
                }
            });

            return (patches, recordCount);
        }

        public async Task<IEnumerable<Patch>> GetForSystemTowerAsync(int systemID, int towerNumber)
        {
            var patches = CreateList<Patch>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        patches.AddRange(dataEntities.PatchesGetForSystemTower(systemID, towerNumber)
                            .Select(p => _mapperConfig.Map<Patch>(p)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting patches for system tower");
                    throw;
                }
            });

            return patches;
        }

        public async Task<(IEnumerable<Patch> patches, int recordCount)> GetForSystemTowerAsync(string systemID, int towerNumber, FilterData filterData)
        {
            var patches = CreateList<Patch>();
            int recordCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var results = dataEntities.PatchesGetForSystemTowerFiltersWithPaging(systemID, towerNumber,
                                filterData.SearchText, filterData.DateFrom, filterData.DateTo, filterData.IDFrom,
                                filterData.IDTo, filterData.FirstSeenFrom, filterData.FirstSeenTo, filterData.LastSeenFrom,
                                filterData.LastSeenTo, filterData.SortField, filterData.SortDirection, filterData.PageNumber, filterData.PageSize)
                            .ToList();

                        recordCount = results.FirstOrDefault()?.RecordCount ?? 0;
                        patches.AddRange(results.Select(p => _mapperConfig.Map<Patch>(p)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting patches for system tower");
                    throw;
                }
            });

            return (patches, recordCount);
        }

        public async Task<int> GetCountForSystemAsync(int systemID)
        {
            int patchCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        patchCount = dataEntities.PatchesGetCountForSystem(systemID).SingleOrDefault() ?? 0;
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting patches for system count");
                    throw;
                }
            });

            return patchCount;
        }

        public async Task<int> GetCountForSystemAsync(int systemID, string searchText)
        {
            int patchCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        patchCount = dataEntities.PatchesGetCountForSystemSearch(systemID, searchText)
                            .SingleOrDefault() ?? 0;
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting patches for system search count");
                    throw;
                }
            });

            return patchCount;
        }
    }
}
