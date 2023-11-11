using DatabaseService;
using Moq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Controllers;
using Xunit;

namespace Web.Tests
{
    public class DatabaseControllerTests
    {
        [Fact]
        public async Task IndexReturnsViewWithData()
        {
            var databaseService = new Mock<IDatabaseService>();
            var databaseStatsViewModel = new DatabaseStatsViewModel(10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150);

            databaseService.Setup(service => service.GetDatabaseStatsAsync()).ReturnsAsync(databaseStatsViewModel);

            var controller = new DatabaseController(databaseService.Object);

            var result = (await controller.Index()) as ViewResult;
            var resultData = (DatabaseStatsViewModel)result.ViewData.Model;

            Assert.Equal("Data Stats", result.ViewBag.Title);

            Assert.Equal(databaseStatsViewModel.ProcessedFilesCount, resultData.ProcessedFilesCount);
            Assert.Equal(databaseStatsViewModel.RowCount, resultData.RowCount);
            Assert.Equal(databaseStatsViewModel.SystemsCount, resultData.SystemsCount);
            Assert.Equal(databaseStatsViewModel.TalkgroupsCount, resultData.TalkgroupsCount);
            Assert.Equal(databaseStatsViewModel.TalkgroupHistoryCount, resultData.TalkgroupHistoryCount);
            Assert.Equal(databaseStatsViewModel.RadiosCount, resultData.RadiosCount);
            Assert.Equal(databaseStatsViewModel.RadioHistoryCount, resultData.RadioHistoryCount);
            Assert.Equal(databaseStatsViewModel.TowersCount, resultData.TowersCount);
            Assert.Equal(databaseStatsViewModel.TowerFrequenciesCount, resultData.TowerFrequenciesCount);
            Assert.Equal(databaseStatsViewModel.TowerFrequencyUsageCount, resultData.TowerFrequencyUsageCount);
            Assert.Equal(databaseStatsViewModel.TowerTalkgroupsCount, resultData.TowerTalkgroupsCount);
            Assert.Equal(databaseStatsViewModel.TowerRadiosCount, resultData.TowerRadiosCount);
            Assert.Equal(databaseStatsViewModel.TowerTalkgroupRadiosCount, resultData.TowerTalkgroupRadiosCount);
            Assert.Equal(databaseStatsViewModel.TowerFrequencyTalkgroupsCount, resultData.TowerFrequencyTalkgroupsCount);
            Assert.Equal(databaseStatsViewModel.TowerFrequencyRadiosCount, resultData.TowerFrequencyRadiosCount);

            Assert.Equal(databaseStatsViewModel.ProcessedFilesCountText, resultData.ProcessedFilesCountText);
            Assert.Equal(databaseStatsViewModel.RowCountText, resultData.RowCountText);
            Assert.Equal(databaseStatsViewModel.SystemsCountText, resultData.SystemsCountText);
            Assert.Equal(databaseStatsViewModel.TalkgroupsCountText, resultData.TalkgroupsCountText);
            Assert.Equal(databaseStatsViewModel.TalkgroupHistoryCountText, resultData.TalkgroupHistoryCountText);
            Assert.Equal(databaseStatsViewModel.RadiosCountText, resultData.RadiosCountText);
            Assert.Equal(databaseStatsViewModel.RadioHistoryCountText, resultData.RadioHistoryCountText);
            Assert.Equal(databaseStatsViewModel.TowersCountText, resultData.TowersCountText);
            Assert.Equal(databaseStatsViewModel.TowerFrequenciesCountText, resultData.TowerFrequenciesCountText);
            Assert.Equal(databaseStatsViewModel.TowerFrequencyUsageCountText, resultData.TowerFrequencyUsageCountText);
            Assert.Equal(databaseStatsViewModel.TowerTalkgroupsCountText, resultData.TowerTalkgroupsCountText);
            Assert.Equal(databaseStatsViewModel.TowerRadiosCountText, resultData.TowerRadiosCountText);
            Assert.Equal(databaseStatsViewModel.TowerTalkgroupsRadiosCountText, resultData.TowerTalkgroupsRadiosCountText);
            Assert.Equal(databaseStatsViewModel.TowerFrequencyTalkgroupsCountText, resultData.TowerFrequencyTalkgroupsCountText);
            Assert.Equal(databaseStatsViewModel.TowerFrequencyRadiosCountText, resultData.TowerFrequencyRadiosCountText);
        }
    }
}
