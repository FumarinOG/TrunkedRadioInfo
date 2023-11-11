using ObjectLibrary.Abstracts;

namespace ObjectLibrary
{
    public class TowerTalkgroupRadio : CounterRecordBase
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

        public string TowerName { get; set; }

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
        private int _towerNumber;
        private int _talkgroupID;
        private int _radioID;

        public TowerTalkgroupRadio()
        {
            _recordType = "tower talkgroup radio";
        }

        public override string ToString() => $"System ID {_systemID}, Tower # {_towerNumber}, Talkgroup ID {_talkgroupID}, Radio ID {_radioID}";
    }
}
