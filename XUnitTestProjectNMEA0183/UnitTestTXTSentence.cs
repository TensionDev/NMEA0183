using System;
using TensionDev.Maritime.NMEA0183;
using Xunit;

namespace XUnitTestProjectNMEA0183
{
    public class UnitTestTXTSentence
    {
        [Fact]
        public void TXTDecoding()
        {
            String sentence = "$GPTXT,01,01,25,DR MODE - ANTENNA FAULT^21*38";

            NMEASentence nmeaSentence = NMEASentence.DecodeSentence(sentence);
            TXTSentence txtSentence = nmeaSentence as TXTSentence;

            Assert.NotNull(nmeaSentence);
            Assert.NotNull(txtSentence);
            Assert.Equal(TalkerIdentifierEnum.GlobalPositioningSystem, txtSentence.TalkerIdentifier.TalkerIdentifierEnum);
            Assert.Equal(01, txtSentence.TotalNumberOfSentences);
            Assert.Equal(01, txtSentence.SentenceNumber);
            Assert.Equal(25, txtSentence.TextIdentifier);
            Assert.Equal("DR MODE - ANTENNA FAULT!", txtSentence.TextMessage);
        }
    }
}
