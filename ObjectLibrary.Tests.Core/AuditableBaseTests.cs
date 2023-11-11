using ObjectLibrary.Abstracts;
using Xunit;

namespace ObjectLibrary.Tests.Core
{
    public class AuditableBaseMock : AuditableBase
    {
        public string Test
        {
            get => _test;
            set => SetProperty(ref _test, value);
        }

        private string _test;
    }

    public class AuditableBaseTests
    {
        [Fact]
        public void SetPropertySetsNewValue()
        {
            var testClass = new AuditableBaseMock
            {
                Test = "test"
            };

            Assert.Equal("test", testClass.Test);
        }

        [Fact]
        public void SetPropertyDoesNotSetDirtyWhenValueIsAssignedThatIsAlreadySet()
        {
            var testClass = new AuditableBaseMock
            {
                Test = "test",
                IsDirty = false
            };

            testClass.Test = "test";

            Assert.Equal("test", testClass.Test);
            Assert.False(testClass.IsDirty);
        }
    }
}
