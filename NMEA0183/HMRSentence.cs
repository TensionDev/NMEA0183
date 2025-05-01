using System;
using System.Text;
using static TensionDev.Maritime.NMEA0183.APBSentence;

namespace TensionDev.Maritime.NMEA0183
{
    /// <summary>
    /// HMR -  Heading Monitor Receive
    /// </summary>
    public class HMRSentence : NMEASentence
    {
        /// <summary>
        /// Heading Sensor 1 ID
        /// </summary>
        public String HeadingSensorOneID { get; set; }

        /// <summary>
        /// Heading Sensor 2 ID
        /// </summary>
        public String HeadingSensorTwoID { get; set; }

        /// <summary>
        /// Difference limit setting, degrees 
        /// </summary>
        public Decimal DifferenceLimitSettingDegrees { get; set; }

        /// <summary>
        /// Actual heading sensor difference, degrees
        /// </summary>
        public Decimal ActualHeadingSensorDifferenceDegrees { get; set; }

        /// <summary>
        /// <para>Warning flag.</para>
        /// <para>True - difference within set limit.</para>
        /// <para>False - difference exceeds set limit.</para>
        /// </summary>
        public Boolean WarningFlag { get; set; }

        /// <summary>
        /// Heading reading, Sensor 1, degrees
        /// </summary>
        public Decimal HeadingReadingSensorOneDegrees { get; set; }

        /// <summary>
        /// Sensor 1 type, T/M
        /// </summary>
        public SensorTypeEnum SensorOneType { get; set; }

        /// <summary>
        /// Deviation, Sensor 1, degrees E/W,
        /// East - Positive,
        /// West - Negative
        /// </summary>
        public Nullable<Decimal> DeviationSensorOneDegrees { get; set; }

        /// <summary>
        /// Heading reading, Sensor 2, degrees
        /// </summary>
        public Decimal HeadingReadingSensorTwoDegrees { get; set; }

        /// <summary>
        /// Sensor 2 type, T/M
        /// </summary>
        public SensorTypeEnum SensorTwoType { get; set; }

        /// <summary>
        /// Deviation, Sensor 2, degrees E/W,
        /// East - Positive,
        /// West - Negative
        /// </summary>
        public Nullable<Decimal> DeviationSensorTwoDegrees { get; set; }

        /// <summary>
        /// Variation, degrees E/W,
        /// East - Positive,
        /// West - Negative
        /// </summary>
        public Nullable<Decimal> VariationDegrees { get; set; }

        public HMRSentence()
        {
            SentenceIdentifier = "HMR";
        }

        public override String EncodeSentence()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("${0}{1},", TalkerIdentifier.ToString(), SentenceIdentifier);

            stringBuilder.AppendFormat("{0},", HeadingSensorOneID);

            stringBuilder.AppendFormat("{0},", HeadingSensorTwoID);

            stringBuilder.AppendFormat("{0},", DifferenceLimitSettingDegrees);

            stringBuilder.AppendFormat("{0},", ActualHeadingSensorDifferenceDegrees);

            stringBuilder.AppendFormat("{0},", WarningFlag ? "A" : "V");

            stringBuilder.AppendFormat("{0},A,", HeadingReadingSensorOneDegrees);

            stringBuilder.AppendFormat("{0},", SensorOneType.ToString());

            if (DeviationSensorOneDegrees.HasValue)
            {
                stringBuilder.AppendFormat("{0},", Math.Abs(DeviationSensorOneDegrees.Value));
                stringBuilder.AppendFormat("{0},", DeviationSensorOneDegrees < 0 ? "W" : "E");
            }
            else
            {
                stringBuilder.Append(",,");
            }

            stringBuilder.AppendFormat("{0},A,", HeadingReadingSensorTwoDegrees);

            stringBuilder.AppendFormat("{0},", SensorTwoType.ToString());

            if (DeviationSensorTwoDegrees.HasValue)
            {
                stringBuilder.AppendFormat("{0},", Math.Abs(DeviationSensorTwoDegrees.Value));
                stringBuilder.AppendFormat("{0},", DeviationSensorTwoDegrees < 0 ? "W" : "E");
            }
            else
            {
                stringBuilder.Append(",,");
            }

            if (VariationDegrees.HasValue)
            {
                stringBuilder.AppendFormat("{0},", Math.Abs(VariationDegrees.Value));
                stringBuilder.AppendFormat("{0},", VariationDegrees < 0 ? "W" : "E");
            }
            else
            {
                stringBuilder.Append(",");
            }

            Byte checksum = CalculateChecksum(stringBuilder.ToString());

            stringBuilder.AppendFormat("*{0}\r\n", checksum.ToString("X2"));

            return stringBuilder.ToString();
        }

        protected override void DecodeInternalSentence(String sentence)
        {
            DecodeTalker(sentence);

            String[] vs = sentence.Split(',', '*');

            // c--c Heading Sensor 1 ID
            HeadingSensorOneID = vs[1];

            // c--c Heading Sensor 2 ID
            HeadingSensorTwoID = vs[2];

            // x.x Difference limit setting, degrees
            DifferenceLimitSettingDegrees = Decimal.Parse(vs[3]);

            // x.x Actual heading sensor difference, degrees 
            ActualHeadingSensorDifferenceDegrees = Decimal.Parse(vs[4]);

            // A Warning flag
            WarningFlag = (vs[5] == "A");

            // x.x Heading reading, Sensor 1, degrees
            HeadingReadingSensorOneDegrees = Decimal.Parse(vs[6]);

            // a Sensor 1 type, T/M
            SensorOneType = (SensorTypeEnum)Enum.Parse(typeof(SensorTypeEnum), vs[8]);

            // x.x Deviation, Sensor 1, degrees
            if (String.IsNullOrWhiteSpace(vs[9]))
            {
                DeviationSensorOneDegrees = null;
            }
            else
            {
                DeviationSensorOneDegrees = Decimal.Parse(vs[9]);

                if (vs[10] == "W")
                    DeviationSensorOneDegrees *= Decimal.MinusOne;
            }

            // x.x Heading reading, Sensor 2, degrees
            HeadingReadingSensorTwoDegrees = Decimal.Parse(vs[11]);

            // a Sensor 2 type, T/M
            SensorTwoType = (SensorTypeEnum)Enum.Parse(typeof(SensorTypeEnum), vs[13]);

            // x.x Deviation, Sensor 2, degrees
            if (String.IsNullOrWhiteSpace(vs[14]))
            {
                DeviationSensorTwoDegrees = null;
            }
            else
            {
                DeviationSensorTwoDegrees = Decimal.Parse(vs[14]);

                if (vs[15] == "W")
                    DeviationSensorTwoDegrees *= Decimal.MinusOne;
            }

            // x.x Variation, degrees E/W
            if (String.IsNullOrWhiteSpace(vs[16]))
            {
                VariationDegrees = null;
            }
            else
            {
                VariationDegrees = Decimal.Parse(vs[16]);

                if (vs[17] == "W")
                    VariationDegrees *= Decimal.MinusOne;
            }
        }

        public enum SensorTypeEnum
        {
            /// <summary>
            /// T - True
            /// </summary>
            T,
            /// <summary>
            /// M - Magnetic
            /// </summary>
            M,
        }
    }
}
