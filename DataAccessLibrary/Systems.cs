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
    using System.Collections.Generic;
    
    public partial class Systems
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Systems()
        {
            this.Patches = new HashSet<Patches>();
            this.ProcessedFiles = new HashSet<ProcessedFiles>();
            this.Radios = new HashSet<Radios>();
            this.Talkgroups = new HashSet<Talkgroups>();
            this.Towers = new HashSet<Towers>();
        }
    
        public int ID { get; set; }
        public string SystemID { get; set; }
        public int SystemIDDecimal { get; set; }
        public string Description { get; set; }
        public string WACN { get; set; }
        public System.DateTime FirstSeen { get; set; }
        public System.DateTime LastSeen { get; set; }
        public System.DateTime LastModified { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Patches> Patches { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProcessedFiles> ProcessedFiles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Radios> Radios { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Talkgroups> Talkgroups { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Towers> Towers { get; set; }
    }
}