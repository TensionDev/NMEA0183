using System;
using TensionDev.Maritime.NMEA0183;
using Xunit;

namespace XUnitTestProjectNMEA0183
{
    public class UnitTestVBWSentence
    {
        [Fact]
        public void VBWDecoding()
        {
            String sentence = "$IIVBW,11.0,02.0,A,06.0,03.0,A,05.3,A,01.0,A*43";

            NMEASentence nmeaSentence = NMEASentence.DecodeSentence(sentence);
            VBWSentence vbwSentence = nmeaSentence as VBWSentence;

            Assert.NotNull(nmeaSentence);
            Assert.NotNull(vbwSentence);
            Assert.Equal(TalkerIdentifierEnum.IntegratedInstrumentation, vbwSentence.TalkerIdentifier.TalkerIdentifierEnum);
            Assert.Equal(11.0M, vbwSentence.LongitudinalWaterSpeedKnots);
            Assert.Equal(02.0M, vbwSentence.TransverseWaterSpeedKnots);
            Assert.True(vbwSentence.IsWaterDataValid);
            Assert.Equal(06.0M, vbwSentence.LongitudinalGroundSpeedKnots);
            Assert.Equal(03.0M, vbwSentence.TransverseGroundSpeedKnots);
            Assert.True(vbwSentence.IsGroundDataValid);
            Assert.Equal(05.3M, vbwSentence.SternTransverseWaterSpeedKnots);
            Assert.True(vbwSentence.IsSternWaterDataValid);
            Assert.Equal(01.0M, vbwSentence.SternTransverseGroundSpeedKnots);
            Assert.True(vbwSentence.IsSternGroundDataValid);
        }
    }
}
