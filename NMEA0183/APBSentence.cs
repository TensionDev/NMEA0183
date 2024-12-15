using System;
using System.Text;

namespace TensionDev.Maritime.NMEA0183
{
    /// <summary>
    /// APB - Heading / Track Controller (Autopilot) Sentence "B"
    /// </summary>
    public class APBSentence : NMEASentence
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
        /// <para>Arrival Circle Entered Status.</para>
        /// <para>True - Arrival Circle Entered.</para>
        /// <para>False - Arrival Circle not Entered.</para>
        /// </summary>
        public Boolean IsArrivalCircleEntered { get; set; }

        /// <summary>
        /// <para>Perpendicular Passed Status.</para>
        /// <para>True - Perpendicular Passed at Waypoint.</para>
        /// <para>False - Perpendicular not Passed.</para>
        /// </summary>
        public Boolean IsPerpendicularPassed { get; set; }

        /// <summary>
        /// Bearing origin to destination
        /// </summary>
        public Decimal BearingOriginToDestination { get; set; }

        /// <summary>
        /// Bearing Type - Origin
        /// </summary>
        public BearingTypeEnum BearingOrignType { get; set; }

        /// <summary>
        /// Waypoint ID
        /// </summary>
        public String WaypointID { get; set; }

        /// <summary>
        /// Bearing present position to destination
        /// </summary>
        public Decimal BearingPresentPositionToDestination { get; set; }

        /// <summary>
        /// Bearing Type - Present Position
        /// </summary>
        public BearingTypeEnum BearingPresentPositionType { get; set; }

        /// <summary>
        /// Heading-to-steer to destination waypoint
        /// </summary>
        public Decimal HeadingToSteerToDestinationWaypoint { get; set; }

        /// <summary>
        /// Heading Type - To Destination Waypoint
        /// </summary>
        public HeadingTypeEnum HeadingToSteerType { get; set; }

        /// <summary>
        /// Mode indicator
        /// </summary>
        public ModeIndicatorEnum ModeIndicator { get; set; }

        public APBSentence()
        {
            SentenceIdentifier = "APB";
            IsReliableFixAvailable = false;
            IsDataValid = false;
            IsArrivalCircleEntered = false;
            IsPerpendicularPassed = false;
            DirectionToSteer = DirectionToSteerEnum.L;
            BearingOrignType = BearingTypeEnum.M;
            BearingPresentPositionType = BearingTypeEnum.M;
            HeadingToSteerType = HeadingTypeEnum.M;
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

            if (IsArrivalCircleEntered)
                stringBuilder.Append("A,");
            else
                stringBuilder.Append("V,");

            if (IsPerpendicularPassed)
                stringBuilder.Append("A,");
            else
                stringBuilder.Append("V,");

            stringBuilder.AppendFormat("{0},{1},", BearingOriginToDestination, BearingOrignType.ToString());

            stringBuilder.AppendFormat("{0},", WaypointID);

            stringBuilder.AppendFormat("{0},{1},", BearingPresentPositionToDestination, BearingPresentPositionType.ToString());

            stringBuilder.AppendFormat("{0},{1},", HeadingToSteerToDestinationWaypoint, HeadingToSteerType.ToString());

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

            // A Arrival Circle Status
            if (vs[6] == "A")
                IsArrivalCircleEntered = true;
            else
                IsArrivalCircleEntered = false;

            // A Perpendicular Passed Status
            if (vs[7] == "A")
                IsPerpendicularPassed = true;
            else
                IsPerpendicularPassed = false;

            // x.x Bearing origin to destination
            BearingOriginToDestination = Decimal.Parse(vs[8]);

            // a Bearing Type
            BearingOrignType = (BearingTypeEnum)Enum.Parse(typeof(BearingTypeEnum), vs[9]);

            // c--c Waypoint ID
            WaypointID = vs[10];

            // x.x Bearing Present position to destination
            BearingPresentPositionToDestination = Decimal.Parse(vs[11]);

            // a Bearing Type
            BearingPresentPositionType = (BearingTypeEnum)Enum.Parse(typeof(BearingTypeEnum), vs[12]);

            // x.x Heading-to-steer to destination waypoin
            HeadingToSteerToDestinationWaypoint = Decimal.Parse(vs[13]);

            // a Heading Type
            HeadingToSteerType = (HeadingTypeEnum)Enum.Parse(typeof(HeadingTypeEnum), vs[14]);

            // a Mode indicator
            if (vs.Length > 16)
                ModeIndicator = (ModeIndicatorEnum)Enum.Parse(typeof(ModeIndicatorEnum), vs[15]);
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

        public enum BearingTypeEnum
        {
            /// <summary>
            /// M - Magnetic
            /// </summary>
            M,
            /// <summary>
            /// T - True
            /// </summary>
            T,
        }

        public enum HeadingTypeEnum
        {
            /// <summary>
            /// M - Magnetic
            /// </summary>
            M,
            /// <summary>
            /// T - True
            /// </summary>
            T,
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
