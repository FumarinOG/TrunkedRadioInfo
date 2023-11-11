using ObjectLibrary.Abstracts;
using System;
using System.Collections.Generic;
using static ObjectLibrary.Factory;

namespace ObjectLibrary
{
    public class SystemInfo : RecordBase
    {
        public string SystemID
        {
            get => _systemID;
            set => SetProperty(ref _systemID, value);
        }

        public int SystemIDDecimal
        {
            get => _systemIDDecimal;
            set => SetProperty(ref _systemIDDecimal, value);
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

        public int TalkgroupCount { get; set; }
        public int RadioCount { get; set; }
        public int TowerCount { get; set; }
        public int RowCount { get; set; }

        public IEnumerable<Tower> Towers => _towers;

        private string _systemID;
        private int _systemIDDecimal;
        private string _description;
        private string _wacn;
        private readonly List<Tower> _towers = CreateList<Tower>();

        public void CopyData(SystemInfo systemInfo)
        {
            ID = systemInfo.ID;
            SystemIDDecimal = systemInfo.SystemIDDecimal;
            WACN = systemInfo.WACN;
            FirstSeen = (systemInfo.FirstSeen < FirstSeen ? systemInfo.FirstSeen : FirstSeen);
            LastSeen = (systemInfo.LastSeen > LastSeen ? systemInfo.LastSeen : LastSeen);
        }

        public void AddTower(int systemID, int towerNumber, string description)
        {
            var tower = Create<Tower>();

            tower.SystemID = systemID;
            tower.TowerNumber = towerNumber;
            tower.Description = description;

            _towers.Add(tower);
        }

        public override string ToString() => $"System ID {_systemID} ({_description})";

        public override bool Equals(object obj)
        {
            if (!(obj is SystemInfo compareObject))
            {
                return false;
            }

            return compareObject.SystemID.Equals(SystemID, StringComparison.OrdinalIgnoreCase) &&
                   (compareObject.SystemIDDecimal == SystemIDDecimal) &&
                   compareObject.Description.Equals(Description, StringComparison.OrdinalIgnoreCase) &&
                   compareObject.WACN.Equals(WACN, StringComparison.OrdinalIgnoreCase) &&
                   ((compareObject.FirstSeen != DateTime.MinValue) || (compareObject.FirstSeen < FirstSeen)) &&
                   ((compareObject.LastSeen != DateTime.MinValue) || (compareObject.LastSeen > LastSeen));
        }

        public override int GetHashCode()
        {
            return SystemID.GetHashCode() ^
                   SystemIDDecimal.GetHashCode() ^
                   Description.GetHashCode() ^
                   (WACN.IsNullOrWhiteSpace() ? 0 : WACN.GetHashCode()) ^
                   FirstSeen.GetHashCode() ^
                   LastSeen.GetHashCode();
        }
    }
}
