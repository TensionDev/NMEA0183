using System;
using TensionDev.Maritime.NMEA0183;
using Xunit;

namespace TensionDev.Maritime.NMEA0183.Tests
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

        [Fact]
        public void HDTEncoding()
        {
            String expected = "$GPHDT,123.4,T*31\r\n";
            HDTSentence hdtSentence = new HDTSentence()
            {
                TalkerIdentifier = new TalkerIdentifier()
                {
                    TalkerIdentifierEnum = TalkerIdentifierEnum.GlobalPositioningSystem,
                },
                HeadingTrue = 123.4M,
            };
            string actual = hdtSentence.EncodeSentence();
            Assert.Equal(expected, actual);
        }
    }
}
