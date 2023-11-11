using System;
using Xunit;

namespace DatabaseService.Tests.Core
{
    public class DatabaseStatsViewModelTests
    {
        [Fact]
        public void ConstructorAssignsProperValuesAndTextIsFormattedProperly()
        {
            var random = new Random(DateTime.Now.Millisecond);
            var processedFilesCount = random.Next(1, 2000000000);
            var rowCount = random.Next(1, 2000000000);
            var systemsCount = random.Next(1, 2000000000);
            var talkgroupsCount = random.Next(1, 2000000000);
            var talkgroupHistoryCount = random.Next(1, 2000000000);
            var radiosCount = random.Next(1, 2000000000);
            var radioHistoryCount = random.Next(1, 2000000000);
            var towersCount = random.Next(1, 2000000000);
            var towerFrequenciesCount = random.Next(1, 2000000000);
            var towerFrequencyUsageCount = random.Next(1, 2000000000);
            var towerTalkgroupsCount = random.Next(1, 2000000000);
            var towerRadiosCount = random.Next(1, 2000000000);
            var towerTalkgroupRadiosCount = random.Next(1, 2000000000);
            var towerFrequencyTalkgroupsCount = random.Next(1, 2000000000);
            var towerFrequencyRadiosCount = random.Next(1, 2000000000);

            var databaseStatsViewModel = new DatabaseStatsViewModel(processedFilesCount, rowCount, systemsCount, talkgroupsCount, talkgroupHistoryCount,
                radiosCount, radioHistoryCount, towersCount, towerFrequenciesCount, towerFrequencyUsageCount, towerTalkgroupsCount, towerRadiosCount,
                towerTalkgroupRadiosCount, towerFrequencyTalkgroupsCount, towerFrequencyRadiosCount);

            Assert.Equal($"{processedFilesCount:#,##0}", databaseStatsViewModel.ProcessedFilesCountText);
            Assert.Equal($"{rowCount:#,##0}", databaseStatsViewModel.RowCountText);
            Assert.Equal($"{systemsCount:#,##0}", databaseStatsViewModel.SystemsCountText);
            Assert.Equal($"{talkgroupsCount:#,##0}", databaseStatsViewModel.TalkgroupsCountText);
            Assert.Equal($"{talkgroupHistoryCount:#,##0}", databaseStatsViewModel.TalkgroupHistoryCountText);
            Assert.Equal($"{radiosCount:#,##0}", databaseStatsViewModel.RadiosCountText);
            Assert.Equal($"{radioHistoryCount:#,##0}", databaseStatsViewModel.RadioHistoryCountText);
            Assert.Equal($"{towersCount:#,##0}", databaseStatsViewModel.TowersCountText);
            Assert.Equal($"{towerFrequenciesCount:#,##0}", databaseStatsViewModel.TowerFrequenciesCountText);
            Assert.Equal($"{towerFrequencyUsageCount:#,##0}", databaseStatsViewModel.TowerFrequencyUsageCountText);
            Assert.Equal($"{towerTalkgroupsCount:#,##0}", databaseStatsViewModel.TowerTalkgroupsCountText);
            Assert.Equal($"{towerRadiosCount:#,##0}", databaseStatsViewModel.TowerRadiosCountText);
            Assert.Equal($"{towerTalkgroupRadiosCount:#,##0}", databaseStatsViewModel.TowerTalkgroupsRadiosCountText);
            Assert.Equal($"{towerFrequencyTalkgroupsCount:#,##0}", databaseStatsViewModel.TowerFrequencyTalkgroupsCountText);
            Assert.Equal($"{towerFrequencyRadiosCount:#,##0}", databaseStatsViewModel.TowerFrequencyRadiosCountText);
        }
    }
}
