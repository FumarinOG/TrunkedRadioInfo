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
using static RadioService.Factory;
using static ServiceCommon.Common;
using static ServiceCommon.Factory;

namespace RadioService
{
    public sealed class RadioService : ServiceBase, IRadioService
    {
        private readonly IRadioRepository _radioRepo;

        public RadioService(IRadioRepository radioRepo) : base((mc) => CreateMapping(mc)) => _radioRepo = radioRepo;

        public static void CreateMapping(IMapperConfigurationExpression config)
        {
            config.CreateMap<(int radioID, string name), RadioNameViewModel>()
                .ForMember(dest => dest.RadioID, opt => opt.MapFrom(src => src.radioID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.name));
            config.CreateMap<Radio, RadioViewModel>()
                .ForMember(dest => dest.RadioName, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.VoiceCount, opt => opt.MapFrom(src => src.VoiceGrantCount))
                .ForMember(dest => dest.EmergencyCount, opt => opt.MapFrom(src => src.EmergencyVoiceGrantCount))
                .ForMember(dest => dest.EncryptedCount, opt => opt.MapFrom(src => src.EncryptedVoiceGrantCount));
        }

        public async Task<RadioViewModel> GetDetailAsync(int systemID, int radioID) => await GetDetailAsync(systemID, radioID, null);

        public async Task<RadioViewModel> GetDetailAsync(int systemID, int radioID, FilterDataModel filters)
        {
            if ((filters != null) && ((filters.DateFrom != null) || (filters.DateTo != null)))
            {
                return _mapper.Map<RadioViewModel>(await _radioRepo.GetDetailAsync(systemID, radioID, filters.DateFrom, filters.DateTo));
            }

            return _mapper.Map<RadioViewModel>(await _radioRepo.GetDetailAsync(systemID, radioID));
        }

        public async Task<RadioDataViewModel> GetDetailAsync(SystemInfo systemInfo, int radioID, SearchDataViewModel searchData) =>
            CreateRadioDataModel(systemInfo, radioID, await GetDetailAsync(systemInfo.ID, radioID, searchData.SearchData), searchData);

        public async Task<IEnumerable<Radio>> GetDetailForSystemAsync(int systemID) => await _radioRepo.GetDetailForSystemAsync(systemID);

        public async Task<IEnumerable<Radio>> GetDetailForSystemAsync(int systemID, string sortField, string sortDirection, int pageNumber, int pageSize) => null;

        public async Task<IEnumerable<RadioViewModel>> GetViewForSystemAsync(int systemID) =>
            (await _radioRepo.GetDetailForSystemAsync(systemID)).Select(r => _mapper.Map<RadioViewModel>(r));

        public async Task<(IEnumerable<RadioViewModel> radios, int recordCount)> GetViewForSystemAsync(string systemID, bool activeOnly, FilterDataModel filterData)
        {
            (IEnumerable<Radio> radios, int recordCount) results;

            CheckFilterRanges(filterData, Radio.MIN_RADIO_ID, Radio.MAX_RADIO_ID);

            if (GetAllRecords(filterData))
            {
                results = await _radioRepo.GetDetailForSystemAsync(systemID, activeOnly, filterData.SortField, filterData.SortDirection,
                    filterData.PageNumber, filterData.PageSize);
            }
            else
            {
                results = await _radioRepo.GetDetailForSystemAsync(systemID, activeOnly, _mapper.Map<FilterData>(filterData));
            }

            return (results.radios.Select(r => _mapper.Map<RadioViewModel>(r)), results.recordCount);
        }

        public async Task<int> GetCountForSystemAsync(int systemID) => await _radioRepo.GetCountForSystemAsync(systemID);

        public async Task<IEnumerable<Radio>> GetForSystemAsync(int systemID) => await _radioRepo.GetListForSystemAsync(systemID);

        public async Task<(IEnumerable<RadioNameViewModel> names, int recordCount)> GetNamesAsync(string systemID, FilterDataModel filterData)
        {
            CheckFilterRanges(filterData, Radio.MIN_RADIO_ID, Radio.MAX_RADIO_ID);

            var (names, recordCount) = await _radioRepo.GetNamesAsync(systemID, _mapper.Map<FilterData>(filterData));

            return (names.Select(rn => _mapper.Map<RadioNameViewModel>(rn)), recordCount);
        }

        public string ProcessRecord(int systemID, int radioID, string description, DateTime timeStamp, string action, ICollection<Radio> radios,
            Action<IRecord, ActionTypes> updateHitCounts)
        {
            var radio = radios.SingleOrDefault(r => r.RadioID == radioID);

            if (radio == null)
            {
                radio = CreateRadio(systemID, radioID, description, timeStamp);
                radios.Add(radio);

                // If the radio description is empty, a name is created so store the newly created name
                description = radio.Description;
            }

            updateHitCounts(radio, action.GetEnumFromDescription<ActionTypes>());
            CheckDates(radio, timeStamp);
            return description;
        }

        public Radio CreateRadio(int systemID, int radioID, string description, DateTime timeStamp)
        {
            var radio = Create<Radio>();

            radio.SystemID = systemID;
            radio.RadioID = radioID;
            radio.Description = string.IsNullOrWhiteSpace(description) ? $"<Unknown> ({radioID:0})" : description;
            radio.FirstSeen = timeStamp;
            radio.LastSeen = timeStamp;

            return radio;
        }
    }
}
