using System;
using TensionDev.Maritime.NMEA0183;
using Xunit;

namespace XUnitTestProjectNMEA0183
{
    public class UnitTestTHSSentence
    {
        [Fact]
        public void THSDecoding()
        {
            String sentence = "$HETHS,23.4,A*0C";

            NMEASentence nmeaSentence = NMEASentence.DecodeSentence(sentence);
            THSSentence thsSentence = nmeaSentence as THSSentence;

            Assert.NotNull(nmeaSentence);
            Assert.NotNull(thsSentence);
            Assert.Equal(TalkerIdentifierEnum.HeadingNorthSeekingGyro, thsSentence.TalkerIdentifier.TalkerIdentifierEnum);
            Assert.Equal(23.4M, thsSentence.HeadingTrue);
            Assert.Equal(THSSentence.ModeIndicatorEnum.A, thsSentence.ModeIndicator);
        }
    }
}
