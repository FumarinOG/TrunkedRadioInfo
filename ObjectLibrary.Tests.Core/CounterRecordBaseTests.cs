using ObjectLibrary.Abstracts;
using System;
using Xunit;

namespace ObjectLibrary.Tests.Core
{
    public class MockCounterRecordBase : CounterRecordBase
    {
        public bool? TestBoolean
        {
            get => _testBoolean;
            set => SetProperty(ref _testBoolean, value);
        }

        public string TestBooleanText
        {
            get => GetBooleanText(_testBoolean);
        }

        private bool? _testBoolean;
    }
    
    public class CounterRecordBaseTests
    {
        [Fact]
        public void DateAssignedProperly()
        {
            var date = DateTime.Now;

            var testClass = new MockCounterRecordBase
            {
                Date = date
            };

            Assert.Equal(date, testClass.Date);
            Assert.True(testClass.IsDirty);
        }

        [Fact]
        public void AffiliationCountAssignedProperly()
        {
            var testClass = new MockCounterRecordBase
            {
                AffiliationCount = 1
            };

            Assert.Equal(1, testClass.AffiliationCount);
            Assert.True(testClass.IsDirty);
        }

        [Fact]
        public void DeniedCountAssignedProperly()
        {
            var testClass = new MockCounterRecordBase
            {
                DeniedCount = 1
            };

            Assert.Equal(1, testClass.DeniedCount);
            Assert.True(testClass.IsDirty);
        }

        [Fact]
        public void VoiceGrantCountAssignedProperly()
        {
            var testClass = new MockCounterRecordBase
            {
                VoiceGrantCount = 1
            };

            Assert.Equal(1, testClass.VoiceGrantCount);
            Assert.True(testClass.IsDirty);
        }

        [Fact]
        public void EmergencyVoiceGrantCountAssignedProperly()
        {
            var testClass = new MockCounterRecordBase
            {
                EmergencyVoiceGrantCount = 1
            };

            Assert.Equal(1, testClass.EmergencyVoiceGrantCount);
            Assert.True(testClass.IsDirty);
        }

        [Fact]
        public void EncryptedVoiceGrantCountAssignedProperly()
        {
            var testClass = new MockCounterRecordBase
            {
                EncryptedVoiceGrantCount = 1
            };

            Assert.Equal(1, testClass.EncryptedVoiceGrantCount);
            Assert.True(testClass.IsDirty);
        }

        [Fact]
        public void DataCountAssignedProperly()
        {
            var testClass = new MockCounterRecordBase
            {
                DataCount = 1
            };

            Assert.Equal(1, testClass.DataCount);
            Assert.True(testClass.IsDirty);
        }

        [Fact]
        public void PrivateDataCountyAssignedProperly()
        {
            var testClass = new MockCounterRecordBase
            {
                PrivateDataCount = 1
            };

            Assert.Equal(1, testClass.PrivateDataCount);
            Assert.True(testClass.IsDirty);
        }

        [Fact]
        public void CWIDCountAssignedProperly()
        {
            var testClass = new MockCounterRecordBase
            {
                CWIDCount = 1
            };

            Assert.Equal(1, testClass.CWIDCount);
            Assert.True(testClass.IsDirty);
        }

        [Fact]
        public void AlertCountAssignedProperly()
        {
            var testClass = new MockCounterRecordBase
            {
                AlertCount = 1
            };

            Assert.Equal(1, testClass.AlertCount);
            Assert.True(testClass.IsDirty);
        }

        [Theory]
        [InlineData(true, "Yes")]
        [InlineData(false, "No")]
        [InlineData(null, "No")]
        public void GetBooleanTextReturnsAppropriateValues(bool? testValue, string result)
        {
            var testClass = new MockCounterRecordBase
            {
                TestBoolean = testValue
            };

            Assert.Equal(result, testClass.TestBooleanText);
        }
    }
}
