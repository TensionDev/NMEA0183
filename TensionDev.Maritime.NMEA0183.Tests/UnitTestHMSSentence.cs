using System;
using TensionDev.Maritime.NMEA0183;
using Xunit;

namespace TensionDev.Maritime.NMEA0183.Tests
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

        [Fact]
        public void HMSEncoding()
        {
            String expected = "$ECHMS,GP01X,HC02K,1.0*5F\r\n";
            HMSSentence hmsSentence = new HMSSentence()
            {
                TalkerIdentifier = new TalkerIdentifier()
                {
                    TalkerIdentifierEnum = TalkerIdentifierEnum.ElectronicChartDisplayInformationSystem,
                },
                HeadingSensorOneID = "GP01X",
                HeadingSensorTwoID = "HC02K",
                MaximumDifferenceDegrees = 1.0M,
            };
            string actual = hmsSentence.EncodeSentence();
            Assert.Equal(expected, actual);
        }
    }
}
