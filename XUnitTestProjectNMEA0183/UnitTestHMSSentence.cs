using System;
using TensionDev.Maritime.NMEA0183;
using Xunit;

namespace XUnitTestProjectNMEA0183
{
    public class UnitTestHMSSentence
    {
        [Fact]
        public void HMSDecoding()
        {
            String sentence = "$ECHMS,GP01X,HC02K,1.0*5F";

            NMEASentence nmeaSentence = NMEASentence.DecodeSentence(sentence);
            HMSSentence hmsSentence = nmeaSentence as HMSSentence;

            Assert.NotNull(nmeaSentence);
            Assert.NotNull(hmsSentence);
            Assert.Equal(TalkerIdentifierEnum.ElectronicChartDisplayInformationSystem, hmsSentence.TalkerIdentifier.TalkerIdentifierEnum);
            Assert.Equal("GP01X", hmsSentence.HeadingSensorOneID);
            Assert.Equal("HC02K", hmsSentence.HeadingSensorTwoID);
            Assert.Equal(1.0M, hmsSentence.MaximumDifferenceDegrees);
        }
    }
}
