using System;
using System.Text;

namespace TensionDev.NMEA0183
{
    /// <summary>
    /// HDT - Heading - True
    /// </summary>
    public class HDTSentence : NMEASentence
    {
        /// <summary>
        /// Heading in Degrees
        /// </summary>
        public Decimal HeadingTrue { get; set; }

        public HDTSentence()
        {
            SentenceIdentifier = "HDT";
        }

        public override String EncodeSentence()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("${0}{1},", TalkerIdentifier.ToString(), SentenceIdentifier);

            stringBuilder.AppendFormat("{0},T", HeadingTrue.ToString());

            Byte checksum = CalculateChecksum(stringBuilder.ToString());

            stringBuilder.AppendFormat("*{0}\r\n", checksum.ToString("X2"));

            return stringBuilder.ToString();
        }

        protected override void DecodeInternalSentence(String sentence)
        {
            DecodeTalker(sentence);

            String[] vs = sentence.Split(new char[] { ',', '*' });

            // x.x Heading True
            HeadingTrue = Decimal.Parse(vs[1]);
        }
    }
}
