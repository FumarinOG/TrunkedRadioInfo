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
    
    public partial class RadiosGet1_Result
    {
        public int ID { get; set; }
        public int SystemID { get; set; }
        public int RadioID { get; set; }
        public string Description { get; set; }
        public System.DateTime LastSeen { get; set; }
        public Nullable<System.DateTime> LastSeenProgram { get; set; }
        public Nullable<long> LastSeenProgramUnix { get; set; }
        public System.DateTime FirstSeen { get; set; }
        public string FGColor { get; set; }
        public string BGColor { get; set; }
        public int HitCount { get; set; }
        public Nullable<bool> PhaseIISeen { get; set; }
        public System.DateTime LastModified { get; set; }
    }
}
