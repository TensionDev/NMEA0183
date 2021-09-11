using System;
using TensionDev.Maritime.NMEA0183;
using Xunit;

namespace XUnitTestProjectNMEA0183
{
    public class UnitTestVTGSentence
    {
        [Fact]
        public void VTGDecoding()
        {
            String sentence = "$GPVTG,191.4,T,191.6,M,25.2,N,46.6,K,A*04";

            NMEASentence nmeaSentence = NMEASentence.DecodeSentence(sentence);
            VTGSentence vtgSentence = nmeaSentence as VTGSentence;

            Assert.NotNull(nmeaSentence);
            Assert.NotNull(vtgSentence);
            Assert.Equal(TalkerIdentifierEnum.GlobalPositioningSystem, vtgSentence.TalkerIdentifier.TalkerIdentifierEnum);
            Assert.Equal(191.4M, vtgSentence.CourseOverGroundTrue);
            Assert.Equal(191.6M, vtgSentence.CourseOverGroundMagnetic);
            Assert.Equal(25.2M, vtgSentence.SpeedOverGroundKnots);
            Assert.Equal(46.6M, vtgSentence.SpeedOverGroundKmh);
            Assert.Equal(VTGSentence.ModeIndicatorEnum.A, vtgSentence.ModeIndicator);
        }
    }
}
