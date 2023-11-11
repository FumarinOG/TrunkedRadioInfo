using System;
using Xunit;

namespace ServiceCommon.Tests.Core
{
    public class DateModelTests
    {
        [Fact]
        public void ConstructorAssignsValuesProperly()
        {
            var date = DateTime.Now;

            var dateModel = new DateModel(date);

            Assert.Equal(date, dateModel.Date);
            Assert.Equal($"{date:yyyy-MM-dd}", dateModel.DateText);
        }
    }
}
