using System;
using TensionDev.Maritime.NMEA0183;
using Xunit;

namespace XUnitTestProjectNMEA0183
{
    public class UnitTestOSDSentence
    {
        [Fact]
        public void OSDDecoding()
        {
            String sentence = "$GPOSD,190.9,A,191.4,B,25.2,B,191.4,25.2,N*43";

            NMEASentence nmeaSentence = NMEASentence.DecodeSentence(sentence);
            OSDSentence osdSentence = nmeaSentence as OSDSentence;

            Assert.NotNull(nmeaSentence);
            Assert.NotNull(osdSentence);
            Assert.Equal(TalkerIdentifierEnum.GlobalPositioningSystem, osdSentence.TalkerIdentifier.TalkerIdentifierEnum);
            Assert.Equal(190.9M, osdSentence.HeadingTrue);
            Assert.True(osdSentence.IsHeadingValid);
            Assert.Equal(191.4M, osdSentence.VesselCourseTrue);
            Assert.Equal(OSDSentence.ReferenceSystemEnum.B, osdSentence.VesselCourseReference);
            Assert.Equal(25.2M, osdSentence.VesselSpeed);
            Assert.Equal(OSDSentence.ReferenceSystemEnum.B, osdSentence.VesselSpeedReference);
            Assert.Equal(191.4M, osdSentence.VesselSetTrue);
            Assert.Equal(25.2M, osdSentence.VesselDrift);
            Assert.Equal(OSDSentence.SpeedUnitsEnum.N, osdSentence.SpeedUnits);
        }
    }
}
