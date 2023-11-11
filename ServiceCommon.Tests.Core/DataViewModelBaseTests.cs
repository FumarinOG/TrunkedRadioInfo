using System;
using Xunit;

namespace ServiceCommon.Tests.Core
{
    public class DataViewModelTestClass : DataViewModelBase
    {
        public DataViewModelTestClass()
        {
        }

        public DataViewModelTestClass(int affiliationCount, int deniedCount, int voiceCount, int emergencyCount, int encryptedCount, bool phaseIISeen,
            DateTime firstSeen, DateTime lastSeen) : base(affiliationCount, deniedCount, voiceCount, emergencyCount, encryptedCount, phaseIISeen,
            firstSeen, lastSeen)
        {
        }
    }

    public class DataViewModelBaseTests
    {
        [Theory]
        [InlineData(true, "Yes")]
        [InlineData(false, "No")]
        public void ConstructorAssignsValuesProperlyAndTextValuesWork(bool phaseIISeen, string expectedPhaseIISeenText)
        {
            var firstSeen = DateTime.Now.AddYears(-1);
            var lastSeen = DateTime.Now;

            var dataViewModelBaseTest = new DataViewModelTestClass(125000, 150000, 175000, 200000, 250000, phaseIISeen, firstSeen, lastSeen);

            Assert.Equal(125000, dataViewModelBaseTest.AffiliationCount);
            Assert.Equal("125,000", dataViewModelBaseTest.AffiliationCountText);
            Assert.Equal(150000, dataViewModelBaseTest.DeniedCount);
            Assert.Equal("150,000", dataViewModelBaseTest.DeniedCountText);
            Assert.Equal(175000, dataViewModelBaseTest.VoiceCount);
            Assert.Equal("175,000", dataViewModelBaseTest.VoiceCountText);
            Assert.Equal(200000, dataViewModelBaseTest.EmergencyCount);
            Assert.Equal("200,000", dataViewModelBaseTest.EmergencyCountText);
            Assert.Equal(250000, dataViewModelBaseTest.EncryptedCount);
            Assert.Equal("250,000", dataViewModelBaseTest.EncryptedCountText);
            Assert.Equal(phaseIISeen, dataViewModelBaseTest.PhaseIISeen);
            Assert.Equal(expectedPhaseIISeenText, dataViewModelBaseTest.PhaseIISeenText);
            Assert.Equal(firstSeen, dataViewModelBaseTest.FirstSeen);
            Assert.Equal($"{firstSeen:MM-dd-yyyy HH:mm}", dataViewModelBaseTest.FirstSeenText);
            Assert.Equal(lastSeen, dataViewModelBaseTest.LastSeen);
            Assert.Equal($"{lastSeen:MM-dd-yyyy HH:mm}", dataViewModelBaseTest.LastSeenText);
        }
    }
}
