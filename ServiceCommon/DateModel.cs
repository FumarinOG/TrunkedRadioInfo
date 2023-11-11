using System;

namespace ServiceCommon
{
    public sealed class DateModel
    {
        public DateTime Date { get; private set; }
        public string DateText => $"{Date:yyyy-MM-dd}";

        public DateModel(DateTime date) => (Date) = (date);
    }
}
