using System;

namespace ProcessedFileService
{
    public sealed class ProcessedFileViewModel
    {
        public string FileName { get; private set; }
        public DateTime FileDate { get; private set; }
        public DateTime DateProcessed { get; private set; }
        public int RowCount { get; set; }

        public ProcessedFileViewModel(string fileName, DateTime fileDate, DateTime dateProcessed, int rowCount) =>
            (FileName, FileDate, DateProcessed, RowCount) = (fileName, fileDate, dateProcessed, rowCount);
    }
}
