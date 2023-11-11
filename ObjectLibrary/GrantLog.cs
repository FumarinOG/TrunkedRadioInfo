using ObjectLibrary.Abstracts;
using System;

namespace ObjectLibrary
{
    public class GrantLog : RecordBase
    {
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

        public string Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        public string Channel
        {
            get => _channel;
            set => SetProperty(ref _channel, value);
        }

        public string Frequency
        {
            get => _frequency;
            set => SetProperty(ref _frequency, value);
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

        private int _towerNumber;
        private DateTime _timeStamp;
        private string _type;
        private string _channel;
        private string _frequency;
        private int _talkgroupID;
        private string _talkgroupDescription;
        private int _radioID;
        private string _radioDescription;

        public override string ToString() => $"Talkgroup ID {_talkgroupID}, Radio ID {_radioID}, Type {_type}";
    }
}
