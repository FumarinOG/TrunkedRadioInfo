using ObjectLibrary.Abstracts;
using System;

namespace ObjectLibrary
{
    public class Patch : RecordBase
    {
        public int SystemID
        {
            get => _systemID;
            set => SetProperty(ref _systemID, value);
        }

        public int TowerNumber
        {
            get => _towerNumber;
            set => SetProperty(ref _towerNumber, value);
        }

        public string TowerName { get; set; }

        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        public int FromTalkgroupID
        {
            get => _fromTalkgroupID;
            set => SetProperty(ref _fromTalkgroupID, value);
        }

        public string FromTalkgroupName { get; set; }

        public int ToTalkgroupID
        {
            get => _toTalkgroupID;
            set => SetProperty(ref _toTalkgroupID, value);
        }

        public string ToTalkgroupName { get; set; }

        public static readonly int MIN_TALKGROUP_ID = Talkgroup.MIN_TALKGROUP_ID;
        public static readonly int MAX_TALKGROUP_ID = Talkgroup.MAX_TALKGROUP_ID;

        private int _systemID;
        private int _towerNumber;
        private DateTime _date;
        private int _fromTalkgroupID;
        private int _toTalkgroupID;

        public override string ToString() => $"System ID {_systemID}, From {_fromTalkgroupID} to {_toTalkgroupID}";

        public override bool Equals(object obj)
        {
            if (!(obj is Patch compareObject))
            {
                return false;
            }

            return ((compareObject.SystemID == SystemID) &&
                    (compareObject.FromTalkgroupID == FromTalkgroupID) &&
                    (compareObject.ToTalkgroupID == ToTalkgroupID) &&
                    (compareObject.LastSeen == LastSeen) &&
                    (compareObject.FirstSeen == FirstSeen) &&
                    (compareObject.HitCount == HitCount));
        }

        public override int GetHashCode() => SystemID.GetHashCode() ^
                   FromTalkgroupID.GetHashCode() ^
                   ToTalkgroupID.GetHashCode() ^
                   LastSeen.GetHashCode() ^
                   FirstSeen.GetHashCode() ^
                   HitCount.GetHashCode();
    }
}
