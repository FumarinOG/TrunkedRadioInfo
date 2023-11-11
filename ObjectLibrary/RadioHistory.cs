using ObjectLibrary.Abstracts;

namespace ObjectLibrary
{
    public class RadioHistory : RecordBase
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

        public int RadioIDKey
        {
            get => _radioIDKey;
            set => SetProperty(ref _radioIDKey, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private int _systemID;
        private int _radioID;
        private int _radioIDKey;
        private string _description;

        public RadioHistory()
        {
            _recordType = "radio history";
        }

        public override string ToString() => $"System ID {_systemID}, Radio ID {_radioID} ({_description})";
    }
}
