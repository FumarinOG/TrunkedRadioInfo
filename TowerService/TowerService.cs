using AutoMapper;
using DataLibrary;
using DataLibrary.Interfaces;
using ObjectLibrary;
using ServiceCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ServiceCommon.Common;

namespace TowerService
{
    public sealed class TowerService : ServiceBase, ITowerService
    {
        private readonly ITowerRepository _towerRepo;

        public TowerService(ITowerRepository towerRepo) : base((mc) => CreateMapping(mc)) => _towerRepo = towerRepo;

        public static void CreateMapping(IMapperConfigurationExpression config) =>
            config.CreateMap<Tower, TowerViewModel>()
                .ForMember(dest => dest.TowerName, opt => opt.MapFrom(src => src.Description));

        public async Task<Tower> GetForTowerAsync(int systemID, int towerNumber) => await _towerRepo.GetAsync(systemID, towerNumber);

        public async Task<TowerViewModel> GetDetailAsync(int systemID, int towerNumber) =>
            _mapper.Map<TowerViewModel>(await _towerRepo.GetAsync(systemID, towerNumber));

        public async Task<TowerDataViewModel> GetDetailAsync(SystemInfo systemInfo, int towerNumber, SearchDataViewModel searchData) =>
            new TowerDataViewModel(systemInfo.SystemID, systemInfo.Description, towerNumber, await GetDetailAsync(systemInfo.ID, towerNumber), searchData);

        public async Task<IEnumerable<Tower>> GetForSystemAsync(int systemID) => await _towerRepo.GetForSystemAsync(systemID);

        public async Task<IEnumerable<TowerViewModel>> GetViewForSystemAsync(int systemID) =>
            (await _towerRepo.GetForSystemAsync(systemID)).Select(t => _mapper.Map<TowerViewModel>(t));

        public async Task<(IEnumerable<TowerViewModel> towers, int recordCount)> GetViewForSystemAsync(string systemID, bool activeOnly, FilterDataModel filterData)
        {
            CheckFilterRanges(filterData, Tower.MIN_TOWER_NUMBER, Tower.MAX_TOWER_NUMBER);

            var (towers, recordCount) = await _towerRepo.GetForSystemAsync(systemID, activeOnly, _mapper.Map<FilterData>(filterData));

            return (towers.Select(t => _mapper.Map<TowerViewModel>(t)), recordCount);
        }

        public async Task<int> GetCountForSystemAsync(int systemID) => await _towerRepo.GetCountForSystemAsync(systemID);

        public void ProcessRecord(Tower tower, DateTime timeStamp) => CheckDates(tower, timeStamp);

    }
}
