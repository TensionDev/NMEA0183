using System;
using System.Text;

namespace TensionDev.Maritime.NMEA0183
{
    /// <summary>
    /// FSI - Frequency Set Information
    /// </summary>
    public class FSISentence : NMEASentence
    {
        private UInt32 powerLevel;

        /// <summary>
        /// Transmitting Frequency
        /// </summary>
        public String TransmittingFrequency { get; set; }

        /// <summary>
        /// Receiving Frequency
        /// </summary>
        public String ReceivingFrequency { get; set; }

        /// <summary>
        /// Mode of operation
        /// </summary>
        public Nullable<ModeOfOperationEnum> ModeOfOperation { get; set; }

        /// <summary>
        /// Power level
        /// </summary>
        public UInt32 PowerLevel { get => powerLevel; set => powerLevel = Math.Min(value, 9); }

        public FSISentence()
        {
            SentenceIdentifier = "FSI";
            TransmittingFrequency = null;
            ReceivingFrequency = null;
            ModeOfOperation = null;
            PowerLevel = 0;
        }

        public override String EncodeSentence()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("${0}{1},", TalkerIdentifier.ToString(), SentenceIdentifier);

            if (String.IsNullOrWhiteSpace(TransmittingFrequency))
                stringBuilder.Append(",");
            else
                stringBuilder.AppendFormat("{0},", TransmittingFrequency);

            if (String.IsNullOrWhiteSpace(ReceivingFrequency))
                stringBuilder.Append(",");
            else
                stringBuilder.AppendFormat("{0},", ReceivingFrequency);

            if (ModeOfOperation.HasValue)
                stringBuilder.AppendFormat("{0},", EncodeModeOfOperation());
            else
                stringBuilder.Append(",");

            stringBuilder.AppendFormat("{0}", PowerLevel);

            Byte checksum = CalculateChecksum(stringBuilder.ToString());

            stringBuilder.AppendFormat("*{0}\r\n", checksum.ToString("X2"));

            return stringBuilder.ToString();
        }

        protected override void DecodeInternalSentence(String sentence)
        {
            DecodeTalker(sentence);

            String[] vs = sentence.Split(',', '*');

            // xxxxxx Transmitting Frequency
            if (vs[1].Length > 0)
            {
                TransmittingFrequency = vs[1];
            }
            else
            {
                TransmittingFrequency = null;
            }

            // xxxxxx Receiving Frequency
            if (vs[2].Length > 0)
            {
                ReceivingFrequency = vs[2];
            }
            else
            {
                ReceivingFrequency = null;
            }

            // c Mode of Operation
            DecodeModeOfOperation(vs[3]);


            // x Power level
            PowerLevel = UInt32.Parse(vs[4]);
        }

        private String EncodeModeOfOperation()
        {
            if (!ModeOfOperation.HasValue)
            {
                return String.Empty;
            }

            switch (ModeOfOperation.Value)
            {
                case ModeOfOperationEnum.F3E_G3E_Simplex_Telephone:
                    return "d";

                case ModeOfOperationEnum.F3E_G3E_Duplex_Telephone:
                    return "e";

                case ModeOfOperationEnum.J3E_Telephone:
                    return "m";

                case ModeOfOperationEnum.H3E_Telephone:
                    return "o";

                case ModeOfOperationEnum.F1B_J2B_FEC_NBDP_Telex_Teleprinter:
                    return "q";

                case ModeOfOperationEnum.F1B_J2B_ARQ_NBDP_Telex_Teleprinter:
                    return "s";

                case ModeOfOperationEnum.F1B_J2B_ReceiveOnly_Teleprinter_DSC:
                    return "t";

                case ModeOfOperationEnum.F1B_J2B_Teleprinter_DSC:
                    return "w";

                case ModeOfOperationEnum.A1A_Morse_Tape_Recorder:
                    return "x";

                case ModeOfOperationEnum.A1A_Morse_Morse_Key_Headset:
                    return "{";

                case ModeOfOperationEnum.F1C_F2C_F3C_FAX_Machine:
                    return "|";

                default:
                    return String.Empty;
            }
        }

        private void DecodeModeOfOperation(String mode)
        {
            if (String.IsNullOrEmpty(mode))
            {
                ModeOfOperation = null;
                return;
            }

            switch (mode)
            {
                case "d":
                    ModeOfOperation = ModeOfOperationEnum.F3E_G3E_Simplex_Telephone;
                    break;

                case "e":
                    ModeOfOperation = ModeOfOperationEnum.F3E_G3E_Duplex_Telephone;
                    break;

                case "m":
                    ModeOfOperation = ModeOfOperationEnum.J3E_Telephone;
                    break;

                case "o":
                    ModeOfOperation = ModeOfOperationEnum.H3E_Telephone;
                    break;

                case "q":
                    ModeOfOperation = ModeOfOperationEnum.F1B_J2B_FEC_NBDP_Telex_Teleprinter;
                    break;

                case "s":
                    ModeOfOperation = ModeOfOperationEnum.F1B_J2B_ARQ_NBDP_Telex_Teleprinter;
                    break;

                case "t":
                    ModeOfOperation = ModeOfOperationEnum.F1B_J2B_ReceiveOnly_Teleprinter_DSC;
                    break;

                case "w":
                    ModeOfOperation = ModeOfOperationEnum.F1B_J2B_Teleprinter_DSC;
                    break;

                case "x":
                    ModeOfOperation = ModeOfOperationEnum.A1A_Morse_Tape_Recorder;
                    break;

                case "{":
                    ModeOfOperation = ModeOfOperationEnum.A1A_Morse_Morse_Key_Headset;
                    break;

                case "|":
                    ModeOfOperation = ModeOfOperationEnum.F1C_F2C_F3C_FAX_Machine;
                    break;

                default:
                    ModeOfOperation = null;
                    break;
            }
        }

        public enum ModeOfOperationEnum
        {
            /// <summary>
            /// F3E/G3E simplex, telephone
            /// </summary>
            F3E_G3E_Simplex_Telephone,
            /// <summary>
            /// F3E/G3E duplex, telephone
            /// </summary>
            F3E_G3E_Duplex_Telephone,
            /// <summary>
            /// J3E, telephone
            /// </summary>
            J3E_Telephone,
            /// <summary>
            /// H3E, telephone
            /// </summary>
            H3E_Telephone,
            /// <summary>
            /// F1B/J2B FEC NBDP, Telex/teleprinter
            /// </summary>
            F1B_J2B_FEC_NBDP_Telex_Teleprinter,
            /// <summary>
            /// F1B/J2B ARQ NBDP, Telex/teleprinter
            /// </summary>
            F1B_J2B_ARQ_NBDP_Telex_Teleprinter,
            /// <summary>
            /// F1B/J2B receive only, teleprinter/DSC 
            /// </summary>
            F1B_J2B_ReceiveOnly_Teleprinter_DSC,
            /// <summary>
            /// F1B/J2B, teleprinter/DSC 
            /// </summary>
            F1B_J2B_Teleprinter_DSC,
            /// <summary>
            /// A1A Morse, tape recorder 
            /// </summary>
            A1A_Morse_Tape_Recorder,
            /// <summary>
            /// A1A Morse, Morse key/head set 
            /// </summary>
            A1A_Morse_Morse_Key_Headset,
            /// <summary>
            /// F1C/F2C/F3C, FAX-machine 
            /// </summary>
            F1C_F2C_F3C_FAX_Machine,
        }
    }
}
