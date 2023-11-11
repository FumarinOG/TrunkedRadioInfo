using AutoMapper;
using DataLibrary;
using DataLibrary.Interfaces;
using ObjectLibrary;
using ObjectLibrary.Interfaces;
using ServiceCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ServiceCommon.Common;

namespace SystemInfoService
{
    public sealed class SystemInfoService : ServiceBase, ISystemInfoService
    {
        private readonly ISystemInfoRepository _systemInfoRepo;
        private readonly ITowerRepository _towerRepo;

        public SystemInfoService(ISystemInfoRepository systemInfoRepo) : base((mc) => CreateMapping(mc)) => _systemInfoRepo = systemInfoRepo;

        public SystemInfoService(ISystemInfoRepository systemInfoRepository, ITowerRepository towerRepository)
            : this(systemInfoRepository) => _towerRepo = towerRepository;

        public static void CreateMapping(IMapperConfigurationExpression config)
        {
            config.CreateMap<SystemInfo, SystemInfoViewModel>();
            config.CreateMap<SystemInfo, SystemViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Description));
        }

        public async Task<SystemInfo> GetAsync(int id) => await _systemInfoRepo.GetAsync(id);

        public async Task<SystemInfo> GetAsync(string systemID) => await _systemInfoRepo.GetAsync(systemID);

        public async Task<SystemViewModel> GetSystemAsync(string systemID) => _mapper.Map<SystemViewModel>(await _systemInfoRepo.GetAsync(systemID));

        public async Task<int> GetSystemIDAsync(string systemID) => await _systemInfoRepo.GetSystemIDAsync(systemID);

        public async Task<string> GetNameAsync(string systemID) => (await _systemInfoRepo.GetAsync(systemID)).Description;

        public async Task<IEnumerable<SystemInfoViewModel>> GetListAsync() =>
            (await _systemInfoRepo.GetListAsync()).Select(s => _mapper.Map<SystemInfoViewModel>(s));

        public async Task<IEnumerable<SystemInfoViewModel>> GetListAsync(FilterDataModel filterData) =>
            (await _systemInfoRepo.GetListAsync(_mapper.Map<FilterData>(filterData))).Select(s => _mapper.Map<SystemInfoViewModel>(s));

        public async Task WriteAsync(SystemInfo systemInfo)
        {
            await WriteSystemAsync(systemInfo);
            await WriteTowersAsync(systemInfo);
        }

        public async Task WriteAsync(IEnumerable<IRecord> system) => await WriteAsync(((IEnumerable<SystemInfo>)system).First());

        private async Task WriteSystemAsync(SystemInfo systemInfo)
        {
            var currentSystem = await GetAsync(systemInfo.SystemID);

            if (currentSystem != null)
            {
                systemInfo.ID = currentSystem.ID;

                if (!systemInfo.Equals(currentSystem))
                {
                    systemInfo.CopyData(currentSystem);
                    systemInfo.IsNew = false;
                }
                else
                {
                    return;
                }
            }

            await _systemInfoRepo.WriteAsync(systemInfo);
        }

        private async Task WriteTowersAsync(SystemInfo systemInfo)
        {
            foreach (var tower in systemInfo.Towers)
            {
                var currentTower = await _towerRepo.GetAsync(systemInfo.ID, tower.TowerNumber);

                if (currentTower != null)
                {
                    if (!currentTower.Equals(tower))
                    {
                        tower.CopyData(currentTower);
                        tower.IsNew = false;
                    }
                    else
                    {
                        continue;
                    }
                }

                tower.SystemID = systemInfo.ID;
                await _towerRepo.WriteAsync(tower);
            }
        }

        public void ProcessRecord(SystemInfo systemInfo, DateTime timeStamp) => CheckDates(systemInfo, timeStamp);
    }
}
