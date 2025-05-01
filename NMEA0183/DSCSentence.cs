using System;
using System.Globalization;
using System.Text;

namespace TensionDev.Maritime.NMEA0183
{
    /// <summary>
    /// DSC - Digital Selective Calling Information
    /// </summary>
    public class DSCSentence : NMEASentence
    {
        /// <summary>
        /// Format Specifier <br />
        /// Use two least-significant digits of symbol codes in ITU-R M.493 Table 3
        /// </summary>
        public Byte FormatSpecifier { get; set; }

        /// <summary>
        /// Address <br />
        /// Maritime Mobile Service Identifier (MMSI) for th e station to be called or the MMSI of the calling station in a received call.
        /// </summary>
        public UInt64 Address { get; set; }

        /// <summary>
        /// Category <br />
        /// Use two least-significant digits of symbol codes in ITU-R M.493 Table 3
        /// </summary>
        public Byte Category { get; set; }

        /// <summary>
        /// Nature of Distress or First Telecommand <br />
        /// Use two least-significant digits of symbol codes in ITU-R M.493 Table 3
        /// Distress calls only
        /// </summary>
        public Nullable<Byte> NatureOfDistress { get; set; }

        /// <summary>
        /// Type of Communication or Second Telecommand <br />
        /// Use two least-significant digits of symbol codes in ITU-R M.493 Table 3
        /// Distress, Distress Acknowledgment, Distress Relay, and Distress Relay Acknowledgment calls only.
        /// </summary>
        public Nullable<Byte> TypeOfCommunication { get; set; }

        /// <summary>
        /// Position or Channel Frequency <br />
        /// Latitude/longitude, degrees and minutes, 10 digits, coded in accordance with ITU-R M.493 paragraph 8.1.2 <br />
        /// Frequency or channel, six or twelve digits, coded in accordance with ITU-R M.493 Table 13. 
        /// </summary>
        public UInt64 Position { get; set; }

        /// <summary>
        /// Time (UTC) of position, four digits, hhmm (hours and minutes) 
        /// </summary>
        public TimeSpan Time { get; set; }

        /// <summary>
        /// Telephone No. 
        /// </summary>
        public UInt64 TelephoneNumber { get; set; }

        /// <summary>
        /// For Distress Acknowledgement, Distress Relay and Distress Relay Acknowledgement calls only, null otherwise
        /// </summary>
        public Nullable<UInt64> MMSIofShipInDistress { get; set; }

        /// <summary>
        /// For Distress Acknowledgement, Distress Relay and Distress Relay Acknowledgement calls only, null otherwise <br />
        /// Nature of Distress <br />
        /// Use two least-significant digits of symbol codes in ITU-R M.493 Table 3
        /// Distress calls only
        /// </summary>
        public Nullable<Byte> NatureOfDistressAck { get; set; }

        /// <summary>
        /// Acknowledgement type: R = Acknowledge Request <br />
        /// B = Acknowledgement <br />
        /// S = Neither(end of sequence) <br />
        /// </summary>
        public AcknowledgementTypeEnum Acknowledgement { get; set; }

        /// <summary>
        /// Expansion indicator = "E", null otherwise. <br />
        /// When set to "E" this sentence is followed by the DSC Expansion sentence $--DSE, <br />
        /// without intervening sentences, <br />
        /// as the next transmitted or received sentence.
        /// </summary>
        public Nullable<ExpansionIndicatorEnum> ExpansionIndicator { get; set; }

        public DSCSentence()
        {
            SentenceIdentifier = "DSC";
        }

        public override String EncodeSentence()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("${0}{1},", TalkerIdentifier.ToString(), SentenceIdentifier);

            stringBuilder.AppendFormat("{0},", FormatSpecifier.ToString());

            stringBuilder.AppendFormat("{0:D10},", Address.ToString());

            stringBuilder.AppendFormat("{0},", Category.ToString());

            stringBuilder.AppendFormat("{0},", NatureOfDistress.ToString());

            stringBuilder.AppendFormat("{0},", TypeOfCommunication.ToString());

            stringBuilder.AppendFormat("{0:D10},", Position.ToString());

            stringBuilder.AppendFormat("{0:HHmm},", Time.ToString());

            stringBuilder.AppendFormat("{0:D10},", MMSIofShipInDistress.ToString());

            stringBuilder.AppendFormat("{0:D10},", NatureOfDistress.ToString());

            stringBuilder.AppendFormat("{0},", NatureOfDistressAck.ToString());

            stringBuilder.AppendFormat("{0},", ExpansionIndicator.ToString());

            Byte checksum = CalculateChecksum(stringBuilder.ToString());

            stringBuilder.AppendFormat("*{0}\r\n", checksum.ToString("X2"));

            return stringBuilder.ToString();
        }

        protected override void DecodeInternalSentence(String sentence)
        {
            DecodeTalker(sentence);

            String[] vs = sentence.Split(',', '*');

            // xx Format Specifier
            FormatSpecifier = Byte.Parse(vs[1]);

            // xxxxxxxxxx Address
            Address = UInt64.Parse(vs[2]);

            // xx Category
            Category = Byte.Parse(vs[3]);

            // xx Nature of Distress or First Telecommand
            NatureOfDistress = Byte.Parse(vs[4]);

            // xx Type of Communication or Second Telecommand
            TypeOfCommunication = Byte.Parse(vs[5]);

            // x.x Position or Channel/ Frequency
            Position = UInt64.Parse(vs[6]);

            // x.x Time or Tel. No.
            bool parse = TimeSpan.TryParseExact(vs[7], "HHmm", CultureInfo.InvariantCulture, out TimeSpan time);
            if (parse)
            {
                Time = time;
            }
            else
            {
                TelephoneNumber = UInt64.Parse(vs[7]);
            }

            // xxxxxxxxxx MMSI of ship in distress
            parse = UInt64.TryParse(vs[8], out UInt64 mmsiDistress);
            if (parse)
            {
                MMSIofShipInDistress = mmsiDistress;
            }
            else
            {
                MMSIofShipInDistress = null;
            }

            // xx Nature of Distress
            parse = Byte.TryParse(vs[9], out Byte nature);
            if (parse)
            {
                NatureOfDistress = nature;
            }
            else
            {
                NatureOfDistress = null;
            }

            // a Acknowledgement
            Acknowledgement = (AcknowledgementTypeEnum)Enum.Parse(typeof(AcknowledgementTypeEnum), vs[10]);

            // a Expansion indicator
            parse = Enum.TryParse<ExpansionIndicatorEnum>(vs[11], out ExpansionIndicatorEnum expansion);
            if (parse)
            {
                ExpansionIndicator = expansion;
            }
            else
            {
                ExpansionIndicator = null;
            }
        }

        public enum AcknowledgementTypeEnum
        {
            /// <summary>
            /// Acknowledge Request
            /// </summary>
            R,
            /// <summary>
            /// Acknowledgement
            /// </summary>
            B,
            /// <summary>
            /// Neither (end of sequence)
            /// </summary>
            S,
        }

        public enum ExpansionIndicatorEnum
        {
            /// <summary>
            /// ExpansionIndicator
            /// </summary>
            E,
        }
    }
}
