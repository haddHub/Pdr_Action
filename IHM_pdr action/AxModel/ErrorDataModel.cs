using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class ErrorDataModel : aXdataModel
    {
        // TODO : A implementer
        #region Fields
        /// <summary>
        /// Gets or sets the config address. See <see cref="ConfigAddresses"/>.
        /// </summary>
        public FrameHeaders Address { get; set; }
        /// <summary>
        /// Gets or sets the error code. Must be defined in <see cref="ErrorEmcyCodes"/>.
        /// </summary>
        /// <exception cref="System.Exception">
        /// Thrown if invalid error code specifed.
        /// </exception>
        public byte NodeId { get; set; }
        public byte Registre { get; set; }
        public ErrorEmcyCodes ErrorCode { get; set; }

        #endregion

        #region Constructors
        /// <summary>
        /// Initialises a new instance of the <see cref="ErrorData"/> class.
        /// </summary>
        public ErrorDataModel()
            : this(FrameHeaders.Error, 0, 0, ErrorEmcyCodes.NoError)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ErrorData"/> class.
        /// </summary>
        /// <param name="errorCode">
        /// Error code. See <see cref="ErrorEmcyCodes"/>.
        /// </param>
        public ErrorDataModel(FrameHeaders adr, byte nId, byte r, ErrorEmcyCodes errorCode)
            : this((int)errorCode)
        {
            Address = adr;
            NodeId = nId;
            Registre = r;
        }

        public ErrorDataModel(FrameHeaders adr, byte nId, byte r, int errorCode)
            : this((int)errorCode)
        {
            Address = adr;
            NodeId = nId;
            Registre = r;
        }

        /// <summary>
        /// Initialises a new instance of the ErrorData class.
        /// </summary>
        /// <param name="errorCode">
        /// Error code. Must be defined in <see cref="ErrorEmcyCodes"/>.
        /// </param>
        public ErrorDataModel(int errorCode)
        {
            if (!Enum.IsDefined(typeof(ErrorEmcyCodes), (int)errorCode))
            {
                //throw new Exception("Invalid error code.");
            }
            //Address = FrameHeaders.Error;
            ErrorCode = (ErrorEmcyCodes)errorCode;
            //NodeId = 0;
            //Registre = 0;
        }

        #endregion

        #region Properties
        #endregion

        #region Methods
        /// <summary>
        /// Message associated with error code.
        /// </summary>
        /// <returns>
        /// Message.
        /// </returns>
        public string GetMessage()
        {
            switch (ErrorCode)
            {
                case (ErrorEmcyCodes.NoError): return "No error.";
                case (ErrorEmcyCodes.GenericError): return "Generic Error.";
                case (ErrorEmcyCodes.OvercurrentError): return "Overcurrent Error.";
                case (ErrorEmcyCodes.OvervoltageError): return "Overvoltage Error.";
                case (ErrorEmcyCodes.UndervoltageError): return "Undervoltage Error.";
                case (ErrorEmcyCodes.OvertemperatureError): return "Overtemperature Error.";
                case (ErrorEmcyCodes.LogicSupplyVoltageTooLowError): return "Logic Supply Voltage Too Low Error.";
                case (ErrorEmcyCodes.SupplyVoltageOutputStageTooLow): return "Supply Voltage Output Stage Too Low.";
                case (ErrorEmcyCodes.InternalSoftwareError): return "Internal Software Error.";
                case (ErrorEmcyCodes.SoftwareParameterError): return "Software Parameter Error.";
                case (ErrorEmcyCodes.PositionSensorError): return "Position Sensor Error.";
                case (ErrorEmcyCodes.CanOverrunErrorObjectsLost): return "CAN Overrun Error Objects Lost.";
                case (ErrorEmcyCodes.CanOverrunError): return "CAN Overrun Error.";
                case (ErrorEmcyCodes.CanPassiveModeError): return "CAN Passive Mode Error.";
                case (ErrorEmcyCodes.CanLifeGuardError): return "CAN Life Guard Error.";
                case (ErrorEmcyCodes.CanTransmitCobIdCollisionError): return "CAN Transmit COB-ID Collision Error.";
                case (ErrorEmcyCodes.CanBusOffError): return "CAN Bus Off Error.";
                case (ErrorEmcyCodes.CanRxQueueOverflowError): return "CAN Rx Queue Overflow Error.";
                case (ErrorEmcyCodes.CanTxQueueOverflowError): return "CAN Tx Queue Overflow Error.";
                case (ErrorEmcyCodes.CanPdoLengthError): return "CAN PDO Length Error.";
                case (ErrorEmcyCodes.FollowingError): return "Following Error.";
                case (ErrorEmcyCodes.HallSensorError): return "HallSensor Error.";
                case (ErrorEmcyCodes.IndexProcessingError): return "Index Processing Error.";
                case (ErrorEmcyCodes.EncoderResolutionError): return "Encoder Resolution Error.";
                case (ErrorEmcyCodes.HallSensorNotFoundError): return "Hall Sensor Not Found Error.";
                case (ErrorEmcyCodes.NegativeLimitSwitchError): return "Negative Limit Switch Error.";
                case (ErrorEmcyCodes.PositiveLimitSwitchError): return "Positive Limit Switch Error.";
                case (ErrorEmcyCodes.HallAngleDetectionError): return "Hall Angle Detection Error.";
                case (ErrorEmcyCodes.SoftwarePositionLimitError): return "Software Position Limit Error.";
                case (ErrorEmcyCodes.PositionSensorBreachError): return "Position Sensor Breach Error.";
                case (ErrorEmcyCodes.SystemOverloadedError): return "System Overloaded Error.";
                case (ErrorEmcyCodes.InterpolatedPositionModeError): return "Interpolated Position Mode Error.";
                case (ErrorEmcyCodes.AutoTuningIdentificationError): return "Auto Tuning Identification Error.";
                case (ErrorEmcyCodes.GearScalingFactorError): return "Gear Scaling Factor Error.";
                case (ErrorEmcyCodes.ControllerGainError): return "Controller Gain Error.";
                case (ErrorEmcyCodes.MainSensorDirectionError): return "Main Sensor Direction Error.";
                case (ErrorEmcyCodes.AuxiliarySensorDirectionError): return "Auxiliary Sensor Direction Error.";
                default: return "";
            }
        }

        #endregion
    }
}
