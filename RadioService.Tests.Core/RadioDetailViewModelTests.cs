using Xunit;

namespace RadioService.Tests.Core
{
    public class RadioDetailViewModelTests
    {
        [Fact]
        public void ConstructorAssingsValuesProperly()
        {
            var radioDetailViewModel = new RadioDetailViewModel("140", 1917101);

            Assert.Equal("140", radioDetailViewModel.SystemID);
            Assert.Equal(1917101, radioDetailViewModel.RadioID);
        }
    }
}
