using System;
using System.Text;

namespace TensionDev.Maritime.NMEA0183
{
    /// <summary>
    /// VTG - Course Over Ground and Ground Speed
    /// </summary>
    public class VTGSentence : NMEASentence
    {
        /// <summary>
        /// Course Over Ground in Degrees True
        /// </summary>
        public Decimal CourseOverGroundTrue { get; set; }

        /// <summary>
        /// Course Over Ground in Degrees Magnetic
        /// </summary>
        public Decimal CourseOverGroundMagnetic { get; set; }

        /// <summary>
        /// Speed Over Ground in Knots
        /// </summary>
        public Decimal SpeedOverGroundKnots { get; set; }

        /// <summary>
        /// Speed Over Ground in Kilometres per Hour
        /// </summary>
        public Decimal SpeedOverGroundKmh { get; set; }

        /// <summary>
        /// Mode Indicator
        /// </summary>
        public ModeIndicatorEnum ModeIndicator { get; set; }

        public VTGSentence()
        {
            SentenceIdentifier = "VTG";
            ModeIndicator = ModeIndicatorEnum.N;
        }

        public override String EncodeSentence()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("${0}{1},", TalkerIdentifier.ToString(), SentenceIdentifier);

            stringBuilder.AppendFormat("{0},T,", CourseOverGroundTrue);

            stringBuilder.AppendFormat("{0},M,", CourseOverGroundMagnetic);

            stringBuilder.AppendFormat("{0},N,", SpeedOverGroundKnots);

            stringBuilder.AppendFormat("{0},K,", SpeedOverGroundKmh);

            stringBuilder.AppendFormat("{0}", ModeIndicator.ToString());

            Byte checksum = CalculateChecksum(stringBuilder.ToString());

            stringBuilder.AppendFormat("*{0}\r\n", checksum.ToString("X2"));

            return stringBuilder.ToString();
        }

        protected override void DecodeInternalSentence(String sentence)
        {
            DecodeTalker(sentence);

            String[] vs = sentence.Split(',', '*');

            // x.x Course Over Ground in Degrees True
            CourseOverGroundTrue = Decimal.Parse(vs[1]);

            // x.x Course Over Ground in Degrees Magnetic
            CourseOverGroundMagnetic = Decimal.Parse(vs[3]);

            // x.x Speed Over Ground in Knots
            SpeedOverGroundKnots = Decimal.Parse(vs[5]);

            // x.x Speed Over Ground in Km/h
            SpeedOverGroundKmh = Decimal.Parse(vs[7]);

            // a Mode Indicator
            if (vs.Length > 10)
                ModeIndicator = (ModeIndicatorEnum)Enum.Parse(typeof(ModeIndicatorEnum), vs[9]);
        }

        public enum ModeIndicatorEnum
        {
            /// <summary>
            /// Autonomous mode
            /// </summary>
            A,
            /// <summary>
            /// Differential mode
            /// </summary>
            D,
            /// <summary>
            /// Estimated (dead reckoning) mode
            /// </summary>
            E,
            /// <summary>
            /// Manual input mode
            /// </summary>
            M,
            /// <summary>
            /// Simulator mode
            /// </summary>
            S,
            /// <summary>
            /// Data not valid
            /// </summary>
            N,
        }
    }
}
