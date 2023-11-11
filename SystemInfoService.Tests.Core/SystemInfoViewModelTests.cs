using System;
using Xunit;

namespace SystemInfoService.Tests.Core
{
    public class SystemInfoViewModelTests
    {
        [Fact]
        public void ConstructorAssignsValuesProperly()
        {
            var id = 1;
            var systemID = "System ID";
            var description = "Description";
            var firstSeen = DateTime.Now;
            var lastSeen = DateTime.Now;
            var talkgroupCount = 10000;
            var radioCount = 11000;
            var towerCount = 12000;
            var rowCount = 13000;
            var systemIDDescription = $"{systemID} - {description}";

            var systemInfoViewModel = new SystemInfoViewModel(id, systemID, description, firstSeen, lastSeen, talkgroupCount, radioCount, towerCount, rowCount);

            Assert.Equal(id, systemInfoViewModel.ID);
            Assert.Equal(systemID, systemInfoViewModel.SystemID);
            Assert.Equal(description, systemInfoViewModel.Description);
            Assert.Equal(firstSeen, systemInfoViewModel.FirstSeen);
            Assert.Equal(lastSeen, systemInfoViewModel.LastSeen);
            Assert.Equal(talkgroupCount, systemInfoViewModel.TalkgroupCount);
            Assert.Equal(radioCount, systemInfoViewModel.RadioCount);
            Assert.Equal(towerCount, systemInfoViewModel.TowerCount);
            Assert.Equal(rowCount, systemInfoViewModel.RowCount);
            Assert.Equal(systemIDDescription, systemInfoViewModel.SystemIDDescription);
        }
    }
}
