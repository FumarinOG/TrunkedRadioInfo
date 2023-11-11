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
    public class SystemInfoRepositoryTests : RepositoryTestBase<SystemInfoRepository>
    {
        private readonly SystemInfo _testSystem = new SystemInfo
        {
            ID = 2,
            SystemID = "87D",
            SystemIDDecimal = 2173,
            Description = "Chicago",
            WACN = "BEE00",
            FirstSeen = DateTime.Parse("04-11-2018 11:33"),
            LastSeen = DateTime.Now.AddDays(-1),
            LastModified = DateTime.Now
        };

        private readonly Systems _testSystem1 = new Systems
        {
            ID = 1,
            SystemID = "140",
            SystemIDDecimal = 320,
            Description = "StarCom 21",
            WACN = "BEE00",
            FirstSeen = DateTime.Parse("02-01-2007 18:12"),
            LastSeen = DateTime.Now.AddHours(-1),
            LastModified = DateTime.Now
        };

        private readonly Systems _testSystem2 = new Systems
        {
            ID = 2,
            SystemID = "87D",
            SystemIDDecimal = 2173,
            Description = "Chicago",
            WACN = "BEE00",
            FirstSeen = DateTime.Parse("04-11-2018 11:33"),
            LastSeen = DateTime.Now.AddDays(-1),
            LastModified = DateTime.Now
        };

        private readonly Systems_Result _testSystemResult1 = new Systems_Result
        {
            ID = 1,
            SystemID = "140",
            Description = "StarCom 21",
            FirstSeen = DateTime.Parse("02-01-2007 18:12"),
            LastSeen = DateTime.Now.AddHours(-1),
            TalkgroupCount = 10000,
            RadioCount = 100000,
            TowerCount = 100,
            RowCount = 1000000,
            LastModified = DateTime.Now
        };

        private readonly Systems_Result _testSystemResult2 = new Systems_Result
        {
            ID = 2,
            SystemID = "87D",
            Description = "Chicago",
            FirstSeen = DateTime.Parse("04-11-2018 11:33"),
            LastSeen = DateTime.Now.AddDays(-1),
            TalkgroupCount = 5000,
            RadioCount = 50000,
            TowerCount = 00,
            RowCount = 100000,
            LastModified = DateTime.Now
        };

        private readonly int? _testSystemID1 = 1;

        private readonly int? _testSystemCount = 150;

        private IEnumerator<Systems> GetSystem()
        {
            yield return _testSystem1;
        }

        private IEnumerator<Systems> GetSystems()
        {
            yield return _testSystem1;
            yield return _testSystem2;
        }

        private IEnumerator<Systems_Result> GetSystemResults()
        {
            yield return _testSystemResult1;
            yield return _testSystemResult2;
        }

        private IEnumerator<int?> GetSystemID()
        {
            yield return _testSystemID1;
        }

        private IEnumerator<int?> GetSystemCount()
        {
            yield return _testSystemCount;
        }

        [Fact]
        public async Task GetAsyncIntReturnsAppropriateValues()
        {
            var mockSystem = new Mock<MockObjectResult<Systems>>();

            mockSystem.Setup(s => s.GetEnumerator()).Returns(GetSystem);
            _context.Setup(ctx => ctx.SystemsGet(It.IsAny<int>())).Returns(mockSystem.Object);
            SetupMockRepo();

            var systemInfoRepo = _mockRepo.Object;

            var result = await systemInfoRepo.GetAsync(1);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<SystemInfo>(result);
            Assert.Equal(_testSystem1.ID, result.ID);
            Assert.Equal(_testSystem1.SystemID, result.SystemID);
            Assert.Equal(_testSystem1.SystemIDDecimal, result.SystemIDDecimal);
            Assert.Equal(_testSystem1.Description, result.Description);
            Assert.Equal(_testSystem1.WACN, result.WACN);
            Assert.Equal(_testSystem1.FirstSeen, result.FirstSeen);
            Assert.Equal(_testSystem1.LastSeen, result.LastSeen);
            Assert.Equal(_testSystem1.LastModified, result.LastModified);
        }

        [Fact]
        public async Task GetAsyncIntThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.SystemsGet(It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var systemInfoRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => systemInfoRepo.GetAsync(1));
        }

        [Fact]
        public async Task GetAsyncStringReturnsAppropriateValues()
        {
            var mockSystem = new Mock<MockObjectResult<Systems>>();

            mockSystem.Setup(s => s.GetEnumerator()).Returns(GetSystem);
            _context.Setup(ctx => ctx.SystemsGetForSystem(It.IsAny<string>())).Returns(mockSystem.Object);
            SetupMockRepo();

            var systemInfoRepo = _mockRepo.Object;

            var result = await systemInfoRepo.GetAsync("140");

            Assert.NotNull(result);
            Assert.IsAssignableFrom<SystemInfo>(result);
            Assert.Equal(_testSystem1.ID, result.ID);
            Assert.Equal(_testSystem1.SystemID, result.SystemID);
            Assert.Equal(_testSystem1.SystemIDDecimal, result.SystemIDDecimal);
            Assert.Equal(_testSystem1.Description, result.Description);
            Assert.Equal(_testSystem1.WACN, result.WACN);
            Assert.Equal(_testSystem1.FirstSeen, result.FirstSeen);
            Assert.Equal(_testSystem1.LastSeen, result.LastSeen);
            Assert.Equal(_testSystem1.LastModified, result.LastModified);
        }

        [Fact]
        public async Task GetAsyncStringThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.SystemsGetForSystem(It.IsAny<string>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var systemInfoRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => systemInfoRepo.GetAsync("140"));
        }

        [Fact]
        public async Task GetSystemIDAsyncReturnsProperValue()
        {
            var mockSystemID = new Mock<MockObjectResult<int?>>();

            mockSystemID.Setup(id => id.GetEnumerator()).Returns(GetSystemID);
            _context.Setup(ctx => ctx.SystemsGetID(It.IsAny<string>())).Returns(mockSystemID.Object);
            SetupMockRepo();

            var systemInfoRepo = _mockRepo.Object;

            var result = await systemInfoRepo.GetSystemIDAsync("140");

            Assert.Equal(_testSystemID1, result);
        }

        [Fact]
        public async Task GetSystemIDAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.SystemsGetID(It.IsAny<string>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var systemInfoRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => systemInfoRepo.GetSystemIDAsync("140"));
        }

        [Fact]
        public async Task GetListAsyncReturnsAppropriateTypes()
        {
            var mockSystemResults = new Mock<MockObjectResult<Systems_Result>>();

            mockSystemResults.Setup(sr => sr.GetEnumerator()).Returns(GetSystemResults);
            _context.Setup(ctx => ctx.SystemsGetList()).Returns(mockSystemResults.Object);
            SetupMockRepo();

            var systemInfoRepo = _mockRepo.Object;

            var result = await systemInfoRepo.GetListAsync();

            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<SystemInfo>>(result);
            Assert.True(result.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetListAsyncReturnsAppropriateValues()
        {
            var mockSystemResults = new Mock<MockObjectResult<Systems_Result>>();

            mockSystemResults.Setup(sr => sr.GetEnumerator()).Returns(GetSystemResults);
            _context.Setup(ctx => ctx.SystemsGetList()).Returns(mockSystemResults.Object);
            SetupMockRepo();

            var systemInfoRepo = _mockRepo.Object;

            var result = await systemInfoRepo.GetListAsync();
            var resultData = result.SingleOrDefault(rd => rd.ID == 2);

            Assert.NotNull(resultData);
            Assert.Equal(_testSystemResult2.ID, resultData.ID);
            Assert.Equal(_testSystemResult2.SystemID, resultData.SystemID);
            Assert.Equal(_testSystemResult2.Description, resultData.Description);
            Assert.Equal(_testSystemResult2.FirstSeen, resultData.FirstSeen);
            Assert.Equal(_testSystemResult2.LastSeen, resultData.LastSeen);
            Assert.Equal(_testSystemResult2.TalkgroupCount, resultData.TalkgroupCount);
            Assert.Equal(_testSystemResult2.RadioCount, resultData.RadioCount);
            Assert.Equal(_testSystemResult2.TowerCount, resultData.TowerCount);
            Assert.Equal(_testSystemResult2.RowCount, resultData.RowCount);
            Assert.Equal(_testSystemResult2.LastModified, resultData.LastModified);
        }

        [Fact]
        public async Task GetListAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.SystemsGetList()).Throws(new Exception("Database error"));
            SetupMockRepo();

            var systemInfoRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => systemInfoRepo.GetListAsync());

        }

        [Fact]
        public async Task GetListAsyncFiltersReturnsAppropriateTypes()
        {
            var mockSystemsResult = new Mock<MockObjectResult<Systems_Result>>();

            mockSystemsResult.Setup(sr => sr.GetEnumerator()).Returns(GetSystemResults);
            _context.Setup(ctx => ctx.SystemsGetListFilters(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>())).Returns(mockSystemsResult.Object);
            SetupMockRepo();

            var systemInfoRepo = _mockRepo.Object;

            var results = await systemInfoRepo.GetListAsync(_filterData);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<SystemInfo>>(results);
            Assert.True(results.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetListAsyncFiltersThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.SystemsGetListFilters(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var systemInfoRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => systemInfoRepo.GetListAsync(_filterData));
        }

        [Fact]
        public async Task GetCountAsyncReturnsCount()
        {
            var mockSystemCount = new Mock<MockObjectResult<int?>>();

            mockSystemCount.Setup(sc => sc.GetEnumerator()).Returns(GetSystemCount);
            _context.Setup(ctx => ctx.SystemsGetCount()).Returns(mockSystemCount.Object);
            SetupMockRepo();

            var systemInfoRepo = _mockRepo.Object;

            var result = await systemInfoRepo.GetCountAsync();

            Assert.IsAssignableFrom<int>(result);
            Assert.Equal(_testSystemCount, result);
        }

        [Fact]
        public async Task GetCountAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.SystemsGetCount()).Throws(new Exception("Database error"));
            SetupMockRepo();

            var systemInfoRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => systemInfoRepo.GetCountAsync());
        }

        [Fact]
        public void EditRecordSetsAppropriateValues()
        {
            var databaseRecord = new Systems();
            var systemInfoRepo = _mockRepo.Object;

            systemInfoRepo.EditRecord(databaseRecord, _testSystem);

            Assert.Equal(_testSystem.SystemID, databaseRecord.SystemID);
            Assert.Equal(_testSystem.SystemIDDecimal, databaseRecord.SystemIDDecimal);
            Assert.Equal(_testSystem.Description, databaseRecord.Description);
            Assert.Equal(_testSystem.WACN, databaseRecord.WACN);
            Assert.Equal(_testSystem.FirstSeen, databaseRecord.FirstSeen);
            Assert.Equal(_testSystem.LastSeen, databaseRecord.LastSeen);
        }

        [Fact]
        public async Task WriteAsyncAddsNewRecord()
        {
            var addSystem = 0;
            var saveChanges = 0;
            var count = 0;

            _context.Setup(ctx => ctx.Systems.Create()).Returns(new Systems() { ID = 10 });
            _context.Setup(ctx => ctx.Systems.Add(It.IsAny<Systems>())).Callback(() => addSystem = count++);
            _context.Setup(ctx => ctx.SaveChanges()).Callback(() => saveChanges = count++);
            SetupMockRepo();

            _testSystem.IsNew = true;
            _testSystem.IsDirty = true;

            var systemInfoRepo = _mockRepo.Object;

            await systemInfoRepo.WriteAsync(_testSystem);

            _context.Verify(ctx => ctx.Systems.Add(It.IsAny<Systems>()), Times.Once());
            _context.Verify(ctx => ctx.SaveChanges(), Times.Once());

            Assert.Equal(0, addSystem);
            Assert.Equal(1, saveChanges);

            Assert.Equal(10, _testSystem.ID);
            Assert.False(_testSystem.IsNew);
            Assert.False(_testSystem.IsDirty);
        }

        [Fact]
        public async Task WriteAsyncUpdatesRecord()
        {
            var saveChanges = 0;
            var count = 0;
            var mockSystem = new Mock<MockObjectResult<Systems>>();

            mockSystem.Setup(s => s.GetEnumerator()).Returns(GetSystem);
            _context.Setup(ctx => ctx.SystemsGet(It.IsAny<int>())).Returns(mockSystem.Object);
            _context.Setup(ctx => ctx.SaveChanges()).Callback(() => saveChanges = count++);
            SetupMockRepo();

            var systemInfoRepo = _mockRepo.Object;

            _testSystem.IsNew = false;
            _testSystem.IsDirty = true;

            await systemInfoRepo.WriteAsync(_testSystem);

            _context.Verify(ctx => ctx.SaveChanges(), Times.Once());

            Assert.Equal(0, saveChanges);

            Assert.False(_testSystem.IsNew);
            Assert.False(_testSystem.IsDirty);
        }

        [Fact]
        public async Task AddRecordAsyncThrowsExceptionWithDatabaseException()
        {
            var addSystem = 0;
            var count = 0;

            _context.Setup(ctx => ctx.Systems.Create()).Returns(new Systems());
            _context.Setup(ctx => ctx.Systems.Add(It.IsAny<Systems>())).Callback(() => addSystem = count++);
            _context.Setup(ctx => ctx.SaveChanges()).Throws(new Exception("Database error"));
            SetupMockRepo();

            _testSystem.IsNew = true;

            var systemInfoRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => systemInfoRepo.WriteAsync(_testSystem));
        }

        [Fact]
        public async Task UpdateRecordAsyncThrowsExceptionWithDatabaseException()
        {
            var mockSystem = new Mock<MockObjectResult<Systems>>();

            mockSystem.Setup(s => s.GetEnumerator()).Returns(GetSystem);
            _context.Setup(ctx => ctx.SystemsGet(It.IsAny<int>())).Returns(mockSystem.Object);
            _context.Setup(ctx => ctx.SaveChanges()).Throws(new Exception("Database error"));
            SetupMockRepo();

            var systemInfoRepo = _mockRepo.Object;

            _testSystem.IsNew = false;
            _testSystem.IsDirty = true;

            await Assert.ThrowsAsync<Exception>(() => systemInfoRepo.WriteAsync(_testSystem));
        }
    }
}
