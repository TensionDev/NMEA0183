using System;
using TensionDev.Maritime.NMEA0183;
using Xunit;

namespace XUnitTestProjectNMEA0183
{
    public class UnitTestRPMSentence
    {
        [Fact]
        public void RPMDecoding()
        {
            String sentence = "$ERRPM,E,0,120.0,10.0,A*72";

            NMEASentence nmeaSentence = NMEASentence.DecodeSentence(sentence);
            RPMSentence rpmSentence = nmeaSentence as RPMSentence;

            Assert.NotNull(nmeaSentence);
            Assert.NotNull(rpmSentence);
            Assert.Equal(TalkerIdentifierEnum.EngineRoomMonitoringSystems, rpmSentence.TalkerIdentifier.TalkerIdentifierEnum);
            Assert.Equal(RPMSentence.SourceEnum.E, rpmSentence.Source);
            Assert.Equal(0u, rpmSentence.EngineOrShaftNumber);
            Assert.Equal(120.0M, rpmSentence.SpeedRevPerMinute);
            Assert.Equal(10.0M, rpmSentence.PropellerPitchPercentage);
            Assert.True(rpmSentence.Status);
        }
    }
}
