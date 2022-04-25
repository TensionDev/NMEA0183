using System;
using TensionDev.Maritime.NMEA0183;
using Xunit;

namespace XUnitTestProjectNMEA0183
{
    public class UnitTestAAMSentence
    {
        [Fact]
        public void AAMDecoding()
        {
            String sentence = "$GPAAM,A,A,0.10,N,WPTNME*43";

            NMEASentence nmeaSentence = NMEASentence.DecodeSentence(sentence);
            AAMSentence aamSentence = nmeaSentence as AAMSentence;

            Assert.NotNull(nmeaSentence);
            Assert.NotNull(aamSentence);
            Assert.Equal(TalkerIdentifierEnum.GlobalPositioningSystem, aamSentence.TalkerIdentifier.TalkerIdentifierEnum);
            Assert.True(aamSentence.IsArrivalCircleEntered);
            Assert.True(aamSentence.IsPerpendicularPassed);
            Assert.Equal(0.10M, aamSentence.ArrivalCircleRadiusNauticalMiles);
            Assert.Equal("WPTNME", aamSentence.WaypointID);
        }
    }
}
