using DataLibrary;
using DataLibrary.Interfaces;
using Moq;
using ObjectLibrary;
using ServiceCommon;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SystemInfoService.Tests.Core
{
    public class SystemInfoServiceTests
    {
        private ISystemInfoService _systemInfoService;

        public SystemInfoServiceTests()
        {
            var systemInfoRepo = new Mock<ISystemInfoRepository>();  
            var systemInfo = new SystemInfo
            {
                ID = 123,
                SystemID = "140",
                SystemIDDecimal = 320,
                Description = "Test System",
                WACN = "BEE00",
                TalkgroupCount = 100,
                RadioCount = 200,
                TowerCount = 10,
                RowCount = 10000
            };
            var systemList = new List<SystemInfo>
            {
                systemInfo
            };
 
            systemInfoRepo.Setup(repo => repo.GetAsync(systemInfo.ID)).ReturnsAsync(systemInfo);
            systemInfoRepo.Setup(repo => repo.GetAsync(987)).ReturnsAsync(default(SystemInfo));
            systemInfoRepo.Setup(repo => repo.GetAsync(systemInfo.SystemID)).ReturnsAsync(systemInfo);
            systemInfoRepo.Setup(repo => repo.GetSystemIDAsync(systemInfo.SystemID)).ReturnsAsync(systemInfo.ID);
            systemInfoRepo.Setup(repo => repo.GetListAsync()).ReturnsAsync(systemList);
            systemInfoRepo.Setup(repo => repo.GetListAsync(It.IsAny<FilterData>())).ReturnsAsync(systemList);

            _systemInfoService = new SystemInfoService(systemInfoRepo.Object);
        }

        [Fact]
        public async void GetWithValidIDReturnsSystemInfoObjectAsync()
        {
            var result = await _systemInfoService.GetAsync(123);

            Assert.IsAssignableFrom<SystemInfo>(result);
        }

        [Fact]
        public async void GetWithInvalidIDReturnsNullAsync()
        {
            var result = await _systemInfoService.GetAsync(987);

            Assert.Null(result);
        }

        [Fact]
        public async void GetWithValidSystemIDReturnsSystemInfoObject()
        {
            var result = await _systemInfoService.GetAsync("140");

            Assert.IsAssignableFrom<SystemInfo>(result);
        }

        [Fact]
        public async void GetSystemReturnsModel()
        {
            var result = await _systemInfoService.GetSystemAsync("140");

            Assert.IsAssignableFrom<SystemViewModel>(result);
        }

        [Fact]
        public async void GetSystemIDReturnsIDForSystemID()
        {
            var result = await _systemInfoService.GetSystemIDAsync("140");

            Assert.Equal(123, result);
        }

        [Fact]
        public async void GetNameReturnsNameForSystemID()
        {
            var result = await _systemInfoService.GetNameAsync("140");

            Assert.Equal("Test System", result);
        }

        [Fact]
        public async void GetListReturnsListOfModels()
        {
            var result = await _systemInfoService.GetListAsync();

            Assert.IsAssignableFrom<IEnumerable<SystemInfoViewModel>>(result);
            Assert.True(result.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async void GetListWithFilterModelReturnsListOfModels()
        {
            var filterDataModel = new FilterDataModel();

            var result = await _systemInfoService.GetListAsync(filterDataModel);

            Assert.IsAssignableFrom<IEnumerable<SystemInfoViewModel>>(result);
            Assert.True(result.Count() > 0, "Result count is 0");
        }
    }
}
