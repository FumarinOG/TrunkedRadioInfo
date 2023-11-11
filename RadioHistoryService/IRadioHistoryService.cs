using ObjectLibrary;
using ServiceCommon;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RadioHistoryService
{
    public interface IRadioHistoryService
    {
        Task<IEnumerable<RadioHistory>> GetForRadioAsync(int systemID, int radioID);
        Task<int> GetForRadioCountAsync(int systemID, int radioID);
        Task<(IEnumerable<RadioHistoryViewModel> radioHistory, int recordCount)> GetForRadioAsync(string systemID, int radioID, FilterDataModel filterData);
        Task<IEnumerable<RadioHistory>> GetForSystemAsync(int systemID);
        void ProcessRecord(int systemID, int radioID, string description, DateTime timeStamp, ICollection<Radio> radios, ICollection<RadioHistory> history);
        RadioHistory CreateRadioHistory(int systemID, int radioID, string description, DateTime timeStamp);
    }
}
