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
    public class TowerTableRepositoryTests : RepositoryTestBase<TowerTableRepository>
    {
        private readonly TowerTables _testTowerTables1 = new TowerTables
        {
            ID = 1,
            SystemID = 140,
            TowerID = 1013,
            TableID = 1,
            BaseFrequency = "851.00625",
            Spacing = "0.00625",
            InputOffset = "-45.00000",
            AssumedConfirmed = "Confirmed",
            Bandwidth = "0.01250",
            Slots = 1,
            LastModified = DateTime.Now
        };

        private readonly TowerTables _testTowerTables2 = new TowerTables
        {
            ID = 2,
            SystemID = 140,
            TowerID = 1013,
            TableID = 3,
            BaseFrequency = "762.00625",
            Spacing = "0.01250",
            InputOffset = "30.00000",
            AssumedConfirmed = "Confirmed",
            Bandwidth = "0.01250",
            Slots = 2,
            LastModified = DateTime.Now
        };

        private readonly TowerTable _testTowerTable = new TowerTable
        {
            SystemID = 140,
            TowerID = 1013,
            TableID = 1,
            BaseFrequency = "851.00625",
            Spacing = "0.00625",
            InputOffset = "-45.00000",
            AssumedConfirmed = "Confirmed",
            Bandwidth = "0.01250",
            Slots = 1
        };

        private IEnumerator<TowerTables> GetTowerTable()
        {
            yield return _testTowerTables2;
        }

        private IEnumerator<TowerTables> GetTowerTables()
        {
            yield return _testTowerTables1;
            yield return _testTowerTables2
;
        }

        [Fact]
        public async Task GetAsyncReturnsAppropriateValues()
        {
            var mockTowerTables = new Mock<MockObjectResult<TowerTables>>();

            mockTowerTables.Setup(tt => tt.GetEnumerator()).Returns(GetTowerTable);
            _context.Setup(ctx => ctx.TowerTablesGetForTowerTable(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerTables.Object);
            SetupMockRepo();

            var towerTableRepo = _mockRepo.Object;

            var result = await towerTableRepo.GetAsync(140, 1031, 3);

            Assert.NotNull(result);
            Assert.Equal(result.ID, _testTowerTables2.ID);
            Assert.Equal(result.SystemID, _testTowerTables2.SystemID);
            Assert.Equal(result.TowerID, _testTowerTables2.TowerID);
            Assert.Equal(result.TableID, _testTowerTables2.TableID);
            Assert.Equal(result.BaseFrequency, _testTowerTables2.BaseFrequency);
            Assert.Equal(result.Spacing, _testTowerTables2.Spacing);
            Assert.Equal(result.InputOffset, _testTowerTables2.InputOffset);
            Assert.Equal(result.AssumedConfirmed, _testTowerTables2.AssumedConfirmed);
            Assert.Equal(result.Bandwidth, _testTowerTables2.Bandwidth);
            Assert.Equal(result.Slots, _testTowerTables2.Slots);
            Assert.Equal(result.LastModified, _testTowerTables2.LastModified);
        }

        [Fact]
        public async Task GetAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerTablesGetForTowerTable(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerTableRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerTableRepo.GetAsync(140, 1031, 3));
        }

        [Fact]
        public async Task GetListForTowerAsyncReturnsAppropriateTypes()
        {
            var mockTowerTables = new Mock<MockObjectResult<TowerTables>>();

            mockTowerTables.Setup(tt => tt.GetEnumerator()).Returns(GetTowerTables);
            _context.Setup(ctx => ctx.TowerTablesGetForTower(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerTables.Object);
            SetupMockRepo();

            var towerTableRepo = _mockRepo.Object;

            var results = await towerTableRepo.GetListForTowerAsync(1, 1013);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<TowerTable>>(results);
            Assert.True(results.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetListForTowerAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerTablesGetForTower(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerTableRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerTableRepo.GetListForTowerAsync(1, 1013));
        }

        [Fact]
        public void EditRecordSetsAppropriateValues()
        {
            var databaseRecord = new TowerTables();
            var towerTableRepo = _mockRepo.Object;

            towerTableRepo.EditRecord(databaseRecord, _testTowerTable);

            Assert.Equal(_testTowerTable.SystemID, databaseRecord.SystemID);
            Assert.Equal(_testTowerTable.TowerID, databaseRecord.TowerID);
            Assert.Equal(_testTowerTable.TableID, databaseRecord.TableID);
            Assert.Equal(_testTowerTable.BaseFrequency, databaseRecord.BaseFrequency);
            Assert.Equal(_testTowerTable.Spacing, databaseRecord.Spacing);
            Assert.Equal(_testTowerTable.InputOffset, databaseRecord.InputOffset);
            Assert.Equal(_testTowerTable.AssumedConfirmed, databaseRecord.AssumedConfirmed);
            Assert.Equal(_testTowerTable.Bandwidth, databaseRecord.Bandwidth);
            Assert.Equal(_testTowerTable.Slots, databaseRecord.Slots);
        }

        [Fact]
        public async Task WriteAsyncAddsNewRecord()
        {
            var addTowerTable = 0;
            var saveChanges = 0;
            var count = 0;

            _context.Setup(ctx => ctx.TowerTables.Create()).Returns(new TowerTables() { ID = 10 });
            _context.Setup(ctx => ctx.TowerTables.Add(It.IsAny<TowerTables>())).Callback(() => addTowerTable = ++count);
            _context.Setup(ctx => ctx.SaveChanges()).Callback(() => saveChanges = ++count);
            SetupMockRepo();

            _testTowerTable.IsNew = true;
            _testTowerTable.IsDirty = true;

            var towerFrequencyRepo = _mockRepo.Object;

            await towerFrequencyRepo.WriteAsync(_testTowerTable);

            _context.Verify(ctx => ctx.TowerTables.Add(It.IsAny<TowerTables>()), Times.Once());
            _context.Verify(ctx => ctx.SaveChanges(), Times.Once());

            Assert.Equal(1, addTowerTable);
            Assert.Equal(2, saveChanges);

            Assert.Equal(10, _testTowerTable.ID);
            Assert.False(_testTowerTable.IsNew);
            Assert.False(_testTowerTable.IsDirty);
        }

        [Fact]
        public async Task WriteAsyncUpdatesRecord()
        {
            var saveChanges = 0;
            var count = 0;
            var mockTowerTable = new Mock<MockObjectResult<TowerTables>>();

            mockTowerTable.Setup(s => s.GetEnumerator()).Returns(GetTowerTable);
            _context.Setup(ctx => ctx.TowerTablesGet(It.IsAny<int>())).Returns(mockTowerTable.Object);
            _context.Setup(ctx => ctx.SaveChanges()).Callback(() => saveChanges = ++count);
            SetupMockRepo();

            var towerTableRepo = _mockRepo.Object;

            _testTowerTable.IsNew = false;
            _testTowerTable.IsDirty = true;

            await towerTableRepo.WriteAsync(_testTowerTable);

            _context.Verify(ctx => ctx.SaveChanges(), Times.Once());

            Assert.Equal(1, saveChanges);

            Assert.False(_testTowerTable.IsNew);
            Assert.False(_testTowerTable.IsDirty);
        }

        [Fact]
        public async Task AddRecordAsyncThrowsExceptionWithDatabaseException()
        {
            var addSystem = 0;
            var count = 0;

            _context.Setup(ctx => ctx.TowerTables.Create()).Returns(new TowerTables());
            _context.Setup(ctx => ctx.TowerTables.Add(It.IsAny<TowerTables>())).Callback(() => addSystem = count++);
            _context.Setup(ctx => ctx.SaveChanges()).Throws(new Exception("Database error"));
            SetupMockRepo();

            _testTowerTable.IsNew = true;

            var towerFrequencyRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerFrequencyRepo.WriteAsync(_testTowerTable));
        }

        [Fact]
        public async Task UpdateRecordAsyncThrowsExceptionWithDatabaseException()
        {
            var mockTowerTable = new Mock<MockObjectResult<TowerTables>>();

            mockTowerTable.Setup(s => s.GetEnumerator()).Returns(GetTowerTable);
            _context.Setup(ctx => ctx.TowerTablesGet(It.IsAny<int>())).Returns(mockTowerTable.Object);
            _context.Setup(ctx => ctx.SaveChanges()).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerFrequencyRepo = _mockRepo.Object;

            _testTowerTable.IsNew = false;
            _testTowerTable.IsDirty = true;

            await Assert.ThrowsAsync<Exception>(() => towerFrequencyRepo.WriteAsync(_testTowerTable));
        }

        [Fact]
        public async Task DeleteForTowerAsyncDeletesRecord()
        {
            var deleteRecord = 0;
            var count = 0;

            _context.Setup(ctx => ctx.TowerTablesDeleteForTower(It.IsAny<int>(), It.IsAny<int>())).Callback(() => deleteRecord = ++count);
            SetupMockRepo();

            var towerTableRepo = _mockRepo.Object;

            await towerTableRepo.DeleteForTowerAsync(1, 1013);

            Assert.Equal(1, deleteRecord);
        }

        [Fact]
        public async Task DeleteAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerTablesDeleteForTower(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerFrequencyRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerFrequencyRepo.DeleteForTowerAsync(1, 1013));
        }
    }
}
