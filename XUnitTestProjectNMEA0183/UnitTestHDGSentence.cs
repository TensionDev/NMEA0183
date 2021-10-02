using System;
using TensionDev.Maritime.NMEA0183;
using Xunit;

namespace XUnitTestProjectNMEA0183
{
    public class UnitTestHDGSentence
    {
        [Fact]
        public void HDGDecoding()
        {
            String sentence = "$HCHDG,98.3,0.0,E,12.6,W*57";

            NMEASentence nmeaSentence = NMEASentence.DecodeSentence(sentence);
            HDGSentence hdgSentence = nmeaSentence as HDGSentence;

            Assert.NotNull(nmeaSentence);
            Assert.NotNull(hdgSentence);
            Assert.Equal(TalkerIdentifierEnum.HeadingMagneticCompass, hdgSentence.TalkerIdentifier.TalkerIdentifierEnum);
            Assert.Equal(98.3M, hdgSentence.MagneticSensorHeading);
            Assert.Equal(0.0M, hdgSentence.MagneticDeviation);
            Assert.Equal(HDGSentence.DirectionEnum.E, hdgSentence.MagneticDeviationDirection);
            Assert.Equal(12.6M, hdgSentence.MagneticVariation);
            Assert.Equal(HDGSentence.DirectionEnum.W, hdgSentence.MagneticVariationDirection);
        }
    }
}
