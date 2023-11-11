using DataAccessLibrary;
using ObjectLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLibrary.Interfaces
{
    public interface IDatabaseStatsRepository
    {
        Task<DatabaseStat> GetDatabaseStatsAsync();
        TrunkedRadioInfoEntities CreateEntities();
        List<T> CreateList<T>() where T : new();
    }
}
