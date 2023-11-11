
using Xunit;

namespace SystemInfoService.Tests.Core
{
    public class SystemViewModelTests
    {
        [Fact]
        public void ConstructorAssignsValuesProperly()
        {
            var id = 1;
            var systemID = "System ID";
            var name = "Name";

            var systemViewModel = new SystemViewModel(id, systemID, name);

            Assert.Equal(id, systemViewModel.ID);
            Assert.Equal(systemID, systemViewModel.SystemID);
            Assert.Equal(name, systemViewModel.Name);
        }
    }
}
