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
using static ServiceCommon.Factory;

namespace RadioHistoryService
{
    public sealed class RadioHistoryService : ServiceBase, IRadioHistoryService
    {
        private readonly IRadioHistoryRepository _radioHistoryRepo;

        public RadioHistoryService(IRadioHistoryRepository radioHistoryRepository) : base((mc) => CreateMapping(mc)) =>
            _radioHistoryRepo = radioHistoryRepository;

        public static void CreateMapping(IMapperConfigurationExpression config) =>
            config.CreateMap<RadioHistory, RadioHistoryViewModel>()
                .ForMember(dest => dest.RadioName, opt => opt.MapFrom(src => src.Description));

        public async Task<IEnumerable<RadioHistory>> GetForRadioAsync(int systemID, int radioID) =>
            await _radioHistoryRepo.GetForRadioAsync(systemID, radioID);

        public async Task<int> GetForRadioCountAsync(int systemID, int radioID) => await _radioHistoryRepo.GetForRadioCountAsync(systemID, radioID);

        public async Task<(IEnumerable<RadioHistoryViewModel> radioHistory, int recordCount)> GetForRadioAsync(string systemID, int radioID,
            FilterDataModel filterData)
        {
            CheckFilterRanges(filterData, Radio.MIN_RADIO_ID, Radio.MAX_RADIO_ID);

            var (radioHistory, recordCount) = await _radioHistoryRepo.GetForRadioAsync(systemID, radioID, _mapper.Map<FilterData>(filterData));

            return (radioHistory.Select(rh => _mapper.Map<RadioHistoryViewModel>(rh)), recordCount);

        }

        public async Task<IEnumerable<RadioHistory>> GetForSystemAsync(int systemID) => await _radioHistoryRepo.GetForSystemAsync(systemID);

        public void ProcessRecord(int systemID, int radioID, string description, DateTime timeStamp, ICollection<Radio> radios, ICollection<RadioHistory> history)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                return;
            }

            var differentRadioDescription = radios.SingleOrDefault(r => r.RadioID == radioID && r.Description.Equals(description, StringComparison.OrdinalIgnoreCase));

            if (differentRadioDescription == null)
            {
                var newRadioHistory = CreateRadioHistory(systemID, radioID, description, timeStamp);

                var radioHistory = history.SingleOrDefault(rh => rh.RadioID == radioID && rh.Description.Equals(description, StringComparison.OrdinalIgnoreCase));

                if (radioHistory == null)
                {
                    history.Add(newRadioHistory);
                }
                else
                {
                    CheckDates(radioHistory, timeStamp);
                }
            }
            else
            {
                var radioHistory = history.SingleOrDefault(rh => rh.RadioID == radioID && rh.Description.Equals(description, StringComparison.OrdinalIgnoreCase));

                if (radioHistory != null)
                {
                    CheckDates(radioHistory, timeStamp);
                }
                else
                {
                    history.Add(CreateRadioHistory(systemID, radioID, description, timeStamp));
                }
            }
        }

        public RadioHistory CreateRadioHistory(int systemID, int radioID, string description, DateTime timeStamp)
        {
            var radioHistory = Create<RadioHistory>();

            radioHistory.SystemID = systemID;
            radioHistory.RadioID = radioID;

            if (string.IsNullOrWhiteSpace(description))
            {
                radioHistory.Description = $"<Unknown> ({radioID:0})";
            }
            else
            {
                radioHistory.Description = description;
            }

            radioHistory.FirstSeen = timeStamp;
            radioHistory.LastSeen = timeStamp;

            return radioHistory;
        }
    }
}
