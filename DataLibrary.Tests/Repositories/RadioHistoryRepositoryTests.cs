using DataAccessLibrary;
using DataLibrary.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DataLibrary.Tests.Repositories
{
    public class RadioHistoryRepositoryTests : RepositoryTestBase<RadioHistoryRepository>
    {
        public static readonly RadioHistory _testRadioHistory1 = new RadioHistory
        {
            ID = 1,
            SystemID = 1,
            RadioID = 1917001,
            Description = "ISP Dispatch (LaSalle) (17-A)",
            FirstSeen = DateTime.Parse("01-01-2017 13:33"),
            LastSeen = DateTime.Now,
            LastModified = DateTime.Now
        };

        public static readonly RadioHistory _testRadioHistory2 = new RadioHistory
        {
            ID = 2,
            SystemID = 1,
            RadioID = 1917001,
            Description = "ISP Dispatch (LaSalle)",
            FirstSeen = DateTime.Parse("06-03-2015 19:42"),
            LastSeen = DateTime.Parse("01-01-2017 10:21"),
            LastModified = DateTime.Parse("01-01-2017 10:22")
        };

        public static readonly RadioHistory_Result _testRadioHistoryResult1 = new RadioHistory_Result
        {
            Description = "ISP Dispatch (LaSalle) (17-A)",
            FirstSeen = DateTime.Parse("01-01-2017 13:33"),
            LastSeen = DateTime.Now,
            RecordCount = 2
        };

        public static readonly RadioHistory_Result _testRadioHistoryResult2 = new RadioHistory_Result
        {
            Description = "ISP Dispatch (LaSalle)",
            FirstSeen = DateTime.Parse("06-03-2015 19:42"),
            LastSeen = DateTime.Parse("01-01-2017 10:21"),
            RecordCount = 2
        };

        public static readonly int? _testRadioHistoryCount = 375;

        public static IEnumerator<RadioHistory> GetRadioHistories()
        {
            yield return _testRadioHistory1;
            yield return _testRadioHistory2;
        }

        public static IEnumerator<RadioHistory_Result> GetRadioHistoryResults()
        {
            yield return _testRadioHistoryResult1;
            yield return _testRadioHistoryResult2;
        }

        public static IEnumerator<int?> GetRadioHistoryCount()
        {
            yield return _testRadioHistoryCount;
        }

        [Fact]
        public async Task GetForRadioAsyncReturnsAppropriateTypes()
        {
            var mockRadioHistories = new Mock<MockObjectResult<RadioHistory>>();

            mockRadioHistories.Setup(rh => rh.GetEnumerator()).Returns(GetRadioHistories);
            _context.Setup(ctx => ctx.RadioHistoryGetForRadio(It.IsAny<int>(), It.IsAny<int>())).Returns(mockRadioHistories.Object);
            SetupMockRepo();

            var radioHistoryRepo = _mockRepo.Object;

            var result = await radioHistoryRepo.GetForRadioAsync(1, 1917001);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<ObjectLibrary.RadioHistory>>(result);
            Assert.True(result.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetForRadioAsyncReturnsAppropriateValues()
        {
            var mockRadioHistories = new Mock<MockObjectResult<RadioHistory>>();

            mockRadioHistories.Setup(rh => rh.GetEnumerator()).Returns(GetRadioHistories);
            _context.Setup(ctx => ctx.RadioHistoryGetForRadio(It.IsAny<int>(), It.IsAny<int>())).Returns(mockRadioHistories.Object);
            SetupMockRepo();

            var radioHistoryRepo = _mockRepo.Object;

            var result = await radioHistoryRepo.GetForRadioAsync(1, 1917001);
            var resultData = result.SingleOrDefault(rd => rd.ID == 2);

            Assert.NotNull(resultData);
            Assert.Equal(_testRadioHistory2.ID, resultData.ID);
            Assert.Equal(_testRadioHistory2.SystemID, resultData.SystemID);
            Assert.Equal(_testRadioHistory2.RadioID, resultData.RadioID);
            Assert.Equal(_testRadioHistory2.Description, resultData.Description);
            Assert.Equal(_testRadioHistory2.FirstSeen, resultData.FirstSeen);
            Assert.Equal(_testRadioHistory2.LastSeen, resultData.LastSeen);
            Assert.Equal(_testRadioHistory2.LastModified, resultData.LastModified);
        }

        [Fact]
        public async Task GetForRadioAsyncThrowsExceptionForDatabaseException()
        {
            _context.Setup(ctx => ctx.RadioHistoryGetForRadio(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var radioHistoryRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => radioHistoryRepo.GetForRadioAsync(1, 1917001));
        }

        [Fact]
        public async Task GetForRadioAsyncWithPagingReturnsAppropriateTypes()
        {
            var mockRadioHistoryResults = new Mock<MockObjectResult<RadioHistory_Result>>();

            mockRadioHistoryResults.Setup(rh => rh.GetEnumerator()).Returns(GetRadioHistoryResults);
            _context.Setup(ctx => ctx.RadioHistoryGetForRadioFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRadioHistoryResults.Object);
            SetupMockRepo();

            var radioHistoryRepo = _mockRepo.Object;

            var (radioHistory, recordCount) = await radioHistoryRepo.GetForRadioAsync("140", 1917001, _filterData);

            Assert.IsAssignableFrom<IEnumerable<ObjectLibrary.RadioHistory>>(radioHistory);
            Assert.IsAssignableFrom<int>(recordCount);
            Assert.True(radioHistory.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetForRadioAsyncWithPagingThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.RadioHistoryGetForRadioFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var radioHistoryRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => radioHistoryRepo.GetForRadioAsync("140", 1917001, _filterData));
        }

        [Fact]
        public async Task GetForRadioCountAsyncReturnsAValue()
        {
            var mockRadioHistoryCount = new Mock<MockObjectResult<int?>>();

            mockRadioHistoryCount.Setup(rhc => rhc.GetEnumerator()).Returns(GetRadioHistoryCount);
            _context.Setup(ctx => ctx.RadioHistoryGetForRadioCount(It.IsAny<int>(), It.IsAny<int>())).Returns(mockRadioHistoryCount.Object);
            SetupMockRepo();

            var radioHistoryRepo = _mockRepo.Object;

            var result = await radioHistoryRepo.GetForRadioCountAsync(1, 1917001);

            Assert.IsAssignableFrom<int>(result);
        }

        [Fact]
        public async Task GetForRadioCountAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.RadioHistoryGetForRadioCount(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var radioHistoryRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => radioHistoryRepo.GetForRadioCountAsync(1, 1917001));
        }

        [Fact]
        public async Task GetForSystemAsyncReturnsAppropriateTypes()
        {
            var mockRadioHistory = new Mock<MockObjectResult<RadioHistory>>();

            mockRadioHistory.Setup(rh => rh.GetEnumerator()).Returns(GetRadioHistories);
            _context.Setup(ctx => ctx.RadioHistoryGetForSystem(It.IsAny<int>())).Returns(mockRadioHistory.Object);
            SetupMockRepo();

            var radioHistoryRepo = _mockRepo.Object;

            var result = await radioHistoryRepo.GetForSystemAsync(1);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<ObjectLibrary.RadioHistory>>(result);
            Assert.True(result.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetForSystemAsyncReturnsAppropriateValues()
        {
            var mockRadioHistory = new Mock<MockObjectResult<RadioHistory>>();

            mockRadioHistory.Setup(rh => rh.GetEnumerator()).Returns(GetRadioHistories);
            _context.Setup(ctx => ctx.RadioHistoryGetForSystem(It.IsAny<int>())).Returns(mockRadioHistory.Object);
            SetupMockRepo();

            var radioHistoryRepo = _mockRepo.Object;

            var result = await radioHistoryRepo.GetForSystemAsync(1);
            var resultData = result.SingleOrDefault(rh => rh.ID == 1);

            Assert.NotNull(resultData);
            Assert.Equal(_testRadioHistory1.ID, resultData.ID);
            Assert.Equal(_testRadioHistory1.SystemID, resultData.SystemID);
            Assert.Equal(_testRadioHistory1.RadioID, resultData.RadioID);
            Assert.Equal(_testRadioHistory1.Description, resultData.Description);
            Assert.Equal(_testRadioHistory1.FirstSeen, resultData.FirstSeen);
            Assert.Equal(_testRadioHistory1.LastSeen, resultData.LastSeen);
            Assert.Equal(_testRadioHistory1.LastModified, resultData.LastModified);
        }

        [Fact]
        public async Task GetForSystemAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.RadioHistoryGetForSystem(It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var radioHistoryRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => radioHistoryRepo.GetForSystemAsync(1));
        }
    }
}
