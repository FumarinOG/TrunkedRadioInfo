using ObjectLibrary.Interfaces;
using System.Runtime.CompilerServices;

namespace ObjectLibrary.Abstracts
{
    public abstract class AuditableBase : IAuditable
    {
        public int ID { get; set; }
        public bool IsNew { get; set; }
        public bool IsDirty { get; set; }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            IsDirty = true;
            return true;
        }
    }
}
