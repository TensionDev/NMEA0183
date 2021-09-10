using System;
using TensionDev.NMEA0183;
using Xunit;

namespace XUnitTestProjectNMEA0183
{
    public class UnitTestDPTSentence
    {
        [Fact]
        public void DPTDecoding()
        {
            String sentence = "$SDDPT,76.1,0.0,100*7A";

            NMEASentence nmeaSentence = NMEASentence.DecodeSentence(sentence);
            DPTSentence dptSentence = nmeaSentence as DPTSentence;

            Assert.NotNull(nmeaSentence);
            Assert.NotNull(dptSentence);
            Assert.Equal(TalkerIdentifierEnum.SounderDepth, dptSentence.TalkerIdentifier.TalkerIdentifierEnum);
            Assert.Equal(76.1M, dptSentence.DepthMetres);
            Assert.Equal(0.0M, dptSentence.OffsetFromTransducerMetres);
            Assert.Equal(100M, dptSentence.MaximumDepthRangeMetres);
        }
    }
}
