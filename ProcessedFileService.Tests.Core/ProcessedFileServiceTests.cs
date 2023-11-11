using DataLibrary.Interfaces;
using Moq;
using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProcessedFileService.Tests.Core
{
    public class ProcessedFileServiceTests
    {
        /*
        (IEnumerable<ProcessedFileViewModel> processedFiles, int recordCount) GetViewForSystem(string systemID, FilterDataModel filterData);
        int GetCountForSystem(int systemID, string searchText);
        void Write(int systemID, UploadFileModel processedFile);
        UploadFileModel CreateFile(string fileName, string fileType, DateTime lastModified, long size);
*/
        private IProcessedFileService _processedFileService;

        public ProcessedFileServiceTests()
        {
            var processedFileRepo = new Mock<IProcessedFileRepository>();
            var processedFile = new ProcessedFile();

            processedFileRepo.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(processedFile);

            processedFileRepo.Setup(repo => repo.FileExistsAsync(1, "SystemExists.txt", DateTime.Parse("07-01-2018 18:00"))).ReturnsAsync(true);
            processedFileRepo.Setup(repo => repo.FileExistsAsync(1, "TowerExists.txt", DateTime.Parse("07-02-2018 18:00"))).ReturnsAsync(true);
            processedFileRepo.Setup(repo => repo.FileExistsAsync(1, "TalkgroupsExists.txt", DateTime.Parse("07-03-2018 18:00"))).ReturnsAsync(true);
            processedFileRepo.Setup(repo => repo.FileExistsAsync(1, "RadiosExists.txt", DateTime.Parse("07-04-2018 18:00"))).ReturnsAsync(true);
            processedFileRepo.Setup(repo => repo.FileExistsAsync(1, "AffiliationExists.txt", null)).ReturnsAsync(true);
            processedFileRepo.Setup(repo => repo.FileExistsAsync(1, "GrantLogExists.txt", null)).ReturnsAsync(true);
            processedFileRepo.Setup(repo => repo.FileExistsAsync(1, "PatchLogExists.txt", null)).ReturnsAsync(true);
            processedFileRepo.Setup(repo => repo.FileExistsAsync(1, "SystemDoesNotExist.txt", DateTime.Parse("07-08-2018 18:00"))).ReturnsAsync(false);
            processedFileRepo.Setup(repo => repo.FileExistsAsync(1, "TowerDoesNotExist.txt", DateTime.Parse("07-09-2018 18:00"))).ReturnsAsync(false);
            processedFileRepo.Setup(repo => repo.FileExistsAsync(1, "TalkgroupsDoesNotExist.txt", DateTime.Parse("07-10-2018 18:00"))).ReturnsAsync(false);
            processedFileRepo.Setup(repo => repo.FileExistsAsync(1, "RadiosDoesNotExist.txt", DateTime.Parse("07-11-2018 18:00"))).ReturnsAsync(false);
            processedFileRepo.Setup(repo => repo.FileExistsAsync(1, "AffiliationDoesNotExist.txt", null)).ReturnsAsync(false);
            processedFileRepo.Setup(repo => repo.FileExistsAsync(1, "GrantLogDoesNotExist.txt", null)).ReturnsAsync(false);
            processedFileRepo.Setup(repo => repo.FileExistsAsync(1, "PatchLogDoesNotExist.txt", null)).ReturnsAsync(false);

            processedFileRepo.Setup(repo => repo.GetForSystemAsync(It.IsAny<int>())).ReturnsAsync(new List<ProcessedFile> { processedFile });

            _processedFileService = new ProcessedFileService(processedFileRepo.Object);
        }

        [Fact]
        public async Task GetReturnsData()
        {
            var result = await _processedFileService.GetAsync(15);

            Assert.IsAssignableFrom<ProcessedFile>(result);
        }

        [Theory]
        [MemberData(nameof(GetFileExistsData))]
        public async Task FileExistsAsyncReturnsAppropriateValues(int systemID, string fileName, DateTime fileDate, FileTypes fileType, bool expectedValue)
        {
            var result = await _processedFileService.FileExistsAsync(systemID, fileName, fileDate, fileType);

            Assert.Equal(expectedValue, result);
        }

        public static IEnumerable<object[]>GetFileExistsData()
        {
            var data = new[]
            {
                new object[] { 1, "SystemExists.txt", DateTime.Parse("07-01-2018 18:00"), FileTypes.System, true },
                new object[] { 1, "TowerExists.txt", DateTime.Parse("07-02-2018 18:00"), FileTypes.Tower, true },
                new object[] { 1, "TalkgroupsExists.txt", DateTime.Parse("07-03-2018 18:00"), FileTypes.Talkgroups, true },
                new object[] { 1, "RadiosExists.txt", DateTime.Parse("07-04-2018 18:00"), FileTypes.Radios, true },
                new object[] { 1, "AffiliationExists.txt", DateTime.Parse("07-05-2018 18:00"), FileTypes.Affiliations, true },
                new object[] { 1, "GrantLogExists.txt", DateTime.Parse("07-06-2018 18:00"), FileTypes.GrantLog, true },
                new object[] { 1, "PatchLogExists.txt", DateTime.Parse("07-07-2018 18:00"), FileTypes.PatchLog, true },
                new object[] { 1, "SystemDoesNotExist.txt", DateTime.Parse("07-08-2018 18:00"), FileTypes.System, false },
                new object[] { 1, "TowerDoesNotExist.txt", DateTime.Parse("07-09-2018 18:00"), FileTypes.Tower, false },
                new object[] { 1, "TalkgroupsDoesNotExist.txt", DateTime.Parse("07-10-2018 18:00"), FileTypes.Talkgroups, false },
                new object[] { 1, "RadiosDoesNotExist.txt", DateTime.Parse("07-11-2018 18:00"), FileTypes.Radios, false },
                new object[] { 1, "AffiliationDoesNotExist.txt", DateTime.Parse("07-12-2018 18:00"), FileTypes.Affiliations, false },
                new object[] { 1, "GrantLogDoesNotExist.txt", DateTime.Parse("07-13-2018 18:00"), FileTypes.GrantLog, false },
                new object[] { 1, "PatchLogDoesNotExist.txt", DateTime.Parse("07-14-2018 18:00"), FileTypes.PatchLog, false }
            };

            return data;
        }

        [Fact]
        public async Task GetForSystemAsyncReturnsViewModel()
        {
            var result = await _processedFileService.GetForSystemAsync(140);

            Assert.IsAssignableFrom<IEnumerable<ProcessedFileViewModel>>(result);
            Assert.True(result.Count() > 0, "Result count is 0");
        }

        [Theory]
        [MemberData(nameof(GetProcessedFileData))]
        public void CreateProcessedFileReturnsAppropriateObject(int systemID, ObjectLibrary.FileTypes fileType, UploadFileModel uploadFile)
        {
            var result = _processedFileService.CreateProcessedFile(systemID, uploadFile);

            Assert.Equal(systemID, result.SystemID);
            Assert.Equal(uploadFile.FileName, result.LongFileName);
            Assert.Equal(fileType, result.Type);
            Assert.Equal(uploadFile.LastModified, result.FileDate);
            Assert.Equal(uploadFile.RowCount, result.RowCount);
            Assert.IsAssignableFrom<DateTime>(result.DateProcessed);
        }

        public static IEnumerable<object[]> GetProcessedFileData()
        {
            var data = new []
            {
                new object[] { 2, ObjectLibrary.FileTypes.Tower, new UploadFileModel { FileName = "c:\\Test\\Tower.txt", Type = FileTypes.Tower,
                    LastModified = DateTime.Parse("01-02-2018"), RowCount = 20000 } },
                new object[] { 3, ObjectLibrary.FileTypes.Talkgroups, new UploadFileModel { FileName = "c:\\Test\\Talkgroups.txt", Type = FileTypes.Talkgroups,
                    LastModified = DateTime.Parse("01-03-2018"), RowCount = 30000 } },
                new object[] { 4, ObjectLibrary.FileTypes.Radios, new UploadFileModel { FileName = "c:\\Test\\Radios.txt", Type = FileTypes.Radios,
                    LastModified = DateTime.Parse("01-04-2018"), RowCount = 40000 } },
                new object[] { 5, ObjectLibrary.FileTypes.Affiliations, new UploadFileModel { FileName = "c:\\Test\\Affiliation.csv",
                    Type = FileTypes.Affiliations, LastModified = DateTime.Parse("01-05-2018"), RowCount = 50000 } },
                new object[] { 6, ObjectLibrary.FileTypes.GrantLog, new UploadFileModel { FileName = "c:\\Test\\GrantLog.csv", Type = FileTypes.GrantLog,
                    LastModified = DateTime.Parse("01-06-2018"), RowCount = 60000 } },
                new object[] { 7, ObjectLibrary.FileTypes.PatchLog, new UploadFileModel { FileName = "c:\\Test\\PatchLog.csv", Type = FileTypes.PatchLog,
                    LastModified = DateTime.Parse("01-07-2018"), RowCount = 70000 } }
            };

            return data;
        }
    }
}
