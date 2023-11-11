using System.ComponentModel;

namespace ProcessedFileService
{
    public enum FileTypes
    {
        [Description("Affiliations")]
        Affiliations,
        [Description("Grant Log")]
        GrantLog,
        [Description("Patch Log")]
        PatchLog,
        [Description("Radios")]
        Radios,
        [Description("System")]
        System,
        [Description("Talkgroups")]
        Talkgroups,
        [Description("Tower")]
        Tower
    }

    public enum FileStatus
    {
        [Description("Processing")]
        Processing,
        [Description("Processed")]
        Processed,
        [Description("Not Processed")]
        NotProcessed,
        [Description("Skipped")]
        Skipped,
        [Description("Loading talkgroups")]
        LoadingTalkgroups,
        [Description("Loading radios")]
        LoadingRadios,
        [Description("Loading talkgroup radios")]
        LoadingTalkgroupRadios,
        [Description("Loading tower radios")]
        LoadingTowerRadios,
        [Description("Loading tower talkgroups")]
        LoadingTowerTalkgroups,
        [Description("Loading tower talkgroup radios")]
        LoadingTowerTalkgroupRadios,
        [Description("Loading talkgroup history")]
        LoadingTalkgroupHistory,
        [Description("Loading radio history")]
        LoadingRadioHistory,
        [Description("Loading system info")]
        LoadingSystemInfo,
        [Description("Loading tower")]
        LoadingTower,
        [Description("Loading tower frequencies")]
        LoadingTowerFrequencies,
        [Description("Loading tower frequency radios")]
        LoadingTowerFrequencyRadios,
        [Description("Loading tower frequency talkgroups")]
        LoadingTowerFrequencyTalkgroups
    }
}
