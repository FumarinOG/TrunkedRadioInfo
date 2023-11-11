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
    public class RadioRepositoryTests : RepositoryTestBase<RadioRepository>
    {
        private readonly Radios _testRadio1 = new Radios
        {
            ID = 1,
            SystemID = 140,
            RadioID = 1901001,
            Description = "ISP Dispatch (Sterling) (01-A)",
            LastSeen = DateTime.Now,
            LastSeenProgram = DateTime.Now,
            LastSeenProgramUnix = (long)(DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds,
            FirstSeen = DateTime.Now.AddYears(-1),
            FGColor = "#000000",
            BGColor = "#FFFFFF",
            PhaseIISeen = false,
            HitCount = 150000,
            LastModified = DateTime.Now
        };

        private readonly Radios _testRadio2 = new Radios
        {
            ID = 2,
            SystemID = 140,
            RadioID = 1917001,
            Description = "ISP Dispatch (LaSalle) (17-A)",
            LastSeen = DateTime.Now,
            LastSeenProgram = DateTime.Now,
            LastSeenProgramUnix = (long)(DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds,
            FirstSeen = DateTime.Now.AddYears(-1),
            FGColor = "#000000",
            BGColor = "#FFFFFF",
            PhaseIISeen = false,
            HitCount = 150000,
            LastModified = DateTime.Now
        };

        private readonly RadioDetails_Result _testRadioDetailsResult1 = new RadioDetails_Result
        {
            ID = 1,
            RadioID = 413487,
            Description = "Normal Engine 17",
            FirstSeen = DateTime.Parse("05-31-2011 04:31"),
            LastSeen = DateTime.Parse("05-14-2018 08:04"),
            AffiliationCount = 73,
            DeniedCount = 4,
            VoiceGrantCount = 185,
            EmergencyVoiceGrantCount = 3,
            EncryptedVoiceGrantCount = 0,
            DataCount = 110,
            PhaseIISeen = false,
            RecordCount = 2
        };

        private readonly RadioDetails_Result _testRadioDetailsResult2 = new RadioDetails_Result
        {
            ID = 2,
            RadioID = 413482,
            Description = "Normal Medic 3N-68",
            FirstSeen = DateTime.Parse("05-15-2008 00:51"),
            LastSeen = DateTime.Parse("05-14-2018 09:31"),
            AffiliationCount = 108,
            DeniedCount = 2,
            VoiceGrantCount = 399,
            EmergencyVoiceGrantCount = 5,
            EncryptedVoiceGrantCount = 1,
            DataCount = 120,
            PhaseIISeen = true,
            RecordCount = 2
        };

        private readonly RadiosNames_Result _testRadioNamesResult1 = new RadiosNames_Result
        {
            RadioID = 1917001,
            Description = "ISP Dispatch (LaSalle) (17-A)",
            RecordCount = 2
        };

        private readonly RadiosNames_Result _testRadioNamesResult2 = new RadiosNames_Result
        {
            RadioID = 1917002,
            Description = "ISP Dispatch (LaSalle) (17-B)",
            RecordCount = 2
        };

        private readonly int? _testRadioCount = 43001;

        private IEnumerator<Radios> GetTestRadios()
        {
            yield return _testRadio1;
            yield return _testRadio2;
        }

        private IEnumerator<RadioDetails_Result> GetTestRadioDetailsResult()
        {
            yield return _testRadioDetailsResult1;
        }

        private IEnumerator<RadioDetails_Result> GetTestRadioDetailsResults()
        {
            yield return _testRadioDetailsResult1;
            yield return _testRadioDetailsResult2;
        }

        private IEnumerator<RadiosNames_Result> GetTestRadioNamesResults()
        {
            yield return _testRadioNamesResult1;
            yield return _testRadioNamesResult2;
        }

        private IEnumerator<int?> GetTestRadioCount()
        {
            yield return _testRadioCount;
        }

        [Fact]
        public async Task GetListForSystemAsyncReturnsAppropriateTypes()
        {
            var mockRadios = new Mock<MockObjectResult<Radios>>();

            mockRadios.Setup(r => r.GetEnumerator()).Returns(GetTestRadios);
            _context.Setup(ctx => ctx.RadiosGetForSystem(It.IsAny<int>())).Returns(mockRadios.Object);
            SetupMockRepo();

            var radioRepo = _mockRepo.Object;

            var results = await radioRepo.GetListForSystemAsync(1);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<Radio>>(results);
            Assert.True(results.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetListForSystemAsyncReturnsAppropriateValues()
        {
            var mockRadios = new Mock<MockObjectResult<Radios>>();

            mockRadios.Setup(r => r.GetEnumerator()).Returns(GetTestRadios);
            _context.Setup(ctx => ctx.RadiosGetForSystem(It.IsAny<int>())).Returns(mockRadios.Object);
            SetupMockRepo();

            var radioRepo = _mockRepo.Object;

            var results = await radioRepo.GetListForSystemAsync(1);
            var resultsData = results.SingleOrDefault(rd => rd.ID == 1);

            Assert.NotNull(resultsData);
            Assert.IsAssignableFrom<Radio>(resultsData);
            Assert.Equal(_testRadio1.ID, resultsData.ID);
            Assert.Equal(_testRadio1.SystemID, resultsData.SystemID);
            Assert.Equal(_testRadio1.RadioID, resultsData.RadioID);
            Assert.Equal(_testRadio1.Description, resultsData.Description);
            Assert.Equal(_testRadio1.LastSeen, resultsData.LastSeen);
            Assert.Equal(_testRadio1.LastSeenProgram, resultsData.LastSeenProgram);
            Assert.Equal(_testRadio1.LastSeenProgramUnix, resultsData.LastSeenProgramUnix);
            Assert.Equal(_testRadio1.FirstSeen, resultsData.FirstSeen);
            Assert.Equal(_testRadio1.FGColor, resultsData.FGColor);
            Assert.Equal(_testRadio1.BGColor, resultsData.BGColor);
            Assert.Equal(_testRadio1.PhaseIISeen, resultsData.PhaseIISeen);
            Assert.Equal(_testRadio1.HitCount, resultsData.HitCount);
            Assert.Equal(_testRadio1.LastModified, resultsData.LastModified);
        }

        [Fact]
        public async Task GetListForSystemAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.RadiosGetForSystem(It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var radioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => radioRepo.GetListForSystemAsync(1));
        }

        [Fact]
        public async Task GetDetailForSystemAsyncReturnsAppropriateTypes()
        {
            var mockRadioDetailsResult = new Mock<MockObjectResult<RadioDetails_Result>>();

            mockRadioDetailsResult.Setup(rdr => rdr.GetEnumerator()).Returns(GetTestRadioDetailsResults);
            _context.Setup(ctx => ctx.RadiosGetDetailForSystem(It.IsAny<int>())).Returns(mockRadioDetailsResult.Object);
            SetupMockRepo();

            var radioRepo = _mockRepo.Object;

            var results = await radioRepo.GetDetailForSystemAsync(1);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<Radio>>(results);
            Assert.True(results.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetDetailForSystemAsyncReturnsAppropriateValues()
        {
            var mockRadioDetailsResult = new Mock<MockObjectResult<RadioDetails_Result>>();

            mockRadioDetailsResult.Setup(rdr => rdr.GetEnumerator()).Returns(GetTestRadioDetailsResults);
            _context.Setup(ctx => ctx.RadiosGetDetailForSystem(It.IsAny<int>())).Returns(mockRadioDetailsResult.Object);
            SetupMockRepo();

            var radioRepo = _mockRepo.Object;

            var results = await radioRepo.GetDetailForSystemAsync(1);
            var resultsData = results.SingleOrDefault(rd => rd.RadioID == 413482);

            Assert.NotNull(resultsData);
            Assert.IsAssignableFrom<Radio>(resultsData);
            Assert.Equal(_testRadioDetailsResult2.RadioID, resultsData.RadioID);
            Assert.Equal(_testRadioDetailsResult2.Description, resultsData.Description);
            Assert.Equal(_testRadioDetailsResult2.FirstSeen, resultsData.FirstSeen);
            Assert.Equal(_testRadioDetailsResult2.LastSeen, resultsData.LastSeen);
            Assert.Equal(_testRadioDetailsResult2.AffiliationCount, resultsData.AffiliationCount);
            Assert.Equal(_testRadioDetailsResult2.DeniedCount, resultsData.DeniedCount);
            Assert.Equal(_testRadioDetailsResult2.VoiceGrantCount, resultsData.VoiceGrantCount);
            Assert.Equal(_testRadioDetailsResult2.EmergencyVoiceGrantCount, resultsData.EmergencyVoiceGrantCount);
            Assert.Equal(_testRadioDetailsResult2.EncryptedVoiceGrantCount, resultsData.EncryptedVoiceGrantCount);
            Assert.Equal(_testRadioDetailsResult2.DataCount, resultsData.DataCount);
            Assert.Equal(_testRadioDetailsResult2.PhaseIISeen, resultsData.PhaseIISeen);
        }

        [Fact]
        public async Task GetDetailForSystemAsyncThrowsExceptionForDatabaseException()
        {
            _context.Setup(ctx => ctx.RadiosGetDetailForSystem(It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var radioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => radioRepo.GetDetailForSystemAsync(1));
        }

        [Fact]
        public async Task GetDetailsAsyncReturnsAppropriateTypes()
        {
            var mockRadioDetailsResult = new Mock<MockObjectResult<RadioDetails_Result>>();

            mockRadioDetailsResult.Setup(rdr => rdr.GetEnumerator()).Returns(GetTestRadioDetailsResult);
            _context.Setup(ctx => ctx.RadiosGetDetail(It.IsAny<int>(), It.IsAny<int>())).Returns(mockRadioDetailsResult.Object);
            SetupMockRepo();

            var radioRepo = _mockRepo.Object;

            var results = await radioRepo.GetDetailAsync(1, 413487);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<Radio>(results);
        }

        [Fact]
        public async Task GetDetailsAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.RadiosGetDetail(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var radioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => radioRepo.GetDetailAsync(1, 413487));
        }

        [Fact]
        public async Task GetDetailsAsyncFiltersReturnsAppropriateTypes()
        {
            var mockRadioDetailsResult = new Mock<MockObjectResult<RadioDetails_Result>>();

            mockRadioDetailsResult.Setup(rdr => rdr.GetEnumerator()).Returns(GetTestRadioDetailsResult);
            _context.Setup(ctx => ctx.RadiosGetDetailFilters(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(mockRadioDetailsResult.Object);
            SetupMockRepo();

            var radioRepo = _mockRepo.Object;

            var results = await radioRepo.GetDetailAsync(1, 413487, DateTime.Now.AddYears(-1), DateTime.Now);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<Radio>(results);
        }

        [Fact]
        public async Task GetDetailsAsyncFiltersThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.RadiosGetDetailFilters(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Throws(new Exception("Database error"));
            SetupMockRepo();

            var radioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => radioRepo.GetDetailAsync(1, 413487, DateTime.Now.AddYears(-1), DateTime.Now));
        }

        [Fact]
        public async Task GetDetailForSystemAsyncActiveReturnsAppropriateTypes()
        {
            var mockRadioDetailtsResults = new Mock<MockObjectResult<RadioDetails_Result>>();

            mockRadioDetailtsResults.Setup(rdr => rdr.GetEnumerator()).Returns(GetTestRadioDetailsResults);
            _context.Setup(ctx => ctx.RadiosGetDetailForSystemActiveWithPaging(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(),
                It.IsAny<int>())).Returns(mockRadioDetailtsResults.Object);
            SetupMockRepo();

            var radioRepo = _mockRepo.Object;

            var (radios, recordCount) = await radioRepo.GetDetailForSystemAsync("140", true, "RadioID", "Ascending", 1, 15);

            Assert.NotNull(radios);
            Assert.IsAssignableFrom<IEnumerable<Radio>>(radios);
            Assert.True(radios.Count() > 0, "Result count is 0");
            Assert.IsAssignableFrom<int>(recordCount);
        }

        [Fact]
        public async Task GetDetailForSystemAsyncAllReturnsAppropriateTypes()
        {
            var mockRadioDetailtsResults = new Mock<MockObjectResult<RadioDetails_Result>>();

            mockRadioDetailtsResults.Setup(rdr => rdr.GetEnumerator()).Returns(GetTestRadioDetailsResults);
            _context.Setup(ctx => ctx.RadiosGetDetailForSystemWithPaging(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(),
                It.IsAny<int>())).Returns(mockRadioDetailtsResults.Object);
            SetupMockRepo();

            var radioRepo = _mockRepo.Object;

            var (radios, recordCount) = await radioRepo.GetDetailForSystemAsync("140", false, "RadioID", "Ascending", 1, 15);

            Assert.NotNull(radios);
            Assert.IsAssignableFrom<IEnumerable<Radio>>(radios);
            Assert.True(radios.Count() > 0, "Result count is 0");
            Assert.IsAssignableFrom<int>(recordCount);
        }

        [Fact]
        public async Task GetDetailForSystemAsyncActiveThrowExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.RadiosGetDetailForSystemActiveWithPaging(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(),
                It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var radioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => radioRepo.GetDetailForSystemAsync("140", true, "RadioID", "Ascending", 1, 15));
        }

        [Fact]
        public async Task GetDetailForSystemAsyncAllThrowExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.RadiosGetDetailForSystemWithPaging(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(),
                It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var radioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => radioRepo.GetDetailForSystemAsync("140", false, "RadioID", "Ascending", 1, 15));
        }

        [Fact]
        public async Task GetDetailForSystemAsyncActiveFiltersReturnsAppropriateTypes()
        {
            var mockRadioDetailtsResults = new Mock<MockObjectResult<RadioDetails_Result>>();

            mockRadioDetailtsResults.Setup(rdr => rdr.GetEnumerator()).Returns(GetTestRadioDetailsResults);
            _context.Setup(ctx => ctx.RadiosGetDetailForSystemActiveFiltersWithPaging(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRadioDetailtsResults.Object);
            SetupMockRepo();

            var radioRepo = _mockRepo.Object;

            var (radios, recordCount) = await radioRepo.GetDetailForSystemAsync("140", true, _filterData);

            Assert.NotNull(radios);
            Assert.IsAssignableFrom<IEnumerable<Radio>>(radios);
            Assert.True(radios.Count() > 0, "Result count is 0");
            Assert.IsAssignableFrom<int>(recordCount);
        }

        [Fact]
        public async Task GetDetailForSystemAsyncAllFiltersReturnsAppropriateTypes()
        {
            var mockRadioDetailtsResults = new Mock<MockObjectResult<RadioDetails_Result>>();

            mockRadioDetailtsResults.Setup(rdr => rdr.GetEnumerator()).Returns(GetTestRadioDetailsResults);
            _context.Setup(ctx => ctx.RadiosGetDetailForSystemFiltersWithPaging(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRadioDetailtsResults.Object);
            SetupMockRepo();

            var radioRepo = _mockRepo.Object;

            var (radios, recordCount) = await radioRepo.GetDetailForSystemAsync("140", false, _filterData);

            Assert.NotNull(radios);
            Assert.IsAssignableFrom<IEnumerable<Radio>>(radios);
            Assert.True(radios.Count() > 0, "Result count is 0");
            Assert.IsAssignableFrom<int>(recordCount);
        }

        [Fact]
        public async Task GetDetailForSystemAsyncActiveFiltersThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.RadiosGetDetailForSystemActiveFiltersWithPaging(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var radioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => radioRepo.GetDetailForSystemAsync("140", true, _filterData));
        }

        [Fact]
        public async Task GetDetailForSystemAsyncAllFiltersThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.RadiosGetDetailForSystemFiltersWithPaging(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var radioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => radioRepo.GetDetailForSystemAsync("140", false, _filterData));
        }

        [Fact]
        public async Task GetCountForSystemAsyncReturnsACount()
        {
            var mockRadioCount = new Mock<MockObjectResult<int?>>();

            mockRadioCount.Setup(rc => rc.GetEnumerator()).Returns(GetTestRadioCount);
            _context.Setup(ctx => ctx.RadiosGetCountForSystem(It.IsAny<int>())).Returns(mockRadioCount.Object);
            SetupMockRepo();

            var radioRepo = _mockRepo.Object;

            var result = await radioRepo.GetCountForSystemAsync(1);

            Assert.IsAssignableFrom<int>(result);
        }

        [Fact]
        public async Task GetCountForSystemAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.RadiosGetCountForSystem(It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var radioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => radioRepo.GetCountForSystemAsync(1));
        }

        [Fact]
        public async Task GetNamesAsyncReturnsAppropriateTypes()
        {
            var mockRadioNameResults = new Mock<MockObjectResult<RadiosNames_Result>>();

            mockRadioNameResults.Setup(rnr => rnr.GetEnumerator()).Returns(GetTestRadioNamesResults);
            _context.Setup(ctx => ctx.RadiosGetSystemNames(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRadioNameResults.Object);
            SetupMockRepo();

            var radioRepo = _mockRepo.Object;

            var (names, recordCount) = await radioRepo.GetNamesAsync("140", _filterData);

            Assert.NotNull(names);
            Assert.IsAssignableFrom<IEnumerable<(int, string)>>(names);
            Assert.True(names.Count() > 0, "Result count is 0");
            Assert.IsAssignableFrom<int>(recordCount);
        }

        [Fact]
        public async Task GetNamesAsyncReturnsAppropriateValues()
        {
            var mockRadioNameResults = new Mock<MockObjectResult<RadiosNames_Result>>();

            mockRadioNameResults.Setup(rnr => rnr.GetEnumerator()).Returns(GetTestRadioNamesResults);
            _context.Setup(ctx => ctx.RadiosGetSystemNames(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRadioNameResults.Object);
            SetupMockRepo();

            var radioRepo = _mockRepo.Object;

            var (names, recordCount) = await radioRepo.GetNamesAsync("140", _filterData);
            var (radioID, name) = names.SingleOrDefault(rd => rd.radioID == _testRadioNamesResult2.RadioID);

            Assert.Equal(radioID, _testRadioNamesResult2.RadioID);
            Assert.Equal(name, _testRadioNamesResult2.Description);
        }

        [Fact]
        public async Task GetNamesAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.RadiosGetSystemNames(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Database error"));
            SetupMockRepo();

            var radioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => radioRepo.GetNamesAsync("140", _filterData));
        }
    }
}
