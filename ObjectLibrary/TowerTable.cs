using ObjectLibrary.Abstracts;
using System;

namespace ObjectLibrary
{
    public class TowerTable : AuditableBase
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

        public int TableID
        {
            get => _tableID;
            set => SetProperty(ref _tableID, value);
        }

        public string BaseFrequency
        {
            get => _baseFrequency;
            set => SetProperty(ref _baseFrequency, value);
        }

        public string Spacing
        {
            get => _spacing;
            set => SetProperty(ref _spacing, value);
        }

        public string InputOffset
        {
            get => _inputOffset;
            set => SetProperty(ref _inputOffset, value);
        }

        public string AssumedConfirmed
        {
            get => _assumedConfirmed;
            set => SetProperty(ref _assumedConfirmed, value);
        }

        public string Bandwidth
        {
            get => _bandwidth;
            set => SetProperty(ref _bandwidth, value);
        }

        public int Slots
        {
            get => _slots;
            set => SetProperty(ref _slots, value);
        }

        public DateTime LastModified { get; set; }

        private int _systemID;
        private int _towerID;
        private int _tableID;
        private string _baseFrequency;
        private string _spacing;
        private string _inputOffset;
        private string _assumedConfirmed;
        private string _bandwidth;
        private int _slots;

        public override string ToString() => $"System ID {_systemID}, Tower # {_towerID}, Table ID {_tableID}";
    }
}
