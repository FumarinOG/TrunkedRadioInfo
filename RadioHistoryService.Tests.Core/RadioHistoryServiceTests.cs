using DataLibrary.Interfaces;
using Moq;
using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RadioHistoryService.Tests.Core
{
    public class RadioHistoryServiceTests
    {
        /*
        public void ProcessRecord(int radioID, string description, DateTime timeStamp, int systemID, ICollection<Radio> radios, ICollection<RadioHistory> history)
        {
            var differentRadioDescription = radios.SingleOrDefault(r => r.RadioID == radioID && r.Description.Equals(description, StringComparison.OrdinalIgnoreCase));

            if (differentRadioDescription == null)
            {
                var newRadioHistory = CreateRadioHistory(systemID, radioID, description, timeStamp);

                var radioHistory = history.SingleOrDefault(rh => rh.RadioID == radioID && rh.Description.Equals(description, StringComparison.OrdinalIgnoreCase));

                if (radioHistory == null)
                {
                    history.Add(newRadioHistory);
                }
                else
                {
                    CheckDates(radioHistory, timeStamp);
                }
            }
            else
            {
                var radioHistory = history.SingleOrDefault(rh => rh.RadioID == radioID && rh.Description.Equals(description, StringComparison.OrdinalIgnoreCase));

                if (radioHistory != null)
                {
                    CheckDates(radioHistory, timeStamp);
                }
                else
                {
                    history.Add(CreateRadioHistory(systemID, radioID, description, timeStamp));
                }
            }
        }
*/
        private IRadioHistoryService _radioHistoryService;
        private List<Radio> _radios = new List<Radio>();
        private List<RadioHistory> _radioHistory = new List<RadioHistory>();

        public RadioHistoryServiceTests()
        {
            var radioHistoryRepo = new Mock<IRadioHistoryRepository>();

            _radioHistoryService = new RadioHistoryService(radioHistoryRepo.Object);

            _radios.Add(new Radio { ID = 1, RadioID = 1917101, Description = "ISP Dispatch (LaSalle) (17-A)" });
            _radios.Add(new Radio { ID = 2, RadioID = 1917102, Description = "ISP Dispatch (LaSalle) (17-B)" });
            _radios.Add(new Radio { ID = 3, RadioID = 1917201, Description = "ISP Dispatch (LaSalle) (17-A)" });
            _radios.Add(new Radio { ID = 4, RadioID = 1917202, Description = "ISP Dispatch (LaSalle) (17-B)" });

            _radioHistory.Add(new RadioHistory
            {
                ID = 10,
                RadioID = 1917101,
                Description = "TG: 9019 DT: 12/01/2007 @ 10:00",
                LastSeen = DateTime.Parse("01-01-2018 13:00")
            });
            _radioHistory.Add(new RadioHistory
            {
                ID = 11,
                RadioID = 1917102,
                Description = "TG: 9020 DT: 12/01/2008 @ 11:11",
                LastSeen = DateTime.Parse("02-02-2018 14:00")
            });
            _radioHistory.Add(new RadioHistory
            {
                ID = 12,
                RadioID = 1917201,
                Description = "TG: 9019 DT: 12/01/2009 @ 12:22",
                LastSeen = DateTime.Parse("03-03-2018 15:00")
            });
            _radioHistory.Add(new RadioHistory
            {
                ID = 13,
                RadioID = 1917202,
                Description = "TG: 9020 DT: 12/01/2010 @ 13:33",
                LastSeen = DateTime.Parse("04-04-2018 16:00")
            });
            _radioHistory.Add(new RadioHistory
            {
                ID = 14,
                RadioID = 1917101,
                Description = "ISP Dispatch (LaSalle) (17-A)",
                LastSeen = DateTime.Parse("05-01-2018 13:00")
            });
            _radioHistory.Add(new RadioHistory
            {
                ID = 15,
                RadioID = 1917102,
                Description = "ISP Dispatch (LaSalle) (17-B)",
                LastSeen = DateTime.Parse("05-02-2018 14:00")
            });
            _radioHistory.Add(new RadioHistory
            {
                ID = 16,
                RadioID = 1917201,
                Description = "ISP Dispatch (LaSalle) (17-A)",
                LastSeen = DateTime.Parse("05-03-2018 15:00")
            });
            _radioHistory.Add(new RadioHistory
            {
                ID = 17,
                RadioID = 1917202,
                Description = "ISP Dispatch (LaSalle) (17-B)",
                LastSeen = DateTime.Parse("05-04-2018 16:00")
            });
        }

        [Fact]
        public void ProcessRecordBailsWithEmptyDescription()
        {
            var radioList = _radios;
            var radioHistoryList = _radioHistory;

            _radioHistoryService.ProcessRecord(1, 9019, string.Empty, DateTime.Now, radioList, radioHistoryList);

            Assert.True(radioHistoryList.SequenceEqual(_radioHistory));
        }

        [Fact]
        public void ProcessRecordAddsHistoryForNewRadioName()
        {
            var currentHistoryCount = _radioHistory.Count;

            _radioHistoryService.ProcessRecord(1, 9019, "New Description", DateTime.Now, _radios, _radioHistory);

            Assert.Equal(currentHistoryCount + 1, _radioHistory.Count);

            var newRadioHistory = _radioHistory.SingleOrDefault(rh => rh.Description.Equals("New Description", StringComparison.OrdinalIgnoreCase));

            Assert.NotNull(newRadioHistory);
            Assert.Equal("New Description", newRadioHistory.Description);

        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void ProcessRecordUpdatesLastSeenForExistingRadioName(int radioID)
        {
            var radio = _radios.Single(r => r.ID == radioID);
            var existingRadioHistoryLastSeen = _radioHistory.Single(rh => rh.RadioID == radio.RadioID && rh.Description.Equals(radio.Description)).LastSeen;

            _radioHistoryService.ProcessRecord(1, radio.RadioID, radio.Description, DateTime.Now, _radios, _radioHistory);

            var radioHistory = _radioHistory.Single(rh => rh.RadioID == radio.RadioID &&
                rh.Description.Equals(radio.Description, StringComparison.OrdinalIgnoreCase));

            Assert.NotEqual(existingRadioHistoryLastSeen, radioHistory.LastSeen);
        }
    }
}
