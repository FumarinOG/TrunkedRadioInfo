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
    public class TowerRadioRepositoryTests : RepositoryTestBase<TowerRadioRepository>
    {
        private readonly TowerRadios _testTowerRadio1 = new TowerRadios
        {
            ID = 1,
            SystemID = 140,
            TowerID = 1031,
            RadioID = 434000,
            Date = DateTime.Now.AddDays(-1),
            AffiliationCount = 250,
            DeniedCount = 500,
            VoiceGrantCount = 750,
            EmergencyVoiceGrantCount = 1000,
            EncryptedVoiceGrantCount = 1250,
            DataCount = 1500,
            PrivateDataCount = 1750,
            AlertCount = 2000,
            FirstSeen = DateTime.Now.AddYears(-3),
            LastSeen = DateTime.Now.AddHours(-10),
            LastModified = DateTime.Now
        };

        private readonly TowerRadios _testTowerRadio2 = new TowerRadios
        {
            ID = 2,
            SystemID = 140,
            TowerID = 1031,
            RadioID = 434001,
            Date = DateTime.Now.AddDays(-2),
            AffiliationCount = 500,
            DeniedCount = 1000,
            VoiceGrantCount = 1500,
            EmergencyVoiceGrantCount = 2000,
            EncryptedVoiceGrantCount = 2500,
            DataCount = 3000,
            PrivateDataCount = 3500,
            AlertCount = 4000,
            FirstSeen = DateTime.Now.AddYears(-4),
            LastSeen = DateTime.Now.AddHours(-8),
            LastModified = DateTime.Now
        };

        private readonly TowerRadios_Result _testTowerRadioResult1 = new TowerRadios_Result
        {
            TowerNumber = 1031,
            TowerDescription = "LaSalle (LaSalle)",
            RadioID = 434000,
            RadioDescription = "Illinois EPA Unit",
            AffiliationCount = 25,
            DeniedCount = 50,
            VoiceGrantCount = 75,
            EmergencyVoiceGrantCount = 100,
            EncryptedVoiceGrantCount = 125,
            DataCount = 150,
            PrivateDataCount = 175,
            AlertCount = 200,
            FirstSeen = DateTime.Now.AddYears(-4),
            LastSeen = DateTime.Now.AddDays(-5),
            RecordCount = 2
        };

        private readonly TowerRadios_Result _testTowerRadioResult2 = new TowerRadios_Result
        {
            TowerNumber = 1031,
            TowerDescription = "LaSalle (LaSalle)",
            RadioID = 434001,
            RadioDescription = "Illinois EPA Unit",
            AffiliationCount = 125,
            DeniedCount = 150,
            VoiceGrantCount = 175,
            EmergencyVoiceGrantCount = 200,
            EncryptedVoiceGrantCount = 225,
            DataCount = 250,
            PrivateDataCount = 275,
            AlertCount = 300,
            FirstSeen = DateTime.Now.AddYears(-3),
            LastSeen = DateTime.Now.AddDays(-4),
            RecordCount = 2
        };

        private readonly TowerList_Result _testTowerList1 = new TowerList_Result
        {
            TowerNumber = 1031,
            TowerDescription = "LaSalle (LaSalle)"
        };

        private readonly TowerList_Result _testTowerList2 = new TowerList_Result
        {
            TowerNumber = 1034,
            TowerDescription = "Ottawa (LaSalle)"
        };

        private readonly int? _testTowerRadioCount = 1402;

        private readonly DateTime? _testTowerRadioDate1 = DateTime.Now.AddDays(-30);
        private readonly DateTime? _testTowerRadioDate2 = DateTime.Now.AddYears(-4);

        private IEnumerator<TowerRadios> GetTowerRadios()
        {
            yield return _testTowerRadio1;
            yield return _testTowerRadio2
;
        }

        private IEnumerator<TowerRadios_Result> GetTowerRadioResults()
        {
            yield return _testTowerRadioResult1;
            yield return _testTowerRadioResult2;
        }

        private IEnumerator<TowerList_Result> GetTowerList()
        {
            yield return _testTowerList1;
            yield return _testTowerList2;
        }

        private IEnumerator<int?> GetTowerRadioCount()
        {
            yield return _testTowerRadioCount;
        }

        private IEnumerator<DateTime?> GetTowerRadioDates()
        {
            yield return _testTowerRadioDate1;
            yield return _testTowerRadioDate2;
        }

        [Fact]
        public async Task GetForTowerAsyncReturnsAppropriateTypes()
        {
            var mockTowerRadios = new Mock<MockObjectResult<TowerRadios>>();

            mockTowerRadios.Setup(tr => tr.GetEnumerator()).Returns(GetTowerRadios);
            _context.Setup(ctx => ctx.TowerRadiosGetForTower(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerRadios.Object);
            SetupMockRepo();

            var towerRadioRepo = _mockRepo.Object;

            var results = await towerRadioRepo.GetForTowerAsync(1, 1031);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<TowerRadio>>(results);
            Assert.True(results.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetForTowerAsyncReturnsAppropriateValues()
        {
            var mockTowerRadios = new Mock<MockObjectResult<TowerRadios>>();

            mockTowerRadios.Setup(tr => tr.GetEnumerator()).Returns(GetTowerRadios);
            _context.Setup(ctx => ctx.TowerRadiosGetForTower(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerRadios.Object);
            SetupMockRepo();

            var towerRadioRepo = _mockRepo.Object;

            var results = await towerRadioRepo.GetForTowerAsync(1, 1031);
            var resultData = results.SingleOrDefault(rd => rd.ID == 2);

            Assert.NotNull(resultData);
            Assert.Equal(_testTowerRadio2.ID, resultData.ID);
            Assert.Equal(_testTowerRadio2.SystemID, resultData.SystemID);
            Assert.Equal(_testTowerRadio2.TowerID, resultData.TowerNumber);
            Assert.Equal(_testTowerRadio2.RadioID, resultData.RadioID);
            Assert.Equal(_testTowerRadio2.Date, resultData.Date);
            Assert.Equal(_testTowerRadio2.AffiliationCount, resultData.AffiliationCount);
            Assert.Equal(_testTowerRadio2.DeniedCount, resultData.DeniedCount);
            Assert.Equal(_testTowerRadio2.VoiceGrantCount, resultData.VoiceGrantCount);
            Assert.Equal(_testTowerRadio2.EmergencyVoiceGrantCount, resultData.EmergencyVoiceGrantCount);
            Assert.Equal(_testTowerRadio2.EncryptedVoiceGrantCount, resultData.EncryptedVoiceGrantCount);
            Assert.Equal(_testTowerRadio2.DataCount, resultData.DataCount);
            Assert.Equal(_testTowerRadio2.PrivateDataCount, resultData.PrivateDataCount);
            Assert.Equal(_testTowerRadio2.AlertCount, resultData.AlertCount);
            Assert.Equal(_testTowerRadio2.FirstSeen, resultData.FirstSeen);
            Assert.Equal(_testTowerRadio2.LastSeen, resultData.LastSeen);
            Assert.Equal(_testTowerRadio2.LastModified, resultData.LastModified);
        }

        [Fact]
        public async Task GetForTowerAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerRadiosGetForTower(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerRadioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerRadioRepo.GetForTowerAsync(1, 1031));
        }

        [Fact]
        public async Task GetForTowerAsyncWithDatesReturnsAppropriateTypes()
        {
            var mockTowerRadios = new Mock<MockObjectResult<TowerRadios>>();

            mockTowerRadios.Setup(tr => tr.GetEnumerator()).Returns(GetTowerRadios);
            _context.Setup(ctx => ctx.TowerRadiosGetForTowerDateRange(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>())).Returns(mockTowerRadios.Object);
            SetupMockRepo();

            var towerRadioRepo = _mockRepo.Object;

            var results = await towerRadioRepo.GetForTowerAsync(1, 1031, DateTime.Now);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<TowerRadio>>(results);
            Assert.True(results.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetForTowerAsyncWithDatesThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerRadiosGetForTowerDateRange(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                .Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerRadioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerRadioRepo.GetForTowerAsync(1, 1031, DateTime.Now));
        }

        [Fact]
        public async Task GetRadiosForTowerAsyncReturnsAppropriateTypes()
        {
            var mockTowerRadioResults = new Mock<MockObjectResult<TowerRadios_Result>>();

            mockTowerRadioResults.Setup(trr => trr.GetEnumerator()).Returns(GetTowerRadioResults);
            _context.Setup(ctx => ctx.TowerRadiosGetRadiosForTower(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerRadioResults.Object);
            SetupMockRepo();

            var towerRadioRepo = _mockRepo.Object;

            var results = await towerRadioRepo.GetRadiosForTowerAsync(1, 1031);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<TowerRadio>>(results);
            Assert.True(results.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetRadiosForTowerAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerRadiosGetRadiosForTower(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerRadioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerRadioRepo.GetRadiosForTowerAsync(1, 1031));
        }

        [Fact]
        public async Task GetRadiosForTowerAsyncWithFiltersReturnsAppropriateTypes()
        {
            var mockTowerRadioResults = new Mock<MockObjectResult<TowerRadios_Result>>();

            mockTowerRadioResults.Setup(trr => trr.GetEnumerator()).Returns(GetTowerRadioResults);
            _context.Setup(ctx => ctx.TowerRadiosGetRadiosForTowerFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerRadioResults.Object);
            SetupMockRepo();

            var towerRadioRepo = _mockRepo.Object;

            var (towerRadios, recordCount) = await towerRadioRepo.GetRadiosForTowerAsync("140", 1031, _filterData);

            Assert.NotNull(towerRadios);
            Assert.IsAssignableFrom<IEnumerable<TowerRadio>>(towerRadios);
            Assert.True(towerRadios.Count() > 0, "Result count is 0");
            Assert.IsAssignableFrom<int>(recordCount);
        }

        [Fact]
        public async Task GetRadiosForTowerAsyncWithFiltersReturnsAppropriateValues()
        {
            var mockTowerRadioResults = new Mock<MockObjectResult<TowerRadios_Result>>();

            mockTowerRadioResults.Setup(trr => trr.GetEnumerator()).Returns(GetTowerRadioResults);
            _context.Setup(ctx => ctx.TowerRadiosGetRadiosForTowerFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerRadioResults.Object);
            SetupMockRepo();

            var towerRadioRepo = _mockRepo.Object;

            var (towerRadios, recordCount) = await towerRadioRepo.GetRadiosForTowerAsync("140", 1031, _filterData);
            var resultData = towerRadios.SingleOrDefault(tr => tr.TowerNumber == 1031 && tr.RadioID == 434000);

            Assert.NotNull(resultData);
            Assert.Equal(_testTowerRadioResult1.TowerNumber, resultData.TowerNumber);
            Assert.Equal(_testTowerRadioResult1.TowerDescription, resultData.TowerName);
            Assert.Equal(_testTowerRadioResult1.RadioID, resultData.RadioID);
            Assert.Equal(_testTowerRadioResult1.RadioDescription, resultData.RadioName);
            Assert.Equal(_testTowerRadioResult1.AffiliationCount, resultData.AffiliationCount);
            Assert.Equal(_testTowerRadioResult1.DeniedCount, resultData.DeniedCount);
            Assert.Equal(_testTowerRadioResult1.VoiceGrantCount, resultData.VoiceGrantCount);
            Assert.Equal(_testTowerRadioResult1.EmergencyVoiceGrantCount, resultData.EmergencyVoiceGrantCount);
            Assert.Equal(_testTowerRadioResult1.EncryptedVoiceGrantCount, resultData.EncryptedVoiceGrantCount);
            Assert.Equal(_testTowerRadioResult1.DataCount, resultData.DataCount);
            Assert.Equal(_testTowerRadioResult1.PrivateDataCount, resultData.PrivateDataCount);
            Assert.Equal(_testTowerRadioResult1.AlertCount, resultData.AlertCount);
            Assert.Equal(_testTowerRadioResult1.FirstSeen, resultData.FirstSeen);
            Assert.Equal(_testTowerRadioResult1.LastSeen, resultData.LastSeen);

            Assert.Equal(_testTowerRadioResult1.RecordCount, recordCount);
        }

        [Fact]
        public async Task GetRadiosForTowerAsyncWithFiltersThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerRadiosGetRadiosForTowerFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerRadioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerRadioRepo.GetRadiosForTowerAsync("140", 1031, _filterData));
        }

        [Fact]
        public async Task GetRadiosForTowerCountAsyncReturnsACount()
        {
            var mockTowerRadioCount = new Mock<MockObjectResult<int?>>();

            mockTowerRadioCount.Setup(trc => trc.GetEnumerator()).Returns(GetTowerRadioCount);
            _context.Setup(ctx => ctx.TowerRadiosGetRadiosForTowerCount(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerRadioCount.Object);
            SetupMockRepo();

            var towerRadioRepo = _mockRepo.Object;

            var result = await towerRadioRepo.GetRadiosForTowerCountAsync(1, 1031);

            Assert.Equal(_testTowerRadioCount, result);
        }

        [Fact]
        public async Task GetRadiosForTowerCountAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerRadiosGetRadiosForTowerCount(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerRadioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerRadioRepo.GetRadiosForTowerCountAsync(1, 1031));
        }

        [Fact]
        public async Task GetRadiosForTowerByDateAsyncReturnsAppropriateTypes()
        {
            var mockTowerRadios = new Mock<MockObjectResult<TowerRadios>>();

            mockTowerRadios.Setup(tr => tr.GetEnumerator()).Returns(GetTowerRadios);
            _context.Setup(ctx => ctx.TowerRadiosGetRadiosForTowerByDate(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerRadios.Object);
            SetupMockRepo();

            var towerRadioRepo = _mockRepo.Object;

            var results = await towerRadioRepo.GetRadiosForTowerByDateAsync(1, 1031);

            Assert.NotNull(results);
            Assert.True(results.Count() > 0, "Result count is 0");
            Assert.IsAssignableFrom<IEnumerable<TowerRadio>>(results);
        }

        [Fact]
        public async Task GetRadiosForTowerByDateAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerRadiosGetRadiosForTowerByDate(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerRadioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerRadioRepo.GetRadiosForTowerByDateAsync(1, 1031));
        }

        [Fact]
        public async Task GetTowersForRadioAsyncReturnsAppropriateTypes()
        {
            var mockTowerRadioResults = new Mock<MockObjectResult<TowerRadios_Result>>();

            mockTowerRadioResults.Setup(trr => trr.GetEnumerator()).Returns(GetTowerRadioResults);
            _context.Setup(ctx => ctx.TowerRadiosGetTowersForRadio(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerRadioResults.Object);
            SetupMockRepo();

            var towerRadioRepo = _mockRepo.Object;

            var results = await towerRadioRepo.GetTowersForRadioAsync(1, 1031);

            Assert.NotNull(results);
            Assert.True(results.Count() > 0, "Result count is 0");
            Assert.IsAssignableFrom<IEnumerable<TowerRadio>>(results);
        }

        [Fact]
        public async Task GetTowersForRadioAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerRadiosGetTowersForRadio(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerRadioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerRadioRepo.GetTowersForRadioAsync(1, 1031));
        }

        [Fact]
        public async Task GetTowersForRadioCountAsyncReturnsACount()
        {
            var mockTowerRadioCount = new Mock<MockObjectResult<int?>>();

            mockTowerRadioCount.Setup(trc => trc.GetEnumerator()).Returns(GetTowerRadioCount);
            _context.Setup(ctx => ctx.TowerRadiosGetTowersForRadioCount(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerRadioCount.Object);
            SetupMockRepo();

            var towerRadioRepo = _mockRepo.Object;

            var result = await towerRadioRepo.GetTowersForRadioCountAsync(1, 434000);

            Assert.Equal(_testTowerRadioCount, result);
        }

        [Fact]
        public async Task GetTowersForRadioCountAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerRadiosGetTowersForRadioCount(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerRadioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerRadioRepo.GetTowersForRadioCountAsync(1, 434000));
        }

        [Fact]
        public async Task GetTowersForRadioAsyncWithFiltersReturnsAppropriateTypes()
        {
            var mockTowerRadioResults = new Mock<MockObjectResult<TowerRadios_Result>>();

            mockTowerRadioResults.Setup(trr => trr.GetEnumerator()).Returns(GetTowerRadioResults);
            _context.Setup(ctx => ctx.TowerRadiosGetTowersForRadioFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerRadioResults.Object);
            SetupMockRepo();

            var towerRadioRepo = _mockRepo.Object;

            var (towerRadios, recordCount) = await towerRadioRepo.GetTowersForRadioAsync("140", 434000, _filterData);

            Assert.NotNull(towerRadios);
            Assert.IsAssignableFrom<IEnumerable<TowerRadio>>(towerRadios);
            Assert.True(towerRadios.Count() > 0, "Tower radios count was 0");
            Assert.Equal(_testTowerRadioResult1.RecordCount, recordCount);
        }

        [Fact]
        public async Task GetTowersForRadioAsyncWithFiltersThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerRadiosGetTowersForRadioFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerRadioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerRadioRepo.GetTowersForRadioAsync("140", 434000, _filterData));
        }

        [Fact]
        public async Task GetTowerListForRadioAsyncReturnsAppropriateValues()
        {
            var mockTowerListResult = new Mock<MockObjectResult<TowerList_Result>>();

            mockTowerListResult.Setup(tlr => tlr.GetEnumerator()).Returns(GetTowerList);
            _context.Setup(ctx => ctx.TowerRadiosGetTowerListForRadio(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(mockTowerListResult.Object);
            SetupMockRepo();

            var towerRadioRepo = _mockRepo.Object;

            var results = await towerRadioRepo.GetTowerListForRadioAsync("140", 414001, _filterData);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<TowerRadio>>(results);
            Assert.True(results.Count() > 0, "Result count is 0");

            var resultsData = results.SingleOrDefault(rd => rd.TowerNumber == 1034);

            Assert.NotNull(resultsData);
            Assert.Equal(_testTowerList2.TowerNumber, resultsData.TowerNumber);
            Assert.Equal(_testTowerList2.TowerDescription, resultsData.TowerName);
        }

        [Fact]
        public async Task GetTowerListForRadioAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerRadiosGetTowerListForRadio(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerRadioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerRadioRepo.GetTowerListForRadioAsync("140", 414001, _filterData));
        }

        [Fact]
        public async Task GetDateListForTowerRadioAsyncReturnsAppropriateValues()
        {
            var mockTowerRadioDates = new Mock<MockObjectResult<DateTime?>>();

            mockTowerRadioDates.Setup(trd => trd.GetEnumerator()).Returns(GetTowerRadioDates);
            _context.Setup(ctx => ctx.TowerRadiosGetDateListForTowerRadio(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>())).Returns(mockTowerRadioDates.Object);
            SetupMockRepo();

            var towerRadioRepo = _mockRepo.Object;

            var results = await towerRadioRepo.GetDateListForTowerRadioAsync("140", 434000, 1031, _filterData);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<DateTime>>(results);
            Assert.True(results.Count() > 0, "Result count is 0");

            var resultsData = results.SingleOrDefault(rd => rd == _testTowerRadioDate2);

            Assert.Equal(_testTowerRadioDate2, resultsData);
        }

        [Fact]
        public async Task GetDateListForTowerRadioAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerRadiosGetDateListForTowerRadio(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerRadioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerRadioRepo.GetDateListForTowerRadioAsync("140", 434000, 1031, _filterData));
        }
    }
}
