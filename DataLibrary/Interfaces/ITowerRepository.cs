using DataAccessLibrary;
using ObjectLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLibrary.Interfaces
{
    public interface ITowerRepository
    {
        Task<Tower> GetAsync(int id);
        Task<Tower> GetAsync(int systemID, int towerNumber);
        Task<IEnumerable<Tower>> GetForSystemAsync(int systemID);
        Task<(IEnumerable<Tower> towers, int recordCount)> GetForSystemAsync(string systemID, bool activeOnly, FilterData filterData);
        Task<int> GetCountForSystemAsync(int systemID);
        Task WriteAsync(Tower tower);
        void EditRecord(Towers databaseRecord, Tower tower);
    }
}
