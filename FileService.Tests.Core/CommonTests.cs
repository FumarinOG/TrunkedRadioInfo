using System;
using Xunit;

namespace FileService.Tests.Core
{
    public class CommonTests
    {
        [Theory]
        [InlineData("c:\\TestDir\\Files\\ABC\\123\\20180618-Affiliations-131.csv", 2018)]
        [InlineData("20070328-GrantLog-201.csv", 2007)]
        [InlineData("c:\\20180514-PatchLog-1010.csv", 2018)]
        public void GetYearReturnsProperYearFromFileName(string fileName, int expectedYear)
        {
            var year = Common.GetYear(fileName);

            Assert.Equal(expectedYear, year);
        }

        [Theory]
        [InlineData("c:\\TestDir\\Files\\ABC\\123\\20a180618-Affiliations-131.csv")]
        [InlineData("2dd70328-GrantLog-201.csv")]
        [InlineData("c:\\abcd514-PatchLog-1010.csv")]
        public void GetYearThrowsExceptionWithInvalidDateInTheFileName(string fileName)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Common.GetYear(fileName));
        }

        [Theory]
        [InlineData("c:\\TestDir\\Files\\ABC\\123\\20180618-Affiliations-131.csv", 1031)]
        [InlineData("20070328-GrantLog-201.csv", 2001)]
        [InlineData("c:\\20180514-PatchLog-1010.csv", 10010)]
        public void ParseTowerNumberReturnsProperTowerNumber(string fileName, int towerNumber)
        {
            var result = Common.ParseTowerNumber(fileName);

            Assert.Equal(towerNumber, result);
        }

        [Theory]
        [InlineData("c:\\TestDir\\Files\\ABC\\123\\20180618-Affiliations-ABC.csv")]
        [InlineData("20070328-GrantLog-2QW1.csv")]
        [InlineData("c:\\20180514-PatchLog-101A.csv")]
        public void ParseTowerNumberThrowsExceptionWithInvalidTowerNumberInTheFileName(string fileName)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Common.ParseTowerNumber(fileName));
        }

        [Theory]
        [InlineData("170", 1070)]
        [InlineData("1031", 1031)]
        [InlineData("1010", 10010)]
        public void FixTowerNumberReturnsProperTowerNumber(string towerNumber, int expectedTowerNumber)
        {
            var result = Common.FixTowerNumber(towerNumber);

            Assert.Equal(expectedTowerNumber, expectedTowerNumber);
        }

        [Theory]
        [InlineData("1A0")]
        [InlineData("DEF1")]
        [InlineData("1010A")]
        public void FixTowerNumberThrowsExceptionWithInvalidTowerNumber(string towerNumber)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Common.FixTowerNumber(towerNumber));
        }

        [Theory]
        [InlineData("01/31 18:07:14", 2010, "01-31-2010 18:07:14")]
        [InlineData("06/18 00:03:17", 2018, "06-18-2018 00:03:17")]
        public void FixDateReturnsAppropriateDate(string date, int year, string expectedDate)
        {
            var result = Common.FixDate(date, year);

            Assert.Equal(DateTime.Parse(expectedDate), result);
        }

        [Theory]
        [InlineData("01-31-2018:18:07 :14", 2010)]
        [InlineData("06/A8 00:03:17", 2018)]
        public void FixDateThrowsExceptionWithInvalidDateText(string date, int year)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Common.FixDate(date, year));
        }

        [Theory]
        [InlineData("", 0)]
        [InlineData("9019", 9019)]
        [InlineData("1917101", 1917101)]
        public void ParseIDReturnsAppropriateID(string id, int expectedID)
        {
            var result = Common.ParseID(id);

            Assert.Equal(expectedID, result);
        }

        [Fact]
        public void ParseIDThrowsExceptionWithInvalidIDText()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Common.ParseID("ABC"));
        }
    }
}
