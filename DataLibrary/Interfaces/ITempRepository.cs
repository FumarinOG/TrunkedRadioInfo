using ObjectLibrary.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLibrary.Interfaces
{
    public interface ITempRepository<in T1, T2> where T1 : ITempRecord<T2> where T2 : IRecord
    {
        Task WriteAsync(IEnumerable<T1> records, string tempTable);
    }
}
