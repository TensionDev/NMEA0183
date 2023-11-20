using System;
using TensionDev.Maritime.NMEA0183;
using Xunit;

namespace XUnitTestProjectNMEA0183
{
    public class UnitTestFSISentence
    {
        [Fact]
        public void FSIDecoding()
        {
            String sentence = "$CTFSI,020230,026140,m,9*1D";

            NMEASentence nmeaSentence = NMEASentence.DecodeSentence(sentence);
            FSISentence fsiSentence = nmeaSentence as FSISentence;

            Assert.NotNull(nmeaSentence);
            Assert.NotNull(fsiSentence);
            Assert.Equal(TalkerIdentifierEnum.CommunicationsRadioTelephoneMFOrHF, fsiSentence.TalkerIdentifier.TalkerIdentifierEnum);
            Assert.Equal("020230", fsiSentence.TransmittingFrequency);
            Assert.Equal("026140", fsiSentence.ReceivingFrequency);
            Assert.Equal(FSISentence.ModeOfOperationEnum.J3E_Telephone, fsiSentence.ModeOfOperation);
            Assert.Equal(9u, fsiSentence.PowerLevel);
        }

        [Fact]
        public void FSIDecoding_NullVariables()
        {
            String sentence = "$CTFSI,,,,0*7B";

            NMEASentence nmeaSentence = NMEASentence.DecodeSentence(sentence);
            FSISentence fsiSentence = nmeaSentence as FSISentence;

            Assert.NotNull(nmeaSentence);
            Assert.NotNull(fsiSentence);
            Assert.Equal(TalkerIdentifierEnum.CommunicationsRadioTelephoneMFOrHF, fsiSentence.TalkerIdentifier.TalkerIdentifierEnum);
            Assert.Null(fsiSentence.TransmittingFrequency);
            Assert.Null(fsiSentence.ReceivingFrequency);
            Assert.Null(fsiSentence.ModeOfOperation);
            Assert.Equal(0u, fsiSentence.PowerLevel);
        }
    }
}
