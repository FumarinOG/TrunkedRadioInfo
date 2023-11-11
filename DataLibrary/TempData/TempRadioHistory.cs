using DataLibrary.Interfaces;
using ObjectLibrary;
using System;

namespace DataLibrary.TempData
{
    public class TempRadioHistory : ITempRecord<RadioHistory>
    {
        public Guid SessionID { get; private set; }
        public int SystemID { get; private set; }
        public int RadioID { get; private set; }
        public string Description { get; private set; }
        public DateTime FirstSeen { get; private set; }
        public DateTime LastSeen { get; private set; }

        [DataTableSkip]
        public string TableName => "TempRadioHistory";

        public void CopyFrom(Guid sessionID, RadioHistory radioHistory)
        {
            SessionID = sessionID;
            SystemID = radioHistory.SystemID;
            RadioID = radioHistory.RadioID;
            Description = radioHistory.Description;
            FirstSeen = radioHistory.FirstSeen;
            LastSeen = radioHistory.LastSeen;
        }

        public override string ToString() => $"Temp - System ID {SystemID}, Radio ID {RadioID} ({Description})";
    }
}
