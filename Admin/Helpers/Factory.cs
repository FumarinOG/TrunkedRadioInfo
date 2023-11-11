using System;
using System.Threading.Tasks;

namespace Admin.Helpers
{
    public static class Factory
    {
        public static T Create<T>() where T : new() => new T();

        public static PageLoadStatus CreatePageLoadStatus(string pageName, Func<Task> loadMethod) => new PageLoadStatus(pageName, loadMethod);
    }
}
