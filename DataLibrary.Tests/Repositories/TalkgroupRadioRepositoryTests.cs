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
    public class TalkgroupRadioRepositoryTests : RepositoryTestBase<TalkgroupRadioRepository>
    {
        private readonly TalkgroupRadios _testTalkgroupRadio1 = new TalkgroupRadios
        {
            ID = 1,
            SystemID = 140,
            TalkgroupID = 9002,
            RadioID = 1902101,
            Date = DateTime.Now,
            AffiliationCount = 50,
            DeniedCount = 100,
            VoiceGrantCount = 150,
            EmergencyVoiceGrantCount = 200,
            EncryptedVoiceGrantCount = 250,
            DataCount = 300,
            PrivateDataCount = 350,
            AlertCount = 400,
            FirstSeen = DateTime.Now.AddYears(-5),
            LastSeen = DateTime.Now.AddHours(-5),
            LastModified = DateTime.Now.AddMinutes(-5)
        };

        private readonly TalkgroupRadios _testTalkgroupRadio2 = new TalkgroupRadios
        {
            ID = 2,
            SystemID = 140,
            TalkgroupID = 9003,
            RadioID = 1902102,
            Date = DateTime.Now,
            AffiliationCount = 150,
            DeniedCount = 1100,
            VoiceGrantCount = 1150,
            EmergencyVoiceGrantCount = 1200,
            EncryptedVoiceGrantCount = 1250,
            DataCount = 1300,
            PrivateDataCount = 1350,
            AlertCount = 1400,
            FirstSeen = DateTime.Now.AddYears(-15),
            LastSeen = DateTime.Now.AddHours(-15),
            LastModified = DateTime.Now.AddMinutes(-15)
        };

        private readonly TalkgroupRadios_Result _testTalkgroupRadioResult1 = new TalkgroupRadios_Result
        {
            TalkgroupID = 9002,
            TalkgroupDescription = "ISP 02-A - Elgin Primary",
            RadioID = 1902101,
            AffiliationCount = 50,
            DeniedCount = 100,
            VoiceGrantCount = 150,
            EmergencyVoiceGrantCount = 200,
            EncryptedVoiceGrantCount = 250,
            DataCount = 300,
            PrivateDataCount = 350,
            AlertCount = 400,
            FirstSeen = DateTime.Now.AddYears(-5),
            LastSeen = DateTime.Now.AddHours(-5)
        };

        private readonly TalkgroupRadios_Result _testTalkgroupRadioResult2 = new TalkgroupRadios_Result
        {
            TalkgroupID = 9003,
            TalkgroupDescription = "ISP 02-B - Elgin Alernate",
            RadioID = 1902102,
            AffiliationCount = 150,
            DeniedCount = 1100,
            VoiceGrantCount = 1150,
            EmergencyVoiceGrantCount = 1200,
            EncryptedVoiceGrantCount = 1250,
            DataCount = 1300,
            PrivateDataCount = 1350,
            AlertCount = 1400,
            FirstSeen = DateTime.Now.AddYears(-15),
            LastSeen = DateTime.Now.AddHours(-15)
        };

        private readonly TalkgroupRadiosWithDates_Result _testTalkgroupRadioWithDatesResult1 = new TalkgroupRadiosWithDates_Result
        {
            TalkgroupID = 9002,
            TalkgroupDescription = "ISP 02-A - Elgin Primary",
            RadioID = 1902101,
            RadioDescription = "ISP Dispatch (Elgin) (02-A)",
            Date = DateTime.Parse("06-15-2018"),
            AffiliationCount = 50,
            DeniedCount = 100,
            VoiceGrantCount = 150,
            EmergencyVoiceGrantCount = 200,
            EncryptedVoiceGrantCount = 250,
            DataCount = 300,
            PrivateDataCount = 350,
            AlertCount = 400,
            FirstSeen = DateTime.Now.AddYears(-5),
            LastSeen = DateTime.Now.AddHours(-5)
        };

        private readonly TalkgroupRadiosWithDates_Result _testTalkgroupRadioWithDatesResult2 = new TalkgroupRadiosWithDates_Result
        {
            TalkgroupID = 9003,
            TalkgroupDescription = "ISP 02-B - Elgin Alernate",
            RadioID = 1902102,
            RadioDescription = "ISP Dispatch (Elgin) (02-B)",
            Date = DateTime.Parse("07-01-2018"),
            AffiliationCount = 150,
            DeniedCount = 1100,
            VoiceGrantCount = 1150,
            EmergencyVoiceGrantCount = 1200,
            EncryptedVoiceGrantCount = 1250,
            DataCount = 1300,
            PrivateDataCount = 1350,
            AlertCount = 1400,
            FirstSeen = DateTime.Now.AddYears(-15),
            LastSeen = DateTime.Now.AddHours(-15)
        };

        private readonly int? _testTalkgroupRadioCount = 493021;

        private IEnumerator<TalkgroupRadios> GetTalkgroupRadios()
        {
            yield return _testTalkgroupRadio1;
            yield return _testTalkgroupRadio2;
        }

        private IEnumerator<TalkgroupRadios_Result> GetTalkgroupRadioResults()
        {
            yield return _testTalkgroupRadioResult1;
            yield return _testTalkgroupRadioResult2;
        }

        private IEnumerator<TalkgroupRadiosWithDates_Result> GetTalkgroupRadioWithDatesResults()
        {
            yield return _testTalkgroupRadioWithDatesResult1;
            yield return _testTalkgroupRadioWithDatesResult2;
        }

        private IEnumerator<int?> GetTalkgroupRadioCount()
        {
            yield return _testTalkgroupRadioCount;
        }

        [Fact]
        public async Task GetForSystemAsyncIntReturnsProperTypes()
        {
            var mockTalkgroupRadios = new Mock<MockObjectResult<TalkgroupRadios>>();

            mockTalkgroupRadios.Setup(tgr => tgr.GetEnumerator()).Returns(GetTalkgroupRadios);
            _context.Setup(ctx => ctx.TalkgroupRadiosGetForSystem(It.IsAny<int>())).Returns(mockTalkgroupRadios.Object);
            SetupMockRepo();

            var talkgroupRadioRepo = _mockRepo.Object;

            var results = await talkgroupRadioRepo.GetForSystemAsync(1);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<TalkgroupRadio>>(results);
            Assert.True(results.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetForSystemAsyncIntReturnsProperValues()
        {
            var mockTalkgroupRadios = new Mock<MockObjectResult<TalkgroupRadios>>();

            mockTalkgroupRadios.Setup(tgr => tgr.GetEnumerator()).Returns(GetTalkgroupRadios);
            _context.Setup(ctx => ctx.TalkgroupRadiosGetForSystem(It.IsAny<int>())).Returns(mockTalkgroupRadios.Object);
            SetupMockRepo();

            var talkgroupRadioRepo = _mockRepo.Object;

            var results = await talkgroupRadioRepo.GetForSystemAsync(1);
            var resultData = results.SingleOrDefault(rd => rd.ID == 1);
            Assert.Equal(_testTalkgroupRadio1.ID, resultData.ID);
            Assert.Equal(_testTalkgroupRadio1.SystemID, resultData.SystemID);
            Assert.Equal(_testTalkgroupRadio1.TalkgroupID, resultData.TalkgroupID);
            Assert.Equal(_testTalkgroupRadio1.RadioID, resultData.RadioID);
            Assert.Equal(_testTalkgroupRadio1.Date, resultData.Date);
            Assert.Equal(_testTalkgroupRadio1.AffiliationCount, resultData.AffiliationCount);
            Assert.Equal(_testTalkgroupRadio1.DeniedCount, resultData.DeniedCount);
            Assert.Equal(_testTalkgroupRadio1.VoiceGrantCount, resultData.VoiceGrantCount);
            Assert.Equal(_testTalkgroupRadio1.EmergencyVoiceGrantCount, resultData.EmergencyVoiceGrantCount);
            Assert.Equal(_testTalkgroupRadio1.EncryptedVoiceGrantCount, resultData.EncryptedVoiceGrantCount);
            Assert.Equal(_testTalkgroupRadio1.DataCount, resultData.DataCount);
            Assert.Equal(_testTalkgroupRadio1.PrivateDataCount, resultData.PrivateDataCount);
            Assert.Equal(_testTalkgroupRadio1.AlertCount, resultData.AlertCount);
            Assert.Equal(_testTalkgroupRadio1.FirstSeen, resultData.FirstSeen);
            Assert.Equal(_testTalkgroupRadio1.LastSeen, resultData.LastSeen);
            Assert.Equal(_testTalkgroupRadio1.LastModified, resultData.LastModified);
        }

        [Fact]
        public async Task GetForSystemAsyncIntThrowsExceptionForDatabaseException()
        {
            _context.Setup(ctx => ctx.TalkgroupRadiosGetForSystem(It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var talkgroupRadioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => talkgroupRadioRepo.GetForSystemAsync(1));
        }

        [Fact]
        public async Task GetForSystemAsyncDateRangeReturnsProperTypes()
        {
            var mockTalkgroupRadios = new Mock<MockObjectResult<TalkgroupRadios>>();

            mockTalkgroupRadios.Setup(tgr => tgr.GetEnumerator()).Returns(GetTalkgroupRadios);
            _context.Setup(ctx => ctx.TalkgroupRadiosGetForSystemDateRange(It.IsAny<int>(), It.IsAny<DateTime>())).Returns(mockTalkgroupRadios.Object);
            SetupMockRepo();

            var talkgroupRadioRepo = _mockRepo.Object;

            var results = await talkgroupRadioRepo.GetForSystemAsync(1, DateTime.Now);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<TalkgroupRadio>>(results);
            Assert.True(results.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetForSystemAsyncDateRangeReturnsProperValues()
        {
            var mockTalkgroupRadios = new Mock<MockObjectResult<TalkgroupRadios>>();

            mockTalkgroupRadios.Setup(tgr => tgr.GetEnumerator()).Returns(GetTalkgroupRadios);
            _context.Setup(ctx => ctx.TalkgroupRadiosGetForSystemDateRange(It.IsAny<int>(), It.IsAny<DateTime>())).Returns(mockTalkgroupRadios.Object);
            SetupMockRepo();

            var talkgroupRadioRepo = _mockRepo.Object;

            var results = await talkgroupRadioRepo.GetForSystemAsync(1, DateTime.Now);
            var resultData = results.SingleOrDefault(rd => rd.ID == 2);

            Assert.NotNull(resultData);
            Assert.Equal(_testTalkgroupRadio2.ID, resultData.ID);
            Assert.Equal(_testTalkgroupRadio2.SystemID, resultData.SystemID);
            Assert.Equal(_testTalkgroupRadio2.TalkgroupID, resultData.TalkgroupID);
            Assert.Equal(_testTalkgroupRadio2.RadioID, resultData.RadioID);
            Assert.Equal(_testTalkgroupRadio2.Date, resultData.Date);
            Assert.Equal(_testTalkgroupRadio2.AffiliationCount, resultData.AffiliationCount);
            Assert.Equal(_testTalkgroupRadio2.DeniedCount, resultData.DeniedCount);
            Assert.Equal(_testTalkgroupRadio2.VoiceGrantCount, resultData.VoiceGrantCount);
            Assert.Equal(_testTalkgroupRadio2.EmergencyVoiceGrantCount, resultData.EmergencyVoiceGrantCount);
            Assert.Equal(_testTalkgroupRadio2.EncryptedVoiceGrantCount, resultData.EncryptedVoiceGrantCount);
            Assert.Equal(_testTalkgroupRadio2.DataCount, resultData.DataCount);
            Assert.Equal(_testTalkgroupRadio2.PrivateDataCount, resultData.PrivateDataCount);
            Assert.Equal(_testTalkgroupRadio2.AlertCount, resultData.AlertCount);
            Assert.Equal(_testTalkgroupRadio2.FirstSeen, resultData.FirstSeen);
            Assert.Equal(_testTalkgroupRadio2.LastSeen, resultData.LastSeen);
            Assert.Equal(_testTalkgroupRadio2.LastModified, resultData.LastModified);
        }

        [Fact]
        public async Task GetForSystemAsyncDateRangeThrowsExceptionForDatabaseException()
        {
            _context.Setup(ctx => ctx.TalkgroupRadiosGetForSystemDateRange(It.IsAny<int>(), It.IsAny<DateTime>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var talkgroupRadioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => talkgroupRadioRepo.GetForSystemAsync(1, DateTime.Now));
        }

        [Fact]
        public async Task GetTalkgroupsForRadioAsyncReturnsProperTypes()
        {
            var mockTalkgroupRadioResults = new Mock<MockObjectResult<TalkgroupRadios_Result>>();

            mockTalkgroupRadioResults.Setup(tgrr => tgrr.GetEnumerator()).Returns(GetTalkgroupRadioResults);
            _context.Setup(ctx => ctx.TalkgroupRadiosGetTalkgroupsForRadio(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTalkgroupRadioResults.Object);
            SetupMockRepo();

            var talkgroupRadioRepo = _mockRepo.Object;

            var results = await talkgroupRadioRepo.GetTalkgroupsForRadioAsync(1, 1902101);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<TalkgroupRadio>>(results);
            Assert.True(results.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetTalkgroupsForRadioAsyncReturnsProperValues()
        {
            var mockTalkgroupRadioResults = new Mock<MockObjectResult<TalkgroupRadios_Result>>();

            mockTalkgroupRadioResults.Setup(tgrr => tgrr.GetEnumerator()).Returns(GetTalkgroupRadioResults);
            _context.Setup(ctx => ctx.TalkgroupRadiosGetTalkgroupsForRadio(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTalkgroupRadioResults.Object);
            SetupMockRepo();

            var talkgroupRadioRepo = _mockRepo.Object;

            var results = await talkgroupRadioRepo.GetTalkgroupsForRadioAsync(1, 1902101);
            var resultsData = results.SingleOrDefault(rd => rd.TalkgroupID == 9002 && rd.RadioID == 1902101);

            Assert.NotNull(resultsData);
            Assert.Equal(_testTalkgroupRadioResult1.TalkgroupID, resultsData.TalkgroupID);
            Assert.Equal(_testTalkgroupRadioResult1.TalkgroupDescription, resultsData.TalkgroupName);
            Assert.Equal(_testTalkgroupRadioResult1.RadioID, resultsData.RadioID);
            Assert.Equal(_testTalkgroupRadioResult1.AffiliationCount, resultsData.AffiliationCount);
            Assert.Equal(_testTalkgroupRadioResult1.DeniedCount, resultsData.DeniedCount);
            Assert.Equal(_testTalkgroupRadioResult1.VoiceGrantCount, resultsData.VoiceGrantCount);
            Assert.Equal(_testTalkgroupRadioResult1.EmergencyVoiceGrantCount, resultsData.EmergencyVoiceGrantCount);
            Assert.Equal(_testTalkgroupRadioResult1.EncryptedVoiceGrantCount, resultsData.EncryptedVoiceGrantCount);
            Assert.Equal(_testTalkgroupRadioResult1.DataCount, resultsData.DataCount);
            Assert.Equal(_testTalkgroupRadioResult1.PrivateDataCount, resultsData.PrivateDataCount);
            Assert.Equal(_testTalkgroupRadioResult1.AlertCount, resultsData.AlertCount);
            Assert.Equal(_testTalkgroupRadioResult1.FirstSeen, resultsData.FirstSeen);
            Assert.Equal(_testTalkgroupRadioResult1.LastSeen, resultsData.LastSeen);
        }

        [Fact]
        public async Task GetTalkgroupsForRadioAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TalkgroupRadiosGetTalkgroupsForRadio(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var talkgroupRadioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => talkgroupRadioRepo.GetTalkgroupsForRadioAsync(1, 1902101));
        }

        [Fact]
        public async Task GetTalkgroupsForRadioAsyncWithFiltersReturnsAppropriateValues()
        {
            var mockTalkgroupRadioResults = new Mock<MockObjectResult<TalkgroupRadios_Result>>();

            mockTalkgroupRadioResults.Setup(tgrr => tgrr.GetEnumerator()).Returns(GetTalkgroupRadioResults);
            _context.Setup(ctx => ctx.TalkgroupRadiosGetTalkgroupsForRadioFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockTalkgroupRadioResults.Object);
            SetupMockRepo();

            var talkgroupRadioRepo = _mockRepo.Object;

            var (talkgroupRadios, recordCount) = await talkgroupRadioRepo.GetTalkgroupsForRadioAsync("140", 1902102, _filterData);

            Assert.NotNull(talkgroupRadios);
            Assert.IsAssignableFrom<IEnumerable<TalkgroupRadio>>(talkgroupRadios);
            Assert.True(talkgroupRadios.Count() > 0, "Result count is 0");
            Assert.IsAssignableFrom<int>(recordCount);
        }

        [Fact]
        public async Task GetTalkgroupsForRadioAsyncWithFiltersThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TalkgroupRadiosGetTalkgroupsForRadioFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var talkgroupRadioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => talkgroupRadioRepo.GetTalkgroupsForRadioAsync("140", 1902102, _filterData));
        }

        [Fact]
        public async Task GetTalkgroupsForRadioCountAsyncReturnsACount()
        {
            var mockTalkgroupRadioCount = new Mock<MockObjectResult<int?>>();

            mockTalkgroupRadioCount.Setup(tgr => tgr.GetEnumerator()).Returns(GetTalkgroupRadioCount);
            _context.Setup(ctx => ctx.TalkgroupRadiosGetTalkgroupsForRadioCount(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTalkgroupRadioCount.Object);
            SetupMockRepo();

            var talkgroupRadioRepo = _mockRepo.Object;

            var result = await talkgroupRadioRepo.GetTalkgroupsForRadioCountAsync(1, 1902101);

            Assert.Equal(result, _testTalkgroupRadioCount);

        }

        [Fact]
        public async Task GetTalkgroupsForRadioCountAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TalkgroupRadiosGetTalkgroupsForRadioCount(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var talkgroupRadioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => talkgroupRadioRepo.GetTalkgroupsForRadioCountAsync(1, 1902101));
        }

        [Fact]
        public async Task GetRadiosForTalkgroupAsyncReturnsAppropriateTypes()
        {
            var mockTalkgroupRadioResults = new Mock<MockObjectResult<TalkgroupRadios_Result>>();

            mockTalkgroupRadioResults.Setup(tgr => tgr.GetEnumerator()).Returns(GetTalkgroupRadioResults());
            _context.Setup(ctx => ctx.TalkgroupRadiosGetRadiosForTalkgroup(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTalkgroupRadioResults.Object);
            SetupMockRepo();

            var talkgroupRadioRepo = _mockRepo.Object;

            var results = await talkgroupRadioRepo.GetRadiosForTalkgroupAsync(1, 1902101);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<TalkgroupRadio>>(results);
            Assert.True(results.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetRadiosForTalkgroupAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TalkgroupRadiosGetRadiosForTalkgroup(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var talkgroupRadioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => talkgroupRadioRepo.GetRadiosForTalkgroupAsync(1, 1902101));
        }

        [Fact]
        public async Task GetRadiosForTalkgroupCountAsyncReturnsACount()
        {
            var mockTalkgroupRadioCount = new Mock<MockObjectResult<int?>>();

            mockTalkgroupRadioCount.Setup(tgr => tgr.GetEnumerator()).Returns(GetTalkgroupRadioCount);
            _context.Setup(ctx => ctx.TalkgroupRadiosGetRadiosForTalkgroupCount(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTalkgroupRadioCount.Object);
            SetupMockRepo();

            var talkgroupRadioRepo = _mockRepo.Object;

            var result = await talkgroupRadioRepo.GetRadiosForTalkgroupCountAsync(1, 1902102);

            Assert.Equal(_testTalkgroupRadioCount, result);
        }

        [Fact]
        public async Task GetRadiosForTalkgroupCountAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TalkgroupRadiosGetRadiosForTalkgroupCount(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var talkgroupRadioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => talkgroupRadioRepo.GetRadiosForTalkgroupCountAsync(1, 1902101));
        }

        [Fact]
        public async Task GetRadiosForTalkgroupAsyncWithFiltersReturnsAppropriateTypes()
        {
            var mockTalkgroupRadioResults = new Mock<MockObjectResult<TalkgroupRadios_Result>>();

            mockTalkgroupRadioResults.Setup(tgr => tgr.GetEnumerator()).Returns(GetTalkgroupRadioResults);
            _context.Setup(ctx => ctx.TalkgroupRadiosGetRadiosForTalkgroupFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(mockTalkgroupRadioResults.Object);
            SetupMockRepo();

            var talkgroupRadioRepo = _mockRepo.Object;

            var (talkgroupRadios, recordCount) = await talkgroupRadioRepo.GetRadiosForTalkgroupAsync("140", 1902102, _filterData);

            Assert.NotNull(talkgroupRadios);
            Assert.IsAssignableFrom<IEnumerable<TalkgroupRadio>>(talkgroupRadios);
            Assert.True(talkgroupRadios.Count() > 0, "Result count is 0");
            Assert.IsAssignableFrom<int>(recordCount);
        }

        [Fact]
        public async Task GetRadiosForTalkgroupAsyncWithFiltersTHrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TalkgroupRadiosGetRadiosForTalkgroupFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new Exception("Database error"));
            SetupMockRepo();

            var talkgroupRadioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => talkgroupRadioRepo.GetRadiosForTalkgroupAsync("140", 1902102, _filterData));
        }

        [Fact]
        public async Task GetRadiosForTalkgroupWithDatesAsyncReturnsAppropriateTypes()
        {
            var mockTalkgroupRadioWithDates = new Mock<MockObjectResult<TalkgroupRadiosWithDates_Result>>();

            mockTalkgroupRadioWithDates.Setup(tgr => tgr.GetEnumerator()).Returns(GetTalkgroupRadioWithDatesResults());
            _context.Setup(ctx => ctx.TalkgroupRadiosGetRadiosForTalkgroupWithDates(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(mockTalkgroupRadioWithDates.Object);
            SetupMockRepo();

            var talkgroupRadioRepo = _mockRepo.Object;

            var results = await talkgroupRadioRepo.GetRadiosForTalkgroupWithDatesAsync(1, 9002);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<TalkgroupRadio>>(results);
            Assert.True(results.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetRadiosForTalkgroupWithDatesAsyncReturnsAppropriateValues()
        {
            var mockTalkgroupRadioWithDates = new Mock<MockObjectResult<TalkgroupRadiosWithDates_Result>>();

            mockTalkgroupRadioWithDates.Setup(tgr => tgr.GetEnumerator()).Returns(GetTalkgroupRadioWithDatesResults());
            _context.Setup(ctx => ctx.TalkgroupRadiosGetRadiosForTalkgroupWithDates(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(mockTalkgroupRadioWithDates.Object);
            SetupMockRepo();

            var talkgroupRadioRepo = _mockRepo.Object;

            var results = await talkgroupRadioRepo.GetRadiosForTalkgroupWithDatesAsync(1, 9002);
            var resultData = results.SingleOrDefault(rd => rd.TalkgroupID == 9002 && rd.RadioID == 1902101);
            Assert.Equal(_testTalkgroupRadioWithDatesResult1.TalkgroupID, resultData.TalkgroupID);
            Assert.Equal(_testTalkgroupRadioWithDatesResult1.TalkgroupDescription, resultData.TalkgroupName);
            Assert.Equal(_testTalkgroupRadioWithDatesResult1.RadioID, resultData.RadioID);
            Assert.Equal(_testTalkgroupRadioWithDatesResult1.RadioDescription, resultData.RadioName);
            Assert.Equal(_testTalkgroupRadioWithDatesResult1.Date, resultData.Date);
            Assert.Equal(_testTalkgroupRadioWithDatesResult1.AffiliationCount, resultData.AffiliationCount);
            Assert.Equal(_testTalkgroupRadioWithDatesResult1.DeniedCount, resultData.DeniedCount);
            Assert.Equal(_testTalkgroupRadioWithDatesResult1.VoiceGrantCount, resultData.VoiceGrantCount);
            Assert.Equal(_testTalkgroupRadioWithDatesResult1.EmergencyVoiceGrantCount, resultData.EmergencyVoiceGrantCount);
            Assert.Equal(_testTalkgroupRadioWithDatesResult1.EncryptedVoiceGrantCount, resultData.EncryptedVoiceGrantCount);
            Assert.Equal(_testTalkgroupRadioWithDatesResult1.DataCount, resultData.DataCount);
            Assert.Equal(_testTalkgroupRadioWithDatesResult1.PrivateDataCount, resultData.PrivateDataCount);
            Assert.Equal(_testTalkgroupRadioWithDatesResult1.AlertCount, resultData.AlertCount);
            Assert.Equal(_testTalkgroupRadioWithDatesResult1.FirstSeen, resultData.FirstSeen);
            Assert.Equal(_testTalkgroupRadioWithDatesResult1.LastSeen, resultData.LastSeen);
        }

        [Fact]
        public async Task GetRadiosForTalkgroupWithDatesAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TalkgroupRadiosGetRadiosForTalkgroupWithDates(It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new Exception("Database error"));
            SetupMockRepo();

            var talkgroupRadioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => talkgroupRadioRepo.GetRadiosForTalkgroupWithDatesAsync(1, 1902102));
        }
    }
}
