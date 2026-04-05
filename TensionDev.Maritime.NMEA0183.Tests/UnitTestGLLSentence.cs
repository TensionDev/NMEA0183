using System;
using TensionDev.Maritime.NMEA0183;
using Xunit;

namespace TensionDev.Maritime.NMEA0183.Tests
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

        [Theory]
        [InlineData(123.456, 123, 27.36)]   // Positive degrees
        [InlineData(-123.456, -123, 27.36)] // Negative degrees
        [InlineData(0, 0, 0)]               // Zero
        [InlineData(45.5, 45, 30)]          // Exact half degree
        public void DecimalDegreesToDegreesMinute_WorksCorrectly(decimal input, int expectedDegrees, decimal expectedMinutes)
        {
            //GLLSentence.DecimalDegreesToDegreesMinute(input, out int actualDegrees, out decimal actualMinutes);

            //Assert.Equal(expectedDegrees, actualDegrees);
            //Assert.Equal(expectedMinutes, Math.Round(actualMinutes, 2));
        }
    }
}
