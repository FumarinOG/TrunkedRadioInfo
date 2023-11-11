using AutoMapper;
using DataLibrary;
using DataLibrary.Interfaces;
using ObjectLibrary;
using ServiceCommon;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PatchService.Factory;

namespace PatchService
{
    public sealed class PatchService : ServiceBase, IPatchService
    {
        private readonly IPatchRepository _patchRepo;

        public PatchService(IPatchRepository patchRepository) : base((mc) => CreateMapping(mc)) => _patchRepo = patchRepository;

        public static void CreateMapping(IMapperConfigurationExpression config)
        {
            config.CreateMap<Patch, PatchViewModel>()
               .ForMember(dest => dest.SystemName, opt => opt.Ignore());
            config.CreateMap<Patch, PatchDatesViewModel>();
        }

        public async Task<PatchViewModel> GetSummaryAsync(SystemInfo systemInfo, int fromTalkgroupID, int toTalkgroupID)
        {
            var patchSummary = await _patchRepo.GetSummaryAsync(systemInfo.ID, fromTalkgroupID, toTalkgroupID);

            return CreatePatchModel(systemInfo, patchSummary);
        }

        public async Task<(IEnumerable<PatchViewModel> patches, int recordCount)> GetSummaryAsync(string systemID, int fromTalkgroupID, 
            int toTalkgroupID, FilterDataModel filterData)
        {
            CheckFilterRanges(filterData, Patch.MIN_TALKGROUP_ID, Patch.MAX_TALKGROUP_ID);

            var (patches, recordCount) = await _patchRepo.GetSummaryAsync(systemID, fromTalkgroupID, toTalkgroupID, _mapper.Map<FilterData>(filterData));

            return (patches.Select(p => _mapper.Map<PatchViewModel>(p)), recordCount);
        }

        public async Task<IEnumerable<Patch>> GetSummaryForSystemAsync(int systemID) => await _patchRepo.GetSummaryForSystemAsync(systemID);

        public async Task<(IEnumerable<PatchViewModel> patches, int recordCount)> GetSummaryForSystemAsync(string systemID, FilterDataModel filterData)
        {
            CheckFilterRanges(filterData, Patch.MIN_TALKGROUP_ID, Patch.MAX_TALKGROUP_ID);

            var (patches, recordCount) = await _patchRepo.GetSummaryForSystemAsync(systemID, _mapper.Map<FilterData>(filterData));

            return (patches.Select(p => _mapper.Map<PatchViewModel>(p)), recordCount);
        }

        public async Task<IEnumerable<Patch>> GetForPatchByDateAsync(int systemID, int fromTalkgroupID, int toTalkgroupID) =>
            await _patchRepo.GetForPatchByDateAsync(systemID, fromTalkgroupID, toTalkgroupID);

        public async Task<IEnumerable<PatchDatesViewModel>> GetForPatchByDateAsync(int systemID, int fromTalkgroupID, int toTalkgroupID,
            int pageNumber, int pageSize)
        {
            var patches = await _patchRepo.GetForPatchByDateAsync(systemID, fromTalkgroupID, toTalkgroupID, pageNumber, pageSize);

            return patches.Select(p => _mapper.Map<PatchDatesViewModel>(p));
        }

        public async Task<int> GetForPatchByDateCountAsync(int systemID, int fromTalkgroupID, int toTalkgropupID) =>
            await _patchRepo.GetForPatchByDateCountAsync(systemID, fromTalkgroupID, toTalkgropupID);

        public async Task<IEnumerable<Patch>> GetForSystemTalkgroupAsync(int systemID, int talkgroupID) =>
            await _patchRepo.GetForSystemTalkgroupAsync(systemID, talkgroupID);

        public async Task<int> GetForSystemTalkgroupCountAsync(int systemID, int talkgroupID) => await _patchRepo.GetForSystemTalkgroupCountAsync(systemID, talkgroupID);

        public async Task<(IEnumerable<Patch> patches, int recordCount)> GetForSystemTalkgroupAsync(string systemID, int talkgroupID, FilterDataModel filterData)
        {
            CheckFilterRanges(filterData, Patch.MIN_TALKGROUP_ID, Patch.MAX_TALKGROUP_ID);

            var (patches, recordCount) = await _patchRepo.GetForSystemTalkgroupAsync(systemID, talkgroupID, _mapper.Map<FilterData>(filterData));

            return (patches, recordCount);
        }

        public async Task<int> GetCountForSystemAsync(int systemID, string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return await _patchRepo.GetCountForSystemAsync(systemID);
            }

            return await _patchRepo.GetCountForSystemAsync(systemID, searchText);
        }

        public async Task<(IEnumerable<Patch> patches, int recordCount)> GetForSystemTowerAsync(string systemID, int talkgroupID, FilterDataModel filterData)
        {
            CheckFilterRanges(filterData, Patch.MIN_TALKGROUP_ID, Patch.MAX_TALKGROUP_ID);

            var (patches, recordCount) = await _patchRepo.GetForSystemTowerAsync(systemID, talkgroupID, _mapper.Map<FilterData>(filterData));

            return (patches, recordCount);
        }
    }
}
