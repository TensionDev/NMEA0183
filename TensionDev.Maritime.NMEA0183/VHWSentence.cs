using System;
using System.Text;

namespace TensionDev.Maritime.NMEA0183
{
    /// <summary>
    /// VHW - Water Speed and Heading
    /// </summary>
    public class VHWSentence : NMEASentence
    {
        /// <summary>
        /// Heading in Degrees True
        /// </summary>
        public Decimal HeadingTrue { get; set; }

        /// <summary>
        /// Heading in Degrees Magnetic
        /// </summary>
        public Decimal HeadingMagnetic { get; set; }

        /// <summary>
        /// Speed in Knots
        /// </summary>
        public Decimal SpeedKnots { get; set; }

        /// <summary>
        /// Speed in Kilometres per Hour
        /// </summary>
        public Decimal SpeedKmh { get; set; }

        public VHWSentence()
        {
            SentenceIdentifier = "VHW";
        }

        public override String EncodeSentence()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("${0}{1},", TalkerIdentifier.ToString(), SentenceIdentifier);

            stringBuilder.AppendFormat("{0},T,", HeadingTrue);

            stringBuilder.AppendFormat("{0},M,", HeadingMagnetic);

            stringBuilder.AppendFormat("{0},N,", SpeedKnots);

            stringBuilder.AppendFormat("{0},K", SpeedKmh);

            Byte checksum = CalculateChecksum(stringBuilder.ToString());

            stringBuilder.AppendFormat("*{0}\r\n", checksum.ToString("X2"));

            return stringBuilder.ToString();
        }

        protected override void DecodeInternalSentence(String sentence)
        {
            DecodeTalker(sentence);

            String[] vs = sentence.Split(',', '*');

            // x.x Heading, degrees True 
            HeadingTrue = Decimal.Parse(vs[1]);

            // x.x Heading, degrees Magnetic 
            HeadingMagnetic = Decimal.Parse(vs[3]);

            // x.x Speed, knots
            SpeedKnots = Decimal.Parse(vs[5]);

            // x.x Speed, Km/h
            SpeedKmh = Decimal.Parse(vs[7]);
        }
    }
}
