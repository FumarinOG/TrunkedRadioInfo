using DataAccessLibrary;
using DataLibrary.Repositories;
using Moq;
using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DataLibrary.Tests.Repositories
{
    public class DatabaseStatsRepositoryTests : RepositoryTestBase<DatabaseStatsRepository>
    {
        private IEnumerator<DatabaseStats_Result> GetResult()
        {
            yield return new DatabaseStats_Result
            {
                ProcessedFilesCount = 1000,
                RowCount = 5000,
                SystemsCount = 10000,
                TalkgroupsCount = 15000,
                TalkgroupHistoryCount = 50000,
                RadiosCount = 100000,
                RadioHistoryCount = 150000,
                TowersCount = 500000,
                TowerFrequenciesCount = 1000000,
                TowerFrequencyUsageCount = 1500000,
                TowerTalkgroupsCount = 5000000,
                TowerRadiosCount = 5500000,
                TowerTalkgroupRadiosCount = 10000000,
                TowerFrequencyTalkgroupsCount = 15000000,
                TowerFrequencyRadiosCount = 50000000
            };
        }

        [Fact]
        public async Task GetDatabaseStatsAsyncReturnsProperModel()
        {
            var mockDatabaseStatsResult = new Mock<MockObjectResult<DatabaseStats_Result>>();

            mockDatabaseStatsResult.Setup(dsr => dsr.GetEnumerator()).Returns(GetResult());
            _context.Setup(entity => entity.f_DatabaseGetStats()).Returns(mockDatabaseStatsResult.Object);
            SetupMockRepo();
 
            var databaseStatsRepo = _mockRepo.Object;
            var result = await databaseStatsRepo.GetDatabaseStatsAsync();

            Assert.IsAssignableFrom<DatabaseStat>(result);
            Assert.Equal(1000, result.ProcessedFilesCount);
            Assert.Equal(5000, result.RowCount);
            Assert.Equal(10000, result.SystemsCount);
            Assert.Equal(15000, result.TalkgroupsCount);
            Assert.Equal(50000, result.TalkgroupHistoryCount);
            Assert.Equal(100000, result.RadiosCount);
            Assert.Equal(150000, result.RadioHistoryCount);
            Assert.Equal(500000, result.TowersCount);
            Assert.Equal(1000000, result.TowerFrequenciesCount);
            Assert.Equal(1500000, result.TowerFrequencyUsageCount);
            Assert.Equal(5000000, result.TowerTalkgroupsCount);
            Assert.Equal(5500000, result.TowerRadiosCount);
            Assert.Equal(10000000, result.TowerTalkgroupRadiosCount);
            Assert.Equal(15000000, result.TowerFrequencyTalkgroupsCount);
            Assert.Equal(50000000, result.TowerFrequencyRadiosCount);
        }

        [Fact]
        public async Task GetDatabaseStatsAsyncThrowsExceptionWhenDatabaseConnectionFails()
        {
            _context.Setup(ctx => ctx.f_DatabaseGetStats()).Throws(new Exception("Database error"));
            SetupMockRepo();

            var databaseStatsRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => databaseStatsRepo.GetDatabaseStatsAsync());
        }
    }
}
