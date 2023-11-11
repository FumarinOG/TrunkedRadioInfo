using ObjectLibrary.Abstracts;
using System.Collections.Generic;

namespace ObjectLibrary
{
    public static class Factory
    {
        public static T Create<T>() where T : AuditableBase, new()
        {
            return new T
            {
                IsNew = true
            };
        }

        public static List<T> CreateList<T>() where T : new()
        {
            return new List<T>();
        }
    }
}
