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
    public class TowerRespositoryTests : RepositoryTestBase<TowerRepository>
    {
        private readonly Tower _testTower = new Tower
        {
            SystemID = 140,
            TowerNumber = 1031,
            TowerNumberHex = "T011F",
            Description = "LaSalle (LaSalle)",
            WACN = "BEE00",
            ControlCapabilities = "Data,Voice,Registration",
            Flavor = "Phase 2",
            CallSigns = "WQIU454 WQCZ742",
            TimeStamp = DateTime.Now.AddDays(-10)
        };

        private readonly Towers _testTower1 = new Towers
        {
            ID = 1,
            SystemID = 140,
            TowerNumber = 1031,
            TowerNumberHex = "T011F",
            Description = "LaSalle (LaSalle)",
            HitCount = 394031,
            WACN = "BEE00",
            ControlCapabilities = "Data,Voice,Registration",
            Flavor = "Phase 2",
            CallSigns = "WQIU454 WQCZ742",
            TimeStamp = DateTime.Now.AddDays(-3),
            FirstSeen = DateTime.Now.AddYears(-8),
            LastSeen = DateTime.Now.AddDays(-2),
            LastModified = DateTime.Now
        };

        private readonly Towers _testTower2 = new Towers
        {
            ID = 2,
            SystemID = 140,
            TowerNumber = 1034,
            TowerNumberHex = "T0122",
            Description = "Ottawa (LaSalle)",
            HitCount = 902392,
            WACN = "BEE00",
            ControlCapabilities = "Data,Voice,Registration",
            Flavor = "Phase 2",
            CallSigns = "WQCZ741 WQIU454",
            TimeStamp = DateTime.Now.AddDays(-5),
            FirstSeen = DateTime.Now.AddYears(-10),
            LastSeen = DateTime.Now.AddDays(-4),
            LastModified = DateTime.Now
        };

        private readonly Towers_Result _testTowerResult1 = new Towers_Result
        {
            TowerNumber = 1031,
            Description = "LaSalle (LaSalle)",
            HitCount = 3920392,
            FirstSeen = DateTime.Now.AddYears(-4),
            LastSeen = DateTime.Now.AddDays(-3),
            RecordCount = 2
        };

        private readonly Towers_Result _testTowerResult2 = new Towers_Result
        {
            TowerNumber = 1034,
            Description = "Ottawa (LaSalle)",
            HitCount = 4803192,
            FirstSeen = DateTime.Now.AddYears(-10),
            LastSeen = DateTime.Now.AddDays(-1),
            RecordCount = 2
        };

        private readonly int? _testCount = 39021;

        private IEnumerator<Towers> GetTower()
        {
            yield return _testTower1;
        }

        private IEnumerator<Towers> GetTowers()
        {
            yield return _testTower1;
            yield return _testTower2;
        }

        private IEnumerator<Towers_Result> GetTowerResults()
        {
            yield return _testTowerResult1;
            yield return _testTowerResult2;
        }

        private IEnumerator<int?> GetTowerCount()
        {
            yield return _testCount;
        }

        [Fact]
        public async Task GetAsyncReturnsAppropriateTypes()
        {
            var mockTower = new Mock<MockObjectResult<Towers>>();

            mockTower.Setup(t => t.GetEnumerator()).Returns(GetTower);
            _context.Setup(ctx => ctx.TowersGet(It.IsAny<int>())).Returns(mockTower.Object);
            SetupMockRepo();

            var towerRepo = _mockRepo.Object;

            var results = await towerRepo.GetAsync(1031);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<Tower>(results);
        }

        [Fact]
        public async Task GetAsyncReturnsAppropriateValues()
        {
            var mockTower = new Mock<MockObjectResult<Towers>>();

            mockTower.Setup(t => t.GetEnumerator()).Returns(GetTower);
            _context.Setup(ctx => ctx.TowersGet(It.IsAny<int>())).Returns(mockTower.Object);
            SetupMockRepo();

            var towerRepo = _mockRepo.Object;

            var results = await towerRepo.GetAsync(1031);

            Assert.Equal(_testTower1.ID, results.ID);
            Assert.Equal(_testTower1.SystemID, results.SystemID);
            Assert.Equal(_testTower1.TowerNumber, results.TowerNumber);
            Assert.Equal(_testTower1.TowerNumberHex, results.TowerNumberHex);
            Assert.Equal(_testTower1.Description, results.Description);
            Assert.Equal(_testTower1.HitCount, results.HitCount);
            Assert.Equal(_testTower1.WACN, results.WACN);
            Assert.Equal(_testTower1.ControlCapabilities, results.ControlCapabilities);
            Assert.Equal(_testTower1.Flavor, results.Flavor);
            Assert.Equal(_testTower1.CallSigns, results.CallSigns);
            Assert.Equal(_testTower1.TimeStamp, results.TimeStamp);
            Assert.Equal(_testTower1.FirstSeen, results.FirstSeen);
            Assert.Equal(_testTower1.LastSeen, results.LastSeen);
            Assert.Equal(_testTower1.LastModified, results.LastModified);

        }

        [Fact]
        public async Task GetAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowersGet(It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerRepo.GetAsync(1031));
        }

        [Fact]
        public async Task GetAsyncSystemTowerReturnsAppropriateTypes()
        {
            var mockTower = new Mock<MockObjectResult<Towers>>();

            mockTower.Setup(t => t.GetEnumerator()).Returns(GetTower);
            _context.Setup(ctx => ctx.TowersGetForSystemTower(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTower.Object);
            SetupMockRepo();

            var towerRepo = _mockRepo.Object;

            var results = await towerRepo.GetAsync(140, 1031);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<Tower>(results);
        }

        [Fact]
        public async Task GetAsyncSystemTowerThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowersGetForSystemTower(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerRepo.GetAsync(140, 1031));
        }

        [Fact]
        public async Task GetForSystemAsyncReturnsAppropriateTypes()
        {
            var mockTowers = new Mock<MockObjectResult<Towers>>();

            mockTowers.Setup(t => t.GetEnumerator()).Returns(GetTowers);
            _context.Setup(ctx => ctx.TowersGetForSystem(It.IsAny<int>())).Returns(mockTowers.Object);
            SetupMockRepo();

            var towerRepo = _mockRepo.Object;

            var results = await towerRepo.GetForSystemAsync(140);

            Assert.NotNull(results);
            Assert.True(results.Count() > 0, "Result count is 0");
            Assert.IsAssignableFrom<IEnumerable<Tower>>(results);
        }

        [Fact]
        public async Task GetForSystemAsyncReturnsAppropriateValues()
        {
            var mockTowers = new Mock<MockObjectResult<Towers>>();

            mockTowers.Setup(t => t.GetEnumerator()).Returns(GetTowers);
            _context.Setup(ctx => ctx.TowersGetForSystem(It.IsAny<int>())).Returns(mockTowers.Object);
            SetupMockRepo();

            var towerRepo = _mockRepo.Object;

            var results = await towerRepo.GetForSystemAsync(140);
            var resultsData = results.SingleOrDefault(rd => rd.ID == 2);

            Assert.NotNull(resultsData);
            Assert.Equal(resultsData.ID, _testTower2.ID);
            Assert.Equal(resultsData.SystemID, _testTower2.SystemID);
            Assert.Equal(resultsData.TowerNumber, _testTower2.TowerNumber);
            Assert.Equal(resultsData.TowerNumberHex, _testTower2.TowerNumberHex);
            Assert.Equal(resultsData.Description, _testTower2.Description);
            Assert.Equal(resultsData.HitCount, _testTower2.HitCount);
            Assert.Equal(resultsData.WACN, _testTower2.WACN);
            Assert.Equal(resultsData.ControlCapabilities, _testTower2.ControlCapabilities);
            Assert.Equal(resultsData.Flavor, _testTower2.Flavor);
            Assert.Equal(resultsData.CallSigns, _testTower2.CallSigns);
            Assert.Equal(resultsData.TimeStamp, _testTower2.TimeStamp);
            Assert.Equal(resultsData.FirstSeen, _testTower2.FirstSeen);
            Assert.Equal(resultsData.LastSeen, _testTower2.LastSeen);
            Assert.Equal(resultsData.LastModified, _testTower2.LastModified);
        }

        [Fact]
        public async Task GetForSystemAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowersGetForSystem(It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerRepo.GetForSystemAsync(140));
        }

        [Fact]
        public async Task GetForSystemAsyncWithFiltersActiveReturnsAppropriateTypes()
        {
            var mockTowerResults = new Mock<MockObjectResult<Towers_Result>>();

            mockTowerResults.Setup(tr => tr.GetEnumerator()).Returns(GetTowerResults);
            _context.Setup(ctx => ctx.TowersGetForSystemActiveFiltersWithPaging(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerResults.Object);
            SetupMockRepo();

            var towerRepo = _mockRepo.Object;

            var (towers, recordCount) = await towerRepo.GetForSystemAsync("140", true, _filterData);

            Assert.NotNull(towers);
            Assert.True(towers.Count() > 0, "Result count is 0");
            Assert.IsAssignableFrom<IEnumerable<Tower>>(towers);
            Assert.Equal(_testTowerResult2.RecordCount, recordCount);
        }

        [Fact]
        public async Task GetForSystemAsyncWithFiltersActiveThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowersGetForSystemActiveFiltersWithPaging(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerRepo.GetForSystemAsync("140", true, _filterData));
        }

        [Fact]
        public async Task GetForSystemAsyncWithFiltersInactiveReturnsAppropriateTypes()
        {
            var mockTowerResults = new Mock<MockObjectResult<Towers_Result>>();

            mockTowerResults.Setup(tr => tr.GetEnumerator()).Returns(GetTowerResults);
            _context.Setup(ctx => ctx.TowersGetForSystemFiltersWithPaging(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerResults.Object);
            SetupMockRepo();

            var towerRepo = _mockRepo.Object;

            var (towers, recordCount) = await towerRepo.GetForSystemAsync("140", false, _filterData);

            Assert.NotNull(towers);
            Assert.True(towers.Count() > 0, "Result count is 0");
            Assert.IsAssignableFrom<IEnumerable<Tower>>(towers);
            Assert.Equal(_testTowerResult2.RecordCount, recordCount);
        }

        [Fact]
        public async Task GetForSystemAsyncWithFiltersInactiveThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowersGetForSystemFiltersWithPaging(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerRepo.GetForSystemAsync("140", false, _filterData));
        }

        [Fact]
        public async Task GetCountForSystemAsyncReturnsACount()
        {
            var mockTowerCount = new Mock<MockObjectResult<int?>>();

            mockTowerCount.Setup(tc => tc.GetEnumerator()).Returns(GetTowerCount);
            _context.Setup(ctx => ctx.TowersGetCountForSystem(It.IsAny<int>())).Returns(mockTowerCount.Object);
            SetupMockRepo();

            var towerRepo = _mockRepo.Object;

            var results = await towerRepo.GetCountForSystemAsync(140);

            Assert.Equal(_testCount, results);
        }

        [Fact]
        public async Task GetCountForSystemAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowersGetCountForSystem(It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerRepo.GetCountForSystemAsync(140));
        }

        [Fact]
        public void EditRecordSetsAppropriateValues()
        {
            var databaseRecord = new Towers();
            var towerRepo = _mockRepo.Object;

            towerRepo.EditRecord(databaseRecord, _testTower);

            Assert.Equal(_testTower.SystemID, databaseRecord.SystemID);
            Assert.Equal(_testTower.TowerNumber, databaseRecord.TowerNumber);
            Assert.Equal(_testTower.TowerNumberHex, databaseRecord.TowerNumberHex);
            Assert.Equal(_testTower.Description, databaseRecord.Description);
            Assert.Equal(_testTower.WACN, databaseRecord.WACN);
            Assert.Equal(_testTower.ControlCapabilities, databaseRecord.ControlCapabilities);
            Assert.Equal(_testTower.Flavor, databaseRecord.Flavor);
            Assert.Equal(_testTower.CallSigns, databaseRecord.CallSigns);
            Assert.Equal(_testTower.TimeStamp, databaseRecord.TimeStamp);
        }

        [Fact]
        public async Task WriteAsyncAddsNewRecord()
        {
            var addTower = 0;
            var saveChanges = 0;
            var count = 0;

            _context.Setup(ctx => ctx.Towers.Create()).Returns(new Towers() { ID = 10 });
            _context.Setup(ctx => ctx.Towers.Add(It.IsAny<Towers>())).Callback(() => addTower = ++count);
            _context.Setup(ctx => ctx.SaveChanges()).Callback(() => saveChanges = ++count);
            SetupMockRepo();

            _testTower.IsNew = true;
            _testTower.IsDirty = true;

            var towerNeighborRepo = _mockRepo.Object;

            await towerNeighborRepo.WriteAsync(_testTower);

            _context.Verify(ctx => ctx.Towers.Add(It.IsAny<Towers>()), Times.Once());
            _context.Verify(ctx => ctx.SaveChanges(), Times.Once());

            Assert.Equal(1, addTower);
            Assert.Equal(2, saveChanges);

            Assert.Equal(10, _testTower.ID);
            Assert.False(_testTower.IsNew);
            Assert.False(_testTower.IsDirty);
        }

        [Fact]
        public async Task WriteAsyncUpdatesRecord()
        {
            var saveChanges = 0;
            var count = 0;
            var mockTower = new Mock<MockObjectResult<Towers>>();

            mockTower.Setup(s => s.GetEnumerator()).Returns(GetTower);
            _context.Setup(ctx => ctx.TowersGet(It.IsAny<int>())).Returns(mockTower.Object);
            _context.Setup(ctx => ctx.SaveChanges()).Callback(() => saveChanges = ++count);
            SetupMockRepo();

            var towerNeighborRepo = _mockRepo.Object;

            _testTower.IsNew = false;
            _testTower.IsDirty = true;

            await towerNeighborRepo.WriteAsync(_testTower);

            _context.Verify(ctx => ctx.SaveChanges(), Times.Once());

            Assert.Equal(1, saveChanges);

            Assert.False(_testTower.IsNew);
            Assert.False(_testTower.IsDirty);
        }
    }
}

