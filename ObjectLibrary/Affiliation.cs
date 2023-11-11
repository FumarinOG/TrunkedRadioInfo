using ObjectLibrary.Abstracts;
using System;

namespace ObjectLibrary
{
    public class Affiliation : RecordBase
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

        public DateTime TimeStamp
        {
            get => _timeStamp;
            set => SetProperty(ref _timeStamp, value);
        }

        public string Function
        {
            get => _function;
            set => SetProperty(ref _function, value);
        }

        public int TalkgroupID
        {
            get => _talkgroupID;
            set => SetProperty(ref _talkgroupID, value);
        }

        public string TalkgroupDescription
        {
            get => _talkgroupDescription;
            set => SetProperty(ref _talkgroupDescription, value);
        }

        public int RadioID
        {
            get => _radioID;
            set => SetProperty(ref _radioID, value);
        }

        public string RadioDescription
        {
            get => _radioDescription;
            set => SetProperty(ref _radioDescription, value);
        }

        private int _systemID;
        private int _towerNumber;
        private DateTime _timeStamp;
        private string _function;
        private int _talkgroupID;
        private string _talkgroupDescription;
        private int _radioID;
        private string _radioDescription;

        public override string ToString() => $"Tower Number {_towerNumber}, Talkgroup ID {_talkgroupID}, Radio ID {_radioID}, Function {_function}";
    }
}
