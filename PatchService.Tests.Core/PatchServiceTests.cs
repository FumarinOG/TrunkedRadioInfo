using DataLibrary.Interfaces;
using Moq;
using ObjectLibrary;
using ServiceCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PatchService.Tests.Core
{
    public class PatchServiceTests
    {
        private IPatchService _patchService;

        public PatchServiceTests()
        {
            var patchRepo = new Mock<IPatchRepository>();
            var patch = new Patch
            {
                ID = 1,
                FromTalkgroupID = 9019,
                FromTalkgroupName = "ISP 17-A - LaSalle Primary",
                ToTalkgroupID = 9000,
                ToTalkgroupName = "ISP 01-A - Sterling Primary",
                Date = DateTime.Now,
                FirstSeen = DateTime.Now.AddYears(-1),
                LastSeen = DateTime.Now,
                HitCount = 100
            };
            var random = new Random(DateTime.Now.Millisecond);

            patchRepo.Setup(repo => repo.GetSummaryForSystemAsync(It.IsAny<int>())).ReturnsAsync(new List<Patch> { patch });
            patchRepo.Setup(repo => repo.GetSummaryAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(patch);
            //patchRepo.Setup(repo => repo.GetSummary(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<FilterData>()))
            //    .Returns((new List<Patch> { patch }, 15));
            //patchRepo.Setup(repo => repo.GetSummaryForSystem(It.IsAny<string>(), It.IsAny<FilterData>())).Returns((new List<Patch> { patch }, 1));
            patchRepo.Setup(repo => repo.GetForPatchByDateAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new List<Patch> { patch });
            patchRepo.Setup(repo => repo.GetForPatchByDateAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(),
                It.IsAny<int>())).ReturnsAsync(new List<Patch> { patch });
            patchRepo.Setup(repo => repo.GetForPatchByDateCountAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(random.Next);
            patchRepo.Setup(repo => repo.GetForSystemTalkgroupAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new List<Patch> { patch });
            patchRepo.Setup(repo => repo.GetForSystemTalkgroupCountAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(random.Next);
            //patchRepo.Setup(repo => repo.GetForSystemTalkgroup(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<FilterData>())).
            //    Returns((new List<Patch> { patch }, 1));
            patchRepo.Setup(repo => repo.GetCountForSystemAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(random.Next);
            //patchRepo.Setup(repo => repo.GetForSystemTower(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<FilterData>())).
            //    Returns((new List<Patch> { patch }, 1));

            _patchService = new PatchService(patchRepo.Object);
        }

        [Fact]
        public async Task GetSummaryForSystemReturnsListOfPatches()
        {
            var result = await _patchService.GetSummaryForSystemAsync(1);

            Assert.IsAssignableFrom<IEnumerable<Patch>>(result);
            Assert.True(result.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetSummaryWithoutFiltersDataReturnsProperViewModel()
        {
            var systemInfo = new SystemInfo
            {
                ID = 1
            };
            var result = await _patchService.GetSummaryAsync(systemInfo, 9019, 9000);

            Assert.IsAssignableFrom<PatchViewModel>(result);
        }

        [Fact]
        public async Task GetSummaryWithFiltersReturnsProperViewModel()
        {
            var filterDataModel = new FilterDataModel();

            var result = await _patchService.GetSummaryAsync("140", 9019, 9000, filterDataModel);

            Assert.IsAssignableFrom<(IEnumerable<PatchViewModel>, int)>(result);
        }

        [Fact]
        public async Task GetSummaryForSystemWithoutFiltersReturnsProperViewModel()
        {
            var filterData = new FilterDataModel();

            var result = await _patchService.GetSummaryForSystemAsync("140", filterData);

            Assert.IsAssignableFrom<(IEnumerable<PatchViewModel>, int)>(result);
        }

        [Fact]
        public async Task GetForPatchByDateReturnsProperViewModel()
        {
            var result = await _patchService.GetForPatchByDateAsync(140, 9019, 9000);

            Assert.IsAssignableFrom<IEnumerable<Patch>>(result);
            Assert.True(result.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetForPatchByDateWithPaginingReturnsProperViewModel()
        {
            var result = await _patchService.GetForPatchByDateAsync(140, 9000, 9019, 1, 15);

            Assert.IsAssignableFrom<IEnumerable<PatchDatesViewModel>>(result);
            Assert.True(result.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetForPatchByDateCountReturnsValue()
        {
            var result = await _patchService.GetForPatchByDateCountAsync(140, 9019, 9000);

            Assert.IsAssignableFrom<int>(result);
        }

        [Fact]
        public async Task GetForSystemTalkgroupWithoutFiltersReturnsObjectList()
        {
            var result = await _patchService.GetForSystemTalkgroupAsync(140, 9019);

            Assert.IsAssignableFrom<IEnumerable<Patch>>(result);
            Assert.True(result.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetForSystemTalkgroupCountReturnsValue()
        {
            var result = await _patchService.GetForSystemTalkgroupCountAsync(140, 9019);

            Assert.IsAssignableFrom<int>(result);
        }

        [Fact]
        public async Task GetForSystemTalkgroupWithFiltersReturnsProperViewModel()
        {
            var filterDataModel = new FilterDataModel();

            var result = await _patchService.GetForSystemTalkgroupAsync("140", 9019, filterDataModel);

            Assert.IsAssignableFrom<(IEnumerable<Patch>, int)>(result);
        }

        [Fact]
        public async Task GetCountForSystemRetursnValue()
        {
            var result = await _patchService.GetCountForSystemAsync(140, "LaSalle");

            Assert.IsAssignableFrom<int>(result);
        }

        [Fact]
        public async Task GetForSystemTowerReturnsProperViewModel()
        {
            var filterDataModel = new FilterDataModel();

            var result = await _patchService.GetForSystemTowerAsync("140", 9019, filterDataModel);

            Assert.IsAssignableFrom<(IEnumerable<Patch>, int)>(result);
        }
    }
}
