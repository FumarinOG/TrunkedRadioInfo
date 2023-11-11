using RadioService;
using ServiceCommon;
using System;
using SystemDetailService;
using TalkgroupService;
using TowerFrequencyService;
using TowerService;

namespace Web.Helpers
{
    public static class Factory
    {
        public static FilterDataHelper CreateFilterDataHelper(params string[] recordType) => new FilterDataHelper(recordType);

        public static FilterDataModel CreateFilterData(DateTime? dateFrom = null, DateTime? dateTo = null) =>
            new FilterDataModel
            {
                DateFrom = dateFrom,
                DateTo = dateTo
            };

        public static SystemDetailViewModel CreateSystemModel(string systemID) => new SystemDetailViewModel(systemID);

        public static TalkgroupDetailViewModel CreateTalkgroupModel(string systemID, int talkgroupID) => new TalkgroupDetailViewModel(systemID, talkgroupID);

        public static RadioDetailViewModel CreateRadioModel(string systemID, int radioID) => new RadioDetailViewModel(systemID, radioID);

        public static TowerDetailViewModel CreateTowerModel(string systemID, int towerNumber) => new TowerDetailViewModel(systemID, towerNumber);

        public static TowerFrequencyDetailViewModel CreateTowerFrequencyModel(string systemID, int towerNumber, string frequency) =>
            new TowerFrequencyDetailViewModel(systemID, towerNumber, frequency);
    }
}