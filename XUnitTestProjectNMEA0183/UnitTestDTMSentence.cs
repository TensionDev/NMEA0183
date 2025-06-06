﻿using System;
using TensionDev.Maritime.NMEA0183;
using Xunit;

namespace XUnitTestProjectNMEA0183
{
    public class UnitTestDTMSentence
    {
        [Fact]
        public void DTMDecoding()
        {
            String sentence = "$GPDTM,999,,0.002,S,0.005,E,005.8,W84*1A";

            NMEASentence nmeaSentence = NMEASentence.DecodeSentence(sentence);
            DTMSentence dtmSentence = nmeaSentence as DTMSentence;

            Assert.NotNull(nmeaSentence);
            Assert.NotNull(dtmSentence);
            Assert.Equal(TalkerIdentifierEnum.GlobalPositioningSystem, dtmSentence.TalkerIdentifier.TalkerIdentifierEnum);
            Assert.Equal("999", dtmSentence.LocalDatumCode);
            Assert.Equal("", dtmSentence.LocalDatumSubdivisionCode);
            Assert.Equal(0.002M, dtmSentence.LatitudeOffsetMinutes);
            Assert.Equal(DTMSentence.LatitudeOffsetDirectionEnum.S, dtmSentence.LatitudeOffsetDirection);
            Assert.Equal(0.005M, dtmSentence.LongitudeOffsetMinutes);
            Assert.Equal(DTMSentence.LongitudeOffsetDirectionEnum.E, dtmSentence.LongitudeOffsetDirection);
            Assert.Equal(5.8M, dtmSentence.AltitudeOffsetMetres);
            Assert.Equal(DTMSentence.ReferenceDatumCodeEnum.W84, dtmSentence.ReferenceDatumCode);
        }

        [Fact]
        public void DTMEncoding()
        {
            String expected = "$GPDTM,999,,0.002,S,0.005,E,005.8,W84*1A\r\n";

            DTMSentence dtmSentence = new DTMSentence()
            {
                TalkerIdentifier = new TalkerIdentifier()
                {
                    TalkerIdentifierEnum = TalkerIdentifierEnum.GlobalPositioningSystem,
                },
                LocalDatumCode = "999",
                LocalDatumSubdivisionCode = "",
                LatitudeOffsetMinutes = 0.002M,
                LatitudeOffsetDirection = DTMSentence.LatitudeOffsetDirectionEnum.S,
                LongitudeOffsetMinutes = 0.005M,
                LongitudeOffsetDirection = DTMSentence.LongitudeOffsetDirectionEnum.E,
                AltitudeOffsetMetres = 5.8M,
                ReferenceDatumCode = DTMSentence.ReferenceDatumCodeEnum.W84,
            };

            string actual = dtmSentence.EncodeSentence();

            Assert.Equal(expected, actual);
        }
    }
}
