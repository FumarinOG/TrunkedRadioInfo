namespace SystemInfoService
{
    public sealed class SystemViewModel
    {
        public int ID { get; private set; }
        public string SystemID { get; private set; }
        public string Name { get; private set; }

        public SystemViewModel()
        {
        }

        public SystemViewModel(int id, string systemID, string name) => (ID, SystemID, Name) = (id, systemID, name);
    }
}
