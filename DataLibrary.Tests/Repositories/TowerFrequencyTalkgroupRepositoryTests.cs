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
    public class TowerFrequencyTalkgroupRepositoryTests : RepositoryTestBase<TowerFrequencyTalkgroupRepository>
    {
        private readonly TowerFrequencyTalkgroups _testTowerFrequencyTalkgroup1 = new TowerFrequencyTalkgroups
        {
            ID = 1,
            SystemID = 1,
            TowerID = 1034,
            Frequency = "851.31250/1",
            TalkgroupID = 2427,
            Date = DateTime.Now.AddDays(-1),
            FirstSeen = DateTime.Now.AddYears(-2),
            LastSeen = DateTime.Now.AddDays(-1).AddHours(-2),
            LastModified = DateTime.Now,
            AffiliationCount = 550,
            DeniedCount = 1050,
            VoiceGrantCount = 1550,
            EmergencyVoiceGrantCount = 10050,
            EncryptedVoiceGrantCount = 15050,
            DataCount = 100050,
            PrivateDataCount = 150050,
            AlertCount = 1000050
        };

        private readonly TowerFrequencyTalkgroups _testTowerFrequencyTalkgroup2 = new TowerFrequencyTalkgroups
        {
            ID = 1,
            SystemID = 1,
            TowerID = 1034,
            Frequency = "851.31250/1",
            TalkgroupID = 2428,
            Date = DateTime.Now.AddDays(-2),
            FirstSeen = DateTime.Now.AddYears(-3),
            LastSeen = DateTime.Now.AddDays(-4).AddHours(-5),
            LastModified = DateTime.Now,
            AffiliationCount = 250,
            DeniedCount = 1250,
            VoiceGrantCount = 2250,
            EmergencyVoiceGrantCount = 10250,
            EncryptedVoiceGrantCount = 15250,
            DataCount = 100250,
            PrivateDataCount = 150250,
            AlertCount = 1002050
        };

        private readonly TowerFrequencyTalkgroups_Result _testTowerFrequencyTalkgroupResult1 = new TowerFrequencyTalkgroups_Result
        {
            TalkgroupID = 2427,
            TalkgroupDescription = "Grundy County Fire 1",
            FirstSeen = DateTime.Now.AddYears(-3),
            LastSeen = DateTime.Now.AddDays(-4).AddHours(-5),
            AffiliationCount = 250,
            DeniedCount = 1250,
            VoiceGrantCount = 2250,
            EmergencyVoiceGrantCount = 10250,
            EncryptedVoiceGrantCount = 15250,
            DataCount = 100250,
            PrivateDataCount = 150250,
            AlertCount = 1002050,
            RecordCount = 2
        };

        private readonly TowerFrequencyTalkgroups_Result _testTowerFrequencyTalkgroupResult2 = new TowerFrequencyTalkgroups_Result
        {
            TalkgroupID = 2428,
            TalkgroupDescription = "Grundy County Fire 2",
            FirstSeen = DateTime.Now.AddYears(-2),
            LastSeen = DateTime.Now.AddDays(-1).AddHours(-2),
            AffiliationCount = 550,
            DeniedCount = 1050,
            VoiceGrantCount = 1550,
            EmergencyVoiceGrantCount = 10050,
            EncryptedVoiceGrantCount = 15050,
            DataCount = 100050,
            PrivateDataCount = 150050,
            AlertCount = 1000050,
            RecordCount = 2
        };

        private IEnumerator<TowerFrequencyTalkgroups> GetTowerFrequencyTalkgroups()
        {
            yield return _testTowerFrequencyTalkgroup1;
            yield return _testTowerFrequencyTalkgroup2;
        }

        private IEnumerator<TowerFrequencyTalkgroups_Result> GetTowerFrequencyTalkgroupResults()
        {
            yield return _testTowerFrequencyTalkgroupResult1;
            yield return _testTowerFrequencyTalkgroupResult2;
        }

        [Fact]
        public async Task GetForTowerAsyncReturnAppropriateTypes()
        {
            var mockTowerFrequencyTalkgroups = new Mock<MockObjectResult<TowerFrequencyTalkgroups>>();

            mockTowerFrequencyTalkgroups.Setup(tftg => tftg.GetEnumerator()).Returns(GetTowerFrequencyTalkgroups);
            _context.Setup(ctx => ctx.TowerFrequencyTalkgroupsGetForTowerDate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(mockTowerFrequencyTalkgroups.Object);
            SetupMockRepo();

            var towerFrequencyTalkgroupRepo = _mockRepo.Object;

            var results = await towerFrequencyTalkgroupRepo.GetForTowerAsync(1, 1034, DateTime.Now);

            Assert.NotNull(results);
            Assert.True(results.Count() > 0, "Result count is 0");
            Assert.IsAssignableFrom<IEnumerable<TowerFrequencyTalkgroup>>(results);
        }

        [Fact]
        public async Task GetForTowerAsyncReturnsAppropriateValues()
        {
            var mockTowerFrequencyTalkgroups = new Mock<MockObjectResult<TowerFrequencyTalkgroups>>();

            mockTowerFrequencyTalkgroups.Setup(tftg => tftg.GetEnumerator()).Returns(GetTowerFrequencyTalkgroups);
            _context.Setup(ctx => ctx.TowerFrequencyTalkgroupsGetForTowerDate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(mockTowerFrequencyTalkgroups.Object);
            SetupMockRepo();

            var towerFrequencyTalkgroupRepo = _mockRepo.Object;

            var results = await towerFrequencyTalkgroupRepo.GetForTowerAsync(1, 1034, DateTime.Now);
            var resultsData = results.SingleOrDefault(tftg => tftg.Frequency == "851.31250/1" && tftg.TalkgroupID == 2428);

            Assert.NotNull(resultsData);
            Assert.Equal(_testTowerFrequencyTalkgroup2.SystemID, resultsData.SystemID);
            Assert.Equal(_testTowerFrequencyTalkgroup2.TowerID, resultsData.TowerID);
            Assert.Equal(_testTowerFrequencyTalkgroup2.Frequency, resultsData.Frequency);
            Assert.Equal(_testTowerFrequencyTalkgroup2.TalkgroupID, resultsData.TalkgroupID);
            Assert.Equal(_testTowerFrequencyTalkgroup2.Date, resultsData.Date);
            Assert.Equal(_testTowerFrequencyTalkgroup2.FirstSeen, resultsData.FirstSeen);
            Assert.Equal(_testTowerFrequencyTalkgroup2.LastSeen, resultsData.LastSeen);
            Assert.Equal(_testTowerFrequencyTalkgroup2.LastModified, resultsData.LastModified);
            Assert.Equal(_testTowerFrequencyTalkgroup2.AffiliationCount, resultsData.AffiliationCount);
            Assert.Equal(_testTowerFrequencyTalkgroup2.DeniedCount, resultsData.DeniedCount);
            Assert.Equal(_testTowerFrequencyTalkgroup2.VoiceGrantCount, resultsData.VoiceGrantCount);
            Assert.Equal(_testTowerFrequencyTalkgroup2.EmergencyVoiceGrantCount, resultsData.EmergencyVoiceGrantCount);
            Assert.Equal(_testTowerFrequencyTalkgroup2.EncryptedVoiceGrantCount, resultsData.EncryptedVoiceGrantCount);
            Assert.Equal(_testTowerFrequencyTalkgroup2.DataCount, resultsData.DataCount);
            Assert.Equal(_testTowerFrequencyTalkgroup2.PrivateDataCount, resultsData.PrivateDataCount);
            Assert.Equal(_testTowerFrequencyTalkgroup2.AlertCount, resultsData.AlertCount);
        }

        [Fact]
        public async Task GetForTowerAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerFrequencyTalkgroupsGetForTowerDate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                .Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerFrequencyTalkgroupRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerFrequencyTalkgroupRepo.GetForTowerAsync(1, 1034, DateTime.Now));
        }

        [Fact]
        public async Task GetTalkgroupsForTowerFrequencyAsyncReturnsAppropriateTypes()
        {
            var mockTowerFrequencyTalkgroupResults = new Mock<MockObjectResult<TowerFrequencyTalkgroups_Result>>();

            mockTowerFrequencyTalkgroupResults.Setup(tftg => tftg.GetEnumerator()).Returns(GetTowerFrequencyTalkgroupResults);
            _context.Setup(ctx => ctx.TowerFrequencyTalkgroupsGetTalkgroupsForFrequenciesWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(),
                It.IsAny<int>())).Returns(mockTowerFrequencyTalkgroupResults.Object);
            SetupMockRepo();

            var towerFrequencyTalkgroupRepo = _mockRepo.Object;

            var (towerFrequencyTalkgroups, recordCount) = await towerFrequencyTalkgroupRepo.GetTalkgroupsForTowerFrequencyAsync("140", 1034, "851.31250/1", _filterData);

            Assert.NotNull(towerFrequencyTalkgroups);
            Assert.IsAssignableFrom<IEnumerable<TowerFrequencyTalkgroup>>(towerFrequencyTalkgroups);
            Assert.True(towerFrequencyTalkgroups.Count() > 0, "Result count is 0");
            Assert.IsAssignableFrom<int>(recordCount);
        }

        [Fact]
        public async Task GetTalkgroupsForTowerFrequencyAsyncReturnsAppropriateValues()
        {
            var mockTowerFrequencyTalkgroupResults = new Mock<MockObjectResult<TowerFrequencyTalkgroups_Result>>();

            mockTowerFrequencyTalkgroupResults.Setup(tftg => tftg.GetEnumerator()).Returns(GetTowerFrequencyTalkgroupResults);
            _context.Setup(ctx => ctx.TowerFrequencyTalkgroupsGetTalkgroupsForFrequenciesWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(),
                It.IsAny<int>())).Returns(mockTowerFrequencyTalkgroupResults.Object);
            SetupMockRepo();

            var towerFrequencyTalkgroupRepo = _mockRepo.Object;

            var (towerFrequencyTalkgroups, recordCount) = await towerFrequencyTalkgroupRepo.GetTalkgroupsForTowerFrequencyAsync("140", 1034, "851.31250/1", _filterData);
            var resultData = towerFrequencyTalkgroups.SingleOrDefault(tftg => tftg.TalkgroupID == 2427);

            Assert.NotNull(resultData);
            Assert.Equal(_testTowerFrequencyTalkgroupResult1.TalkgroupID, resultData.TalkgroupID);
            Assert.Equal(_testTowerFrequencyTalkgroupResult1.TalkgroupDescription, resultData.TalkgroupName);
            Assert.Equal(_testTowerFrequencyTalkgroupResult1.FirstSeen, resultData.FirstSeen);
            Assert.Equal(_testTowerFrequencyTalkgroupResult1.LastSeen, resultData.LastSeen);
            Assert.Equal(_testTowerFrequencyTalkgroupResult1.AffiliationCount, resultData.AffiliationCount);
            Assert.Equal(_testTowerFrequencyTalkgroupResult1.DeniedCount, resultData.DeniedCount);
            Assert.Equal(_testTowerFrequencyTalkgroupResult1.VoiceGrantCount, resultData.VoiceGrantCount);
            Assert.Equal(_testTowerFrequencyTalkgroupResult1.EmergencyVoiceGrantCount, resultData.EmergencyVoiceGrantCount);
            Assert.Equal(_testTowerFrequencyTalkgroupResult1.EncryptedVoiceGrantCount, resultData.EncryptedVoiceGrantCount);
            Assert.Equal(_testTowerFrequencyTalkgroupResult1.DataCount, resultData.DataCount);
            Assert.Equal(_testTowerFrequencyTalkgroupResult1.PrivateDataCount, resultData.PrivateDataCount);
            Assert.Equal(_testTowerFrequencyTalkgroupResult1.AlertCount, resultData.AlertCount);

            Assert.Equal(_testTowerFrequencyTalkgroupResult1.RecordCount, recordCount);
        }

        [Fact]
        public async Task GetTalkgroupsForTowerFrequencyAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerFrequencyTalkgroupsGetTalkgroupsForFrequenciesWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(),
                It.IsAny<int>())).Throws(new Exception());
            SetupMockRepo();

            var towerFrequencyTalkgroupRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerFrequencyTalkgroupRepo.GetTalkgroupsForTowerFrequencyAsync("140", 1034, "851.31250/1", _filterData));
        }
    }
}
