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
    public class TowerNeighborRepositoryTests : RepositoryTestBase<TowerNeighborRepository>
    {
        private readonly TowerNeighbor _testTowerNeighbor = new TowerNeighbor
        {
            SystemID = 140,
            TowerNumber = 1001,
            NeighborSystemID = 140,
            NeighborTowerID = 1015,
            NeighborTowerNumberHex = "T010F",
            NeighborChannel = "00-0309",
            NeighborFrequency = "852.93750",
            NeighborTowerName = "Lake County",
            FirstSeen = DateTime.Now.AddYears(-4),
            LastSeen = DateTime.Now.AddDays(-1)
        };

        private readonly TowerNeighbors _testTowerNeighbors1 = new TowerNeighbors
        {
            ID = 1,
            SystemID = 140,
            TowerID = 1001,
            NeighborSystemID = 140,
            NeighborTowerID = 1015,
            NeighborTowerNumberHex = "T010F",
            NeighborChannel = "00-0309",
            NeighborFrequency = "852.93750",
            NeighborTowerName = "Lake County",
            FirstSeen = DateTime.Now.AddYears(-4),
            LastSeen = DateTime.Now.AddDays(-1),
            LastModified = DateTime.Now
        };

        private readonly TowerNeighbors _testTowerNeighbors2 = new TowerNeighbors
        {
            ID = 1,
            SystemID = 140,
            TowerID = 1001,
            NeighborSystemID = 140,
            NeighborTowerID = 1049,
            NeighborTowerNumberHex = "T0131",
            NeighborChannel = "00-0085",
            NeighborFrequency = "851.53750",
            NeighborTowerName = "Chicago North (Cook)",
            FirstSeen = DateTime.Now.AddYears(-5),
            LastSeen = DateTime.Now.AddDays(-2),
            LastModified = DateTime.Now
        };

        private readonly TowerNeighbors_Result _testTowerNeighborResults1 = new TowerNeighbors_Result
        {
            NeighborTowerNumber = 1015,
            NeighborTowerName = "Lake County",
            NeighborFrequency = "852.93750",
            FirstSeen = DateTime.Now.AddYears(-4),
            LastSeen = DateTime.Now.AddDays(-1),
            RecordCount = 2
        };

        private readonly TowerNeighbors_Result _testTowerNeighborResults2 = new TowerNeighbors_Result
        {
            NeighborTowerNumber = 1049,
            NeighborTowerName = "Chicago North (Cook)",
            NeighborFrequency = "851.53750",
            FirstSeen = DateTime.Now.AddYears(-5),
            LastSeen = DateTime.Now.AddDays(-2),
            RecordCount = 2
        };

        private IEnumerator<TowerNeighbors> GetTowerNeighbor()
        {
            yield return _testTowerNeighbors1;
        }

        private IEnumerator<TowerNeighbors_Result> GetTowerNeighborResults()
        {
            yield return _testTowerNeighborResults1;
            yield return _testTowerNeighborResults2;
        }

        [Fact]
        public async Task GetAsyncReturnsAppropriateValues()
        {
            var mockTowerNeighbor = new Mock<MockObjectResult<TowerNeighbors>>();

            mockTowerNeighbor.Setup(tn => tn.GetEnumerator()).Returns(GetTowerNeighbor);
            _context.Setup(ctx => ctx.TowerNeighborsGetForSystemTower(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(mockTowerNeighbor.Object);
            SetupMockRepo();

            var towerNeighorRepo = _mockRepo.Object;

            var result = await towerNeighorRepo.GetAsync(140, 1001, 140, 1015);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<TowerNeighbor>(result);
            Assert.Equal(_testTowerNeighbors1.ID, result.ID);
            Assert.Equal(_testTowerNeighbors1.SystemID, result.SystemID);
            Assert.Equal(_testTowerNeighbors1.TowerID, result.TowerNumber);
            Assert.Equal(_testTowerNeighbors1.NeighborSystemID, result.NeighborSystemID);
            Assert.Equal(_testTowerNeighbors1.NeighborTowerID, result.NeighborTowerID);
            Assert.Equal(_testTowerNeighbors1.NeighborTowerNumberHex, result.NeighborTowerNumberHex);
            Assert.Equal(_testTowerNeighbors1.NeighborChannel, result.NeighborChannel);
            Assert.Equal(_testTowerNeighbors1.NeighborFrequency, result.NeighborFrequency);
            Assert.Equal(_testTowerNeighbors1.NeighborTowerName, result.NeighborTowerName);
            Assert.Equal(_testTowerNeighbors1.FirstSeen, result.FirstSeen);
            Assert.Equal(_testTowerNeighbors1.LastSeen, result.LastSeen);
            Assert.Equal(_testTowerNeighbors1.LastModified, result.LastModified);
        }

        [Fact]
        public async Task GetAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerNeighborsGetForSystemTower(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerNeighorRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerNeighorRepo.GetAsync(140, 1001, 140, 1015));
        }

        [Fact]
        public async Task GetForSystemTowerNumberAsyncReturnsAppropriateTypes()
        {
            var mockTowerNeighbor = new Mock<MockObjectResult<TowerNeighbors>>();

            mockTowerNeighbor.Setup(tn => tn.GetEnumerator()).Returns(GetTowerNeighbor);
            _context.Setup(ctx => ctx.TowerNeighborsGetForSystemTowerNumber(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(mockTowerNeighbor.Object);
            SetupMockRepo();

            var towerNeighborsRepo = _mockRepo.Object;

            var results = await towerNeighborsRepo.GetForSystemTowerNumberAsync(140, 1001, 140, 1049);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<TowerNeighbor>(results);
        }

        [Fact]
        public async Task GetForSystemTowerNumberAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerNeighborsGetForSystemTowerNumber(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerNeighborsRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerNeighborsRepo.GetForSystemTowerNumberAsync(140, 1001, 140, 1049));
        }

        [Fact]
        public async Task GetNeighborsForTowerAsyncReturnsAppropriateTypes()
        {
            var mockTowerNeighorResults = new Mock<MockObjectResult<TowerNeighbors_Result>>();

            mockTowerNeighorResults.Setup(tnr => tnr.GetEnumerator()).Returns(GetTowerNeighborResults);
            _context.Setup(ctx => ctx.TowerNeighborsGetNeighborsForTower(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerNeighorResults.Object);
            SetupMockRepo();

            var towerNeighborRepo = _mockRepo.Object;

            var results = await towerNeighborRepo.GetNeighborsForTowerAsync(140, 1001);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<TowerNeighbor>>(results);
            Assert.True(results.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetNeighborsForTowerAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerNeighborsGetNeighborsForTower(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerNeighborRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerNeighborRepo.GetNeighborsForTowerAsync(140, 1001));
        }

        [Fact]
        public async Task GetNeighborsForTowerAsyncWithFiltersReturnsAppropriateTypes()
        {
            var mockTowerNeighborResults = new Mock<MockObjectResult<TowerNeighbors_Result>>();

            mockTowerNeighborResults.Setup(tnr => tnr.GetEnumerator()).Returns(GetTowerNeighborResults);
            _context.Setup(ctx => ctx.TowerNeighborsGetNeighborsForSystemTowerFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerNeighborResults.Object);
            SetupMockRepo();

            var towerNeighborRepo = _mockRepo.Object;

            var (towerNeighbors, recordCount) = await towerNeighborRepo.GetNeighborsForTowerAsync("140", 1001, _filterData);

            Assert.NotNull(towerNeighbors);
            Assert.IsAssignableFrom<IEnumerable<TowerNeighbor>>(towerNeighbors);
            Assert.True(towerNeighbors.Count() > 0, "Result count is 0");
            Assert.IsAssignableFrom<int>(recordCount);
            Assert.NotEqual(0, recordCount);
        }

        [Fact]
        public async Task GetNeighborsForTowerAsyncWithFiltersReturnsAppropriateValues()
        {
            var mockTowerNeighborResults = new Mock<MockObjectResult<TowerNeighbors_Result>>();

            mockTowerNeighborResults.Setup(tnr => tnr.GetEnumerator()).Returns(GetTowerNeighborResults);
            _context.Setup(ctx => ctx.TowerNeighborsGetNeighborsForSystemTowerFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerNeighborResults.Object);
            SetupMockRepo();

            var towerNeighborRepo = _mockRepo.Object;

            var (towerNeighbors, recordCount) = await towerNeighborRepo.GetNeighborsForTowerAsync("140", 1001, _filterData);
            var resultData = towerNeighbors.SingleOrDefault(rd => rd.NeighborTowerID == 1015);

            Assert.NotNull(resultData);
            Assert.Equal(_testTowerNeighborResults1.NeighborTowerNumber, resultData.NeighborTowerID);
            Assert.Equal(_testTowerNeighborResults1.NeighborTowerName, resultData.NeighborTowerName);
            Assert.Equal(_testTowerNeighborResults1.NeighborFrequency, resultData.NeighborFrequency);
            Assert.Equal(_testTowerNeighborResults1.FirstSeen, resultData.FirstSeen);
            Assert.Equal(_testTowerNeighborResults1.LastSeen, resultData.LastSeen);

            Assert.Equal(_testTowerNeighborResults1.RecordCount, recordCount);
        }

        [Fact]
        public async Task GetNeighborsForTowerAsyncWithFiltersThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerNeighborsGetNeighborsForSystemTowerFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerNeighborRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerNeighborRepo.GetNeighborsForTowerAsync("140", 1001, _filterData));
        }

        [Fact]
        public void EditRecordSetsAppropriateValues()
        {
            var databaseRecord = new TowerNeighbors();
            var towerNeighborRepo = _mockRepo.Object;

            towerNeighborRepo.EditRecord(databaseRecord, _testTowerNeighbor);

            Assert.Equal(_testTowerNeighbor.SystemID, databaseRecord.SystemID);
            Assert.Equal(_testTowerNeighbor.TowerNumber, databaseRecord.TowerID);
            Assert.Equal(_testTowerNeighbor.NeighborSystemID, databaseRecord.NeighborSystemID);
            Assert.Equal(_testTowerNeighbor.NeighborTowerID, databaseRecord.NeighborTowerID);
            Assert.Equal(_testTowerNeighbor.NeighborTowerNumberHex, databaseRecord.NeighborTowerNumberHex);
            Assert.Equal(_testTowerNeighbor.NeighborChannel, databaseRecord.NeighborChannel);
            Assert.Equal(_testTowerNeighbor.NeighborFrequency, databaseRecord.NeighborFrequency);
            Assert.Equal(_testTowerNeighbor.NeighborTowerName, databaseRecord.NeighborTowerName);
            Assert.Equal(_testTowerNeighbor.FirstSeen, databaseRecord.FirstSeen);
            Assert.Equal(_testTowerNeighbor.LastSeen, databaseRecord.LastSeen);
        }

        [Fact]
        public async Task WriteAsyncAddsNewRecord()
        {
            var addTowerNeighbor = 0;
            var saveChanges = 0;
            var count = 0;

            _context.Setup(ctx => ctx.TowerNeighbors.Create()).Returns(new TowerNeighbors() { ID = 10 });
            _context.Setup(ctx => ctx.TowerNeighbors.Add(It.IsAny<TowerNeighbors>())).Callback(() => addTowerNeighbor = ++count);
            _context.Setup(ctx => ctx.SaveChanges()).Callback(() => saveChanges = ++count);
            SetupMockRepo();

            _testTowerNeighbor.IsNew = true;
            _testTowerNeighbor.IsDirty = true;

            var towerNeighborRepo = _mockRepo.Object;

            await towerNeighborRepo.WriteAsync(_testTowerNeighbor);

            _context.Verify(ctx => ctx.TowerNeighbors.Add(It.IsAny<TowerNeighbors>()), Times.Once());
            _context.Verify(ctx => ctx.SaveChanges(), Times.Once());

            Assert.Equal(1, addTowerNeighbor);
            Assert.Equal(2, saveChanges);

            Assert.Equal(10, _testTowerNeighbor.ID);
            Assert.False(_testTowerNeighbor.IsNew);
            Assert.False(_testTowerNeighbor.IsDirty);
        }

        [Fact]
        public async Task WriteAsyncUpdatesRecord()
        {
            var saveChanges = 0;
            var count = 0;
            var mockTowerFrequencyNeighbor = new Mock<MockObjectResult<TowerNeighbors>>();

            mockTowerFrequencyNeighbor.Setup(s => s.GetEnumerator()).Returns(GetTowerNeighbor);
            _context.Setup(ctx => ctx.TowerNeighborsGet(It.IsAny<int>())).Returns(mockTowerFrequencyNeighbor.Object);
            _context.Setup(ctx => ctx.SaveChanges()).Callback(() => saveChanges = ++count);
            SetupMockRepo();

            var towerNeighborRepo = _mockRepo.Object;

            _testTowerNeighbor.IsNew = false;
            _testTowerNeighbor.IsDirty = true;

            await towerNeighborRepo.WriteAsync(_testTowerNeighbor);

            _context.Verify(ctx => ctx.SaveChanges(), Times.Once());

            Assert.Equal(1, saveChanges);

            Assert.False(_testTowerNeighbor.IsNew);
            Assert.False(_testTowerNeighbor.IsDirty);
        }

        [Fact]
        public async Task AddRecordAsyncThrowsExceptionWithDatabaseException()
        {
            var addSystem = 0;
            var count = 0;

            _context.Setup(ctx => ctx.TowerNeighbors.Create()).Returns(new TowerNeighbors());
            _context.Setup(ctx => ctx.TowerNeighbors.Add(It.IsAny<TowerNeighbors>())).Callback(() => addSystem = count++);
            _context.Setup(ctx => ctx.SaveChanges()).Throws(new Exception("Database error"));
            SetupMockRepo();

            _testTowerNeighbor.IsNew = true;

            var towerFrequencyRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerFrequencyRepo.WriteAsync(_testTowerNeighbor));
        }

        [Fact]
        public async Task UpdateRecordAsyncThrowsExceptionWithDatabaseException()
        {
            var mockTowerNeighbor = new Mock<MockObjectResult<TowerNeighbors>>();

            mockTowerNeighbor.Setup(s => s.GetEnumerator()).Returns(GetTowerNeighbor);
            _context.Setup(ctx => ctx.TowerNeighborsGet(It.IsAny<int>())).Returns(mockTowerNeighbor.Object);
            _context.Setup(ctx => ctx.SaveChanges()).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerFrequencyRepo = _mockRepo.Object;

            _testTowerNeighbor.IsNew = false;
            _testTowerNeighbor.IsDirty = true;

            await Assert.ThrowsAsync<Exception>(() => towerFrequencyRepo.WriteAsync(_testTowerNeighbor));
        }

        [Fact]
        public async Task DeleteForTowersAsyncDeletesRecord()
        {
            var deleteRecord = 0;
            var count = 0;

            _context.Setup(ctx => ctx.TowerNeighborsDeleteForTower(It.IsAny<int>(), It.IsAny<int>())).Callback(() => deleteRecord = ++count);
            SetupMockRepo();

            var towerNeighborRepo = _mockRepo.Object;

            await towerNeighborRepo.DeleteForTowerAsync(1, 1001);

            Assert.Equal(1, deleteRecord);
        }

        [Fact]
        public async Task DeleteForTowersAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerNeighborsDeleteForTower(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerFrequencyRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerFrequencyRepo.DeleteForTowerAsync(1, 1001));
        }
    }
}
