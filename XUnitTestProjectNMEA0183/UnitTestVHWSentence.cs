using System;
using TensionDev.Maritime.NMEA0183;
using Xunit;

namespace XUnitTestProjectNMEA0183
{
    public class UnitTestVHWSentence
    {
        [Fact]
        public void VHWDecoding()
        {
            String sentence = "$GPVHW,190.9,T,190.5,M,25.2,N,46.6,K*4F";

            NMEASentence nmeaSentence = NMEASentence.DecodeSentence(sentence);
            VHWSentence osdSentence = nmeaSentence as VHWSentence;

            Assert.NotNull(nmeaSentence);
            Assert.NotNull(osdSentence);
            Assert.Equal(TalkerIdentifierEnum.GlobalPositioningSystem, osdSentence.TalkerIdentifier.TalkerIdentifierEnum);
            Assert.Equal(190.9M, osdSentence.HeadingTrue);
            Assert.Equal(190.5M, osdSentence.HeadingMagnetic);
            Assert.Equal(25.2M, osdSentence.SpeedKnots);
            Assert.Equal(46.6M, osdSentence.SpeedKmh);
        }
    }
}
