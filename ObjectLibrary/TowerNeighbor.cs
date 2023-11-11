using ObjectLibrary.Abstracts;
using System;

namespace ObjectLibrary
{
    public class TowerNeighbor : AuditableBase
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

        public int NeighborSystemID
        {
            get => _neighborSystemID;
            set => SetProperty(ref _neighborSystemID, value);
        }

        public int NeighborTowerID
        {
            get => _neighborTowerID;
            set => SetProperty(ref _neighborTowerID, value);
        }

        public string NeighborTowerNumberHex
        {
            get => _neighborTowerNumberHex;
            set => SetProperty(ref _neighborTowerNumberHex, value);
        }

        public string NeighborChannel
        {
            get => _neighborChannel;
            set => SetProperty(ref _neighborChannel, value);
        }

        public string NeighborFrequency
        {
            get => _neighborFrequency;
            set => SetProperty(ref _neighborFrequency, value);
        }

        public string NeighborTowerName
        {
            get => _neighborTowerName;
            set => SetProperty(ref _neighborTowerName, value);
        }

        public DateTime FirstSeen
        {
            get => _firstSeen;
            set => SetProperty(ref _firstSeen, value);
        }

        public DateTime LastSeen
        {
            get => _lastSeen;
            set => SetProperty(ref _lastSeen, value);
        }

        public DateTime LastModified { get; set; }

        private int _systemID;
        private int _towerNumber;
        private int _neighborSystemID;
        private int _neighborTowerID;
        private string _neighborTowerNumberHex;
        private string _neighborChannel;
        private string _neighborFrequency;
        private string _neighborTowerName;
        private DateTime _firstSeen;
        private DateTime _lastSeen;

        public override string ToString() => $"System ID {_systemID}, Tower # {_towerNumber}, Neighbor Tower # {_neighborTowerID} ({_neighborTowerName})";
    }
}
