//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccessLibrary
{
    using System;
    
    public partial class TowerFrequencyTalkgroups_Result
    {
        public int TalkgroupID { get; set; }
        public Nullable<int> AffiliationCount { get; set; }
        public Nullable<int> DeniedCount { get; set; }
        public Nullable<int> VoiceGrantCount { get; set; }
        public Nullable<int> EmergencyVoiceGrantCount { get; set; }
        public Nullable<int> EncryptedVoiceGrantCount { get; set; }
        public Nullable<int> DataCount { get; set; }
        public Nullable<int> PrivateDataCount { get; set; }
        public Nullable<int> AlertCount { get; set; }
        public Nullable<System.DateTime> FirstSeen { get; set; }
        public Nullable<System.DateTime> LastSeen { get; set; }
        public Nullable<int> RecordCount { get; set; }
        public string TalkgroupDescription { get; set; }
    }
}
