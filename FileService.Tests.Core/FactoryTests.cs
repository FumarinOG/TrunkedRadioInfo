using DataLibrary.TempData;
using FileService.Interfaces;
using FileService.Parsers;
using ObjectLibrary;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static FileService.Factory;

namespace FileService.Tests.Core
{
    public class FactoryTests
    {
        [Fact]
        public void CreateReturnsNewObjectWithIsNewSet()
        {
            var result = Create<ProcessedFile>();

            Assert.IsAssignableFrom<ProcessedFile>(result);
            Assert.True(result.IsNew);
            Assert.False(result.IsDirty);
        }

        [Fact]
        public void CreateListReturnsListOfAClass()
        {
            var result = CreatList<Radio>();

            Assert.IsAssignableFrom<List<Radio>>(result);
        }

        [Fact]
        public void CreateTempCreatesTempVersionOfAClass()
        {
            var result = CreateTemp<TempRadio, Radio>();

            Assert.IsAssignableFrom<TempRadio>(result);
        }

        [Fact]
        public void CreateTempListCreatesAListOfTempObjects()
        {
            var result = CreateTempList<TempRadio, Radio>();

            Assert.IsAssignableFrom<List<TempRadio>>(result);
        }

        [Fact]
        public void CreateParserReturnsInstanceOfTheCSVReader()
        {
            var result = CreateParser(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes("ABC"))));

            Assert.IsAssignableFrom<CSVReader>(result);
        }

        [Fact]
        public void CreateReaderReturnsInstanceOfTheCSVReader()
        {
            var result = CreateReader("c:\\TestFile.csv");

            Assert.IsAssignableFrom<Task<StreamReader>>(result);
        }

        [Theory]
        [MemberData(nameof(GetAffiliationParsers))]
        public void GetAffiliationParserReturnsProperParser(string[] dataRow, IAffiliationParser affiliationParser)
        {
            var result = GetAffiliationParser(1, 1, 2018, dataRow);

            if (affiliationParser != null)
            {
                Assert.IsType(affiliationParser.GetType(), result);
            }
            else
            {
                Assert.Null(result);
            }
        }

        public static IEnumerable<object[]> GetAffiliationParsers()
        {
            var data = new[]
            {
                new object[] { new string[] { "07/04 23:19:28", "Denied", " 2372", "36332", "" }, new ParseAffiliationsV1(1, 1, 2018) },
                new object[] { new string[] { "07/05 00:12:24", "Affiliate", "30341", "ITTF Incident 5", "419002", "IL National Guard Dixon Armory" },
                    new ParseAffiliationsV2(1, 1, 2018) },
                new object[] { new string[] { "07/05 00:12:24", "Affiliate", "30341", "ITTF Incident 5", "419002", "IL National Guard Dixon Armory",
                    "07/05 00:12:24", "Affiliate", "30341", "ITTF Incident 5", "419002", "IL National Guard Dixon Armory"},
                    new ParseAffiliationsLong(1, 1, 2018) },
                new object[] { new string[] { "" }, null }
            };

            return data;
        }

        [Theory]
        [MemberData(nameof(GetPatchLogParsers))]
        public void GetPatchLogParserReturnsProperParser(string[] dataRow, IPatchLogParser patchLogParser)
        {
            var result = GetPatchLogParser(1, 1, 2018, dataRow);

            if (patchLogParser != null)
            {
                Assert.IsType(patchLogParser.GetType(), result);
            }
            else
            {
                Assert.Null(result);
            }
        }

        public static IEnumerable<object[]> GetPatchLogParsers()
        {
            var data = new[]
            {
                new object[] { new string[] { "07/06 09:56:06","Added Patch:   9013 (ISP 05-A - Joliet Primary) --> 9002 (ISP 02-A - Elgin Primary)" },
                    new ParsePatchLogV1(1, 1, 2018) },
                new object[] { new string[] { "07/06 09:56:06","Added Patch:   9013 (ISP 05-A - Joliet Primary) --> 9002 (ISP 02-A - Elgin Primary)",
                    "07/06 09:56:09","Removed Patch: 9013 (ISP 05-A - Joliet Primary) --> 9002 (ISP 02-A - Elgin Primary)" },
                    new ParsePatchLogLong(1, 1, 2018) },
                new object[] { new string[] { "" }, null }
            };

            return data;
        }

        [Theory]
        [MemberData(nameof(GetRadioParsers))]
        public void GetRadioParserReturnsProperParser(int version, IRadioParser radioParser)
        {
            var result = GetRadioParser(version, 1);

            if (radioParser != null)
            {
                Assert.IsType(radioParser.GetType(), radioParser);
            }
            else
            {
                Assert.Null(result);
            }
        }

        public static IEnumerable<object[]> GetRadioParsers()
        {
            var data = new[]
            {
                new object[] { 4, new ParseRadiosV4(1) },
                new object[] { -1, null }
            };

            return data;
        }

        [Theory]
        [MemberData(nameof(GetTalkgroupParsers))]
        public void GetTalkgroupParsersReturnsProperParser(int version, ITalkgroupParser talkgroupParser)
        {
            var result = GetTalkgroupParser(version, 1);

            if (talkgroupParser != null)
            {
                Assert.IsType(talkgroupParser.GetType(), talkgroupParser);
            }
            else
            {
                Assert.Null(result);
            }
        }

        public static IEnumerable<object[]> GetTalkgroupParsers()
        {
            var data = new[]
            {
                new object[] { 5, new ParseTalkgroupsV5(1) },
                new object[] { 6, new ParseTalkgroupsV6(1) },
                new object[] { 7, new ParseTalkgroupsV7(1) },
                new object[] { -1, null },
            };

            return data;
        }

        [Theory]
        [MemberData(nameof(GetTowerFrequencyParsers))]
        public void GetTowerFrequencyParserReturnsProperParser(int version, ITowerFrequencyParser towerFrequencyParser)
        {
            var result = GetTowerFrequencyParser(1, version);

            if (towerFrequencyParser != null)
            {
                Assert.IsType(towerFrequencyParser.GetType(), result);
            }
            else
            {
                Assert.Null(result);
            }
        }

        public static IEnumerable<object[]> GetTowerFrequencyParsers()
        {
            var data = new[]
            {
                new object[] { 4, new ParseTowerFrequencyV4(1) },
                new object[] { 5, new ParseTowerFrequencyV5(1) },
                new object[] { 6, new ParseTowerFrequencyV6(1) },
                new object[] { 7, new ParseTowerFrequencyV7(1) },
                new object[] { 8, new ParseTowerFrequencyV8(1) },
                new object[] { -1, null }
            };

            return data;
        }

        [Theory]
        [MemberData(nameof(GetTowerTableParsers))]
        public void GetTowerTableParserReturnsProperParser(int version, ITowerTableParser towerTableParser)
        {
            var result = GetTowerTableParser(1, version);

            if (towerTableParser != null)
            {
                Assert.IsType(towerTableParser.GetType(), result);
            }
            else
            {
                Assert.Null(result);
            }
        }

        public static IEnumerable<object[]> GetTowerTableParsers()
        {
            var data = new[]
            {
                new object[] { 4, new ParseTowerTableV4(1) },
                new object[] { 5, new ParseTowerTableV5(1) },
                new object[] { 6, new ParseTowerTableV6(1) },
                new object[] { 7, new ParseTowerTableV7(1) },
                new object[] { 8, new ParseTowerTableV8(1) },
                new object[] { -1, null }
            };

            return data;
        }

        [Theory]
        [MemberData(nameof(GetTowerNeighborParsers))]
        public void GetTowerNeighborReturnsProperParser(int version, ITowerNeighborParser towerNeighborParser)
        {
            var result = GetTowerNeighborParser(1, version);

            if (towerNeighborParser != null)
            {
                Assert.IsType(towerNeighborParser.GetType(), result);
            }
            else
            {
                Assert.Null(result);
            }
        }

        public static IEnumerable<object[]> GetTowerNeighborParsers()
        {
            var data = new[]
            {
                new object[] { 4, new ParseTowerNeighborV4(1) },
                new object[] { 5, new ParseTowerNeighborV5(1) },
                new object[] { 6, new ParseTowerNeighborV6(1) },
                new object[] { 7, new ParseTowerNeighborV7(1) },
                new object[] { 8, new ParseTowerNeighborV8(1) },
                new object[] { -1, null }
            };

            return data;
        }
    }
}
