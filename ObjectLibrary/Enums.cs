using System.ComponentModel;

namespace ObjectLibrary
{
    public enum ActionTypes
    {
        [Description("Alert")]
        Alert,
        [Description("CWID")]
        CWID,
        [Description("Station ID")]
        StationID,
        [Description("Data")]
        Data,
        [Description("Group")]
        Group,
        [Description("Group (Emrg)")]
        GroupEmergency,
        [Description("Group (Enc)")]
        GroupEncrypted,
        [Description("GrpData")]
        GroupData,
        [Description("PvtData")]
        PrivateData,
        [Description("Queued")]
        Queued,
        [Description("Queued (Data Grant")]
        QueuedDataGrant,
        [Description("Affiliate")]
        Affiliate,
        [Description("Unaffiliate")]
        Unaffiliate,
        [Description("Denied")]
        Denied,
        [Description("Forced")]
        Forced,
        [Description("Refused")]
        Refused
    }

    public enum FileTypes
    {
        Talkgroups,
        Radios,
        Tower,
        Affiliations,
        GrantLog,
        PatchLog
    }
}
