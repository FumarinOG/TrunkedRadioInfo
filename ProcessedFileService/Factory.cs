using System;

namespace ProcessedFileService
{
    public static class Factory
    {
        public static UploadFileModel CreateUploadFile(string fileName, string fileType, DateTime lastModified, long size) =>
            new UploadFileModel
            {
                FileName = fileName,
                Type = fileType.ToEnum<FileTypes>(),
                FileDate = GetFileDate(fileName, lastModified),
                LastModified = lastModified,
                Size = size,
                Status = FileStatus.NotProcessed
            };

        public static DateTime GetFileDate(string fileName, DateTime lastModified)
        {
            if (fileName.Contains("-"))
            {
                return ServiceCommon.Factory.GetFileDate(fileName);
            }

            return lastModified;
        }

        public static DateTime GetFileDate(UploadFileModel uploadFile) => GetFileDate(uploadFile.FileName, uploadFile.LastModified);
    }
}
