using System;
using System.Text;

namespace TensionDev.Maritime.NMEA0183
{
    /// <summary>
    /// DPT - Depth of Water
    /// </summary>
    public class DPTSentence : NMEASentence
    {
        /// <summary>
        /// Depth in Metres
        /// </summary>
        public Decimal DepthMetres { get; set; }

        /// <summary>
        /// Offset from transducer;
        /// positive means distance from transducer to water line,
        /// negative means distance from transducer to keel
        /// </summary>
        public Decimal OffsetFromTransducerMetres { get; set; }

        /// <summary>
        /// Maximum Depth Range of the transducer in Metres
        /// </summary>
        public Decimal MaximumDepthRangeMetres { get; set; }

        public DPTSentence()
        {
            SentenceIdentifier = "DPT";
        }

        public override String EncodeSentence()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("${0}{1},", TalkerIdentifier.ToString(), SentenceIdentifier);

            stringBuilder.AppendFormat("{0},", DepthMetres.ToString());

            stringBuilder.AppendFormat("{0},", OffsetFromTransducerMetres.ToString());

            stringBuilder.AppendFormat("{0}", MaximumDepthRangeMetres.ToString());

            Byte checksum = CalculateChecksum(stringBuilder.ToString());

            stringBuilder.AppendFormat("*{0}\r\n", checksum.ToString("X2"));

            return stringBuilder.ToString();
        }

        protected override void DecodeInternalSentence(String sentence)
        {
            DecodeTalker(sentence);

            String[] vs = sentence.Split(',', '*');

            // x.x Water Depth
            DepthMetres = Decimal.Parse(vs[1]);

            // x.x Transducer Offset
            OffsetFromTransducerMetres = Decimal.Parse(vs[2]);

            // x.x Maximum Range Scale
            if (vs.Length > 4)
                MaximumDepthRangeMetres = Decimal.Parse(vs[3]);
        }
    }
}
