using DataAccessLibrary;
using DataLibrary.Repositories;
using Moq;
using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DataLibrary.Tests.Repositories
{
    public class PatchRepositoryTests : RepositoryTestBase<PatchRepository>
    {
        private static readonly Patches _testPatch1 = new Patches
        {
            ID = 1,
            SystemID = 1,
            TowerID = 1,
            FromTalkgroupID = 9000,
            ToTalkgroupID = 9001,
            Date = DateTime.Now,
            FirstSeen = DateTime.Now.AddYears(-1),
            LastSeen = DateTime.Now,
            HitCount = 500,
            LastModified = DateTime.Now
        };

        private static readonly Patches _testPatch2 = new Patches
        {
            ID = 2,
            SystemID = 2,
            TowerID = 2,
            FromTalkgroupID = 9019,
            ToTalkgroupID = 9020,
            Date = DateTime.Now,
            FirstSeen = DateTime.Now.AddYears(-1),
            LastSeen = DateTime.Now,
            HitCount = 5000,
            LastModified = DateTime.Now
        };

        private static readonly PatchesSummary_Result _testPatchSummaryResult1 = new PatchesSummary_Result
        {
            FromTalkgroupID = 9000,
            FromTalkgroupDescription = "ISP 01-A - Sterling Primary",
            ToTalkgroupID = 9001,
            ToTalkgroupDescription = "ISP 01-B - Sterling Alternate",
            HitCount = 5000,
            FirstSeen = DateTime.Now.AddYears(-1),
            LastSeen = DateTime.Now,
            RecordCount = 2
        };

        private static readonly PatchesSummary_Result _testPatchSummaryResult2 = new PatchesSummary_Result
        {
            FromTalkgroupID = 9019,
            FromTalkgroupDescription = "ISP 17-A - LaSalle Primary",
            ToTalkgroupID = 9020,
            ToTalkgroupDescription = "ISP 17-B - LaSalle Alternate",
            HitCount = 10000,
            FirstSeen = DateTime.Now.AddYears(-1),
            LastSeen = DateTime.Now,
            RecordCount = 2
        };

        private static readonly PatchesByDate_Result _testPatchByDateResult1 = new PatchesByDate_Result
        {
            FromTalkgroupID = 9000,
            FromTalkgroupDescription = "ISP 01-A - Sterling Primary",
            ToTalkgroupID = 9001,
            ToTalkgroupDescription = "ISP 01-B - Sterling Alternate",
            TowerNumber = 1031,
            TowerDescription = "LaSalle (LaSalle)",
            Date = DateTime.Now,
            HitCount = 5000
        };

        private static readonly PatchesByDate_Result _testPatchByDateResult2 = new PatchesByDate_Result
        {
            FromTalkgroupID = 9019,
            FromTalkgroupDescription = "ISP 17-A - LaSalle Primary",
            ToTalkgroupID = 9020,
            ToTalkgroupDescription = "ISP 17-B - LaSalle Alternate",
            TowerNumber = 1031,
            TowerDescription = "LaSalle (LaSalle)",
            Date = DateTime.Now,
            HitCount = 50000
        };

        private static readonly Patches_Result _testPatchResult1 = new Patches_Result
        {
            TowerNumber = 1031,
            TowerDescription = "LaSalle (LaSalle)",
            FromTalkgroupID = 9000,
            FromTalkgroupDescription = "ISP 01-A - Sterling Primary",
            ToTalkgroupID = 9001,
            ToTalkgroupDescription = "ISP 01-B - Sterling Alternate",
            FirstSeen = DateTime.Now.AddYears(-1),
            LastSeen = DateTime.Now,
            HitCount = 100000
        };

        private static readonly Patches_Result _testPatchResult2 = new Patches_Result
        {
            TowerNumber = 1031,
            TowerDescription = "LaSalle (LaSalle)",
            FromTalkgroupID = 9019,
            FromTalkgroupDescription = "ISP 17-A - LaSalle Primary",
            ToTalkgroupID = 9020,
            ToTalkgroupDescription = "ISP 17-B - LaSalle Alternate",
            FirstSeen = DateTime.Now.AddYears(-1),
            LastSeen = DateTime.Now,
            HitCount = 100000
        };

        private static readonly int? _testCount1 = 50000;

        public static IEnumerator<Patches> GetPatch()
        {
            yield return _testPatch1;
        }

        public static IEnumerator<Patches> GetPatches()
        {
            yield return _testPatch1;
            yield return _testPatch2;
        }

        public static IEnumerator<PatchesSummary_Result> GetPatchSummary()
        {
            yield return _testPatchSummaryResult1;
        }

        public static IEnumerator<PatchesSummary_Result> GetPatchSummaries()
        {
            yield return _testPatchSummaryResult1;
            yield return _testPatchSummaryResult2;
        }

        public static IEnumerator<PatchesByDate_Result> GetPatchesByDate()
        {
            yield return _testPatchByDateResult1;
            yield return _testPatchByDateResult2;
        }

        public static IEnumerator<Patches_Result> GetPatchResult()
        {
            yield return _testPatchResult1;
        }

        public static IEnumerator<Patches_Result> GetPatchResults()
        {
            yield return _testPatchResult1;
            yield return _testPatchResult2;
        }

        public static IEnumerator<int?> GetCount()
        {
            yield return _testCount1;
        }

        [Fact]
        public async Task GetAsyncReturnsProperObject()
        {
            var mockPatches = new Mock<MockObjectResult<Patches>>();

            mockPatches.Setup(p => p.GetEnumerator()).Returns(GetPatch);
            _context.Setup(ctx => ctx.PatchesGet(It.IsAny<int>())).Returns(mockPatches.Object);
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;
            var result = await patchRepo.GetAsync(1);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<Patch>(result);
            Assert.Equal(_testPatch1.ID, result.ID);
            Assert.Equal(_testPatch1.SystemID, result.SystemID);
            Assert.Equal(_testPatch1.TowerID, result.TowerNumber);
            Assert.Equal(_testPatch1.FromTalkgroupID, result.FromTalkgroupID);
            Assert.Equal(_testPatch1.ToTalkgroupID, result.ToTalkgroupID);
            Assert.Equal(_testPatch1.Date, result.Date);
            Assert.Equal(_testPatch1.FirstSeen, result.FirstSeen);
            Assert.Equal(_testPatch1.LastSeen, result.LastSeen);
            Assert.Equal(_testPatch1.HitCount, result.HitCount);
            Assert.Equal(_testPatch1.LastModified, result.LastModified);
        }

        [Fact]
        public async Task GetAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.PatchesGet(It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => patchRepo.GetAsync(1));
        }

        [Fact]
        public async Task GetForSystemAsyncReturnsAppropriateTypeOfObjects()
        {
            var mockPatches = new Mock<MockObjectResult<Patches>>();

            mockPatches.Setup(p => p.GetEnumerator()).Returns(GetPatches);
            _context.Setup(ctx => ctx.PatchesGetForSystem(It.IsAny<int>())).Returns(mockPatches.Object);
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;
            var result = await patchRepo.GetForSystemAsync(1);

            Assert.IsAssignableFrom<IEnumerable<Patch>>(result);
            Assert.True(result.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetForSystemAsyncReturnsAppropriateObject()
        {
            var mockPatches = new Mock<MockObjectResult<Patches>>();

            mockPatches.Setup(p => p.GetEnumerator()).Returns(GetPatches);
            _context.Setup(ctx => ctx.PatchesGetForSystem(It.IsAny<int>())).Returns(mockPatches.Object);
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;
            var result = await patchRepo.GetForSystemAsync(1);
            var resultData = result.SingleOrDefault(rd => rd.ID == 2);

            Assert.NotNull(resultData);
            Assert.Equal(_testPatch2.ID, resultData.ID);
            Assert.Equal(_testPatch2.SystemID, resultData.SystemID);
            Assert.Equal(_testPatch2.TowerID, resultData.TowerNumber);
            Assert.Equal(_testPatch2.FromTalkgroupID, resultData.FromTalkgroupID);
            Assert.Equal(_testPatch2.ToTalkgroupID, resultData.ToTalkgroupID);
            Assert.Equal(_testPatch2.Date, resultData.Date);
            Assert.Equal(_testPatch2.FirstSeen, resultData.FirstSeen);
            Assert.Equal(_testPatch2.LastSeen, resultData.LastSeen);
            Assert.Equal(_testPatch2.HitCount, resultData.HitCount);
            Assert.Equal(_testPatch2.LastModified, resultData.LastModified);
        }

        [Fact]
        public async Task GetForSystemAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.PatchesGetForSystem(It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => patchRepo.GetForSystemAsync(1));
        }

        [Fact]
        public async Task GetSummaryAsyncReturnsAppropriateObject()
        {
            var mockPatchSummary = new Mock<MockObjectResult<PatchesSummary_Result>>();

            mockPatchSummary.Setup(ps => ps.GetEnumerator()).Returns(GetPatchSummary);
            _context.Setup(ctx => ctx.PatchesGetSummary(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockPatchSummary.Object);
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;
            var result = await patchRepo.GetSummaryAsync(1, 1, 1);

            Assert.IsAssignableFrom<Patch>(result);

            Assert.Equal(_testPatchSummaryResult1.FromTalkgroupID, result.FromTalkgroupID);
            Assert.Equal(_testPatchSummaryResult1.FromTalkgroupDescription, result.FromTalkgroupName);
            Assert.Equal(_testPatchSummaryResult1.ToTalkgroupID, result.ToTalkgroupID);
            Assert.Equal(_testPatchSummaryResult1.ToTalkgroupDescription, result.ToTalkgroupName);
            Assert.Equal(_testPatchSummaryResult1.HitCount, result.HitCount);
            Assert.Equal(_testPatchSummaryResult1.FirstSeen, result.FirstSeen);
            Assert.Equal(_testPatchSummaryResult1.LastSeen, result.LastSeen);
        }

        [Fact]
        public async Task GetSummaryAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.PatchesGetSummary(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => patchRepo.GetSummaryAsync(1, 1, 1));

        }

        [Fact]
        public async Task GetSummaryAsyncReturnsAppropriateTypeOfObjectsForSystem()
        {
            var mockPatchSummary = new Mock<MockObjectResult<PatchesSummary_Result>>();

            mockPatchSummary.Setup(ps => ps.GetEnumerator()).Returns(GetPatchSummaries);
            _context.Setup(ctx => ctx.PatchesGetSummaryForSystemFiltersWithPaging(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockPatchSummary.Object);
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;
            var (patches, recordCount) = await patchRepo.GetSummaryAsync("140", 1, 1, _filterData);

            Assert.NotNull(patches);
            Assert.IsAssignableFrom<IEnumerable<Patch>>(patches);
            Assert.True(patches.Count() > 0, "Result count is 0");
            Assert.IsAssignableFrom<int>(recordCount);
        }

        [Fact]
        public async Task GetSummaryAsyncReturnsAppropriateResultsForSystem()
        {
            var mockPatchSummary = new Mock<MockObjectResult<PatchesSummary_Result>>();

            mockPatchSummary.Setup(ps => ps.GetEnumerator()).Returns(GetPatchSummaries);
            _context.Setup(ctx => ctx.PatchesGetSummaryForSystemFiltersWithPaging(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockPatchSummary.Object);
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;
            var (patches, recordCount) = await patchRepo.GetSummaryAsync("140", 1, 1, _filterData);
            var result = patches.SingleOrDefault(ps => ps.FromTalkgroupID == 9019 && ps.ToTalkgroupID == 9020);

            Assert.NotNull(result);
            Assert.Equal(_testPatchSummaryResult2.FromTalkgroupID, result.FromTalkgroupID);
            Assert.Equal(_testPatchSummaryResult2.FromTalkgroupDescription, result.FromTalkgroupName);
            Assert.Equal(_testPatchSummaryResult2.ToTalkgroupID, result.ToTalkgroupID);
            Assert.Equal(_testPatchSummaryResult2.ToTalkgroupDescription, result.ToTalkgroupName);
            Assert.Equal(_testPatchSummaryResult2.HitCount, result.HitCount);
            Assert.Equal(_testPatchSummaryResult2.FirstSeen, result.FirstSeen);
            Assert.Equal(_testPatchSummaryResult2.LastSeen, result.LastSeen);
        }

        [Fact]
        public async Task GetSummaryAsyncThrowsExceptionWithDatabaseExceptionForSystem()
        {
            _context.Setup(ctx => ctx.PatchesGetSummaryForSystemFiltersWithPaging(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("DatabaseError"));
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => patchRepo.GetSummaryAsync("140", 1, 1, _filterData));
        }

        [Fact]
        public async Task GetSummaryForSystemAsyncReturnsAppropriateResults()
        {
            var mockPatchSummary = new Mock<MockObjectResult<PatchesSummary_Result>>();

            mockPatchSummary.Setup(ps => ps.GetEnumerator()).Returns(GetPatchSummaries);
            _context.Setup(ctx => ctx.PatchesGetSummaryForSystem(It.IsAny<int>())).Returns(mockPatchSummary.Object);
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;
            var result = await patchRepo.GetSummaryForSystemAsync(1);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<Patch>>(result);
            Assert.True(result.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetSummaryForSystemAsyncThrowsExceptionForDatabaseException()
        {
            _context.Setup(ctx => ctx.PatchesGetSummaryForSystem(It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => patchRepo.GetSummaryForSystemAsync(1));
        }

        [Fact]
        public async Task GetSummaryForSystemAsyncWithFiltersReturnsAppropriateDataTypes()
        {
            var mockPatchSummary = new Mock<MockObjectResult<PatchesSummary_Result>>();

            mockPatchSummary.Setup(ps => ps.GetEnumerator()).Returns(GetPatchSummaries);
            _context.Setup(ctx => ctx.PatchesGetSummaryForSystemFiltersWithPaging(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockPatchSummary.Object);
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;
            var (patches, recordCount) = await patchRepo.GetSummaryForSystemAsync("140", _filterData);

            Assert.NotNull(patches);
            Assert.IsAssignableFrom<IEnumerable<Patch>>(patches);
            Assert.True(patches.Count() > 0, "Result count is 0");
            Assert.IsAssignableFrom<int>(recordCount);
        }

        [Fact]
        public async Task GetSummaryForSystemAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.PatchesGetSummaryForSystemFiltersWithPaging(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("DatabaseError"));
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => patchRepo.GetSummaryForSystemAsync("140", _filterData));
        }

        [Fact]
        public async Task GetForPatchByDateAsyncReturnsAppropriateResults()
        {
            var mockPatchesByDate = new Mock<MockObjectResult<PatchesByDate_Result>>();

            mockPatchesByDate.Setup(pbd => pbd.GetEnumerator()).Returns(GetPatchesByDate);
            _context.Setup(ctx => ctx.PatchesGetForPatchByDate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockPatchesByDate.Object);
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;
            var result = await patchRepo.GetForPatchByDateAsync(1, 9000, 9001);
            var resultData = result.SingleOrDefault(rd => rd.FromTalkgroupID == 9000 && rd.ToTalkgroupID == 9001);

            Assert.IsAssignableFrom<IEnumerable<Patch>>(result);
            Assert.True(result.Count() > 0, "Result count is 0");
            Assert.NotNull(resultData);
            Assert.Equal(_testPatchByDateResult1.FromTalkgroupID, resultData.FromTalkgroupID);
            Assert.Equal(_testPatchByDateResult1.FromTalkgroupDescription, resultData.FromTalkgroupName);
            Assert.Equal(_testPatchByDateResult1.ToTalkgroupID, resultData.ToTalkgroupID);
            Assert.Equal(_testPatchByDateResult1.ToTalkgroupDescription, resultData.ToTalkgroupName);
            Assert.Equal(_testPatchByDateResult1.TowerNumber, resultData.TowerNumber);
            Assert.Equal(_testPatchByDateResult1.TowerDescription, resultData.TowerName);
            Assert.Equal(_testPatchByDateResult1.Date, resultData.Date);
            Assert.Equal(_testPatchByDateResult1.HitCount, resultData.HitCount);
        }

        [Fact]
        public async Task GetForPatchByDateAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.PatchesGetForPatchByDate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => patchRepo.GetForPatchByDateAsync(1, 9000, 9001));
        }

        [Fact]
        public async Task GetForPatchByDateAsyncReturnsAppropriateResultsWithPaging()
        {
            var mockPatchesByDate = new Mock<MockObjectResult<PatchesByDate_Result>>();

            mockPatchesByDate.Setup(pbd => pbd.GetEnumerator()).Returns(GetPatchesByDate);
            _context.Setup(ctx => ctx.PatchesGetForPatchByDateWithPaging(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(mockPatchesByDate.Object);
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;
            var result = await patchRepo.GetForPatchByDateAsync(1, 9000, 9001, 1, 15);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<Patch>>(result);
            Assert.True(result.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetForPatchByDateAsyncWithPagingThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.PatchesGetForPatchByDateWithPaging(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new Exception("Database error")); SetupMockRepo();

            var patchRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => patchRepo.GetForPatchByDateAsync(1, 9000, 9001, 1, 15));
        }

        [Fact]
        public async Task GetForPatchByDateCountAsyncReturnsACount()
        {
            var mockPatchByDateCount = new Mock<MockObjectResult<int?>>();

            mockPatchByDateCount.Setup(pdc => pdc.GetEnumerator()).Returns(GetCount);
            _context.Setup(ctx => ctx.PatchesGetForPatchByDateCount(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockPatchByDateCount.Object);
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;
            var result = await patchRepo.GetForPatchByDateCountAsync(1, 9000, 9001);

            Assert.IsAssignableFrom<int>(result);
        }

        [Fact]
        public async Task GetForPatchByDateCountAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.PatchesGetForPatchByDateCount(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => patchRepo.GetForPatchByDateCountAsync(1, 9000, 9001));
        }

        [Fact]
        public async Task GetForSystemFromTalkgroupAsyncReturnsAppropriateTypes()
        {
            var mockPatchResults = new Mock<MockObjectResult<Patches_Result>>();

            mockPatchResults.Setup(pr => pr.GetEnumerator()).Returns(GetPatchResults);
            _context.Setup(ctx => ctx.PatchesGetForSystemFromTalkgroupID(It.IsAny<int>(), It.IsAny<int>())).Returns(mockPatchResults.Object);
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;
            var result = await patchRepo.GetForSystemFromTalkgroupAsync(1, 9000);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<Patch>>(result);
            Assert.True(result.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetForSystemFromTalkgroupAsyncReturnsAppropriateValues()
        {
            var mockPatchResults = new Mock<MockObjectResult<Patches_Result>>();

            mockPatchResults.Setup(pr => pr.GetEnumerator()).Returns(GetPatchResults);
            _context.Setup(ctx => ctx.PatchesGetForSystemFromTalkgroupID(It.IsAny<int>(), It.IsAny<int>())).Returns(mockPatchResults.Object);
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;
            var result = await patchRepo.GetForSystemFromTalkgroupAsync(1, 9019);
            var resultData = result.SingleOrDefault(rd => rd.FromTalkgroupID == 9019 && rd.ToTalkgroupID == 9020);

            Assert.NotNull(resultData);
            Assert.Equal(_testPatchResult2.TowerNumber, resultData.TowerNumber);
            Assert.Equal(_testPatchResult2.TowerDescription, resultData.TowerName);
            Assert.Equal(_testPatchResult2.FromTalkgroupID, resultData.FromTalkgroupID);
            Assert.Equal(_testPatchResult2.FromTalkgroupDescription, resultData.FromTalkgroupName);
            Assert.Equal(_testPatchResult2.ToTalkgroupID, resultData.ToTalkgroupID);
            Assert.Equal(_testPatchResult2.FirstSeen, resultData.FirstSeen);
            Assert.Equal(_testPatchResult2.LastSeen, resultData.LastSeen);
            Assert.Equal(_testPatchResult2.HitCount, resultData.HitCount);
        }

        [Fact]
        public async Task GetForSystemFromTalkgroupAsyncThrowsExceptionForDatabaseException()
        {
            _context.Setup(ctx => ctx.PatchesGetForSystemFromTalkgroupID(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => patchRepo.GetForSystemFromTalkgroupAsync(1, 9000));
        }

        [Fact]
        public async Task GetForSystemToTalkgroupAsyncReturnsAppropriateTypes()
        {
            var mockPatchResults = new Mock<MockObjectResult<Patches_Result>>();

            mockPatchResults.Setup(pr => pr.GetEnumerator()).Returns(GetPatchResults);
            _context.Setup(ctx => ctx.PatchesGetForSystemToTalkgroupID(It.IsAny<int>(), It.IsAny<int>())).Returns(mockPatchResults.Object);
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;
            var result = await patchRepo.GetForSystemToTalkgroupAsync(1, 9001);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<Patch>>(result);
            Assert.True(result.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetForSystemTomTalkgroupAsyncThrowsExceptionForDatabaseException()
        {
            _context.Setup(ctx => ctx.PatchesGetForSystemToTalkgroupID(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => patchRepo.GetForSystemToTalkgroupAsync(1, 9000));
        }

        [Fact]
        public async Task GetForSystemTalkgroupAsyncReturnsAppropriateTypes()
        {
            var mockPatchResults = new Mock<MockObjectResult<Patches_Result>>();

            mockPatchResults.Setup(pr => pr.GetEnumerator()).Returns(GetPatchResults);
            _context.Setup(ctx => ctx.PatchesGetForSystemTalkgroup(It.IsAny<int>(), It.IsAny<int>())).Returns(mockPatchResults.Object);
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;
            var result = await patchRepo.GetForSystemTalkgroupAsync(1, 9000);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<Patch>>(result);
            Assert.True(result.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetForSystemTalkgroupAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.PatchesGetForSystemTalkgroup(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database errpr"));
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => patchRepo.GetForSystemTalkgroupAsync(1, 9000));
        }

        [Fact]
        public async Task GetForSystemTalkgroupCountAsyncReturnsACount()
        {
            var mockPatchByDateCount = new Mock<MockObjectResult<int?>>();

            mockPatchByDateCount.Setup(pdc => pdc.GetEnumerator()).Returns(GetCount);
            _context.Setup(ctx => ctx.PatchesGetForSystemTalkgroupCount(It.IsAny<int>(), It.IsAny<int>())).Returns(mockPatchByDateCount.Object);
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;
            var result = await patchRepo.GetForSystemTalkgroupCountAsync(1, 9000);

            Assert.IsAssignableFrom<int>(result);
        }

        [Fact]
        public async Task GetForSystemTalkgroupCountAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.PatchesGetForSystemTalkgroupCount(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => patchRepo.GetForSystemTalkgroupCountAsync(1, 9000));
        }

        [Fact]
        public async Task GetForSystemTalkgroupAsyncWithPagingReturnsAppropriateTypes()
        {
            var mockPatchSummaryResults = new Mock<MockObjectResult<PatchesSummary_Result>>();

            mockPatchSummaryResults.Setup(pr => pr.GetEnumerator()).Returns(GetPatchSummaries);
            _context.Setup(ctx => ctx.PatchesGetForSystemTalkgroupFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(mockPatchSummaryResults.Object);

            SetupMockRepo();

            var patchRepo = _mockRepo.Object;
            var (patches, recordCount) = await patchRepo.GetForSystemTalkgroupAsync("140", 9000, _filterData);

        }

        [Fact]
        public async Task GetForSystemTalkgroupAsyncWithPagingThrowsExceptionForDatabaseException()
        {
            _context.Setup(ctx => ctx.PatchesGetForSystemTalkgroupFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new Exception("Database error"));
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => patchRepo.GetForSystemTalkgroupAsync("140", 9000, _filterData));
        }

        [Fact]
        public async Task GetForSystemTowerAsyncReturnsAppropriateTypes()
        {
            var mockPatchResults = new Mock<MockObjectResult<Patches_Result>>();

            mockPatchResults.Setup(pr => pr.GetEnumerator()).Returns(GetPatchResults);
            _context.Setup(ctx => ctx.PatchesGetForSystemTower(It.IsAny<int>(), It.IsAny<int>())).Returns(mockPatchResults.Object);
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;
            var results = await patchRepo.GetForSystemTowerAsync(1, 1031);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<Patch>>(results);
            Assert.True(results.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetForSystemTowerAsyncThrowsExceptionForDatabaseException()
        {
            _context.Setup(ctx => ctx.PatchesGetForSystemTower(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => patchRepo.GetForSystemTowerAsync(1, 1031));
        }

        [Fact]
        public async Task GetForSystemTowerAsyncWithPagingReturnsAppropriateTypes()
        {
            var mockPatchSummaryResults = new Mock<MockObjectResult<PatchesSummary_Result>>();

            mockPatchSummaryResults.Setup(psr => psr.GetEnumerator()).Returns(GetPatchSummaries);
            _context.Setup(ctx => ctx.PatchesGetForSystemTowerFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockPatchSummaryResults.Object);
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;
            var (patches, recordCount) = await patchRepo.GetForSystemTowerAsync("140", 1031, _filterData);

            Assert.IsAssignableFrom<IEnumerable<Patch>>(patches);
            Assert.True(patches.Count() > 0, "Result count is 0");
            Assert.IsAssignableFrom<int>(recordCount);
        }

        [Fact]
        public async Task GetForSystemTowerAsyncWithPagingThrowsExceptionForDatabaseException()
        {
            _context.Setup(ctx => ctx.PatchesGetForSystemTowerFiltersWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => patchRepo.GetForSystemTowerAsync("140", 1031, _filterData));
        }

        [Fact]
        public async Task GetCountForSystemAsyncReturnsAValue()
        {
            var mockPatchByDateCount = new Mock<MockObjectResult<int?>>();

            mockPatchByDateCount.Setup(pdc => pdc.GetEnumerator()).Returns(GetCount);
            _context.Setup(ctx => ctx.PatchesGetCountForSystem(It.IsAny<int>())).Returns(mockPatchByDateCount.Object);
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;
            var result = await patchRepo.GetCountForSystemAsync(1);

            Assert.IsAssignableFrom<int>(result);
        }

        [Fact]
        public async Task GetCountForSystemAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.PatchesGetCountForSystem(It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => patchRepo.GetCountForSystemAsync(1));
        }

        [Fact]
        public async Task GetCountForSystemAsyncSearchReturnsAValue()
        {
            var mockPatchByDateCount = new Mock<MockObjectResult<int?>>();

            mockPatchByDateCount.Setup(pdc => pdc.GetEnumerator()).Returns(GetCount);
            _context.Setup(ctx => ctx.PatchesGetCountForSystemSearch(It.IsAny<int>(), It.IsAny<string>())).Returns(mockPatchByDateCount.Object);
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;
            var result = await patchRepo.GetCountForSystemAsync(1, "LaSalle");

            Assert.IsAssignableFrom<int>(result);
        }

        [Fact]
        public async Task GetCountForSystemAsyncSearchThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.PatchesGetCountForSystemSearch(It.IsAny<int>(), It.IsAny<string>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var patchRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => patchRepo.GetCountForSystemAsync(1, "LaSalle"));
        }
    }
}
