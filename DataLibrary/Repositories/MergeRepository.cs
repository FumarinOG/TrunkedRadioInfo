using DataLibrary.Interfaces;
using System;
using System.Threading.Tasks;

namespace DataLibrary.Repositories
{
    public sealed class MergeRepository : RepositoryBase, IMergeRepository
    {
        public async Task MergeRecordsAsync(Guid sessionID) =>
            await Task.Run(() =>
                {
                    try
                    {
                        using (var dataEntities = CreateEntities())
                        {
                            dataEntities.Database.CommandTimeout = 1800;
                            dataEntities.MergeRecords(sessionID);
                        }
                    }
                    catch (Exception exception)
                    {
                        _logger.Error(exception, "Error merging records");
                        throw;
                    }
                });

        public async Task DeleteTempTablesAsync(Guid sessionID) => 
            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        dataEntities.MergeDeleteTempTables(sessionID);
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error deleting temp tables");
                    throw;
                }
            });
    }
}
