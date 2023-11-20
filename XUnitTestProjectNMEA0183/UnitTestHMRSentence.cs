using System;
using System.Reflection.PortableExecutable;
using TensionDev.Maritime.NMEA0183;
using Xunit;

namespace XUnitTestProjectNMEA0183
{
    public class UnitTestHMRSentence
    {
        [Fact]
        public void HMRDecoding()
        {
            String sentence = "$HCHMR,GP01X,HC02K,1.0,0.5,A,359.0,A,M,1.0,W,358.5,A,M,1.0,E,0.1,E*45";

            NMEASentence nmeaSentence = NMEASentence.DecodeSentence(sentence);
            HMRSentence hmrSentence = nmeaSentence as HMRSentence;

            Assert.NotNull(nmeaSentence);
            Assert.NotNull(hmrSentence);
            Assert.Equal(TalkerIdentifierEnum.HeadingMagneticCompass, hmrSentence.TalkerIdentifier.TalkerIdentifierEnum);
            Assert.Equal("GP01X", hmrSentence.HeadingSensorOneID);
            Assert.Equal("HC02K", hmrSentence.HeadingSensorTwoID);
            Assert.Equal(1.0M, hmrSentence.DifferenceLimitSettingDegrees);
            Assert.Equal(0.5M, hmrSentence.ActualHeadingSensorDifferenceDegrees);
            Assert.True(hmrSentence.WarningFlag);
            Assert.Equal(359.0M, hmrSentence.HeadingReadingSensorOneDegrees);
            Assert.Equal(HMRSentence.SensorTypeEnum.M, hmrSentence.SensorOneType);
            Assert.Equal(-1.0M, hmrSentence.DeviationSensorOneDegrees);
            Assert.Equal(358.5M, hmrSentence.HeadingReadingSensorTwoDegrees);
            Assert.Equal(HMRSentence.SensorTypeEnum.M, hmrSentence.SensorTwoType);
            Assert.Equal(1.0M, hmrSentence.DeviationSensorTwoDegrees);
            Assert.Equal(0.1M, hmrSentence.VariationDegrees);
        }

        [Fact]
        public void HMRDecoding_NullVariables()
        {
            String sentence = "$HCHMR,GP01X,HC02K,1.0,0.5,A,359.0,A,M,,,358.5,A,M,,,,*3D";

            NMEASentence nmeaSentence = NMEASentence.DecodeSentence(sentence);
            HMRSentence hmrSentence = nmeaSentence as HMRSentence;

            Assert.NotNull(nmeaSentence);
            Assert.NotNull(hmrSentence);
            Assert.Equal(TalkerIdentifierEnum.HeadingMagneticCompass, hmrSentence.TalkerIdentifier.TalkerIdentifierEnum);
            Assert.Equal("GP01X", hmrSentence.HeadingSensorOneID);
            Assert.Equal("HC02K", hmrSentence.HeadingSensorTwoID);
            Assert.Equal(1.0M, hmrSentence.DifferenceLimitSettingDegrees);
            Assert.Equal(0.5M, hmrSentence.ActualHeadingSensorDifferenceDegrees);
            Assert.True(hmrSentence.WarningFlag);
            Assert.Equal(359.0M, hmrSentence.HeadingReadingSensorOneDegrees);
            Assert.Equal(HMRSentence.SensorTypeEnum.M, hmrSentence.SensorOneType);
            Assert.Null(hmrSentence.DeviationSensorOneDegrees);
            Assert.Equal(358.5M, hmrSentence.HeadingReadingSensorTwoDegrees);
            Assert.Equal(HMRSentence.SensorTypeEnum.M, hmrSentence.SensorTwoType);
            Assert.Null(hmrSentence.DeviationSensorTwoDegrees);
            Assert.Null(hmrSentence.VariationDegrees);
        }
    }
}
