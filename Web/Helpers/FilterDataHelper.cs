using Kendo.Mvc;
using Kendo.Mvc.UI;
using ServiceCommon;
using System;
using System.Collections.Generic;
using static Web.Helpers.Factory;

namespace Web.Helpers
{
    public class FilterDataHelper
    {
        public static readonly string SYSTEM = "System";
        public static readonly string TALKGROUP = "Talkgroup";
        public static readonly string RADIO = "Radio";
        public static readonly string TOWER = "Tower";
        public static readonly string NEIGHBOR_TOWER = "NeighborTower";
        public static readonly string TALKGROUP_FROM = "FromTalkgroup";
        public static readonly string TALKGROUP_TO = "ToTalkgroup";
        public static readonly string FILE = "File";

        public static readonly string SEARCH_DATA = "searchData";

        private readonly string[] _recordType;
        private string[] _fieldNames;

        private const string ID = "ID";
        private const string NAME = "Name";
        private const string NUMBER = "Number";
        private const string FIRST_SEEN = "FirstSeen";
        private const string LAST_SEEN = "LastSeen";

        public FilterDataHelper(params string[] recordType)
        {
            _recordType = recordType;
            BuildFieldNames();
        }

        private void BuildFieldNames()
        {
            var fieldNames = new List<string>();

            foreach (var field in _recordType)
            {
                fieldNames.Add($"{field}{ID}");
                fieldNames.Add($"{field}{NAME}");
                fieldNames.Add($"{field}{NUMBER}");
                fieldNames.Add(NAME);
            }

            _fieldNames = fieldNames.ToArray();
        }

        public FilterDataModel ConvertRequest(DataSourceRequest request, DateTime? dateFrom, DateTime? dateTo) =>
            ConvertRequest(request, CreateFilterData(dateFrom, dateTo));

        public FilterDataModel ConvertRequest(DataSourceRequest request, FilterDataModel filterData = null)
        {
            if (filterData == null)
            {
                filterData = CreateFilterData();
            }

            if (request.Filters != null)
            {
                GetFilters(request, filterData);
            }

            if (request.Sorts != null)
            {
                GetSortData(request, filterData);
            }

            GetPagingData(request, filterData);
            return filterData;
        }

        private void GetFilters(DataSourceRequest request, FilterDataModel filterData)
        {
            var filters = new List<FilterDescriptor>();

            RecurseFilterDescriptors(request.Filters, filters);

            foreach (var filter in filters)
            {
                ParseFilter(filter, filterData);
            }
        }

        private static void RecurseFilterDescriptors(IEnumerable<IFilterDescriptor> requestFilters, ICollection<FilterDescriptor> allFilters)
        {
            foreach (var filterDescriptor in requestFilters)
            {
                switch (filterDescriptor)
                {
                    case FilterDescriptor singleFilter:
                        allFilters.Add(singleFilter);
                        break;

                    case CompositeFilterDescriptor compositeFilter:
                        RecurseFilterDescriptors(compositeFilter.FilterDescriptors, allFilters);
                        break;
                }
            }
        }

        private void ParseFilter(FilterDescriptor filterDescriptor, FilterDataModel searchData)
        {
            if (filterDescriptor.Member.In(_fieldNames))
            {
                if (filterDescriptor.Member.Contains(ID) || filterDescriptor.Member.Contains(NUMBER))
                {
                    switch (filterDescriptor.Operator)
                    {
                        case FilterOperator.IsGreaterThanOrEqualTo:
                            searchData.IDFrom = Convert.ToInt32(filterDescriptor.ConvertedValue);
                            break;

                        case FilterOperator.IsLessThanOrEqualTo:
                            searchData.IDTo = Convert.ToInt32(filterDescriptor.ConvertedValue);
                            break;

                        default:
                            throw new ArgumentOutOfRangeException(nameof(filterDescriptor));
                    }
                }
                else if (filterDescriptor.Member.Contains(NAME))
                {
                    switch (filterDescriptor.Operator)
                    {
                        case FilterOperator.Contains:
                            searchData.SearchText = filterDescriptor.ConvertedValue.ToString();
                            break;

                        default:
                            throw new ArgumentOutOfRangeException(nameof(filterDescriptor));
                    }
                }
            }
            else if (filterDescriptor.Member.Equals(FIRST_SEEN))
            {
                switch (filterDescriptor.Operator)
                {
                    case FilterOperator.IsGreaterThanOrEqualTo:
                        searchData.FirstSeenFrom = Convert.ToDateTime(filterDescriptor.ConvertedValue);
                        break;

                    case FilterOperator.IsLessThanOrEqualTo:
                        searchData.FirstSeenTo = Convert.ToDateTime(filterDescriptor.ConvertedValue);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(filterDescriptor));
                }
            }
            else if (filterDescriptor.Member.Equals(LAST_SEEN))
            {
                switch (filterDescriptor.Operator)
                {
                    case FilterOperator.IsGreaterThanOrEqualTo:
                        searchData.LastSeenFrom = Convert.ToDateTime(filterDescriptor.ConvertedValue);
                        break;

                    case FilterOperator.IsLessThanOrEqualTo:
                        searchData.LastSeenTo = Convert.ToDateTime(filterDescriptor.ConvertedValue);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(filterDescriptor));
                }
            }
        }

        private static void GetSortData(DataSourceRequest request, FilterDataModel filterData)
        {
            if (request.Sorts[0].Member != null)
            {
                filterData.SortField = request.Sorts[0].Member;
                filterData.SortDirection = request.Sorts[0].SortDirection.ToString();
            }
        }

        private static void GetPagingData(DataSourceRequest request, FilterDataModel filterData)
        {
            filterData.PageNumber = request.Page;
            filterData.PageSize = request.PageSize;
        }
    }
}