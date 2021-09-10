using System;
using System.Text;

namespace TensionDev.NMEA0183
{
    /// <summary>
    /// GLL - Geographic Position – Latitude/Longitude
    /// </summary>
    public class GLLSentence : NMEASentence
    {
        /// <summary>
        /// Latitude in Decimal Degrees, North - Positive, South - Negative
        /// </summary>
        public Decimal LatitudeDecimalDegrees { get; set; }

        /// <summary>
        /// Longitude in Decimal Degrees, East - Positive, West - Negative
        /// </summary>
        public Decimal LongitudeDecimalDegrees { get; set; }

        /// <summary>
        /// UTC Time At Position
        /// </summary>
        public DateTime UTCTimeAtPosition { get; set; }

        /// <summary>
        /// Is the data valid?
        /// </summary>
        public Boolean IsDataValid { get; set; }

        public GLLSentence()
        {
            SentenceIdentifier = "GLL";
            UTCTimeAtPosition = DateTime.UtcNow;
            IsDataValid = false;
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

            stringBuilder.AppendFormat("{0},", UTCTimeAtPosition.ToString("HHmmss.FF"));

            if (IsDataValid)
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

            // XXyy.yy Latitude
            String degreeDec = vs[1].Substring(0, 2);
            String degreeMin = vs[1].Substring(2);
            Decimal degreeMinute = Decimal.Parse(degreeMin);
            LatitudeDecimalDegrees = Decimal.Parse(degreeDec) + (degreeMinute / 60.0M);

            // North or South
            if (vs[2] == "S")
                LatitudeDecimalDegrees *= Decimal.MinusOne;

            // XXXyy.yy Latitude
            degreeDec = vs[3].Substring(0, 3);
            degreeMin = vs[3].Substring(3);
            degreeMinute = Decimal.Parse(degreeMin);
            LongitudeDecimalDegrees = Decimal.Parse(degreeDec) + (degreeMinute / 60.0M);

            // North or South
            if (vs[4] == "W")
                LongitudeDecimalDegrees *= Decimal.MinusOne;

            // UTC Time
            String time = vs[5];
            time = time.Insert(4, ":");
            time = time.Insert(2, ":");
            TimeSpan timeSpan = TimeSpan.Parse(time);
            DateTime today = DateTime.UtcNow;
            UTCTimeAtPosition = new DateTime(today.Year, today.Month, today.Day, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds, DateTimeKind.Utc);

            if (vs[6] == "A")
                IsDataValid = true;
            else
                IsDataValid = false;
        }
    }
}
