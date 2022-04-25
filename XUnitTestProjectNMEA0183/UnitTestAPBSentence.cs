using System;
using TensionDev.Maritime.NMEA0183;
using Xunit;

namespace XUnitTestProjectNMEA0183
{
    public class UnitTestAPBSentence
    {
        [Fact]
        public void APBDecoding()
        {
            String sentence = "$GPAPB,A,A,0.10,R,N,V,V,011,M,DEST,012,M,013,M,S*42";

            NMEASentence nmeaSentence = NMEASentence.DecodeSentence(sentence);
            APBSentence apbSentence = nmeaSentence as APBSentence;

            Assert.NotNull(nmeaSentence);
            Assert.NotNull(apbSentence);
            Assert.Equal(TalkerIdentifierEnum.GlobalPositioningSystem, apbSentence.TalkerIdentifier.TalkerIdentifierEnum);
            Assert.True(apbSentence.IsReliableFixAvailable);
            Assert.True(apbSentence.IsDataValid);
            Assert.Equal(0.10M, apbSentence.MagnitudeCrossTrackErrorNauticalMiles);
            Assert.Equal(APBSentence.DirectionToSteerEnum.R, apbSentence.DirectionToSteer);
            Assert.False(apbSentence.IsArrivalCircleEntered);
            Assert.False(apbSentence.IsPerpendicularPassed);
            Assert.Equal(11M, apbSentence.BearingOriginToDestination);
            Assert.Equal(APBSentence.BearingTypeEnum.M, apbSentence.BearingOrignType);
            Assert.Equal("DEST", apbSentence.WaypointID);
            Assert.Equal(12M, apbSentence.BearingPresentPositionToDestination);
            Assert.Equal(APBSentence.BearingTypeEnum.M, apbSentence.BearingPresentPositionType);
            Assert.Equal(13M, apbSentence.HeadingToSteerToDestinationWaypoint);
            Assert.Equal(APBSentence.HeadingTypeEnum.M, apbSentence.HeadingToSteerType);
            Assert.Equal(APBSentence.ModeIndicatorEnum.S, apbSentence.ModeIndicator);
        }
    }
}
