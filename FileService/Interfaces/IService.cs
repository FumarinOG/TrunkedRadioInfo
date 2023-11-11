using ObjectLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileService.Interfaces
{
    public interface IService
    {
        Task<int> ProcessRecordsAsync(IEnumerable<IRecord> records, Action<string, string, int, int> updateProgress, Action<bool> completedTasks,
            bool showCompleted);
    }
}
