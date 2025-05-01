using System;

namespace TensionDev.Maritime.NMEA0183
{
    public abstract class NMEASentence
    {
        /// <summary>
        /// Talker Identifier
        /// </summary>
        public TalkerIdentifier TalkerIdentifier { get; set; }

        /// <summary>
        /// Sentence Identifier
        /// </summary>
        protected String SentenceIdentifier { get; set; }

        protected NMEASentence()
        {
        }

        /// <summary>
        /// Decodes the NMEA 0183 sentence and returns the appropriate NMEA 0183 object.
        /// </summary>
        /// <param name="sentence">The NMEA 0183 sentence</param>
        /// <returns>The NMEA 0183 object</returns>
        public static NMEASentence DecodeSentence(String sentence)
        {
            String sentenceIdentifier = sentence.Substring(3, 3);

            NMEASentence nmeaSentence;
            switch (sentenceIdentifier)
            {
                case "AAM":
                    nmeaSentence = new AAMSentence();
                    break;

                case "APB":
                    nmeaSentence = new APBSentence();
                    break;

                case "DPT":
                    nmeaSentence = new DPTSentence();
                    break;

                case "DSC":
                    nmeaSentence = new DSCSentence();
                    break;

                case "DTM":
                    nmeaSentence = new DTMSentence();
                    break;

                case "FSI":
                    nmeaSentence = new FSISentence();
                    break;

                case "GGA":
                    nmeaSentence = new GGASentence();
                    break;

                case "GLL":
                    nmeaSentence = new GLLSentence();
                    break;

                case "GST":
                    nmeaSentence = new GSTSentence();
                    break;

                case "HDG":
                    nmeaSentence = new HDGSentence();
                    break;

                case "HDT":
                    nmeaSentence = new HDTSentence();
                    break;

                case "HMR":
                    nmeaSentence = new HMRSentence();
                    break;

                case "HMS":
                    nmeaSentence = new HMSSentence();
                    break;

                case "MWV":
                    nmeaSentence = new MWVSentence();
                    break;

                case "OSD":
                    nmeaSentence = new OSDSentence();
                    break;

                case "ROT":
                    nmeaSentence = new ROTSentence();
                    break;

                case "RPM":
                    nmeaSentence = new RPMSentence();
                    break;

                case "RSA":
                    nmeaSentence = new RSASentence();
                    break;

                case "THS":
                    nmeaSentence = new THSSentence();
                    break;

                case "TTM":
                    nmeaSentence = new TTMSentence();
                    break;

                case "TXT":
                    nmeaSentence = new TXTSentence();
                    break;

                case "VBW":
                    nmeaSentence = new VBWSentence();
                    break;

                case "VHW":
                    nmeaSentence = new VHWSentence();
                    break;

                case "VTG":
                    nmeaSentence = new VTGSentence();
                    break;

                case "XTE":
                    nmeaSentence = new XTESentence();
                    break;

                case "ZDA":
                    nmeaSentence = new ZDASentence();
                    break;

                default:
                    throw new NotImplementedException("Sentence Identifier not recognised.");
            }

            nmeaSentence.DecodeTalker(sentence);
            nmeaSentence.DecodeInternalSentence(sentence);

            return nmeaSentence;
        }

        /// <summary>
        /// Decodes the Talker Identifier
        /// </summary>
        /// <param name="sentence">The NMEA 0183 sentence</param>
        protected void DecodeTalker(String sentence)
        {
            String identifier = sentence.Substring(1, 2);

            if (identifier.StartsWith("P"))
            {
                identifier = identifier.Substring(0, 1);
            }

            TalkerIdentifier = TalkerIdentifier.FromString(identifier);
        }

        public static Boolean IsChecksumValid(String sentence)
        {
            Int32 index = sentence.IndexOf('*');
            if (index == -1)
                return false;

            Byte checksum = Byte.Parse(sentence.Substring(index + 1, 2), System.Globalization.NumberStyles.HexNumber);

            Byte calculatedChecksum = CalculateChecksum(sentence.Remove(index));

            return calculatedChecksum == checksum;
        }

        /// <summary>
        /// Calculate the checksum value for the NMEA 0183 sentence
        /// </summary>
        /// <param name="sentence">The NMEA 0183 sentence to be computed, inclusive of the start delimiter "$" and just before the checksum delimiter "*"</param>
        /// <returns>The 8-bit XOR value.</returns>
        protected static Byte CalculateChecksum(String sentence)
        {
            Byte checksum = 0b0;
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(sentence.Substring(1));

            checksum = CalculateChecksum(checksum, data);

            return checksum;
        }

        /// <summary>
        /// Calculate NMEA 0183 checksum given a series of bytes. Consist of one round of XOR operation for each byte.
        /// </summary>
        /// <param name="initial">Initial byte value used.</param>
        /// <param name="data">Series of bytes to calculate the checksum for.</param>
        /// <returns>NMEA 0183 checksum</returns>
        protected static Byte CalculateChecksum(Byte initial, Byte[] data)
        {

            Byte checksum = initial;

            for (int i = 0; i < data.Length; ++i)
            {
                checksum = CalculateChecksum(checksum, data[i]);
            }

            return checksum;
        }

        /// <summary>
        /// Calculate NMEA 0183 checksum given a series of bytes. Consist of one round of XOR operation.
        /// </summary>
        /// <param name="initial">Initial byte value used.</param>
        /// <param name="data">Byte to calculate checksum for.</param>
        /// <returns>NMEA 0183 checksum</returns>
        protected static Byte CalculateChecksum(Byte initial, Byte data)
        {

            Byte checksum = initial;

            checksum = (Byte)(checksum ^ data);

            return checksum;
        }

        /// <summary>
        /// Decoding of the internal structure of the NMEA 0183 sentence based on the respective type.
        /// </summary>
        /// <param name="sentence">The NMEA 0183 sentence</param>
        protected abstract void DecodeInternalSentence(String sentence);

        /// <summary>
        /// Encodes the NMEA 0183 object and returns the appropriate NMEA 0183 sentence.
        /// </summary>
        /// <returns>The NMEA 0183 sentence</returns>
        public abstract String EncodeSentence();
    }
}
