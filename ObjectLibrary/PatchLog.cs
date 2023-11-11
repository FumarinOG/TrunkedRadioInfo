using ObjectLibrary.Abstracts;
using System;
using System.ComponentModel;

namespace ObjectLibrary
{
    public class PatchLog : RecordBase
    {
        public enum Actions
        {
            [Description("Added")]
            Added,
            [Description("Removed")]
            Removed
        }

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

        public DateTime TimeStamp
        {
            get => _timeStamp;
            set => SetProperty(ref _timeStamp, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public Actions Action { get; set; }

        private int _systemID;
        private int _towerNumber;
        private DateTime _timeStamp;
        private string _description;

        public override string ToString() => $"Timestamp {_timeStamp:MM-dd-yyyy HH:mm}, Description {_description}";
    }
}
