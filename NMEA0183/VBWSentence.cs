using System;
using System.Collections.Generic;
using System.Text;

namespace TensionDev.Maritime.NMEA0183
{
    /// <summary>
    /// VBW - Dual Ground/Water Speed
    /// </summary>
    public class VBWSentence : NMEASentence
    {
        /// <summary>
        /// Longitudinal water speed in knots.
        /// Negative means moving towards astern.
        /// </summary>
        public Decimal LongitudinalWaterSpeedKnots { get; set; }

        /// <summary>
        /// Transverse water speed in knots.
        /// Negative means moving towards port.
        /// </summary>
        public Decimal TransverseWaterSpeedKnots { get; set; }

        /// <summary>
        /// Is the data for Water Speed valid?
        /// </summary>
        public Boolean IsWaterDataValid { get; set; }

        /// <summary>
        /// Longitudinal ground speed in knots.
        /// Negative means moving towards astern.
        /// </summary>
        public Decimal LongitudinalGroundSpeedKnots { get; set; }

        /// <summary>
        /// Transverse ground speed in knots.
        /// Negative means moving towards port.
        /// </summary>
        public Decimal TransverseGroundSpeedKnots { get; set; }

        /// <summary>
        /// Is the data for Ground Speed valid?
        /// </summary>
        public Boolean IsGroundDataValid { get; set; }

        /// <summary>
        /// Stern Transverse water speed in knots.
        /// Negative means moving towards port.
        /// </summary>
        public Decimal SternTransverseWaterSpeedKnots { get; set; }

        /// <summary>
        /// Is the data for Stern Water Speed valid?
        /// </summary>
        public Boolean IsSternWaterDataValid { get; set; }

        /// <summary>
        /// Stern Transverse ground speed in knots.
        /// Negative means moving towards port.
        /// </summary>
        public Decimal SternTransverseGroundSpeedKnots { get; set; }

        /// <summary>
        /// Is the data for Stern Ground Speed valid?
        /// </summary>
        public Boolean IsSternGroundDataValid { get; set; }

        public VBWSentence()
        {
            SentenceIdentifier = "VBW";
            IsWaterDataValid = false;
            IsGroundDataValid = false;
            IsSternWaterDataValid = false;
            IsSternGroundDataValid = false;
        }

        public override String EncodeSentence()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("${0}{1},", TalkerIdentifier.ToString(), SentenceIdentifier);

            stringBuilder.AppendFormat("{0},", LongitudinalWaterSpeedKnots);

            stringBuilder.AppendFormat("{0},", TransverseWaterSpeedKnots);

            if (IsWaterDataValid)
                stringBuilder.Append("A,");
            else
                stringBuilder.Append("V,");

            stringBuilder.AppendFormat("{0},", LongitudinalGroundSpeedKnots);

            stringBuilder.AppendFormat("{0},", TransverseGroundSpeedKnots);

            if (IsGroundDataValid)
                stringBuilder.Append("A,");
            else
                stringBuilder.Append("V,");

            stringBuilder.AppendFormat("{0},", SternTransverseWaterSpeedKnots);

            if (IsSternWaterDataValid)
                stringBuilder.Append("A,");
            else
                stringBuilder.Append("V,");

            stringBuilder.AppendFormat("{0},", SternTransverseGroundSpeedKnots);

            if (IsSternGroundDataValid)
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

            // x.x Longitudinal Water Speed in Knots
            LongitudinalWaterSpeedKnots = Decimal.Parse(vs[1]);

            // x.x Transverse Water Speed in Knots
            TransverseWaterSpeedKnots = Decimal.Parse(vs[2]);

            // A Water Speed Data Valid?
            IsWaterDataValid = (vs[3] == "A");

            // x.x Longitudinal Ground Speed in Knots
            LongitudinalGroundSpeedKnots = Decimal.Parse(vs[4]);

            // x.x Transverse Ground Speed in Knots
            TransverseGroundSpeedKnots = Decimal.Parse(vs[5]);

            // A Ground Speed Data Valid?
            IsGroundDataValid = (vs[6] == "A");

            // x.x Stern Transverse Water Speed in Knots
            SternTransverseWaterSpeedKnots = Decimal.Parse(vs[7]);

            // A Stern Water Speed Data Valid?
            IsSternWaterDataValid = (vs[8] == "A");

            // x.x Stern Transverse Ground Speed in Knots
            SternTransverseGroundSpeedKnots = Decimal.Parse(vs[9]);

            // A Stern Ground Speed Data Valid?
            IsSternGroundDataValid = (vs[10] == "A");
        }
    }
}
