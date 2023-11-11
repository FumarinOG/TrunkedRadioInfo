using System;

namespace ObjectLibrary.Interfaces
{
    public interface IRecord
    {
        int ID { get; set; }
        int HitCount { get; set; }
        DateTime FirstSeen { get; set; }
        DateTime LastSeen { get; set; }
        DateTime LastModified { get; set; }
        bool IsNew { get; set; }
        bool IsDirty { get; set; }
    }
}
