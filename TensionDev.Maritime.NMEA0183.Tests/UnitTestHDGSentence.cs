using System;
using TensionDev.Maritime.NMEA0183;
using Xunit;

namespace TensionDev.Maritime.NMEA0183.Tests
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

        [Fact]
        public void HDGEncoding()
        {
            String expected = "$HCHDG,123.4,1.2,E,5.6,W*54\r\n";
            HDGSentence hdgSentence = new HDGSentence()
            {
                TalkerIdentifier = new TalkerIdentifier()
                {
                    TalkerIdentifierEnum = TalkerIdentifierEnum.HeadingMagneticCompass,
                },
                MagneticSensorHeading = 123.4M,
                MagneticDeviation = 1.2M,
                MagneticDeviationDirection = HDGSentence.DirectionEnum.E,
                MagneticVariation = 5.6M,
                MagneticVariationDirection = HDGSentence.DirectionEnum.W,
            };
            string actual = hdgSentence.EncodeSentence();
            Assert.Equal(expected, actual);
        }
    }
}
