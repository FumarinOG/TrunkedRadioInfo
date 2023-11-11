using System;
using Xunit;

namespace ProcessedFileService.Tests.Core
{
    public class UploadFileModelTests
    {
        [Fact]
        public void PropertiesAssignedProperly()
        {
            var fileDate = DateTime.Now.AddDays(-3);
            var lastModified = DateTime.Now;
            var random = new Random(DateTime.Now.Millisecond);
            var fileSize = random.Next();
            var rowCount = random.Next();

            var uploadFileModel = new UploadFileModel
            {
                SystemID = "140",
                FileName = "20180615-PatchLog-131.csv",
                Type = FileTypes.PatchLog,
                FileDate = fileDate,
                LastModified = lastModified,
                Size = fileSize,
                Status = FileStatus.Processing,
                RowCount = rowCount
            };

            Assert.Equal("140", uploadFileModel.SystemID);
            Assert.Equal("20180615-PatchLog-131.csv", uploadFileModel.FileName);
            Assert.Equal(FileTypes.PatchLog, uploadFileModel.Type);
            Assert.Equal("Patch Log", uploadFileModel.TypeText);
            Assert.Equal(fileDate, uploadFileModel.FileDate);
            Assert.Equal(lastModified, uploadFileModel.LastModified);
            Assert.Equal(fileSize, uploadFileModel.Size);
            Assert.Equal(fileSize / 1024, uploadFileModel.SizeK);
            Assert.Equal(FileStatus.Processing, uploadFileModel.Status);
            Assert.Equal("Processing", uploadFileModel.StatusText);
            Assert.Equal(rowCount, uploadFileModel.RowCount);
        }

        [Fact]
        public void ToStringWorks()
        {
            var uploadFileModel = new UploadFileModel
            {
                SystemID = "140",
                FileName = "20180615-PatchLog-131.csv",
                Type = FileTypes.PatchLog,
            };

            Assert.Equal("Filename 20180615-PatchLog-131.csv, Type PatchLog", uploadFileModel.ToString());
        }
    }
}
