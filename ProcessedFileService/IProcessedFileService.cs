using ObjectLibrary;
using ServiceCommon;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProcessedFileService
{
    public interface IProcessedFileService
    {
        Task<ProcessedFile> GetAsync(int id);
        Task<bool> FileExistsAsync(int systemID, string fileName, DateTime fileDate, FileTypes fileType);
        Task<IEnumerable<ProcessedFileViewModel>> GetForSystemAsync(int systemID);
        Task<(IEnumerable<ProcessedFileViewModel> processedFiles, int recordCount)> GetViewForSystemAsync(string systemID, FilterDataModel filterData);
        Task<int> GetCountForSystemAsync(int systemID, string searchText);
        Task WriteAsync(int systemID, UploadFileModel processedFile);
        ProcessedFile CreateProcessedFile(int systemID, UploadFileModel uploadFile);
        UploadFileModel CreateUploadFileModel(string fileName, string fileType, DateTime lastModified, long size);
    }
}
