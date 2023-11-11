using DataAccessLibrary;
using DataLibrary.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DataLibrary.Tests.Repositories
{
    public class TowerFrequencyUsageRepositoryTests : RepositoryTestBase<TowerFrequencyUsageRepository>
    {
        private readonly TowerFrequencyUsage_Result _testTowerFrequencyUsageResult1 = new TowerFrequencyUsage_Result
        {
            Frequency = "851.57500",
            Channel = "00-0091",
            TalkgroupID = 25194,
            RadioID = 5764298,
            Date = DateTime.Parse("07-25-2018 02:31:25"),
            HitCount = 100,
            AffiliationCount = 200,
            DeniedCount = 300,
            VoiceGrantCount = 400,
            EmergencyVoiceGrantCount = 500,
            EncryptedVoiceGrantCount = 600,
            DataCount = 700,
            PrivateDataCount = 800,
            CWIDCount = 900,
            AlertCount = 1000,
            FirstSeen = DateTime.Now.AddYears(-1),
            LastSeen = DateTime.Now
        };

        private readonly TowerFrequencyUsage_Result _testTowerFrequencyUsageResult2 = new TowerFrequencyUsage_Result
        {
            Frequency = "853.23750",
            Channel = "00-0357",
            TalkgroupID = 25212,
            RadioID = 5725116,
            Date = DateTime.Parse("07-25-2018 02:36:29"),
            HitCount = 1000,
            AffiliationCount = 2000,
            DeniedCount = 3000,
            VoiceGrantCount = 4000,
            EmergencyVoiceGrantCount = 5000,
            EncryptedVoiceGrantCount = 6000,
            DataCount = 7000,
            PrivateDataCount = 8000,
            CWIDCount = 9000,
            AlertCount = 10000,
            FirstSeen = DateTime.Now.AddYears(-1),
            LastSeen = DateTime.Now
        };

        private IEnumerator<TowerFrequencyUsage_Result> GetTowerFrequencyUsageResults()
        {
            yield return _testTowerFrequencyUsageResult1;
            yield return _testTowerFrequencyUsageResult2;
        }

        [Fact]
        public async Task GetFrequenciesForTowerAsyncReturnsAppropriateValues()
        {
            var mockTowerFrequencyUsageResult = new Mock<MockObjectResult<TowerFrequencyUsage_Result>>();

            mockTowerFrequencyUsageResult.Setup(tfur => tfur.GetEnumerator()).Returns(GetTowerFrequencyUsageResults);
            _context.Setup(ctx => ctx.TowerFrequencyUsageGetFrequenciesForTower(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(mockTowerFrequencyUsageResult.Object);
            SetupMockRepo();

            var towerFrequencyUsageRepo = _mockRepo.Object;

            var results = await towerFrequencyUsageRepo.GetFrequenciesForTowerAsync(1, 5017);
            var resultData = results.SingleOrDefault(rd => rd.Frequency == "853.23750");

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<ObjectLibrary.TowerFrequencyUsage>>(results);
            Assert.True(results.Count() > 0, "Result count is 0");

            Assert.NotNull(resultData);
            Assert.Equal(_testTowerFrequencyUsageResult2.Frequency, resultData.Frequency);
            Assert.Equal(_testTowerFrequencyUsageResult2.Channel, resultData.Channel);
            Assert.Equal(_testTowerFrequencyUsageResult2.Date, resultData.Date);
            Assert.Equal(_testTowerFrequencyUsageResult2.HitCount, resultData.HitCount);
            Assert.Equal(_testTowerFrequencyUsageResult2.AffiliationCount, resultData.AffiliationCount);
            Assert.Equal(_testTowerFrequencyUsageResult2.DeniedCount, resultData.DeniedCount);
            Assert.Equal(_testTowerFrequencyUsageResult2.VoiceGrantCount, resultData.VoiceGrantCount);
            Assert.Equal(_testTowerFrequencyUsageResult2.EmergencyVoiceGrantCount, resultData.EmergencyVoiceGrantCount);
            Assert.Equal(_testTowerFrequencyUsageResult2.EncryptedVoiceGrantCount, resultData.EncryptedVoiceGrantCount);
            Assert.Equal(_testTowerFrequencyUsageResult2.DataCount, resultData.DataCount);
            Assert.Equal(_testTowerFrequencyUsageResult2.PrivateDataCount, resultData.PrivateDataCount);
            Assert.Equal(_testTowerFrequencyUsageResult2.CWIDCount, resultData.CWIDCount);
            Assert.Equal(_testTowerFrequencyUsageResult2.AlertCount, resultData.AlertCount);
            Assert.Equal(_testTowerFrequencyUsageResult2.FirstSeen, resultData.FirstSeen);
            Assert.Equal(_testTowerFrequencyUsageResult2.LastSeen, resultData.LastSeen);
        }

        [Fact]
        public async Task GetFrequenciesForTowerAsyncThrowsExceptionForDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerFrequencyUsageGetFrequenciesForTower(It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerFrequencyUsageRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerFrequencyUsageRepo.GetFrequenciesForTowerAsync(1, 5017));
        }

        [Fact]
        public async Task GetFrequenciesForTowerAsyncWithDatesReturnsAppropriateValues()
        {
            var mockTowerFrequencyUsageResult = new Mock<MockObjectResult<TowerFrequencyUsage_Result>>();

            mockTowerFrequencyUsageResult.Setup(tfur => tfur.GetEnumerator()).Returns(GetTowerFrequencyUsageResults);
            _context.Setup(ctx => ctx.TowerFrequencyUsageGetFrequenciesForTowerForDate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(mockTowerFrequencyUsageResult.Object);
            SetupMockRepo();

            var towerFrequencyUsageRepo = _mockRepo.Object;

            var results = await towerFrequencyUsageRepo.GetFrequenciesForTowerAsync(1, 5017, DateTime.Now);
            var resultsData = results.SingleOrDefault(rd => rd.Frequency == "851.57500");

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<ObjectLibrary.TowerFrequencyUsage>>(results);
            Assert.True(results.Count() > 0, "Result count is 0");

            Assert.NotNull(resultsData);
            Assert.Equal(_testTowerFrequencyUsageResult1.Frequency, resultsData.Frequency);
            Assert.Equal(_testTowerFrequencyUsageResult1.Channel, resultsData.Channel);
            Assert.Equal(_testTowerFrequencyUsageResult1.Date, resultsData.Date);
            Assert.Equal(_testTowerFrequencyUsageResult1.HitCount, resultsData.HitCount);
            Assert.Equal(_testTowerFrequencyUsageResult1.AffiliationCount, resultsData.AffiliationCount);
            Assert.Equal(_testTowerFrequencyUsageResult1.DeniedCount, resultsData.DeniedCount);
            Assert.Equal(_testTowerFrequencyUsageResult1.VoiceGrantCount, resultsData.VoiceGrantCount);
            Assert.Equal(_testTowerFrequencyUsageResult1.EmergencyVoiceGrantCount, resultsData.EmergencyVoiceGrantCount);
            Assert.Equal(_testTowerFrequencyUsageResult1.EncryptedVoiceGrantCount, resultsData.EncryptedVoiceGrantCount);
            Assert.Equal(_testTowerFrequencyUsageResult1.DataCount, resultsData.DataCount);
            Assert.Equal(_testTowerFrequencyUsageResult1.PrivateDataCount, resultsData.PrivateDataCount);
            Assert.Equal(_testTowerFrequencyUsageResult1.CWIDCount, resultsData.CWIDCount);
            Assert.Equal(_testTowerFrequencyUsageResult1.AlertCount, resultsData.AlertCount);
            Assert.Equal(_testTowerFrequencyUsageResult1.FirstSeen, resultsData.FirstSeen);
            Assert.Equal(_testTowerFrequencyUsageResult1.LastSeen, resultsData.LastSeen);
        }

        [Fact]
        public async Task GetFrequenciesForTowerAsyncWithDatesThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerFrequencyUsageGetFrequenciesForTowerForDate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                .Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerFrequencyUsageRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerFrequencyUsageRepo.GetFrequenciesForTowerAsync(1, 5017, DateTime.Now));
        }
    }
}
