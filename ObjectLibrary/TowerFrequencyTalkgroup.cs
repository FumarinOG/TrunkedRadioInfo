using ObjectLibrary.Abstracts;

namespace ObjectLibrary
{
    public class TowerFrequencyTalkgroup : CounterRecordBase
    {
        public int SystemID
        {
            get => _systemID;
            set => SetProperty(ref _systemID, value);
        }

        public int TowerID
        {
            get => _towerID;
            set => SetProperty(ref _towerID, value);
        }

        public string TowerName { get; set; }

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

        public string TalkgroupName { get; set; }

        private int _systemID;
        private int _towerID;
        private string _frequency;
        private int _talkgroupID;

        public TowerFrequencyTalkgroup()
        {
            _recordType = "tower frequency talkgroup";
        }

        public override string ToString() => $"System ID {_systemID}, Tower # {_towerID}, Frequency {_frequency}, Talkgroup ID {_talkgroupID}";
    }
}
