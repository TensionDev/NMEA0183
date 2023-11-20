using System;
using TensionDev.Maritime.NMEA0183;
using Xunit;

namespace XUnitTestProjectNMEA0183
{
    public class UnitTestRSASentence
    {
        [Fact]
        public void RSADecoding()
        {
            String sentence = "$ERRSA,-10.0,A,0.0,V*5C";

            NMEASentence nmeaSentence = NMEASentence.DecodeSentence(sentence);
            RSASentence rsaSentence = nmeaSentence as RSASentence;

            Assert.NotNull(nmeaSentence);
            Assert.NotNull(rsaSentence);
            Assert.Equal(TalkerIdentifierEnum.EngineRoomMonitoringSystems, rsaSentence.TalkerIdentifier.TalkerIdentifierEnum);
            Assert.Equal(-10.0M, rsaSentence.StarboardRudderSensor);
            Assert.True(rsaSentence.StarboardRudderSensorStatus);
            Assert.Equal(0.0M, rsaSentence.PortRudderSensor);
            Assert.False(rsaSentence.PortRudderSensorStatus);
        }
    }
}
