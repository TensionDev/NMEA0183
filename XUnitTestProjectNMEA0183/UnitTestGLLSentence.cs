using System;
using TensionDev.Maritime.NMEA0183;
using Xunit;

namespace XUnitTestProjectNMEA0183
{
    public class UnitTestGLLSentence
    {
        [Fact]
        public void GLLDecoding()
        {
            String sentence = "$GPGLL,4916.45,N,12311.12,W,225444,A*31";
            TimeSpan timeUTC = new TimeSpan(22, 54, 44);

            NMEASentence nmeaSentence = NMEASentence.DecodeSentence(sentence);
            GLLSentence gllSentence = nmeaSentence as GLLSentence;

            Assert.NotNull(nmeaSentence);
            Assert.NotNull(gllSentence);
            Assert.Equal(TalkerIdentifierEnum.GlobalPositioningSystem, gllSentence.TalkerIdentifier.TalkerIdentifierEnum);
            Assert.Equal(49.274167M, gllSentence.LatitudeDecimalDegrees, 5);
            Assert.Equal(-123.185333M, gllSentence.LongitudeDecimalDegrees, 5);
            Assert.Equal(timeUTC, gllSentence.UTCTimeAtPosition.TimeOfDay);
            Assert.True(gllSentence.IsDataValid);
        }
    }
}
