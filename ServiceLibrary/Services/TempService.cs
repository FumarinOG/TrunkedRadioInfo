using DataLibrary.Interfaces;
using ObjectLibrary.Interfaces;
using ServiceLibrary.Interfaces.Services;
using System.Collections.Generic;

namespace ServiceLibrary.Services
{
    public class TempService<T1, T2> : ITempService<T1, T2> where T1 : ITempRecord<T2> where T2 : IRecord
    {
        private readonly ITempRepository<T1, T2> _tempRepository;

        public TempService(ITempRepository<T1, T2> tempRepository)
        {
            _tempRepository = tempRepository;
        }

        public void Write(IEnumerable<T1> records, string tempTableName)
        {
            _tempRepository.Write(records, tempTableName);
        }
    }
}
