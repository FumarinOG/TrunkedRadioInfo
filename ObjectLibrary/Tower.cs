using ObjectLibrary.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using static ObjectLibrary.Factory;

namespace ObjectLibrary
{
    public class Tower : RecordBase
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

        public string TowerNumberHex
        {
            get => _towerNumberHex;
            set => SetProperty(ref _towerNumberHex, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public string WACN
        {
            get => _wacn;
            set => SetProperty(ref _wacn, value);
        }

        public string ControlCapabilities
        {
            get => _controlCapabilities;
            set => SetProperty(ref _controlCapabilities, value);
        }

        public string Flavor
        {
            get => _flavor;
            set => SetProperty(ref _flavor, value);
        }

        public string CallSigns
        {
            get => _callSigns;
            set => SetProperty(ref _callSigns, value);
        }

        public DateTime? TimeStamp
        {
            get => _timeStamp;
            set => SetProperty(ref _timeStamp, value);
        }

        public IEnumerable<TowerFrequency> Frequencies => _frequencies;
        public IEnumerable<TowerTable> Tables => _tables;
        public IEnumerable<TowerNeighbor> Neighbors => _neighbors;

        public static readonly int MIN_TOWER_NUMBER = 0;
        public static readonly int MAX_TOWER_NUMBER = 65536;

        private int _systemID;
        private int _towerNumber;
        private string _towerNumberHex;
        private string _description;
        private string _wacn;
        private string _controlCapabilities;
        private string _flavor;
        private string _callSigns;
        private DateTime? _timeStamp;

        private ICollection<TowerFrequency> _frequencies = CreateList<TowerFrequency>();
        private ICollection<TowerTable> _tables = CreateList<TowerTable>();
        private ICollection<TowerNeighbor> _neighbors = CreateList<TowerNeighbor>();

        public Tower()
        {
        }

        public Tower(int systemID, int towerNumber, string description)
        {
            SystemID = systemID;
            TowerNumber = towerNumber;
            Description = description;
        }

        public void AddFrequencies(IEnumerable<TowerFrequency> frequencies)
        {
            _frequencies = frequencies.ToList();
        }

        public void AddTables(IEnumerable<TowerTable> tables)
        {
            _tables = tables.ToList();
        }

        public void AddNeighbors(IEnumerable<TowerNeighbor> neighbors)
        {
            _neighbors = neighbors.ToList();
        }

        public void CopyData(Tower tower)
        {
            ID = tower.ID;
            SystemID = tower.SystemID;
            TowerNumberHex = tower.TowerNumberHex;
            WACN = tower.WACN;
            ControlCapabilities = tower.ControlCapabilities;
            Flavor = tower.Flavor;
            CallSigns = tower.CallSigns;
            TimeStamp = tower.TimeStamp;
            FirstSeen = (tower.FirstSeen < FirstSeen ? tower.FirstSeen : FirstSeen);
            LastSeen = (tower.LastSeen > LastSeen ? tower.LastSeen : LastSeen);
        }

        public override string ToString() => $"System ID {_systemID}, Tower # {_towerNumber} ({_description})";

        public override bool Equals(object obj)
        {
            if (!(obj is Tower compareObj))
            {
                return false;
            }

            return compareObj.TowerNumber == TowerNumber &&
                   compareObj.Description.Equals(Description, StringComparison.OrdinalIgnoreCase) &&
                   (compareObj.WACN.IsNullOrWhiteSpace() ||
                    compareObj.WACN.Equals(WACN, StringComparison.OrdinalIgnoreCase)) &&
                   (compareObj.ControlCapabilities.IsNullOrWhiteSpace() ||
                    compareObj.ControlCapabilities.Equals(ControlCapabilities, StringComparison.OrdinalIgnoreCase)) &&
                   (compareObj.Flavor.IsNullOrWhiteSpace() ||
                    compareObj.Flavor.Equals(Flavor, StringComparison.OrdinalIgnoreCase)) &&
                   (compareObj.CallSigns.IsNullOrWhiteSpace() ||
                    compareObj.CallSigns.Equals(CallSigns, StringComparison.OrdinalIgnoreCase)) &&
                   ((compareObj.FirstSeen == DateTime.MinValue) || (compareObj.FirstSeen < FirstSeen)) &&
                   ((compareObj.LastSeen == DateTime.MinValue) || (compareObj.LastSeen > LastSeen));
        }

        public override int GetHashCode()
        {
            return TowerNumber.GetHashCode() ^
                   Description.GetHashCode() ^
                   (!WACN.IsNullOrWhiteSpace() ? WACN.GetHashCode() : 0) ^
                   (!ControlCapabilities.IsNullOrWhiteSpace() ? ControlCapabilities.GetHashCode() : 0) ^
                   (!Flavor.IsNullOrWhiteSpace() ? Flavor.GetHashCode() : 0) ^
                   (!CallSigns.IsNullOrWhiteSpace() ? CallSigns.GetHashCode() : 0) ^
                   FirstSeen.GetHashCode() ^
                   LastSeen.GetHashCode();
        }
    }
}
