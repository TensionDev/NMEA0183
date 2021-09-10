using System;
using System.Text;

namespace TensionDev.Maritime.NMEA0183
{
    /// <summary>
    /// GGA - Global Positioning System Fix Data. Time, Position and fix related data for a GPS receiver
    /// </summary>
    public class GGASentence : NMEASentence
    {
        private Byte numberofSatellitesInUse;
        private UInt16? differentialReferenceStationID;

        /// <summary>
        /// UTC Time At Position
        /// </summary>
        public DateTime UTCTimeAtPosition { get; set; }

        /// <summary>
        /// Latitude in Decimal Degrees, North - Positive, South - Negative
        /// </summary>
        public Decimal LatitudeDecimalDegrees { get; set; }

        /// <summary>
        /// Longitude in Decimal Degrees, East - Positive, West - Negative
        /// </summary>
        public Decimal LongitudeDecimalDegrees { get; set; }

        /// <summary>
        /// GPS Quality Indicator
        /// </summary>
        public GPSQualityIndicatorEnum GPSQualityIndicator { get; set; }

        /// <summary>
        /// Number of satellites in use, 00-12, may be different from the number in view
        /// </summary>
        public Byte NumberofSatellitesInUse { get => numberofSatellitesInUse; set => numberofSatellitesInUse = Math.Min(value, (Byte)12); }

        /// <summary>
        /// Horizontal Dilution of precision in Metres
        /// </summary>
        public Decimal HorizontalDilutionOfPrecision { get; set; }

        /// <summary>
        /// Antenna Altitude Above Mean Sea Level in Metres
        /// </summary>
        public Decimal AntennaAltitudeAboveMeanSeaLevel { get; set; }

        /// <summary>
        /// The difference between the WGS-84 earth ellipsoid surface and mean-sea-level (geoid) surface in Metres
        /// </summary>
        public Decimal GeoidalSeparation { get; set; }

        /// <summary>
        /// Age of differential GPS data, time in seconds since last SC104 type 1 or 9 update, null field when DGPS is not used
        /// </summary>
        public Nullable<Decimal> AgeOfDifferentialGPSData { get; set; }

        /// <summary>
        /// Differential reference station ID, 0000-1023
        /// </summary>
        public Nullable<UInt16> DifferentialReferenceStationID { get => differentialReferenceStationID; set => differentialReferenceStationID = value; }

        public GGASentence()
        {
            SentenceIdentifier = "GGA";
            UTCTimeAtPosition = DateTime.UtcNow;
            GPSQualityIndicator = GPSQualityIndicatorEnum.FixNotAvailableOrInvalid;
        }

        /// <summary>
        /// Conversion from Decimal Degrees to Degrees, Minute
        /// </summary>
        /// <param name="decimalDegrees">Decimal Degress</param>
        /// <param name="degrees">Degrees output</param>
        /// <param name="decimalMinutes">Decimal Minutes output</param>
        protected static void DecimalDegreesToDegreesMinute(Decimal decimalDegrees, out Int32 degrees, out Decimal decimalMinutes)
        {
            Decimal absDecimalDegrees = Math.Abs(decimalDegrees);

            degrees = Convert.ToInt32(Math.Floor(absDecimalDegrees));

            decimalMinutes = absDecimalDegrees - degrees;
            decimalMinutes *= 60.0M;

            if (decimalDegrees < 0)
                degrees *= -1;
        }

        public override String EncodeSentence()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("${0}{1},", TalkerIdentifier.ToString(), SentenceIdentifier);

            stringBuilder.AppendFormat("{0},", UTCTimeAtPosition.ToString("HHmmss.FF"));

            DecimalDegreesToDegreesMinute(LatitudeDecimalDegrees, out Int32 LatDegrees, out Decimal LatMinutes);
            stringBuilder.AppendFormat("{0}{1},", Math.Abs(LatDegrees).ToString("D2"), LatMinutes.ToString("00.#######"));
            if (LatDegrees < 0)
                stringBuilder.Append("S,");
            else
                stringBuilder.Append("N,");

            DecimalDegreesToDegreesMinute(LongitudeDecimalDegrees, out Int32 LonDegrees, out Decimal LonMinutes);
            stringBuilder.AppendFormat("{0}{1},", Math.Abs(LonDegrees).ToString("D3"), LonMinutes.ToString("00.#######"));
            if (LonDegrees < 0)
                stringBuilder.Append("W,");
            else
                stringBuilder.Append("E,");

            stringBuilder.AppendFormat("{0},", (Int32)GPSQualityIndicator);

            stringBuilder.AppendFormat("{0},", NumberofSatellitesInUse.ToString("D2"));

            stringBuilder.AppendFormat("{0},", HorizontalDilutionOfPrecision);

            stringBuilder.AppendFormat("{0},M,", AntennaAltitudeAboveMeanSeaLevel);

            stringBuilder.AppendFormat("{0},M,", GeoidalSeparation);

            if (AgeOfDifferentialGPSData.HasValue)
            {
                stringBuilder.AppendFormat("{0},", AgeOfDifferentialGPSData);
            }
            else
            {
                stringBuilder.Append(",");
            }

            if (DifferentialReferenceStationID.HasValue)
            {
                stringBuilder.AppendFormat("{0}", DifferentialReferenceStationID.Value.ToString("D4"));
            }
            else
            {
                stringBuilder.Append("");
            }

            Byte checksum = CalculateChecksum(stringBuilder.ToString());

            stringBuilder.AppendFormat("*{0}\r\n", checksum.ToString("X2"));

            return stringBuilder.ToString();
        }

        protected override void DecodeInternalSentence(String sentence)
        {
            DecodeTalker(sentence);

            String[] vs = sentence.Split(new char[] { ',', '*' });

            // UTC Time
            String time = vs[1];
            time = time.Insert(4, ":");
            time = time.Insert(2, ":");
            TimeSpan timeSpan = TimeSpan.Parse(time);
            DateTime today = DateTime.UtcNow;
            UTCTimeAtPosition = new DateTime(today.Year, today.Month, today.Day, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds, DateTimeKind.Utc);

            // XXyy.yy Latitude
            String degreeDec = vs[2].Substring(0, 2);
            String degreeMin = vs[2].Substring(2);
            Decimal degreeMinute = Decimal.Parse(degreeMin);
            LatitudeDecimalDegrees = Decimal.Parse(degreeDec) + (degreeMinute / 60.0M);

            // North or South
            if (vs[3] == "S")
                LatitudeDecimalDegrees *= Decimal.MinusOne;

            // XXXyy.yy Latitude
            degreeDec = vs[4].Substring(0, 3);
            degreeMin = vs[4].Substring(3);
            degreeMinute = Decimal.Parse(degreeMin);
            LongitudeDecimalDegrees = Decimal.Parse(degreeDec) + (degreeMinute / 60.0M);

            // North or South
            if (vs[5] == "W")
                LongitudeDecimalDegrees *= Decimal.MinusOne;

            // GPS Quality Indicator
            GPSQualityIndicator = (GPSQualityIndicatorEnum)Int32.Parse(vs[6]);

            // Number of Satellites in use
            NumberofSatellitesInUse = Byte.Parse(vs[7]);

            // Horizontal dilution of precision
            HorizontalDilutionOfPrecision = Decimal.Parse(vs[8]);

            // Antenna altitude above mean sea level
            AntennaAltitudeAboveMeanSeaLevel = Decimal.Parse(vs[9]);

            // Geoidal separation
            GeoidalSeparation = Decimal.Parse(vs[11]);

            if (vs[13].Length > 0)
                AgeOfDifferentialGPSData = Decimal.Parse(vs[13]);
            else
                AgeOfDifferentialGPSData = null;

            if (vs[14].Length > 0)
                DifferentialReferenceStationID = UInt16.Parse(vs[14]);
            else
                DifferentialReferenceStationID = null;
        }

        public enum GPSQualityIndicatorEnum
        {
            FixNotAvailableOrInvalid,
            GPSStandardPositioningService,
            DifferentialGPSStandardPositioningService,
            GPSPrecisePositioningService,
            RealTimeKinematic,
            FloatRealTimeKinematic,
            EstimatedDeadReckoningMode,
            ManualInputMode,
            SimulatorMode,
        }
    }
}
