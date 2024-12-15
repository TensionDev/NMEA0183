using System;
using System.Collections.Generic;
using System.Text;

namespace TensionDev.Maritime.NMEA0183
{
    /// <summary>
    /// THS - True Heading and Status
    /// </summary>
    public class THSSentence : NMEASentence
    {
        /// <summary>
        /// Heading in Degrees
        /// </summary>
        public Decimal HeadingTrue { get; set; }

        /// <summary>
        /// Mode Indicator
        /// </summary>
        public ModeIndicatorEnum ModeIndicator { get; set; }

        public THSSentence()
        {
            SentenceIdentifier = "THS";
            ModeIndicator = ModeIndicatorEnum.V;
        }

        public override String EncodeSentence()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("${0}{1},", TalkerIdentifier.ToString(), SentenceIdentifier);

            stringBuilder.AppendFormat("{0},", HeadingTrue);

            stringBuilder.AppendFormat("{0}", ModeIndicator.ToString());

            Byte checksum = CalculateChecksum(stringBuilder.ToString());

            stringBuilder.AppendFormat("*{0}\r\n", checksum.ToString("X2"));

            return stringBuilder.ToString();
        }

        protected override void DecodeInternalSentence(String sentence)
        {
            DecodeTalker(sentence);

            String[] vs = sentence.Split(',', '*');

            // x.x Heading True
            HeadingTrue = Decimal.Parse(vs[1]);

            // a Mode Indicator
            ModeIndicator = (ModeIndicatorEnum)Enum.Parse(typeof(ModeIndicatorEnum), vs[2]);
        }

        public enum ModeIndicatorEnum
        {
            /// <summary>
            /// Autnomous
            /// </summary>
            A,
            /// <summary>
            /// Estimated (Dead-Reckoning)
            /// </summary>
            E,
            /// <summary>
            /// Manual Input
            /// </summary>
            M,
            /// <summary>
            /// Simulator
            /// </summary>
            S,
            /// <summary>
            /// Data not valid (including standby)
            /// </summary>
            V,
        }
    }
}
