using System.IO;
using System.Text;
using Xunit;

namespace FileService.Tests.Core
{
    public class CSVReaderTests
    {
        [Fact]
        public void CSVReaderParsesCSVText()
        {
            var csvLine1 = "\"06/09 06:42:58\",\"Group\",\"01-1384\",\"770.65625\",\"1951\",\"Romeoville PD Dispatch\",\"0\",\"\"";
            var csvLine2 = "\"06/09 06:42:58\",\"Added Patch:   2007 (ISTHA Maintenance South) --> 2002 (ISTHA Maintenance North)\"";
            var csvLine3 = "\"06/09 07:49:02\",\"Affiliate\",2991,\"Metra PD 1 - Dispatch\",92032,\"Metra PD Unit\"";
            var csvLine = csvLine1 + "\r\n" + csvLine2 + "\r\n" + csvLine3 + "\r\n";
            var csvStream = new MemoryStream(Encoding.UTF8.GetBytes(csvLine));

            var csvReader = new CSVReader(new StreamReader(csvStream));

            var row = csvReader.GetNextRow();
            Assert.Equal(8, row.Length);
            row = csvReader.GetNextRow();
            Assert.Equal(2, row.Length);
            row = csvReader.GetNextRow();
            Assert.Equal(6, row.Length);
            row = csvReader.GetNextRow();
            Assert.Null(row);
        }
    }
}
