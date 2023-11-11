using ObjectLibrary.Abstracts;
using System;

namespace ServiceCommon
{
    public static class Factory
    {
        public static T Create<T>() where T : AuditableBase, new() =>
            new T
            {
                IsNew = true
            };

        public static DateModel CreateDateModel(DateTime date) => new DateModel(date);

        public static DateTime GetFileDate(string longFileName)
        {
            var fileName = longFileName.Substring(longFileName.LastIndexOf("\\", StringComparison.Ordinal) + 1);
            var fileDateOriginal = fileName.Substring(0, fileName.IndexOf("-", StringComparison.Ordinal));
            var fileDateParsed = $"{fileDateOriginal.Substring(4, 2)}-{fileDateOriginal.Substring(6, 2)}-{fileDateOriginal.Substring(0, 4)}";

            if (DateTime.TryParse(fileDateParsed, out var fileDate))
            {
                return fileDate;
            }

            throw new ArgumentException("Invalid date in filename", nameof(longFileName));
        }
    }
}
