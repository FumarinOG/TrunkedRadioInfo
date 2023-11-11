using ObjectLibrary.Abstracts;

namespace ObjectLibrary
{
    public class TalkgroupHistory : RecordBase
    {
        public int SystemID
        {
            get => _systemID;
            set => SetProperty(ref _systemID, value);
        }

        public int TalkgroupID
        {
            get => _talkgroupID;
            set => SetProperty(ref _talkgroupID, value);
        }

        public int TalkgroupIDKey
        {
            get => _talkgroupIDKey;
            set => SetProperty(ref _talkgroupIDKey, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private int _systemID;
        private int _talkgroupID;
        private int _talkgroupIDKey;
        private string _description;

        public TalkgroupHistory()
        {
            _recordType = "talkgroup history";
        }

        public override string ToString() => $"System ID {_systemID}, Talkgroup ID {_talkgroupID} ({_description})";
    }
}
