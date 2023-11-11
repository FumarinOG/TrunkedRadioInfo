using AutoMapper;
using DataLibrary;
using DataLibrary.Interfaces;
using ObjectLibrary;
using ServiceCommon;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ServiceCommon.Factory;

namespace TowerNeighborService
{
    public sealed class TowerNeighborService : ServiceBase, ITowerNeighborService
    {
        private readonly ITowerNeighborRepository _towerNeighborRepo;

        public TowerNeighborService(ITowerNeighborRepository towerNeighborRepository) : base((mc) => CreateMapping(mc)) =>
            _towerNeighborRepo = towerNeighborRepository;

        public static void CreateMapping(IMapperConfigurationExpression config) =>
            config.CreateMap<TowerNeighbor, TowerNeighborViewModel>()
                .ForMember(dest => dest.NeighborTowerNumber, opt => opt.MapFrom(src => src.NeighborTowerID))
                .ForMember(dest => dest.NeighborControlChannel, opt => opt.MapFrom(src => src.NeighborFrequency));

        public async Task<TowerNeighbor> GetAsync(int systemID, int towerNumber, int neighborSystemID, int neighborTowerNumber) =>
            await _towerNeighborRepo.GetAsync(systemID, towerNumber, neighborSystemID, neighborTowerNumber);

        public async Task<TowerNeighbor> GetForSystemTowerNumberAsync(int systemID, int towerNumber, int neighborSystemID, int neighborTowerNumber) =>
            await _towerNeighborRepo.GetForSystemTowerNumberAsync(systemID, towerNumber, neighborSystemID, neighborTowerNumber);

        public async Task<IEnumerable<TowerNeighbor>> GetForTowerAsync(int systemID, int towerNumber) =>
            await _towerNeighborRepo.GetNeighborsForTowerAsync(systemID, towerNumber);

        public async Task<(IEnumerable<TowerNeighborViewModel> towerNeighbors, int recordCount)> GetForTowerAsync(string systemID, int towerNumber,
            FilterDataModel filterData)
        {
            var (towerNeighbors, recordCount) = await _towerNeighborRepo.GetNeighborsForTowerAsync(systemID, towerNumber,
                _mapper.Map<FilterData>(filterData));

            return (towerNeighbors.Select(tn => _mapper.Map<TowerNeighborViewModel>(tn)), recordCount);
        }

        public async Task WriteAsync(TowerNeighbor towerNeighbor) => await _towerNeighborRepo.WriteAsync(towerNeighbor);

        public async Task DeleteForTowerAsync(int systemID, int towerNumber) => await _towerNeighborRepo.DeleteForTowerAsync(systemID, towerNumber);

        public TowerNeighbor Create(int systemID, int towerNumber, int neighborSystemID, int neighborTowerID, string neighborTowerNumberHex, string neighborTowerName, string neighborFrequency, string neighborChannel)
        {
            var towerNeighbor = Create<TowerNeighbor>();

            towerNeighbor.SystemID = systemID;
            towerNeighbor.TowerNumber = towerNumber;
            towerNeighbor.NeighborSystemID = neighborSystemID;
            towerNeighbor.NeighborTowerID = neighborTowerID;
            towerNeighbor.NeighborTowerNumberHex = neighborTowerNumberHex;
            towerNeighbor.NeighborTowerName = neighborTowerName;
            towerNeighbor.NeighborFrequency = neighborFrequency;
            towerNeighbor.NeighborChannel = neighborChannel;

            return towerNeighbor;
        }

        public void Fill(TowerNeighbor neighbor, TowerNeighbor currentNeighbor)
        {
            currentNeighbor.NeighborTowerNumberHex = neighbor.NeighborTowerNumberHex;
            currentNeighbor.NeighborChannel = neighbor.NeighborChannel;
            currentNeighbor.NeighborFrequency = neighbor.NeighborFrequency;
            currentNeighbor.NeighborTowerID = neighbor.NeighborTowerID;
            currentNeighbor.NeighborTowerName = neighbor.NeighborTowerName;
        }
    }
}
