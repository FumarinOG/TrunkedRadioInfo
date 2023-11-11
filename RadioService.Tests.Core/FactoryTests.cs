using ObjectLibrary;
using ServiceCommon;
using Xunit;

namespace RadioService.Tests.Core
{
    public class FactoryTests
    {
        [Fact]
        public void CreateRadioDataModelReturnsProperModel()
        {
            var systemInfo = new SystemInfo() { SystemID = "140", Description = "StarCom 21" };
            var radioViewModel = new RadioViewModel();
            var searchDataViewModel = new SearchDataViewModel();

            var result = Factory.CreateRadioDataModel(systemInfo, 1917101, radioViewModel, searchDataViewModel);

            Assert.Equal("140", result.SystemID);
            Assert.Equal("StarCom 21", result.SystemName);
            Assert.Equal(1917101, result.RadioID);
            Assert.IsType(radioViewModel.GetType(), result.RadioData);
            Assert.IsType(searchDataViewModel.GetType(), result.SearchData);
        }
    }
}
