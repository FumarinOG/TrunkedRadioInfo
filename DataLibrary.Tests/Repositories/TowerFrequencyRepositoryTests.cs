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
    public class TowerFrequencyRepositoryTests : RepositoryTestBase<TowerFrequencyRepository>
    {
        private readonly TowerFrequencies _testTowerFrequencies1 = new TowerFrequencies
        {
            ID = 1,
            SystemID = 1,
            TowerID = 1002,
            Channel = "01-1394",
            Usage = "avd",
            Frequency = "770.71875",
            InputChannel = "01-1394",
            InputFrequency = "800.71875",
            InputExplicit = 0,
            HitCount = 1377,
            FirstSeen = DateTime.Now.AddYears(-5),
            LastSeen = DateTime.Now.AddHours(-5),
            LastModified = DateTime.Now
        };

        private readonly TowerFrequencies _testTowerFrequencies2 = new TowerFrequencies
        {
            ID = 2,
            SystemID = 1,
            TowerID = 1002,
            Channel = "01-2214",
            Usage = "vdi",
            Frequency = "775.59375",
            InputChannel = "01-2174",
            InputFrequency = "805.59375",
            InputExplicit = 0,
            HitCount = 1377,
            FirstSeen = DateTime.Now.AddYears(-3),
            LastSeen = DateTime.Now.AddHours(-3),
            LastModified = DateTime.Now
        };

        private readonly TowerFrequencies_Result _testTowerFrequencyResult1 = new TowerFrequencies_Result
        {
            Frequency = "770.71875",
            Channel = "01-1394",
            Usage = "avd",
            AffiliationCount = 25,
            DeniedCount = 125,
            VoiceGrantCount = 525,
            EmergencyVoiceGrantCount = 1025,
            EncryptedVoiceGrantCount = 1525,
            DataCount = 10025,
            PrivateDataCount = 10525,
            CWIDCount = 15025,
            AlertCount = 15525,
            FirstSeen = DateTime.Now.AddYears(-5),
            LastSeen = DateTime.Now.AddHours(-5),
            RecordCount = 2
        };

        private readonly TowerFrequencies_Result _testTowerFrequencyResult2 = new TowerFrequencies_Result
        {
            Frequency = "775.59375",
            Channel = "01-2214",
            Usage = "vdi",
            AffiliationCount = 125,
            DeniedCount = 1125,
            VoiceGrantCount = 1525,
            EmergencyVoiceGrantCount = 11025,
            EncryptedVoiceGrantCount = 11525,
            DataCount = 110025,
            PrivateDataCount = 110525,
            CWIDCount = 115025,
            AlertCount = 115525,
            FirstSeen = DateTime.Now.AddYears(-4),
            LastSeen = DateTime.Now.AddHours(-4),
            RecordCount = 2
        };

        private readonly TowerFrequencySummary_Result _testTowerFrequencySummary1 = new TowerFrequencySummary_Result
        {
            Channel = "01-1394",
            Usage = "avd",
            Frequency = "770.71875",
            InputChannel = "01-1394",
            InputFrequency = "800.71875",
            AffiliationCount = 25,
            DeniedCount = 125,
            VoiceGrantCount = 525,
            EmergencyVoiceGrantCount = 1025,
            EncryptedVoiceGrantCount = 1525,
            DataCount = 10025,
            PrivateDataCount = 10525,
            CWIDCount = 15025,
            AlertCount = 15525,
            FirstSeen = DateTime.Now.AddYears(-5),
            LastSeen = DateTime.Now.AddHours(-5)
        };

        private readonly TowerFrequencySummary_Result _testTowerFrequencySummary2 = new TowerFrequencySummary_Result
        {
            Channel = "01-2214",
            Usage = "vdi",
            Frequency = "775.59375",
            InputChannel = "01-2174",
            InputFrequency = "805.59375",
            AffiliationCount = 125,
            DeniedCount = 1125,
            VoiceGrantCount = 1525,
            EmergencyVoiceGrantCount = 11025,
            EncryptedVoiceGrantCount = 11525,
            DataCount = 110025,
            PrivateDataCount = 110525,
            CWIDCount = 115025,
            AlertCount = 115525,
            FirstSeen = DateTime.Now.AddYears(-4),
            LastSeen = DateTime.Now.AddHours(-4)
        };

        private readonly TowerFrequency _testTowerFrequency = new TowerFrequency
        {
            SystemID = 1,
            TowerID = 1002,
            Channel = "01-1394",
            Usage = "avd",
            Frequency = "770.71875",
            InputChannel = "01-1394",
            InputFrequency = "800.71875",
            InputExplicit = 0,
            HitCount = 1377,
            FirstSeen = DateTime.Now.AddYears(-5),
            LastSeen = DateTime.Now.AddHours(-5),
        };

        private readonly int? _testTowerFrequencyCount = 10;

        private IEnumerator<TowerFrequencies> GetTowerFrequency()
        {
            yield return _testTowerFrequencies2;
        }

        private IEnumerator<TowerFrequencies> GetTowerFrequencies()
        {
            yield return _testTowerFrequencies1;
            yield return _testTowerFrequencies2;
        }

        private IEnumerator<TowerFrequencies_Result> GetTowerFrequenciesResults()
        {
            yield return _testTowerFrequencyResult1;
            yield return _testTowerFrequencyResult2;
        }

        private IEnumerator<TowerFrequencySummary_Result> GetTowerFrequencySumamryResult()
        {
            yield return _testTowerFrequencySummary1;
        }

        private IEnumerator<int?> GetTowerFrequencyCount()
        {
            yield return _testTowerFrequencyCount;
        }
    
        [Fact]
        public async Task GetForFrequencyAsyncReturnsAppropriateValues()
        {
            var mockTowerFrequencies = new Mock<MockObjectResult<TowerFrequencies>>();

            mockTowerFrequencies.Setup(tf => tf.GetEnumerator()).Returns(GetTowerFrequency);
            _context.Setup(ctx => ctx.TowerFrequenciesGetForFrequency(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(mockTowerFrequencies.Object);
            SetupMockRepo();

            var towerFrequencyRepo = _mockRepo.Object;

            var results = await towerFrequencyRepo.GetForFrequencyAsync(1, 1002, "775.59375");

            Assert.NotNull(results);
            Assert.Equal(_testTowerFrequencies2.ID, results.ID);
            Assert.Equal(_testTowerFrequencies2.SystemID, results.SystemID);
            Assert.Equal(_testTowerFrequencies2.TowerID, results.TowerID);
            Assert.Equal(_testTowerFrequencies2.Channel, results.Channel);
            Assert.Equal(_testTowerFrequencies2.Usage, results.Usage);
            Assert.Equal(_testTowerFrequencies2.Frequency, results.Frequency);
            Assert.Equal(_testTowerFrequencies2.InputChannel, results.InputChannel);
            Assert.Equal(_testTowerFrequencies2.InputFrequency, results.InputFrequency);
            Assert.Equal(_testTowerFrequencies2.InputExplicit, results.InputExplicit);
            Assert.Equal(_testTowerFrequencies2.HitCount, results.HitCount);
            Assert.Equal(_testTowerFrequencies2.FirstSeen, results.FirstSeen);
            Assert.Equal(_testTowerFrequencies2.LastSeen, results.LastSeen);
            Assert.Equal(_testTowerFrequencies2.LastModified, results.LastModified);
        }

        [Fact]
        public async Task GetForFrequencyAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerFrequenciesGetForFrequency(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerFrequencyRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerFrequencyRepo.GetForFrequencyAsync(1, 1002, "775.59375"));
        }

        [Fact]
        public async Task GetSummaryAsyncReturnsAppropriateValues()
        {
            var mockTowerFrequencySummary = new Mock<MockObjectResult<TowerFrequencySummary_Result>>();

            mockTowerFrequencySummary.Setup(tfs => tfs.GetEnumerator()).Returns(GetTowerFrequencySumamryResult);
            _context.Setup(ctx => ctx.TowerFrequencyGetSummaryForFrequency(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(mockTowerFrequencySummary.Object);
            SetupMockRepo();

            var towerFrequencyRepo = _mockRepo.Object;

            var result = await towerFrequencyRepo.GetSummaryAsync("140", 1002, "770.71875");

            Assert.NotNull(result);
            Assert.Equal(_testTowerFrequencySummary1.Channel, result.Channel);
            Assert.Equal(_testTowerFrequencySummary1.Usage, result.Usage);
            Assert.Equal(_testTowerFrequencySummary1.Frequency, result.Frequency);
            Assert.Equal(_testTowerFrequencySummary1.InputChannel, result.InputChannel);
            Assert.Equal(_testTowerFrequencySummary1.InputFrequency, result.InputFrequency);
            Assert.Equal(_testTowerFrequencySummary1.AffiliationCount, result.AffiliationCount);
            Assert.Equal(_testTowerFrequencySummary1.DeniedCount, result.DeniedCount);
            Assert.Equal(_testTowerFrequencySummary1.VoiceGrantCount, result.VoiceGrantCount);
            Assert.Equal(_testTowerFrequencySummary1.EmergencyVoiceGrantCount, result.EmergencyVoiceGrantCount);
            Assert.Equal(_testTowerFrequencySummary1.EncryptedVoiceGrantCount, result.EncryptedVoiceGrantCount);
            Assert.Equal(_testTowerFrequencySummary1.DataCount, result.DataCount);
            Assert.Equal(_testTowerFrequencySummary1.PrivateDataCount, result.PrivateDataCount);
            Assert.Equal(_testTowerFrequencySummary1.CWIDCount, result.CWIDCount);
            Assert.Equal(_testTowerFrequencySummary1.AlertCount, result.AlertCount);
            Assert.Equal(_testTowerFrequencySummary1.FirstSeen, result.FirstSeen);
            Assert.Equal(_testTowerFrequencySummary1.LastSeen, result.LastSeen);
        }

        [Fact]
        public async Task GetSummaryAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerFrequencyGetSummaryForFrequency(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()))
                .Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerFrequencyRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerFrequencyRepo.GetSummaryAsync("140", 1002, "770.71875"));
        }

        [Fact]
        public async Task GetForTowerAsyncReturnsAppropriateTypes()
        {
            var mockTowerFrequencies = new Mock<MockObjectResult<TowerFrequencies>>();

            mockTowerFrequencies.Setup(mtf => mtf.GetEnumerator()).Returns(GetTowerFrequencies);
            _context.Setup(ctx => ctx.TowerFrequenciesGetForTower(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerFrequencies.Object);
            SetupMockRepo();

            var towerFrequencyRepo = _mockRepo.Object;

            var results = await towerFrequencyRepo.GetForTowerAsync(1, 1002);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<TowerFrequency>>(results);
            Assert.True(results.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetForTowerAsyncReturnsAppropriateValues()
        {
            var mockTowerFrequencies = new Mock<MockObjectResult<TowerFrequencies>>();

            mockTowerFrequencies.Setup(mtf => mtf.GetEnumerator()).Returns(GetTowerFrequencies);
            _context.Setup(ctx => ctx.TowerFrequenciesGetForTower(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerFrequencies.Object);
            SetupMockRepo();

            var towerFrequencyRepo = _mockRepo.Object;

            var results = await towerFrequencyRepo.GetForTowerAsync(1, 1002);
            var resultData = results.SingleOrDefault(tf => tf.ID == 1);

            Assert.NotNull(resultData);
            Assert.Equal(_testTowerFrequencies1.ID, resultData.ID);
            Assert.Equal(_testTowerFrequencies1.SystemID, resultData.SystemID);
            Assert.Equal(_testTowerFrequencies1.TowerID, resultData.TowerID);
            Assert.Equal(_testTowerFrequencies1.Channel, resultData.Channel);
            Assert.Equal(_testTowerFrequencies1.Usage, resultData.Usage);
            Assert.Equal(_testTowerFrequencies1.Frequency, resultData.Frequency);
            Assert.Equal(_testTowerFrequencies1.InputChannel, resultData.InputChannel);
            Assert.Equal(_testTowerFrequencies1.InputFrequency, resultData.InputFrequency);
            Assert.Equal(_testTowerFrequencies1.InputExplicit, resultData.InputExplicit);
            Assert.Equal(_testTowerFrequencies1.HitCount, resultData.HitCount);
            Assert.Equal(_testTowerFrequencies1.FirstSeen, resultData.FirstSeen);
            Assert.Equal(_testTowerFrequencies1.LastSeen, resultData.LastSeen);
            Assert.Equal(_testTowerFrequencies1.LastModified, resultData.LastModified);
        }

        [Fact]
        public async Task GetForTowerAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerFrequenciesGetForTower(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerFrequencyRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerFrequencyRepo.GetForTowerAsync(1, 1002));
        }

        [Fact]
        public async Task GetFrequenciesForTowerCountAsyncReturnsACount()
        {
            var mockTowerFrequencyCount = new Mock<MockObjectResult<int?>>();

            mockTowerFrequencyCount.Setup(tfc => tfc.GetEnumerator()).Returns(GetTowerFrequencyCount);
            _context.Setup(ctx => ctx.TowerFrequenciesGetFrequenciesForTowerCount(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerFrequencyCount.Object);
            SetupMockRepo();

            var towerFrequencyRepo = _mockRepo.Object;

            var result = await towerFrequencyRepo.GetFrequenciesForTowerCountAsync(1, 1002);

            Assert.Equal(_testTowerFrequencyCount, result);
        }

        [Fact]
        public async Task GetFrequenciesForTowerCountAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerFrequenciesGetFrequenciesForTowerCount(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerFrequencyRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerFrequencyRepo.GetFrequenciesForTowerCountAsync(1, 1002));
        }

        [Fact]
        public async Task GetFrequenciesForTowerAsyncReturnsAppropriateTypes()
        {
            var mockTowerFrequencyResults = new Mock<MockObjectResult<TowerFrequencies_Result>>();

            mockTowerFrequencyResults.Setup(tfr => tfr.GetEnumerator()).Returns(GetTowerFrequenciesResults);
            _context.Setup(ctx => ctx.TowerFrequenciesGetFrequenciesForTower(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerFrequencyResults.Object);
            SetupMockRepo();

            var towerFrequencyRepo = _mockRepo.Object;

            var results = await towerFrequencyRepo.GetFrequenciesForTowerAsync(1, 1002);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<TowerFrequency>>(results);
            Assert.True(results.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetFrequenciesForTowerAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerFrequenciesGetFrequenciesForTower(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerFrequencyRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerFrequencyRepo.GetFrequenciesForTowerAsync(1, 1002));
        }

        [Fact]
        public async Task GetFrequenciesForTowerAsyncFiltersReturnsAppropriateTypes()
        {
            var mockTowerFrequencyResults = new Mock<MockObjectResult<TowerFrequencies_Result>>();

            mockTowerFrequencyResults.Setup(tfr => tfr.GetEnumerator()).Returns(GetTowerFrequenciesResults);
            _context.Setup(ctx => ctx.TowerFrequenciesGetFrequenciesForTowerFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerFrequencyResults.Object);
            SetupMockRepo();

            var towerFrequencyRepo = _mockRepo.Object;

            var (towerFrequencies, recordCount) = await towerFrequencyRepo.GetFrequenciesForTowerAsync("140", 1002, _filterData);

            Assert.NotNull(towerFrequencies);
            Assert.IsAssignableFrom<IEnumerable<TowerFrequency>>(towerFrequencies);
            Assert.True(towerFrequencies.Count() > 0, "Result count is 0");
            Assert.IsAssignableFrom<int>(recordCount);
        }

        [Fact]
        public async Task GetFrequenciesForTowerAsyncFiltersReturnsAppropriateValues()
        {
            var mockTowerFrequencyResults = new Mock<MockObjectResult<TowerFrequencies_Result>>();

            mockTowerFrequencyResults.Setup(tfr => tfr.GetEnumerator()).Returns(GetTowerFrequenciesResults);
            _context.Setup(ctx => ctx.TowerFrequenciesGetFrequenciesForTowerFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerFrequencyResults.Object);
            SetupMockRepo();

            var towerFrequencyRepo = _mockRepo.Object;

            var (towerFrequencies, recordCount) = await towerFrequencyRepo.GetFrequenciesForTowerAsync("140", 1002, _filterData);
            var resultData = towerFrequencies.SingleOrDefault(rd => rd.Frequency == "775.59375");

            Assert.NotNull(resultData);
            Assert.Equal(_testTowerFrequencyResult2.Frequency, resultData.Frequency);
            Assert.Equal(_testTowerFrequencyResult2.Channel, resultData.Channel);
            Assert.Equal(_testTowerFrequencyResult2.Usage, resultData.Usage);
            Assert.Equal(_testTowerFrequencyResult2.AffiliationCount, resultData.AffiliationCount);
            Assert.Equal(_testTowerFrequencyResult2.DeniedCount, resultData.DeniedCount);
            Assert.Equal(_testTowerFrequencyResult2.VoiceGrantCount, resultData.VoiceGrantCount);
            Assert.Equal(_testTowerFrequencyResult2.EmergencyVoiceGrantCount, resultData.EmergencyVoiceGrantCount);
            Assert.Equal(_testTowerFrequencyResult2.EncryptedVoiceGrantCount, resultData.EncryptedVoiceGrantCount);
            Assert.Equal(_testTowerFrequencyResult2.DataCount, resultData.DataCount);
            Assert.Equal(_testTowerFrequencyResult2.PrivateDataCount, resultData.PrivateDataCount);
            Assert.Equal(_testTowerFrequencyResult2.CWIDCount, resultData.CWIDCount);
            Assert.Equal(_testTowerFrequencyResult2.AlertCount, resultData.AlertCount);
            Assert.Equal(_testTowerFrequencyResult2.FirstSeen, resultData.FirstSeen);
            Assert.Equal(_testTowerFrequencyResult2.LastSeen, resultData.LastSeen);

            Assert.Equal(recordCount, _testTowerFrequencyResult2.RecordCount);
        }

        [Fact]
        public async Task GetFrequenciesForTowerAsyncFiltersThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerFrequenciesGetFrequenciesForTowerFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerFrequencyRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerFrequencyRepo.GetFrequenciesForTowerAsync("140", 1002, _filterData));
        }

        [Fact]
        public async Task GetFrequenciesForTowerAllCountAsyncReturnsACount()
        {
            var mockTowerFrequencyCount = new Mock<MockObjectResult<int?>>();

            mockTowerFrequencyCount.Setup(tfc => tfc.GetEnumerator()).Returns(GetTowerFrequencyCount);
            _context.Setup(ctx => ctx.TowerFrequenciesGetFrequenciesForTowerAllCount(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerFrequencyCount.Object);
            SetupMockRepo();

            var towerFrequencyRepo = _mockRepo.Object;

            var result = await towerFrequencyRepo.GetFrequenciesForTowerAllCountAsync(1, 1002);

            Assert.Equal(_testTowerFrequencyCount, result);
        }

        [Fact]
        public async Task GetFrequenciesForTowerAllCountAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerFrequenciesGetFrequenciesForTowerAllCount(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerFrequencyRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerFrequencyRepo.GetFrequenciesForTowerAllCountAsync(1, 1002));
        }

        [Fact]
        public async Task GetFrequenciesForTowerAllAsyncReturnsAppropriateTypes()
        {
            var mockTowerFrequencyResults = new Mock<MockObjectResult<TowerFrequencies_Result>>();

            mockTowerFrequencyResults.Setup(tfr => tfr.GetEnumerator()).Returns(GetTowerFrequenciesResults);
            _context.Setup(ctx => ctx.TowerFrequenciesGetFrequenciesForTowerAll(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerFrequencyResults.Object);
            SetupMockRepo();

            var towerFrequencyRepo = _mockRepo.Object;

            var results = await towerFrequencyRepo.GetFrequenciesForTowerAllAsync(1, 1002);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<TowerFrequency>>(results);
            Assert.True(results.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetFrequenciesForTowerAllAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerFrequenciesGetFrequenciesForTowerAll(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerFrequencyRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerFrequencyRepo.GetFrequenciesForTowerAllAsync(1, 1002));
        }

        [Fact]
        public async Task GetFrequenciesForTowerAllAsyncFiltersReturnsAppropriateTypes()
        {
            var mockTowerFrequencyResults = new Mock<MockObjectResult<TowerFrequencies_Result>>();

            mockTowerFrequencyResults.Setup(tfr => tfr.GetEnumerator()).Returns(GetTowerFrequenciesResults);
            _context.Setup(ctx => ctx.TowerFrequenciesGetFrequenciesForTowerAllFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerFrequencyResults.Object);
            SetupMockRepo();

            var towerFrequencyRepo = _mockRepo.Object;

            var (towerFrequencies, recordCount) = await towerFrequencyRepo.GetFrequenciesForTowerAllAsync("140", 1002, _filterData);

            Assert.NotNull(towerFrequencies);
            Assert.IsAssignableFrom<IEnumerable<TowerFrequency>>(towerFrequencies);
            Assert.True(towerFrequencies.Count() > 0, "Result count is 0");
            Assert.IsAssignableFrom<int>(recordCount);
        }

        [Fact]
        public async Task GetFrequenciesForTowerAllAsyncFiltersThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerFrequenciesGetFrequenciesForTowerAllFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerFrequencyRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerFrequencyRepo.GetFrequenciesForTowerAllAsync("140", 1002, _filterData));
        }

        [Fact]
        public async Task GetFrequenciesForTowerNotCurrentCountAsyncReturnsACount()
        {
            var mockTowerFrequencyCount = new Mock<MockObjectResult<int?>>();

            mockTowerFrequencyCount.Setup(tfc => tfc.GetEnumerator()).Returns(GetTowerFrequencyCount);
            _context.Setup(ctx => ctx.TowerFrequenciesGetFrequenciesForTowerNotCurrentCount(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(mockTowerFrequencyCount.Object);
            SetupMockRepo();

            var towerFrequencyRepo = _mockRepo.Object;

            var result = await towerFrequencyRepo.GetFrequenciesForTowerNotCurrentCountAsync(1, 1002);

            Assert.Equal(_testTowerFrequencyCount, result);
        }

        [Fact]
        public async Task GetFrequenciesForTowerNotCurrentCountAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerFrequenciesGetFrequenciesForTowerNotCurrentCount(It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerFrequencyRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerFrequencyRepo.GetFrequenciesForTowerNotCurrentCountAsync(1, 1002));
        }

        [Fact]
        public async Task GetFrequenciesForTowerNotCurrentAsyncReturnsAppropriateTypes()
        {
            var mockTowerFrequencyResults = new Mock<MockObjectResult<TowerFrequencies_Result>>();

            mockTowerFrequencyResults.Setup(tfr => tfr.GetEnumerator()).Returns(GetTowerFrequenciesResults);
            _context.Setup(ctx => ctx.TowerFrequenciesGetFrequenciesForTowerNotCurrent(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(mockTowerFrequencyResults.Object);
            SetupMockRepo();

            var towerFrequencyRepo = _mockRepo.Object;

            var results = await towerFrequencyRepo.GetFrequenciesForTowerNotCurrentAsync(1, 1002);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<TowerFrequency>>(results);
            Assert.True(results.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetFrequenciesForTowerNotCurrentAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerFrequenciesGetFrequenciesForTowerNotCurrent(It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerFrequencyRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerFrequencyRepo.GetFrequenciesForTowerNotCurrentAsync(1, 1002));
        }

        [Fact]
        public async Task GetFrequenciesForTowerNotCurrentAsyncFiltersReturnsAppropriateTypes()
        {
            var mockTowerFrequencyResults = new Mock<MockObjectResult<TowerFrequencies_Result>>();

            mockTowerFrequencyResults.Setup(tfr => tfr.GetEnumerator()).Returns(GetTowerFrequenciesResults);
            _context.Setup(ctx => ctx.TowerFrequenciesGetFrequenciesForTowerNotCurrentFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockTowerFrequencyResults.Object);
            SetupMockRepo();

            var towerFrequencyRepo = _mockRepo.Object;

            var (towerFrequencies, recordCount) = await towerFrequencyRepo.GetFrequenciesForTowerNotCurrentAsync("140", 1002, _filterData);

            Assert.NotNull(towerFrequencies);
            Assert.IsAssignableFrom<IEnumerable<TowerFrequency>>(towerFrequencies);
            Assert.True(towerFrequencies.Count() > 0, "Result count is 0");
            Assert.IsAssignableFrom<int>(recordCount);
        }

        [Fact]
        public async Task GetFrequenciesForTowerNotCurrentAsyncFiltersThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerFrequenciesGetFrequenciesForTowerNotCurrentFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerFrequencyRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerFrequencyRepo.GetFrequenciesForTowerNotCurrentAsync("140", 1002, _filterData));
        }

        [Fact]
        public void EditRecordSetsAppropriateValues()
        {
            var databaseRecord = new TowerFrequencies();
            var towerFrequencyRepo = _mockRepo.Object;

            towerFrequencyRepo.EditRecord(databaseRecord, _testTowerFrequency);

            Assert.Equal(databaseRecord.SystemID, _testTowerFrequency.SystemID);
            Assert.Equal(databaseRecord.TowerID, _testTowerFrequency.TowerID);
            Assert.Equal(databaseRecord.Channel, _testTowerFrequency.Channel);
            Assert.Equal(databaseRecord.Usage, _testTowerFrequency.Usage);
            Assert.Equal(databaseRecord.Frequency, _testTowerFrequency.Frequency);
            Assert.Equal(databaseRecord.InputChannel, _testTowerFrequency.InputChannel);
            Assert.Equal(databaseRecord.InputFrequency, _testTowerFrequency.InputFrequency);
            Assert.Equal(databaseRecord.InputExplicit, _testTowerFrequency.InputExplicit);
            Assert.Equal(databaseRecord.HitCount, _testTowerFrequency.HitCount);
            Assert.Equal(databaseRecord.FirstSeen, _testTowerFrequency.FirstSeen);
            Assert.Equal(databaseRecord.LastSeen, _testTowerFrequency.LastSeen);
        }

        [Fact]
        public async Task WriteAsyncAddsNewRecord()
        {
            var addTowerFrequency = 0;
            var saveChanges = 0;
            var count = 0;

            _context.Setup(ctx => ctx.TowerFrequencies.Create()).Returns(new TowerFrequencies() { ID = 10 });
            _context.Setup(ctx => ctx.TowerFrequencies.Add(It.IsAny<TowerFrequencies>())).Callback(() => addTowerFrequency = ++count);
            _context.Setup(ctx => ctx.SaveChanges()).Callback(() => saveChanges = ++count);
            SetupMockRepo();

            _testTowerFrequency.IsNew = true;
            _testTowerFrequency.IsDirty = true;

            var towerFrequencyRepo = _mockRepo.Object;

            await towerFrequencyRepo.WriteAsync(_testTowerFrequency);

            _context.Verify(ctx => ctx.TowerFrequencies.Add(It.IsAny<TowerFrequencies>()), Times.Once());
            _context.Verify(ctx => ctx.SaveChanges(), Times.Once());

            Assert.Equal(1, addTowerFrequency);
            Assert.Equal(2, saveChanges);

            Assert.Equal(10, _testTowerFrequency.ID);
            Assert.False(_testTowerFrequency.IsNew);
            Assert.False(_testTowerFrequency.IsDirty);
        }

        [Fact]
        public async Task WriteAsyncUpdatesRecord()
        {
            var saveChanges = 0;
            var count = 0;
            var mockTowerFrequency = new Mock<MockObjectResult<TowerFrequencies>>();

            mockTowerFrequency.Setup(s => s.GetEnumerator()).Returns(GetTowerFrequency);
            _context.Setup(ctx => ctx.TowerFrequenciesGet(It.IsAny<int>())).Returns(mockTowerFrequency.Object);
            _context.Setup(ctx => ctx.SaveChanges()).Callback(() => saveChanges = ++count);
            SetupMockRepo();

            var towerFrequencyRepo = _mockRepo.Object;

            _testTowerFrequency.IsNew = false;
            _testTowerFrequency.IsDirty = true;

            await towerFrequencyRepo.WriteAsync(_testTowerFrequency);

            _context.Verify(ctx => ctx.SaveChanges(), Times.Once());

            Assert.Equal(1, saveChanges);

            Assert.False(_testTowerFrequency.IsNew);
            Assert.False(_testTowerFrequency.IsDirty);
        }

        [Fact]
        public async Task AddRecordAsyncThrowsExceptionWithDatabaseException()
        {
            var addSystem = 0;
            var count = 0;

            _context.Setup(ctx => ctx.TowerFrequencies.Create()).Returns(new TowerFrequencies());
            _context.Setup(ctx => ctx.TowerFrequencies.Add(It.IsAny<TowerFrequencies>())).Callback(() => addSystem = count++);
            _context.Setup(ctx => ctx.SaveChanges()).Throws(new Exception("Database error"));
            SetupMockRepo();

            _testTowerFrequency.IsNew = true;

            var towerFrequencyRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerFrequencyRepo.WriteAsync(_testTowerFrequency));
        }

        [Fact]
        public async Task UpdateRecordAsyncThrowsExceptionWithDatabaseException()
        {
            var mockTowerFrequency = new Mock<MockObjectResult<TowerFrequencies>>();

            mockTowerFrequency.Setup(s => s.GetEnumerator()).Returns(GetTowerFrequency);
            _context.Setup(ctx => ctx.TowerFrequenciesGet(It.IsAny<int>())).Returns(mockTowerFrequency.Object);
            _context.Setup(ctx => ctx.SaveChanges()).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerFrequencyRepo = _mockRepo.Object;

            _testTowerFrequency.IsNew = false;
            _testTowerFrequency.IsDirty = true;

            await Assert.ThrowsAsync<Exception>(() => towerFrequencyRepo.WriteAsync(_testTowerFrequency));
        }

        [Fact]
        public async Task DeleteAsyncDeletesRecord()
        {
            var deleteRecord = 0;
            var count = 0;

            _context.Setup(ctx => ctx.TowerFrequenciesDelete(It.IsAny<int>())).Callback(() => deleteRecord = ++count);
            SetupMockRepo();

            var towerFrequencyRepo = _mockRepo.Object;

            await towerFrequencyRepo.DeleteAsync(1);

            Assert.Equal(1, deleteRecord);
        }

        [Fact]
        public async Task DeleteAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerFrequenciesDelete(It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerFrequencyRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerFrequencyRepo.DeleteAsync(1));
        }
    }
}
