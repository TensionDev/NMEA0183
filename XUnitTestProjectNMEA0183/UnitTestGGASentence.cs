using System;
using TensionDev.NMEA0183;
using Xunit;

namespace XUnitTestProjectNMEA0183
{
    public class UnitTestGGASentence
    {
        [Fact]
        public void GGADecoding()
        {
            String sentence = "$GPGGA,172814.0,3723.46587704,N,12202.26957864,W,2,6,1.2,18.893,M,-25.669,M,2.0,0031*4F";
            TimeSpan timeUTC = new TimeSpan(17, 28, 14);

            NMEASentence nmeaSentence = NMEASentence.DecodeSentence(sentence);
            GGASentence ggaSentence = nmeaSentence as GGASentence;

            Assert.NotNull(nmeaSentence);
            Assert.NotNull(ggaSentence);
            Assert.Equal(TalkerIdentifierEnum.GlobalPositioningSystem, ggaSentence.TalkerIdentifier.TalkerIdentifierEnum);
            Assert.Equal(timeUTC, ggaSentence.UTCTimeAtPosition.TimeOfDay);
            Assert.Equal(37.391098M, ggaSentence.LatitudeDecimalDegrees, 5);
            Assert.Equal(-122.037826M, ggaSentence.LongitudeDecimalDegrees, 5);
            Assert.Equal(GGASentence.GPSQualityIndicatorEnum.DifferentialGPSStandardPositioningService, ggaSentence.GPSQualityIndicator);
            Assert.Equal(6, ggaSentence.NumberofSatellitesInUse);
            Assert.Equal(1.2M, ggaSentence.HorizontalDilutionOfPrecision);
            Assert.Equal(18.893M, ggaSentence.AntennaAltitudeAboveMeanSeaLevel);
            Assert.Equal(-25.669M, ggaSentence.GeoidalSeparation);
            Assert.Equal(2.0M, ggaSentence.AgeOfDifferentialGPSData);
            Assert.Equal((UInt16)31, ggaSentence.DifferentialReferenceStationID);
        }

        [Fact]
        public void GGADecoding_NullVariables()
        {
            String sentence = "$GPGGA,123519,4807.038,N,01131.000,E,1,08,0.9,545.4,M,46.9,M,,*47";
            TimeSpan timeUTC = new TimeSpan(12, 35, 19);

            NMEASentence nmeaSentence = NMEASentence.DecodeSentence(sentence);
            GGASentence ggaSentence = nmeaSentence as GGASentence;

            Assert.NotNull(nmeaSentence);
            Assert.NotNull(ggaSentence);
            Assert.Equal(TalkerIdentifierEnum.GlobalPositioningSystem, ggaSentence.TalkerIdentifier.TalkerIdentifierEnum);
            Assert.Equal(timeUTC, ggaSentence.UTCTimeAtPosition.TimeOfDay);
            Assert.Equal(48.117300M, ggaSentence.LatitudeDecimalDegrees, 5);
            Assert.Equal(11.516667M, ggaSentence.LongitudeDecimalDegrees, 5);
            Assert.Equal(GGASentence.GPSQualityIndicatorEnum.GPSStandardPositioningService, ggaSentence.GPSQualityIndicator);
            Assert.Equal(8, ggaSentence.NumberofSatellitesInUse);
            Assert.Equal(0.9M, ggaSentence.HorizontalDilutionOfPrecision);
            Assert.Equal(545.4M, ggaSentence.AntennaAltitudeAboveMeanSeaLevel);
            Assert.Equal(46.9M, ggaSentence.GeoidalSeparation);
            Assert.Null(ggaSentence.AgeOfDifferentialGPSData);
            Assert.Null(ggaSentence.DifferentialReferenceStationID);
        }
    }
}
