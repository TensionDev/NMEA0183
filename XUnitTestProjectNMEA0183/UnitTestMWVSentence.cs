using System;
using TensionDev.Maritime.NMEA0183;
using Xunit;

namespace XUnitTestProjectNMEA0183
{
    public class UnitTestMWVSentence
    {
        [Fact]
        public void MWVDecoding()
        {
            String sentence = "$WIMWV,214.8,R,0.1,K,A*28";

            NMEASentence nmeaSentence = NMEASentence.DecodeSentence(sentence);
            MWVSentence mwvSentence = nmeaSentence as MWVSentence;

            Assert.NotNull(nmeaSentence);
            Assert.NotNull(mwvSentence);
            Assert.Equal(TalkerIdentifierEnum.WeatherInstruments, mwvSentence.TalkerIdentifier.TalkerIdentifierEnum);
            Assert.Equal(214.8M, mwvSentence.WindAngleDegrees);
            Assert.Equal(MWVSentence.WindReferenceEnum.R, mwvSentence.WindReference);
            Assert.Equal(0.1M, mwvSentence.WindSpeed);
            Assert.Equal(MWVSentence.WindSpeedUnitsEnum.K, mwvSentence.WindSpeedUnits);
            Assert.True(mwvSentence.IsDataValid);
        }
    }
}
