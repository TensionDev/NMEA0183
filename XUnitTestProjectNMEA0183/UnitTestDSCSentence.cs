using System;
using TensionDev.Maritime.NMEA0183;
using Xunit;

namespace XUnitTestProjectNMEA0183
{
    public class UnitTestDSCSentence
    {
        [Fact]
        public void DSCDecoding()
        {
            String sentence = "$CDDSC,20,3380400790,00,21,26,1423108312,2021,,,B,E*73";

            NMEASentence nmeaSentence = NMEASentence.DecodeSentence(sentence);
            DSCSentence dscSentence = nmeaSentence as DSCSentence;

            Assert.NotNull(nmeaSentence);
            Assert.NotNull(dscSentence);
            Assert.Equal(TalkerIdentifierEnum.CommunicationsDigitalSelectiveCalling, dscSentence.TalkerIdentifier.TalkerIdentifierEnum);
        }
    }
}
