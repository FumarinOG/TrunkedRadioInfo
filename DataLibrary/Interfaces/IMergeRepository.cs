using System;
using System.Threading.Tasks;

namespace DataLibrary.Interfaces
{
    public interface IMergeRepository
    {
        Task MergeRecordsAsync(Guid sessionID);
        Task DeleteTempTablesAsync(Guid sessionID);
    }
}
