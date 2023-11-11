using System;

namespace DataLibrary
{
    public sealed class FilterData
    {
        public string SystemID { get; set; }
        public bool ActiveOnly { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
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
    }
}
