using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Common.Logging;
using AxLanguage;
using System.IO;
using System.Data.SqlClient;
using AxModel;
using AxError.Exceptions;
using System.Data;
using System.Windows.Threading;
namespace AxError
{
    public static class GestionErreur
    {
        public static ILog logger = LogManager.GetCurrentClassLogger();
        public static IMessageBoxService MessageService = new MessageBoxService();
        private static string errorMessage;

        public static void GerrerErreur(Exception ex)
        {
            try
            {
                if (ex is OutOfMemoryException)//Plus de place sur le dd.
                {
                    errorMessage = AxLanguage.Languages.REAplan_Erreur_PlaceDisqueDur+"\n"+AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone;
                    AfficherWarningMessageBox(errorMessage);
                    LoggerWarning(ex);
                }
                else if (ex is FileNotFoundException)//fichier ou dossier non présent
                {
                    errorMessage = AxLanguage.Languages.REAplan_Erreur_Fichier + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone;
                    AfficherErreurMessageBox(errorMessage);
                    LoggerError(ex);
                }
                else if (ex is UnauthorizedAccessException)//pas les droits d'admin
                {
                    errorMessage = AxLanguage.Languages.REAplan_Erreur_Admin + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone;
                    AfficherErreurMessageBox(errorMessage);
                    LoggerError(ex);
                }
                else if (ex is AlreadyOpenSerialPortException)//Le port serie est déjà utilisé
                {
                    errorMessage = AxLanguage.Languages.REAplan_Erreur_PortSerie_1 + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone;
                    AfficherWarningMessageBox(errorMessage);
                    LoggerWarning(ex);
                }
                else if (ex is WrongSerialPortException)//Mauvais port COM
                {
                    errorMessage = AxLanguage.Languages.REAplan_Erreur_PortSerie_2 + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone;
                    AfficherWarningMessageBox(errorMessage);
                    LoggerWarning(ex);
                }
                else if (ex is SqlException)//Erreur avec la bd
                {
                    TrouverSqlErreur(ex as SqlException);
                    AfficherErreurMessageBox(errorMessage);
                    LoggerError(ex);
                }
                else if (ex is RobotException)
                {
                    MessageErreurRobot(ex as RobotException);
                    AfficherErreurMessageBox(errorMessage);
                    LoggerRobotError(ex as RobotException);
                }
                else if (ex is UpdateException)
                {
                    SqlException innerException = ex.InnerException as SqlException;
                    TrouverSqlErreur(innerException as SqlException);
                    AfficherErreurMessageBox(errorMessage);
                    LoggerError(ex);
                }
                else if (ex is EntityException || ex is ConstraintException || ex is DuplicateNameException || ex is InvalidCommandTreeException || ex is MissingPrimaryKeyException)
                {
                    errorMessage = AxLanguage.Languages.REAplan_Erreur_BD+ "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone;
                    AfficherErreurMessageBox(errorMessage);
                    LoggerErrorEntity(ex);
                }
                else if (ex is MauvaiseFermetureException)
                {
                    LoggerMauvaiseFermeture(ex);
                }
                else //Reste des Exception, erreurs critique qui ferment l'application
                {
                    errorMessage = AxLanguage.Languages.REAplan_Erreur_Critique + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone;
                    AfficherErreurMessageBox(errorMessage);
                    LoggerFatalError(ex);
                    Singleton.CriticalError = true;
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Application.Current.Shutdown();
                    }), DispatcherPriority.Normal);
                }
            }
            catch (Exception exe)
            {

                GerrerErreur(exe);
            }
        }

        private static void LoggerFatalError(Exception ex)
        {
            logger.Fatal(ex.GetType().Name + " | " + ex.Message + " |" + ex.StackTrace);
        }
        private static void LoggerError(Exception ex)
        {
            logger.Error(ex.GetType().Name + " | " + ex.Message + " |" + ex.StackTrace);
        }
        private static void LoggerMauvaiseFermeture(Exception ex)
        {
            logger.Error(ex.GetType().Name + " |  Mauvaise Fermeutre de l'application");
        }
        private static void LoggerWarning(Exception ex)
        {
            logger.Warn(ex.GetType().Name + " | " + ex.Message + " |" + ex.StackTrace);
        }
        private static void LoggerRobotError(RobotException rex)
        {
            logger.Error(rex.GetType().Name + " | NodeId: " + rex.NodeId + " | Adresse: " + rex.Adresse + " | ErrorCode :" + rex.ErrorCode + " | " + rex.Message + " |" + rex.StackTrace);
        }
        private static void LoggerErrorEntity(Exception ex)
        {
            logger.Error(ex.GetType().Name + " | " + ex.Message + " |" + ex.InnerException.Message + "|"+ ex.StackTrace);
        }
        private static void AfficherErreurMessageBox(string msg)
        {
            MessageService.ShowError(msg);
        }
        private static void AfficherWarningMessageBox(string msg)
        {
            MessageService.ShowWarning(msg);
        }
        private static void TrouverSqlErreur(SqlException ex)
        {
            switch (ex.Number)
            {
                case 2 :
                case 53: 
                case 10054 :errorMessage = AxLanguage.Languages.REAplan_Erreur_Sql_Serveur;//erreur avec le serveur de db
                    break;
                case 2627: errorMessage = AxLanguage.Languages.REAplan_Erreur_Sql_Unique;//erreur sur une colone UNIQUE
                    break;
                case 4060: errorMessage = AxLanguage.Languages.REAplan_Erreur_Sql_Fichier;//erreur avec le fichier de db
                    break;
                case 18452 :
                case 18456: errorMessage = AxLanguage.Languages.REAplan_Erreur_Sql_Mdp;//mauvais mdp
                    break;
                default: errorMessage = AxLanguage.Languages.REAplan_Erreur_Sql_Defaut;
                    break;
            }
        }
        private static void MessageErreurRobot(RobotException rex)
        {
            
            switch (rex.ErrorCode)
            {
                case ErrorEmcyCodes.OvercurrentError: errorMessage = rex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n"+AxLanguage.Languages.REAplan_Erreur_ArretRobot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone;
                    break;
                case ErrorEmcyCodes.OvervoltageError: errorMessage = rex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Calibration;
                    break;
                case ErrorEmcyCodes.UndervoltageError: errorMessage = rex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Calibration;
                    break;
                case ErrorEmcyCodes.OvertemperatureError: errorMessage = rex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_ArretRobot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone;
                    break;
                case ErrorEmcyCodes.LogicSupplyVoltageTooLowError: errorMessage = rex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone;
                    break;
                case ErrorEmcyCodes.SupplyVoltageOutputStageTooLow: errorMessage = rex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone;
                    break;
                case ErrorEmcyCodes.InternalSoftwareError: errorMessage = rex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone;
                    break;
                case ErrorEmcyCodes.SoftwareParameterError: errorMessage = rex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone;
                    break;
                case ErrorEmcyCodes.PositionSensorError: errorMessage = rex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone;
                    break;
                case ErrorEmcyCodes.CanOverrunErrorObjectsLost: errorMessage = rex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Calibration;
                    break;
                case ErrorEmcyCodes.CanOverrunError: errorMessage = rex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Calibration;
                    break;
                case ErrorEmcyCodes.CanPassiveModeError: errorMessage = rex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Calibration;
                    break;
                case ErrorEmcyCodes.CanLifeGuardError: errorMessage = MessageCalibOuCall(rex);
                    break;
                case ErrorEmcyCodes.CanTransmitCobIdCollisionError: errorMessage = rex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Calibration;
                    break;
                case ErrorEmcyCodes.CanBusOffError: errorMessage = MessageCalibOuCall(rex);
                    break;
                case ErrorEmcyCodes.CanRxQueueOverflowError: errorMessage = rex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Calibration;
                    break;
                case ErrorEmcyCodes.CanTxQueueOverflowError: errorMessage = rex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Calibration;
                    break;
                case ErrorEmcyCodes.CanPdoLengthError: errorMessage = rex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Calibration;
                    break;
                case ErrorEmcyCodes.FollowingError: errorMessage = rex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Calibration;
                    break;
                case ErrorEmcyCodes.HallSensorError: errorMessage = rex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone;
                    break;
                case ErrorEmcyCodes.IndexProcessingError: errorMessage = rex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone;
                    break;
                case ErrorEmcyCodes.EncoderResolutionError: errorMessage = rex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone;
                    break;
                case ErrorEmcyCodes.HallSensorNotFoundError: errorMessage = rex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone;
                    break;
                case ErrorEmcyCodes.NegativeLimitSwitchError: errorMessage = MessageCalibOuCall(rex);
                    break;
                case ErrorEmcyCodes.PositiveLimitSwitchError: errorMessage = MessageCalibOuCall(rex);
                    break;
                case ErrorEmcyCodes.HallAngleDetectionError: errorMessage = rex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Calibration;
                    break;
                case ErrorEmcyCodes.SoftwarePositionLimitError: errorMessage = rex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Calibration;
                    break;
                case ErrorEmcyCodes.PositionSensorBreachError: errorMessage = MessageCalibOuCall(rex);
                    break;
                case ErrorEmcyCodes.SystemOverloadedError: errorMessage = rex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone;
                    break;
                case ErrorEmcyCodes.InterpolatedPositionModeError: errorMessage = rex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone;
                    break;
                case ErrorEmcyCodes.AutoTuningIdentificationError: errorMessage = rex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone;
                    break;
                case ErrorEmcyCodes.GearScalingFactorError: errorMessage = rex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone;
                    break;
                case ErrorEmcyCodes.ControllerGainError: errorMessage = rex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone;
                    break;
                case ErrorEmcyCodes.MainSensorDirectionError: errorMessage = rex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone;
                    break;
                case ErrorEmcyCodes.AuxiliarySensorDirectionError: errorMessage = rex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone;
                    break;
                default: errorMessage ="";
                    break;
            }
        }

        private static string MessageCalibOuCall(RobotException rex)
        {
            bool find = false;
            string month, day;
            DateTime dateJour = DateTime.Now;
            if (dateJour.Month.ToString().Length == 1)
                month = "0" + dateJour.Month.ToString();
            else
                month = dateJour.Month.ToString();
            if (dateJour.Day.ToString().Length == 1)
                day = "0" + dateJour.Day.ToString();
            else
                day = dateJour.Day.ToString();
            var line = File.ReadLines("../../../AxView/bin/Debug/log/Error.txt").SkipWhile(l => !l.Contains(dateJour.Year.ToString()+"-"+month+"-"+day));
            for (int i = 0; i < line.Count(); i++)
            {
                if (line.ElementAt(i).Contains(rex.ErrorCode.ToString()))
                    find = true;
            }
            if(find == false)
                return rex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Calibration;
            else
               return rex.Message + "\n" + AxLanguage.Languages.REAplan_Erreur_Robot + "\n" + AxLanguage.Languages.REAplan_Erreur_Call + "\n" + AxLanguage.Languages.REAplan_Erreur_NumeroTelephone;
        }
    }
}
