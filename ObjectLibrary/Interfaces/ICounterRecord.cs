using System;

namespace ObjectLibrary.Interfaces
{
    public interface ICounterRecord
    {
        DateTime Date { get; set; }
        int AffiliationCount { get; set; }
        int DeniedCount { get; set; }
        int VoiceGrantCount { get; set; }
        int EmergencyVoiceGrantCount { get; set; }
        int EncryptedVoiceGrantCount { get; set; }
        int DataCount { get; set; }
        int PrivateDataCount { get; set; }
        int CWIDCount { get; set; }
        int AlertCount { get; set; }
    }
}
