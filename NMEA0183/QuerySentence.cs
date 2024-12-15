using System;
using System.Collections.Generic;
using System.Text;

namespace TensionDev.Maritime.NMEA0183
{
    /// <summary>
    /// Q - Query Sentence
    /// </summary>
    public class QuerySentence : NMEASentence
    {
        /// <summary>
        /// Listener Identifier
        /// </summary>
        public TalkerIdentifier ListenerIdentifier { get; set; }

        /// <summary>
        /// Sentence Identifier
        /// </summary>
        public String QuerySentenceIdentifier { get; set; }

        public QuerySentence()
        {
            SentenceIdentifier = "Q";
        }

        public override String EncodeSentence()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("${0}{1}{2},", TalkerIdentifier.ToString(), ListenerIdentifier.ToString(), SentenceIdentifier);

            stringBuilder.AppendFormat("{0}", QuerySentenceIdentifier);

            Byte checksum = CalculateChecksum(stringBuilder.ToString());

            stringBuilder.AppendFormat("*{0}\r\n", checksum.ToString("X2"));

            return stringBuilder.ToString();
        }

        protected override void DecodeInternalSentence(String sentence)
        {
            DecodeTalker(sentence);

            String[] vs = sentence.Split(',', '*');

            ListenerIdentifier = TalkerIdentifier.FromString(vs[0].Substring(3, 2));

            QuerySentenceIdentifier = vs[1];
        }
    }
}
