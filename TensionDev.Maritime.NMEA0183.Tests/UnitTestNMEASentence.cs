using System;
using TensionDev.Maritime.NMEA0183;
using Xunit;

namespace TensionDev.Maritime.NMEA0183.Tests
{
    public class UnitTestNMEASentence
    {
        [Theory]
        [InlineData("$GPGLL,5057.970,N,00146.110,E,142451,A*27")]
        [InlineData("$GPVTG,089.0,T,,,15.2,N,,*7F")]
        public void IsChecksumValid(String sentence)
        {
            Boolean result = NMEASentence.IsChecksumValid(sentence);

            Assert.True(result);
        }
    }
}
