using FileService.Interfaces;
using ProcessedFileService;
using SystemInfoService;
using TowerService;

namespace Admin.Helpers
{
    public interface IResolver
    {
        // Services
        IRadioFileService GetRadioFileService(int systemID);
        ISystemInfoService GetSystemInfoService();
        ITalkgroupFileService GetTalkgroupFileService(int systemID);
        ITowerService GetTowerService();
        IService GetFileService(int systemID, UploadFileModel uploadFile);

        // Parsers
        IParser GetParser(string parserType);
        IParser GetParser(string parserType, int systemID);

        // Forms
        PatchDataRadForm GetPatchDataForm(int systemID, int fromTalkgroupID, int toTalkgroupID);
        ProgressRadForm GetProgressForm();
        RadioDataRadForm GetRadioDataForm(SystemViewModel systemInfo, int radioID);
        SystemInfoRadForm GetSystemInfoForm();
        TalkgroupDataRadForm GetTalkgroupDataForm(SystemViewModel systemInfo, int talkgroupID);
        TowerDataRadForm GetTowerDataForm(SystemViewModel systemInfo, int towerNumber);
        UploadFilesRadForm GetUploadFilesForm(SystemViewModel systemInfo);
    }
}
