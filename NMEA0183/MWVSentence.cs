using System;
using System.Collections.Generic;
using System.Text;

namespace TensionDev.Maritime.NMEA0183
{
    /// <summary>
    /// MWV - Wind Speed and Angle
    /// </summary>
    public class MWVSentence : NMEASentence
    {
        /// <summary>
        /// Wind angle, 0 to 359 degrees 
        /// </summary>
        public Decimal WindAngleDegrees { get; set; }

        /// <summary>
        /// Reference, Relative or Theoretical
        /// </summary>
        public WindReferenceEnum WindReference { get; set; }

        /// <summary>
        /// Wind speed
        /// </summary>
        public Decimal WindSpeed { get; set; }

        /// <summary>
        /// Wind speed units, K/M/N
        /// </summary>
        public WindSpeedUnitsEnum WindSpeedUnits { get; set; }

        /// <summary>
        /// Is the data valid?
        /// </summary>
        public Boolean IsDataValid { get; set; }

        public MWVSentence()
        {
            SentenceIdentifier = "MWV";
            WindReference = WindReferenceEnum.R;
            WindSpeedUnits = WindSpeedUnitsEnum.N;
            IsDataValid = false;
        }

        public override String EncodeSentence()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("${0}{1},", TalkerIdentifier.ToString(), SentenceIdentifier);

            stringBuilder.AppendFormat("{0},", WindAngleDegrees);

            stringBuilder.AppendFormat("{0},", WindReference.ToString());

            stringBuilder.AppendFormat("{0},", WindSpeed);

            stringBuilder.AppendFormat("{0},", WindSpeedUnits.ToString());

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

            // x.x Wind angle
            WindAngleDegrees = Decimal.Parse(vs[1]);

            // a Wind reference
            WindReference = (WindReferenceEnum)Enum.Parse(typeof(WindReferenceEnum), vs[2]);

            // x.x Wind speed
            WindSpeed = Decimal.Parse(vs[3]);

            // a Wind speed units
            WindSpeedUnits = (WindSpeedUnitsEnum)Enum.Parse(typeof(WindSpeedUnitsEnum), vs[4]);

            // A Status
            if (vs[5] == "A")
                IsDataValid = true;
            else
                IsDataValid = false;
        }

        public enum WindReferenceEnum
        {
            /// <summary>
            /// Relative
            /// </summary>
            R,
            /// <summary>
            /// Theoretical
            /// </summary>
            T,
        }

        public enum WindSpeedUnitsEnum
        {
            /// <summary>
            /// km/h - Kilometres per Hour
            /// </summary>
            K,
            /// <summary>
            /// m/s - Metres per Second
            /// </summary>
            M,
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
