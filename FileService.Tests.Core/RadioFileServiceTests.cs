using DataLibrary.Interfaces;
using DataLibrary.TempData;
using FileService.Interfaces;
using Moq;
using ObjectLibrary;
using RadioHistoryService;
using System;
using System.Collections.Generic;

namespace FileService.Tests.Core
{
    public class RadioFileServiceTests
    {
        private readonly IRadioFileService _radioFileService;

        public RadioFileServiceTests()
        {
            var systemID = 1;
            var radioRepo = new Mock<IRadioRepository>();
            var radioHistoryService = new Mock<IRadioHistoryService>();
            var tempRadioService = new Mock<ITempService<TempRadio, Radio>>();
            var tempRadioHistoryService = new Mock<ITempService<TempRadioHistory, RadioHistory>>();
            var mergeService = new Mock<IMergeService>();

            _radioFileService = new RadioFileService(systemID, radioRepo.Object, radioHistoryService.Object, tempRadioService.Object,
                tempRadioHistoryService.Object, mergeService.Object);
        }

        public static IEnumerable<object[]> GetRadioData()
        {
            var data = new[]
            {
                new object[] { 140, 1917101, "ISP Dispatch (LaSalle) (17-A)", DateTime.Now, "ISP Dispatch (LaSalle) (17-A)" },
                new object[] { 140, 1917199, string.Empty, DateTime.Now, "<Unknown> (1917199)" }
            };

            return data;
        }
    }
}
