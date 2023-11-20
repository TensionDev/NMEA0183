using System;
using System.Collections.Generic;
using System.Text;

namespace TensionDev.Maritime.NMEA0183
{
    /// <summary>
    /// RSA - Rudder Sensor Angle
    /// </summary>
    public class RSASentence : NMEASentence
    {
        /// <summary>
        /// <para>Starboard (or single) rudder sensor</para>
        /// <para>Relative measurement of rudder angle without units, "-" = "Bow Turns To Port".<br />
        /// Sensor output is proportional to rudder angle but not necessarily 1:1.</para>
        /// </summary>
        public Decimal StarboardRudderSensor { get; set; }

        /// <summary>
        /// Status, True = Data Valid, False = Data Invalid
        /// </summary>
        public Boolean StarboardRudderSensorStatus { get; set; }

        /// <summary>
        /// <para>Port rudder sensor</para>
        /// <para>Relative measurement of rudder angle without units, "-" = "Bow Turns To Port".<br />
        /// Sensor output is proportional to rudder angle but not necessarily 1:1.</para>
        /// </summary>
        public Decimal PortRudderSensor { get; set; }

        /// <summary>
        /// Status, True = Data Valid, False = Data Invalid
        /// </summary>
        public Boolean PortRudderSensorStatus { get; set; }

        public RSASentence()
        {
            SentenceIdentifier = "RSA";
        }

        public override String EncodeSentence()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("${0}{1},", TalkerIdentifier.ToString(), SentenceIdentifier);

            stringBuilder.AppendFormat("{0},", StarboardRudderSensor);

            if (StarboardRudderSensorStatus)
                stringBuilder.Append("A,");
            else
                stringBuilder.Append("V,");

            stringBuilder.AppendFormat("{0},", PortRudderSensor);

            if (PortRudderSensorStatus)
                stringBuilder.Append("A");
            else
                stringBuilder.Append("V");

            Byte checksum = CalculateChecksum(stringBuilder.ToString());

            stringBuilder.AppendFormat("*{0}\r\n", checksum.ToString("X2"));

            return stringBuilder.ToString();
        }

        protected override void DecodeInternalSentence(String sentence)
        {
            DecodeTalker(sentence);

            String[] vs = sentence.Split(new char[] { ',', '*' });

            // x.x Starboard (or single) rudder sensor
            StarboardRudderSensor = Decimal.Parse(vs[1]);

            // A Status
            if (vs[2] == "A")
                StarboardRudderSensorStatus = true;
            else
                StarboardRudderSensorStatus = false;

            // x.x Port rudder sensor
            PortRudderSensor = Decimal.Parse(vs[3]);

            // A Status
            if (vs[4] == "A")
                PortRudderSensorStatus = true;
            else
                PortRudderSensorStatus = false;
        }
    }
}
