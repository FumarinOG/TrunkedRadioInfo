using System;

namespace ServiceCommon
{
    public sealed class FilterDataModel
    {
        public string SystemID { get; set; }
        public int? TowerNumber { get; set; }
        public bool ActiveOnly { get; set; }

        public string ActiveOnlyText => ActiveOnly ? "Yes" : "No";

        public DateTime? DateFrom { get; set; }

        public string DateFromText => DateFrom != null ? $"{DateFrom:MM-dd-yyyy}" : string.Empty;

        public DateTime? DateTo { get; set; }

        public string DateToText => DateTo != null ? $"{DateTo:MM-dd-yyyy}" : string.Empty;

        public int? IDFrom { get; set; }
        public int? IDTo { get; set; }
        public string SearchText { get; set; }
        public DateTime? FirstSeenFrom { get; set; }
        public DateTime? FirstSeenTo { get; set; }
        public DateTime? LastSeenFrom { get; set; }
        public DateTime? LastSeenTo { get; set; }
        public string SortField { get; set; }
        public string SortDirection { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public bool IsFiltered => ActiveOnly || (DateFrom != null) || (DateTo != null);
    }
}
