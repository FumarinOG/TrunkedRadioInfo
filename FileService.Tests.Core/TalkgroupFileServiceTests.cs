using DataLibrary.Interfaces;
using DataLibrary.TempData;
using FileService.Interfaces;
using Moq;
using ObjectLibrary;
using System;
using System.Collections.Generic;
using TalkgroupHistoryService;

namespace FileService.Tests.Core
{
    public class TalkgroupFileServiceTests
    {
        private readonly ITalkgroupFileService _talkgroupFileService;

        public TalkgroupFileServiceTests()
        {
            var systemID = 1;
            var talkgroupRepo = new Mock<ITalkgroupRepository>();
            var talkgroupHistoryService = new Mock<ITalkgroupHistoryService>();
            var tempTalkgroupService = new Mock<ITempService<TempTalkgroup, Talkgroup>>();
            var tempTalkgroupHistoryService = new Mock<ITempService<TempTalkgroupHistory, TalkgroupHistory>>();
            var mergeService = new Mock<IMergeService>();

            _talkgroupFileService = new TalkgroupFileService(systemID, talkgroupRepo.Object, talkgroupHistoryService.Object, tempTalkgroupService.Object,
                tempTalkgroupHistoryService.Object, mergeService.Object);
        }

        public static IEnumerable<object[]> GetRadioData()
        {
            var data = new[]
            {
                new object[] { 140, 9019, "ISP 17-A - LaSalle Primary", DateTime.Now, "ISP 17-A - LaSalle Primary" },
                new object[] { 140, 9199, string.Empty, DateTime.Now, "<Unknown> (9199)" }
            };

            return data;
        }
    }
}
