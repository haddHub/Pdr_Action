using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxError;
using AxModel;
using NUnit.Framework;
using Moq;
using Common.Logging;
using System.IO;
using System.Data.SqlClient;
using AxError.Exceptions;
using System.Windows;
using System.Data;
namespace AxError.Test
{
    [TestFixture]
    public class GestionErreurTest
    {
        Mock<IMessageBoxService> msbs;
        Mock<ILog> log;
        [SetUp]
        public void Init()
        {
            msbs = new Mock<IMessageBoxService>();
            log = new Mock<ILog>();
            GestionErreur.MessageService = msbs.Object;
            GestionErreur.logger = log.Object;
        }
        //Verifie que les erreurs spécifique soit bien gérée.
        [Test]
        public void Erreur_OutOfMemoryException_Bien_Traite()
        {
            OutOfMemoryException ex = new OutOfMemoryException("OutOfMemory");
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowWarning(AxLanguage.Languages.REAplan_Erreur_PlaceDisqueDur+"\n"+AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone));
            log.Verify(ll => ll.Warn(ex.GetType().Name + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_FileNotFoundException_Bien_Traite()
        {
            FileNotFoundException ex = new FileNotFoundException("FileNotFound");
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(AxLanguage.Languages.REAplan_Erreur_Fichier + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_UnauthorizedAccessException_Bien_Traite()
        {
            UnauthorizedAccessException ex = new UnauthorizedAccessException("UnauthorizedAccessException");
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(AxLanguage.Languages.REAplan_Erreur_Admin + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_AlreadyOpenSerialPortException_Bien_Traite()
        {
            AlreadyOpenSerialPortException ex = new AlreadyOpenSerialPortException();
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowWarning(AxLanguage.Languages.REAplan_Erreur_PortSerie_1 + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone));
            log.Verify(ll => ll.Warn(ex.GetType().Name + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_WrongSerialPortException_Bien_Traite()
        {
            WrongSerialPortException ex = new WrongSerialPortException();
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowWarning(AxLanguage.Languages.REAplan_Erreur_PortSerie_2 + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone));
            log.Verify(ll => ll.Warn(ex.GetType().Name + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_Robot_OvercurrentError_Bien_Traite()
        {
            RobotException ex = new RobotException(1, FrameHeaders.Hardware, ErrorEmcyCodes.OvercurrentError, "RobotException");
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(ex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_ArretRobot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | NodeId: " + ex.NodeId + " | Adresse: " + ex.Adresse + " | ErrorCode :" + ex.ErrorCode + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_Robot_OvervoltageError_Bien_Traite()
        {
            RobotException ex = new RobotException(1, FrameHeaders.Hardware, ErrorEmcyCodes.OvervoltageError, "RobotException");
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(ex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Calibration));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | NodeId: " + ex.NodeId + " | Adresse: " + ex.Adresse + " | ErrorCode :" + ex.ErrorCode + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_Robot_UndervoltageError_Bien_Traite()
        {
            RobotException ex = new RobotException(1, FrameHeaders.Hardware, ErrorEmcyCodes.UndervoltageError, "RobotException");
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(ex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Calibration));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | NodeId: " + ex.NodeId + " | Adresse: " + ex.Adresse + " | ErrorCode :" + ex.ErrorCode + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_Robot_OvertemperatureError_Bien_Traite()
        {
            RobotException ex = new RobotException(1, FrameHeaders.Hardware, ErrorEmcyCodes.OvertemperatureError, "RobotException");
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(ex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_ArretRobot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | NodeId: " + ex.NodeId + " | Adresse: " + ex.Adresse + " | ErrorCode :" + ex.ErrorCode + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_Robot_LogicSupplyVoltageTooLowError_Bien_Traite()
        {
            RobotException ex = new RobotException(1, FrameHeaders.Hardware, ErrorEmcyCodes.LogicSupplyVoltageTooLowError, "RobotException");
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(ex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | NodeId: " + ex.NodeId + " | Adresse: " + ex.Adresse + " | ErrorCode :" + ex.ErrorCode + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_Robot_SupplyVoltageOutputStageTooLow_Bien_Traite()
        {
            RobotException ex = new RobotException(1, FrameHeaders.Hardware, ErrorEmcyCodes.SupplyVoltageOutputStageTooLow, "RobotException");
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(ex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | NodeId: " + ex.NodeId + " | Adresse: " + ex.Adresse + " | ErrorCode :" + ex.ErrorCode + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_Robot_InternalSoftwareError_Bien_Traite()
        {
            RobotException ex = new RobotException(1, FrameHeaders.Hardware, ErrorEmcyCodes.InternalSoftwareError, "RobotException");
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(ex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | NodeId: " + ex.NodeId + " | Adresse: " + ex.Adresse + " | ErrorCode :" + ex.ErrorCode + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_Robot_SoftwareParameterError_Bien_Traite()
        {
            RobotException ex = new RobotException(1, FrameHeaders.Hardware, ErrorEmcyCodes.SoftwareParameterError, "RobotException");
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(ex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | NodeId: " + ex.NodeId + " | Adresse: " + ex.Adresse + " | ErrorCode :" + ex.ErrorCode + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_Robot_PositionSensorError_Bien_Traite()
        {
            RobotException ex = new RobotException(1, FrameHeaders.Hardware, ErrorEmcyCodes.PositionSensorError, "RobotException");
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(ex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | NodeId: " + ex.NodeId + " | Adresse: " + ex.Adresse + " | ErrorCode :" + ex.ErrorCode + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_Robot_CanOverrunErrorObjectsLost_Bien_Traite()
        {
            RobotException ex = new RobotException(1, FrameHeaders.Hardware, ErrorEmcyCodes.CanOverrunErrorObjectsLost, "RobotException");
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(ex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Calibration));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | NodeId: " + ex.NodeId + " | Adresse: " + ex.Adresse + " | ErrorCode :" + ex.ErrorCode + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_Robot_CanOverrunError_Bien_Traite()
        {
            RobotException ex = new RobotException(1, FrameHeaders.Hardware, ErrorEmcyCodes.CanOverrunError, "RobotException");
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(ex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Calibration));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | NodeId: " + ex.NodeId + " | Adresse: " + ex.Adresse + " | ErrorCode :" + ex.ErrorCode + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_Robot_CanPassiveModeError_Bien_Traite()
        {
            RobotException ex = new RobotException(1, FrameHeaders.Hardware, ErrorEmcyCodes.CanPassiveModeError, "RobotException");
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(ex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Calibration));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | NodeId: " + ex.NodeId + " | Adresse: " + ex.Adresse + " | ErrorCode :" + ex.ErrorCode + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_Robot_CanTransmitCobIdCollisionError_Bien_Traite()
        {
            RobotException ex = new RobotException(1, FrameHeaders.Hardware, ErrorEmcyCodes.CanTransmitCobIdCollisionError, "RobotException");
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(ex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Calibration));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | NodeId: " + ex.NodeId + " | Adresse: " + ex.Adresse + " | ErrorCode :" + ex.ErrorCode + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_Robot_CanRxQueueOverflowError_Bien_Traite()
        {
            RobotException ex = new RobotException(1, FrameHeaders.Hardware, ErrorEmcyCodes.CanRxQueueOverflowError, "RobotException");
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(ex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Calibration));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | NodeId: " + ex.NodeId + " | Adresse: " + ex.Adresse + " | ErrorCode :" + ex.ErrorCode + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_Robot_CanTxQueueOverflowError_Bien_Traite()
        {
            RobotException ex = new RobotException(1, FrameHeaders.Hardware, ErrorEmcyCodes.CanTxQueueOverflowError, "RobotException");
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(ex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Calibration));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | NodeId: " + ex.NodeId + " | Adresse: " + ex.Adresse + " | ErrorCode :" + ex.ErrorCode + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_Robot_CanPdoLengthError_Bien_Traite()
        {
            RobotException ex = new RobotException(1, FrameHeaders.Hardware, ErrorEmcyCodes.CanPdoLengthError, "RobotException");
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(ex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Calibration));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | NodeId: " + ex.NodeId + " | Adresse: " + ex.Adresse + " | ErrorCode :" + ex.ErrorCode + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_Robot_FollowingError_Bien_Traite()
        {
            RobotException ex = new RobotException(1, FrameHeaders.Hardware, ErrorEmcyCodes.FollowingError, "RobotException");
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(ex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Calibration));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | NodeId: " + ex.NodeId + " | Adresse: " + ex.Adresse + " | ErrorCode :" + ex.ErrorCode + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_Robot_HallSensorError_Bien_Traite()
        {
            RobotException ex = new RobotException(1, FrameHeaders.Hardware, ErrorEmcyCodes.HallSensorError, "RobotException");
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(ex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | NodeId: " + ex.NodeId + " | Adresse: " + ex.Adresse + " | ErrorCode :" + ex.ErrorCode + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_Robot_IndexProcessingError_Bien_Traite()
        {
            RobotException ex = new RobotException(1, FrameHeaders.Hardware, ErrorEmcyCodes.IndexProcessingError, "RobotException");
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(ex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | NodeId: " + ex.NodeId + " | Adresse: " + ex.Adresse + " | ErrorCode :" + ex.ErrorCode + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_Robot_EncoderResolutionError_Bien_Traite()
        {
            RobotException ex = new RobotException(1, FrameHeaders.Hardware, ErrorEmcyCodes.EncoderResolutionError, "RobotException");
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(ex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | NodeId: " + ex.NodeId + " | Adresse: " + ex.Adresse + " | ErrorCode :" + ex.ErrorCode + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_Robot_HallSensorNotFoundError_Bien_Traite()
        {
            RobotException ex = new RobotException(1, FrameHeaders.Hardware, ErrorEmcyCodes.HallSensorNotFoundError, "RobotException");
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(ex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | NodeId: " + ex.NodeId + " | Adresse: " + ex.Adresse + " | ErrorCode :" + ex.ErrorCode + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_Robot_HallAngleDetectionError_Bien_Traite()
        {
            RobotException ex = new RobotException(1, FrameHeaders.Hardware, ErrorEmcyCodes.HallAngleDetectionError, "RobotException");
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(ex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Calibration));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | NodeId: " + ex.NodeId + " | Adresse: " + ex.Adresse + " | ErrorCode :" + ex.ErrorCode + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_Robot_SoftwarePositionLimitError_Bien_Traite()
        {
            RobotException ex = new RobotException(1, FrameHeaders.Hardware, ErrorEmcyCodes.SoftwarePositionLimitError, "RobotException");
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(ex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Calibration));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | NodeId: " + ex.NodeId + " | Adresse: " + ex.Adresse + " | ErrorCode :" + ex.ErrorCode + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_Robot_SystemOverloadedError_Bien_Traite()
        {
            RobotException ex = new RobotException(1, FrameHeaders.Hardware, ErrorEmcyCodes.SystemOverloadedError, "RobotException");
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(ex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | NodeId: " + ex.NodeId + " | Adresse: " + ex.Adresse + " | ErrorCode :" + ex.ErrorCode + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_Robot_InterpolatedPositionModeError_Bien_Traite()
        {
            RobotException ex = new RobotException(1, FrameHeaders.Hardware, ErrorEmcyCodes.InterpolatedPositionModeError, "RobotException");
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(ex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | NodeId: " + ex.NodeId + " | Adresse: " + ex.Adresse + " | ErrorCode :" + ex.ErrorCode + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_Robot_AutoTuningIdentificationError_Bien_Traite()
        {
            RobotException ex = new RobotException(1, FrameHeaders.Hardware, ErrorEmcyCodes.AutoTuningIdentificationError, "RobotException");
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(ex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | NodeId: " + ex.NodeId + " | Adresse: " + ex.Adresse + " | ErrorCode :" + ex.ErrorCode + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_Robot_GearScalingFactorError_Bien_Traite()
        {
            RobotException ex = new RobotException(1, FrameHeaders.Hardware, ErrorEmcyCodes.GearScalingFactorError, "RobotException");
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(ex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | NodeId: " + ex.NodeId + " | Adresse: " + ex.Adresse + " | ErrorCode :" + ex.ErrorCode + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_Robot_ControllerGainError_Bien_Traite()
        {
            RobotException ex = new RobotException(1, FrameHeaders.Hardware, ErrorEmcyCodes.ControllerGainError, "RobotException");
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(ex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | NodeId: " + ex.NodeId + " | Adresse: " + ex.Adresse + " | ErrorCode :" + ex.ErrorCode + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_Robot_MainSensorDirectionError_Bien_Traite()
        {
            RobotException ex = new RobotException(1, FrameHeaders.Hardware, ErrorEmcyCodes.MainSensorDirectionError, "RobotException");
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(ex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | NodeId: " + ex.NodeId + " | Adresse: " + ex.Adresse + " | ErrorCode :" + ex.ErrorCode + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_Robot_AuxiliarySensorDirectionError_Bien_Traite()
        {
            RobotException ex = new RobotException(1, FrameHeaders.Hardware, ErrorEmcyCodes.AuxiliarySensorDirectionError, "RobotException");
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(ex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | NodeId: " + ex.NodeId + " | Adresse: " + ex.Adresse + " | ErrorCode :" + ex.ErrorCode + " | " + ex.Message + " |" + ex.StackTrace));
        }
        //Verifie les erreur Sql numéro par numéro.
        //SqlEcxeption ne possedant pas de constructeur, il faut passer par SqlExceptionHelper.
        [Test]
        public void Erreur_SqlException_Num2_Bien_Traite()
        {
            SqlException ex = SqlExceptionHelper.GenerateMain(2);
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError("Impossible de se connecter à la base de données !"));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_SqlException_Num53_Bien_Traite()
        {
            SqlException ex = SqlExceptionHelper.GenerateMain(53);
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError("Impossible de se connecter à la base de données !"));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_SqlException_Num10054_Bien_Traite()
        {
            SqlException ex = SqlExceptionHelper.GenerateMain(10054);
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError("Impossible de se connecter à la base de données !"));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_SqlException_Num4060_Bien_Traite()
        {
            SqlException ex = SqlExceptionHelper.GenerateMain(4060);
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError("Impossible d'ouvrir la base de données !"));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_SqlException_Num18452_Bien_Traite()
        {
            SqlException ex = SqlExceptionHelper.GenerateMain(18452);
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(AxLanguage.Languages.REAplan_Erreur_Sql_Mdp));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_SqlException_Num18456_Bien_Traite()
        {
            SqlException ex = SqlExceptionHelper.GenerateMain(18456);
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(AxLanguage.Languages.REAplan_Erreur_Sql_Mdp));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | " + ex.Message + " |" + ex.StackTrace));
        }
        [Test]
        public void Erreur_SqlException_Autres_Num_Bien_Traite()
        {
            SqlException ex = SqlExceptionHelper.GenerateMain(5);
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError("Erreur avec la base données !"));
            log.Verify(ll => ll.Error(ex.GetType().Name + " | " + ex.Message + " |" + ex.StackTrace));
        }
        //Verfie que tout autres type d'exception font planter l'application.
        //Pour pouvoir utiliser le Application.Current.Shutdown() une nouvelle instance de Application doit être créée.
        [Test]
        public void Erreur_Autres_Exception_Doit_Couper_Le_Programme()
        {
            new Application();
            Exception ex = new Exception("Exception");
            GestionErreur.GerrerErreur(ex);
            msbs.Verify(mm => mm.ShowError(AxLanguage.Languages.REAplan_Erreur_Critique + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone));
            log.Verify(ll => ll.Fatal(ex.GetType().Name + " | " + ex.Message + " |" + ex.StackTrace));
        }
    }
}
