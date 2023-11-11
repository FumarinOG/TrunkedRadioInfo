using System;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace DataLibrary.Tests
{
    public class ExtensionsTests
    {
        private IEnumerable<TestClass> _testList;
        private DateTime _testDate = DateTime.Now;

        private class TestClass
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public DateTime Date { get; set; }
        }

        public ExtensionsTests()
        {
            _testList = new List<TestClass>
            {
                new TestClass
                {
                    ID = 1,
                    Name = "First",
                    Date = _testDate
                },
                new TestClass
                {
                    ID = 2,
                    Name = "Second",
                    Date = _testDate
                }
            };
        }

        [Fact]
        public void ToDataTableReturnsDataTable()
        {
            Assert.IsAssignableFrom<DataTable>(_testList.ToDataTable());
        }

        [Fact]
        public void ToDataTableColumnsProperType()
        {
            var dataTable = _testList.ToDataTable();

            Assert.Equal(typeof(int), dataTable.Columns["ID"].DataType);
            Assert.Equal(typeof(string), dataTable.Columns["Name"].DataType);
            Assert.Equal(typeof(DateTime), dataTable.Columns["Date"].DataType);
        }
    }
}
