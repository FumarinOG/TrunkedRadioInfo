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
    public class TalkgroupHistoryRepositoryTests : RepositoryTestBase<TalkgroupHistoryRepository>
    {
        public static readonly TalkgroupHistory _testTalkgroupHistory1 = new TalkgroupHistory
        {
            ID = 1,
            SystemID = 1,
            TalkgroupID = 9019,
            Description = "ISP 17-A - LaSalle Primary",
            FirstSeen = DateTime.Parse("01-01-2017 13:33"),
            LastSeen = DateTime.Now,
            LastModified = DateTime.Now
        };

        public static readonly TalkgroupHistory _testTalkgroupHistory2 = new TalkgroupHistory
        {
            ID = 2,
            SystemID = 1,
            TalkgroupID = 9019,
            Description = "ISP LaSalle 17A",
            FirstSeen = DateTime.Parse("06-03-2015 19:42"),
            LastSeen = DateTime.Parse("01-01-2017 10:21"),
            LastModified = DateTime.Parse("01-01-2017 10:22")
        };

        public static readonly TalkgroupHistory_Result _testTalkgroupHistoryResult1 = new TalkgroupHistory_Result
        {
            Description = "ISP 17-A - LaSalle Primary",
            FirstSeen = DateTime.Parse("01-01-2017 13:33"),
            LastSeen = DateTime.Now,
            RecordCount = 2
        };

        public static readonly TalkgroupHistory_Result _testTalkgroupHistoryResult2 = new TalkgroupHistory_Result
        {
            Description = "ISP LaSalle 17A",
            FirstSeen = DateTime.Parse("06-03-2015 19:42"),
            LastSeen = DateTime.Parse("01-01-2017 10:21"),
            RecordCount = 2
        };

        public static readonly TalkgroupHistoryForSystem_Result _testTalkgroupHistoryForSystemResult1 = new TalkgroupHistoryForSystem_Result
        {
            Description = "ISP 17-A - LaSalle Primary",
            FirstSeen = DateTime.Parse("01-01-2017 13:33"),
            LastSeen = DateTime.Now
        };

        public static readonly TalkgroupHistoryForSystem_Result _testTalkgroupHistoryForSystemResult2 = new TalkgroupHistoryForSystem_Result
        {
            Description = "ISP LaSalle 17A",
            FirstSeen = DateTime.Parse("06-03-2015 19:42"),
            LastSeen = DateTime.Parse("01-01-2017 10:21")
        };

        public static readonly int? _testTalkgroupHistoryCount = 375;

        public static IEnumerator<TalkgroupHistory> GetTalkgroupHistories()
        {
            yield return _testTalkgroupHistory1;
            yield return _testTalkgroupHistory2;
        }

        public static IEnumerator<TalkgroupHistory_Result> GetTalkgroupHistoryResults()
        {
            yield return _testTalkgroupHistoryResult1;
            yield return _testTalkgroupHistoryResult2;
        }

        public static IEnumerator<TalkgroupHistoryForSystem_Result> GetTalkgroupHistoryForSystemResults()
        {
            yield return _testTalkgroupHistoryForSystemResult1;
            yield return _testTalkgroupHistoryForSystemResult2;
        }

        public static IEnumerator<int?> GetTalkgroupHistoryCount()
        {
            yield return _testTalkgroupHistoryCount;
        }

        [Fact]
        public async Task GetForTalkgroupAsyncReturnsAppropriateTypes()
        {
            var mockTalkgroupHistories = new Mock<MockObjectResult<TalkgroupHistory>>();

            mockTalkgroupHistories.Setup(rh => rh.GetEnumerator()).Returns(GetTalkgroupHistories);
            _context.Setup(ctx => ctx.TalkgroupHistoryGetForTalkgroup(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTalkgroupHistories.Object);
            SetupMockRepo();

            var TalkgroupHistoryRepo = _mockRepo.Object;

            var result = await TalkgroupHistoryRepo.GetForTalkgroupAsync(1, 1917001);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<ObjectLibrary.TalkgroupHistory>>(result);
            Assert.True(result.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetForTalkgroupAsyncReturnsAppropriateValues()
        {
            var mockTalkgroupHistories = new Mock<MockObjectResult<TalkgroupHistory>>();

            mockTalkgroupHistories.Setup(rh => rh.GetEnumerator()).Returns(GetTalkgroupHistories);
            _context.Setup(ctx => ctx.TalkgroupHistoryGetForTalkgroup(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTalkgroupHistories.Object);
            SetupMockRepo();

            var TalkgroupHistoryRepo = _mockRepo.Object;

            var result = await TalkgroupHistoryRepo.GetForTalkgroupAsync(1, 1917001);
            var resultData = result.SingleOrDefault(rd => rd.ID == 2);

            Assert.NotNull(resultData);
            Assert.Equal(_testTalkgroupHistory2.ID, resultData.ID);
            Assert.Equal(_testTalkgroupHistory2.SystemID, resultData.SystemID);
            Assert.Equal(_testTalkgroupHistory2.TalkgroupID, resultData.TalkgroupID);
            Assert.Equal(_testTalkgroupHistory2.Description, resultData.Description);
            Assert.Equal(_testTalkgroupHistory2.FirstSeen, resultData.FirstSeen);
            Assert.Equal(_testTalkgroupHistory2.LastSeen, resultData.LastSeen);
            Assert.Equal(_testTalkgroupHistory2.LastModified, resultData.LastModified);
        }

        [Fact]
        public async Task GetForTalkgroupAsyncThrowsExceptionForDatabaseException()
        {
            _context.Setup(ctx => ctx.TalkgroupHistoryGetForTalkgroup(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var TalkgroupHistoryRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => TalkgroupHistoryRepo.GetForTalkgroupAsync(1, 1917001));
        }

        [Fact]
        public async Task GetForTalkgroupAsyncWithPagingReturnsAppropriateTypes()
        {
            var mockTalkgroupHistoryResults = new Mock<MockObjectResult<TalkgroupHistory_Result>>();

            mockTalkgroupHistoryResults.Setup(rh => rh.GetEnumerator()).Returns(GetTalkgroupHistoryResults);
            _context.Setup(ctx => ctx.TalkgroupHistoryGetForTalkgroupFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(mockTalkgroupHistoryResults.Object);
            SetupMockRepo();

            var talkgroupHistoryRepo = _mockRepo.Object;

            var (talkgroupHistory, recordCount) = await talkgroupHistoryRepo.GetForTalkgroupAsync("140", 1917001, _filterData);

            Assert.IsAssignableFrom<IEnumerable<ObjectLibrary.TalkgroupHistory>>(talkgroupHistory);
            Assert.True(talkgroupHistory.Count() > 0, "Result count is 0");
            Assert.IsAssignableFrom<int>(recordCount);
        }

        [Fact]
        public async Task GetForTalkgroupAsyncWithPagingThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TalkgroupHistoryGetForTalkgroupFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new Exception("Database error"));
            SetupMockRepo();

            var TalkgroupHistoryRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => TalkgroupHistoryRepo.GetForTalkgroupAsync("140", 1917001, _filterData));
        }

        [Fact]
        public async Task GetForTalkgroupCountAsyncReturnsAValue()
        {
            var mockTalkgroupHistoryCount = new Mock<MockObjectResult<int?>>();

            mockTalkgroupHistoryCount.Setup(rhc => rhc.GetEnumerator()).Returns(GetTalkgroupHistoryCount);
            _context.Setup(ctx => ctx.TalkgroupHistoryGetForTalkgroupCount(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTalkgroupHistoryCount.Object);
            SetupMockRepo();

            var TalkgroupHistoryRepo = _mockRepo.Object;

            var result = await TalkgroupHistoryRepo.GetForTalkgroupCountAsync(1, 1917001);

            Assert.IsAssignableFrom<int>(result);
        }

        [Fact]
        public async Task GetForTalkgroupCountAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TalkgroupHistoryGetForTalkgroupCount(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var TalkgroupHistoryRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => TalkgroupHistoryRepo.GetForTalkgroupCountAsync(1, 1917001));
        }

        [Fact]
        public async Task GetForSystemAsyncReturnsAppropriateTypes()
        {
            var mockTalkgroupHistory = new Mock<MockObjectResult<TalkgroupHistoryForSystem_Result>>();

            mockTalkgroupHistory.Setup(rh => rh.GetEnumerator()).Returns(GetTalkgroupHistoryForSystemResults);
            _context.Setup(ctx => ctx.TalkgroupHistoryGetForSystem(It.IsAny<int>())).Returns(mockTalkgroupHistory.Object);
            SetupMockRepo();

            var TalkgroupHistoryRepo = _mockRepo.Object;

            var result = await TalkgroupHistoryRepo.GetForSystemAsync(1);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<ObjectLibrary.TalkgroupHistory>>(result);
            Assert.True(result.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetForSystemAsyncReturnsAppropriateValues()
        {
            var mockTalkgroupHistory = new Mock<MockObjectResult<TalkgroupHistoryForSystem_Result>>();

            mockTalkgroupHistory.Setup(rh => rh.GetEnumerator()).Returns(GetTalkgroupHistoryForSystemResults);
            _context.Setup(ctx => ctx.TalkgroupHistoryGetForSystem(It.IsAny<int>())).Returns(mockTalkgroupHistory.Object);
            SetupMockRepo();

            var TalkgroupHistoryRepo = _mockRepo.Object;

            var result = await TalkgroupHistoryRepo.GetForSystemAsync(1);
            var resultData = result.SingleOrDefault(tgh => tgh.Description == "ISP 17-A - LaSalle Primary");

            Assert.NotNull(resultData);
            Assert.Equal(_testTalkgroupHistoryForSystemResult1.Description, resultData.Description);
            Assert.Equal(_testTalkgroupHistoryForSystemResult1.FirstSeen, resultData.FirstSeen);
            Assert.Equal(_testTalkgroupHistoryForSystemResult1.LastSeen, resultData.LastSeen);
        }

        [Fact]
        public async Task GetForSystemAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TalkgroupHistoryGetForSystem(It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var TalkgroupHistoryRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => TalkgroupHistoryRepo.GetForSystemAsync(1));
        }
    }
}
