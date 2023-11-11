using System;
using System.Threading.Tasks;

namespace Admin.Helpers
{
    public class PageLoadStatus
    {
        public string PageName { get; }
        public bool IsLoaded { get; set; }
        public Func<Task> LoadMethod { get; }

        public PageLoadStatus(string pageName, Func<Task> loadMethod) => (PageName, LoadMethod) = (pageName, loadMethod);
    }
}
