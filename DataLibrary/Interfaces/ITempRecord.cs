using ObjectLibrary.Interfaces;
using System;

namespace DataLibrary.Interfaces
{
    public interface ITempRecord<T> where T : IRecord
    {
        Guid SessionID { get; }
        DateTime FirstSeen { get; }
        DateTime LastSeen { get; }
        string TableName { get; }

        void CopyFrom(Guid sessionID, T record);
    }
}
