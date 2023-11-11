using ObjectLibrary.Abstracts;
using System;
using Xunit;

namespace ObjectLibrary.Tests.Core
{
    public class MockRecordBase : RecordBase
    {
    }

    public class RecordBaseTests
    {
        [Fact]
        public void HitCountAssignsProperly()
        {
            var mockRecordBase = new MockRecordBase
            {
                HitCount = 10
            };

            Assert.Equal(10, mockRecordBase.HitCount);
            Assert.True(mockRecordBase.IsDirty);
        }

        [Fact]
        public void FirstSeenAssignsProperly()
        {
            var date = DateTime.Now;

            var mockRecordBase = new MockRecordBase
            {
                FirstSeen = date
            };

            Assert.Equal(date, mockRecordBase.FirstSeen);
            Assert.True(mockRecordBase.IsDirty);
        }

        [Fact]
        public void LastSeenAssignsProperly()
        {
            var date = DateTime.Now;

            var mockRecordBase = new MockRecordBase
            {
                LastSeen = date
            };

            Assert.Equal(date, mockRecordBase.LastSeen);
            Assert.True(mockRecordBase.IsDirty);
        }
    }
}
