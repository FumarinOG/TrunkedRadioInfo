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
    
    public partial class TowerNeighborsGet1_Result
    {
        public int ID { get; set; }
        public int SystemID { get; set; }
        public int TowerID { get; set; }
        public int NeighborSystemID { get; set; }
        public int NeighborTowerID { get; set; }
        public string NeighborTowerNumberHex { get; set; }
        public string NeighborChannel { get; set; }
        public string NeighborFrequency { get; set; }
        public string NeighborTowerName { get; set; }
        public System.DateTime FirstSeen { get; set; }
        public System.DateTime LastSeen { get; set; }
        public System.DateTime LastModified { get; set; }
    }
}
