using DataLibrary.Interfaces;
using System;
using System.Threading.Tasks;

namespace SystemDetailService
{
    public sealed class SystemDetailService : ISystemDetailService
    {
        private readonly ISystemInfoRepository _systemInfoRepository;
        private readonly ITalkgroupRepository _talkgroupRepository;
        private readonly IRadioRepository _radioRepository;
        private readonly ITowerRepository _towerRepository;
        private readonly IPatchRepository _patchRepository;
        private readonly IProcessedFileRepository _processedFileRepository;

        public SystemDetailService(ISystemInfoRepository systemInfoRepository, ITalkgroupRepository talkgroupRepository, IRadioRepository radioRepository,
            ITowerRepository towerRepository, IPatchRepository patchRepository, IProcessedFileRepository processedFileRepository)
        {
            _systemInfoRepository = systemInfoRepository;
            _talkgroupRepository = talkgroupRepository;
            _radioRepository = radioRepository;
            _towerRepository = towerRepository;
            _patchRepository = patchRepository;
            _processedFileRepository = processedFileRepository;
        }

        public async Task<SystemDetailViewModel> GetAsync(string systemID)
        {
            var systemInfo = await _systemInfoRepository.GetAsync(systemID);

            if (systemInfo == null)
            {
                throw new ArgumentException("Invalid system ID", nameof(systemID));
            }

            return new SystemDetailViewModel(systemID, 
                systemInfo.Description, 
                await _talkgroupRepository.GetCountForSystemAsync(systemInfo.ID),
                await _radioRepository.GetCountForSystemAsync(systemInfo.ID),
                await _towerRepository.GetCountForSystemAsync(systemInfo.ID),
                await _patchRepository.GetCountForSystemAsync(systemInfo.ID),
                await _processedFileRepository.GetCountForSystemAsync(systemInfo.ID));
        }
    }
}
