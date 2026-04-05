using System;
using System.Text;

namespace TensionDev.Maritime.NMEA0183
{
    /// <summary>
    /// XTE - Cross-Track Error, Measured
    /// </summary>
    public class XTESentence : NMEASentence
    {
        /// <summary>
        /// <para>Reliable Fix Available.</para>
        /// <para>True - Data valid.</para>
        /// <para>False - Loran-C Blink or SNR warning.</para>
        /// </summary>
        public Boolean IsReliableFixAvailable { get; set; }

        /// <summary>
        /// <para>Loran-C Cycle Lock Warning.</para>
        /// <para>True - Data valid or not used.</para>
        /// <para>False - Loran-C Cycle Lock warning flag.</para>
        /// </summary>
        public Boolean IsDataValid { get; set; }

        /// <summary>
        /// Magnitude of XTE (cross-track-error) in Nautical Miles.
        /// </summary>
        public Decimal MagnitudeCrossTrackErrorNauticalMiles { get; set; }

        /// <summary>
        /// Direction to steer, L/R
        /// </summary>
        public DirectionToSteerEnum DirectionToSteer { get; set; }

        /// <summary>
        /// Mode indicator
        /// </summary>
        public ModeIndicatorEnum ModeIndicator { get; set; }

        public XTESentence()
        {
            SentenceIdentifier = "XTE";
            IsReliableFixAvailable = false;
            IsDataValid = false;
            DirectionToSteer = DirectionToSteerEnum.L;
            ModeIndicator = ModeIndicatorEnum.N;
        }

        public override String EncodeSentence()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("${0}{1},", TalkerIdentifier.ToString(), SentenceIdentifier);

            if (IsReliableFixAvailable)
                stringBuilder.Append("A,");
            else
                stringBuilder.Append("V,");

            if (IsDataValid)
                stringBuilder.Append("A,");
            else
                stringBuilder.Append("V,");

            stringBuilder.AppendFormat("{0},", MagnitudeCrossTrackErrorNauticalMiles);

            stringBuilder.AppendFormat("{0},", DirectionToSteer.ToString());

            stringBuilder.Append("N,");

            stringBuilder.AppendFormat("{0}", ModeIndicator.ToString());

            Byte checksum = CalculateChecksum(stringBuilder.ToString());

            stringBuilder.AppendFormat("*{0}\r\n", checksum.ToString("X2"));

            return stringBuilder.ToString();
        }

        protected override void DecodeInternalSentence(String sentence)
        {
            DecodeTalker(sentence);

            String[] vs = sentence.Split(',', '*');

            // A Reliable Fix Available
            if (vs[1] == "A")
                IsReliableFixAvailable = true;
            else
                IsReliableFixAvailable = false;

            // A Data Valid
            if (vs[2] == "A")
                IsDataValid = true;
            else
                IsDataValid = false;

            // x.x Magnitude of XTE (cross-track-error)
            MagnitudeCrossTrackErrorNauticalMiles = Decimal.Parse(vs[3]);

            // a Direction to steer, L/R
            DirectionToSteer = (DirectionToSteerEnum)Enum.Parse(typeof(DirectionToSteerEnum), vs[4]);

            // a Mode indicator
            if (vs.Length > 7)
                ModeIndicator = (ModeIndicatorEnum)Enum.Parse(typeof(ModeIndicatorEnum), vs[6]);
        }

        public enum DirectionToSteerEnum
        {
            /// <summary>
            /// L - Left
            /// </summary>
            L,
            /// <summary>
            /// R - Right
            /// </summary>
            R,
        }

        public enum ModeIndicatorEnum
        {
            /// <summary>
            /// Autnomous mode
            /// </summary>
            A,
            /// <summary>
            /// Differential mode
            /// </summary>
            D,
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
            /// Data not valid
            /// </summary>
            N,
        }
    }
}
