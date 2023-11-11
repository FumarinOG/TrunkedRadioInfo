using ObjectLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLibrary.Interfaces
{
    public interface IRadioHistoryRepository
    {
        Task<IEnumerable<RadioHistory>> GetForRadioAsync(int systemID, int radioID);
        Task<(IEnumerable<RadioHistory> radioHistory, int recordCount)> GetForRadioAsync(string systemID, int radioID, FilterData filterData);
        Task<int> GetForRadioCountAsync(int systemID, int radioID);
        Task<IEnumerable<RadioHistory>> GetForSystemAsync(int systemID);
    }
}
