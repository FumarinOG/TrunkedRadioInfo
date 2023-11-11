using System;
using Xunit;

namespace ObjectLibrary.Tests.Core
{
    public class ProcessedFileTests
    {
        [Fact]
        public void PropertiesAssignedProperly()
        {
            var fileDate = DateTime.Now.AddDays(-1);
            var dateProcessed = DateTime.Now;

            var processedFile = new ProcessedFile
            {
                SystemID = 1,
                LongFileName = "c:\\Pro96Com\\System140\\20180612-GrantLog-131.csv",
                Type = FileTypes.GrantLog,
                FileDate = fileDate,
                DateProcessed = dateProcessed,
                RowCount = 12345
            };

            Assert.Equal(1, processedFile.SystemID);
            Assert.Equal("c:\\Pro96Com\\System140\\20180612-GrantLog-131.csv", processedFile.LongFileName);
            Assert.Equal(FileTypes.GrantLog, processedFile.Type);
            Assert.Equal(fileDate, processedFile.FileDate);
            Assert.Equal(dateProcessed, processedFile.DateProcessed);
            Assert.Equal(12345, processedFile.RowCount);
            Assert.True(processedFile.IsDirty);
        }

        [Fact]
        public void ToStringWorks()
        {
            var processedFile = new ProcessedFile
            {
                LongFileName = "c:\\Pro96Com\\System140\\20180612-Affiliations-131.csv"
            };

            Assert.Equal("Filename 20180612-Affiliations-131.csv", processedFile.ToString());
        }

        [Theory]
        [InlineData("c:\\Pro96Com\\System140\\20180612-Affiliations-131.csv", "20180612-Affiliations-131.csv")]
        [InlineData("c:\\Pro96Com\\20180612-Affiliations-131.csv", "20180612-Affiliations-131.csv")]
        [InlineData("c:\\20180612-Affiliations-131.csv", "20180612-Affiliations-131.csv")]
        [InlineData("20180612-Affiliations-131.csv", "20180612-Affiliations-131.csv")]
        public void GetFileNameReturnsProperValues(string fileName, string result)
        {
            Assert.Equal(result, ProcessedFile.GetFileName(fileName));
        }
    }
}
