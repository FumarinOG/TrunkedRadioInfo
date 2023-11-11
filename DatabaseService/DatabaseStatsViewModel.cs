namespace DatabaseService
{
    public sealed class DatabaseStatsViewModel
    {
        public int ProcessedFilesCount { get; private set; }
        public string ProcessedFilesCountText => $"{ProcessedFilesCount:#,##0}";
        public int RowCount { get; private set; }
        public string RowCountText => $"{RowCount:#,##0}";
        public int SystemsCount { get; private set; }
        public string SystemsCountText => $"{SystemsCount:#,##0}";
        public int TalkgroupsCount { get; private set; }
        public string TalkgroupsCountText => $"{TalkgroupsCount:#,##0}";
        public int TalkgroupHistoryCount { get; private set; }
        public string TalkgroupHistoryCountText => $"{TalkgroupHistoryCount:#,##0}";
        public int RadiosCount { get; private set; }
        public string RadiosCountText => $"{RadiosCount:#,##0}";
        public int RadioHistoryCount { get; private set; }
        public string RadioHistoryCountText => $"{RadioHistoryCount:#,##0}";
        public int TowersCount { get; private set; }
        public string TowersCountText => $"{TowersCount:#,##0}";
        public int TowerFrequenciesCount { get; private set; }
        public string TowerFrequenciesCountText => $"{TowerFrequenciesCount:#,##0}";
        public int TowerFrequencyUsageCount { get; private set; }
        public string TowerFrequencyUsageCountText => $"{TowerFrequencyUsageCount:#,##0}";
        public int TowerTalkgroupsCount { get; private set; }
        public string TowerTalkgroupsCountText => $"{TowerTalkgroupsCount:#,##0}";
        public int TowerRadiosCount { get; private set; }
        public string TowerRadiosCountText => $"{TowerRadiosCount:#,##0}";
        public int TowerTalkgroupRadiosCount { get; private set; }
        public string TowerTalkgroupsRadiosCountText => $"{TowerTalkgroupRadiosCount:#,##0}";
        public int TowerFrequencyTalkgroupsCount { get; private set; }
        public string TowerFrequencyTalkgroupsCountText => $"{TowerFrequencyTalkgroupsCount:#,##0}";
        public int TowerFrequencyRadiosCount { get; private set; }
        public string TowerFrequencyRadiosCountText => $"{TowerFrequencyRadiosCount:#,##0}";

        public DatabaseStatsViewModel(int processedFilesCount, int rowCount, int systemsCount, int talkgroupsCount, int talkgroupHistoryCount, int radiosCount,
            int radioHistoryCount, int towersCount, int towerFrequenciesCount, int towerFrequencyUsageCount, int towerTalkgroupsCount, int towerRadiosCount,
            int towerTalkgroupRadiosCount, int towerFrequencyTalkgroupsCount, int towerFrequencyRadiosCount) =>

            (ProcessedFilesCount, RowCount, SystemsCount, TalkgroupsCount, TalkgroupHistoryCount, RadiosCount, RadioHistoryCount,
            TowersCount, TowerFrequenciesCount, TowerFrequencyUsageCount, TowerTalkgroupsCount, TowerRadiosCount, TowerTalkgroupRadiosCount,
            TowerFrequencyTalkgroupsCount, TowerFrequencyRadiosCount) = (processedFilesCount, rowCount, systemsCount, talkgroupsCount, talkgroupHistoryCount,
            radiosCount, radioHistoryCount, towersCount, towerFrequenciesCount, towerFrequencyUsageCount, towerTalkgroupsCount, towerRadiosCount,
            towerTalkgroupRadiosCount, towerFrequencyTalkgroupsCount, towerFrequencyRadiosCount);
    }
}
