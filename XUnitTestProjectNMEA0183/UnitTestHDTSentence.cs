using System;
using TensionDev.Maritime.NMEA0183;
using Xunit;

namespace XUnitTestProjectNMEA0183
{
    public class UnitTestHDTSentence
    {
        [Fact]
        public void HDTDecoding()
        {
            String sentence = "$GPHDT,355.7,T*31";

            NMEASentence nmeaSentence = NMEASentence.DecodeSentence(sentence);
            HDTSentence hdtSentence = nmeaSentence as HDTSentence;

            Assert.NotNull(nmeaSentence);
            Assert.NotNull(hdtSentence);
            Assert.Equal(TalkerIdentifierEnum.GlobalPositioningSystem, hdtSentence.TalkerIdentifier.TalkerIdentifierEnum);
            Assert.Equal(355.7M, hdtSentence.HeadingTrue);
        }
    }
}
