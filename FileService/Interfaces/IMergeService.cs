using System;
using System.Threading.Tasks;

namespace FileService.Interfaces
{
    public interface IMergeService
    {
        Task MergeRecordsAsync(Guid sessionID, Action<string, string, int, int> updateProgress);
        Task DeleteTempTablesAsync(Guid sessionID);
        Task DeleteTempTablesAsync(Guid sessionID, Action<string, string, int, int> updateProgrss);
    }
}
