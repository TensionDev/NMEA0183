using System;
using System.Text;

namespace TensionDev.Maritime.NMEA0183
{
    /// <summary>
    /// GST - GNSS Pseudorange Error Statistics
    /// </summary>
    public class GSTSentence : NMEASentence
    {
        /// <summary>
        /// UTC time of the GGA or GNS fix associated with this sentence.
        /// </summary>
        public DateTime UTCTimeOfTheFix { get; set; }

        /// <summary>
        /// RMS value of the standard deviation of the range inputs to the navigation process.
        /// </summary>
        public Decimal RootMeanSquaredValueOfTheStandardDeviation { get; set; }

        /// <summary>
        /// Standard deviation of semi-major axis of error ellipse (meters)
        /// </summary>
        public Decimal StandardDeviationSemiMajorAxisOfErrorEllipseMetres { get; set; }

        /// <summary>
        /// Standard deviation of semi-minor axis of error ellipse (meters) 
        /// </summary>
        public Decimal StandardDeviationSemiMinorAxisOfErrorEllipseMetres { get; set; }

        /// <summary>
        /// Orientation of semi-major axis of error ellipse (degrees from true north) 
        /// </summary>
        public Decimal OrientationOfSemiMajorAxisOfErrorEllipseDegrees { get; set; }

        /// <summary>
        /// Standard deviation of latitude error (meters)
        /// </summary>
        public Decimal StandardDeviationLatitudeMetres { get; set; }

        /// <summary>
        /// Standard deviation of longitude error (meters)
        /// </summary>
        public Decimal StandardDeviationLongitudeMetres { get; set; }

        /// <summary>
        /// Standard deviation of altitude error (meters)
        /// </summary>
        public Decimal StandardDeviationAltitudeMetres { get; set; }

        public GSTSentence()
        {
            SentenceIdentifier = "GST";
            UTCTimeOfTheFix = DateTime.UtcNow;
        }

        public override String EncodeSentence()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("${0}{1},", TalkerIdentifier.ToString(), SentenceIdentifier);

            stringBuilder.AppendFormat("{0},", UTCTimeOfTheFix.ToString("HHmmss.FF"));

            stringBuilder.AppendFormat("{0},", RootMeanSquaredValueOfTheStandardDeviation);

            stringBuilder.AppendFormat("{0},", StandardDeviationSemiMajorAxisOfErrorEllipseMetres);

            stringBuilder.AppendFormat("{0},", StandardDeviationSemiMinorAxisOfErrorEllipseMetres);

            stringBuilder.AppendFormat("{0},", OrientationOfSemiMajorAxisOfErrorEllipseDegrees);

            stringBuilder.AppendFormat("{0},", StandardDeviationLatitudeMetres);

            stringBuilder.AppendFormat("{0},", StandardDeviationLongitudeMetres);

            stringBuilder.AppendFormat("{0}", StandardDeviationAltitudeMetres);

            Byte checksum = CalculateChecksum(stringBuilder.ToString());

            stringBuilder.AppendFormat("*{0}\r\n", checksum.ToString("X2"));

            return stringBuilder.ToString();
        }

        protected override void DecodeInternalSentence(String sentence)
        {
            DecodeTalker(sentence);

            String[] vs = sentence.Split(',', '*');

            // hhmmss.ss UTC Time
            String time = vs[1];
            time = time.Insert(4, ":");
            time = time.Insert(2, ":");
            TimeSpan timeSpan = TimeSpan.Parse(time);
            DateTime today = DateTime.UtcNow;
            UTCTimeOfTheFix = new DateTime(today.Year, today.Month, today.Day, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds, DateTimeKind.Utc);

            // x.x RMS value of the standard deviation of the range inputs to the navigation process
            RootMeanSquaredValueOfTheStandardDeviation = Decimal.Parse(vs[2]);

            // x.x Standard deviation of semi-major axis of error ellipse
            StandardDeviationSemiMajorAxisOfErrorEllipseMetres = Decimal.Parse(vs[3]);

            // x.x Standard deviation of semi-minor axis of error ellipse
            StandardDeviationSemiMinorAxisOfErrorEllipseMetres = Decimal.Parse(vs[4]);

            // x.x Orientation of semi-major axis of error ellipse
            OrientationOfSemiMajorAxisOfErrorEllipseDegrees = Decimal.Parse(vs[5]);

            // x.x Standard deviation of latitude error
            StandardDeviationLatitudeMetres = Decimal.Parse(vs[6]);

            // x.x Standard deviation of longitude error
            StandardDeviationLongitudeMetres = Decimal.Parse(vs[7]);

            // x.x Standard deviation of altitude error
            StandardDeviationAltitudeMetres = Decimal.Parse(vs[8]);
        }
    }
}
