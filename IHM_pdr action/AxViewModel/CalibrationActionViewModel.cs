using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using AxModel;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Command;
using Navegar;
using AxCommunication;
using AxModel.Helpers;
using System.Xml.Linq;
using System.IO;
using AxAnalyse;
using AxAction;
using AxModelExercice;

namespace AxViewModel
{
    public class CalibrationActionViewModel : ViewModelBase
    {
        #region Field
        // Services
        private IUnityGameController gameController;
        private INavigation nav;
        private ActionRobot pss;

        private bool isPlayingAmplitude = false;
        private bool isPlayingAmplitudeArticualire = false;
        private bool isPlayingAmplitudeArticualireAide = false;
        private bool isPlayingForce = false;

        private double amplitudeMax = 0.0;
        private double amplitudePixel = 0.0;
        private double forceMax = 0.0;
        private Singleton singleton = Singleton.getInstance();
        private string pathPatient = string.Empty;
        private const byte VISCMIN = 15;
        private List<int> cibleAmplitudeArticulaire = new List<int>();
        private List<int> cibleAmplitudeArticulaireAide = new List<int>();
        #endregion

        #region Command
        public RelayCommand StartAmplitudeCommand { get; set; }
        public RelayCommand StopAmplitudeCommand { get; set; }
        public RelayCommand StartAmplitudeArticulaireCommand { get; set; }
        public RelayCommand StopAmplitudeArticulaireCommand { get; set; }
        public RelayCommand StartAmplitudeArticulaireAideCommand { get; set; }
        public RelayCommand StopAmplitudeArticulaireAideCommand { get; set; }
        public RelayCommand StartForceCommand { get; set; }
        public RelayCommand StopForceCommand { get; set; }
        public RelayCommand HomeViewModelCommand { get; set; }
        #endregion

        public CalibrationActionViewModel()
        {
            this.gameController = SimpleIoc.Default.GetInstance<IUnityGameController>();
            this.nav = SimpleIoc.Default.GetInstance<INavigation>();
            this.pss = SimpleIoc.Default.GetInstance<ActionRobot>();

            // Commands
            this.StartAmplitudeCommand = new RelayCommand(this.StartAmplitude, this.StartAmplitude_CanExecute);
            this.StopAmplitudeCommand = new RelayCommand(this.StopAmplitude, this.StopAmplitude_CanExecute);

            this.StartAmplitudeArticulaireCommand = new RelayCommand(this.StartAmplitudeArticulaire,this.StartAmplitudeArticulaire_CanExecute);
            this.StopAmplitudeArticulaireCommand = new RelayCommand(this.StopAmplitudeArticulaire, this.StopAmplitudeArticulaire_CanExecute);

            this.StartAmplitudeArticulaireAideCommand = new RelayCommand(this.StartAmplitudeArticulaireAide, this.StartAmplitudeArticulaireAide_CanExecute);
            this.StopAmplitudeArticulaireAideCommand = new RelayCommand(this.StopAmplitudeArticulaireAide, this.StopAmplitudeArticulaireAide_CanExecute);

            this.StartForceCommand = new RelayCommand(this.StartForce, this.StartForce_CanExecute);
            this.StopForceCommand = new RelayCommand(this.StopForce, this.StopForce_CanExecute);
            this.HomeViewModelCommand = new RelayCommand(this.GoHome);

            this.pathPatient = "Files/Patients/" + singleton.Patient.Nom + singleton.Patient.Prenom + singleton.Patient.DateDeNaissance.ToString().Replace("/", string.Empty);

        }

        #region Methodes
        private void StartAmplitude()
        {
            this.isPlayingAmplitude = true;
            // Level 0 = amplitude
            this.gameController.StartGame("pdr action", 0, 0, 0);
            this.gameController.onLevelStarted += new AxReaLabToUnity.NewLevelStartedHandler(gameController_onLevelStarted);
            this.pss.Pss.SendCommandFrame(CommandCodes.mod_home);
            this.ListenPortSerie();

            this.NewVisco(VISCMIN);
        }

        void gameController_onLevelStarted(object obj, EventArgs messageArgs)
        {
            if (this.isPlayingAmplitude == true || this.isPlayingAmplitudeArticualire == true || this.isPlayingAmplitudeArticualireAide == true)
            {
                this.pss.Pss.SendCommandFrame(CommandCodes.mode_libre_visc);
            }
        }

        private void StopAmplitude()
        {
            this.pss.SendCommandFrame(CommandCodes.STOPnv);
            this.isPlayingAmplitude = false;
            this.UnListenPortSerie();
            this.gameController.onLevelStarted -= new AxReaLabToUnity.NewLevelStartedHandler(gameController_onLevelStarted);
            // Sauvegarde de l'amplitude max et amplitude 60%
            XDocument doc;

            if (File.Exists(this.pathPatient + "/InfoPatient.xml"))
            {
                doc = XDocument.Load(this.pathPatient + "/InfoPatient.xml");
            }
            else
            {
                if (!Directory.Exists(this.pathPatient))
                {
                    Directory.CreateDirectory(this.pathPatient);
                }

                doc = new XDocument(
                new XDeclaration("1.0", "UTF-16", null),
                new XElement("Calibration_Settings",
                    new XElement("AmplitudeMax"),
                    new XElement("Amplitude60"),
                    new XElement("CibleAmpliArt"),
                    new XElement("CibleAmpliArtAide"),
                    new XElement("ForceMax"),
                    new XElement("Force75")));
            }

            doc.Root.Element("AmplitudeMax").SetValue(Math.Round(this.amplitudeMax, 3));
            doc.Root.Element("Amplitude60").SetValue(Math.Round((this.amplitudeMax * 0.4) + this.amplitudeMax, 3));
            doc.Save(this.pathPatient + "/InfoPatient.xml");

            // Envoie de l'amplitude 60% au jeu
            // TEMPORAIRE SUR REA2PLAN car monter décremente les coordonnées.
            amplitudePixel = (int)EchelleUtils.MiseEchelleYPosition(((this.amplitudeMax * 100) * 0.4) + (this.amplitudeMax * 100)); // *100 car on divise par 100 dans la réception
            //int amplitudePixel = (int)EchelleUtils.MiseEchelleYPosition((this.amplitudeMax * 100));

            //int amplitudePixel = (int)EchelleUtils.MiseEchelleYPosition(((this.amplitudeMax * 100) * 0.6)); // *100 car on divise par 100 dans la réception

            this.gameController.StopGame();

            // Reset de la viscosité pour être sur qu'elle ne soit pas gardé d'un exercice à l'autre
            this.NewVisco(50);
        }

        private void StartAmplitudeArticulaire()
        {
            this.isPlayingAmplitudeArticualire = true;
            // Level 4 = amplitude articulaire
            this.gameController.StartGame("pdr action", 0, 0, 4);
            this.gameController.onLevelStarted += new AxReaLabToUnity.NewLevelStartedHandler(gameController_onLevelStarted);
            this.gameController.onCheckpointReached += new AxReaLabToUnity.NewCheckpointReachedHandler(gameController_onCheckpointReached);
            this.pss.Pss.SendCommandFrame(CommandCodes.mod_home);
            this.ListenPortSerie();

            this.NewVisco(VISCMIN);
        }

        void gameController_onCheckpointReached(object obj, AxReaLabToUnity.Models.MessageEvent messageArgs)
        {
            if (this.isPlayingAmplitudeArticualire == true)
            {
                this.cibleAmplitudeArticulaire.Add(messageArgs.Data);
            }
            else if (this.isPlayingAmplitudeArticualireAide == true)
            {
                this.cibleAmplitudeArticulaireAide.Add(messageArgs.Data);
            }
        }

        private void StopAmplitudeArticulaire()
        {
            this.pss.SendCommandFrame(CommandCodes.STOPnv);
            this.isPlayingAmplitudeArticualire = false;
            this.UnListenPortSerie();
            this.gameController.onLevelStarted -= new AxReaLabToUnity.NewLevelStartedHandler(gameController_onLevelStarted);
            this.gameController.onCheckpointReached -= new AxReaLabToUnity.NewCheckpointReachedHandler(gameController_onCheckpointReached);

            // Sauvegarde des cibles attrapéess en amplitude articulaire
            XDocument doc;

            if (File.Exists(this.pathPatient + "/InfoPatient.xml"))
            {
                doc = XDocument.Load(this.pathPatient + "/InfoPatient.xml");

                // Création du string à enregistrer
                string cibles = string.Empty;

                if (this.cibleAmplitudeArticulaire.Count > 0)
                {
                    foreach (var cible in this.cibleAmplitudeArticulaire)
                    {
                        cibles += cible;
                        if (cible != this.cibleAmplitudeArticulaire.Last())
                            cibles += ",";
                    }
                }

                doc.Root.Element("CibleAmpliArt").SetValue(cibles);
                doc.Save(this.pathPatient + "/InfoPatient.xml");
            }

            this.gameController.StopGame();

            // Reset de la viscosité pour être sur qu'elle ne soit pas gardé d'un exercice à l'autre
            this.NewVisco(50);
        }

        private void StartAmplitudeArticulaireAide()
        {
            this.isPlayingAmplitudeArticualireAide = true;
            // Level 4 = amplitude articulaire
            this.gameController.StartGame("pdr action", 0, 0, 4);
            this.gameController.onLevelStarted += new AxReaLabToUnity.NewLevelStartedHandler(gameController_onLevelStarted);
            this.gameController.onCheckpointReached += new AxReaLabToUnity.NewCheckpointReachedHandler(gameController_onCheckpointReached);
            this.pss.Pss.SendCommandFrame(CommandCodes.mod_home);
            this.ListenPortSerie();

            this.NewVisco(VISCMIN);
        }

        private void StopAmplitudeArticulaireAide()
        {
            this.pss.SendCommandFrame(CommandCodes.STOPnv);
            this.isPlayingAmplitudeArticualireAide = false;
            this.UnListenPortSerie();
            this.gameController.onLevelStarted -= new AxReaLabToUnity.NewLevelStartedHandler(gameController_onLevelStarted);
            this.gameController.onCheckpointReached -= new AxReaLabToUnity.NewCheckpointReachedHandler(gameController_onCheckpointReached);

            // Sauvegarde des cibles attrapéess en amplitude articulaire
            XDocument doc;

            if (File.Exists(this.pathPatient + "/InfoPatient.xml"))
            {
                doc = XDocument.Load(this.pathPatient + "/InfoPatient.xml");

                // Création du string à enregistrer
                string cibles = string.Empty;

                if (this.cibleAmplitudeArticulaireAide.Count > 0)
                {
                    foreach (var cible in this.cibleAmplitudeArticulaireAide)
                    {
                        cibles += cible;
                        if (cible != this.cibleAmplitudeArticulaireAide.Last())
                            cibles += ",";
                    }
                }

                doc.Root.Element("CibleAmpliArtAide").SetValue(cibles);
                doc.Save(this.pathPatient + "/InfoPatient.xml");
            }

            this.gameController.StopGame();

            // Reset de la viscosité pour être sur qu'elle ne soit pas gardé d'un exercice à l'autre
            this.NewVisco(50);
        }

        private void StartForce()
        {
            this.isPlayingForce = true;
            // Level 1 = force
            // Envoie le robot a 60% de l'amplitude max x=4260
            FrameConfigDataModel _configFrame = new FrameConfigDataModel();
            _configFrame.Address = ConfigAddresses.mod_pos;
            _configFrame.Data1_2 = 4260;
            _configFrame.Data3_4 = (short)(EchelleUtils.MiseEchelleEnvoyerY(amplitudePixel) * 100);
            this.pss.Pss.SendConfigFrame(_configFrame);

            this.gameController.StartGame("pdr action", 0, 0, 1);
            this.gameController.onNewTrajectory += new AxReaLabToUnity.NewTrajectoryHandler(gameController_onNewTrajectory);
            this.ListenPortSerie();
        }

        void gameController_onNewTrajectory(object obj, AxReaLabToUnity.Models.PathEvent trajectoryArgs)
        {
            // Astuce pour envoyer au bon moment le point au demarrage du jeu
            this.gameController.SetValue("AmplitudeY", (int)amplitudePixel);
        }

        private void StopForce()
        {
            this.pss.SendCommandFrame(CommandCodes.STOPnv);
            this.isPlayingForce = false;
            this.UnListenPortSerie();

            // Sauvegarde de la force max et force 75%
            XDocument doc;

            if (File.Exists(this.pathPatient + "/InfoPatient.xml"))
            {
                doc = XDocument.Load(this.pathPatient + "/InfoPatient.xml");

                doc.Root.Element("ForceMax").SetValue(Math.Round(this.forceMax, 3));
                doc.Root.Element("Force75").SetValue(Math.Round((this.forceMax * 0.75), 3));
                doc.Save(this.pathPatient + "/InfoPatient.xml");
            }

            // Arret du jeu
            this.gameController.StopGame();

            // Reset de la viscosité pour être sur qu'elle ne soit pas gardé d'un exercice à l'autre
            this.NewVisco(50);
        }

        private void NewVisco(byte visc)
        {
            FrameExerciceDataModel newConfig = new FrameExerciceDataModel();
            newConfig.Address = ConfigAddresses.MasseVisco;
            newConfig.Data1 = 0;
            newConfig.Data2 = 15;
            newConfig.Data3 = 0;
            newConfig.Data4 = visc;
            this.pss.Pss.SendExerciceFrame(newConfig);
        }
        #endregion

        #region PortSerie
        private void ListenPortSerie()
        {
            this.pss.Pss.PositionDataReceived += new onPositionDataReceived(pss_PositionDataReceived);

            if(this.isPlayingForce == true)
                this.pss.Pss.ForceDataReceived += new onForceDataReceived(pss_ForceDataReceived);
        }

        private void pss_PositionDataReceived(object sender, PositionDataModel e)
        {
            var pixelX = EchelleUtils.MiseEchelleXPosition(e.PositionX);
            var pixelY = EchelleUtils.MiseEchelleYPosition(e.PositionY);
            this.gameController.SetPositions(pixelX, pixelY);

            if (this.isPlayingAmplitude == true)
            {
                if (e.PositionYd > this.amplitudeMax)
                {
                    this.amplitudeMax = e.PositionYd / 100.0;
                }
            }
        }

        private void pss_ForceDataReceived(object sender, ForceDataModel e)
        {
            this.gameController.SetForces(e.ForceX, e.ForceY);

            var newForce = Ax_Generique.Pythagorean(e.ForceX, e.ForceY);

            if (newForce > this.forceMax)
            {
                this.forceMax = newForce;
            }
        }

        private void UnListenPortSerie()
        {
            this.pss.Pss.PositionDataReceived -= new onPositionDataReceived(pss_PositionDataReceived);
            this.pss.Pss.ForceDataReceived -= new onForceDataReceived(pss_ForceDataReceived);
        }
        #endregion

        #region CanExecute
        private bool StartAmplitude_CanExecute()
        {
            if (this.isPlayingAmplitude == false && isPlayingForce == false)
                return true;
            else
                return false;
        }

        private bool StopAmplitude_CanExecute()
        {
            if (this.isPlayingAmplitude == true && isPlayingForce == false)
                return true;
            else
                return false;
        }

        private bool StartAmplitudeArticulaire_CanExecute()
        {
            if (this.isPlayingAmplitude == false && isPlayingForce == false && isPlayingAmplitudeArticualireAide == false && isPlayingAmplitudeArticualire == false && this.amplitudeMax != 0.0)
                return true;
            else
                return false;
        }

        private bool StopAmplitudeArticulaire_CanExecute()
        {
            if (this.isPlayingAmplitude == false && isPlayingForce == false && isPlayingAmplitudeArticualire == true && isPlayingAmplitudeArticualireAide == false)
                return true;
            else
                return false;
        }

        private bool StartAmplitudeArticulaireAide_CanExecute()
        {
            if (this.isPlayingAmplitude == false && isPlayingForce == false && isPlayingAmplitudeArticualire == false && isPlayingAmplitudeArticualireAide == false && this.amplitudeMax != 0.0 && this.cibleAmplitudeArticulaire.Count > 0)
                return true;
            else
                return false;
        }

        private bool StopAmplitudeArticulaireAide_CanExecute()
        {
            if (this.isPlayingAmplitude == false && isPlayingForce == false && isPlayingAmplitudeArticualire == false && isPlayingAmplitudeArticualireAide == true)
                return true;
            else
                return false;
        }

        private bool StartForce_CanExecute()
        {
            if (this.isPlayingAmplitude == false && isPlayingForce == false && isPlayingAmplitudeArticualire == false && isPlayingAmplitudeArticualire == false && isPlayingAmplitudeArticualireAide == false && this.amplitudeMax != 0.0 && this.cibleAmplitudeArticulaire.Count > 0 && this.cibleAmplitudeArticulaireAide.Count > 0)
                return true;
            else
                return false;
        }

        private bool StopForce_CanExecute()
        {
            if (this.isPlayingAmplitude == false && isPlayingForce == true && isPlayingAmplitudeArticualire == false && isPlayingAmplitudeArticualireAide == false)
                return true;
            else
                return false;
        }
        #endregion

        #region Navigation
        /// <summary>
        /// Retour à l'accueil
        /// </summary>
        private void GoHome()
        {
            this.gameController.StopGame();
            nav.NavigateTo<HomeViewModel>("SetIsRetour", new object[] { true });
        }
        #endregion 
    }
}
