﻿using System;
using TensionDev.Maritime.NMEA0183;
using Xunit;

namespace XUnitTestProjectNMEA0183
{
    public class UnitTestZDASentence
    {
        [Fact]
        public void ZDADecoding()
        {
            String sentence = "$GPZDA,160012.71,11,03,2004,-1,00*7D";
            DateTime dateTimeUTC = new DateTime(2004, 03, 11, 16, 00, 12, 710, DateTimeKind.Utc);
            TimeSpan timeZone = new TimeSpan(1, 0, 0);

            NMEASentence nmeaSentence = NMEASentence.DecodeSentence(sentence);
            ZDASentence zdaSentence = nmeaSentence as ZDASentence;

            Assert.NotNull(nmeaSentence);
            Assert.NotNull(zdaSentence);
            Assert.Equal(TalkerIdentifierEnum.GlobalPositioningSystem, zdaSentence.TalkerIdentifier.TalkerIdentifierEnum);
            Assert.Equal(dateTimeUTC, zdaSentence.UTCDateTimeAtPosition);
            Assert.Equal(timeZone, zdaSentence.LocalTimeZone);
        }

        [Fact]
        public void ZDAEncoding()
        {
            String expected = "$GPZDA,160012.71,11,03,2004,-01,00*4D\r\n";

            ZDASentence zdaSentence = new ZDASentence()
            {
                TalkerIdentifier = new TalkerIdentifier()
                {
                    TalkerIdentifierEnum = TalkerIdentifierEnum.GlobalPositioningSystem,
                },
                UTCDateTimeAtPosition = new DateTime(2004, 03, 11, 16, 00, 12, 710, DateTimeKind.Utc),
                LocalTimeZone = new TimeSpan(1, 0, 0),
            };

            string actual = zdaSentence.EncodeSentence();

            Assert.Equal(expected, actual);
        }
    }
}
