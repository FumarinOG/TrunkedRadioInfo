using DataAccessLibrary;
using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLibrary.Interfaces
{
    public interface IProcessedFileRepository
    {
        Task<ProcessedFile> GetAsync(int id);
        Task<ProcessedFile> GetForTypeAsync(int systemID, FileTypes fileType, string fileName);
        Task<bool> FileExistsAsync(int systemID, string fileName, DateTime? fileDate);
        Task<IEnumerable<ProcessedFile>> GetForSystemAsync(int systemID);
        Task<(IEnumerable<ProcessedFile> processedFiles, int recordCount)> GetForSystemAsync(string systemID, string sortField, string sortDirection,
            int pageNumber, int pageSize);
        Task<(IEnumerable<ProcessedFile> processedFiles, int recordCount)> GetForSystemAsync(string systemID, FilterData filterData);
        Task<int> GetCountForSystemAsync(int systemID);
        Task WriteAsync(ProcessedFile processedFile);
        void EditRecord(ProcessedFiles databaseRecord, ProcessedFile processedFile);
    }
}
