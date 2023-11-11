using DataLibrary.Interfaces;
using DataLibrary.Repositories;
using DataLibrary.TempData;
using FileService;
using FileService.Interfaces;
using FileService.Parsers;
using ObjectLibrary;
using PatchService;
using ProcessedFileService;
using RadioHistoryService;
using RadioService;
using SearchDataService;
using SystemInfoService;
using TalkgroupHistoryService;
using TalkgroupRadioService;
using TalkgroupService;
using TowerFrequencyRadioService;
using TowerFrequencyService;
using TowerFrequencyTalkgroupService;
using TowerFrequencyUsageService;
using TowerNeighborService;
using TowerRadioService;
using TowerService;
using TowerTableService;
using TowerTalkgroupRadioService;
using TowerTalkgroupService;
using Unity;
using Unity.Injection;

namespace Admin.Helpers
{
    public class UnityConfig
    {
        public static readonly string PARSER_AFFILIATIONS = "Affiliations";
        public static readonly string PARSER_GRANT_LOG = "GrantLog";
        public static readonly string PARSER_PATCH_LOG = "PatchLog";
        public static readonly string PARSER_RADIO = "Radio";
        public static readonly string PARSER_SYSTEM = "System";
        public static readonly string PARSER_TALKGROUP = "Talkgroup";
        public static readonly string PARSER_TOWER = "Tower";

        public static readonly string SERVICE_AFFILIATIONS = "Affiliations";
        public static readonly string SERVICE_BASE_TOWER = "BaseTowerService";
        public static readonly string SERVICE_BASE_TOWER_NEIGHBOR = "BaseTowerNeighborService";
        public static readonly string SERVICE_GRANT_LOG = "GrantLog";
        public static readonly string SERVICE_PATCH_LOG = "PatchLog";
        public static readonly string SERVICE_UPLOAD_TOWER = "UploadTowerService";
        public static readonly string SERVICE_UPLOAD_TOWER_FREQUENCY = "UploadTowerFrequencyService";
        public static readonly string SERVICE_UPLOAD_TOWER_NEIGHBOR = "UploadTowerNeighborService";
        public static readonly string SERVICE_UPLOAD_TOWER_TABLE = "UploadTowerTableService";

        private readonly IUnityContainer _container;

        public UnityConfig(IUnityContainer container)
        {
            _container = container;
        }

        public void Config()
        {
            _container.RegisterType<IResolver, Resolver>();
            RegisterServices();
            RegisterRepositories();
            RegisterTempServices();
            RegisterTempRepositories();
            RegisterParsers();
            RegisterForms();

            _container.RegisterInstance(_container);
        }

        private void RegisterRepositories() =>
            _container.RegisterType<IMergeRepository, MergeRepository>()
                .RegisterType<IPatchRepository, PatchRepository>()
                .RegisterType<IProcessedFileRepository, ProcessedFileRepository>()
                .RegisterType<IRadioRepository, RadioRepository>()
                .RegisterType<IRadioHistoryRepository, RadioHistoryRepository>()
                .RegisterType<ISystemInfoRepository, SystemInfoRepository>()
                .RegisterType<ITalkgroupRepository, TalkgroupRepository>()
                .RegisterType<ITalkgroupHistoryRepository, TalkgroupHistoryRepository>()
                .RegisterType<ITalkgroupRadioRepository, TalkgroupRadioRepository>()
                .RegisterType<ITowerRepository, TowerRepository>()
                .RegisterType<ITowerFrequencyRepository, TowerFrequencyRepository>()
                .RegisterType<ITowerFrequencyRadioRepository, TowerFrequencyRadioRepository>()
                .RegisterType<ITowerFrequencyTalkgroupRepository, TowerFrequencyTalkgroupRepository>()
                .RegisterType<ITowerFrequencyUsageRepository, TowerFrequencyUsageRepository>()
                .RegisterType<ITowerNeighborRepository, TowerNeighborRepository>()
                .RegisterType<ITowerRadioRepository, TowerRadioRepository>()
                .RegisterType<ITowerTableRepository, TowerTableRepository>()
                .RegisterType<ITowerTalkgroupRepository, TowerTalkgroupRepository>()
                .RegisterType<ITowerTalkgroupRadioRepository, TowerTalkgroupRadioRepository>();

        private void RegisterServices() =>
            _container.RegisterType<IMergeService, MergeService>()
                .RegisterType<IPatchService, PatchService.PatchService>()
                .RegisterType<IProcessedFileService, ProcessedFileService.ProcessedFileService>()
                .RegisterType<ISearchDataService, SearchDataService.SearchDataService>()
                .RegisterType<IService, AffiliationLogService>(SERVICE_AFFILIATIONS)
                .RegisterType<IService, GrantLogService>(SERVICE_GRANT_LOG)
                .RegisterType<IService, PatchLogService>(SERVICE_PATCH_LOG)
                .RegisterType<IRadioFileService, RadioFileService>(new InjectionConstructor(typeof(int), typeof(IRadioRepository), typeof(IRadioHistoryService),
                    typeof(ITempService<TempRadio, Radio>), typeof(ITempService<TempRadioHistory, RadioHistory>), typeof(IMergeService)))
                .RegisterType<IRadioService, RadioService.RadioService>()
                .RegisterType<IRadioHistoryService, RadioHistoryService.RadioHistoryService>()
                .RegisterType<ISystemInfoService, SystemInfoService.SystemInfoService>()
                .RegisterType<ITalkgroupFileService, TalkgroupFileService>(new InjectionConstructor(typeof(int), typeof(ITalkgroupRepository), typeof(ITalkgroupHistoryService),
                    typeof(ITempService<TempTalkgroup, Talkgroup>), typeof(ITempService<TempTalkgroupHistory, TalkgroupHistory>), typeof(IMergeService)))
                .RegisterType<ITalkgroupService, TalkgroupService.TalkgroupService>()
                .RegisterType<ITalkgroupHistoryService, TalkgroupHistoryService.TalkgroupHistoryService>()
                .RegisterType<ITalkgroupRadioService, TalkgroupRadioService.TalkgroupRadioService>()
                .RegisterType<ITowerFileService, TowerFileService>()
                .RegisterType<ITowerService, TowerService.TowerService>()
                .RegisterType<ITowerFrequencyService, TowerFrequencyService.TowerFrequencyService>()
                .RegisterType<ITowerFrequencyRadioService, TowerFrequencyRadioService.TowerFrequencyRadioService>()
                .RegisterType<ITowerFrequencyTalkgroupService, TowerFrequencyTalkgroupService.TowerFrequencyTalkgroupService>()
                .RegisterType<ITowerFrequencyUsageService, TowerFrequencyUsageService.TowerFrequencyUsageService>()
                .RegisterType<ITowerNeighborService, TowerNeighborService.TowerNeighborService>()
                .RegisterType<ITowerRadioService, TowerRadioService.TowerRadioService>()
                .RegisterType<ITowerTableService, TowerTableService.TowerTableService>()
                .RegisterType<ITowerTalkgroupService, TowerTalkgroupService.TowerTalkgroupService>()
                .RegisterType<ITowerTalkgroupRadioService, TowerTalkgroupRadioService.TowerTalkgroupRadioService>();

        private void RegisterTempRepositories() =>
            _container.RegisterType<ITempRepository<TempPatch, Patch>, TempRepository<TempPatch, Patch>>()
                .RegisterType<ITempRepository<TempRadio, Radio>, TempRepository<TempRadio, Radio>>()
                .RegisterType<ITempRepository<TempRadioHistory, RadioHistory>, TempRepository<TempRadioHistory, RadioHistory>>()
                .RegisterType<ITempRepository<TempSystemInfo, SystemInfo>, TempRepository<TempSystemInfo, SystemInfo>>()
                .RegisterType<ITempRepository<TempTalkgroup, Talkgroup>, TempRepository<TempTalkgroup, Talkgroup>>()
                .RegisterType<ITempRepository<TempTalkgroupHistory, TalkgroupHistory>, TempRepository<TempTalkgroupHistory, TalkgroupHistory>>()
                .RegisterType<ITempRepository<TempTalkgroupRadio, TalkgroupRadio>, TempRepository<TempTalkgroupRadio, TalkgroupRadio>>()
                .RegisterType<ITempRepository<TempTower, Tower>, TempRepository<TempTower, Tower>>()
                .RegisterType<ITempRepository<TempTowerFrequencyRadio, TowerFrequencyRadio>, TempRepository<TempTowerFrequencyRadio, TowerFrequencyRadio>>()
                .RegisterType<ITempRepository<TempTowerFrequencyTalkgroup, TowerFrequencyTalkgroup>, TempRepository<TempTowerFrequencyTalkgroup, TowerFrequencyTalkgroup>>()
                .RegisterType<ITempRepository<TempTowerFrequencyUsage, TowerFrequencyUsage>, TempRepository<TempTowerFrequencyUsage, TowerFrequencyUsage>>()
                .RegisterType<ITempRepository<TempTowerRadio, TowerRadio>, TempRepository<TempTowerRadio, TowerRadio>>()
                .RegisterType<ITempRepository<TempTowerTalkgroup, TowerTalkgroup>, TempRepository<TempTowerTalkgroup, TowerTalkgroup>>()
                .RegisterType<ITempRepository<TempTowerTalkgroupRadio, TowerTalkgroupRadio>, TempRepository<TempTowerTalkgroupRadio, TowerTalkgroupRadio>>();

        private void RegisterTempServices() =>
            _container.RegisterType<ITempService<TempPatch, Patch>, TempService<TempPatch, Patch>>()
                .RegisterType<ITempService<TempRadio, Radio>, TempService<TempRadio, Radio>>()
                .RegisterType<ITempService<TempRadioHistory, RadioHistory>, TempService<TempRadioHistory, RadioHistory>>()
                .RegisterType<ITempService<TempSystemInfo, SystemInfo>, TempService<TempSystemInfo, SystemInfo>>()
                .RegisterType<ITempService<TempTalkgroup, Talkgroup>, TempService<TempTalkgroup, Talkgroup>>()
                .RegisterType<ITempService<TempTalkgroupHistory, TalkgroupHistory>, TempService<TempTalkgroupHistory, TalkgroupHistory>>()
                .RegisterType<ITempService<TempTalkgroupRadio, TalkgroupRadio>, TempService<TempTalkgroupRadio, TalkgroupRadio>>()
                .RegisterType<ITempService<TempTower, Tower>, TempService<TempTower, Tower>>()
                .RegisterType<ITempService<TempTowerFrequencyRadio, TowerFrequencyRadio>, TempService<TempTowerFrequencyRadio, TowerFrequencyRadio>>()
                .RegisterType<ITempService<TempTowerFrequencyTalkgroup, TowerFrequencyTalkgroup>, TempService<TempTowerFrequencyTalkgroup, TowerFrequencyTalkgroup>>()
                .RegisterType<ITempService<TempTowerFrequencyUsage, TowerFrequencyUsage>, TempService<TempTowerFrequencyUsage, TowerFrequencyUsage>>()
                .RegisterType<ITempService<TempTowerRadio, TowerRadio>, TempService<TempTowerRadio, TowerRadio>>()
                .RegisterType<ITempService<TempTowerTalkgroup, TowerTalkgroup>, TempService<TempTowerTalkgroup, TowerTalkgroup>>()
                .RegisterType<ITempService<TempTowerTalkgroupRadio, TowerTalkgroupRadio>, TempService<TempTowerTalkgroupRadio, TowerTalkgroupRadio>>();

        private void RegisterParsers() =>
            _container.RegisterType<IParser, ParseAffiliations>(PARSER_AFFILIATIONS)
                .RegisterType<IParser, ParseGrantLog>(PARSER_GRANT_LOG)
                .RegisterType<IParser, ParsePatchLog>(PARSER_PATCH_LOG)
                .RegisterType<IParser, ParseRadios>(PARSER_RADIO)
                .RegisterType<IParser, ParseSystem>(PARSER_SYSTEM)
                .RegisterType<IParser, ParseTalkgroups>(PARSER_TALKGROUP)
                .RegisterType<IParser, ParseTower>(PARSER_TOWER);

        private void RegisterForms()
        {
            _container.RegisterType<MainRadForm>();
            _container.RegisterType<PatchDataRadForm>();
            _container.RegisterType<ProgressRadForm>();
            _container.RegisterType<RadioDataRadForm>();
            _container.RegisterType<SystemInfoRadForm>();
            _container.RegisterType<TalkgroupDataRadForm>();
            _container.RegisterType<TowerDataRadForm>();
            _container.RegisterType<UploadFilesRadForm>();
        }
    }
}
