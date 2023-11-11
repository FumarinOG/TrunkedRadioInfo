using DataLibrary.Interfaces;
using ServiceLibrary.Abstracts;
using ServiceLibrary.Interfaces.Services;
using System;

namespace ServiceLibrary.Services
{
    public class MergeService : ServiceBase, IMergeService
    {
        private readonly IMergeRepository _mergeRepo;

        public MergeService(IMergeRepository mergeRepository)
        {
            _mergeRepo = mergeRepository;
        }

        public void MergeRecords(Guid sessionID, Action<string, string, int, int> updateProgress)
        {
            updateProgress("Merging temp records", "Merging temp records", 1, 1);
            _mergeRepo.MergeRecords(sessionID);
        }

        public void DeleteTempTables(Guid sessionID)
        {
            _mergeRepo.DeleteTempTables(sessionID);
        }

        public void DeleteTempTables(Guid sessionID, Action<string, string, int, int> updateProgress)
        {
            updateProgress("Deleting temp records", "Deleting temp records", 1, 1);
            _mergeRepo.DeleteTempTables(sessionID);
        }
    }
}
