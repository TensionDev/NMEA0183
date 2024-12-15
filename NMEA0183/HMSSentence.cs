using System;
using System.Text;

namespace TensionDev.Maritime.NMEA0183
{
    /// <summary>
    /// HMS - Heading Monitor Set
    /// </summary>
    public class HMSSentence : NMEASentence
    {
        /// <summary>
        /// Heading Sensor 1 ID
        /// </summary>
        public String HeadingSensorOneID { get; set; }

        /// <summary>
        /// Heading Sensor 2 ID
        /// </summary>
        public String HeadingSensorTwoID { get; set; }

        /// <summary>
        /// Maximum difference, degrees 
        /// </summary>
        public Decimal MaximumDifferenceDegrees { get; set; }

        public HMSSentence()
        {
            SentenceIdentifier = "HMS";
        }

        public override String EncodeSentence()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("${0}{1},", TalkerIdentifier.ToString(), SentenceIdentifier);

            stringBuilder.AppendFormat("{0},", HeadingSensorOneID);

            stringBuilder.AppendFormat("{0},", HeadingSensorTwoID);

            stringBuilder.AppendFormat("{0}", MaximumDifferenceDegrees);

            Byte checksum = CalculateChecksum(stringBuilder.ToString());

            stringBuilder.AppendFormat("*{0}\r\n", checksum.ToString("X2"));

            return stringBuilder.ToString();
        }

        protected override void DecodeInternalSentence(string sentence)
        {
            DecodeTalker(sentence);

            String[] vs = sentence.Split(',', '*');

            // c--c Heading Sensor 1 ID
            HeadingSensorOneID = vs[1];

            // c--c Heading Sensor 2 ID
            HeadingSensorTwoID = vs[2];

            // x.x Maximum difference, degrees
            MaximumDifferenceDegrees = Decimal.Parse(vs[3]);
        }
    }
}
