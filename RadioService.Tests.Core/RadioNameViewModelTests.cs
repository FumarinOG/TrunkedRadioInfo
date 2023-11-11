using Xunit;

namespace RadioService.Tests.Core
{
    public class RadioNameViewModelTests
    {
        [Fact]
        public void ConstructorAssignsValuesProperly()
        {
            var radioNameViewModel = new RadioNameViewModel(1917101, "ISP Dispatch (LaSalle) (17-A)");

            Assert.Equal(1917101, radioNameViewModel.RadioID);
            Assert.Equal("ISP Dispatch (LaSalle) (17-A)", radioNameViewModel.Name);
        }
    }
}
