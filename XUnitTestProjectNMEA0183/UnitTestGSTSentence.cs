using System;
using TensionDev.Maritime.NMEA0183;
using Xunit;

namespace XUnitTestProjectNMEA0183
{
    public class UnitTestGSTSentence
    {
        [Fact]
        public void GSTDecoding()
        {
            String sentence = "$GPGST,172814.0,0.006,0.023,0.020,273.6,0.023,0.020,0.031*6A";
            TimeSpan timeUTC = new TimeSpan(17, 28, 14);

            NMEASentence nmeaSentence = NMEASentence.DecodeSentence(sentence);
            GSTSentence gstSentence = nmeaSentence as GSTSentence;

            Assert.NotNull(nmeaSentence);
            Assert.NotNull(gstSentence);
            Assert.Equal(TalkerIdentifierEnum.GlobalPositioningSystem, gstSentence.TalkerIdentifier.TalkerIdentifierEnum);
            Assert.Equal(timeUTC, gstSentence.UTCTimeOfTheFix.TimeOfDay);
            Assert.Equal(0.006M, gstSentence.RootMeanSquaredValueOfTheStandardDeviation);
            Assert.Equal(0.023M, gstSentence.StandardDeviationSemiMajorAxisOfErrorEllipseMetres);
            Assert.Equal(0.020M, gstSentence.StandardDeviationSemiMinorAxisOfErrorEllipseMetres);
            Assert.Equal(273.6M, gstSentence.OrientationOfSemiMajorAxisOfErrorEllipseDegrees);
            Assert.Equal(0.023M, gstSentence.StandardDeviationLatitudeMetres);
            Assert.Equal(0.020M, gstSentence.StandardDeviationLongitudeMetres);
            Assert.Equal(0.031M, gstSentence.StandardDeviationAltitudeMetres);
        }
    }
}
