using DataAccessLibrary;
using DataLibrary.Repositories;
using Moq;
using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DataLibrary.Tests.Repositories
{
    public class ProcessedFileRepositoryTests : RepositoryTestBase<ProcessedFileRepository>
    {
        private readonly ProcessedFiles _testProcessedFile1 = new ProcessedFiles
        {
            ID = 1,
            SystemID = 140,
            FileName = "20180824-GrantLog-216.csv",
            Type = "GrantLog",
            FileDate = DateTime.Parse("08-24-2018 18:41"),
            DateProcessed = DateTime.Now.AddDays(-5),
            RowCount = 59302,
            LastModified = DateTime.Parse("08-24-2018 18:41:33")
        };

        private readonly ProcessedFiles _testProcessedFile2 = new ProcessedFiles
        {
            ID = 2,
            SystemID = 140,
            FileName = "20180907-Affiliations-101.csv",
            Type = "Affiliations",
            FileDate = DateTime.Parse("09-07-2018 21:17"),
            DateProcessed = DateTime.Now.AddDays(-3),
            RowCount = 109302,
            LastModified = DateTime.Parse("09-07-2018 21:17:51")
        };

        private readonly ProcessedFiles_Result _testProcessedFileResult1 = new ProcessedFiles_Result
        {
            ID = 1,
            SystemID = 140,
            FileName = "20180824-GrantLog-216.csv",
            DateProcessed = DateTime.Now.AddDays(-5),
            RowCount = 59302,
            LastModified = DateTime.Parse("08-24-2018 18:41:33"),
            FileDate = DateTime.Parse("08-24-2018 18:41"),
            RecordCount = 2
        };

        private readonly ProcessedFiles_Result _testProcessedFileResult2 = new ProcessedFiles_Result
        {
            ID = 2,
            SystemID = 140,
            FileName = "20180907-Affiliations-101.csv",
            DateProcessed = DateTime.Now.AddDays(-3),
            RowCount = 109302,
            LastModified = DateTime.Parse("09-07-2018 21:17:51"),
            FileDate = DateTime.Parse("09-07-2018 21:17"),
            RecordCount = 2
        };

        private readonly ProcessedFile _testProcessedFile = new ProcessedFile
        {
            SystemID = 140,
            LongFileName = "c:\\Pro96Com\\System140\\20180907-Affiliations-101.csv",
            FileName = "20180907-Affiliations-101.csv",
            Type = FileTypes.Affiliations,
            FileDate = DateTime.Parse("09-07-2018 21:17"),
            DateProcessed = DateTime.Now.AddDays(-3),
            RowCount = 109302,
            LastModified = DateTime.Parse("09-07-2018 21:17:51")
        };

        private readonly int? _testFileExists = 1;
        private readonly int? _testFileDoesNotExist = 0;
        private readonly int? _testFileCount = 30204;

        private IEnumerator<ProcessedFiles> GetProcessedFile()
        {
            yield return _testProcessedFile1;
        }

        private IEnumerator<ProcessedFiles> GetProcessedFiles()
        {
            yield return _testProcessedFile1;
            yield return _testProcessedFile2
;
        }

        private IEnumerator<ProcessedFiles_Result> GetProcessedFilesResults()
        {
            yield return _testProcessedFileResult1;
            yield return _testProcessedFileResult2;
        }

        private IEnumerator<int?> GetProcessedFileExists()
        {
            yield return _testFileExists;
        }

        private IEnumerator<int?> GetProcessedFileDoesNotExist()
        {
            yield return _testFileDoesNotExist;
        }

        private IEnumerator<int?> GetProcessedFileCount()
        {
            yield return _testFileCount;
        }

        [Fact]
        public async Task GetAsyncReturnsAppropriateType()
        {
            var mockProcessedFile = new Mock<MockObjectResult<ProcessedFiles>>();

            mockProcessedFile.Setup(pf => pf.GetEnumerator()).Returns(GetProcessedFile);
            _context.Setup(ctx => ctx.ProcessedFilesGet(It.IsAny<int>())).Returns(mockProcessedFile.Object);
            SetupMockRepo();

            var processedFileRepo = _mockRepo.Object;

            var results = await processedFileRepo.GetAsync(140);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<ProcessedFile>(results);
        }

        [Fact]
        public async Task GetAsyncReturnsAppropriateValues()
        {
            var mockProcessedFile = new Mock<MockObjectResult<ProcessedFiles>>();

            mockProcessedFile.Setup(pf => pf.GetEnumerator()).Returns(GetProcessedFile);
            _context.Setup(ctx => ctx.ProcessedFilesGet(It.IsAny<int>())).Returns(mockProcessedFile.Object);
            SetupMockRepo();

            var processedFileRepo = _mockRepo.Object;

            var results = await processedFileRepo.GetAsync(140);

            Assert.Equal(_testProcessedFile1.ID, results.ID);
            Assert.Equal(_testProcessedFile1.SystemID, results.SystemID);
            Assert.Equal(_testProcessedFile1.FileName, results.FileName);
            Assert.Equal(_testProcessedFile1.Type, results.Type.ToString());
            Assert.Equal(_testProcessedFile1.FileDate, results.FileDate);
            Assert.Equal(_testProcessedFile1.DateProcessed, results.DateProcessed);
            Assert.Equal(_testProcessedFile1.RowCount, results.RowCount);
            Assert.Equal(_testProcessedFile1.LastModified, results.LastModified);
       }

        [Fact]
        public async Task GetAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.ProcessedFilesGet(It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var processedFileRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => processedFileRepo.GetAsync(140));
        }

        [Fact]
        public async Task GetForTypeAsyncReturnsAppropriateType()
        {
            var mockProcessedFile = new Mock<MockObjectResult<ProcessedFiles>>();

            mockProcessedFile.Setup(pf => pf.GetEnumerator()).Returns(GetProcessedFile);
            _context.Setup(ctx => ctx.ProcessedFilesGetForType(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(mockProcessedFile.Object);
            SetupMockRepo();

            var processedFileRepo = _mockRepo.Object;

            var results = await processedFileRepo.GetForTypeAsync(140, FileTypes.GrantLog, "20180824-GrantLog-216.csv");

            Assert.NotNull(results);
            Assert.IsAssignableFrom<ProcessedFile>(results);
        }

        [Fact]
        public async Task GetForTypesAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.ProcessedFilesGetForType(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception("Database error"));
            SetupMockRepo();

            var processedFileRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => processedFileRepo.GetForTypeAsync(140, FileTypes.GrantLog, "20180824-GrantLog-216.csv"));
        }

        [Fact]
        public async Task FileExistsAsyncReturnsTrueWhenFileCountNotZero()
        {
            var mockFileCount = new Mock<MockObjectResult<int?>>();

            mockFileCount.Setup(fc => fc.GetEnumerator()).Returns(GetProcessedFileExists);
            _context.Setup(ctx => ctx.ProcessedFilesCheckFileExists(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(mockFileCount.Object);
            SetupMockRepo();

            var processedFileRepo = _mockRepo.Object;

            var results = await processedFileRepo.FileExistsAsync(140, "20180907-Affiliations-101.csv", DateTime.Parse("09-07-2018 21:17"));

            Assert.True(results);
        }

        [Fact]
        public async Task FileExistsAsyncReturnsTrueWhenFileCountZero()
        {
            var mockFileCount = new Mock<MockObjectResult<int?>>();

            mockFileCount.Setup(fc => fc.GetEnumerator()).Returns(GetProcessedFileDoesNotExist);
            _context.Setup(ctx => ctx.ProcessedFilesCheckFileExists(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(mockFileCount.Object);
            SetupMockRepo();

            var processedFileRepo = _mockRepo.Object;

            var results = await processedFileRepo.FileExistsAsync(140, "20180907-Affiliations-113.csv", DateTime.Parse("09-07-2018 19:22"));

            Assert.False(results);
        }

        [Fact]
        public async Task FileExistsAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.ProcessedFilesCheckFileExists(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                .Throws(new Exception("Database error"));
            SetupMockRepo();

            var processedFileRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => processedFileRepo.FileExistsAsync(140, "20180907-Affiliations-113.csv",
                DateTime.Parse("09-07-2018 19:22")));
        }

        [Fact]
        public async Task GetForSystemAsyncReturnsAppropriateTypes()
        {
            var mockProcessedFiles = new Mock<MockObjectResult<ProcessedFiles>>();

            mockProcessedFiles.Setup(pf => pf.GetEnumerator()).Returns(GetProcessedFiles);
            _context.Setup(ctx => ctx.ProcessedFilesGetForSystem(It.IsAny<int>())).Returns(mockProcessedFiles.Object);
            SetupMockRepo();

            var processedFileRepo = _mockRepo.Object;

            var results = await processedFileRepo.GetForSystemAsync(140);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<ProcessedFile>>(results);
            Assert.True(results.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetForSystemAsyncReturnsAppropriateValues()
        {
            var mockProcessedFiles = new Mock<MockObjectResult<ProcessedFiles>>();

            mockProcessedFiles.Setup(pf => pf.GetEnumerator()).Returns(GetProcessedFiles);
            _context.Setup(ctx => ctx.ProcessedFilesGetForSystem(It.IsAny<int>())).Returns(mockProcessedFiles.Object);
            SetupMockRepo();

            var processedFileRepo = _mockRepo.Object;

            var results = await processedFileRepo.GetForSystemAsync(140);
            var resultsData = results.SingleOrDefault(rd => rd.ID == 2);

            Assert.NotNull(resultsData);
            Assert.Equal(_testProcessedFile2.ID, resultsData.ID);
            Assert.Equal(_testProcessedFile2.SystemID, resultsData.SystemID);
            Assert.Equal(_testProcessedFile2.FileName, resultsData.FileName);
            Assert.Equal(_testProcessedFile2.Type, resultsData.Type.ToString());
            Assert.Equal(_testProcessedFile2.FileDate, resultsData.FileDate);
            Assert.Equal(_testProcessedFile2.DateProcessed, resultsData.DateProcessed);
            Assert.Equal(_testProcessedFile2.RowCount, resultsData.RowCount);
            Assert.Equal(_testProcessedFile2.LastModified, resultsData.LastModified);
        }

        [Fact]
        public async Task GetForSystemAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.ProcessedFilesGetForSystem(It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var processedFileRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => processedFileRepo.GetForSystemAsync(140));
        }

        [Fact]
        public async Task GetForSystemAsyncWithPagingReturnsAppropriateTypes()
        {
            var mockProcessedFilesResult = new Mock<MockObjectResult<ProcessedFiles_Result>>();

            mockProcessedFilesResult.Setup(pf => pf.GetEnumerator()).Returns(GetProcessedFilesResults);
            _context.Setup(ctx => ctx.ProcessedFilesGetForSystemWithPaging(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(),
                It.IsAny<int>())).Returns(mockProcessedFilesResult.Object);
            SetupMockRepo();

            var processedFileRepo = _mockRepo.Object;

            var (processedFiles, recordCount) = await processedFileRepo.GetForSystemAsync("140", "DateProcessed", "Descending", 1, 15);

            Assert.NotNull(processedFiles);
            Assert.IsAssignableFrom<IEnumerable<ProcessedFile>>(processedFiles);
            Assert.True(processedFiles.Count() > 0, "Result count is 0");
            Assert.Equal(_testProcessedFileResult1.RecordCount, recordCount);
        }

        [Fact]
        public async Task GetForSystemAsyncWithPagingThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.ProcessedFilesGetForSystemWithPaging(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(),
                It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var processedFileRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => processedFileRepo.GetForSystemAsync("140", "DateProcessed", "Descending", 1, 15));
        }

        [Fact]
        public async Task GetForSystemAsyncWithFiltersReturnsAppropriateTypes()
        {
            var mockProcessedFilesResult = new Mock<MockObjectResult<ProcessedFiles_Result>>();

            mockProcessedFilesResult.Setup(pf => pf.GetEnumerator()).Returns(GetProcessedFilesResults);
            _context.Setup(ctx => ctx.ProcessedFilesGetForSystemFiltersWithPaging(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockProcessedFilesResult.Object);
            SetupMockRepo();

            var processedFileRepo = _mockRepo.Object;

            var (processedFiles, recordCount) = await processedFileRepo.GetForSystemAsync("140", _filterData);

            Assert.NotNull(processedFiles);
            Assert.IsAssignableFrom<IEnumerable<ProcessedFile>>(processedFiles);
            Assert.True(processedFiles.Count() > 0, "Result count is 0");
            Assert.Equal(_testProcessedFileResult1.RecordCount, recordCount);
        }

        [Fact]
        public async Task GetForSystemAsyncWithFiltersThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.ProcessedFilesGetForSystemFiltersWithPaging(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new Exception("Database error"));
            SetupMockRepo();

            var processedFileRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => processedFileRepo.GetForSystemAsync("140", _filterData));
        }

        [Fact]
        public async Task GetCountForSystemAsyncReturnsACount()
        {
            var mockProcessedFileCount = new Mock<MockObjectResult<int?>>();

            mockProcessedFileCount.Setup(pfc => pfc.GetEnumerator()).Returns(GetProcessedFileCount);
            _context.Setup(ctx => ctx.ProcessedFilesGetCountForSystem(It.IsAny<int>())).Returns(mockProcessedFileCount.Object);
            SetupMockRepo();

            var processedFileRepo = _mockRepo.Object;

            var results = await processedFileRepo.GetCountForSystemAsync(140);

            Assert.Equal(_testFileCount, results);
        }

        [Fact]
        public async Task GetCountForSystemAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.ProcessedFilesGetCountForSystem(It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var processedFileRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => processedFileRepo.GetCountForSystemAsync(140));
        }

        [Fact]
        public void EditRecordSetsAppropriateValues()
        {
            var databaseRecord = new ProcessedFiles();
            var processedFileRepo = _mockRepo.Object;

            processedFileRepo.EditRecord(databaseRecord, _testProcessedFile);

            Assert.Equal(_testProcessedFile.SystemID, databaseRecord.SystemID);
            Assert.Equal(_testProcessedFile.FileName, databaseRecord.FileName);
            Assert.Equal(_testProcessedFile.Type.ToString(), databaseRecord.Type);
            Assert.Equal(_testProcessedFile.FileDate, databaseRecord.FileDate);
            Assert.Equal(_testProcessedFile.DateProcessed, databaseRecord.DateProcessed);
            Assert.Equal(_testProcessedFile.RowCount, databaseRecord.RowCount);
        }

        [Fact]
        public async Task WriteAsyncAddsNewRecord()
        {
            var addProcessedFile = 0;
            var saveChanges = 0;
            var count = 0;

            _context.Setup(ctx => ctx.ProcessedFiles.Create()).Returns(new ProcessedFiles() { ID = 10 });
            _context.Setup(ctx => ctx.ProcessedFiles.Add(It.IsAny<ProcessedFiles>())).Callback(() => addProcessedFile = ++count);
            _context.Setup(ctx => ctx.SaveChanges()).Callback(() => saveChanges = ++count);
            SetupMockRepo();

            _testProcessedFile.IsNew = true;
            _testProcessedFile.IsDirty = true;

            var towerNeighborRepo = _mockRepo.Object;

            await towerNeighborRepo.WriteAsync(_testProcessedFile);

            _context.Verify(ctx => ctx.ProcessedFiles.Add(It.IsAny<ProcessedFiles>()), Times.Once());
            _context.Verify(ctx => ctx.SaveChanges(), Times.Once());

            Assert.Equal(1, addProcessedFile);
            Assert.Equal(2, saveChanges);

            Assert.Equal(10, _testProcessedFile.ID);
            Assert.False(_testProcessedFile.IsNew);
            Assert.False(_testProcessedFile.IsDirty);
        }

        [Fact]
        public async Task WriteAsyncUpdatesRecord()
        {
            var saveChanges = 0;
            var count = 0;
            var mockProcessedFile = new Mock<MockObjectResult<ProcessedFiles>>();

            mockProcessedFile.Setup(s => s.GetEnumerator()).Returns(GetProcessedFile);
            _context.Setup(ctx => ctx.ProcessedFilesGet(It.IsAny<int>())).Returns(mockProcessedFile.Object);
            _context.Setup(ctx => ctx.SaveChanges()).Callback(() => saveChanges = ++count);
            SetupMockRepo();

            var towerNeighborRepo = _mockRepo.Object;

            _testProcessedFile.IsNew = false;
            _testProcessedFile.IsDirty = true;

            await towerNeighborRepo.WriteAsync(_testProcessedFile);

            _context.Verify(ctx => ctx.SaveChanges(), Times.Once());

            Assert.Equal(1, saveChanges);

            Assert.False(_testProcessedFile.IsNew);
            Assert.False(_testProcessedFile.IsDirty);
        }
    }
}
