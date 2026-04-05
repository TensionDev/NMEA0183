using System;

namespace TensionDev.Maritime.NMEA0183
{
    public class TalkerIdentifier
    {
        public TalkerIdentifierEnum TalkerIdentifierEnum { get; set; }

        public TalkerIdentifier()
        {
            TalkerIdentifierEnum = TalkerIdentifierEnum.GlobalPositioningSystem;
        }

        /// <summary>
        /// Returns the Talker Identifer code
        /// </summary>
        /// <returns>The Talker Identifier code</returns>
        public override String ToString()
        {
            switch (TalkerIdentifierEnum)
            {
                case TalkerIdentifierEnum.AutopilotGeneral:
                    return "AG";
                case TalkerIdentifierEnum.AutopilotMagnetic:
                    return "AP";
                case TalkerIdentifierEnum.CommunicationsDigitalSelectiveCalling:
                    return "CD";
                case TalkerIdentifierEnum.CommunicationsReceiverOrBeaconReceiver:
                    return "CR";
                case TalkerIdentifierEnum.CommunicationsSatellite:
                    return "CS";
                case TalkerIdentifierEnum.CommunicationsRadioTelephoneMFOrHF:
                    return "CT";
                case TalkerIdentifierEnum.CommunicationsRadioTelephoneVHF:
                    return "CV";
                case TalkerIdentifierEnum.CommunicationsScanningReceiver:
                    return "CX";
                case TalkerIdentifierEnum.DirectionFinder:
                    return "DF";
                case TalkerIdentifierEnum.ElectronicChartDisplayInformationSystem:
                    return "EC";
                case TalkerIdentifierEnum.EmergencyPositionIndicatingBeacon:
                    return "EP";
                case TalkerIdentifierEnum.EngineRoomMonitoringSystems:
                    return "ER";
                case TalkerIdentifierEnum.GlobalPositioningSystem:
                    return "GP";
                case TalkerIdentifierEnum.HeadingMagneticCompass:
                    return "HC";
                case TalkerIdentifierEnum.HeadingNorthSeekingGyro:
                    return "HE";
                case TalkerIdentifierEnum.HeadingNonNorthSeekingGyro:
                    return "HN";
                case TalkerIdentifierEnum.IntegratedInstrumentation:
                    return "II";
                case TalkerIdentifierEnum.IntegratedNavigation:
                    return "IN";
                case TalkerIdentifierEnum.LoranC:
                    return "LC";
                case TalkerIdentifierEnum.ProprietaryCode:
                    return "P";
                case TalkerIdentifierEnum.RADAROrARPA:
                    return "RA";
                case TalkerIdentifierEnum.SounderDepth:
                    return "SD";
                case TalkerIdentifierEnum.ElectronicPositioningSystemOtherOrGeneral:
                    return "SN";
                case TalkerIdentifierEnum.SounderScanning:
                    return "SS";
                case TalkerIdentifierEnum.TurnRateIndicator:
                    return "TI";
                case TalkerIdentifierEnum.VelocitySensorDopplerOtherOrGeneral:
                    return "VD";
                case TalkerIdentifierEnum.VelocitySensorSpeedLogWaterMagnetic:
                    return "DM";
                case TalkerIdentifierEnum.VelocitySensorSpeedLogWaterMechanical:
                    return "VW";
                case TalkerIdentifierEnum.WeatherInstruments:
                    return "WI";
                case TalkerIdentifierEnum.Transducer:
                    return "YX";
                case TalkerIdentifierEnum.TimekeeperAtomicClock:
                    return "ZA";
                case TalkerIdentifierEnum.TimekeeperChronometer:
                    return "ZC";
                case TalkerIdentifierEnum.TimekeeperQuartz:
                    return "ZQ";
                case TalkerIdentifierEnum.TimekeeperRadioUpdate:
                    return "ZV";

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Returns the Talker Identifier object
        /// </summary>
        /// <param name="identifier">The Talker Identifier code</param>
        /// <returns>The Talker Identifier object</returns>
        public static TalkerIdentifier FromString(String identifier)
        {
            TalkerIdentifierEnum talkerIdentifierEnum;
            switch (identifier)
            {
                case "AG":
                    talkerIdentifierEnum = TalkerIdentifierEnum.AutopilotGeneral;
                    break;
                case "AP":
                    talkerIdentifierEnum = TalkerIdentifierEnum.AutopilotMagnetic;
                    break;
                case "CD":
                    talkerIdentifierEnum = TalkerIdentifierEnum.CommunicationsDigitalSelectiveCalling;
                    break;
                case "CR":
                    talkerIdentifierEnum = TalkerIdentifierEnum.CommunicationsReceiverOrBeaconReceiver;
                    break;
                case "CS":
                    talkerIdentifierEnum = TalkerIdentifierEnum.CommunicationsSatellite;
                    break;
                case "CT":
                    talkerIdentifierEnum = TalkerIdentifierEnum.CommunicationsRadioTelephoneMFOrHF;
                    break;
                case "CV":
                    talkerIdentifierEnum = TalkerIdentifierEnum.CommunicationsRadioTelephoneVHF;
                    break;
                case "CX":
                    talkerIdentifierEnum = TalkerIdentifierEnum.CommunicationsScanningReceiver;
                    break;
                case "DF":
                    talkerIdentifierEnum = TalkerIdentifierEnum.DirectionFinder;
                    break;
                case "EC":
                    talkerIdentifierEnum = TalkerIdentifierEnum.ElectronicChartDisplayInformationSystem;
                    break;
                case "EP":
                    talkerIdentifierEnum = TalkerIdentifierEnum.EmergencyPositionIndicatingBeacon;
                    break;
                case "ER":
                    talkerIdentifierEnum = TalkerIdentifierEnum.EngineRoomMonitoringSystems;
                    break;
                case "GP":
                    talkerIdentifierEnum = TalkerIdentifierEnum.GlobalPositioningSystem;
                    break;
                case "HC":
                    talkerIdentifierEnum = TalkerIdentifierEnum.HeadingMagneticCompass;
                    break;
                case "HE":
                    talkerIdentifierEnum = TalkerIdentifierEnum.HeadingNorthSeekingGyro;
                    break;
                case "HN":
                    talkerIdentifierEnum = TalkerIdentifierEnum.HeadingNonNorthSeekingGyro;
                    break;
                case "II":
                    talkerIdentifierEnum = TalkerIdentifierEnum.IntegratedInstrumentation;
                    break;
                case "IN":
                    talkerIdentifierEnum = TalkerIdentifierEnum.IntegratedNavigation;
                    break;
                case "LC":
                    talkerIdentifierEnum = TalkerIdentifierEnum.LoranC;
                    break;
                case "P":
                    talkerIdentifierEnum = TalkerIdentifierEnum.ProprietaryCode;
                    break;
                case "RA":
                    talkerIdentifierEnum = TalkerIdentifierEnum.RADAROrARPA;
                    break;
                case "SD":
                    talkerIdentifierEnum = TalkerIdentifierEnum.SounderDepth;
                    break;
                case "SN":
                    talkerIdentifierEnum = TalkerIdentifierEnum.ElectronicPositioningSystemOtherOrGeneral;
                    break;
                case "SS":
                    talkerIdentifierEnum = TalkerIdentifierEnum.SounderScanning;
                    break;
                case "TI":
                    talkerIdentifierEnum = TalkerIdentifierEnum.TurnRateIndicator;
                    break;
                case "VD":
                    talkerIdentifierEnum = TalkerIdentifierEnum.VelocitySensorDopplerOtherOrGeneral;
                    break;
                case "DM":
                    talkerIdentifierEnum = TalkerIdentifierEnum.VelocitySensorSpeedLogWaterMagnetic;
                    break;
                case "VW":
                    talkerIdentifierEnum = TalkerIdentifierEnum.VelocitySensorSpeedLogWaterMechanical;
                    break;
                case "WI":
                    talkerIdentifierEnum = TalkerIdentifierEnum.WeatherInstruments;
                    break;
                case "YX":
                    talkerIdentifierEnum = TalkerIdentifierEnum.Transducer;
                    break;
                case "ZA":
                    talkerIdentifierEnum = TalkerIdentifierEnum.TimekeeperAtomicClock;
                    break;
                case "ZC":
                    talkerIdentifierEnum = TalkerIdentifierEnum.TimekeeperChronometer;
                    break;
                case "ZQ":
                    talkerIdentifierEnum = TalkerIdentifierEnum.TimekeeperQuartz;
                    break;
                case "ZV":
                    talkerIdentifierEnum = TalkerIdentifierEnum.TimekeeperRadioUpdate;
                    break;

                default:
                    return null;
            }

            return new TalkerIdentifier() { TalkerIdentifierEnum = talkerIdentifierEnum };
        }
    }

    public enum TalkerIdentifierEnum
    {
        AutopilotGeneral,
        AutopilotMagnetic,
        CommunicationsDigitalSelectiveCalling,
        CommunicationsReceiverOrBeaconReceiver,
        CommunicationsSatellite,
        CommunicationsRadioTelephoneMFOrHF,
        CommunicationsRadioTelephoneVHF,
        CommunicationsScanningReceiver,
        DirectionFinder,
        ElectronicChartDisplayInformationSystem,
        EmergencyPositionIndicatingBeacon,
        EngineRoomMonitoringSystems,
        GlobalPositioningSystem,
        HeadingMagneticCompass,
        HeadingNorthSeekingGyro,
        HeadingNonNorthSeekingGyro,
        IntegratedInstrumentation,
        IntegratedNavigation,
        LoranC,
        ProprietaryCode,
        RADAROrARPA,
        SounderDepth,
        ElectronicPositioningSystemOtherOrGeneral,
        SounderScanning,
        TurnRateIndicator,
        VelocitySensorDopplerOtherOrGeneral,
        VelocitySensorSpeedLogWaterMagnetic,
        VelocitySensorSpeedLogWaterMechanical,
        WeatherInstruments,
        Transducer,
        TimekeeperAtomicClock,
        TimekeeperChronometer,
        TimekeeperQuartz,
        TimekeeperRadioUpdate
    }
}
