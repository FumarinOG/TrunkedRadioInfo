using DataLibrary.Interfaces;
using FileService.Interfaces;
using ObjectLibrary.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileService
{
    public sealed class TempService<T1, T2> : ITempService<T1, T2> where T1 : ITempRecord<T2> where T2 : IRecord
    {
        private readonly ITempRepository<T1, T2> _tempRepository;

        public TempService(ITempRepository<T1, T2> tempRepository) => _tempRepository = tempRepository;

        public async Task WriteAsync(IEnumerable<T1> records, string tempTableName) => await _tempRepository.WriteAsync(records, tempTableName);
    }
}
