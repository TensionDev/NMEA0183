using System;
using System.Text;

namespace TensionDev.Maritime.NMEA0183
{
    /// <summary>
    /// OSD - Own Ship Data
    /// </summary>
    public class OSDSentence : NMEASentence
    {
        /// <summary>
        /// Heading in Degrees
        /// </summary>
        public Decimal HeadingTrue { get; set; }

        /// <summary>
        /// Is the Heading valid?
        /// </summary>
        public Boolean IsHeadingValid { get; set; }

        /// <summary>
        /// Vessel Course in Degrees
        /// </summary>
        public Decimal VesselCourseTrue { get; set; }

        /// <summary>
        /// Course Reference
        /// </summary>
        public ReferenceSystemEnum VesselCourseReference { get; set; }

        /// <summary>
        /// Vessel Speed
        /// </summary>
        public Decimal VesselSpeed { get; set; }

        /// <summary>
        /// Speed Reference
        /// </summary>
        public ReferenceSystemEnum VesselSpeedReference { get; set; }

        /// <summary>
        /// Vessel Set in Degrees
        /// </summary>
        public Decimal VesselSetTrue { get; set; }

        /// <summary>
        /// Vessel Drift
        /// </summary>
        public Decimal VesselDrift { get; set; }

        /// <summary>
        /// Speed units
        /// </summary>
        public SpeedUnitsEnum SpeedUnits { get; set; }

        public OSDSentence()
        {
            SentenceIdentifier = "OSD";
            IsHeadingValid = false;
            VesselCourseReference = ReferenceSystemEnum.M;
            VesselSpeedReference = ReferenceSystemEnum.M;
            SpeedUnits = SpeedUnitsEnum.N;
        }

        public override String EncodeSentence()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("${0}{1},", TalkerIdentifier.ToString(), SentenceIdentifier);

            stringBuilder.AppendFormat("{0},", HeadingTrue.ToString());

            if (IsHeadingValid)
                stringBuilder.Append("A,");
            else
                stringBuilder.Append("V,");

            stringBuilder.AppendFormat("{0},", VesselCourseTrue.ToString());

            stringBuilder.AppendFormat("{0},", VesselCourseReference.ToString());

            stringBuilder.AppendFormat("{0},", VesselSpeed.ToString());

            stringBuilder.AppendFormat("{0},", VesselSpeedReference.ToString());

            stringBuilder.AppendFormat("{0},", VesselSetTrue.ToString());

            stringBuilder.AppendFormat("{0},", VesselDrift.ToString());

            stringBuilder.AppendFormat("{0}", SpeedUnits.ToString());

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

            // Heading Status
            if (vs[2] == "A")
                IsHeadingValid = true;
            else
                IsHeadingValid = false;

            // x.x Vessel Course True
            VesselCourseTrue = Decimal.Parse(vs[3]);

            // a Course Reference
            VesselCourseReference = (ReferenceSystemEnum)Enum.Parse(typeof(ReferenceSystemEnum), vs[4]);

            // x.x Vessel Speed
            VesselSpeed = Decimal.Parse(vs[5]);

            // a Course Reference
            VesselSpeedReference = (ReferenceSystemEnum)Enum.Parse(typeof(ReferenceSystemEnum), vs[6]);

            // x.x Vessel Set True
            VesselSetTrue = Decimal.Parse(vs[7]);

            // x.x Vessel Drift
            VesselDrift = Decimal.Parse(vs[8]);

            // a Speed Units
            SpeedUnits = (SpeedUnitsEnum)Enum.Parse(typeof(SpeedUnitsEnum), vs[9]);
        }

        public enum ReferenceSystemEnum
        {
            /// <summary>
            /// Bottom Tracking Log
            /// </summary>
            B,
            /// <summary>
            /// Manually Entered
            /// </summary>
            M,
            /// <summary>
            /// Water Referenced
            /// </summary>
            W,
            /// <summary>
            /// Radar Tracking (of Fixed Target)
            /// </summary>
            R,
            /// <summary>
            /// Positioning System Ground Reference
            /// </summary>
            P,
        }

        public enum SpeedUnitsEnum
        {
            /// <summary>
            /// km/h - Kilometres per Hour
            /// </summary>
            K,
            /// <summary>
            /// Knots - Nautical Mile per Hour
            /// </summary>
            N,
            /// <summary>
            /// mi/h - Statute Mile per Hour
            /// </summary>
            S,
        }
    }
}
