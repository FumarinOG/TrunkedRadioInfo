using DataLibrary.Interfaces;
using FileService.Interfaces;
using System;
using System.Threading.Tasks;

namespace FileService
{
    public sealed class MergeService : IMergeService
    {
        private readonly IMergeRepository _mergeRepo;

        public MergeService(IMergeRepository mergeRepository) => _mergeRepo = mergeRepository;

        public async Task MergeRecordsAsync(Guid sessionID, Action<string, string, int, int> updateProgress)
        {
            updateProgress("Merging temp records", "Merging temp records", 1, 1);
            await _mergeRepo.MergeRecordsAsync(sessionID);
        }

        public async Task DeleteTempTablesAsync(Guid sessionID) => await _mergeRepo.DeleteTempTablesAsync(sessionID);

        public async Task DeleteTempTablesAsync(Guid sessionID, Action<string, string, int, int> updateProgress)
        {
            updateProgress("Deleting temp records", "Deleting temp records", 1, 1);
            await _mergeRepo.DeleteTempTablesAsync(sessionID);
        }
    }
}
