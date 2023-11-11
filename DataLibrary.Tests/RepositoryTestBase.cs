using DataAccessLibrary;
using Moq;
using System;
using System.Data.Entity.Core.Objects;

namespace DataLibrary.Tests
{
    public abstract class RepositoryTestBase<T> where T : RepositoryBase
    {
        public class MockObjectResult<T1> : ObjectResult<T1>
        {
        }

        protected readonly Mock<TrunkedRadioInfoEntities> _context;
        protected readonly Mock<T> _mockRepo;

        protected readonly FilterData _filterData = new FilterData()
        {
            SearchText = "ABC",
            DateFrom = DateTime.Now.AddYears(-1),
            DateTo = DateTime.Now,
            IDFrom = 9019,
            IDTo = 9020,
            FirstSeenFrom = DateTime.Now.AddYears(-1),
            FirstSeenTo = DateTime.Now,
            LastSeenFrom = DateTime.Now.AddYears(-1),
            LastSeenTo = DateTime.Now,
            SortField = "FromTalkgroupID",
            SortDirection = "Ascending",
            PageNumber = 1,
            PageSize = 15
        };

        public RepositoryTestBase()
        {
            _context = new Mock<TrunkedRadioInfoEntities>();
            _mockRepo = new Mock<T>() { CallBase = true };
        }

        protected void SetupMockRepo()
        {
            _mockRepo.Setup(repo => repo.CreateEntities()).Returns(_context.Object);
        }
    }
}
