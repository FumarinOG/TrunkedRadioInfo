using DataLibrary.Interfaces;
using FileService.Interfaces;
using NLog;
using ObjectLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static FileService.Factory;

namespace FileService
{
    public abstract class ServiceBase
    {
        protected readonly Guid _sessionID;

        protected static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        protected ServiceBase() => _sessionID = Guid.NewGuid();

        protected async Task WriteTempDataAsync<T1, T2>(ITempService<T1, T2> tempService, ICollection<T2> records, Action<string, string, int, int> updateProgress)
           where T1 : ITempRecord<T2>, new() where T2 : IRecord
        {
            var changeCount = records.Count(r => r.IsNew || r.IsDirty);
            var tempType = CreateTemp<T1, T2>();

            updateProgress($"Writing temporary {tempType.TableName}", GetWrittenText(changeCount), changeCount, changeCount);
            await tempService.WriteAsync(GetChanges<T1, T2>(records), tempType.TableName);
        }

        protected static string GetWrittenText(int recordCount) => $"{recordCount:#,##0} records written";

        protected IEnumerable<T1> GetChanges<T1, T2>(IEnumerable<T2> records) where T1 : ITempRecord<T2>, new() where T2 : IRecord
        {
            var tempRecords = CreateTempList<T1, T2>();

            foreach (var record in records.Where(r => r.IsNew || r.IsDirty))
            {
                var newRecord = CreateTemp<T1, T2>();

                newRecord.CopyFrom(_sessionID, record);
                tempRecords.Add(newRecord);
            }

            return tempRecords;
        }
    }
}
