using DataAccessLibrary;
using DataLibrary.Interfaces;
using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLibrary.Repositories
{
    public class ProcessedFileRepository : RepositoryBase, IProcessedFileRepository
    {
        public async Task<ProcessedFile> GetAsync(int id)
        {
            ProcessedFile processedFile = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var processedFileData = dataEntities.ProcessedFilesGet(id).SingleOrDefault();

                        if (processedFileData != null)
                        {
                            processedFile = _mapperConfig.Map<ProcessedFile>(processedFileData);
                        }
                        else
                        {
                            processedFile = null;
                        }
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting processed file");
                    throw;
                }
            });

            return processedFile;
        }

        public async Task<ProcessedFile> GetForTypeAsync(int systemID, FileTypes fileType, string fileName)
        {
            ProcessedFile processedFile = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var processedFileData = dataEntities.ProcessedFilesGetForType(systemID, fileType.ToString(), fileName).SingleOrDefault();

                        if (processedFileData != null)
                        {
                            processedFile = _mapperConfig.Map<ProcessedFile>(processedFileData);
                        }
                        else
                        {
                            processedFile = null;
                        }
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting processed file for type");
                    throw;
                }
            });

            return processedFile;
        }

        public async Task<bool> FileExistsAsync(int systemID, string fileName, DateTime? fileDate)
        {
            var fileExists = false;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var simpleFileName = ProcessedFile.GetFileName(fileName);
                        var processedFile = dataEntities
                            .ProcessedFilesCheckFileExists(systemID, simpleFileName, fileDate).SingleOrDefault();

                        fileExists = (processedFile != 0);
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error checking if a processed file exists");
                    throw;
                }
            });

            return fileExists;
        }

        public async Task<IEnumerable<ProcessedFile>> GetForSystemAsync(int systemID)
        {
            var processedFiles = CreateList<ProcessedFile>();

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        processedFiles.AddRange(dataEntities.ProcessedFilesGetForSystem(systemID)
                            .Select(pf => _mapperConfig.Map<ProcessedFile>(pf)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting processed files for system");
                    throw;
                }
            });

            return processedFiles;
        }

        public async Task<(IEnumerable<ProcessedFile> processedFiles, int recordCount)> GetForSystemAsync(string systemID, string sortField,
            string sortDirection, int pageNumber, int pageSize)
        {
            var processedFiles = CreateList<ProcessedFile>();
            int recordCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var results = dataEntities.ProcessedFilesGetForSystemWithPaging(systemID, sortField, sortDirection, pageNumber, pageSize).ToList();

                        recordCount = results.FirstOrDefault()?.RecordCount ?? 0;
                        processedFiles.AddRange(results.Select(pf => _mapperConfig.Map<ProcessedFile>(pf)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting processed files for system");
                    throw;
                }
            });

            return (processedFiles, recordCount);
        }

        public async Task<(IEnumerable<ProcessedFile> processedFiles, int recordCount)> GetForSystemAsync(string systemID, FilterData filterData)
        {
            var processedFiles = CreateList<ProcessedFile>();
            int recordCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var results = dataEntities.ProcessedFilesGetForSystemFiltersWithPaging(systemID, filterData.SearchText, filterData.DateFrom,
                                filterData.DateTo, filterData.SortField, filterData.SortDirection, filterData.PageNumber, filterData.PageSize)
                            .ToList();

                        recordCount = results.FirstOrDefault()?.RecordCount ?? 0;
                        processedFiles.AddRange(results.Select(pf => _mapperConfig.Map<ProcessedFile>(pf)));
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting processed files for system");
                    throw;
                }
            });

            return (processedFiles, recordCount);
        }

        public async Task<int> GetCountForSystemAsync(int systemID)
        {
            int fileCount = default;

            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        fileCount = dataEntities.ProcessedFilesGetCountForSystem(systemID).SingleOrDefault() ?? 0;
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error getting processed files for system count");
                    throw;
                }
            });

            return fileCount;
        }

        public async Task WriteAsync(ProcessedFile processedFile)
        {
            if (processedFile.IsNew)
            {
                await AddRecordAsync(processedFile);
            }
            else
            {
                await UpdateRecordAsync(processedFile);
            }
        }

        private async Task AddRecordAsync(ProcessedFile processedFile)
        {
            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var newRecord = dataEntities.ProcessedFiles.Create();

                        EditRecord(newRecord, processedFile);
                        dataEntities.ProcessedFiles.Add(newRecord);
                        dataEntities.SaveChanges();

                        processedFile.ID = newRecord.ID;
                        processedFile.IsNew = false;
                        processedFile.IsDirty = false;
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error adding processed file");
                    throw;
                }
            });
        }

        private async Task UpdateRecordAsync(ProcessedFile processedFile)
        {
            await Task.Run(() =>
            {
                try
                {
                    using (var dataEntities = CreateEntities())
                    {
                        var selectedRecord = dataEntities.ProcessedFilesGet(processedFile.ID).SingleOrDefault();

                        if (selectedRecord == null)
                        {
                            throw new ArgumentException(@"Invalid processed file record", nameof(processedFile));
                        }

                        EditRecord(selectedRecord, processedFile);
                        dataEntities.SaveChanges();

                        processedFile.IsNew = false;
                        processedFile.IsDirty = false;
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error updating processed file");
                    throw;
                }
            });
        }

        public void EditRecord(ProcessedFiles databaseRecord, ProcessedFile processedFile)
        {
            databaseRecord.SystemID = processedFile.SystemID;
            databaseRecord.FileName = processedFile.FileName;
            databaseRecord.Type = processedFile.Type.ToString();
            databaseRecord.FileDate = processedFile.FileDate;
            databaseRecord.DateProcessed = processedFile.DateProcessed;
            databaseRecord.RowCount = processedFile.RowCount;
        }
    }
}
