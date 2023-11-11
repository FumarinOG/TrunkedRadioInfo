using DataLibrary.Interfaces;
using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLibrary.Repositories
{
    public class RadioHistoryRepository : RepositoryBase, IRadioHistoryRepository
    {
        public async Task<IEnumerable<RadioHistory>> GetForRadioAsync(int systemID, int radioID)
        {
            var radioHistories = CreateList<RadioHistory>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        radioHistories.AddRange(dataEntities.RadioHistoryGetForRadio(systemID, radioID).Select(rh => _mapperConfig.Map<RadioHistory>(rh)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting radio history for radio");
                    throw;
                }
            });

            return radioHistories;
        }

        public async Task<(IEnumerable<RadioHistory> radioHistory, int recordCount)> GetForRadioAsync(string systemID, int radioID,
            FilterData filterData)
        {
            var radioHistory = CreateList<RadioHistory>();
            int recordCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var results = dataEntities.RadioHistoryGetForRadioFiltersWithPaging(systemID, radioID, filterData.SearchText, filterData.DateFrom,
                                filterData.DateTo, filterData.IDFrom, filterData.IDTo, filterData.FirstSeenFrom, filterData.FirstSeenTo,
                                filterData.LastSeenFrom, filterData.LastSeenTo, filterData.SortField, filterData.SortDirection, filterData.PageNumber,
                                filterData.PageSize)
                            .ToList();

                        recordCount = results.FirstOrDefault()?.RecordCount ?? 0;
                        radioHistory.AddRange(results.Select(rh => _mapperConfig.Map<RadioHistory>(rh)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting radio history for radio");
                    throw;
                }
            });

            return (radioHistory, recordCount);
        }

        public async Task<int> GetForRadioCountAsync(int systemID, int radioID)
        {
            int radioHistoryCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        radioHistoryCount = dataEntities.RadioHistoryGetForRadioCount(systemID, radioID)
                            .SingleOrDefault() ?? 0;
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting radio history for radio count");
                    throw;
                }
            });

            return radioHistoryCount;
        }

        public async Task<IEnumerable<RadioHistory>> GetForSystemAsync(int systemID)
        {
            var radioHistories = CreateList<RadioHistory>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        radioHistories.AddRange(dataEntities.RadioHistoryGetForSystem(systemID)
                            .Select(rh => _mapperConfig.Map<RadioHistory>(rh)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting radio history for system");
                    throw;
                }
            });

            return radioHistories;
        }
    }
}
