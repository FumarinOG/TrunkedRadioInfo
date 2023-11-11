using DataLibrary.Interfaces;
using ObjectLibrary;
using System;

namespace DataLibrary.TempData
{
    public sealed class TempPatch : ITempRecord<Patch>
    {
        public Guid SessionID { get; private set; }
        public int SystemID { get; private set; }
        public int TowerID { get; private set; }
        public int FromTalkgroupID { get; private set; }
        public int ToTalkgroupID { get; private set; }
        public DateTime Date { get; private set; }
        public DateTime FirstSeen { get; private set; }
        public DateTime LastSeen { get; private set; }
        public int HitCount { get; private set; }

        [DataTableSkip]
        public string TableName =>  "TempPatches";

        public void CopyFrom(Guid sessionID, Patch patch)
        {
            SessionID = sessionID;
            SystemID = patch.SystemID;
            TowerID = patch.TowerNumber;
            FromTalkgroupID = patch.FromTalkgroupID;
            ToTalkgroupID = patch.ToTalkgroupID;
            Date = patch.Date;
            FirstSeen = patch.FirstSeen;
            LastSeen = patch.LastSeen;
            HitCount = patch.HitCount;
        }

        public override string ToString() => $"Temp - TowerID {TowerID}, From {FromTalkgroupID} to {ToTalkgroupID}";
    }
}
