using ObjectLibrary.Abstracts;

namespace ObjectLibrary
{
    public class TowerRadio : CounterRecordBase
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

        public int RadioID
        {
            get => _radioID;
            set => SetProperty(ref _radioID, value);
        }

        public string RadioName { get; set; }

        private int _systemID;
        private int _towerNumber;
        private int _radioID;

        public TowerRadio()
        {
            _recordType = "tower radio";
        }

        public override string ToString() => $"System ID {_systemID}, Tower # {_towerNumber}, Radio ID {_radioID}";
    }
}
