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
    public class TowerFrequencyRadioRepositoryTests : RepositoryTestBase<TowerFrequencyRadioRepository>
    {
        private readonly TowerFrequencyRadios _testTowerFrequencyRadio1 = new TowerFrequencyRadios
        {
            ID = 1,
            SystemID = 1,
            TowerID = 1034,
            Frequency = "851.31250/1",
            RadioID = 3005908,
            Date = DateTime.Now.AddDays(-1),
            FirstSeen = DateTime.Now.AddYears(-2),
            LastSeen = DateTime.Now.AddDays(-1).AddHours(-2),
            LastModified = DateTime.Now,
            AffiliationCount = 550,
            DeniedCount = 1050,
            VoiceGrantCount = 1550,
            EmergencyVoiceGrantCount = 10050,
            EncryptedVoiceGrantCount = 15050,
            DataCount = 100050,
            PrivateDataCount = 150050,
            AlertCount = 1000050
        };

        private readonly TowerFrequencyRadios _testTowerFrequencyRadio2 = new TowerFrequencyRadios
        {
            ID = 1,
            SystemID = 1,
            TowerID = 1034,
            Frequency = "851.31250/1",
            RadioID = 3105908,
            Date = DateTime.Now.AddDays(-2),
            FirstSeen = DateTime.Now.AddYears(-3),
            LastSeen = DateTime.Now.AddDays(-4).AddHours(-5),
            LastModified = DateTime.Now,
            AffiliationCount = 250,
            DeniedCount = 1250,
            VoiceGrantCount = 2250,
            EmergencyVoiceGrantCount = 10250,
            EncryptedVoiceGrantCount = 15250,
            DataCount = 100250,
            PrivateDataCount = 150250,
            AlertCount = 1002050
        };

        private readonly TowerFrequencyRadios_Result _testTowerFrequencyRadioResult1 = new TowerFrequencyRadios_Result
        {
            RadioID = 3005908,
            RadioDescription = "ISP Unit 18-47 (Litchfield)",
            FirstSeen = DateTime.Now.AddYears(-3),
            LastSeen = DateTime.Now.AddDays(-4).AddHours(-5),
            AffiliationCount = 250,
            DeniedCount = 1250,
            VoiceGrantCount = 2250,
            EmergencyVoiceGrantCount = 10250,
            EncryptedVoiceGrantCount = 15250,
            DataCount = 100250,
            PrivateDataCount = 150250,
            AlertCount = 1002050,
            RecordCount = 2
        };

        private readonly TowerFrequencyRadios_Result _testTowerFrequencyRadioResult2 = new TowerFrequencyRadios_Result
        {
            RadioID = 3105908,
            RadioDescription = "ISP Unit 18-47 (Litchfield)",
            FirstSeen = DateTime.Now.AddYears(-2),
            LastSeen = DateTime.Now.AddDays(-1).AddHours(-2),
            AffiliationCount = 550,
            DeniedCount = 1050,
            VoiceGrantCount = 1550,
            EmergencyVoiceGrantCount = 10050,
            EncryptedVoiceGrantCount = 15050,
            DataCount = 100050,
            PrivateDataCount = 150050,
            AlertCount = 1000050,
            RecordCount = 2
        };

        private IEnumerator<TowerFrequencyRadios> GetTowerFrequencyRadios()
        {
            yield return _testTowerFrequencyRadio1;
            yield return _testTowerFrequencyRadio2;
        }

        private IEnumerator<TowerFrequencyRadios_Result> GetTowerFrequencyRadioResults()
        {
            yield return _testTowerFrequencyRadioResult1;
            yield return _testTowerFrequencyRadioResult2;
        }

        [Fact]
        public async Task GetForTowerAsyncReturnAppropriateTypes()
        {
            var mockTowerFrequencyRadios = new Mock<MockObjectResult<TowerFrequencyRadios>>();

            mockTowerFrequencyRadios.Setup(tfr => tfr.GetEnumerator()).Returns(GetTowerFrequencyRadios);
            _context.Setup(ctx => ctx.TowerFrequencyRadiosGetForTowerDate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(mockTowerFrequencyRadios.Object);
            SetupMockRepo();

            var towerFrequencyRadioRepo = _mockRepo.Object;

            var results = await towerFrequencyRadioRepo.GetForTowerAsync(1, 1034, DateTime.Now);

            Assert.NotNull(results);
            Assert.IsAssignableFrom<IEnumerable<TowerFrequencyRadio>>(results);
            Assert.True(results.Count() > 0, "Result count is 0");
        }

        [Fact]
        public async Task GetForTowerAsyncReturnsAppropriateValues()
        {
            var mockTowerFrequencyRadios = new Mock<MockObjectResult<TowerFrequencyRadios>>();

            mockTowerFrequencyRadios.Setup(tfr => tfr.GetEnumerator()).Returns(GetTowerFrequencyRadios);
            _context.Setup(ctx => ctx.TowerFrequencyRadiosGetForTowerDate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(mockTowerFrequencyRadios.Object);
            SetupMockRepo();

            var towerFrequencyRadioRepo = _mockRepo.Object;

            var results = await towerFrequencyRadioRepo.GetForTowerAsync(1, 1034, DateTime.Now);
            var resultsData = results.SingleOrDefault(tfr => tfr.Frequency == "851.31250/1" && tfr.RadioID == 3105908);

            Assert.NotNull(resultsData);
            Assert.Equal(_testTowerFrequencyRadio2.SystemID, resultsData.SystemID);
            Assert.Equal(_testTowerFrequencyRadio2.TowerID, resultsData.TowerID);
            Assert.Equal(_testTowerFrequencyRadio2.Frequency, resultsData.Frequency);
            Assert.Equal(_testTowerFrequencyRadio2.RadioID, resultsData.RadioID);
            Assert.Equal(_testTowerFrequencyRadio2.Date, resultsData.Date);
            Assert.Equal(_testTowerFrequencyRadio2.FirstSeen, resultsData.FirstSeen);
            Assert.Equal(_testTowerFrequencyRadio2.LastSeen, resultsData.LastSeen);
            Assert.Equal(_testTowerFrequencyRadio2.LastModified, resultsData.LastModified);
            Assert.Equal(_testTowerFrequencyRadio2.AffiliationCount, resultsData.AffiliationCount);
            Assert.Equal(_testTowerFrequencyRadio2.DeniedCount, resultsData.DeniedCount);
            Assert.Equal(_testTowerFrequencyRadio2.VoiceGrantCount, resultsData.VoiceGrantCount);
            Assert.Equal(_testTowerFrequencyRadio2.EmergencyVoiceGrantCount, resultsData.EmergencyVoiceGrantCount);
            Assert.Equal(_testTowerFrequencyRadio2.EncryptedVoiceGrantCount, resultsData.EncryptedVoiceGrantCount);
            Assert.Equal(_testTowerFrequencyRadio2.DataCount, resultsData.DataCount);
            Assert.Equal(_testTowerFrequencyRadio2.PrivateDataCount, resultsData.PrivateDataCount);
            Assert.Equal(_testTowerFrequencyRadio2.AlertCount, resultsData.AlertCount);
        }

        [Fact]
        public async Task GetForTowerAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerFrequencyRadiosGetForTowerDate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                .Throws(new Exception("Database error"));
            SetupMockRepo();

            var towerFrequencyRadioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerFrequencyRadioRepo.GetForTowerAsync(1, 1034, DateTime.Now));
        }

        [Fact]
        public async Task GetRadiosForTowerFrequencyAsyncReturnsAppropriateTypes()
        {
            var mockTowerFrequencyRadioResults = new Mock<MockObjectResult<TowerFrequencyRadios_Result>>();

            mockTowerFrequencyRadioResults.Setup(tfr => tfr.GetEnumerator()).Returns(GetTowerFrequencyRadioResults);
            _context.Setup(ctx => ctx.TowerFrequencyRadiosGetRadiosForFrequenciesWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(),
                It.IsAny<int>())).Returns(mockTowerFrequencyRadioResults.Object);
            SetupMockRepo();

            var towerFrequencyRadioRepo = _mockRepo.Object;

            var (towerFrequencyRadios, recordCount) = await towerFrequencyRadioRepo.GetRadiosForTowerFrequencyAsync("140", 1034, "851.31250/1", _filterData);

            Assert.NotNull(towerFrequencyRadios);
            Assert.IsAssignableFrom<IEnumerable<TowerFrequencyRadio>>(towerFrequencyRadios);
            Assert.True(towerFrequencyRadios.Count() > 0, "Result count is 0");
            Assert.IsAssignableFrom<int>(recordCount);
        }

        [Fact]
        public async Task GetRadiosForTowerFrequencyAsyncReturnsAppropriateValues()
        {
            var mockTowerFrequencyRadioResults = new Mock<MockObjectResult<TowerFrequencyRadios_Result>>();

            mockTowerFrequencyRadioResults.Setup(tfr => tfr.GetEnumerator()).Returns(GetTowerFrequencyRadioResults);
            _context.Setup(ctx => ctx.TowerFrequencyRadiosGetRadiosForFrequenciesWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(),
                It.IsAny<int>())).Returns(mockTowerFrequencyRadioResults.Object);
            SetupMockRepo();

            var towerFrequencyRadioRepo = _mockRepo.Object;

            var (towerFrequencyRadios, recordCount) = await towerFrequencyRadioRepo.GetRadiosForTowerFrequencyAsync("140", 1034, "851.31250/1", _filterData);
            var resultData = towerFrequencyRadios.SingleOrDefault(tfr => tfr.RadioID == 3005908);

            Assert.NotNull(resultData);
            Assert.Equal(_testTowerFrequencyRadioResult1.RadioID, resultData.RadioID);
            Assert.Equal(_testTowerFrequencyRadioResult1.RadioDescription, resultData.RadioName);
            Assert.Equal(_testTowerFrequencyRadioResult1.FirstSeen, resultData.FirstSeen);
            Assert.Equal(_testTowerFrequencyRadioResult1.LastSeen, resultData.LastSeen);
            Assert.Equal(_testTowerFrequencyRadioResult1.AffiliationCount, resultData.AffiliationCount);
            Assert.Equal(_testTowerFrequencyRadioResult1.DeniedCount, resultData.DeniedCount);
            Assert.Equal(_testTowerFrequencyRadioResult1.VoiceGrantCount, resultData.VoiceGrantCount);
            Assert.Equal(_testTowerFrequencyRadioResult1.EmergencyVoiceGrantCount, resultData.EmergencyVoiceGrantCount);
            Assert.Equal(_testTowerFrequencyRadioResult1.EncryptedVoiceGrantCount, resultData.EncryptedVoiceGrantCount);
            Assert.Equal(_testTowerFrequencyRadioResult1.DataCount, resultData.DataCount);
            Assert.Equal(_testTowerFrequencyRadioResult1.PrivateDataCount, resultData.PrivateDataCount);
            Assert.Equal(_testTowerFrequencyRadioResult1.AlertCount, resultData.AlertCount);

            Assert.Equal(_testTowerFrequencyRadioResult1.RecordCount, recordCount);
        }

        [Fact]
        public async Task GetRadiosForTowerFrequencyAsyncThrowsExceptionWithDatabaseException()
        {
            _context.Setup(ctx => ctx.TowerFrequencyRadiosGetRadiosForFrequenciesWithPaging(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(),
                It.IsAny<int>())).Throws(new Exception());
            SetupMockRepo();

            var towerFrequencyRadioRepo = _mockRepo.Object;

            await Assert.ThrowsAsync<Exception>(() => towerFrequencyRadioRepo.GetRadiosForTowerFrequencyAsync("140", 1034, "851.31250/1", _filterData));
        }
    }
}
