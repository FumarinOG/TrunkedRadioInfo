using System;
using Xunit;

namespace ServiceCommon.Tests.Core
{
    public class FilterDataModelTests
    {
        [Theory]
        [InlineData(true, "Yes")]
        [InlineData(false, "No")]
        public void ActiveOnlyTextPropertiesReturnProperValues(bool activeOnly, string expectedActiveOnlyText)
        {
            var filterDataModel = new FilterDataModel
            {
                ActiveOnly = activeOnly
            };

            Assert.Equal(expectedActiveOnlyText, filterDataModel.ActiveOnlyText);
        }

        [Fact]
        public void DateFromAndDateToTextReturnFormattedValues()
        {
            var dateFrom = DateTime.Now.AddYears(-1);
            var dateTo = DateTime.Now;

            var filterDataModel = new FilterDataModel
            {
                DateFrom = dateFrom,
                DateTo = dateTo
            };

            Assert.Equal($"{dateFrom:MM-dd-yyyy}", filterDataModel.DateFromText);
            Assert.Equal($"{dateTo:MM-dd-yyyy}", filterDataModel.DateToText);
        }

        [Fact]
        public void DateFromAndToTextReturnEmptyForNulls()
        {
            var filterDataModel = new FilterDataModel
            {
                DateFrom = null,
                DateTo = null
            };

            Assert.Equal(string.Empty, filterDataModel.DateFromText);
            Assert.Equal(string.Empty, filterDataModel.DateToText);
        }
    }
}
