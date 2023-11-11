namespace ObjectLibrary.Interfaces
{
    public interface IAuditable
    {
        bool IsNew { get; set; }
        bool IsDirty { get; set; }
    }
}
