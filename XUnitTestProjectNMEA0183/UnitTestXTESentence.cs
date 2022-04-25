using System;
using TensionDev.Maritime.NMEA0183;
using Xunit;

namespace XUnitTestProjectNMEA0183
{
    public class UnitTestXTESentence
    {
        [Fact]
        public void XTEDecoding()
        {
            String sentence = "$GPXTE,A,A,0.050,L,N*5E";

            NMEASentence nmeaSentence = NMEASentence.DecodeSentence(sentence);
            XTESentence xteSentence = nmeaSentence as XTESentence;

            Assert.NotNull(nmeaSentence);
            Assert.NotNull(xteSentence);
            Assert.Equal(TalkerIdentifierEnum.GlobalPositioningSystem, xteSentence.TalkerIdentifier.TalkerIdentifierEnum);
            Assert.True(xteSentence.IsReliableFixAvailable);
            Assert.True(xteSentence.IsDataValid);
            Assert.Equal(0.050M, xteSentence.MagnitudeCrossTrackErrorNauticalMiles);
            Assert.Equal(XTESentence.DirectionToSteerEnum.L, xteSentence.DirectionToSteer);
            Assert.Equal(XTESentence.ModeIndicatorEnum.N, xteSentence.ModeIndicator);
        }
    }
}
