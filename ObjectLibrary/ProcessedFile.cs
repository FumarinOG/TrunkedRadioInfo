using ObjectLibrary.Abstracts;
using System;

namespace ObjectLibrary
{
    public class ProcessedFile : AuditableBase
    {
        public int SystemID
        {
            get => _systemID;
            set => SetProperty(ref _systemID, value);
        }

        public string LongFileName
        {
            get => _longFileName;
            set => SetProperty(ref _longFileName, value);
        }

        public string FileName
        {
            get => GetFileName(_longFileName);
            set => _longFileName = value;
        }

        public FileTypes Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        public DateTime FileDate
        {
            get => _fileDate;
            set => SetProperty(ref _fileDate, value);
        }

        public DateTime? DateProcessed
        {
            get => _dateProcessed;
            set => SetProperty(ref _dateProcessed, value);
        }

        public int RowCount
        {
            get => _rowCount;
            set => SetProperty(ref _rowCount, value);
        }

        public DateTime LastModified { get; set; }

        public override string ToString() => $"Filename {FileName}";

        private int _systemID;
        private string _longFileName;
        private FileTypes _type;
        private DateTime _fileDate;
        private DateTime? _dateProcessed;
        private int _rowCount;

        public static string GetFileName(string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName) && (fileName.Contains("\\")))
            {
                return fileName.Substring(fileName.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase) + 1);
            }

            return fileName;
        }
    }
}
