namespace RadioService
{
    public sealed class RadioDetailViewModel
    {
        public string SystemID { get; private set; }
        public int RadioID { get; private set; }

        public RadioDetailViewModel(string systemID, int radioID) => (SystemID, RadioID) = (systemID, radioID);
    }
}
