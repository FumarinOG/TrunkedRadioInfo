using ObjectLibrary.Abstracts;
using System;

namespace ObjectLibrary
{
    public class Radio : CounterRecordBase
    {
        public int SystemID
        {
            get => _systemID;
            set => SetProperty(ref _systemID, value);
        }

        public int RadioID
        {
            get => _radioID;
            set => SetProperty(ref _radioID, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public DateTime? LastSeenProgram
        {
            get => _lastSeenProgram;
            set => SetProperty(ref _lastSeenProgram, value);
        }

        public long? LastSeenProgramUnix
        {
            get => _lastSeenProgramUnix;
            set => SetProperty(ref _lastSeenProgramUnix, value);
        }

        public string FGColor
        {
            get => _fgColor;
            set => SetProperty(ref _fgColor, value);
        }

        public string BGColor
        {
            get => _bgColor;
            set => SetProperty(ref _bgColor, value);
        }

        public bool? PhaseIISeen
        {
            get => _phaseIISeen;
            set => SetProperty(ref _phaseIISeen, value);
        }

        public string PhaseIISeenText => GetBooleanText(_phaseIISeen);

        public static readonly int MIN_RADIO_ID = 1;
        public static readonly int MAX_RADIO_ID = 16777216;

        private int _systemID;
        private int _radioID;
        private string _description;
        private DateTime? _lastSeenProgram;
        private long? _lastSeenProgramUnix;
        private string _fgColor;
        private string _bgColor;
        private bool? _phaseIISeen;

        public Radio()
        {
            _recordType = "radio";
        }

        public void Assign(Radio radio)
        {
            ID = radio.ID;
            LastSeen = radio.LastSeen;
            FirstSeen = radio.FirstSeen;
            HitCount += radio.HitCount;
            LastModified = radio.LastModified;
        }

        public override string ToString() => $"System ID {SystemID}, Radio ID {RadioID} ({Description})";

        public override bool Equals(object obj)
        {
            if (!(obj is Radio compareObject))
            {
                return false;
            }

            return (compareObject.SystemID == SystemID) &&
                   (compareObject.RadioID == RadioID) &&
                   compareObject.Description.Equals(Description, StringComparison.OrdinalIgnoreCase) &&
                   (compareObject.LastSeen == LastSeen) &&
                   (compareObject.LastSeenProgram == LastSeenProgram) &&
                   (compareObject.LastSeenProgramUnix == LastSeenProgramUnix) &&
                   (compareObject.FirstSeen == FirstSeen) &&
                   compareObject.FGColor.Equals(FGColor, StringComparison.OrdinalIgnoreCase) &&
                   compareObject.BGColor.Equals(BGColor, StringComparison.OrdinalIgnoreCase) &&
                   (compareObject.HitCount == HitCount) &&
                   (compareObject.PhaseIISeen == PhaseIISeen);
        }

        public override int GetHashCode()
        {
            return SystemID.GetHashCode() ^
                   RadioID.GetHashCode() ^
                   Description.GetHashCode() ^ LastSeen.GetHashCode() ^
                   (LastSeenProgram == null ? 0 : LastSeenProgram.GetHashCode()) ^
                   (LastSeenProgramUnix == null ? 0 : LastSeenProgramUnix.GetHashCode()) ^
                   FirstSeen.GetHashCode() ^
                   (FGColor.IsNullOrWhiteSpace() ? 0 : FGColor.GetHashCode()) ^
                   (BGColor.IsNullOrWhiteSpace() ? 0 : BGColor.GetHashCode()) ^
                   HitCount.GetHashCode() ^
                   (PhaseIISeen == null ? 0 : PhaseIISeen.GetHashCode());
        }
    }
}
