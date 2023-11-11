using FileService;
using FileService.Interfaces;
using ProcessedFileService;
using SystemInfoService;
using TowerService;
using Unity;
using Unity.Resolution;

namespace Admin.Helpers
{
    public class Resolver : IResolver
    {
        private const string PARAMETER_SYSTEM_INFO = "systemInfo";
        private const string PARAMETER_SYSTEM_ID = "systemID";
        private const string PARAMETER_TALKGROUP_ID = "talkgroupID";
        private const string PARAMETER_RADIO_ID = "radioID";
        private const string PARAMETER_TOWER_NUMBER = "towerNumber";
        private const string PARAMETER_FROM_TALKGROUP_ID = "fromTalkgroupID";
        private const string PARAMETER_TO_TALKGROUP_ID = "toTalkgroupID";
        private const string PARAMETER_UPLOAD_FILE = "uploadFile";
        private const string PARAMETER_TOWER_SERVICE = "towerService";
        private readonly IUnityContainer _container;

        public Resolver(IUnityContainer container) => (_container) = (container);

        // Services
        public IRadioFileService GetRadioFileService(int systemID) => 
            _container.Resolve<IRadioFileService>(new ParameterOverride(PARAMETER_SYSTEM_ID, systemID));

        public ISystemInfoService GetSystemInfoService() => _container.Resolve<ISystemInfoService>();

        public ITalkgroupFileService GetTalkgroupFileService(int systemID) =>
            _container.Resolve<ITalkgroupFileService>(new ParameterOverride(PARAMETER_SYSTEM_ID, systemID));

        public ITowerService GetTowerService() => _container.Resolve<ITowerService>();

        public IService GetFileService(int systemID, UploadFileModel uploadFile) =>
            _container.Resolve<IService>(uploadFile.Type.ToString(),
                new ParameterOverride(PARAMETER_SYSTEM_ID, systemID),
                new ParameterOverride(PARAMETER_TOWER_NUMBER, Common.ParseTowerNumber(uploadFile.FileName)),
                new ParameterOverride(PARAMETER_UPLOAD_FILE, uploadFile),
                new ParameterOverride(PARAMETER_TOWER_SERVICE, GetTowerService()));

        // Parsers
        public IParser GetParser(string parserType) => _container.Resolve<IParser>(parserType);

        public IParser GetParser(string parserType, int systemID) => _container.Resolve<IParser>(parserType, new ParameterOverride(PARAMETER_SYSTEM_ID, systemID));

        // Forms
        public PatchDataRadForm GetPatchDataForm(int systemID, int fromTalkgroupID, int toTalkgroupID) =>
            _container.Resolve<PatchDataRadForm>(new ParameterOverride(PARAMETER_SYSTEM_ID, systemID),
                new ParameterOverride(PARAMETER_FROM_TALKGROUP_ID, fromTalkgroupID),
                new ParameterOverride(PARAMETER_TO_TALKGROUP_ID, toTalkgroupID));

        public ProgressRadForm GetProgressForm() => _container.Resolve<ProgressRadForm>();

        public RadioDataRadForm GetRadioDataForm(SystemViewModel systemInfo, int radioID) =>
            _container.Resolve<RadioDataRadForm>(new ParameterOverride(PARAMETER_SYSTEM_INFO, systemInfo),
                new ParameterOverride(PARAMETER_RADIO_ID, radioID));

        public SystemInfoRadForm GetSystemInfoForm() => _container.Resolve<SystemInfoRadForm>();

        public TalkgroupDataRadForm GetTalkgroupDataForm(SystemViewModel systemInfo, int talkgroupID) =>
            _container.Resolve<TalkgroupDataRadForm>(new ParameterOverride(PARAMETER_SYSTEM_INFO, systemInfo),
                new ParameterOverride(PARAMETER_TALKGROUP_ID, talkgroupID));

        public TowerDataRadForm GetTowerDataForm(SystemViewModel systemInfo, int towerNumber) =>
            _container.Resolve<TowerDataRadForm>(new ParameterOverride(PARAMETER_SYSTEM_INFO, systemInfo),
                new ParameterOverride(PARAMETER_TOWER_NUMBER, towerNumber));

        public UploadFilesRadForm GetUploadFilesForm(SystemViewModel systemInfo) =>
            _container.Resolve<UploadFilesRadForm>(new ParameterOverride(PARAMETER_SYSTEM_INFO, systemInfo));
    }
}
