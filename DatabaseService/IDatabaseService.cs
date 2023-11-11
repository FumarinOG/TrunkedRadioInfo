using System.Threading.Tasks;

namespace DatabaseService
{
    public interface IDatabaseService
    {
        Task<DatabaseStatsViewModel> GetDatabaseStatsAsync();
    }
}
