using AutoMapper;
using DataLibrary;
using NLog;
using System;

namespace ServiceCommon
{
    public abstract class ServiceBase
    {
        protected static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        protected static IMapperConfigurationExpression _mapperConfig;
        protected IMapper _mapper;

        protected ServiceBase(Action<IMapperConfigurationExpression> addMap) => _mapper = AutoMapperSetup(addMap);

        protected IMapper AutoMapperSetup(Action<IMapperConfigurationExpression> addMap = null)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<FilterDataModel, FilterData>();

                addMap?.Invoke(cfg);
            });

            return config.CreateMapper();
        }

        protected static void CheckFilterRanges(FilterDataModel filterData, int minimumID, int maximumID)
        {
            CheckIDRanges(filterData, minimumID, maximumID);
            CheckFirstSeenRanges(filterData);
            CheckLastSeenRanges(filterData);
        }

        private static void CheckIDRanges(FilterDataModel filterData, int minimumID, int maximumID)
        {
            if ((filterData.IDFrom == null) && (filterData.IDTo == null))
            {
                return;
            }

            if (filterData.IDFrom == null)
            {
                filterData.IDFrom = minimumID;
            }
            else if (filterData.IDTo == null)
            {
                filterData.IDTo = maximumID;
            }
        }

        private static void CheckFirstSeenRanges(FilterDataModel filterData)
        {
            if ((filterData.FirstSeenFrom == null) && (filterData.FirstSeenTo == null))
            {
                return;
            }

            if (filterData.FirstSeenFrom == null)
            {
                filterData.FirstSeenFrom = DateTime.MinValue;
            }
            else if (filterData.FirstSeenTo == null)
            {
                filterData.FirstSeenTo = DateTime.MaxValue;
            }
        }

        private static void CheckLastSeenRanges(FilterDataModel filterData)
        {
            if ((filterData.LastSeenFrom == null) && (filterData.LastSeenTo == null))
            {
                return;
            }

            if (filterData.LastSeenFrom == null)
            {
                filterData.LastSeenFrom = DateTime.MinValue;
            }
            else if (filterData.LastSeenTo == null)
            {
                filterData.LastSeenTo = DateTime.MaxValue;
            }
        }

        protected static bool GetAllRecords(FilterDataModel filterData) =>
            (filterData.IDFrom == null) &&
                   (filterData.IDTo == null) &&
                   filterData.SearchText.IsNullOrWhiteSpace() &&
                   (filterData.DateFrom == null) &&
                   (filterData.DateTo == null) &&
                   (filterData.FirstSeenFrom == null) &&
                   (filterData.FirstSeenTo == null) &&
                   (filterData.LastSeenFrom == null) &&
                   (filterData.LastSeenTo == null);
    }
}
