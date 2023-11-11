using DataLibrary.Interfaces;
using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLibrary.Repositories
{
    public class TowerFrequencyUsageRepository : RepositoryBase, ITowerFrequencyUsageRepository
    {
        public async Task<IEnumerable<TowerFrequencyUsage>> GetFrequenciesForTowerAsync(int systemID, int towerNumber)
        {
            var towerFrequencies = CreateList<TowerFrequencyUsage>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towerFrequencies.AddRange(dataEntities
                            .TowerFrequencyUsageGetFrequenciesForTower(systemID, towerNumber)
                            .Select(tf => _mapperConfig.Map<TowerFrequencyUsage>(tf)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting frequency usage for tower");
                    throw;
                }
            });

            return towerFrequencies;
        }

        public async Task<IEnumerable<TowerFrequencyUsage>> GetFrequenciesForTowerAsync(int systemID, int towerNumber, DateTime date)
        {
            var towerFrequencies = CreateList<TowerFrequencyUsage>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towerFrequencies.AddRange(dataEntities.TowerFrequencyUsageGetFrequenciesForTowerForDate(systemID, towerNumber, date)
                            .Select(tf => _mapperConfig.Map<TowerFrequencyUsage>(tf)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting usage for tower frequencies for date");
                    throw;
                }
            });

            return towerFrequencies;
        }
    }
}
