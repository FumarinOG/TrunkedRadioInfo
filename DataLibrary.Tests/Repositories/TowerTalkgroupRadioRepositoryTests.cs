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
    public class TowerTalkgroupRadioRepositoryTests : RepositoryTestBase<TowerTalkgroupRadioRepository>
    {
        private readonly TowerTalkgroupRadios _testTowerTalkgroupRadio1 = new TowerTalkgroupRadios
        {
            ID = 1,
            SystemID = 140,
            TowerID = 1031,
            TalkgroupID = 9092,
            RadioID = 1005485,
            Date = DateTime.Now.AddDays(-2),
            AffiliationCount = 175,
            DeniedCount = 275,
            VoiceGrantCount = 375,
            EmergencyVoiceGrantCount = 475,
            EncryptedVoiceGrantCount = 575,
            DataCount = 675,
            PrivateDataCount = 775,
            AlertCount = 875,
            FirstSeen = DateTime.Now.AddYears(-5),
            LastSeen = DateTime.Now.AddDays(-3),
            LastModified = DateTime.Now
        };

        private readonly TowerTalkgroupRadios _testTowerTalkgroupRadio2 = new TowerTalkgroupRadios
        {
            ID = 2,
            SystemID = 140,
            TowerID = 1031,
            TalkgroupID = 9093,
            RadioID = 1105485,
            Date = DateTime.Now.AddDays(-1),
            AffiliationCount = 1175,
            DeniedCount = 1275,
            VoiceGrantCount = 1375,
            EmergencyVoiceGrantCount = 1475,
            EncryptedVoiceGrantCount = 1575,
            DataCount = 1675,
            PrivateDataCount = 1775,
            AlertCount = 1875,
            FirstSeen = DateTime.Now.AddYears(-4),
            LastSeen = DateTime.Now.AddDays(-5),
            LastModified = DateTime.Now
        };

        private readonly TowerTalkgroupRadios_Result _testTowerTalkgroupRadioResults1 = new TowerTalkgroupRadios_Result
        {
            TowerNumber = 1031,
            TowerDescription = "LaSalle (LaSalle)",
            TalkgroupID = 9092,
            TalkgroupDescription = "ISP OSC 1A",
            RadioID = 1005485,
            RadioDescription = "ISP TRT Unit (25-52)",
            AffiliationCount = 2175,
            DeniedCount = 2275,
            VoiceGrantCount = 2375,
            EmergencyVoiceGrantCount = 2475,
            EncryptedVoiceGrantCount = 2575,
            DataCount = 2675,
            PrivateDataCount = 2775,
            AlertCount = 2875,
            FirstSeen = DateTime.Now.AddYears(-5),
            LastSeen = DateTime.Now.AddDays(-3),
            RecordCount = 2
        };

        private readonly TowerTalkgroupRadios_Result _testTowerTalkgroupRadioResults2 = new TowerTalkgroupRadios_Result
        {
            TowerNumber = 1031,
            TowerDescription = "LaSalle (LaSalle)",
            TalkgroupID = 9093,
            TalkgroupDescription = "ISP OSC 1B",
            RadioID = 1105485,
            RadioDescription = "ISP TRT Unit (25-52)",
            AffiliationCount = 3175,
            DeniedCount = 3275,
            VoiceGrantCount = 3375,
            EmergencyVoiceGrantCount = 3475,
            EncryptedVoiceGrantCount = 3575,
            DataCount = 3675,
            PrivateDataCount = 3775,
            AlertCount = 3875,
            FirstSeen = DateTime.Now.AddYears(-4),
            LastSeen = DateTime.Now.AddDays(-5)
        };

        private readonly TowerTalkgroupRadiosWithDates_Result _testTowerTalkgroupRadioWithDatesResults1 = new TowerTalkgroupRadiosWithDates_Result
        {
            TowerNumber = 1031,
            TowerDescription = "LaSalle (LaSalle)",
            TalkgroupID = 9092,
            TalkgroupDescription = "ISP OSC 1A",
            RadioID = 1005485,
            RadioDescription = "ISP TRT Unit (25-52)",
            Date = DateTime.Now.AddDays(-1),
            AffiliationCount = 2175,
            DeniedCount = 2275,
            VoiceGrantCount = 2375,
            EmergencyVoiceGrantCount = 2475,
            EncryptedVoiceGrantCount = 2575,
            DataCount = 2675,
            PrivateDataCount = 2775,
            AlertCount = 2875,
            FirstSeen = DateTime.Now.AddYears(-5),
            LastSeen = DateTime.Now.AddDays(-3)
        };

        private readonly TowerTalkgroupRadiosWithDates_Result _testTowerTalkgroupRadioWithDatesResults2 = new TowerTalkgroupRadiosWithDates_Result
        {
            TowerNumber = 1031,
            TowerDescription = "LaSalle (LaSalle)",
            TalkgroupID = 9093,
            TalkgroupDescription = "ISP OSC 1B",
            RadioID = 1105485,
            RadioDescription = "ISP TRT Unit (25-52)",
            Date = DateTime.Now.AddDays(-2),
            AffiliationCount = 3175,
            DeniedCount = 3275,
            VoiceGrantCount = 3375,
            EmergencyVoiceGrantCount = 3475,
            EncryptedVoiceGrantCount = 3575,
            DataCount = 3675,
            PrivateDataCount = 3775,
            AlertCount = 3875,
            FirstSeen = DateTime.Now.AddYears(-4),
            LastSeen = DateTime.Now.AddDays(-5)
        };

        private readonly RadioTowerTalkgroups_Result _testRadioTowerTalkgroupsResults1 = new RadioTowerTalkgroups_Result
        {
            RadioID = 1005485,
            RadioDescription = "ISP TRT Unit (25-52)",
            AffiliationCount = 12175,
            DeniedCount = 12275,
            VoiceGrantCount = 12375,
            EmergencyVoiceGrantCount = 12475,
            EncryptedVoiceGrantCount = 12575,
            DataCount = 12675,
            AlertCount = 12875,
            FirstSeen = DateTime.Now.AddYears(-5),
            LastSeen = DateTime.Now.AddDays(-3),
            RecordCount = 2
        };

        private readonly RadioTowerTalkgroups_Result _testRadioTowerTalkgroupsResults2 = new RadioTowerTalkgroups_Result
        {
            RadioID = 1105485,
            RadioDescription = "ISP TRT Unit (25-52)",
            AffiliationCount = 22175,
            DeniedCount = 22275,
            VoiceGrantCount = 22375,
            EmergencyVoiceGrantCount = 22475,
            EncryptedVoiceGrantCount = 22575,
            DataCount = 22675,
            AlertCount = 22875,
            FirstSeen = DateTime.Now.AddYears(-4),
            LastSeen = DateTime.Now.AddDays(-5),
            RecordCount = 2
        };

        private readonly TalkgroupTowerRadios_Result _testTalkgroupTowerRadiosResults1 = new TalkgroupTowerRadios_Result
        {
            TalkgroupID = 9092,
            TalkgroupDescription = "ISP OSC 1A",
            AffiliationCount = 112175,
            DeniedCount = 112275,
            VoiceGrantCount = 112375,
            EmergencyVoiceGrantCount = 112475,
            EncryptedVoiceGrantCount = 112575,
            DataCount = 112675,
            AlertCount = 112875,
            FirstSeen = DateTime.Now.AddYears(-5),
            LastSeen = DateTime.Now.AddDays(-3),
            RecordCount = 2
        };

        private readonly TalkgroupTowerRadios_Result _testTalkgroupTowerRadiosResults2 = new TalkgroupTowerRadios_Result
        {
            TalkgroupID = 9093,
            TalkgroupDescription = "ISP OSC 1B",
            AffiliationCount = 112175,
            DeniedCount = 112275,
            VoiceGrantCount = 112375,
            EmergencyVoiceGrantCount = 112475,
            EncryptedVoiceGrantCount = 112575,
            DataCount = 112675,
            AlertCount = 112875,
            FirstSeen = DateTime.Now.AddYears(-5),
            LastSeen = DateTime.Now.AddDays(-3),
            RecordCount = 2
        };

        private IEnumerator<TowerTalkgroupRadios> GetTowerTalkgroupRadios()
        {
            yield return _testTowerTalkgroupRadio1;
            yield return _testTowerTalkgroupRadio2;
        }

        private IEnumerator<TowerTalkgroupRadios_Result> GetTowerTalkgroupRadioResults()
        {
            yield return _testTowerTalkgroupRadioResults1;
            yield return _testTowerTalkgroupRadioResults2;
        }

        private IEnumerator<TowerTalkgroupRadiosWithDates_Result> GetTowerTalkgroupRadioWithDatesResults()
        {
            yield return _testTowerTalkgroupRadioWithDatesResults1;
            yield return _testTowerTalkgroupRadioWithDatesResults2;
        }

        private IEnumerator<RadioTowerTalkgroups_Result> GetRadioTowerTalkgroupsResults()
        {
            yield return _testRadioTowerTalkgroupsResults1;
            yield return _testRadioTowerTalkgroupsResults2;
        }

        private IEnumerator<TalkgroupTowerRadios_Result> GetTalkgroupTowerRadiosResults()
        {
            yield return _testTalkgroupTowerRadiosResults1;
            yield return _testTalkgroupTowerRadiosResults2;
        }

        [Fact]
        public async Task GetForTowerAsyncReturnsAppropriateTypes()
        {
            var mockTowerTalkgroupRadios = new Mock<MockObjectResult<TowerTalkgroupRadios>>();

            mockTowerTalkgroupRadios.Setup(ttgr => ttgr.GetEnumerator()).Returns(GetTowerTalkgroupRadios);
            _context.Setup(ctx => ctx.TowerTalkgroupRadiosGetForTower(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerTalkgroupRadios.Object);
            SetupMockRepo();

            var towerTalkgroupRadioRepo = _mockRepo.Object;

            var results = await towerTalkgroupRadioRepo.GetForTowerAsync(1, 1031);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<TowerTalkgroupRadio>>(results);
            Assert.True(results.Count() > 0, "Results count is 0");
        }

        [Fact]
        public async Task GetForTowerAsyncReturnsAppropriateValues()
        {
            var mockTowerTalkgroupRadios = new Mock<MockObjectResult<TowerTalkgroupRadios>>();

            mockTowerTalkgroupRadios.Setup(ttgr => ttgr.GetEnumerator()).Returns(GetTowerTalkgroupRadios);
            _context.Setup(ctx => ctx.TowerTalkgroupRadiosGetForTower(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerTalkgroupRadios.Object);
            SetupMockRepo();

            var towerTalkgroupRadioRepo = _mockRepo.Object;

            var results = await towerTalkgroupRadioRepo.GetForTowerAsync(1, 1031);
            var resultData = results.SingleOrDefault(rd => rd.ID == 2);

            Assert.NotNull(resultData);
            Assert.Equal(_testTowerTalkgroupRadio2.ID, resultData.ID);
            Assert.Equal(_testTowerTalkgroupRadio2.SystemID, resultData.SystemID);
            Assert.Equal(_testTowerTalkgroupRadio2.TowerID, resultData.TowerNumber);
            Assert.Equal(_testTowerTalkgroupRadio2.TalkgroupID, resultData.TalkgroupID);
            Assert.Equal(_testTowerTalkgroupRadio2.RadioID, resultData.RadioID);
            Assert.Equal(_testTowerTalkgroupRadio2.Date, resultData.Date);
            Assert.Equal(_testTowerTalkgroupRadio2.AffiliationCount, resultData.AffiliationCount);
            Assert.Equal(_testTowerTalkgroupRadio2.DeniedCount, resultData.DeniedCount);
            Assert.Equal(_testTowerTalkgroupRadio2.VoiceGrantCount, resultData.VoiceGrantCount);
            Assert.Equal(_testTowerTalkgroupRadio2.EmergencyVoiceGrantCount, resultData.EmergencyVoiceGrantCount);
            Assert.Equal(_testTowerTalkgroupRadio2.EncryptedVoiceGrantCount, resultData.EncryptedVoiceGrantCount);
            Assert.Equal(_testTowerTalkgroupRadio2.DataCount, resultData.DataCount);
            Assert.Equal(_testTowerTalkgroupRadio2.PrivateDataCount, resultData.PrivateDataCount);
            Assert.Equal(_testTowerTalkgroupRadio2.AlertCount, resultData.AlertCount);
            Assert.Equal(_testTowerTalkgroupRadio2.FirstSeen, resultData.FirstSeen);
            Assert.Equal(_testTowerTalkgroupRadio2.LastSeen, resultData.LastSeen);
            Assert.Equal(_testTowerTalkgroupRadio2.LastModified, resultData.LastModified);
        }

        [Fact]
        public async Task GetForTowerAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerTalkgroupRadiosGetForTower(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerTalkgroupRadioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerTalkgroupRadioRepo.GetForTowerAsync(1, 1031));

        }

        [Fact]
        public async Task GetForTowersAsyncDatesReturnsAppropriateTypes()
        {
            var mockTowerTalkgroupRadios = new Mock<MockObjectResult<TowerTalkgroupRadios>>();

            mockTowerTalkgroupRadios.Setup(ttgr => ttgr.GetEnumerator()).Returns(GetTowerTalkgroupRadios);
            _context.Setup(ctx => ctx.TowerTalkgroupRadiosGetForTowerDateRange(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(mockTowerTalkgroupRadios.Object);
            SetupMockRepo();

            var towerTalkgroupRadioRepo = _mockRepo.Object;

            var results = await towerTalkgroupRadioRepo.GetForTowerAsync(1, 1031, DateTime.Now);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<TowerTalkgroupRadio>>(results);
            Assert.True(results.Count() > 0, "Results count is 0");
        }

        [Fact]
        public async Task GetForTowerAsyncDatesThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerTalkgroupRadiosGetForTowerDateRange(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                .Throws(new Exception());
            SetupMockRepo();

            var towerTalkgroupRadioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerTalkgroupRadioRepo.GetForTowerAsync(1, 1031, DateTime.Now));
        }

        [Fact]
        public async Task GetTowersForTalkgroupRadioAsyncReturnsAppropriateTypes()
        {
            var mockTowerTalkgroupRadioResults = new Mock<MockObjectResult<TowerTalkgroupRadios_Result>>();

            mockTowerTalkgroupRadioResults.Setup(ttgrr => ttgrr.GetEnumerator()).Returns(GetTowerTalkgroupRadioResults());
            _context.Setup(ctx => ctx.TowerTalkgroupRadiosGetTowersForTalkgroupRadio(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(mockTowerTalkgroupRadioResults.Object);
            SetupMockRepo();

            var towerTalkgroupRadioRepo = _mockRepo.Object;

            var results = await towerTalkgroupRadioRepo.GetTowersForTalkgroupRadioAsync(140, 9092, 1005485);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<TowerTalkgroupRadio>>(results);
            Assert.True(results.Count() > 0, "Results count is 0");
        }

        [Fact]
        public async Task GetTowersForTalkgroupRadioAsyncReturnsAppropriateValues()
        {
            var mockTowerTalkgroupRadioResults = new Mock<MockObjectResult<TowerTalkgroupRadios_Result>>();

            mockTowerTalkgroupRadioResults.Setup(ttgrr => ttgrr.GetEnumerator()).Returns(GetTowerTalkgroupRadioResults());
            _context.Setup(ctx => ctx.TowerTalkgroupRadiosGetTowersForTalkgroupRadio(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(mockTowerTalkgroupRadioResults.Object);
            SetupMockRepo();

            var towerTalkgroupRadioRepo = _mockRepo.Object;

            var results = await towerTalkgroupRadioRepo.GetTowersForTalkgroupRadioAsync(140, 9092, 1005485);
            var resultsData = results.SingleOrDefault(rd => rd.TalkgroupID == 9092 && rd.RadioID == 1005485);

            Assert.NotNull(resultsData);
            Assert.Equal(_testTowerTalkgroupRadioResults1.TowerNumber, resultsData.TowerNumber);
            Assert.Equal(_testTowerTalkgroupRadioResults1.TowerDescription, resultsData.TowerName);
            Assert.Equal(_testTowerTalkgroupRadioResults1.TalkgroupID, resultsData.TalkgroupID);
            Assert.Equal(_testTowerTalkgroupRadioResults1.TalkgroupDescription, resultsData.TalkgroupName);
            Assert.Equal(_testTowerTalkgroupRadioResults1.RadioID, resultsData.RadioID);
            Assert.Equal(_testTowerTalkgroupRadioResults1.RadioDescription, resultsData.RadioName);
            Assert.Equal(_testTowerTalkgroupRadioResults1.AffiliationCount, resultsData.AffiliationCount);
            Assert.Equal(_testTowerTalkgroupRadioResults1.DeniedCount, resultsData.DeniedCount);
            Assert.Equal(_testTowerTalkgroupRadioResults1.VoiceGrantCount, resultsData.VoiceGrantCount);
            Assert.Equal(_testTowerTalkgroupRadioResults1.EmergencyVoiceGrantCount, resultsData.EmergencyVoiceGrantCount);
            Assert.Equal(_testTowerTalkgroupRadioResults1.EncryptedVoiceGrantCount, resultsData.EncryptedVoiceGrantCount);
            Assert.Equal(_testTowerTalkgroupRadioResults1.DataCount, resultsData.DataCount);
            Assert.Equal(_testTowerTalkgroupRadioResults1.PrivateDataCount, resultsData.PrivateDataCount);
            Assert.Equal(_testTowerTalkgroupRadioResults1.AlertCount, resultsData.AlertCount);
            Assert.Equal(_testTowerTalkgroupRadioResults1.FirstSeen, resultsData.FirstSeen);
            Assert.Equal(_testTowerTalkgroupRadioResults1.LastSeen, resultsData.LastSeen);
        }

        [Fact]
        public async Task GetTowersForTalkgroupRadioAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerTalkgroupRadiosGetTowersForTalkgroupRadio(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerTalkgroupRadioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerTalkgroupRadioRepo.GetTowersForTalkgroupRadioAsync(140, 9092, 1005485));
        }

        [Fact]
        public async Task GetRadiosForTalkgroupWithDatesAsyncReturnsAppropriateTypes()
        {
            var mockTowerTalkgroupRadioWithDatesResults = new Mock<MockObjectResult<TowerTalkgroupRadiosWithDates_Result>>();

            mockTowerTalkgroupRadioWithDatesResults.Setup(ttgrr => ttgrr.GetEnumerator()).Returns(GetTowerTalkgroupRadioWithDatesResults);
            _context.Setup(ctx => ctx.TowerTalkgroupRadiosGetRadiosForTalkgroupWithDates(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(mockTowerTalkgroupRadioWithDatesResults.Object);
            SetupMockRepo();

            var towerTalkgroupRadioRepo = _mockRepo.Object;

            var results = await towerTalkgroupRadioRepo.GetRadiosForTalkgroupWithDatesAsync(134, 9092);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<TowerTalkgroupRadio>>(results);
            Assert.True(results.Count() > 0, "Results count is 0");
        }

        [Fact]
        public async Task GetRadiosForTalkgroupWithDatesAsyncReturnsAppropriateValues()
        {
            var mockTowerTalkgroupRadioWithDatesResults = new Mock<MockObjectResult<TowerTalkgroupRadiosWithDates_Result>>();

            mockTowerTalkgroupRadioWithDatesResults.Setup(ttgrr => ttgrr.GetEnumerator()).Returns(GetTowerTalkgroupRadioWithDatesResults);
            _context.Setup(ctx => ctx.TowerTalkgroupRadiosGetRadiosForTalkgroupWithDates(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(mockTowerTalkgroupRadioWithDatesResults.Object);
            SetupMockRepo();

            var towerTalkgroupRadioRepo = _mockRepo.Object;

            var results = await towerTalkgroupRadioRepo.GetRadiosForTalkgroupWithDatesAsync(134, 9092);
            var resultsData = results.SingleOrDefault(rd => rd.TalkgroupID == 9092 && rd.RadioID == 1005485);

            Assert.NotNull(resultsData);
            Assert.Equal(_testTowerTalkgroupRadioWithDatesResults1.TowerNumber, resultsData.TowerNumber);
            Assert.Equal(_testTowerTalkgroupRadioWithDatesResults1.TowerDescription, resultsData.TowerName);
            Assert.Equal(_testTowerTalkgroupRadioWithDatesResults1.TalkgroupID, resultsData.TalkgroupID);
            Assert.Equal(_testTowerTalkgroupRadioWithDatesResults1.TalkgroupDescription, resultsData.TalkgroupName);
            Assert.Equal(_testTowerTalkgroupRadioWithDatesResults1.RadioID, resultsData.RadioID);
            Assert.Equal(_testTowerTalkgroupRadioWithDatesResults1.RadioDescription, resultsData.RadioName);
            Assert.Equal(_testTowerTalkgroupRadioWithDatesResults1.Date, resultsData.Date);
            Assert.Equal(_testTowerTalkgroupRadioWithDatesResults1.AffiliationCount, resultsData.AffiliationCount);
            Assert.Equal(_testTowerTalkgroupRadioWithDatesResults1.DeniedCount, resultsData.DeniedCount);
            Assert.Equal(_testTowerTalkgroupRadioWithDatesResults1.VoiceGrantCount, resultsData.VoiceGrantCount);
            Assert.Equal(_testTowerTalkgroupRadioWithDatesResults1.EmergencyVoiceGrantCount, resultsData.EmergencyVoiceGrantCount);
            Assert.Equal(_testTowerTalkgroupRadioWithDatesResults1.EncryptedVoiceGrantCount, resultsData.EncryptedVoiceGrantCount);
            Assert.Equal(_testTowerTalkgroupRadioWithDatesResults1.DataCount, resultsData.DataCount);
            Assert.Equal(_testTowerTalkgroupRadioWithDatesResults1.PrivateDataCount, resultsData.PrivateDataCount);
            Assert.Equal(_testTowerTalkgroupRadioWithDatesResults1.AlertCount, resultsData.AlertCount);
            Assert.Equal(_testTowerTalkgroupRadioWithDatesResults1.FirstSeen, resultsData.FirstSeen);
            Assert.Equal(_testTowerTalkgroupRadioWithDatesResults1.LastSeen, resultsData.LastSeen);
        }

        [Fact]
        public async Task GetRadiosForTalkgroupWithDatesAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerTalkgroupRadiosGetRadiosForTalkgroupWithDates(It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerTalkgroupRadioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerTalkgroupRadioRepo.GetRadiosForTalkgroupWithDatesAsync(134, 9092));
        }

        [Fact]
        public async Task GetTalkgroupsForRadioWithDatesAsyncReturnsAppropriateTypes()
        {
            var mockTowerTalkgroupRadioWithDatesResults = new Mock<MockObjectResult<TowerTalkgroupRadiosWithDates_Result>>();

            mockTowerTalkgroupRadioWithDatesResults.Setup(ttgrr => ttgrr.GetEnumerator()).Returns(GetTowerTalkgroupRadioWithDatesResults);
            _context.Setup(ctx => ctx.TowerTalkgroupRadiosGetTalkgroupsForRadioWithDates(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(mockTowerTalkgroupRadioWithDatesResults.Object);
            SetupMockRepo();

            var towerTalkgroupRadioRepo = _mockRepo.Object;

            var results = await towerTalkgroupRadioRepo.GetTalkgroupsForRadioWithDatesAsync(134, 9092);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<TowerTalkgroupRadio>>(results);
            Assert.True(results.Count() > 0, "Results count is 0");
        }

        [Fact]
        public async Task GetTalkgroupsForRadioWithDatesAsyncThrowsExeptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerTalkgroupRadiosGetTalkgroupsForRadioWithDates(It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerTalkgroupRadioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerTalkgroupRadioRepo.GetTalkgroupsForRadioWithDatesAsync(134, 9092));
        }

        [Fact]
        public async Task GetRadiosForTalkgroupTowerAsyncReturnsAppropriateTypes()
        {
            var mockRadioTowerTalkgroupsResults = new Mock<MockObjectResult<RadioTowerTalkgroups_Result>>();

            mockRadioTowerTalkgroupsResults.Setup(rttgr => rttgr.GetEnumerator()).Returns(GetRadioTowerTalkgroupsResults);
            _context.Setup(ctx => ctx.TowerTalkgroupRadiosGetRadiosForTowerTalkgroupFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(),
                It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(mockRadioTowerTalkgroupsResults.Object);
            SetupMockRepo();

            var towerTalkgroupRadioRepo = _mockRepo.Object;

            var (towerRadios, recordCount) = await towerTalkgroupRadioRepo.GetRadiosForTalkgroupTowerAsync("140", 9093, 1031, _filterData);

            Assert.NotNull(towerRadios);
            Assert.IsAssignableFrom<IEnumerable<TowerTalkgroupRadio>>(towerRadios);
            Assert.True(towerRadios.Count() > 0, "Results count is 0");
            Assert.IsAssignableFrom<int>(recordCount);
        }

        [Fact]
        public async Task GetRadiosForTalkgroupTowerAsyncReturnsAppropriateValues()
        {
            var mockRadioTowerTalkgroupsResults = new Mock<MockObjectResult<RadioTowerTalkgroups_Result>>();

            mockRadioTowerTalkgroupsResults.Setup(rttgr => rttgr.GetEnumerator()).Returns(GetRadioTowerTalkgroupsResults);
            _context.Setup(ctx => ctx.TowerTalkgroupRadiosGetRadiosForTowerTalkgroupFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(),
                It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(mockRadioTowerTalkgroupsResults.Object);
            SetupMockRepo();

            var towerTalkgroupRadioRepo = _mockRepo.Object;

            var (towerRadios, recordCount) = await towerTalkgroupRadioRepo.GetRadiosForTalkgroupTowerAsync("140", 9093, 1031, _filterData);
            var resultsData = towerRadios.SingleOrDefault(rd => rd.RadioID == 1105485);

            Assert.NotNull(resultsData);
            Assert.Equal(_testRadioTowerTalkgroupsResults2.RadioID, resultsData.RadioID);
            Assert.Equal(_testRadioTowerTalkgroupsResults2.RadioDescription, resultsData.RadioName);
            Assert.Equal(_testRadioTowerTalkgroupsResults2.AffiliationCount, resultsData.AffiliationCount);
            Assert.Equal(_testRadioTowerTalkgroupsResults2.DeniedCount, resultsData.DeniedCount);
            Assert.Equal(_testRadioTowerTalkgroupsResults2.VoiceGrantCount, resultsData.VoiceGrantCount);
            Assert.Equal(_testRadioTowerTalkgroupsResults2.EmergencyVoiceGrantCount, resultsData.EmergencyVoiceGrantCount);
            Assert.Equal(_testRadioTowerTalkgroupsResults2.EncryptedVoiceGrantCount, resultsData.EncryptedVoiceGrantCount);
            Assert.Equal(_testRadioTowerTalkgroupsResults2.DataCount, resultsData.DataCount);
            Assert.Equal(_testRadioTowerTalkgroupsResults2.AlertCount, resultsData.AlertCount);
            Assert.Equal(_testRadioTowerTalkgroupsResults2.FirstSeen, resultsData.FirstSeen);
            Assert.Equal(_testRadioTowerTalkgroupsResults2.LastSeen, resultsData.LastSeen);
            Assert.Equal(_testRadioTowerTalkgroupsResults2.RecordCount, recordCount);
        }

        [Fact]
        public async Task GetRadiosForTalkgroupTowerAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerTalkgroupRadiosGetRadiosForTowerTalkgroupFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(),
                It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerTalkgroupRadioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerTalkgroupRadioRepo.GetRadiosForTalkgroupTowerAsync("140", 9093, 1031, _filterData));
        }

        [Fact]
        public async Task GetTalkgroupsForRadioTowerAsyncReturnsAppropriateTypes()
        {
            var mockTalkgroupTowerRadiosResults = new Mock<MockObjectResult<TalkgroupTowerRadios_Result>>();

            mockTalkgroupTowerRadiosResults.Setup(rttgr => rttgr.GetEnumerator()).Returns(GetTalkgroupTowerRadiosResults);
            _context.Setup(ctx => ctx.TowerTalkgroupRadiosGetTalkgroupsForTowerRadioFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(),
                It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(mockTalkgroupTowerRadiosResults.Object);
            SetupMockRepo();

            var towerTalkgroupRadioRepo = _mockRepo.Object;

            var (towerRadios, recordCount) = await towerTalkgroupRadioRepo.GetTalkgroupsForTowerRadioAsync("140", 1005485, 1031, _filterData);

            Assert.NotNull(towerRadios);
            Assert.IsAssignableFrom<IEnumerable<TowerTalkgroupRadio>>(towerRadios);
            Assert.True(towerRadios.Count() > 0, "Results count is 0");
            Assert.IsAssignableFrom<int>(recordCount);
        }

        [Fact]
        public async Task GetTalkgroupsForRadioTowerAsyncReturnsAppropriateValues()
        {
            var mockTalkgroupTowerRadiosResults = new Mock<MockObjectResult<TalkgroupTowerRadios_Result>>();

            mockTalkgroupTowerRadiosResults.Setup(rttgr => rttgr.GetEnumerator()).Returns(GetTalkgroupTowerRadiosResults);
            _context.Setup(ctx => ctx.TowerTalkgroupRadiosGetTalkgroupsForTowerRadioFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(),
                It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(mockTalkgroupTowerRadiosResults.Object);
            SetupMockRepo();

            var towerTalkgroupRadioRepo = _mockRepo.Object;

            var (towerRadios, recordCount) = await towerTalkgroupRadioRepo.GetTalkgroupsForTowerRadioAsync("140", 1005485, 1031, _filterData);
            var resultsData = towerRadios.SingleOrDefault(rd => rd.TalkgroupID == 9092);

            Assert.NotNull(resultsData);
            Assert.Equal(_testTalkgroupTowerRadiosResults1.TalkgroupID, resultsData.TalkgroupID);
            Assert.Equal(_testTalkgroupTowerRadiosResults1.TalkgroupDescription, resultsData.TalkgroupName);
            Assert.Equal(_testTalkgroupTowerRadiosResults1.AffiliationCount, resultsData.AffiliationCount);
            Assert.Equal(_testTalkgroupTowerRadiosResults1.DeniedCount, resultsData.DeniedCount);
            Assert.Equal(_testTalkgroupTowerRadiosResults1.VoiceGrantCount, resultsData.VoiceGrantCount);
            Assert.Equal(_testTalkgroupTowerRadiosResults1.EmergencyVoiceGrantCount, resultsData.EmergencyVoiceGrantCount);
            Assert.Equal(_testTalkgroupTowerRadiosResults1.EncryptedVoiceGrantCount, resultsData.EncryptedVoiceGrantCount);
            Assert.Equal(_testTalkgroupTowerRadiosResults1.DataCount, resultsData.DataCount);
            Assert.Equal(_testTalkgroupTowerRadiosResults1.AlertCount, resultsData.AlertCount);
            Assert.Equal(_testTalkgroupTowerRadiosResults1.FirstSeen, resultsData.FirstSeen);
            Assert.Equal(_testTalkgroupTowerRadiosResults1.LastSeen, resultsData.LastSeen);
            Assert.Equal(_testTalkgroupTowerRadiosResults1.RecordCount, recordCount);
        }

        [Fact]
        public async Task GetTalkgroupsForRadioTowerAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerTalkgroupRadiosGetTalkgroupsForTowerRadioFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(),
                It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerTalkgroupRadioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerTalkgroupRadioRepo.GetTalkgroupsForTowerRadioAsync("140", 1005485, 1031, _filterData));
        }
    }
}
