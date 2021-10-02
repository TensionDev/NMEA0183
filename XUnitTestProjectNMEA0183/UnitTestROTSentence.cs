using System;
using TensionDev.Maritime.NMEA0183;
using Xunit;

namespace XUnitTestProjectNMEA0183
{
    public class UnitTestROTSentence
    {
        [Fact]
        public void ROTDecoding()
        {
            String sentence = "$GPROT,35.6,A*4E";

            NMEASentence nmeaSentence = NMEASentence.DecodeSentence(sentence);
            ROTSentence rotSentence = nmeaSentence as ROTSentence;

            Assert.NotNull(nmeaSentence);
            Assert.NotNull(rotSentence);
            Assert.Equal(TalkerIdentifierEnum.GlobalPositioningSystem, rotSentence.TalkerIdentifier.TalkerIdentifierEnum);
            Assert.Equal(35.6M, rotSentence.RateOfTurn);
            Assert.True(rotSentence.IsDataValid);
        }
    }
}
