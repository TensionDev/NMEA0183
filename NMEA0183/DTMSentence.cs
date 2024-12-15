using System;
using System.Text;

namespace TensionDev.Maritime.NMEA0183
{
    /// <summary>
    /// DTM - Datum Reference 
    /// </summary>
    public class DTMSentence : NMEASentence
    {
        /// <summary>
        /// Three character alpha code for local datum.
        /// If not one of the listed earth-centered datums, or 999 for user defined datum, use IHO datum code from International Hydrographic Organization Publication S60 Appendices B and C.
        /// Null field if unknown.
        /// </summary>
        public String LocalDatumCode { get; set; }

        /// <summary>
        /// Known Earth Centred Datum for the local datum.
        /// </summary>
        public EarthCentredDatumEnum LocalDatumEarthCentred
        {
            get
            {
                bool parsed = Enum.TryParse<EarthCentredDatumEnum>(LocalDatumCode, true, out EarthCentredDatumEnum result);
                if (!parsed)
                    return EarthCentredDatumEnum.UserDefined;
                return result;
            }
            set
            {
                switch (value)
                {
                    case EarthCentredDatumEnum.WGS84:
                        LocalDatumCode = "W84";
                        break;

                    case EarthCentredDatumEnum.WGS72:
                        LocalDatumCode = "W72";
                        break;

                    case EarthCentredDatumEnum.SGS85:
                        LocalDatumCode = "S85";
                        break;

                    case EarthCentredDatumEnum.PE90:
                        LocalDatumCode = "P90";
                        break;

                    case EarthCentredDatumEnum.UserDefined:
                        LocalDatumCode = "999";
                        break;
                }
            }
        }

        /// <summary>
        /// One character subdivision datum code when available or user defined reference character for user defined datums, null field otherwise.
        /// Subdivision character from IHO Publication S-60 Appendices B and C. 
        /// </summary>
        public String LocalDatumSubdivisionCode { get; set; }

        /// <summary>
        /// <para>Latitude and longitude offsets are positive numbers, the altitude offset may be negative.</para>
        /// <para>Offsets change with position; position in the local datum is offset from the position in the reference datum in the directions indicated:</para>
        /// <para>P(local datum) = P(ref datum) + offset</para>
        /// </summary>
        public Decimal LatitudeOffsetMinutes { get; set; }

        /// <summary>
        /// Latitude Offset Direction
        /// </summary>
        public LatitudeOffsetDirectionEnum LatitudeOffsetDirection { get; set; }

        /// <summary>
        /// <para>Latitude and longitude offsets are positive numbers, the altitude offset may be negative.</para>
        /// <para>Offsets change with position; position in the local datum is offset from the position in the reference datum in the directions indicated:</para>
        /// <para>P(local datum) = P(ref datum) + offset</para>
        /// </summary>
        public Decimal LongitudeOffsetMinutes { get; set; }

        /// <summary>
        /// Longitude Offset Direction
        /// </summary>
        public LongitudeOffsetDirectionEnum LongitudeOffsetDirection { get; set; }

        /// <summary>
        /// <para>Latitude and longitude offsets are positive numbers, the altitude offset may be negative.</para>
        /// <para>Offsets change with position; position in the local datum is offset from the position in the reference datum in the directions indicated:</para>
        /// <para>P(local datum) = P(ref datum) + offset</para>
        /// </summary>
        public Decimal AltitudeOffsetMetres { get; set; }

        /// <summary>
        /// Reference datum code
        /// </summary>
        public ReferenceDatumCodeEnum ReferenceDatumCode { get; set; }

        public DTMSentence()
        {
            SentenceIdentifier = "DTM";
            LocalDatumCode = String.Empty;
            LocalDatumSubdivisionCode = String.Empty;
            LatitudeOffsetDirection = LatitudeOffsetDirectionEnum.N;
            LongitudeOffsetDirection = LongitudeOffsetDirectionEnum.E;
            ReferenceDatumCode = ReferenceDatumCodeEnum.W84;
        }

        public override String EncodeSentence()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("${0}{1},", TalkerIdentifier.ToString(), SentenceIdentifier);

            stringBuilder.AppendFormat("{0},", LocalDatumCode);

            stringBuilder.AppendFormat("{0},", LocalDatumSubdivisionCode);

            stringBuilder.AppendFormat("{0},", LatitudeOffsetMinutes.ToString());

            stringBuilder.AppendFormat("{0},", LatitudeOffsetDirection.ToString());

            stringBuilder.AppendFormat("{0},", LongitudeOffsetMinutes.ToString());

            stringBuilder.AppendFormat("{0},", LongitudeOffsetDirection.ToString());

            stringBuilder.AppendFormat("{0},", AltitudeOffsetMetres.ToString());

            stringBuilder.AppendFormat("{0}", ReferenceDatumCode.ToString());

            Byte checksum = CalculateChecksum(stringBuilder.ToString());

            stringBuilder.AppendFormat("*{0}\r\n", checksum.ToString("X2"));

            return stringBuilder.ToString();
        }

        protected override void DecodeInternalSentence(String sentence)
        {
            DecodeTalker(sentence);

            String[] vs = sentence.Split(',', '*');

            // ccc Local Datum Code
            LocalDatumCode = vs[1];

            // a Local Datum Subdivision Code
            LocalDatumSubdivisionCode = vs[2];

            // x.x Latitude Offset Minutes
            LatitudeOffsetMinutes = Decimal.Parse(vs[3]);

            // a North / South
            LatitudeOffsetDirection = (LatitudeOffsetDirectionEnum)Enum.Parse(typeof(LatitudeOffsetDirectionEnum), vs[4]);

            // x.x Longitude Offset Minutes
            LongitudeOffsetMinutes = Decimal.Parse(vs[5]);

            // a East / West
            LongitudeOffsetDirection = (LongitudeOffsetDirectionEnum)Enum.Parse(typeof(LongitudeOffsetDirectionEnum), vs[6]);

            // x.x Altitude Offset Metres
            AltitudeOffsetMetres = Decimal.Parse(vs[7]);

            // ccc Reference Datum
            ReferenceDatumCode = (ReferenceDatumCodeEnum)Enum.Parse(typeof(ReferenceDatumCodeEnum), vs[8]);
        }

        public enum EarthCentredDatumEnum
        {
            /// <summary>
            /// WGS84
            /// </summary>
            WGS84,
            /// <summary>
            /// WGS72
            /// </summary>
            WGS72,
            /// <summary>
            /// SGS85
            /// </summary>
            SGS85,
            /// <summary>
            /// PE90
            /// </summary>
            PE90,
            /// <summary>
            /// User defined
            /// </summary>
            UserDefined,
        }

        public enum LatitudeOffsetDirectionEnum
        {
            /// <summary>
            /// North
            /// </summary>
            N,
            /// <summary>
            ///  South
            /// </summary>
            S,
        }

        public enum LongitudeOffsetDirectionEnum
        {
            /// <summary>
            /// East
            /// </summary>
            E,
            /// <summary>
            ///  West
            /// </summary>
            W,
        }

        public enum ReferenceDatumCodeEnum
        {
            /// <summary>
            /// WGS84
            /// </summary>
            W84,
            /// <summary>
            /// WGS72
            /// </summary>
            W72,
            /// <summary>
            /// SGS85
            /// </summary>
            S85,
            /// <summary>
            /// PE90
            /// </summary>
            P90,
        }
    }
}
