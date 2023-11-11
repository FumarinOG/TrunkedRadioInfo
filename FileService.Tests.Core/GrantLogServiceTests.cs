using ObjectLibrary;
using ObjectLibrary.Interfaces;
using System;
using System.Collections.Generic;
using Xunit;

namespace FileService.Tests.Core
{
    public class GrantLogServiceTests
    {
        [Fact]
        public void CreateAlertGrantLogRadioReturnsProperObject()
        {
            var grantLog = new GrantLog
            {
                TimeStamp = DateTime.Now,
                Type = "Alert",
                Channel = string.Empty,
                Frequency = string.Empty,
                TalkgroupID = 365930,
                TalkgroupDescription = "IDoT ETP Unit (930)",
                RadioID = 369008,
                RadioDescription = "IDoT Control"
            };

            var result = GrantLogService.CreateAlertGrantLogRadio(grantLog);

            Assert.Equal(grantLog.TimeStamp, result.TimeStamp);
            Assert.Equal(grantLog.Type, result.Type);
            Assert.Equal(grantLog.Channel, result.Channel);
            Assert.Equal(grantLog.Frequency, result.Frequency);
            Assert.Equal(0, result.TalkgroupID);
            Assert.Equal(string.Empty, result.TalkgroupDescription);
            Assert.Equal(grantLog.RadioID, result.RadioID);
            Assert.Equal(grantLog.RadioDescription, result.RadioDescription);
        }

        [Fact]
        public void CreateAlertGrantLogTalkgroupReturnsProperObject()
        {
            var grantLog = new GrantLog
            {
                TimeStamp = DateTime.Now,
                Type = "Alert",
                Channel = string.Empty,
                Frequency = string.Empty,
                TalkgroupID = 365930,
                TalkgroupDescription = "IDoT ETP Unit (930)",
                RadioID = 369008,
                RadioDescription = "IDoT Control"
            };

            var result = GrantLogService.CreateAlertGrantLogTalkgroup(grantLog);

            Assert.Equal(grantLog.TimeStamp, result.TimeStamp);
            Assert.Equal(grantLog.Type, result.Type);
            Assert.Equal(grantLog.Channel, result.Channel);
            Assert.Equal(grantLog.Frequency, result.Frequency);
            Assert.Equal(0, result.TalkgroupID);
            Assert.Equal(string.Empty, result.TalkgroupDescription);
            Assert.Equal(grantLog.TalkgroupID, result.RadioID);
            Assert.Equal(grantLog.TalkgroupDescription, result.RadioDescription);
        }

        [Theory]
        [MemberData(nameof(GetCounterRecords))]
        public void UpdateCountersUpdatesCounts(ICounterRecord record, ActionTypes action, string fieldName, int expectedValue)
        {
            GrantLogService.UpdateCounters(record, action);

            var property = record.GetType().GetProperty(fieldName).GetValue(record);

            Assert.Equal(expectedValue, (int)property);
        }

        public static IEnumerable<object[]> GetCounterRecords()
        {
            var data = new[]
            {
                new object[] { new Radio { AlertCount = 10 }, ActionTypes.Alert, "AlertCount", 11 },
                new object[] { new Radio { DataCount = 20 }, ActionTypes.Data, "DataCount", 21 },
                new object[] { new Radio { VoiceGrantCount = 30 }, ActionTypes.Group, "VoiceGrantCount", 31 },
                new object[] { new Radio { EmergencyVoiceGrantCount = 40 }, ActionTypes.GroupEmergency, "EmergencyVoiceGrantCount", 41 },
                new object[] { new Radio { EncryptedVoiceGrantCount = 50 }, ActionTypes.GroupEncrypted, "EncryptedVoiceGrantCount", 51 },
                new object[] { new Radio { PrivateDataCount = 60 }, ActionTypes.PrivateData, "PrivateDataCount", 61 },
                new object[] { new Radio { CWIDCount = 70 }, ActionTypes.CWID, "CWIDCount", 71 },
                new object[] { new Radio { CWIDCount = 80 }, ActionTypes.StationID, "CWIDCount", 81 },
                new object[] { new Radio { DataCount = 90 }, ActionTypes.GroupData, "DataCount", 90 },
                new object[] { new Radio { VoiceGrantCount = 100 }, ActionTypes.Queued, "VoiceGrantCount", 100 },
                new object[] { new Radio { DataCount = 110 }, ActionTypes.QueuedDataGrant, "DataCount", 110 },
                new object[] { new Radio { VoiceGrantCount = 120 }, ActionTypes.Affiliate, "VoiceGrantCount", 120 },
                new object[] { new Radio { VoiceGrantCount = 130 }, ActionTypes.Unaffiliate, "VoiceGrantCount", 130 },
                new object[] { new Radio { VoiceGrantCount = 140 }, ActionTypes.Denied, "VoiceGrantCount", 140 },
                new object[] { new Radio { VoiceGrantCount = 150 }, ActionTypes.Forced, "VoiceGrantCount", 150 },
                new object[] { new Radio { VoiceGrantCount = 160 }, ActionTypes.Refused, "VoiceGrantCount", 160 }
            };

            return data;
        }

        [Fact]
        public void UpdateCountersThrowsExceptionForUnknownAction()
        {
            var radio = new Radio();

            Assert.Throws<Exception>(() => GrantLogService.UpdateCounters(radio, (ActionTypes)(-1)));
        }

        [Theory]
        [MemberData(nameof(GetHitCountRecords))]
        public void UpdateHitCountsUpdatesCounts(IRecord record, ActionTypes action, string fieldName, int expectedValue)
        {
            GrantLogService.UpdateHitCounts(record, action);

            var property = record.GetType().GetProperty(fieldName).GetValue(record);

            Assert.Equal(expectedValue, (int)property);
        }

        public static IEnumerable<object[]> GetHitCountRecords()
        {
            var data = new[]
            {
                new object[] { new GrantLog { HitCount = 10 }, ActionTypes.Alert, "HitCount", 10 },
                new object[] { new GrantLog { HitCount = 20 }, ActionTypes.Data, "HitCount", 20  },
                new object[] { new GrantLog { HitCount = 30 }, ActionTypes.Group, "HitCount", 31  },
                new object[] { new GrantLog { HitCount = 40 }, ActionTypes.GroupEmergency, "HitCount", 41  },
                new object[] { new GrantLog { HitCount = 50 }, ActionTypes.GroupEncrypted, "HitCount", 51  },
                new object[] { new GrantLog { HitCount = 60 }, ActionTypes.PrivateData, "HitCount", 60 },
                new object[] { new GrantLog { HitCount = 70 }, ActionTypes.CWID, "HitCount", 70 },
                new object[] { new GrantLog { HitCount = 80 }, ActionTypes.StationID, "HitCount", 80 },
                new object[] { new GrantLog { HitCount = 90 }, ActionTypes.GroupData, "HitCount", 90 },
                new object[] { new GrantLog { HitCount = 100 }, ActionTypes.Queued, "HitCount", 100 },
                new object[] { new GrantLog { HitCount = 110 }, ActionTypes.QueuedDataGrant, "HitCount", 110 },
                new object[] { new GrantLog { HitCount = 120 }, ActionTypes.Affiliate, "HitCount", 120 },
                new object[] { new GrantLog { HitCount = 130 }, ActionTypes.Unaffiliate, "HitCount", 130 },
                new object[] { new GrantLog { HitCount = 140 }, ActionTypes.Denied, "HitCount", 140 },
                new object[] { new GrantLog { HitCount = 150 }, ActionTypes.Forced, "HitCount", 150 },
                new object[] { new GrantLog { HitCount = 160 }, ActionTypes.Refused, "HitCount", 160 }
            };

            return data;
        }
    }
}

