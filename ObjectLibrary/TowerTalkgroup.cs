using ObjectLibrary.Abstracts;

namespace ObjectLibrary
{
    public class TowerTalkgroup : CounterRecordBase
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

        private int _systemID;
        private int _towerNumber;
        private int _talkgroupID;

        public TowerTalkgroup()
        {
            _recordType = "tower talkgroup";
        }

        public override string ToString() => $"System ID {_systemID}, Tower # {_towerNumber}, Talkgroup ID {_talkgroupID}";
    }
}
