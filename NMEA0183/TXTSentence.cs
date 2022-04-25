using System;
using System.Text;

namespace TensionDev.Maritime.NMEA0183
{
    /// <summary>
    /// TXT - Text Transmission
    /// </summary>
    public class TXTSentence : NMEASentence
    {
        private Byte _totalNumberOfSentences;
        private Byte _sentenceNumber;
        private Byte _textIdentifier;

        /// <summary>
        /// Total number of sentences, 01 to 99
        /// </summary>
        public Byte TotalNumberOfSentences { get => _totalNumberOfSentences; set => _totalNumberOfSentences = Math.Min(Math.Max(value, (Byte)1), (Byte)99); }

        /// <summary>
        /// Sentence number, 01 to 99
        /// </summary>
        public Byte SentenceNumber { get => _sentenceNumber; set => _sentenceNumber = Math.Min(Math.Max(value, (Byte)1), (Byte)99); }

        /// <summary>
        /// Text identifier, 01 to 99
        /// </summary>
        public Byte TextIdentifier { get => _textIdentifier; set => _textIdentifier = Math.Min(Math.Max(value, (Byte)1), (Byte)99); }

        /// <summary>
        /// Text message
        /// </summary>
        public String TextMessage { get; set; }

        public TXTSentence()
        {
            SentenceIdentifier = "TXT";
            _totalNumberOfSentences = 1;
            _sentenceNumber = 1;
            _textIdentifier = 1;
        }

        public override String EncodeSentence()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("${0}{1},", TalkerIdentifier.ToString(), SentenceIdentifier);

            stringBuilder.AppendFormat("{0},", TotalNumberOfSentences.ToString("D2"));

            stringBuilder.AppendFormat("{0},", SentenceNumber.ToString("D2"));

            stringBuilder.AppendFormat("{0},", TextIdentifier.ToString("D2"));

            String encoded = EncodeUndefinedCharacters(TextMessage);
            stringBuilder.AppendFormat("{0}", encoded);

            Byte checksum = CalculateChecksum(stringBuilder.ToString());

            stringBuilder.AppendFormat("*{0}\r\n", checksum.ToString("X2"));

            return stringBuilder.ToString();
        }

        protected override void DecodeInternalSentence(String sentence)
        {
            DecodeTalker(sentence);

            String[] vs = sentence.Split(new char[] { ',', '*' });

            // xx Total number of sentences
            TotalNumberOfSentences = Byte.Parse(vs[1]);

            // xx Sentence number
            SentenceNumber = Byte.Parse(vs[2]);

            // xx Text identifier
            TextIdentifier = Byte.Parse(vs[3]);

            // c--c Text message
            string encoded = vs[4];
            TextMessage = DecodeUndefinedCharacters(encoded);
        }

        private String EncodeUndefinedCharacters(String textMessage)
        {
            String vs = textMessage;

            // ^
            vs = vs.Replace("^", "^5E");

            // <CR>
            vs = vs.Replace("\r", "^0D");

            // <LF>
            vs = vs.Replace("\n", "^0A");

            // $
            vs = vs.Replace("$", "^24");

            // *
            vs = vs.Replace("*", "^2A");

            // ,
            vs = vs.Replace(",", "^2C");

            // !
            vs = vs.Replace("!", "^21");

            // \
            vs = vs.Replace("\\", "^5C");

            for (ushort i = 0; i < 32; ++i)
            {
                char c = (char)i;
                string s = String.Format("^{0}", i.ToString("X2"));
                vs = vs.Replace(c.ToString(), s);
            }

            for (ushort i = 128; i < 256; ++i)
            {
                char c = (char)i;
                string s = String.Format("^{0}", i.ToString("X2"));
                vs = vs.Replace(c.ToString(), s);
            }

            // ~
            // <del>

            return vs;
        }

        private String DecodeUndefinedCharacters(String textMessage)
        {
            String vs = textMessage;

            // <del>
            // ~

            for (ushort i = 128; i < 256; ++i)
            {
                char c = (char)i;
                string s = String.Format("^{0}", i.ToString("X2"));
                vs = vs.Replace(s, c.ToString());
            }

            for (ushort i = 0; i < 32; ++i)
            {
                char c = (char)i;
                string s = String.Format("^{0}", i.ToString("X2"));
                vs = vs.Replace(s, c.ToString());
            }

            // \
            vs = vs.Replace("^5C", "\\");

            // !
            vs = vs.Replace("^21", "!");

            // ,
            vs = vs.Replace("^2C", ",");

            // *
            vs = vs.Replace("^2A", "*");

            // $
            vs = vs.Replace("^24", "$");

            // <LF>
            vs = vs.Replace("^0A", "\n");

            // <CR>
            vs = vs.Replace("^0D", "\r");

            // ^
            vs = vs.Replace("^", "^5E");

            return vs;
        }
    }
}
