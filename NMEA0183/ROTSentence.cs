using System;
using System.Collections.Generic;
using System.Text;

namespace TensionDev.Maritime.NMEA0183
{
    /// <summary>
    /// HDT - Heading, Deviation & Variation
    /// </summary>
    public class ROTSentence : NMEASentence
    {
        /// <summary>
        /// Rate of turn in Degrees per Minute
        /// Negative means bow turns to port.
        /// </summary>
        public Decimal RateOfTurn { get; set; }

        /// <summary>
        /// Is the data valid?
        /// </summary>
        public Boolean IsDataValid { get; set; }

        public ROTSentence()
        {
            SentenceIdentifier = "ROT";
        }

        public override String EncodeSentence()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("${0}{1},", TalkerIdentifier.ToString(), SentenceIdentifier);

            stringBuilder.AppendFormat("{0},", RateOfTurn.ToString());

            if (IsDataValid)
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

            // x.x Rate of turn
            RateOfTurn = Decimal.Parse(vs[1]);

            // A Status
            if (vs[2] == "A")
                IsDataValid = true;
            else
                IsDataValid = false;
        }
    }
}
