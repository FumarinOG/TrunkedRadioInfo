using System;
using Xunit;

namespace ProcessedFileService.Tests.Core
{
    public class ProcessedFileViewModelTests
    {
        [Fact]
        public void ConstructorAssignsProperValues()
        {
            var fileDate = DateTime.Now.AddDays(-3);
            var dateProcessed = DateTime.Now;
            var random = new Random(DateTime.Now.Millisecond);
            var rowCount = random.Next();

            var processedFileViewModel = new ProcessedFileViewModel("20180618-GrantLog-131.csv", fileDate, dateProcessed, rowCount);

            Assert.Equal("20180618-GrantLog-131.csv", processedFileViewModel.FileName);
            Assert.Equal(fileDate, processedFileViewModel.FileDate);
            Assert.Equal(dateProcessed, processedFileViewModel.DateProcessed);
            Assert.Equal(rowCount, processedFileViewModel.RowCount);
        }
    }
}
