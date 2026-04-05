using System;
using System.Collections.Generic;
using System.Text;

namespace TensionDev.Maritime.NMEA0183
{
    /// <summary>
    /// RPM - Revolutions
    /// </summary>
    public class RPMSentence : NMEASentence
    {
        /// <summary>
        /// Source, shaft/engine S/E
        /// </summary>
        public SourceEnum Source { get; set; }

        /// <summary>
        /// <para>Engine or shaft number, numbered from centerline</para>
        /// <para>0 = single or on centerline</para>
        /// <para>odd = starboard</para>
        /// <para>even = port</para>
        /// </summary>
        public UInt32 EngineOrShaftNumber { get; set; }

        /// <summary>
        /// Speed, rev/min, "-" = counter-clockwise
        /// </summary>
        public Decimal SpeedRevPerMinute { get; set; }

        /// <summary>
        /// Propeller pitch, % of max., "-" = astern
        /// </summary>
        public Decimal PropellerPitchPercentage { get; set; }

        /// <summary>
        /// Status,
        /// True = Data valid,
        /// False = Data invalid
        /// </summary>
        public Boolean Status { get; set; }

        public RPMSentence()
        {
            SentenceIdentifier = "RPM";
        }

        public override String EncodeSentence()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("${0}{1},", TalkerIdentifier.ToString(), SentenceIdentifier);

            stringBuilder.AppendFormat("{0},", Source.ToString());

            stringBuilder.AppendFormat("{0},", EngineOrShaftNumber);

            stringBuilder.AppendFormat("{0},", SpeedRevPerMinute);

            stringBuilder.AppendFormat("{0},", PropellerPitchPercentage);

            if (Status)
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

            String[] vs = sentence.Split(',', '*');

            // a Source, shaft/engine S/E
            Source = (SourceEnum)Enum.Parse(typeof(SourceEnum), vs[1]);

            // x Engine or shaft number, numbered from centerline
            EngineOrShaftNumber = UInt32.Parse(vs[2]);

            // x.x Speed, rev/min
            SpeedRevPerMinute = Decimal.Parse(vs[3]);

            // x.x Propeller pitch, % of max
            PropellerPitchPercentage = Decimal.Parse(vs[4]);

            // A Status
            if (vs[5] == "A")
                Status = true;
            else
                Status = false;
        }

        public enum SourceEnum
        {
            /// <summary>
            /// S - Shaft
            /// </summary>
            S,
            /// <summary>
            /// E - Engine
            /// </summary>
            E,
        }
    }
}
