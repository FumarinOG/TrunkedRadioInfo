using System;

namespace ServiceCommon
{
    public interface IViewModel
    {
        DateTime FirstSeen { get; }
        DateTime LastSeen { get; }
    }
}
