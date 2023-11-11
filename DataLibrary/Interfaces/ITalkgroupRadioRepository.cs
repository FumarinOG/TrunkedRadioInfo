using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLibrary.Interfaces
{
    public interface ITalkgroupRadioRepository
    {
        Task<IEnumerable<TalkgroupRadio>> GetForSystemAsync(int systemID);
        Task<IEnumerable<TalkgroupRadio>> GetForSystemAsync(int systemID, DateTime date);
        Task<IEnumerable<TalkgroupRadio>> GetTalkgroupsForRadioAsync(int systemID, int radioID);
        Task<(IEnumerable<TalkgroupRadio> talkgroupRadios, int recordCount)> GetTalkgroupsForRadioAsync(string systemID, int radioID, FilterData filterData);
        Task<int> GetTalkgroupsForRadioCountAsync(int systemID, int radioID);
        Task<IEnumerable<TalkgroupRadio>> GetRadiosForTalkgroupAsync(int systemID, int talkgroupID);
        Task<int> GetRadiosForTalkgroupCountAsync(int systemID, int talkgroupID);
        Task<(IEnumerable<TalkgroupRadio> talkgroupRadios, int recordCount)> GetRadiosForTalkgroupAsync(string systemID, int talkgroupID, FilterData filterData);
        Task<IEnumerable<TalkgroupRadio>> GetRadiosForTalkgroupWithDatesAsync(int systemID, int talkgroupID);
    }
}
