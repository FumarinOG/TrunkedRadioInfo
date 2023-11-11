using AutoMapper;
using DataAccessLibrary;
using ObjectLibrary;
using System;

namespace DataLibrary
{
    public static class Config
    {
        public static IMapper AutoMapperSetup()
        {
            var config = CreateMaps();

            return config.CreateMapper();
        }

        public static MapperConfiguration CreateMaps()
        {
            var config = new MapperConfiguration(cfg =>
            {
                MapDatabaseStatsResult(cfg);
                MapPatches(cfg);
                MapPatchesResult(cfg);
                MapPatchesByDate(cfg);
                MapPatchesSummary(cfg);
                MapProcessedFiles(cfg);
                MapProcessedFilesResult(cfg);
                MapRadios(cfg);
                MapRadioDetails(cfg);
                MapRadioHistory(cfg);
                MapRadioHistoryResult(cfg);
                MapRadioNameResult(cfg);
                MapRadioTowerTalkgroupsResult(cfg);
                MapSystems(cfg);
                MapSystemsResult(cfg);
                MapTalkgroups(cfg);
                MapTalkgroupsDetail(cfg);
                MapTalkgroupsDetailWithTowers(cfg);
                MapTalkgroupHistory(cfg);
                MapTalkgroupHistoryResult(cfg);
                MapTalkgroupHistorySystemResult(cfg);
                MapTalkgroupRadios(cfg);
                MapTalkgroupRadiosResult(cfg);
                MapTalkgroupRadiosWithDates(cfg);
                MapTalkgroupTowerRadiosResult(cfg);
                MapTowers(cfg);
                MapTowersResult(cfg);
                MapTowerFrequencies(cfg);
                MapTowerFrequenciesResult(cfg);
                MapTowerFrequencyRadios(cfg);
                MapTowerFrequencyRadioResult(cfg);
                MapTowerFrequencySummary(cfg);
                MapTowerFrequencyTalkgroups(cfg);
                MapTowerFrequencyTalkgroupResult(cfg);
                MapTowerFrequencyUsage(cfg);
                MapTowerList(cfg);
                MapTowerNeighbors(cfg);
                MapTowerNeighborsResult(cfg);
                MapTowerRadios(cfg);
                MapTowerRadiosResult(cfg);
                MapTowerTables(cfg);
                MapTowerTalkgroups(cfg);
                MapTowerTalkgroupsResult(cfg);
                MapTowerTalkgroupImportResult(cfg);
                MapTowerTalkgroupRadios(cfg);
                MapTowerTalkgroupRadiosResult(cfg);
                MapTowerTalkgroupRadiosWithDates(cfg);
            });

            return config;
        }

        private static void MapDatabaseStatsResult(IProfileExpression config) =>
            config.CreateMap<DatabaseStats_Result, DatabaseStat>();

        private static void MapPatches(IProfileExpression config) =>
            config.CreateMap<Patches, Patch>()
               .ForMember(dest => dest.TowerNumber, opt => opt.MapFrom(src => src.TowerID))
               .ForMember(dest => dest.TowerName, opt => opt.Ignore())
               .ForMember(dest => dest.FromTalkgroupName, opt => opt.Ignore())
               .ForMember(dest => dest.ToTalkgroupName, opt => opt.Ignore())
               .ForMember(dest => dest.IsNew, opt => opt.Ignore())
               .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
               .AfterMap((src, dest) => dest.IsNew = false)
               .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapPatchesResult(IProfileExpression config) => 
            config.CreateMap<Patches_Result, Patch>()
                .ForMember(dest => dest.TowerName, opt => opt.MapFrom(src => src.TowerDescription))
                .ForMember(dest => dest.FromTalkgroupName, opt => opt.MapFrom(src => src.FromTalkgroupDescription))
                .ForMember(dest => dest.ToTalkgroupName, opt => opt.MapFrom(src => src.ToTalkgroupDescription))
                .ForMember(dest => dest.HitCount, opt => opt.MapFrom(src => src != null ? src.HitCount : 0))
                .ForMember(dest => dest.ID, opt => opt.Ignore())
                .ForMember(dest => dest.SystemID, opt => opt.Ignore())
                .ForMember(dest => dest.Date, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .ForMember(dest => dest.LastModified, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapPatchesByDate(IProfileExpression config) => 
            config.CreateMap<PatchesByDate_Result, Patch>()
                .ForMember(dest => dest.FromTalkgroupName, opt => opt.MapFrom(src => src.FromTalkgroupDescription))
                .ForMember(dest => dest.ToTalkgroupName, opt => opt.MapFrom(src => src.ToTalkgroupDescription))
                .ForMember(dest => dest.TowerName, opt => opt.MapFrom(src => src.TowerDescription))
                .ForMember(dest => dest.HitCount, opt => opt.MapFrom(src => src != null ? src.HitCount : 0))
                .ForMember(dest => dest.ID, opt => opt.Ignore())
                .ForMember(dest => dest.SystemID, opt => opt.Ignore())
                .ForMember(dest => dest.FirstSeen, opt => opt.Ignore())
                .ForMember(dest => dest.LastSeen, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .ForMember(dest => dest.LastModified, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapPatchesSummary(IProfileExpression config) =>
            config.CreateMap<PatchesSummary_Result, Patch>()
                .ForMember(dest => dest.FromTalkgroupName, opt => opt.MapFrom(src => src.FromTalkgroupDescription))
                .ForMember(dest => dest.ToTalkgroupName, opt => opt.MapFrom(src => src.ToTalkgroupDescription))
                .ForMember(dest => dest.HitCount, opt => opt.MapFrom(src => src != null ? src.HitCount : 0))
                .ForMember(dest => dest.ID, opt => opt.Ignore())
                .ForMember(dest => dest.SystemID, opt => opt.Ignore())
                .ForMember(dest => dest.TowerNumber, opt => opt.Ignore())
                .ForMember(dest => dest.TowerName, opt => opt.Ignore())
                .ForMember(dest => dest.Date, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .ForMember(dest => dest.LastModified, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapProcessedFiles(IProfileExpression config) =>
            config.CreateMap<ProcessedFiles, ProcessedFile>()
                .ForMember(dest => dest.LongFileName, opt => opt.MapFrom(src => src.FileName))
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapProcessedFilesResult(IProfileExpression config) =>
            config.CreateMap<ProcessedFiles_Result, ProcessedFile>()
                .ForSourceMember(src => src.RecordCount, opt => opt.DoNotValidate())
                .ForMember(dest => dest.LongFileName, opt => opt.MapFrom(src => src.FileName))
                .ForMember(dest => dest.Type, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore());

        private static void MapRadios(IProfileExpression config) =>
            config.CreateMap<Radios, Radio>()
                .ForMember(dest => dest.AffiliationCount, opt => opt.Ignore())
                .ForMember(dest => dest.DeniedCount, opt => opt.Ignore())
                .ForMember(dest => dest.VoiceGrantCount, opt => opt.Ignore())
                .ForMember(dest => dest.EmergencyVoiceGrantCount, opt => opt.Ignore())
                .ForMember(dest => dest.EncryptedVoiceGrantCount, opt => opt.Ignore())
                .ForMember(dest => dest.DataCount, opt => opt.Ignore())
                .ForMember(dest => dest.PrivateDataCount, opt => opt.Ignore())
                .ForMember(dest => dest.CWIDCount, opt => opt.Ignore())
                .ForMember(dest => dest.AlertCount, opt => opt.Ignore())
                .ForMember(dest => dest.Date, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapRadioDetails(IProfileExpression config) =>
            config.CreateMap<RadioDetails_Result, Radio>()
                .ForMember(dest => dest.ID, opt => opt.Ignore())
                .ForMember(dest => dest.SystemID, opt => opt.Ignore())
                .ForMember(dest => dest.LastSeenProgram, opt => opt.Ignore())
                .ForMember(dest => dest.LastSeenProgramUnix, opt => opt.Ignore())
                .ForMember(dest => dest.FGColor, opt => opt.Ignore())
                .ForMember(dest => dest.BGColor, opt => opt.Ignore())
                .ForMember(dest => dest.HitCount, opt => opt.Ignore())
                .ForMember(dest => dest.PrivateDataCount, opt => opt.Ignore())
                .ForMember(dest => dest.CWIDCount, opt => opt.Ignore())
                .ForMember(dest => dest.AlertCount, opt => opt.Ignore())
                .ForMember(dest => dest.Date, opt => opt.Ignore())
                .ForMember(dest => dest.LastModified, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .ForMember(dest => dest.ID, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapRadioHistory(IProfileExpression config) =>
            config.CreateMap<DataAccessLibrary.RadioHistory, ObjectLibrary.RadioHistory>()
                .ForMember(dest => dest.RadioIDKey, opt => opt.Ignore())
                .ForMember(dest => dest.HitCount, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapRadioHistoryResult(IProfileExpression config) =>
            config.CreateMap<RadioHistory_Result, ObjectLibrary.RadioHistory>()
                .ForMember(dest => dest.ID, opt => opt.Ignore())
                .ForMember(dest => dest.SystemID, opt => opt.Ignore())
                .ForMember(dest => dest.RadioID, opt => opt.Ignore())
                .ForMember(dest => dest.RadioIDKey, opt => opt.Ignore())
                .ForMember(dest => dest.LastModified, opt => opt.Ignore())
                .ForMember(dest => dest.HitCount, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapRadioNameResult(IProfileExpression config) =>
            config.CreateMap<RadiosNames_Result, (int radioID, string name)>()
                .ForMember(dest => dest.radioID, opt => opt.MapFrom(src => src.RadioID))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Description));

        private static void MapRadioTowerTalkgroupsResult(IProfileExpression config) =>
            config.CreateMap<RadioTowerTalkgroups_Result, TowerTalkgroupRadio>()
                .ForMember(dest => dest.TowerNumber, opt => opt.Ignore())
                .ForMember(dest => dest.TowerName, opt => opt.Ignore())
                .ForMember(dest => dest.TalkgroupID, opt => opt.Ignore())
                .ForMember(dest => dest.TalkgroupName, opt => opt.Ignore())
                .ForMember(dest => dest.RadioName, opt => opt.MapFrom(src => src.RadioDescription))
                .ForMember(dest => dest.AffiliationCount, opt => opt.MapFrom(src => src != null ? src.AffiliationCount : 0))
                .ForMember(dest => dest.DeniedCount, opt => opt.MapFrom(src => src != null ? src.DeniedCount : 0))
                .ForMember(dest => dest.VoiceGrantCount, opt => opt.MapFrom(src => src != null ? src.VoiceGrantCount : 0))
                .ForMember(dest => dest.EmergencyVoiceGrantCount, opt => opt.MapFrom(src => src != null ? src.EmergencyVoiceGrantCount : 0))
                .ForMember(dest => dest.EncryptedVoiceGrantCount, opt => opt.MapFrom(src => src != null ? src.EncryptedVoiceGrantCount : 0))
                .ForMember(dest => dest.DataCount, opt => opt.MapFrom(src => src != null ? src.DataCount : 0))
                .ForMember(dest => dest.PrivateDataCount, opt => opt.Ignore())
                .ForMember(dest => dest.AlertCount, opt => opt.MapFrom(src => src != null ? src.AlertCount : 0))
                .ForMember(dest => dest.FirstSeen, opt => opt.MapFrom(src => src != null ? src.FirstSeen : DateTime.Now))
                .ForMember(dest => dest.FirstSeen, opt => opt.MapFrom(src => src != null ? src.FirstSeen : DateTime.Now))
                .ForMember(dest => dest.ID, opt => opt.Ignore())
                .ForMember(dest => dest.SystemID, opt => opt.Ignore())
                .ForMember(dest => dest.TowerName, opt => opt.Ignore())
                .ForMember(dest => dest.Date, opt => opt.Ignore())
                .ForMember(dest => dest.CWIDCount, opt => opt.Ignore())
                .ForMember(dest => dest.HitCount, opt => opt.Ignore())
                .ForMember(dest => dest.LastModified, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapSystems(IProfileExpression config) =>
            config.CreateMap<Systems, SystemInfo>()
                .ForMember(dest => dest.TalkgroupCount, opt => opt.Ignore())
                .ForMember(dest => dest.RadioCount, opt => opt.Ignore())
                .ForMember(dest => dest.TowerCount, opt => opt.Ignore())
                .ForMember(dest => dest.RowCount, opt => opt.Ignore())
                .ForMember(dest => dest.HitCount, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .ForMember(dest => dest.Towers, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapSystemsResult(IProfileExpression config) =>
            config.CreateMap<Systems_Result, SystemInfo>()
                .ForMember(dest => dest.SystemIDDecimal, opt => opt.Ignore())
                .ForMember(dest => dest.WACN, opt => opt.Ignore())
                .ForMember(dest => dest.HitCount, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .ForMember(dest => dest.Towers, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapTalkgroups(IProfileExpression config) =>
            config.CreateMap<Talkgroups, Talkgroup>()
                .ForMember(dest => dest.AffiliationCount, opt => opt.Ignore())
                .ForMember(dest => dest.DeniedCount, opt => opt.Ignore())
                .ForMember(dest => dest.VoiceGrantCount, opt => opt.Ignore())
                .ForMember(dest => dest.EmergencyVoiceGrantCount, opt => opt.Ignore())
                .ForMember(dest => dest.EncryptedVoiceGrantCount, opt => opt.Ignore())
                .ForMember(dest => dest.DataCount, opt => opt.Ignore())
                .ForMember(dest => dest.PrivateDataCount, opt => opt.Ignore())
                .ForMember(dest => dest.CWIDCount, opt => opt.Ignore())
                .ForMember(dest => dest.AlertCount, opt => opt.Ignore())
                .ForMember(dest => dest.PatchCount, opt => opt.Ignore())
                .ForMember(dest => dest.Date, opt => opt.Ignore())
                .ForMember(dest => dest.Towers, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapTalkgroupsDetail(IProfileExpression config) =>
            config.CreateMap<TalkgroupDetails_Result, Talkgroup>()
                .ForSourceMember(src => src.RecordCount, opt => opt.DoNotValidate())
                .ForMember(dest => dest.SystemID, opt => opt.Ignore())
                .ForMember(dest => dest.Priority, opt => opt.Ignore())
                .ForMember(dest => dest.LastSeenProgram, opt => opt.Ignore())
                .ForMember(dest => dest.LastSeenProgramUnix, opt => opt.Ignore())
                .ForMember(dest => dest.FirstSeenProgram, opt => opt.Ignore())
                .ForMember(dest => dest.FirstSeenProgramUnix, opt => opt.Ignore())
                .ForMember(dest => dest.FGColor, opt => opt.Ignore())
                .ForMember(dest => dest.BGColor, opt => opt.Ignore())
                .ForMember(dest => dest.IgnoreEmergencySignal, opt => opt.Ignore())
                .ForMember(dest => dest.HitCount, opt => opt.Ignore())
                .ForMember(dest => dest.DataCount, opt => opt.Ignore())
                .ForMember(dest => dest.PrivateDataCount, opt => opt.Ignore())
                .ForMember(dest => dest.CWIDCount, opt => opt.Ignore())
                .ForMember(dest => dest.AlertCount, opt => opt.Ignore())
                .ForMember(dest => dest.HitCountProgram, opt => opt.Ignore())
                .ForMember(dest => dest.Date, opt => opt.Ignore())
                .ForMember(dest => dest.Towers, opt => opt.Ignore())
                .ForMember(dest => dest.LastModified, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .ForMember(dest => dest.ID, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapTalkgroupsDetailWithTowers(IProfileExpression config) =>
            config.CreateMap<TalkgroupDetailsWithTowers_Result, Talkgroup>()
                .ForSourceMember(src => src.RecordCount, opt => opt.DoNotValidate())
                .ForMember(dest => dest.SystemID, opt => opt.Ignore())
                .ForMember(dest => dest.Priority, opt => opt.Ignore())
                .ForMember(dest => dest.LastSeenProgram, opt => opt.Ignore())
                .ForMember(dest => dest.LastSeenProgramUnix, opt => opt.Ignore())
                .ForMember(dest => dest.FirstSeenProgram, opt => opt.Ignore())
                .ForMember(dest => dest.FirstSeenProgramUnix, opt => opt.Ignore())
                .ForMember(dest => dest.FGColor, opt => opt.Ignore())
                .ForMember(dest => dest.BGColor, opt => opt.Ignore())
                .ForMember(dest => dest.IgnoreEmergencySignal, opt => opt.Ignore())
                .ForMember(dest => dest.HitCount, opt => opt.Ignore())
                .ForMember(dest => dest.DataCount, opt => opt.Ignore())
                .ForMember(dest => dest.PrivateDataCount, opt => opt.Ignore())
                .ForMember(dest => dest.CWIDCount, opt => opt.Ignore())
                .ForMember(dest => dest.AlertCount, opt => opt.Ignore())
                .ForMember(dest => dest.HitCountProgram, opt => opt.Ignore())
                .ForMember(dest => dest.Date, opt => opt.Ignore())
                .ForMember(dest => dest.LastModified, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .ForMember(dest => dest.ID, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapTalkgroupHistory(IProfileExpression config) =>
            config.CreateMap<DataAccessLibrary.TalkgroupHistory, ObjectLibrary.TalkgroupHistory>()
                .ForMember(dest => dest.TalkgroupIDKey, opt => opt.Ignore())
                .ForMember(dest => dest.HitCount, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapTalkgroupHistoryResult(IProfileExpression config) =>
            config.CreateMap<TalkgroupHistory_Result, ObjectLibrary.TalkgroupHistory>()
                .ForMember(dest => dest.ID, opt => opt.Ignore())
                .ForMember(dest => dest.SystemID, opt => opt.Ignore())
                .ForMember(dest => dest.TalkgroupID, opt => opt.Ignore())
                .ForMember(dest => dest.TalkgroupIDKey, opt => opt.Ignore())
                .ForMember(dest => dest.LastModified, opt => opt.Ignore())
                .ForMember(dest => dest.HitCount, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapTalkgroupHistorySystemResult(IProfileExpression config) =>
            config.CreateMap<TalkgroupHistoryForSystem_Result, ObjectLibrary.TalkgroupHistory>()
                .ForMember(dest => dest.HitCount, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapTalkgroupRadios(IProfileExpression config) =>
            config.CreateMap<TalkgroupRadios, TalkgroupRadio>()
                .ForMember(dest => dest.TalkgroupName, opt => opt.Ignore())
                .ForMember(dest => dest.RadioName, opt => opt.Ignore())
                .ForMember(dest => dest.HitCount, opt => opt.Ignore())
                .ForMember(dest => dest.CWIDCount, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapTalkgroupRadiosResult(IProfileExpression config) =>
            config.CreateMap<TalkgroupRadios_Result, TalkgroupRadio>()
                .ForMember(dest => dest.ID, opt => opt.Ignore())
                .ForMember(dest => dest.SystemID, opt => opt.Ignore())
                .ForMember(dest => dest.TalkgroupName, opt => opt.MapFrom(src => src.TalkgroupDescription))
                .ForMember(dest => dest.RadioName, opt => opt.MapFrom(src => src.RadioDescription))
                .ForMember(dest => dest.Date, opt => opt.Ignore())
                .ForMember(dest => dest.HitCount, opt => opt.Ignore())
                .ForMember(dest => dest.CWIDCount, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .ForMember(dest => dest.LastModified, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapTalkgroupRadiosWithDates(IProfileExpression config) =>
            config.CreateMap<TalkgroupRadiosWithDates_Result, TalkgroupRadio>()
                .ForMember(dest => dest.TalkgroupName, opt => opt.MapFrom(src => src.TalkgroupDescription))
                .ForMember(dest => dest.RadioName, opt => opt.MapFrom(src => src.RadioDescription))
                .ForMember(dest => dest.SystemID, opt => opt.Ignore())
                .ForMember(dest => dest.ID, opt => opt.Ignore())
                .ForMember(dest => dest.HitCount, opt => opt.Ignore())
                .ForMember(dest => dest.CWIDCount, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .ForMember(dest => dest.LastModified, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapTalkgroupTowerRadiosResult(IProfileExpression config) =>
            config.CreateMap<TalkgroupTowerRadios_Result, TowerTalkgroupRadio>()
                .ForMember(dest => dest.TowerNumber, opt => opt.Ignore())
                .ForMember(dest => dest.TowerName, opt => opt.Ignore())
                .ForMember(dest => dest.RadioID, opt => opt.Ignore())
                .ForMember(dest => dest.RadioName, opt => opt.Ignore())
                .ForMember(dest => dest.TalkgroupName, opt => opt.MapFrom(src => src.TalkgroupDescription))
                .ForMember(dest => dest.AffiliationCount, opt => opt.MapFrom(src => src != null ? src.AffiliationCount : 0))
                .ForMember(dest => dest.DeniedCount, opt => opt.MapFrom(src => src != null ? src.DeniedCount : 0))
                .ForMember(dest => dest.VoiceGrantCount, opt => opt.MapFrom(src => src != null ? src.VoiceGrantCount : 0))
                .ForMember(dest => dest.EmergencyVoiceGrantCount, opt => opt.MapFrom(src => src != null ? src.EmergencyVoiceGrantCount : 0))
                .ForMember(dest => dest.EncryptedVoiceGrantCount, opt => opt.MapFrom(src => src != null ? src.EncryptedVoiceGrantCount : 0))
                .ForMember(dest => dest.DataCount, opt => opt.MapFrom(src => src != null ? src.DataCount : 0))
                .ForMember(dest => dest.PrivateDataCount, opt => opt.Ignore())
                .ForMember(dest => dest.AlertCount, opt => opt.MapFrom(src => src != null ? src.AlertCount : 0))
                .ForMember(dest => dest.FirstSeen, opt => opt.MapFrom(src => src != null ? src.FirstSeen : DateTime.Now))
                .ForMember(dest => dest.FirstSeen, opt => opt.MapFrom(src => src != null ? src.FirstSeen : DateTime.Now))
                .ForMember(dest => dest.ID, opt => opt.Ignore())
                .ForMember(dest => dest.SystemID, opt => opt.Ignore())
                .ForMember(dest => dest.TowerName, opt => opt.Ignore())
                .ForMember(dest => dest.Date, opt => opt.Ignore())
                .ForMember(dest => dest.CWIDCount, opt => opt.Ignore())
                .ForMember(dest => dest.HitCount, opt => opt.Ignore())
                .ForMember(dest => dest.LastModified, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapTowers(IProfileExpression config) =>
            config.CreateMap<Towers, Tower>()
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapTowersResult(IProfileExpression config) =>
            config.CreateMap<Towers_Result, Tower>()
                .ForMember(dest => dest.ID, opt => opt.Ignore())
                .ForMember(dest => dest.SystemID, opt => opt.Ignore())
                .ForMember(dest => dest.TowerNumberHex, opt => opt.Ignore())
                .ForMember(dest => dest.WACN, opt => opt.Ignore())
                .ForMember(dest => dest.ControlCapabilities, opt => opt.Ignore())
                .ForMember(dest => dest.Flavor, opt => opt.Ignore())
                .ForMember(dest => dest.CallSigns, opt => opt.Ignore())
                .ForMember(dest => dest.TimeStamp, opt => opt.Ignore())
                .ForMember(dest => dest.LastModified, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore());

        private static void MapTowerFrequencies(IProfileExpression config) =>
            config.CreateMap<TowerFrequencies, TowerFrequency>()
                .ForMember(dest => dest.Date, opt => opt.Ignore())
                .ForMember(dest => dest.AffiliationCount, opt => opt.Ignore())
                .ForMember(dest => dest.DeniedCount, opt => opt.Ignore())
                .ForMember(dest => dest.VoiceGrantCount, opt => opt.Ignore())
                .ForMember(dest => dest.EmergencyVoiceGrantCount, opt => opt.Ignore())
                .ForMember(dest => dest.EncryptedVoiceGrantCount, opt => opt.Ignore())
                .ForMember(dest => dest.DataCount, opt => opt.Ignore())
                .ForMember(dest => dest.PrivateDataCount, opt => opt.Ignore())
                .ForMember(dest => dest.CWIDCount, opt => opt.Ignore())
                .ForMember(dest => dest.AlertCount, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapTowerFrequenciesResult(IProfileExpression config) =>
            config.CreateMap<TowerFrequencies_Result, TowerFrequency>()
                .ForMember(dest => dest.ID, opt => opt.Ignore())
                .ForMember(dest => dest.SystemID, opt => opt.Ignore())
                .ForMember(dest => dest.TowerID, opt => opt.Ignore())
                .ForMember(dest => dest.InputChannel, opt => opt.Ignore())
                .ForMember(dest => dest.InputFrequency, opt => opt.Ignore())
                .ForMember(dest => dest.InputExplicit, opt => opt.Ignore())
                .ForMember(dest => dest.Date, opt => opt.Ignore())
                .ForMember(dest => dest.LastModified, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapTowerFrequencyRadios(IProfileExpression config) =>
           config.CreateMap<TowerFrequencyRadios, TowerFrequencyRadio>()
                .ForMember(dest => dest.TowerName, opt => opt.Ignore())
                .ForMember(dest => dest.RadioName, opt => opt.Ignore())
                .ForMember(dest => dest.HitCount, opt => opt.Ignore())
                .ForMember(dest => dest.CWIDCount, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapTowerFrequencyRadioResult(IProfileExpression config) =>
            config.CreateMap<TowerFrequencyRadios_Result, TowerFrequencyRadio>()
                .ForMember(dest => dest.ID, opt => opt.Ignore())
                .ForMember(dest => dest.SystemID, opt => opt.Ignore())
                .ForMember(dest => dest.RadioName, opt => opt.MapFrom(src => src.RadioDescription))
                .ForMember(dest => dest.TowerID, opt => opt.Ignore())
                .ForMember(dest => dest.TowerName, opt => opt.Ignore())
                .ForMember(dest => dest.Frequency, opt => opt.Ignore())
                .ForMember(dest => dest.Date, opt => opt.Ignore())
                .ForMember(dest => dest.CWIDCount, opt => opt.Ignore())
                .ForMember(dest => dest.HitCount, opt => opt.Ignore())
                .ForMember(dest => dest.LastModified, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore());

        private static void MapTowerFrequencySummary(IProfileExpression config) =>
            config.CreateMap<TowerFrequencySummary_Result, TowerFrequency>()
                .ForMember(dest => dest.ID, opt => opt.Ignore())
                .ForMember(dest => dest.SystemID, opt => opt.Ignore())
                .ForMember(dest => dest.TowerID, opt => opt.Ignore())
                .ForMember(dest => dest.InputExplicit, opt => opt.Ignore())
                .ForMember(dest => dest.Date, opt => opt.Ignore())
                .ForMember(dest => dest.HitCount, opt => opt.Ignore())
                .ForMember(dest => dest.LastModified, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapTowerFrequencyTalkgroups(IProfileExpression config) =>
           config.CreateMap<TowerFrequencyTalkgroups, TowerFrequencyTalkgroup>()
                .ForMember(dest => dest.TowerName, opt => opt.Ignore())
                .ForMember(dest => dest.TalkgroupName, opt => opt.Ignore())
                .ForMember(dest => dest.HitCount, opt => opt.Ignore())
                .ForMember(dest => dest.CWIDCount, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapTowerFrequencyTalkgroupResult(IProfileExpression config) =>
            config.CreateMap<TowerFrequencyTalkgroups_Result, TowerFrequencyTalkgroup>()
                .ForMember(dest => dest.ID, opt => opt.Ignore())
                .ForMember(dest => dest.SystemID, opt => opt.Ignore())
                .ForMember(dest => dest.TalkgroupName, opt => opt.MapFrom(src => src.TalkgroupDescription))
                .ForMember(dest => dest.TowerID, opt => opt.Ignore())
                .ForMember(dest => dest.TowerName, opt => opt.Ignore())
                .ForMember(dest => dest.Frequency, opt => opt.Ignore())
                .ForMember(dest => dest.Date, opt => opt.Ignore())
                .ForMember(dest => dest.CWIDCount, opt => opt.Ignore())
                .ForMember(dest => dest.HitCount, opt => opt.Ignore())
                .ForMember(dest => dest.LastModified, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore());

        private static void MapTowerFrequencyUsage(IProfileExpression config) =>
            config.CreateMap<TowerFrequencyUsage_Result, ObjectLibrary.TowerFrequencyUsage>()
                .ForMember(dest => dest.ID, opt => opt.Ignore())
                .ForMember(dest => dest.SystemID, opt => opt.Ignore())
                .ForMember(dest => dest.TowerID, opt => opt.Ignore())
                .ForMember(dest => dest.LastModified, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapTowerList(IProfileExpression config)
        {
            config.CreateMap<TowerList_Result, TowerTalkgroup>()
                .ForMember(dest => dest.ID, opt => opt.Ignore())
                .ForMember(dest => dest.SystemID, opt => opt.Ignore())
                .ForMember(dest => dest.TowerName, opt => opt.MapFrom(src => src.TowerDescription))
                .ForMember(dest => dest.TalkgroupID, opt => opt.Ignore())
                .ForMember(dest => dest.TalkgroupName, opt => opt.Ignore())
                .ForMember(dest => dest.AffiliationCount, opt => opt.Ignore())
                .ForMember(dest => dest.DeniedCount, opt => opt.Ignore())
                .ForMember(dest => dest.VoiceGrantCount, opt => opt.Ignore())
                .ForMember(dest => dest.EmergencyVoiceGrantCount, opt => opt.Ignore())
                .ForMember(dest => dest.EncryptedVoiceGrantCount, opt => opt.Ignore())
                .ForMember(dest => dest.DataCount, opt => opt.Ignore())
                .ForMember(dest => dest.PrivateDataCount, opt => opt.Ignore())
                .ForMember(dest => dest.AlertCount, opt => opt.Ignore())
                .ForMember(dest => dest.FirstSeen, opt => opt.Ignore())
                .ForMember(dest => dest.LastSeen, opt => opt.Ignore())
                .ForMember(dest => dest.Date, opt => opt.Ignore())
                .ForMember(dest => dest.HitCount, opt => opt.Ignore())
                .ForMember(dest => dest.CWIDCount, opt => opt.Ignore())
                .ForMember(dest => dest.LastModified, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

            config.CreateMap<TowerList_Result, TowerRadio>()
                .ForMember(dest => dest.ID, opt => opt.Ignore())
                .ForMember(dest => dest.SystemID, opt => opt.Ignore())
                .ForMember(dest => dest.TowerName, opt => opt.MapFrom(src => src.TowerDescription))
                .ForMember(dest => dest.RadioID, opt => opt.Ignore())
                .ForMember(dest => dest.RadioName, opt => opt.Ignore())
                .ForMember(dest => dest.AffiliationCount, opt => opt.Ignore())
                .ForMember(dest => dest.DeniedCount, opt => opt.Ignore())
                .ForMember(dest => dest.VoiceGrantCount, opt => opt.Ignore())
                .ForMember(dest => dest.EmergencyVoiceGrantCount, opt => opt.Ignore())
                .ForMember(dest => dest.EncryptedVoiceGrantCount, opt => opt.Ignore())
                .ForMember(dest => dest.DataCount, opt => opt.Ignore())
                .ForMember(dest => dest.PrivateDataCount, opt => opt.Ignore())
                .ForMember(dest => dest.AlertCount, opt => opt.Ignore())
                .ForMember(dest => dest.FirstSeen, opt => opt.Ignore())
                .ForMember(dest => dest.LastSeen, opt => opt.Ignore())
                .ForMember(dest => dest.Date, opt => opt.Ignore())
                .ForMember(dest => dest.HitCount, opt => opt.Ignore())
                .ForMember(dest => dest.CWIDCount, opt => opt.Ignore())
                .ForMember(dest => dest.LastModified, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);
        }

        private static void MapTowerNeighbors(IProfileExpression config) =>
            config.CreateMap<TowerNeighbors, TowerNeighbor>()
                .ForMember(dest => dest.TowerNumber, opt => opt.MapFrom(src => src.TowerID))
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapTowerNeighborsResult(IProfileExpression config) =>
            config.CreateMap<TowerNeighbors_Result, TowerNeighbor>()
                .ForMember(dest => dest.ID, opt => opt.Ignore())
                .ForMember(dest => dest.SystemID, opt => opt.Ignore())
                .ForMember(dest => dest.TowerNumber, opt => opt.Ignore())
                .ForMember(dest => dest.NeighborSystemID, opt => opt.Ignore())
                .ForMember(dest => dest.NeighborTowerID, opt => opt.MapFrom(src => src.NeighborTowerNumber))
                .ForMember(dest => dest.NeighborTowerNumberHex, opt => opt.Ignore())
                .ForMember(dest => dest.NeighborChannel, opt => opt.MapFrom(src => src.NeighborFrequency))
                .ForMember(dest => dest.LastModified, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapTowerRadios(IProfileExpression config) =>
            config.CreateMap<TowerRadios, TowerRadio>()
                .ForMember(dest => dest.TowerNumber, opt => opt.MapFrom(src => src.TowerID))
                .ForMember(dest => dest.TowerName, opt => opt.Ignore())
                .ForMember(dest => dest.RadioName, opt => opt.Ignore())
                .ForMember(dest => dest.HitCount, opt => opt.Ignore())
                .ForMember(dest => dest.CWIDCount, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapTowerRadiosResult(IProfileExpression config) =>
            config.CreateMap<TowerRadios_Result, TowerRadio>()
                .ForMember(dest => dest.TowerName, opt => opt.MapFrom(src => src.TowerDescription))
                .ForMember(dest => dest.RadioName, opt => opt.MapFrom(src => src.RadioDescription))
                .ForMember(dest => dest.AffiliationCount, opt => opt.MapFrom(src => src != null ? src.AffiliationCount : 0))
                .ForMember(dest => dest.DeniedCount, opt => opt.MapFrom(src => src != null ? src.DeniedCount : 0))
                .ForMember(dest => dest.VoiceGrantCount, opt => opt.MapFrom(src => src != null ? src.VoiceGrantCount : 0))
                .ForMember(dest => dest.EmergencyVoiceGrantCount, opt => opt.MapFrom(src => src != null ? src.EmergencyVoiceGrantCount : 0))
                .ForMember(dest => dest.EncryptedVoiceGrantCount, opt => opt.MapFrom(src => src != null ? src.EncryptedVoiceGrantCount : 0))
                .ForMember(dest => dest.DataCount, opt => opt.MapFrom(src => src != null ? src.DataCount : 0))
                .ForMember(dest => dest.PrivateDataCount, opt => opt.MapFrom(src => src != null ? src.PrivateDataCount : 0))
                .ForMember(dest => dest.AlertCount, opt => opt.MapFrom(src => src != null ? src.AlertCount : 0))
                .ForMember(dest => dest.FirstSeen, opt => opt.MapFrom(src => src != null ? src.FirstSeen : DateTime.Now))
                .ForMember(dest => dest.FirstSeen, opt => opt.MapFrom(src => src != null ? src.FirstSeen : DateTime.Now))
                .ForMember(dest => dest.ID, opt => opt.Ignore())
                .ForMember(dest => dest.SystemID, opt => opt.Ignore())
                .ForMember(dest => dest.Date, opt => opt.Ignore())
                .ForMember(dest => dest.HitCount, opt => opt.Ignore())
                .ForMember(dest => dest.CWIDCount, opt => opt.Ignore())
                .ForMember(dest => dest.LastModified, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapTowerTables(IProfileExpression config) =>
            config.CreateMap<TowerTables, TowerTable>()
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapTowerTalkgroups(IProfileExpression config) =>
            config.CreateMap<TowerTalkgroups, TowerTalkgroup>()
                .ForMember(dest => dest.TowerNumber, opt => opt.MapFrom(src => src.TowerID))
                .ForMember(dest => dest.TowerName, opt => opt.Ignore())
                .ForMember(dest => dest.TalkgroupName, opt => opt.Ignore())
                .ForMember(dest => dest.HitCount, opt => opt.Ignore())
                .ForMember(dest => dest.CWIDCount, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapTowerTalkgroupsResult(IProfileExpression config) =>
            config.CreateMap<TowerTalkgroups_Result, TowerTalkgroup>()
                .ForMember(dest => dest.TowerName, opt => opt.MapFrom(src => src.TowerDescription))
                .ForMember(dest => dest.TalkgroupName, opt => opt.MapFrom(src => src.TalkgroupDescription))
                .ForMember(dest => dest.AffiliationCount, opt => opt.MapFrom(src => src != null ? src.AffiliationCount : 0))
                .ForMember(dest => dest.DeniedCount, opt => opt.MapFrom(src => src != null ? src.DeniedCount : 0))
                .ForMember(dest => dest.VoiceGrantCount, opt => opt.MapFrom(src => src != null ? src.VoiceGrantCount : 0))
                .ForMember(dest => dest.EmergencyVoiceGrantCount, opt => opt.MapFrom(src => src != null ? src.EmergencyVoiceGrantCount : 0))
                .ForMember(dest => dest.EncryptedVoiceGrantCount, opt => opt.MapFrom(src => src != null ? src.EncryptedVoiceGrantCount : 0))
                .ForMember(dest => dest.DataCount, opt => opt.MapFrom(src => src != null ? src.DataCount : 0))
                .ForMember(dest => dest.PrivateDataCount, opt => opt.MapFrom(src => src != null ? src.PrivateDataCount : 0))
                .ForMember(dest => dest.AlertCount, opt => opt.MapFrom(src => src != null ? src.AlertCount : 0))
                .ForMember(dest => dest.FirstSeen, opt => opt.MapFrom(src => src != null ? src.FirstSeen : DateTime.Now))
                .ForMember(dest => dest.FirstSeen, opt => opt.MapFrom(src => src != null ? src.FirstSeen : DateTime.Now))
                .ForMember(dest => dest.ID, opt => opt.Ignore())
                .ForMember(dest => dest.SystemID, opt => opt.Ignore())
                .ForMember(dest => dest.Date, opt => opt.Ignore())
                .ForMember(dest => dest.HitCount, opt => opt.Ignore())
                .ForMember(dest => dest.CWIDCount, opt => opt.Ignore())
                .ForMember(dest => dest.LastModified, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapTowerTalkgroupImportResult(IProfileExpression config) =>
            config.CreateMap<TowerTalkgroupsImport_Result, TowerTalkgroup>()
                .ForMember(dest => dest.TowerNumber, opt => opt.MapFrom(src => src.TowerID))
                .ForMember(dest => dest.TowerName, opt => opt.Ignore())
                .ForMember(dest => dest.TalkgroupName, opt => opt.Ignore())
                .ForMember(dest => dest.HitCount, opt => opt.Ignore())
                .ForMember(dest => dest.CWIDCount, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapTowerTalkgroupRadios(IProfileExpression config) =>
            config.CreateMap<TowerTalkgroupRadios, TowerTalkgroupRadio>()
                .ForMember(dest => dest.SystemID, opt => opt.MapFrom(src => src.SystemID))
                .ForMember(dest => dest.TowerNumber, opt => opt.MapFrom(src => src.TowerID))
                .ForMember(dest => dest.TowerName, opt => opt.Ignore())
                .ForMember(dest => dest.TalkgroupName, opt => opt.Ignore())
                .ForMember(dest => dest.RadioName, opt => opt.Ignore())
                .ForMember(dest => dest.HitCount, opt => opt.Ignore())
                .ForMember(dest => dest.CWIDCount, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapTowerTalkgroupRadiosResult(IProfileExpression config) =>
            config.CreateMap<TowerTalkgroupRadios_Result, TowerTalkgroupRadio>()
                .ForMember(dest => dest.TowerName, opt => opt.MapFrom(src => src.TowerDescription))
                .ForMember(dest => dest.TalkgroupName, opt => opt.MapFrom(src => src.TalkgroupDescription))
                .ForMember(dest => dest.RadioName, opt => opt.MapFrom(src => src.RadioDescription))
                .ForMember(dest => dest.AffiliationCount, opt => opt.MapFrom(src => src != null ? src.AffiliationCount : 0))
                .ForMember(dest => dest.DeniedCount, opt => opt.MapFrom(src => src != null ? src.DeniedCount : 0))
                .ForMember(dest => dest.VoiceGrantCount, opt => opt.MapFrom(src => src != null ? src.VoiceGrantCount : 0))
                .ForMember(dest => dest.EmergencyVoiceGrantCount, opt => opt.MapFrom(src => src != null ? src.EmergencyVoiceGrantCount : 0))
                .ForMember(dest => dest.EncryptedVoiceGrantCount, opt => opt.MapFrom(src => src != null ? src.EncryptedVoiceGrantCount : 0))
                .ForMember(dest => dest.DataCount, opt => opt.MapFrom(src => src != null ? src.DataCount : 0))
                .ForMember(dest => dest.PrivateDataCount, opt => opt.MapFrom(src => src != null ? src.PrivateDataCount : 0))
                .ForMember(dest => dest.AlertCount, opt => opt.MapFrom(src => src != null ? src.AlertCount : 0))
                .ForMember(dest => dest.FirstSeen, opt => opt.MapFrom(src => src != null ? src.FirstSeen : DateTime.Now))
                .ForMember(dest => dest.FirstSeen, opt => opt.MapFrom(src => src != null ? src.FirstSeen : DateTime.Now))
                .ForMember(dest => dest.ID, opt => opt.Ignore())
                .ForMember(dest => dest.SystemID, opt => opt.Ignore())
                .ForMember(dest => dest.Date, opt => opt.Ignore())
                .ForMember(dest => dest.CWIDCount, opt => opt.Ignore())
                .ForMember(dest => dest.HitCount, opt => opt.Ignore())
                .ForMember(dest => dest.LastModified, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);

        private static void MapTowerTalkgroupRadiosWithDates(IProfileExpression config) =>
            config.CreateMap<TowerTalkgroupRadiosWithDates_Result, TowerTalkgroupRadio>()
                .ForMember(dest => dest.TowerName, opt => opt.MapFrom(src => src.TowerDescription))
                .ForMember(dest => dest.TalkgroupName, opt => opt.MapFrom(src => src.TalkgroupDescription))
                .ForMember(dest => dest.RadioName, opt => opt.MapFrom(src => src.RadioDescription))
                .ForMember(dest => dest.AffiliationCount, opt => opt.MapFrom(src => src != null ? src.AffiliationCount : 0))
                .ForMember(dest => dest.DeniedCount, opt => opt.MapFrom(src => src != null ? src.DeniedCount : 0))
                .ForMember(dest => dest.VoiceGrantCount, opt => opt.MapFrom(src => src != null ? src.VoiceGrantCount : 0))
                .ForMember(dest => dest.EmergencyVoiceGrantCount, opt => opt.MapFrom(src => src != null ? src.EmergencyVoiceGrantCount : 0))
                .ForMember(dest => dest.EncryptedVoiceGrantCount, opt => opt.MapFrom(src => src != null ? src.EncryptedVoiceGrantCount : 0))
                .ForMember(dest => dest.DataCount, opt => opt.MapFrom(src => src != null ? src.DataCount : 0))
                .ForMember(dest => dest.PrivateDataCount, opt => opt.MapFrom(src => src != null ? src.PrivateDataCount : 0))
                .ForMember(dest => dest.AlertCount, opt => opt.MapFrom(src => src != null ? src.AlertCount : 0))
                .ForMember(dest => dest.FirstSeen, opt => opt.MapFrom(src => src != null ? src.FirstSeen : DateTime.Now))
                .ForMember(dest => dest.FirstSeen, opt => opt.MapFrom(src => src != null ? src.FirstSeen : DateTime.Now))
                .ForMember(dest => dest.SystemID, opt => opt.Ignore())
                .ForMember(dest => dest.ID, opt => opt.Ignore())
                .ForMember(dest => dest.HitCount, opt => opt.Ignore())
                .ForMember(dest => dest.CWIDCount, opt => opt.Ignore())
                .ForMember(dest => dest.LastModified, opt => opt.Ignore())
                .ForMember(dest => dest.IsNew, opt => opt.Ignore())
                .ForMember(dest => dest.IsDirty, opt => opt.Ignore())
                .AfterMap((src, dest) => dest.IsNew = false)
                .AfterMap((src, dest) => dest.IsDirty = false);
    }
}
