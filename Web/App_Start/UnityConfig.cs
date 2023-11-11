using DatabaseService;
using DataLibrary.Interfaces;
using DataLibrary.Repositories;
using PatchService;
using ProcessedFileService;
using RadioHistoryService;
using RadioService;
using SearchDataService;
using System;
using SystemDetailService;
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
using TowerTalkgroupRadioService;
using TowerTalkgroupService;
using Unity;

namespace Web
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            RegisterServices(container);
            RegisterRepositories(container);
        }

        private static void RegisterServices(IUnityContainer container)
        {
            container.RegisterType<IDatabaseService, DatabaseService.DatabaseService>()
                .RegisterType<IPatchService, PatchService.PatchService>()
                .RegisterType<IProcessedFileService, ProcessedFileService.ProcessedFileService>()
                .RegisterType<IRadioService, RadioService.RadioService>()
                .RegisterType<IRadioHistoryService, RadioHistoryService.RadioHistoryService>()
                .RegisterType<ISearchDataService, SearchDataService.SearchDataService>()
                .RegisterType<ITalkgroupService, TalkgroupService.TalkgroupService>()
                .RegisterType<ITalkgroupHistoryService, TalkgroupHistoryService.TalkgroupHistoryService>()
                .RegisterType<ITalkgroupRadioService, TalkgroupRadioService.TalkgroupRadioService>()
                .RegisterType<ITowerService, TowerService.TowerService>()
                .RegisterType<ITowerFrequencyService, TowerFrequencyService.TowerFrequencyService>()
                .RegisterType<ITowerFrequencyRadioService, TowerFrequencyRadioService.TowerFrequencyRadioService>()
                .RegisterType<ITowerFrequencyTalkgroupService, TowerFrequencyTalkgroupService.TowerFrequencyTalkgroupService>()
                .RegisterType<ITowerFrequencyUsageService, TowerFrequencyUsageService.TowerFrequencyUsageService>()
                .RegisterType<ITowerNeighborService, TowerNeighborService.TowerNeighborService>()
                .RegisterType<ITowerRadioService, TowerRadioService.TowerRadioService>()
                .RegisterType<ITowerTalkgroupService, TowerTalkgroupService.TowerTalkgroupService>()
                .RegisterType<ITowerTalkgroupRadioService, TowerTalkgroupRadioService.TowerTalkgroupRadioService>()
                .RegisterType<ISystemDetailService, SystemDetailService.SystemDetailService>()
                .RegisterType<ISystemInfoService, SystemInfoService.SystemInfoService>();
        }

        private static void RegisterRepositories(IUnityContainer container)
        {
            container.RegisterType<IDatabaseStatsRepository, DatabaseStatsRepository>()
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
                .RegisterType<ITowerTalkgroupRepository, TowerTalkgroupRepository>()
                .RegisterType<ITowerTalkgroupRadioRepository, TowerTalkgroupRadioRepository>();
        }
    }
}