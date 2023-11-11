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
    public class TalkgroupRepositoryTests : RepositoryTestBase<TalkgroupRepository>
    {
        private readonly Talkgroups _testTalkgroup1 = new Talkgroups
        {
            ID = 1,
            SystemID = 1,
            TalkgroupID  = 25086,
            Priority = 50,
            Description = "Cook County SO Patrol North",
            LastSeen = DateTime.Parse("07-15-2018 17:21"),
            LastSeenProgram = DateTime.Parse("07-14-2018 09:31"),
            LastSeenProgramUnix = 1531578696,
            FirstSeen = DateTime.Parse("05-01-2011 12:12"),
            FirstSeenProgram = DateTime.Parse("05-14-2011 08:18"),
            FirstSeenProgramUnix = 1305379113,
            FGColor = "#FFFFFF",
            BGColor = "#000000",
            EncryptionSeen = true,
            IgnoreEmergencySignal = false,
            HitCount = 30193,
            HitCountProgram = 1610,
            PhaseIISeen = true,
            LastModified = DateTime.Now
        };

        private readonly Talkgroups _testTalkgroup2 = new Talkgroups
        {
            ID = 2,
            SystemID = 1,
            TalkgroupID = 25087,
            Priority = 50,
            Description = "Cook County SO Patrol South",
            LastSeen = DateTime.Parse("07-15-2018 11:21"),
            LastSeenProgram = DateTime.Parse("07-14-2018 09:35"),
            LastSeenProgramUnix = 1531578915,
            FirstSeen = DateTime.Parse("05-03-2011 11:10"),
            FirstSeenProgram = DateTime.Parse("05-14-2011 08:19"),
            FirstSeenProgramUnix = 1305379142,
            FGColor = "#FFFFFF",
            BGColor = "#000000",
            EncryptionSeen = true,
            IgnoreEmergencySignal = false,
            HitCount = 51193,
            HitCountProgram = 1714,
            PhaseIISeen = true,
            LastModified = DateTime.Now
        };

        private readonly TalkgroupDetails_Result _testTalkgroupDetailsResult1 = new TalkgroupDetails_Result
        {
            ID = 1,
            TalkgroupID = 25086,
            Description = "Cook County SO Patrol North",
            FirstSeen = DateTime.Parse("05-01-2011 12:12"),
            LastSeen = DateTime.Parse("07-15-2018 17:21"),
            EncryptionSeen = true,
            AffiliationCount = 1000,
            DeniedCount = 1500,
            VoiceGrantCount = 2000,
            EmergencyVoiceGrantCount = 2500,
            EncryptedVoiceGrantCount = 3000,
            PhaseIISeen = true,
            PatchCount = 3500,
            RecordCount = 2
        };

        private readonly TalkgroupDetails_Result _testTalkgroupDetailsResult2 = new TalkgroupDetails_Result
        {
            ID = 2,
            TalkgroupID = 25087,
            Description = "Cook County SO Patrol South",
            FirstSeen = DateTime.Parse("05-03-2011 11:10"),
            LastSeen = DateTime.Parse("07-15-2018 11:21"),
            EncryptionSeen = true,
            AffiliationCount = 10000,
            DeniedCount = 15000,
            VoiceGrantCount = 20000,
            EmergencyVoiceGrantCount = 25000,
            EncryptedVoiceGrantCount = 30000,
            PhaseIISeen = true,
            PatchCount = 35000,
            RecordCount = 2
        };

        private readonly TalkgroupDetailsWithTowers_Result _testTalkgroupDetailsWithTowersResult1 = new TalkgroupDetailsWithTowers_Result
        {
            TalkgroupID = 25086,
            Description = "Cook County SO Patrol North",
            FirstSeen = DateTime.Parse("05-01-2011 12:12"),
            LastSeen = DateTime.Parse("07-15-2018 17:21"),
            EncryptionSeen = true,
            AffiliationCount = 1000,
            DeniedCount = 1500,
            VoiceGrantCount = 2000,
            EmergencyVoiceGrantCount = 2500,
            EncryptedVoiceGrantCount = 3000,
            PhaseIISeen = true,
            PatchCount = 3500,
            Towers = "1001, 1002, 1031"
        };

        private readonly TalkgroupDetailsWithTowers_Result _testTalkgroupDetailsWithTowersResult2 = new TalkgroupDetailsWithTowers_Result
        {
            TalkgroupID = 25087,
            Description = "Cook County SO Patrol South",
            FirstSeen = DateTime.Parse("05-03-2011 11:10"),
            LastSeen = DateTime.Parse("07-15-2018 11:21"),
            EncryptionSeen = true,
            AffiliationCount = 10000,
            DeniedCount = 15000,
            VoiceGrantCount = 20000,
            EmergencyVoiceGrantCount = 25000,
            EncryptedVoiceGrantCount = 30000,
            PhaseIISeen = true,
            PatchCount = 35000,
            Towers = "1002, 1034, 1049"
        };

        private readonly int? _testTalkgroupCount = 1500;

        private IEnumerator<Talkgroups> GetTalkgroups()
        {
            yield return _testTalkgroup1;
            yield return _testTalkgroup2;
        }

        private IEnumerator<TalkgroupDetails_Result> GetTalkgroupDetailResult()
        {
            yield return _testTalkgroupDetailsResult1;
        }

        private IEnumerator<TalkgroupDetails_Result> GetTalkgroupDetailResults()
        {
            yield return _testTalkgroupDetailsResult1;
            yield return _testTalkgroupDetailsResult2;
        }

        private IEnumerator<TalkgroupDetailsWithTowers_Result> GetTalkgroupDetailsWithTowersResults()
        {
            yield return _testTalkgroupDetailsWithTowersResult1;
            yield return _testTalkgroupDetailsWithTowersResult2;
        }

        private IEnumerator<int?> GetTalkgroupCount()
        {
            yield return _testTalkgroupCount;
        }

        [Fact]
        public async Task GetForSystemAsyncReturnsAppropriateTypes()
        {
            var mockTalkgroups = new Mock<MockObjectResult<Talkgroups>>();

            mockTalkgroups.Setup(tg => tg.GetEnumerator()).Returns(GetTalkgroups());
            _context.Setup(ctx => ctx.TalkgroupsGetForSystem(It.IsAny<int>())).Returns(mockTalkgroups.Object);
            SetupMockRepo();

            var talkgroupRepo = _mockRepo.Object;

            var results = await talkgroupRepo.GetForSystemAsync(1);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<Talkgroup>>(results);
            Assert.True(results.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetForSystemAsyncReturnsAppropriateValues()
        {
            var mockTalkgroups = new Mock<MockObjectResult<Talkgroups>>();

            mockTalkgroups.Setup(tg => tg.GetEnumerator()).Returns(GetTalkgroups());
            _context.Setup(ctx => ctx.TalkgroupsGetForSystem(It.IsAny<int>())).Returns(mockTalkgroups.Object);
            SetupMockRepo();

            var talkgroupRepo = _mockRepo.Object;

            var results = await talkgroupRepo.GetForSystemAsync(1);
            var resultData = results.SingleOrDefault(rd => rd.ID == 2);

            Assert.NotNull(resultData);
            Assert.Equal(_testTalkgroup2.ID, resultData.ID);
            Assert.Equal(_testTalkgroup2.SystemID, resultData.SystemID);
            Assert.Equal(_testTalkgroup2.TalkgroupID, resultData.TalkgroupID);
            Assert.Equal(_testTalkgroup2.Priority, resultData.Priority);
            Assert.Equal(_testTalkgroup2.Description, resultData.Description);
            Assert.Equal(_testTalkgroup2.LastSeen, resultData.LastSeen);
            Assert.Equal(_testTalkgroup2.LastSeenProgram, resultData.LastSeenProgram);
            Assert.Equal(_testTalkgroup2.LastSeenProgramUnix, resultData.LastSeenProgramUnix);
            Assert.Equal(_testTalkgroup2.FirstSeen, resultData.FirstSeen);
            Assert.Equal(_testTalkgroup2.FirstSeenProgram, resultData.FirstSeenProgram);
            Assert.Equal(_testTalkgroup2.FirstSeenProgramUnix, resultData.FirstSeenProgramUnix);
            Assert.Equal(_testTalkgroup2.FGColor, resultData.FGColor);
            Assert.Equal(_testTalkgroup2.BGColor, resultData.BGColor);
            Assert.Equal(_testTalkgroup2.EncryptionSeen, resultData.EncryptionSeen);
            Assert.Equal(_testTalkgroup2.IgnoreEmergencySignal, resultData.IgnoreEmergencySignal);
            Assert.Equal(_testTalkgroup2.HitCount, resultData.HitCount);
            Assert.Equal(_testTalkgroup2.HitCountProgram, resultData.HitCountProgram);
            Assert.Equal(_testTalkgroup2.PhaseIISeen, resultData.PhaseIISeen);
            Assert.Equal(_testTalkgroup2.LastModified, resultData.LastModified);
        }

        [Fact]
        public async Task GetForSystemAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TalkgroupsGetForSystem(It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var talkgroupRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => talkgroupRepo.GetForSystemAsync(1));
        }

        [Fact]
        public async Task GetDetailAsyncReturnsAppropriateTypes()
        {
            var mockTalkgroupDetailResults = new Mock<MockObjectResult<TalkgroupDetails_Result>>();

            mockTalkgroupDetailResults.Setup(tgd => tgd.GetEnumerator()).Returns(GetTalkgroupDetailResult());
            _context.Setup(ctx => ctx.TalkgroupsGetDetail(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTalkgroupDetailResults.Object);
            SetupMockRepo();

            var talkgroupRepo = _mockRepo.Object;

            var results = await talkgroupRepo.GetDetailAsync(1, 25086);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<Talkgroup>(results);
        }

        [Fact]
        public async Task GetDetailAsyncReturnsAppropriateValues()
        {
            var mockTalkgroupDetailResult = new Mock<MockObjectResult<TalkgroupDetails_Result>>();

            mockTalkgroupDetailResult.Setup(tgd => tgd.GetEnumerator()).Returns(GetTalkgroupDetailResult());
            _context.Setup(ctx => ctx.TalkgroupsGetDetail(It.IsAny<int>(), It.IsAny<int>())).Returns(mockTalkgroupDetailResult.Object);
            SetupMockRepo();

            var talkgroupRepo = _mockRepo.Object;

            var result = await talkgroupRepo.GetDetailAsync(1, 25086);

            Assert.Equal(_testTalkgroupDetailsResult1.TalkgroupID, result.TalkgroupID);
            Assert.Equal(_testTalkgroupDetailsResult1.Description, result.Description);
            Assert.Equal(_testTalkgroupDetailsResult1.FirstSeen, result.FirstSeen);
            Assert.Equal(_testTalkgroupDetailsResult1.LastSeen, result.LastSeen);
            Assert.Equal(_testTalkgroupDetailsResult1.EncryptionSeen, result.EncryptionSeen);
            Assert.Equal(_testTalkgroupDetailsResult1.AffiliationCount, result.AffiliationCount);
            Assert.Equal(_testTalkgroupDetailsResult1.DeniedCount, result.DeniedCount);
            Assert.Equal(_testTalkgroupDetailsResult1.VoiceGrantCount, result.VoiceGrantCount);
            Assert.Equal(_testTalkgroupDetailsResult1.EmergencyVoiceGrantCount, result.EmergencyVoiceGrantCount);
            Assert.Equal(_testTalkgroupDetailsResult1.EncryptedVoiceGrantCount, result.EncryptedVoiceGrantCount);
            Assert.Equal(_testTalkgroupDetailsResult1.PhaseIISeen, result.PhaseIISeen);
            Assert.Equal(_testTalkgroupDetailsResult1.PatchCount, result.PatchCount);
        }

        [Fact]
        public async Task GetDetailAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TalkgroupsGetDetail(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var talkgroupRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => talkgroupRepo.GetDetailAsync(1, 25086));
        }

        [Fact]
        public async Task GetDetailAsyncFiltersReturnsAppropriateTypes()
        {
            var mockTalkgroupDetailResults = new Mock<MockObjectResult<TalkgroupDetails_Result>>();

            mockTalkgroupDetailResults.Setup(tgd => tgd.GetEnumerator()).Returns(GetTalkgroupDetailResult());
            _context.Setup(ctx => ctx.TalkgroupsGetDetailFilters(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(mockTalkgroupDetailResults.Object);
            SetupMockRepo();

            var talkgroupRepo = _mockRepo.Object;

            var results = await talkgroupRepo.GetDetailAsync(1, 25087, DateTime.Now.AddYears(-2), DateTime.Now);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<Talkgroup>(results);
        }

        [Fact]
        public async Task GetDetailAsyncFiltersThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TalkgroupsGetDetailFilters(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Throws(new Exception("Database error"));
            SetupMockRepo();

            var talkgroupRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => talkgroupRepo.GetDetailAsync(1, 25087, DateTime.Now.AddYears(-2), DateTime.Now));
        }

        [Fact]
        public async Task GetDetailForSystemAsyncReturnsAppropriateTypes()
        {
            var mockTalkgroupDetailResults = new Mock<MockObjectResult<TalkgroupDetails_Result>>();

            mockTalkgroupDetailResults.Setup(tgd => tgd.GetEnumerator()).Returns(GetTalkgroupDetailResults);
            _context.Setup(ctx => ctx.TalkgroupsGetDetailForSystem(It.IsAny<int>())).Returns(mockTalkgroupDetailResults.Object);
            SetupMockRepo();

            var talkgroupRepo = _mockRepo.Object;

            var results = await talkgroupRepo.GetDetailForSystemAsync(1);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<Talkgroup>>(results);
            Assert.True(results.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetDetailForSystemAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TalkgroupsGetDetailForSystem(It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var talkgroupRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => talkgroupRepo.GetDetailForSystemAsync(1));
        }

        [Fact]
        public async Task GetDetailForSystemAsyncActiveFiltersReturnsAppropriateTypes()
        {
            var mockTalkgroupDetailResults = new Mock<MockObjectResult<TalkgroupDetails_Result>>();

            mockTalkgroupDetailResults.Setup(tgr => tgr.GetEnumerator()).Returns(GetTalkgroupDetailResults);
            _context.Setup(ctx => ctx.TalkgroupsGetDetailForSystemActiveFiltersWithPaging(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(mockTalkgroupDetailResults.Object);
            SetupMockRepo();

            var talkgroupRepo = _mockRepo.Object;

            var (talkgroups, recordCount) = await talkgroupRepo.GetDetailForSystemAsync("140", true, _filterData);

            Assert.NotNull(talkgroups);
            Assert.IsAssignableFrom<IEnumerable<Talkgroup>>(talkgroups);
            Assert.True(talkgroups.Count() > 0, "Result count is 0");
            Assert.IsAssignableFrom<int>(recordCount);
            Assert.Equal(_testTalkgroupDetailsResult1.RecordCount, recordCount);
        }

        [Fact]
        public async Task GetDetailForSystemAsyncAllFiltersReturnsAppropriateTypes()
        {
            var mockTalkgroupDetailResults = new Mock<MockObjectResult<TalkgroupDetails_Result>>();

            mockTalkgroupDetailResults.Setup(tgr => tgr.GetEnumerator()).Returns(GetTalkgroupDetailResults);
            _context.Setup(ctx => ctx.TalkgroupsGetDetailForSystemFiltersWithPaging(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(mockTalkgroupDetailResults.Object);
            SetupMockRepo();

            var talkgroupRepo = _mockRepo.Object;

            var (talkgroups, recordCount) = await talkgroupRepo.GetDetailForSystemAsync("140", false, _filterData);

            Assert.NotNull(talkgroups);
            Assert.IsAssignableFrom<IEnumerable<Talkgroup>>(talkgroups);
            Assert.True(talkgroups.Count() > 0, "Result count is 0");
            Assert.IsAssignableFrom<int>(recordCount);
            Assert.Equal(_testTalkgroupDetailsResult1.RecordCount, recordCount);
        }

        [Fact]
        public async Task GetDetailForSystemAsyncFiltersActiveThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TalkgroupsGetDetailForSystemActiveFiltersWithPaging(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new Exception("Database error"));
            SetupMockRepo();

            var talkgroupRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => talkgroupRepo.GetDetailForSystemAsync("140", true, _filterData));
        }

        [Fact]
        public async Task GetDetailForSystemAsyncFiltersAllThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TalkgroupsGetDetailForSystemFiltersWithPaging(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new Exception("Database error"));
            SetupMockRepo();

            var talkgroupRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => talkgroupRepo.GetDetailForSystemAsync("140", false, _filterData));
        }

        [Fact]
        public async Task GetDetailForSystemUnknownAsyncReturnsAppropriateTypes()
        {
            var mockTalkgroupDetailWithTowersResults = new Mock<MockObjectResult<TalkgroupDetailsWithTowers_Result>>();

            mockTalkgroupDetailWithTowersResults.Setup(tgdt => tgdt.GetEnumerator()).Returns(GetTalkgroupDetailsWithTowersResults);
            _context.Setup(ctx => ctx.TalkgroupsGetDetailForSystemUnknownFiltersWithPaging(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockTalkgroupDetailWithTowersResults.Object);
            SetupMockRepo();

            var talkgroupRepo = _mockRepo.Object;

            var (talkgroups, recordCount) = await talkgroupRepo.GetDetailForSystemUnknownAsync("140", _filterData);

            Assert.NotNull(talkgroups);
            Assert.IsAssignableFrom<IEnumerable<Talkgroup>>(talkgroups);
            Assert.True(talkgroups.Count() > 0, "Result count is 0");
            Assert.IsAssignableFrom<int>(recordCount);
        }

        [Fact]
        public async Task GetDetailForSystemUnknownAsyncReturnsAppropriateValues()
        {
            var mockTalkgroupDetailWithTowersResults = new Mock<MockObjectResult<TalkgroupDetailsWithTowers_Result>>();

            mockTalkgroupDetailWithTowersResults.Setup(tgdt => tgdt.GetEnumerator()).Returns(GetTalkgroupDetailsWithTowersResults);
            _context.Setup(ctx => ctx.TalkgroupsGetDetailForSystemUnknownFiltersWithPaging(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockTalkgroupDetailWithTowersResults.Object);
            SetupMockRepo();

            var talkgroupRepo = _mockRepo.Object;

            var (talkgroups, recordCount) = await talkgroupRepo.GetDetailForSystemUnknownAsync("140", _filterData);
            var resultData = talkgroups.SingleOrDefault(rd => rd.TalkgroupID == 25087);

            Assert.NotNull(resultData);
            Assert.Equal(_testTalkgroupDetailsWithTowersResult2.TalkgroupID, resultData.TalkgroupID);
            Assert.Equal(_testTalkgroupDetailsWithTowersResult2.Description, resultData.Description);
            Assert.Equal(_testTalkgroupDetailsWithTowersResult2.FirstSeen, resultData.FirstSeen);
            Assert.Equal(_testTalkgroupDetailsWithTowersResult2.LastSeen, resultData.LastSeen);
            Assert.Equal(_testTalkgroupDetailsWithTowersResult2.EncryptionSeen, resultData.EncryptionSeen);
            Assert.Equal(_testTalkgroupDetailsWithTowersResult2.AffiliationCount, resultData.AffiliationCount);
            Assert.Equal(_testTalkgroupDetailsWithTowersResult2.DeniedCount, resultData.DeniedCount);
            Assert.Equal(_testTalkgroupDetailsWithTowersResult2.VoiceGrantCount, resultData.VoiceGrantCount);
            Assert.Equal(_testTalkgroupDetailsWithTowersResult2.EmergencyVoiceGrantCount, resultData.EmergencyVoiceGrantCount);
            Assert.Equal(_testTalkgroupDetailsWithTowersResult2.EncryptedVoiceGrantCount, resultData.EncryptedVoiceGrantCount);
            Assert.Equal(_testTalkgroupDetailsWithTowersResult2.PhaseIISeen, resultData.PhaseIISeen);
            Assert.Equal(_testTalkgroupDetailsWithTowersResult2.PatchCount, resultData.PatchCount);
            Assert.Equal(_testTalkgroupDetailsWithTowersResult2.Towers, resultData.Towers);
        }

        [Fact]
        public async Task GetDetailForSystemUnknownAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TalkgroupsGetDetailForSystemUnknownFiltersWithPaging(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var talkgroupRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => talkgroupRepo.GetDetailForSystemUnknownAsync("140", _filterData));
        }

        [Fact]
        public async Task GetCountForSystemAsyncReturnsACount()
        {
            var mockTalkgroupCount = new Mock<MockObjectResult<int?>>();

            mockTalkgroupCount.Setup(tgc => tgc.GetEnumerator()).Returns(GetTalkgroupCount);
            _context.Setup(ctx => ctx.TalkgroupsGetCountForSystem(It.IsAny<int>())).Returns(mockTalkgroupCount.Object);
            SetupMockRepo();

            var talkgroupRepo = _mockRepo.Object;

            var result = await talkgroupRepo.GetCountForSystemAsync(1);

            Assert.Equal(_testTalkgroupCount, result);
        }

        [Fact]
        public async Task GetCountForSystemAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TalkgroupsGetCountForSystem(It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var talkgroupRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => talkgroupRepo.GetCountForSystemAsync(1));
        }
    }
}
