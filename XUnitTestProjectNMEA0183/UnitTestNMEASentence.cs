using System;
using TensionDev.Maritime.NMEA0183;
using Xunit;

namespace XUnitTestProjectNMEA0183
{
    public class UnitTestNMEASentence
    {
        [Fact]
        public void IsChecksumValid1()
        {
            String sentence = "$GPGLL,5057.970,N,00146.110,E,142451,A*27";

            Boolean result = NMEASentence.IsChecksumValid(sentence);

            Assert.True(result);
        }

        [Fact]
        public void IsChecksumValid2()
        {
            String sentence = "$GPVTG,089.0,T,,,15.2,N,,*7F";

            Boolean result = NMEASentence.IsChecksumValid(sentence);

            Assert.True(result);
        }
    }
}
