using ObjectLibrary;
using ObjectLibrary.Interfaces;
using ServiceCommon;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TalkgroupRadioService
{
    public interface ITalkgroupRadioService
    {
        Task<IEnumerable<TalkgroupRadio>> GetForSystemAsync(int systemID);
        Task<IEnumerable<TalkgroupRadio>> GetForSystemAsync(int systemID, DateTime date);
        Task<IEnumerable<TalkgroupRadio>> GetRadiosForTalkgroupAsync(int systemID, int talkgroupID);
        Task<int> GetRadiosForTalkgroupCountAsync(int systemID, int talkgroupID);
        Task<(IEnumerable<TalkgroupRadioViewModel> talkgroupRadios, int recordCount)> GetRadiosForTalkgroupAsync(string systemID, int talkgroupID,
            FilterDataModel filterData);
        Task<int> GetRadiosForTalkgroupCountAsync(int systemID, int talkgroupID, string searchText, string searchData);
        Task<IEnumerable<TalkgroupRadio>> GetRadiosForTalkgroupWithDatesAsync(int systemID, int talkgroupID);
        Task<IEnumerable<TalkgroupRadio>> GetTalkgroupsForRadioAsync(int systemID, int radioID);
        Task<(IEnumerable<RadioTalkgroupViewModel> radioTalkgroups, int recordCount)> GetTalkgroupsForRadioAsync(string systemID, int radioID,
            FilterDataModel filterData);
        Task<int> GetTalkgroupsForRadioCountAsync(int systemID, int radioID);
        void ProcessRecord(int systemID, int talkgroupID, int radioID, ICollection<TalkgroupRadio> talkgroupRadios, DateTime timeStamp, string action,
            Action<ICounterRecord, ActionTypes> updateCounters);
        TalkgroupRadio CreateTalkgroupRadio(int systemID, int talkgroupID, int radioID, DateTime timeStamp);
    }
}
