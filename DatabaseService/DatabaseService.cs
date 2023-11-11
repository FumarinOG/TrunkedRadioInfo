using AutoMapper;
using DataLibrary.Interfaces;
using ObjectLibrary;
using ServiceCommon;
using System.Threading.Tasks;

namespace DatabaseService
{
    public sealed class DatabaseService : ServiceBase, IDatabaseService
    {
        private IDatabaseStatsRepository _databaseStatsRepository;

        public DatabaseService(IDatabaseStatsRepository databaseStatsRepository) : base((mc) => CreateMapping(mc)) =>
            _databaseStatsRepository = databaseStatsRepository;

        public static void CreateMapping(IMapperConfigurationExpression config) => config.CreateMap<DatabaseStat, DatabaseStatsViewModel>();

        public async Task<DatabaseStatsViewModel> GetDatabaseStatsAsync()
        {
            var databaseStats = await _databaseStatsRepository.GetDatabaseStatsAsync();

            return _mapper.Map<DatabaseStatsViewModel>(databaseStats);
        }
    }
}
