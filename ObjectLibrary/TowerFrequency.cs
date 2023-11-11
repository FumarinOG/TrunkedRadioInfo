using ObjectLibrary.Abstracts;
using System;

namespace ObjectLibrary
{
    public class TowerFrequency : CounterRecordBase
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

        public string Channel
        {
            get => _channel;
            set => SetProperty(ref _channel, value);
        }

        public string Usage
        {
            get => _usage;
            set => SetProperty(ref _usage, value);
        }

        public string Frequency
        {
            get => _frequency;
            set => SetProperty(ref _frequency, value);
        }

        public string InputChannel
        {
            get => _inputChannel;
            set => SetProperty(ref _inputChannel, value);
        }

        public string InputFrequency
        {
            get => _inputFrequency;
            set => SetProperty(ref _inputFrequency, value);
        }

        public int? InputExplicit
        {
            get => _inputExplicit;
            set => SetProperty(ref _inputExplicit, value);
        }

        private int _systemID;
        private int _towerID;
        private string _channel;
        private string _usage;
        private string _frequency;
        private string _inputChannel;
        private string _inputFrequency;
        private int? _inputExplicit;

        public override string ToString() => $"System ID {_systemID}, Tower # {_towerID}, Frequency {_frequency}";

        public override bool Equals(object obj)
        {
            if (!(obj is TowerFrequency compareObject))
            {
                return false;
            }

            return compareObject != null &&
                   compareObject.SystemID == SystemID &&
                   compareObject.TowerID == TowerID &&
                   compareObject.Channel.Equals(Channel, StringComparison.OrdinalIgnoreCase) &&
                   compareObject.Usage.Equals(Usage, StringComparison.OrdinalIgnoreCase) &&
                   compareObject.Frequency.Equals(Frequency, StringComparison.OrdinalIgnoreCase) &&
                   (compareObject.InputChannel.IsNullOrWhiteSpace() ||
                    compareObject.InputChannel.Equals(InputChannel, StringComparison.OrdinalIgnoreCase)) &&
                   compareObject.InputFrequency.Equals(InputFrequency, StringComparison.OrdinalIgnoreCase) &&
                   ((compareObject.InputExplicit == null) || (compareObject.InputExplicit == InputExplicit)) &&
                   compareObject.FirstSeen == FirstSeen &&
                   compareObject.LastSeen == LastSeen;
        }

        public override int GetHashCode()
        {
            return SystemID.GetHashCode()
                   ^ (Channel.IsNullOrWhiteSpace() ? 0 : Channel.GetHashCode())
                   ^ (Usage.IsNullOrWhiteSpace() ? 0 : Usage.GetHashCode())
                   ^ Frequency.GetHashCode()
                   ^ (InputChannel.IsNullOrWhiteSpace() ? 0 : InputChannel.GetHashCode())
                   ^ (InputFrequency.IsNullOrWhiteSpace() ? 0 : InputFrequency.GetHashCode())
                   ^ (InputExplicit == null ? 0 : InputExplicit.GetHashCode())
                   ^ HitCount.GetHashCode()
                   ^ FirstSeen.GetHashCode()
                   ^ LastSeen.GetHashCode();
        }
    }
}
