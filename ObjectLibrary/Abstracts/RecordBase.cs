using ObjectLibrary.Interfaces;
using System;

namespace ObjectLibrary.Abstracts
{
    public abstract class RecordBase : AuditableBase, IRecord
    {
        public int HitCount
        {
            get => _hitCount;
            set => SetProperty(ref _hitCount, value);
        }

        public DateTime FirstSeen
        {
            get => _firstSeen;
            set => SetProperty(ref _firstSeen, value);
        }

        public DateTime LastSeen
        {
            get => _lastSeen;
            set => SetProperty(ref _lastSeen, value);
        }

        public DateTime LastModified { get; set; }

        protected int _hitCount;
        protected DateTime _firstSeen;
        protected DateTime _lastSeen;
        protected string _recordType;
    }
}
