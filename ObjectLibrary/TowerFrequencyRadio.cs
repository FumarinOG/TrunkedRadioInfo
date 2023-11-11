using ObjectLibrary.Abstracts;

namespace ObjectLibrary
{
    public class TowerFrequencyRadio : CounterRecordBase
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
        public int RadioID
        {
            get => _radioID;
            set => SetProperty(ref _radioID, value);
        }

        public string RadioName { get; set; }

        private int _systemID;
        private int _towerID;
        private string _frequency;
        private int _radioID;

        public TowerFrequencyRadio()
        {
            _recordType = "tower frequency radio";
        }

        public override string ToString() => $"System ID {_systemID}, Tower # {_towerID}, Frequency {_frequency}, Radio ID {_radioID}";
    }
}
