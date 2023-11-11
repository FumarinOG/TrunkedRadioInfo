namespace RadioService
{
    public sealed class RadioNameViewModel
    {
        public int RadioID { get; private set; }
        public string Name { get; private set; }

        public RadioNameViewModel()
        {
        }

        public RadioNameViewModel(int radioID, string name) => (RadioID, Name) = (radioID, name);
    }
}
