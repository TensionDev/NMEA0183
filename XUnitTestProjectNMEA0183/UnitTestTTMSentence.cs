using System;
using TensionDev.Maritime.NMEA0183;
using Xunit;

namespace XUnitTestProjectNMEA0183
{
    public class UnitTestTTMSentence
    {
        [Fact]
        public void TTMDecoding()
        {
            String sentence = "$RATTM,46,1.45,003.9,T,14.50,093.9,T,0.000000,0.000000,K,TANGO,T,,213907.55,A*64";
            TimeSpan expectedTimeUTC = new TimeSpan(0, 21, 39, 07, 550);

            NMEASentence nmeaSentence = NMEASentence.DecodeSentence(sentence);
            TTMSentence ttmSentence = nmeaSentence as TTMSentence;

            Assert.NotNull(nmeaSentence);
            Assert.NotNull(ttmSentence);
            Assert.Equal(TalkerIdentifierEnum.RADAROrARPA, ttmSentence.TalkerIdentifier.TalkerIdentifierEnum);
            Assert.Equal(46, ttmSentence.TargetNumber);
            Assert.Equal(1.45M, ttmSentence.TargetDistance);
            Assert.Equal(3.9M, ttmSentence.BearingFromOwnship);
            Assert.Equal(TTMSentence.DegreesEnum.T, ttmSentence.BearingFromOwnshipTrueRelative);
            Assert.Equal(14.5M, ttmSentence.TargetSpeed);
            Assert.Equal(93.9M, ttmSentence.TargetCourse);
            Assert.Equal(TTMSentence.DegreesEnum.T, ttmSentence.TargetCourseTrueRelative);
            Assert.Equal(0.0M, ttmSentence.ClosestPointOfApproach);
            Assert.Equal(0.0M, ttmSentence.TimeToClosestPointOfApproachMinutes);
            Assert.Equal(TTMSentence.SpeedDistanceUnitsEnum.K, ttmSentence.SpeedDistanceUnits);
            Assert.Equal("TANGO", ttmSentence.TargetName);
            Assert.Equal(TTMSentence.TargetStatusEnum.T, ttmSentence.TargetStatus);
            Assert.False(ttmSentence.ReferenceTarget);
            Assert.Equal(expectedTimeUTC, ttmSentence.UTCOfData.TimeOfDay);
            Assert.Equal(TTMSentence.TypeOfAcquisitionEnum.A, ttmSentence.TypeOfAcquisition);
        }
    }
}
