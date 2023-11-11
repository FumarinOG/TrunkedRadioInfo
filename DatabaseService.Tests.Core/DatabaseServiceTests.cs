using DataLibrary.Interfaces;
using Moq;
using ObjectLibrary;
using Xunit;

namespace DatabaseService.Tests.Core
{
    public class DatabaseServiceTests
    {
        private IDatabaseService _databaseService;

        public DatabaseServiceTests()
        {
            var databaseRepo = new Mock<IDatabaseStatsRepository>();
            var databaseStats = new DatabaseStat();

            databaseRepo.Setup(repo => repo.GetDatabaseStatsAsync()).ReturnsAsync(databaseStats);
            _databaseService = new DatabaseService(databaseRepo.Object);
        }

        [Fact]
        public async void GetDatabaseStatsReturnsProperViewModel()
        {
            var result = await _databaseService.GetDatabaseStatsAsync();

            Assert.IsAssignableFrom<DatabaseStatsViewModel>(result);
        }
    }
}
