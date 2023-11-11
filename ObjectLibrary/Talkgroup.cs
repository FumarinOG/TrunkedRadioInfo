using ObjectLibrary.Abstracts;
using System;

namespace ObjectLibrary
{
    public class Talkgroup : CounterRecordBase
    {
        public int SystemID
        {
            get => _systemID;
            set => SetProperty(ref _systemID, value);
        }

        public int TalkgroupID
        {
            get => _talkgroupID;
            set => SetProperty(ref _talkgroupID, value);
        }

        public int? Priority
        {
            get => _priority;
            set => SetProperty(ref _priority, value);
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

        public DateTime? FirstSeenProgram
        {
            get => _firstSeenProgram;
            set => SetProperty(ref _firstSeenProgram, value);
        }

        public long? FirstSeenProgramUnix
        {
            get => _firstSeenProgramUnix;
            set => SetProperty(ref _firstSeenProgramUnix, value);
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

        public bool? EncryptionSeen
        {
            get => _encryptionSeen;
            set => SetProperty(ref _encryptionSeen, value);
        }

        public string EncryptionSeenText => GetBooleanText(_encryptionSeen);

        public bool? IgnoreEmergencySignal
        {
            get => _ignoreEmergencySignal;
            set => SetProperty(ref _ignoreEmergencySignal, value);
        }

        public string IgnoreEmergencySignalText => GetBooleanText(_ignoreEmergencySignal);

        public int HitCountProgram
        {
            get => _hitCountProgram;
            set => SetProperty(ref _hitCountProgram, value);
        }

        public bool? PhaseIISeen
        {
            get => _phaseIISeen;
            set => SetProperty(ref _phaseIISeen, value);
        }

        public string PhaseIISeenText => GetBooleanText(_phaseIISeen);

        public int PatchCount
        {
            get => _patchCount;
            set => SetProperty(ref _patchCount, value);
        }

        public string Towers { get; set; }

        public static readonly int MIN_TALKGROUP_ID = 1;
        public static readonly int MAX_TALKGROUP_ID = 65536;

        private int _systemID;
        private int _talkgroupID;
        private int? _priority;
        private string _description;
        private DateTime? _lastSeenProgram;
        private long? _lastSeenProgramUnix;
        private DateTime? _firstSeenProgram;
        private long? _firstSeenProgramUnix;
        private string _fgColor;
        private string _bgColor;
        private bool? _encryptionSeen;
        private bool? _ignoreEmergencySignal;
        private int _hitCountProgram;
        private bool? _phaseIISeen;
        private int _patchCount;

        public Talkgroup()
        {
            _recordType = "talkgroup";
        }

        public void Assign(Talkgroup talkgroup)
        {
            ID = talkgroup.ID;
            LastSeen = talkgroup.LastSeen;
            FirstSeen = talkgroup.FirstSeen;
            LastModified = talkgroup.LastModified;
        }

        public override string ToString() => $"System ID {_systemID}, Talkgroup ID {_talkgroupID} ({_description})";

        public override bool Equals(object obj)
        {
            if (!(obj is Talkgroup compareObject))
            {
                return false;
            }

            return (compareObject.SystemID == SystemID) &&
                   (compareObject.TalkgroupID == TalkgroupID) &&
                   ((compareObject.Priority == null) || (compareObject.Priority == Priority)) &&
                   (compareObject.Description.Equals(Description, StringComparison.OrdinalIgnoreCase)) &&
                   (compareObject.LastSeen == LastSeen) &&
                   ((compareObject.LastSeenProgram == null) || (compareObject.LastSeenProgram == LastSeenProgram)) &&
                   ((compareObject.LastSeenProgramUnix == null) || (compareObject.LastSeenProgramUnix == LastSeenProgramUnix)) &&
                   (compareObject.FirstSeen == FirstSeen) &&
                   ((compareObject.FirstSeenProgram == null) || (compareObject.FirstSeenProgram == FirstSeenProgram)) &&
                   ((compareObject.FirstSeenProgramUnix == null) || (compareObject.FirstSeenProgramUnix == FirstSeenProgramUnix)) &&
                   (compareObject.FGColor.IsNullOrWhiteSpace() || compareObject.FGColor.Equals(FGColor, StringComparison.OrdinalIgnoreCase)) &&
                   (compareObject.BGColor.IsNullOrWhiteSpace() || compareObject.BGColor.Equals(BGColor, StringComparison.OrdinalIgnoreCase)) &&
                   ((compareObject.EncryptionSeen == null) || (compareObject.EncryptionSeen == EncryptionSeen)) &&
                   (compareObject.HitCount == HitCount) &&
                   (compareObject.HitCountProgram == HitCountProgram) &&
                   ((compareObject.IgnoreEmergencySignal == null) || (compareObject.IgnoreEmergencySignal == IgnoreEmergencySignal)) &&
                   (compareObject.PhaseIISeen == PhaseIISeen);
        }

        public override int GetHashCode()
        {
            return SystemID.GetHashCode() ^
                   TalkgroupID.GetHashCode() ^
                   (Priority == null ? 0 : Priority.GetHashCode()) ^
                   Description.GetHashCode() ^
                   LastSeen.GetHashCode() ^
                   (LastSeenProgram == null ? 0 : LastSeenProgram.GetHashCode()) ^
                   (LastSeenProgramUnix == null ? 0 : LastSeenProgramUnix.GetHashCode()) ^
                   FirstSeen.GetHashCode() ^
                   (FirstSeenProgram == null ? 0 : FirstSeenProgram.GetHashCode()) ^
                   (FirstSeenProgramUnix == null ? 0 : FirstSeenProgramUnix.GetHashCode()) ^
                   (FGColor.IsNullOrWhiteSpace() ? 0 : FGColor.GetHashCode()) ^
                   (BGColor.IsNullOrWhiteSpace() ? 0 : BGColor.GetHashCode()) ^
                   (EncryptionSeen == null ? 0 : EncryptionSeen.GetHashCode()) ^
                   HitCount.GetHashCode() ^
                   HitCountProgram.GetHashCode() ^
                   (IgnoreEmergencySignal == null ? 0 : IgnoreEmergencySignal.GetHashCode()) ^
                   (PhaseIISeen == null ? 0 : PhaseIISeen.GetHashCode());
        }
    }
}
