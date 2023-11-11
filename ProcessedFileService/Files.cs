using System.Collections.Generic;

namespace ProcessedFileService
{
    public static class Files
    {
        private const string AFFILIATIONS_FILE_NAME = "Affiliations File|*-Affiliations-*.csv";
        private const string GRANT_LOG_FILE_NAME = "Grant Log File| *-GrantLog-*.csv";
        private const string PATCH_LOG_FILE_NAME = "Patch Log File| *-PatchLog-*.csv";
        private const string RADIOS_FILE_NAME = "Radios File | Radios.txt";
        private const string SYSTEM_FILES = "System Files | *.ini";
        private const string TALKGROUPS_FILE_NAME = "Talkgroups File | Talkgroups.txt";
        private const string TOWER_FILE_NAME = "Tower File | Tower*.txt";

        private const string FILE_TYPE_AFFILIATIONS = "Affiliations";
        private const string FILE_TYPE_GRANT_LOG = "Grant Log";
        private const string FILE_TYPE_PATCH_LOG = "Patch Log";
        private const string FILE_TYPE_RADIOS = "Radios";
        private const string FILE_TYPE_SYSTEM = "System";
        private const string FILE_TYPE_TALKGROUPS = "Talkgroups";
        private const string FILE_TYPE_TOWER = "Tower";

        public static Dictionary<FileTypes, string> FileAssociations { get; } = new Dictionary<FileTypes, string>
        {
            {FileTypes.Affiliations, AFFILIATIONS_FILE_NAME},
            {FileTypes.GrantLog, GRANT_LOG_FILE_NAME},
            {FileTypes.PatchLog, PATCH_LOG_FILE_NAME},
            {FileTypes.Radios, RADIOS_FILE_NAME},
            {FileTypes.System, SYSTEM_FILES},
            {FileTypes.Talkgroups, TALKGROUPS_FILE_NAME},
            {FileTypes.Tower, TOWER_FILE_NAME}
        };

        public static Dictionary<FileTypes, string> FileTypeNames { get; } = new Dictionary<FileTypes, string>
        {
            {FileTypes.Affiliations, FILE_TYPE_AFFILIATIONS},
            {FileTypes.GrantLog, FILE_TYPE_GRANT_LOG},
            {FileTypes.PatchLog, FILE_TYPE_PATCH_LOG},
            {FileTypes.Radios, FILE_TYPE_RADIOS},
            {FileTypes.System, FILE_TYPE_SYSTEM},
            {FileTypes.Talkgroups, FILE_TYPE_TALKGROUPS},
            {FileTypes.Tower, FILE_TYPE_TOWER}
        };
    }
}
