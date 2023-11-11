using System;
using Xunit;

namespace ProcessedFileService.Tests.Core
{
    public static class FactoryTests
    {
        [Theory]
        [InlineData("20180618-Affiliations-131.txt", "Affiliations", "06-18-2018 17:01:15", 1750302, FileTypes.Affiliations, "06-18-2018")]
        [InlineData("20180617-GrantLog-131.txt", "GrantLog", "06-17-2018 19:10:51", 320302, FileTypes.GrantLog, "06-17-2018")]
        [InlineData("20180616-PatchLog-131.txt", "PatchLog", "06-16-2018 09:12:33", 1220302, FileTypes.PatchLog, "06-16-2018")]
        [InlineData("Radios.txt", "Radios", "06-16-2018 10:12:32", 1320302, FileTypes.Radios, "06-16-2018 10:12:32")]
        [InlineData("System.ini", "System", "06-15-2018 22:31:52", 20302, FileTypes.System, "06-15-2018 22:31:52")]
        [InlineData("Talkgroups.txt", "Talkgroups", "06-14-2018 23:11:24", 5320302, FileTypes.Talkgroups, "06-14-2018 23:11:24")]
        [InlineData("Tower001031.txt", "Tower", "06-13-2018 14:41:21", 3113, FileTypes.Tower, "06-13-2018 14:41:21")]
        public static void CreateUploadFileReturnsAppropriateModel(string fileName, string fileType, string lastModified, int fileSize,
            FileTypes expectedFileType, string expectedFileDate)
        {
            var result = Factory.CreateUploadFile(fileName, fileType, DateTime.Parse(lastModified), fileSize);

            Assert.Equal(fileName, result.FileName);
            Assert.Equal(expectedFileType, result.Type);
            Assert.Equal(DateTime.Parse(expectedFileDate), result.FileDate);
            Assert.Equal(DateTime.Parse(lastModified), result.LastModified);
            Assert.Equal(fileSize, result.Size);
            Assert.Equal(FileStatus.NotProcessed, result.Status);
        }
    }
}
