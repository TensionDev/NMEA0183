using System;
using TensionDev.Maritime.NMEA0183;
using Xunit;

namespace XUnitTestProjectNMEA0183
{
    public class UnitTestQuerySentence
    {
        [Fact]
        public void QueryEncoding()
        {
            String expectedSentence = "$ECGPQ,GGA*2D\r\n";

            QuerySentence querySentence = new QuerySentence()
            {
                TalkerIdentifier = new TalkerIdentifier() { TalkerIdentifierEnum = TalkerIdentifierEnum.ElectronicChartDisplayInformationSystem },
                ListenerIdentifier = new TalkerIdentifier() { TalkerIdentifierEnum = TalkerIdentifierEnum.GlobalPositioningSystem },
                QuerySentenceIdentifier = "GGA"
            };

            String sentence = querySentence.EncodeSentence();

            Assert.Equal(expectedSentence, sentence);
        }
    }
}
