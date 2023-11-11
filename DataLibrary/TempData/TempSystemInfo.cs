using DataLibrary.Interfaces;
using ObjectLibrary;
using System;

namespace DataLibrary.TempData
{
    public class TempSystemInfo : ITempRecord<SystemInfo>
    {
        public Guid SessionID { get; private set; }
        public string SystemID { get; private set; }
        public int SystemIDDecimal { get; private set; }
        public string Description { get; private set; }
        public string WACN { get; private set; }
        public DateTime FirstSeen { get; private set; }
        public DateTime LastSeen { get; private set; }

        [DataTableSkip]
        public string TableName => "TempSystems";

        public void CopyFrom(Guid sessionID, SystemInfo systemInfo)
        {
            SessionID = sessionID;
            SystemID = systemInfo.SystemID;
            SystemIDDecimal = systemInfo.SystemIDDecimal;
            Description = systemInfo.Description;
            WACN = systemInfo.WACN;
            FirstSeen = systemInfo.FirstSeen;
            LastSeen = systemInfo.LastSeen;
        }

        public override string ToString() => $"Temp - System ID {SystemID} ({Description})";
    }
}
