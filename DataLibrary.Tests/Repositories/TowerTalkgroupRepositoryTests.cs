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
    public class TowerTalkgroupRepositoryTests : RepositoryTestBase<TowerTalkgroupRepository>
    {
        private readonly TowerTalkgroups _testTowerTalkgroup1 = new TowerTalkgroups
        {
            ID = 1,
            SystemID = 140,
            TowerID = 1031,
            TalkgroupID = 13004,
            Date = DateTime.Now.AddDays(-1),
            AffiliationCount = 20,
            DeniedCount = 40,
            VoiceGrantCount = 60,
            EmergencyVoiceGrantCount = 80,
            EncryptedVoiceGrantCount = 100,
            DataCount = 120,
            PrivateDataCount = 140,
            AlertCount = 160,
            FirstSeen = DateTime.Now.AddYears(-5),
            LastSeen = DateTime.Now.AddDays(-2),
            LastModified = DateTime.Now
        };

        private readonly TowerTalkgroups _testTowerTalkgroup2 = new TowerTalkgroups
        {
            ID = 2,
            SystemID = 140,
            TowerID = 1031,
            TalkgroupID = 13005,
            Date = DateTime.Now.AddDays(-2),
            AffiliationCount = 120,
            DeniedCount = 140,
            VoiceGrantCount = 160,
            EmergencyVoiceGrantCount = 180,
            EncryptedVoiceGrantCount = 1100,
            DataCount = 1120,
            PrivateDataCount = 1140,
            AlertCount = 1160,
            FirstSeen = DateTime.Now.AddYears(-4),
            LastSeen = DateTime.Now.AddDays(-3),
            LastModified = DateTime.Now
        };

        private readonly TowerTalkgroupsImport_Result _testTowerTalkgroupImport1 = new TowerTalkgroupsImport_Result
        {
            ID = 1,
            SystemID = 140,
            TowerID = 1031,
            TalkgroupID = 13004,
            Date = DateTime.Now.AddDays(-1),
            AffiliationCount = 1120,
            DeniedCount = 1140,
            VoiceGrantCount = 1160,
            EmergencyVoiceGrantCount = 1180,
            EncryptedVoiceGrantCount = 11100,
            DataCount = 11120,
            PrivateDataCount = 11140,
            AlertCount = 11160,
            FirstSeen = DateTime.Now.AddYears(-5),
            LastSeen = DateTime.Now.AddDays(-2),
            LastModified = DateTime.Now
        };

        private readonly TowerTalkgroupsImport_Result _testTowerTalkgroupImport2 = new TowerTalkgroupsImport_Result
        {
            ID = 2,
            SystemID = 140,
            TowerID = 1031,
            TalkgroupID = 13005,
            Date = DateTime.Now.AddDays(-2),
            AffiliationCount = 21120,
            DeniedCount = 21140,
            VoiceGrantCount = 21160,
            EmergencyVoiceGrantCount = 21180,
            EncryptedVoiceGrantCount = 211100,
            DataCount = 211120,
            PrivateDataCount = 211140,
            AlertCount = 211160,
            FirstSeen = DateTime.Now.AddYears(-4),
            LastSeen = DateTime.Now.AddDays(-3),
            LastModified = DateTime.Now
        };

        private readonly TowerTalkgroups_Result _testTowerTalkgroupResult1 = new TowerTalkgroups_Result
        {
            TowerNumber = 1031,
            TowerDescription = "LaSalle (LaSalle)",
            TalkgroupID = 13004,
            TalkgroupDescription = "ISP 09-A - Springfield Primary",
            AffiliationCount = 51120,
            DeniedCount = 51140,
            VoiceGrantCount = 51160,
            EmergencyVoiceGrantCount = 51180,
            EncryptedVoiceGrantCount = 511100,
            DataCount = 511120,
            PrivateDataCount = 511140,
            AlertCount = 511160,
            FirstSeen = DateTime.Now.AddYears(-5),
            LastSeen = DateTime.Now.AddDays(-2),
            RecordCount = 2
        };

        private readonly TowerTalkgroups_Result _testTowerTalkgroupResult2 = new TowerTalkgroups_Result
        {
            TowerNumber = 1031,
            TowerDescription = "LaSalle (LaSalle)",
            TalkgroupID = 13005,
            TalkgroupDescription = "ISP 09-B - Springfield Alternate",
            AffiliationCount = 151120,
            DeniedCount = 151140,
            VoiceGrantCount = 151160,
            EmergencyVoiceGrantCount = 151180,
            EncryptedVoiceGrantCount = 1511100,
            DataCount = 1511120,
            PrivateDataCount = 1511140,
            AlertCount = 1511160,
            FirstSeen = DateTime.Now.AddYears(-4),
            LastSeen = DateTime.Now.AddDays(-3),
            RecordCount = 2
        };

        private readonly TowerList_Result _testTowerListResult1 = new TowerList_Result
        {
            TowerNumber = 1031,
            TowerDescription = "LaSalle (LaSalle)"
        };

        private readonly TowerList_Result _testTowerListResult2 = new TowerList_Result
        {
            TowerNumber = 1034,
            TowerDescription = "Ottawa (LaSalle)"
        };

        private readonly int? _testTowerTalkgroupCount = 14102;

        private readonly DateTime? _testTowerDate1 = DateTime.Now.AddDays(-143);
        private readonly DateTime? _testTowerDate2 = DateTime.Now.AddDays(-302);

        private IEnumerator<TowerTalkgroups> GetTowerTalkgroups()
        {
            yield return _testTowerTalkgroup1;
            yield return _testTowerTalkgroup2;
        }

        private IEnumerator<TowerTalkgroupsImport_Result> GetTowerTalkgroupImports()
        {
            yield return _testTowerTalkgroupImport1;
            yield return _testTowerTalkgroupImport2;
        }

        private IEnumerator<TowerTalkgroups_Result> GetTowerTalkgroupResults()
        {
            yield return _testTowerTalkgroupResult1;
            yield return _testTowerTalkgroupResult2;
        }

        private IEnumerator<TowerList_Result> GetTowerListResults()
        {
            yield return _testTowerListResult1;
            yield return _testTowerListResult2;
        }

        private IEnumerator<int?> GetTowerTalkgroupCount()
        {
            yield return _testTowerTalkgroupCount;
        }

        private IEnumerator<DateTime?> GetTowerDates()
        {
            yield return _testTowerDate1;
            yield return _testTowerDate2;
        }

        [Fact]
        public async Task GetForTowerAsyncReturnsAppropriateTypes()
        {
            var mockTowerTalkgroups = new Mock<MockObjectResult<TowerTalkgroups>>();

            mockTowerTalkgroups.Setup(ttg => ttg.GetEnumerator()).Returns(GetTowerTalkgroups);
            _context.Setup(ctx => ctx.TowerTalkgroupsGetForTower(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerTalkgroups.Object);
            SetupMockRepo();

            var towerTalkgroupRepo = _mockRepo.Object;

            var results = await towerTalkgroupRepo.GetForTowerAsync(140, 1031);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<TowerTalkgroup>>(results);
            Assert.True(results.Count() > 0, "Results count is 0");
        }

        [Fact]
        public async Task GetForTowerAsyncReturnsAppropriateValues()
        {
            var mockTowerTalkgroups = new Mock<MockObjectResult<TowerTalkgroups>>();

            mockTowerTalkgroups.Setup(ttg => ttg.GetEnumerator()).Returns(GetTowerTalkgroups);
            _context.Setup(ctx => ctx.TowerTalkgroupsGetForTower(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerTalkgroups.Object);
            SetupMockRepo();

            var towerTalkgroupRepo = _mockRepo.Object;

            var results = await towerTalkgroupRepo.GetForTowerAsync(140, 1031);
            var resultData = results.SingleOrDefault(rd => rd.ID == 2);

            Assert.NotNull(resultData);
            Assert.Equal(_testTowerTalkgroup2.ID, resultData.ID);
            Assert.Equal(_testTowerTalkgroup2.SystemID, resultData.SystemID);
            Assert.Equal(_testTowerTalkgroup2.TowerID, resultData.TowerNumber);
            Assert.Equal(_testTowerTalkgroup2.TalkgroupID, resultData.TalkgroupID);
            Assert.Equal(_testTowerTalkgroup2.Date, resultData.Date);
            Assert.Equal(_testTowerTalkgroup2.AffiliationCount, resultData.AffiliationCount);
            Assert.Equal(_testTowerTalkgroup2.DeniedCount, resultData.DeniedCount);
            Assert.Equal(_testTowerTalkgroup2.VoiceGrantCount, resultData.VoiceGrantCount);
            Assert.Equal(_testTowerTalkgroup2.EmergencyVoiceGrantCount, resultData.EmergencyVoiceGrantCount);
            Assert.Equal(_testTowerTalkgroup2.EncryptedVoiceGrantCount, resultData.EncryptedVoiceGrantCount);
            Assert.Equal(_testTowerTalkgroup2.DataCount, resultData.DataCount);
            Assert.Equal(_testTowerTalkgroup2.PrivateDataCount, resultData.PrivateDataCount);
            Assert.Equal(_testTowerTalkgroup2.AlertCount, resultData.AlertCount);
            Assert.Equal(_testTowerTalkgroup2.FirstSeen, resultData.FirstSeen);
            Assert.Equal(_testTowerTalkgroup2.LastSeen, resultData.LastSeen);
            Assert.Equal(_testTowerTalkgroup2.LastModified, resultData.LastModified);
        }

        [Fact]
        public async Task GetForTowerAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerTalkgroupsGetForTower(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerTalkgroupRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerTalkgroupRepo.GetForTowerAsync(140, 1031));
        }

        [Fact]
        public async Task GetForTowerImportAsyncReturnsAppropriateTypes()
        {
            var mockTowerTalkgroupImports = new Mock<MockObjectResult<TowerTalkgroupsImport_Result>>();

            mockTowerTalkgroupImports.Setup(ttgir => ttgir.GetEnumerator()).Returns(GetTowerTalkgroupImports);
            _context.Setup(ctx => ctx.TowerTalkgroupsGetForTowerImport(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerTalkgroupImports.Object);
            SetupMockRepo();

            var towerTalkgroupRepo = _mockRepo.Object;

            var results = await towerTalkgroupRepo.GetForTowerImportAsync(140, 1031);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<TowerTalkgroup>>(results);
            Assert.True(results.Count() > 0, "Results count is 0");
        }

        [Fact]
        public async Task GetForTowerImportAsyncReturnsAppropriateValues()
        {
            var mockTowerTalkgroupImports = new Mock<MockObjectResult<TowerTalkgroupsImport_Result>>();

            mockTowerTalkgroupImports.Setup(ttgir => ttgir.GetEnumerator()).Returns(GetTowerTalkgroupImports);
            _context.Setup(ctx => ctx.TowerTalkgroupsGetForTowerImport(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerTalkgroupImports.Object);
            SetupMockRepo();

            var towerTalkgroupRepo = _mockRepo.Object;

            var results = await towerTalkgroupRepo.GetForTowerImportAsync(140, 1031);
            var resultData = results.SingleOrDefault(rd => rd.ID == 2);

            Assert.NotNull(resultData);
            Assert.Equal(_testTowerTalkgroupImport2.ID, resultData.ID);
            Assert.Equal(_testTowerTalkgroupImport2.SystemID, resultData.SystemID);
            Assert.Equal(_testTowerTalkgroupImport2.TowerID, resultData.TowerNumber);
            Assert.Equal(_testTowerTalkgroupImport2.TalkgroupID, resultData.TalkgroupID);
            Assert.Equal(_testTowerTalkgroupImport2.Date, resultData.Date);
            Assert.Equal(_testTowerTalkgroupImport2.AffiliationCount, resultData.AffiliationCount);
            Assert.Equal(_testTowerTalkgroupImport2.DeniedCount, resultData.DeniedCount);
            Assert.Equal(_testTowerTalkgroupImport2.VoiceGrantCount, resultData.VoiceGrantCount);
            Assert.Equal(_testTowerTalkgroupImport2.EmergencyVoiceGrantCount, resultData.EmergencyVoiceGrantCount);
            Assert.Equal(_testTowerTalkgroupImport2.EncryptedVoiceGrantCount, resultData.EncryptedVoiceGrantCount);
            Assert.Equal(_testTowerTalkgroupImport2.DataCount, resultData.DataCount);
            Assert.Equal(_testTowerTalkgroupImport2.PrivateDataCount, resultData.PrivateDataCount);
            Assert.Equal(_testTowerTalkgroupImport2.AlertCount, resultData.AlertCount);
            Assert.Equal(_testTowerTalkgroupImport2.FirstSeen, resultData.FirstSeen);
            Assert.Equal(_testTowerTalkgroupImport2.LastSeen, resultData.LastSeen);
            Assert.Equal(_testTowerTalkgroupImport2.LastModified, resultData.LastModified);
        }

        [Fact]
        public async Task GetForTowerImportAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerTalkgroupsGetForTowerImport(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerTalkgroupRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerTalkgroupRepo.GetForTowerImportAsync(140, 1031));
        }

        [Fact]
        public async Task GetForTowerImportAsyncWithDatesReturnsAppropriateTypes()
        {
            var mockTowerTalkgroupImports = new Mock<MockObjectResult<TowerTalkgroupsImport_Result>>();

            mockTowerTalkgroupImports.Setup(ttgir => ttgir.GetEnumerator()).Returns(GetTowerTalkgroupImports);
            _context.Setup(ctx => ctx.TowerTalkgroupsGetForTowerImportDateRange(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(mockTowerTalkgroupImports.Object);
            SetupMockRepo();

            var towerTalkgroupRepo = _mockRepo.Object;

            var results = await towerTalkgroupRepo.GetForTowerImportAsync(140, 1031, DateTime.Now);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<TowerTalkgroup>>(results);
            Assert.True(results.Count() > 0, "Results count is 0");
        }

        [Fact]
        public async Task GetForTowerImportAsyncWithDatesThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerTalkgroupsGetForTowerImportDateRange(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                .Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerTalkgroupRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerTalkgroupRepo.GetForTowerImportAsync(140, 1031, DateTime.Now));
        }

        [Fact]
        public async Task GetTalkgroupsForTowerAsyncReturnsAppropriateTypes()
        {
            var mockTowerTalkgroupResults = new Mock<MockObjectResult<TowerTalkgroups_Result>>();

            mockTowerTalkgroupResults.Setup(ttgr => ttgr.GetEnumerator()).Returns(GetTowerTalkgroupResults);
            _context.Setup(ctx => ctx.TowerTalkgroupsGetTalkgroupsForTower(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerTalkgroupResults.Object);
            SetupMockRepo();

            var towerTalkgroupRepo = _mockRepo.Object;

            var results = await towerTalkgroupRepo.GetTalkgroupsForTowerAsync(140, 1031);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<TowerTalkgroup>>(results);
            Assert.True(results.Count() > 0, "Results count is 0");
        }

        [Fact]
        public async Task GetTalkgroupsForTowerAsyncReturnsAppropriateValues()
        {
            var mockTowerTalkgroupResults = new Mock<MockObjectResult<TowerTalkgroups_Result>>();

            mockTowerTalkgroupResults.Setup(ttgr => ttgr.GetEnumerator()).Returns(GetTowerTalkgroupResults);
            _context.Setup(ctx => ctx.TowerTalkgroupsGetTalkgroupsForTower(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerTalkgroupResults.Object);
            SetupMockRepo();

            var towerTalkgroupRepo = _mockRepo.Object;

            var results = await towerTalkgroupRepo.GetTalkgroupsForTowerAsync(140, 1031);
            var resultData = results.SingleOrDefault(rd => rd.TowerNumber == 1031 && rd.TalkgroupID == 13005);

            Assert.NotNull(resultData);
            Assert.Equal(_testTowerTalkgroupResult2.TowerNumber, resultData.TowerNumber);
            Assert.Equal(_testTowerTalkgroupResult2.TowerDescription, resultData.TowerName);
            Assert.Equal(_testTowerTalkgroupResult2.TalkgroupID, resultData.TalkgroupID);
            Assert.Equal(_testTowerTalkgroupResult2.TalkgroupDescription, resultData.TalkgroupName);
            Assert.Equal(_testTowerTalkgroupResult2.AffiliationCount, resultData.AffiliationCount);
            Assert.Equal(_testTowerTalkgroupResult2.DeniedCount, resultData.DeniedCount);
            Assert.Equal(_testTowerTalkgroupResult2.VoiceGrantCount, resultData.VoiceGrantCount);
            Assert.Equal(_testTowerTalkgroupResult2.EmergencyVoiceGrantCount, resultData.EmergencyVoiceGrantCount);
            Assert.Equal(_testTowerTalkgroupResult2.EncryptedVoiceGrantCount, resultData.EncryptedVoiceGrantCount);
            Assert.Equal(_testTowerTalkgroupResult2.DataCount, resultData.DataCount);
            Assert.Equal(_testTowerTalkgroupResult2.PrivateDataCount, resultData.PrivateDataCount);
            Assert.Equal(_testTowerTalkgroupResult2.AlertCount, resultData.AlertCount);
            Assert.Equal(_testTowerTalkgroupResult2.FirstSeen, resultData.FirstSeen);
            Assert.Equal(_testTowerTalkgroupResult2.LastSeen, resultData.LastSeen);
        }

        [Fact]
        public async Task GetTalkgroupsForTowerAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerTalkgroupsGetTalkgroupsForTower(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerTalkgroupRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerTalkgroupRepo.GetTalkgroupsForTowerAsync(140, 1031));
        }

        [Fact]
        public async Task GetTalkgroupsForTowerAsyncWithFiltersReturnsAppropriateTypes()
        {
            var mockTowerTalkgroupResults = new Mock<MockObjectResult<TowerTalkgroups_Result>>();

            mockTowerTalkgroupResults.Setup(ttgr => ttgr.GetEnumerator()).Returns(GetTowerTalkgroupResults);
            _context.Setup(ctx => ctx.TowerTalkgroupsGetTalkgroupsForTowerFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerTalkgroupResults.Object);
            SetupMockRepo();

            var towerTalkgroupRepo = _mockRepo.Object;

            var (towerTalkgroups, recordCount) = await towerTalkgroupRepo.GetTalkgroupsForTowerAsync("140", 1031, _filterData);

            Assert.NotNull(towerTalkgroups);
            Assert.IsAssignableFrom<IEnumerable<TowerTalkgroup>>(towerTalkgroups);
            Assert.True(towerTalkgroups.Count() > 0, "Results count is 0");
            Assert.Equal(_testTowerTalkgroupResult1.RecordCount, recordCount);
        }

        [Fact]
        public async Task GetTalkgroupsForTowerAsyncWithFiltersThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerTalkgroupsGetTalkgroupsForTowerFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerTalkgroupRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerTalkgroupRepo.GetTalkgroupsForTowerAsync("140", 1031, _filterData));
        }

        [Fact]
        public async Task GetTalkgroupsForTowerCountAsyncReturnsACount()
        {
            var mockTowerTalkgroupCount = new Mock<MockObjectResult<int?>>();

            mockTowerTalkgroupCount.Setup(trc => trc.GetEnumerator()).Returns(GetTowerTalkgroupCount);
            _context.Setup(ctx => ctx.TowerTalkgroupsGetTalkgroupsForTowerCount(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerTalkgroupCount.Object);
            SetupMockRepo();

            var towerTalkgroupRepo = _mockRepo.Object;

            var result = await towerTalkgroupRepo.GetTalkgroupsForTowerCountAsync(1, 1031);

            Assert.Equal(_testTowerTalkgroupCount, result);
        }

        [Fact]
        public async Task GetTalkgroupsForTowerCountAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerTalkgroupsGetTalkgroupsForTowerCount(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerTalkgroupRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerTalkgroupRepo.GetTalkgroupsForTowerCountAsync(1, 1031));
        }

        [Fact]
        public async Task GetTalkgroupsForTowerByDateAsyncReturnsAppropriateTypes()
        {
            var mockTowerTalkgroupResults = new Mock<MockObjectResult<TowerTalkgroups_Result>>();

            mockTowerTalkgroupResults.Setup(ttgr => ttgr.GetEnumerator()).Returns(GetTowerTalkgroupResults);
            _context.Setup(ctx => ctx.TowerTalkgroupsGetTalkgroupsForTowerByDate(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerTalkgroupResults.Object);
            SetupMockRepo();

            var towerTalkgroupRepo = _mockRepo.Object;

            var results = await towerTalkgroupRepo.GetTalkgroupsForTowerByDateAsync(140, 1031);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<TowerTalkgroup>>(results);
            Assert.True(results.Count() > 0, "Results count is 0");

        }

        [Fact]
        public async Task GetTalkgroupsForTowerByDateAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerTalkgroupsGetTalkgroupsForTowerByDate(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerTalkgroupRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerTalkgroupRepo.GetTalkgroupsForTowerByDateAsync(140, 1031));
        }

        [Fact]
        public async Task GetTowersForTalkgroupAsyncReturnsAppropriateTypes()
        {
            var mockTowerTalkgroupResults = new Mock<MockObjectResult<TowerTalkgroups_Result>>();

            mockTowerTalkgroupResults.Setup(ttgr => ttgr.GetEnumerator()).Returns(GetTowerTalkgroupResults);
            _context.Setup(ctx => ctx.TowerTalkgroupsGetTowersForTalkgroup(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerTalkgroupResults.Object);
            SetupMockRepo();

            var towerTalkgroupRepo = _mockRepo.Object;

            var results = await towerTalkgroupRepo.GetTowersForTalkgroupAsync(140, 1031);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<TowerTalkgroup>>(results);
            Assert.True(results.Count() > 0, "Results count is 0");

        }

        [Fact]
        public async Task GetTowersForTalkgroupAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerTalkgroupsGetTowersForTalkgroup(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerTalkgroupRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerTalkgroupRepo.GetTowersForTalkgroupAsync(140, 1031));
        }

        [Fact]
        public async Task GetTowersForTalkgroupCountAsyncReturnsACount()
        {
            var mockTowerTalkgroupCount = new Mock<MockObjectResult<int?>>();

            mockTowerTalkgroupCount.Setup(trc => trc.GetEnumerator()).Returns(GetTowerTalkgroupCount);
            _context.Setup(ctx => ctx.TowerTalkgroupsGetTowersForTalkgroupCount(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerTalkgroupCount.Object);
            SetupMockRepo();

            var towerTalkgroupRepo = _mockRepo.Object;

            var result = await towerTalkgroupRepo.GetTowersForTalkgroupCountAsync(1, 1031);

            Assert.Equal(_testTowerTalkgroupCount, result);
        }

        [Fact]
        public async Task GetTowersForTalkgroupCountAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerTalkgroupsGetTowersForTalkgroupCount(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerTalkgroupRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerTalkgroupRepo.GetTowersForTalkgroupCountAsync(1, 1031));
        }

        [Fact]
        public async Task GetTowersForTalkgroupsAsyncWithFiltersReturnsAppropriateTypes()
        {
            var mockTowerTalkgroupResults = new Mock<MockObjectResult<TowerTalkgroups_Result>>();

            mockTowerTalkgroupResults.Setup(ttgr => ttgr.GetEnumerator()).Returns(GetTowerTalkgroupResults);
            _context.Setup(ctx => ctx.TowerTalkgroupsGetTowersForTalkgroupFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerTalkgroupResults.Object);
            SetupMockRepo();

            var towerTalkgroupRepo = _mockRepo.Object;

            var (towerTalkgroups, recordCount) = await towerTalkgroupRepo.GetTowersForTalkgroupsAsync("140", 1031, _filterData);

            Assert.NotNull(towerTalkgroups);
            Assert.IsAssignableFrom<IEnumerable<TowerTalkgroup>>(towerTalkgroups);
            Assert.True(towerTalkgroups.Count() > 0, "Results count is 0");
            Assert.Equal(_testTowerTalkgroupResult1.RecordCount, recordCount);
        }

        [Fact]
        public async Task GetTowersForTalkgroupsAsyncWithFiltersThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerTalkgroupsGetTowersForTalkgroupFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerTalkgroupRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerTalkgroupRepo.GetTowersForTalkgroupsAsync("140", 1031, _filterData));
        }

        [Fact]
        public async Task GetTowerListForTalkgroupAsyncReturnsAppropriateTypes()
        {
            var mockTowerListResults = new Mock<MockObjectResult<TowerList_Result>>();

            mockTowerListResults.Setup(tlr => tlr.GetEnumerator()).Returns(GetTowerListResults);
            _context.Setup(ctx => ctx.TowerTalkgroupsGetTowerListForTalkgroup(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(mockTowerListResults.Object);
            SetupMockRepo();

            var towerTalkgroupRepo = _mockRepo.Object;

            var results = await towerTalkgroupRepo.GetTowerListForTalkgroupAsync("140", 13005, _filterData);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<TowerTalkgroup>>(results);
            Assert.True(results.Count() > 0, "Results count is 0");
        }

        [Fact]
        public async Task GetTowerListForTalkgroupAsyncReturnsAppropriateValues()
        {
            var mockTowerListResults = new Mock<MockObjectResult<TowerList_Result>>();

            mockTowerListResults.Setup(tlr => tlr.GetEnumerator()).Returns(GetTowerListResults);
            _context.Setup(ctx => ctx.TowerTalkgroupsGetTowerListForTalkgroup(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(mockTowerListResults.Object);
            SetupMockRepo();

            var towerTalkgroupRepo = _mockRepo.Object;

            var results = await towerTalkgroupRepo.GetTowerListForTalkgroupAsync("140", 13005, _filterData);
            var resultsData = results.SingleOrDefault(rd => rd.TowerNumber == 1034);

            Assert.NotNull(resultsData);
            Assert.Equal(_testTowerListResult2.TowerNumber, resultsData.TowerNumber);
            Assert.Equal(_testTowerListResult2.TowerDescription, resultsData.TowerName);
        }

        [Fact]
        public async Task GetTowerListForTalkgroupAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerTalkgroupsGetTowerListForTalkgroup(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerTalkgroupRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerTalkgroupRepo.GetTowerListForTalkgroupAsync("140", 13005, _filterData));
        }

        [Fact]
        public async Task GetDateListForTowerTalkgroupAsyncReturnsAppropriateInformation()
        {
            var mockTowerDates = new Mock<MockObjectResult<DateTime?>>();

            mockTowerDates.Setup(td => td.GetEnumerator()).Returns(GetTowerDates);
            _context.Setup(ctx => ctx.TowerTalkgroupsGetDateListForTowerTalkgroup(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>())).Returns(mockTowerDates.Object);
            SetupMockRepo();

            var towerTalkgroupRepo = _mockRepo.Object;

            var results = await towerTalkgroupRepo.GetDateListForTowerTalkgroupAsync("140", 13005, 1031, _filterData);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<DateTime>>(results);
            Assert.True(results.Count() > 0, "Results count is 0");
        }

        [Fact]
        public async Task GetDateListForTowerTalkgroupAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerTalkgroupsGetDateListForTowerTalkgroup(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerTalkgroupRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerTalkgroupRepo.GetDateListForTowerTalkgroupAsync("140", 13005, 1031, _filterData));
        }
    }
}
