using AutoMapper;
using DataLibrary;
using DataLibrary.Interfaces;
using ObjectLibrary;
using ServiceCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ServiceCommon.Factory;

namespace ProcessedFileService
{
    public sealed class ProcessedFileService : ServiceBase, IProcessedFileService
    {
        private readonly IProcessedFileRepository _processedFileRepo;

        public ProcessedFileService(IProcessedFileRepository processedFileRepoRepository) : base((mc) => CreateMapping(mc)) =>
            _processedFileRepo = processedFileRepoRepository;

        public static void CreateMapping(IMapperConfigurationExpression config) => config.CreateMap<ProcessedFile, ProcessedFileViewModel>();

        public async Task<ProcessedFile> GetAsync(int id) => await _processedFileRepo.GetAsync(id);

        public async Task<bool> FileExistsAsync(int systemID, string fileName, DateTime fileDate, FileTypes fileType)
        {
            switch (fileType)
            {
                case FileTypes.System:
                case FileTypes.Tower:
                case FileTypes.Talkgroups:      // Only process these files if the dates have changed
                case FileTypes.Radios:
                    return await _processedFileRepo.FileExistsAsync(systemID, fileName, fileDate);

                default:
                    return await _processedFileRepo.FileExistsAsync(systemID, fileName, null);
            }
        }

        public async Task<IEnumerable<ProcessedFileViewModel>> GetForSystemAsync(int systemID)
        {
            var processedFiles = await _processedFileRepo.GetForSystemAsync(systemID);
            
            return processedFiles.Select(pf => _mapper.Map<ProcessedFileViewModel>(pf));
        }

        public async Task<(IEnumerable<ProcessedFileViewModel> processedFiles, int recordCount)> GetViewForSystemAsync(string systemID, FilterDataModel filterData)
        {
            (IEnumerable<ProcessedFile> processedFiles, int recordCount) results;

            CheckFilterRanges(filterData, 0, 0);

            if (GetAllRecords(filterData))
            {
                results = await _processedFileRepo.GetForSystemAsync(systemID, filterData.SortField, filterData.SortDirection,
                    filterData.PageNumber, filterData.PageSize);
            }
            else
            {
                results = await _processedFileRepo.GetForSystemAsync(systemID, _mapper.Map<FilterData>(filterData));
            }

            return (results.processedFiles.Select(pf => _mapper.Map<ProcessedFileViewModel>(pf)), results.recordCount);
        }

        public async Task<int> GetCountForSystemAsync(int systemID, string searchText) => await _processedFileRepo.GetCountForSystemAsync(systemID);

        public async Task WriteAsync(int systemID, UploadFileModel processedFile)
        {
            switch (processedFile.Type)
            {
                case FileTypes.System:
                case FileTypes.Tower:
                case FileTypes.Talkgroups:
                case FileTypes.Radios:
                    var existingFile = await _processedFileRepo.GetForTypeAsync(systemID, processedFile.Type.ToString().ToEnum<ObjectLibrary.FileTypes>(),
                        processedFile.FileName);
                    ProcessedFile fileProcessed = default;

                    if (existingFile == null)
                    {
                        fileProcessed = Create<ProcessedFile>();

                        fileProcessed.SystemID = systemID;
                        fileProcessed.LongFileName = processedFile.FileName;
                        fileProcessed.Type = processedFile.Type.ToString().ToEnum<ObjectLibrary.FileTypes>();
                        fileProcessed.FileDate = processedFile.FileDate;
                        fileProcessed.RowCount = processedFile.RowCount;
                        fileProcessed.DateProcessed = DateTime.Now;
                    }
                    else
                    {
                        fileProcessed = CreateProcessedFile(systemID, processedFile);
                        fileProcessed.IsNew = false;
                    }

                    await _processedFileRepo.WriteAsync(CreateProcessedFile(systemID, processedFile));
                    break;

                case FileTypes.Affiliations:
                case FileTypes.GrantLog:
                case FileTypes.PatchLog:
                    await _processedFileRepo.WriteAsync(CreateProcessedFile(systemID, processedFile));
                    break;
            }

        }

        public ProcessedFile CreateProcessedFile(int systemID, UploadFileModel uploadFile)
        {
            var processedFile = Create<ProcessedFile>();

            processedFile.SystemID = systemID;
            processedFile.LongFileName = uploadFile.FileName;
            processedFile.Type = uploadFile.Type.ToString().ToEnum<ObjectLibrary.FileTypes>();
            processedFile.FileDate = uploadFile.LastModified;
            processedFile.RowCount = uploadFile.RowCount;
            processedFile.DateProcessed = DateTime.Now;

            return processedFile;
        }

        public UploadFileModel CreateUploadFileModel(string fileName, string fileType, DateTime lastModified, long size)
        {
            return Factory.CreateUploadFile(fileName, fileType, lastModified, size);
        }
    }
}
