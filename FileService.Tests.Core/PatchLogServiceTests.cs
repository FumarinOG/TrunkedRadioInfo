using DataLibrary.Interfaces;
using DataLibrary.TempData;
using FileService.Interfaces;
using Moq;
using ObjectLibrary;
using ProcessedFileService;
using RadioHistoryService;
using RadioService;
using System;
using System.Collections.Generic;
using SystemInfoService;
using TalkgroupHistoryService;
using TalkgroupRadioService;
using TalkgroupService;
using TowerFrequencyRadioService;
using TowerFrequencyTalkgroupService;
using TowerFrequencyUsageService;
using TowerRadioService;
using TowerService;
using TowerTalkgroupRadioService;
using TowerTalkgroupService;
using Xunit;

namespace FileService.Tests.Core
{
    public class PatchLogServiceTests
    {
        private readonly PatchLogService _patchLogService;

        public PatchLogServiceTests()
        {
            var systemID = 1;
            var towerNumber = 1031;
            var uploadFile = new UploadFileModel { FileName = "20180706-PatchLog-131.csv", LastModified = DateTime.Now };
            var patchRepository = new Mock<IPatchRepository>();
            var tempPatchService = new Mock<ITempService<TempPatch, Patch>>();
            var radioService = new Mock<IRadioService>();
            var radioHistoryService = new Mock<IRadioHistoryService>();
            var systemInfoService = new Mock<ISystemInfoService>();
            var talkgroupService = new Mock<ITalkgroupService>();
            var talkgroupHistoryService = new Mock<ITalkgroupHistoryService>();
            var talkgroupRadioService = new Mock<ITalkgroupRadioService>();
            var towerService = new Mock<ITowerService>();
            var towerFrequencyRadioService = new Mock<ITowerFrequencyRadioService>();
            var towerFrequencyTalkgroupService = new Mock<ITowerFrequencyTalkgroupService>();
            var towerFrequencyUsageService = new Mock<ITowerFrequencyUsageService>();
            var towerRadioService = new Mock<ITowerRadioService>();
            var towerTalkgroupService = new Mock<ITowerTalkgroupService>();
            var towerTalkgroupRadioService = new Mock<ITowerTalkgroupRadioService>();
            var mergeService = new Mock<IMergeService>();
            var tempRadioService = new Mock<ITempService<TempRadio, Radio>>();
            var tempRadioHistoryService = new Mock<ITempService<TempRadioHistory, RadioHistory>>();
            var tempSystemInfoService = new Mock<ITempService<TempSystemInfo, SystemInfo>>();
            var tempTalkgroupService = new Mock<ITempService<TempTalkgroup, Talkgroup>>();
            var tempTalkgroupHistoryService = new Mock<ITempService<TempTalkgroupHistory, TalkgroupHistory>>();
            var tempTalkgroupRadioService = new Mock<ITempService<TempTalkgroupRadio, TalkgroupRadio>>();
            var tempTowerService = new Mock<ITempService<TempTower, Tower>>();
            var tempTowerFrequencyRadioService = new Mock<ITempService<TempTowerFrequencyRadio, TowerFrequencyRadio>>();
            var tempTowerFrequencyTalkgroupService = new Mock<ITempService<TempTowerFrequencyTalkgroup, TowerFrequencyTalkgroup>>();
            var tempTowerFrequencyUsageService = new Mock<ITempService<TempTowerFrequencyUsage, TowerFrequencyUsage>>();
            var tempTowerRadioService = new Mock<ITempService<TempTowerRadio, TowerRadio>>();
            var tempTowerTalkgroupService = new Mock<ITempService<TempTowerTalkgroup, TowerTalkgroup>>();
            var tempTowerTalkgroupRadioService = new Mock<ITempService<TempTowerTalkgroupRadio, TowerTalkgroupRadio>>();

            var patchList = new List<Patch>
            {
                new Patch()
                {
                    SystemID = 1,
                    TowerNumber = 1,
                    Date = new DateTime(2018, 7, 1),
                    FromTalkgroupID = 9000,
                    ToTalkgroupID = 9001,
                    FirstSeen = DateTime.Now.AddYears(-1),
                    LastSeen = DateTime.Now,
                    HitCount = 10
                },
                new Patch()
                {
                    SystemID = 1,
                    TowerNumber = 1,
                    Date = new DateTime(2018, 7, 1),
                    FromTalkgroupID = 9002,
                    ToTalkgroupID = 9003,
                    FirstSeen = DateTime.Now.AddYears(-1),
                    LastSeen = DateTime.Now,
                    HitCount = 10 },
                new Patch()
                {
                    SystemID = 1,
                    TowerNumber = 1,
                    Date = new DateTime(2018, 7, 1),
                    FromTalkgroupID = 9004,
                    ToTalkgroupID = 9005,
                    FirstSeen = DateTime.Now.AddYears(-1),
                    LastSeen = DateTime.Now,
                    HitCount = 10
                },
                new Patch()
                {
                    SystemID = 1,
                    TowerNumber = 1,
                    Date = new DateTime(2018, 7, 1),
                    FromTalkgroupID = 9006,
                    ToTalkgroupID = 9007,
                    FirstSeen = DateTime.Now.AddYears(-1),
                    LastSeen = DateTime.Now,
                    HitCount = 10
                },
                new Patch()
                {
                    SystemID = 1,
                    TowerNumber = 1,
                    Date = new DateTime(2018, 7, 1),
                    FromTalkgroupID = 9008,
                    ToTalkgroupID = 9009,
                    FirstSeen = DateTime.Now.AddYears(-1),
                    LastSeen = DateTime.Now,
                    HitCount = 10
                }
            };

            patchRepository.Setup(repo => repo.GetForSystemAsync(It.IsAny<int>())).ReturnsAsync(patchList);
            systemInfoService.Setup(service => service.GetAsync(It.IsAny<int>())).ReturnsAsync(new SystemInfo() { ID = 1, SystemID = "140" });

            _patchLogService = new PatchLogService(systemID, towerNumber, uploadFile, patchRepository.Object, tempPatchService.Object,
                radioService.Object, radioHistoryService.Object, systemInfoService.Object, talkgroupService.Object, talkgroupHistoryService.Object,
                talkgroupRadioService.Object, towerService.Object, towerFrequencyRadioService.Object, towerFrequencyTalkgroupService.Object,
                towerFrequencyUsageService.Object, towerRadioService.Object, towerTalkgroupService.Object, towerTalkgroupRadioService.Object,
                mergeService.Object, tempRadioService.Object, tempRadioHistoryService.Object, tempSystemInfoService.Object,
                tempTalkgroupService.Object, tempTalkgroupHistoryService.Object, tempTalkgroupRadioService.Object,
                tempTowerService.Object, tempTowerFrequencyRadioService.Object, tempTowerFrequencyTalkgroupService.Object,
                tempTowerFrequencyUsageService.Object, tempTowerRadioService.Object, tempTowerTalkgroupService.Object,
                tempTowerTalkgroupRadioService.Object);
        }

        [Theory]
        [MemberData(nameof(GetPatchesForDescriptionTests))]
        public void ParseDescriptionParsesDescription(PatchLog patchLog, PatchLog.Actions expectedAction)
        {
            PatchLogService.ParseDescription(patchLog);

            Assert.Equal(expectedAction, patchLog.Action);
        }

        public static IEnumerable<object[]> GetPatchesForDescriptionTests()
        {
            var data = new[]
            {
                new object[]
                {
                    new PatchLog { Description = "Added Patch:   9013 (ISP 05-A - Joliet Primary) --> 9002 (ISP 02-A - Elgin Primary)" },
                    PatchLog.Actions.Added
                },
                new object[] 
                {
                    new PatchLog { Description = "Removed Patch: 9005 (ISP Chicago A - Priority) --> 9007 (ISP Chicago C - South)" },
                    PatchLog.Actions.Removed
                }
            };

            return data;
        }

        [Fact]
        public void ParseDescriptionThrowsExceptionForInvalidData()
        {
            var patchLogShort = new PatchLog { Description = "Invalid" };
            var patchLogLong = new PatchLog { Description = "This is invalid data" };

            Assert.Throws<ArgumentOutOfRangeException>(() => PatchLogService.ParseDescription(patchLogShort));
            Assert.Throws<Exception>(() => PatchLogService.ParseDescription(patchLogLong));
        }

        [Theory]
        [MemberData(nameof(GetPatchesForTalkgroupTests))]
        public void GetTalkgroupIDsParsesTextProperly(PatchLog patchLog, int expectedFromTalkgroupID, int expectedToTalkgroupID)
        {
            var patch = new Patch();

            PatchLogService.GetTalkgroupIDs(patchLog, patch);

            Assert.Equal(expectedFromTalkgroupID, patch.FromTalkgroupID);
            Assert.Equal(expectedToTalkgroupID, patch.ToTalkgroupID);
        }

        public static IEnumerable<object[]> GetPatchesForTalkgroupTests()
        {
            var data = new[]
            {
                new object[]
                {
                    new PatchLog
                    {
                        Action = PatchLog.Actions.Added,
                        Description = "Added Patch:   9013 (ISP 05-A - Joliet Primary) --> 9002 (ISP 02-A - Elgin Primary)"
                    }, 9013, 9002
                },
                new object[]
                {
                    new PatchLog
                    {
                        Action = PatchLog.Actions.Removed,
                        Description = "Removed Patch: 9005 (ISP Chicago A - Priority) --> 9007 (ISP Chicago C - South)"
                    }, 9005, 9007
                }
            };

            return data;
        }

        [Fact]
        public void GetTalkgroupIDThrowsExceptionsForInvalidFromTalkgroup()
        {
            var patchLog = new PatchLog
            {
                Action = PatchLog.Actions.Added,
                Description = "Added Patch:   901A (ISP 05-A - Joliet Primary) --> 9002 (ISP 02-A - Elgin Primary)"
            };
            var patch = new Patch();

            Assert.Throws<Exception>(() => PatchLogService.GetTalkgroupIDs(patchLog, patch));
        }

        [Fact]
        public void GetTalkgroupIDThrowsExceptionsForInvalidToTalkgroup()
        {
            var patchLog = new PatchLog
            {
                Action = PatchLog.Actions.Added,
                Description = "Added Patch:   9013 (ISP 05-A - Joliet Primary) --> 90B2 (ISP 02-A - Elgin Primary)"
            };
            var patch = new Patch();

            Assert.Throws<Exception>(() => PatchLogService.GetTalkgroupIDs(patchLog, patch));
        }
    }
}
