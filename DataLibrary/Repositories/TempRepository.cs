using DataLibrary.Interfaces;
using ObjectLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DataLibrary.Repositories
{
    public sealed class TempRepository<T1, T2> : RepositoryBase, ITempRepository<T1, T2> where T1 : ITempRecord<T2> where T2 : IRecord
    {
        public async Task WriteAsync(IEnumerable<T1> records, string tempTableName) => 

            await Task.Run(() =>
            {
                try
                {
                    using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TrunkedRadioInfo"].ToString()))
                    {
                        connection.Open();

                        using (var bulkCopy = new SqlBulkCopy(connection))
                        {
                            bulkCopy.BulkCopyTimeout = 0;                   // Wait indefinitely to timeout
                            bulkCopy.DestinationTableName = tempTableName;
                            bulkCopy.WriteToServer(records.ToDataTable());
                        }
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error(exception, "Error writing temporary record");
                    throw;
                }
            });
    }
}
