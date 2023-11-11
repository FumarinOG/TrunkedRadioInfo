using ServiceCommon;

namespace RadioService
{
    public sealed class RadioDataViewModel
    {
        public string SystemID { get; private set; }
        public string SystemName { get; private set; }
        public int RadioID { get; private set; }
        public RadioViewModel RadioData { get; private set; }
        public SearchDataViewModel SearchData { get; private set; }

        public RadioDataViewModel(string systemID, string systemName, int radioID, RadioViewModel radioData, SearchDataViewModel searchData) =>
            (SystemID, SystemName, RadioID, RadioData, SearchData) = (systemID, systemName, radioID, radioData, searchData);
    }
}
