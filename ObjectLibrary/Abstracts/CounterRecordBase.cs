using ObjectLibrary.Interfaces;
using System;

namespace ObjectLibrary.Abstracts
{
    public abstract class CounterRecordBase : RecordBase, ICounterRecord
    {
        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        public int AffiliationCount
        {
            get => _affiliationCount;
            set => SetProperty(ref _affiliationCount, value);
        }

        public int DeniedCount
        {
            get => _deniedCount;
            set => SetProperty(ref _deniedCount, value);
        }

        public int VoiceGrantCount
        {
            get => _voiceGrantCount;
            set => SetProperty(ref _voiceGrantCount, value);
        }

        public int EmergencyVoiceGrantCount
        {
            get => _emergencyVoiceGrantCount;
            set => SetProperty(ref _emergencyVoiceGrantCount, value);
        }

        public int EncryptedVoiceGrantCount
        {
            get => _encryptedVoiceGrantCount;
            set => SetProperty(ref _encryptedVoiceGrantCount, value);
        }

        public int DataCount
        {
            get => _dataCount;
            set => SetProperty(ref _dataCount, value);
        }

        public int PrivateDataCount
        {
            get => _privateDataCount;
            set => SetProperty(ref _privateDataCount, value);
        }

        public int CWIDCount
        {
            get => _cwidCount;
            set => SetProperty(ref _cwidCount, value);
        }

        public int AlertCount
        {
            get => _alertCount;
            set => SetProperty(ref _alertCount, value);
        }

        private DateTime _date;
        private int _affiliationCount;
        private int _deniedCount;
        private int _voiceGrantCount;
        private int _emergencyVoiceGrantCount;
        private int _encryptedVoiceGrantCount;
        private int _dataCount;
        private int _privateDataCount;
        private int _cwidCount;
        private int _alertCount;

        protected static string GetBooleanText(bool? value)
        {
            if (value == true)
            {
                return "Yes";
            }

            return "No";
        }
    }
}
