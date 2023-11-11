using DataLibrary.Interfaces;
using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLibrary.Repositories
{
    public class TowerFrequencyTalkgroupRepository : RepositoryBase, ITowerFrequencyTalkgroupRepository
    {
        public async Task<IEnumerable<TowerFrequencyTalkgroup>> GetForTowerAsync(int systemID, int towerNumber, DateTime date)
        {
            var towerFrequencyRadios = CreateList<TowerFrequencyTalkgroup>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        towerFrequencyRadios.AddRange(dataEntities
                            .TowerFrequencyTalkgroupsGetForTowerDate(systemID, towerNumber, date)
                            .Select(tfr => _mapperConfig.Map<TowerFrequencyTalkgroup>(tfr)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting tower frequency talkgroup list");
                    throw;
                }
            });

            return towerFrequencyRadios;
        }

        public async Task<(IEnumerable<TowerFrequencyTalkgroup> towerFrequencyTalkgroups, int recordCount)> GetTalkgroupsForTowerFrequencyAsync(
            string systemID, int towerNumber, string frequency, FilterData filterData)
        {
            var towerFrequencyTalkgroups = CreateList<TowerFrequencyTalkgroup>();
            int recordCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var results = dataEntities.TowerFrequencyTalkgroupsGetTalkgroupsForFrequenciesWithPaging(systemID, towerNumber, frequency,
                                filterData.SearchText, filterData.DateFrom, filterData.DateTo, filterData.IDFrom, filterData.IDTo, filterData.FirstSeenFrom,
                                filterData.FirstSeenTo, filterData.LastSeenFrom, filterData.LastSeenTo, filterData.SortField, filterData.SortDirection,
                                filterData.PageNumber, filterData.PageSize)
                            .ToList();

                        recordCount = results.FirstOrDefault()?.RecordCount ?? 0;
                        towerFrequencyTalkgroups.AddRange(results.Select(tftg => _mapperConfig.Map<TowerFrequencyTalkgroup>(tftg)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting talkgroups for tower frequency");
                    throw;
                }
            });

            return (towerFrequencyTalkgroups, recordCount);
        }
    }
}
