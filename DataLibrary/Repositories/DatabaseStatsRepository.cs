using DataLibrary.Interfaces;
using ObjectLibrary;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DataLibrary.Repositories
{
    public class DatabaseStatsRepository : RepositoryBase, IDatabaseStatsRepository
    {
        public async Task<DatabaseStat> GetDatabaseStatsAsync()
        {
            DatabaseStat databaseStats = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var stats = dataEntities.f_DatabaseGetStats().SingleOrDefault();

                        if (stats != null)
                        {
                            databaseStats = _mapperConfig.Map<DatabaseStat>(stats);
                        }
                        else
                        {
                            databaseStats = null;
                        }
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting database stats");
                    throw;
                }
            });

            return databaseStats;
        }
    }
}
