using DataLibrary.Interfaces;
using ObjectLibrary;
using System;

namespace DataLibrary.TempData
{
    public class TempTower : ITempRecord<Tower>
    {
        public Guid SessionID { get; private set; }
        public int SystemID { get; private set; }
        public int TowerNumber { get; private set; }
        public string TowerNumberHex { get; private set; }
        public string Description { get; private set; }
        public int? HitCount { get; private set; }
        public string WACN { get; private set; }
        public string ControlCapabilities { get; private set; }
        public string Flavor { get; private set; }
        public string CallSigns { get; private set; }
        public DateTime? TimeStamp { get; private set; }
        public DateTime FirstSeen { get; private set; }
        public DateTime LastSeen { get; private set; }

        [DataTableSkip]
        public string TableName => "TempTowers";

        public void CopyFrom(Guid sessionID, Tower tower)
        {
            SessionID = sessionID;
            SystemID = tower.SystemID;
            TowerNumber = tower.TowerNumber;
            TowerNumberHex = tower.TowerNumberHex;
            Description = tower.Description;
            HitCount = tower.HitCount;
            WACN = tower.WACN;
            ControlCapabilities = tower.ControlCapabilities;
            Flavor = tower.Flavor;
            CallSigns = tower.CallSigns;
            TimeStamp = tower.TimeStamp;
            FirstSeen = tower.FirstSeen;
            LastSeen = tower.LastSeen;
        }

        public override string ToString() => $"Temp - System ID {SystemID}, Tower # {TowerNumber} ({Description})";
    }
}
