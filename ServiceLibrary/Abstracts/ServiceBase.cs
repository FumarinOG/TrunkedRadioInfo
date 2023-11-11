using AutoMapper;
using DataLibrary.Interfaces;
using NLog;
using ObjectLibrary.Interfaces;
using ServiceCommon;
using ServiceLibrary.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using static ServiceLibrary.Factory;

namespace ServiceLibrary.Abstracts
{
    public abstract class ServiceBase
    {
        protected readonly Guid _sessionID;

        protected static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        protected readonly IMapper _mapperConfig;

        protected ServiceBase()
        {
            _sessionID = Guid.NewGuid();
            _mapperConfig = Config.AutoMapperSetup();
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

        protected static bool GetAllRecords(FilterDataModel filterData)
        {
            return (filterData.IDFrom == null) &&
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
}
