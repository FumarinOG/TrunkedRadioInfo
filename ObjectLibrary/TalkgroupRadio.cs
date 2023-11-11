using ObjectLibrary.Abstracts;

namespace ObjectLibrary
{
    public class TalkgroupRadio : CounterRecordBase
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

        public string TalkgroupName { get; set; }

        public int RadioID
        {
            get => _radioID;
            set => SetProperty(ref _radioID, value);
        }

        public string RadioName { get; set; }

        private int _systemID;
        private int _talkgroupID;
        private int _radioID;

        public TalkgroupRadio()
        {
            _recordType = "talkgroup radio";
        }

        public override string ToString() => $"System ID {_systemID}, Talkgroup ID {_talkgroupID} ({TalkgroupName}), Radio ID {_radioID} ({RadioName})";
    }
}
