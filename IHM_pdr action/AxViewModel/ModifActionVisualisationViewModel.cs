using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using AxModel;
using Navegar;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using AxAction;
using AxModel.Helpers;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace AxViewModel
{
    public class ModifActionVisualisationViewModel : ViewModelBase
    {
        #region Field
        // Services
        private INavigation nav;
        private Singleton singleton = Singleton.getInstance();
        private IUnityGameController gameController;
        private ActionRobot pss;

        private string exerciceName;
        private bool isPlaying = false;
        private bool isTraining = false;
        private string currentCible;
        private string maxCible;
        private bool pause = false;
        private bool isGoingHome = false;
        private const byte VISCMIN = 15;
        private List<PositionDataModel> listPosition = new List<PositionDataModel>();
        private List<ForceDataModel> listForce = new List<ForceDataModel>();
        private List<short> listKlat = new List<short>();
        private List<short> listClong = new List<short>();
        private bool canDoMath = false;
        private string pathPatient = string.Empty;
        private int[] tabRepetition = new int[30];
        private List<System.Windows.Point> cibles = new List<System.Windows.Point>();
        private bool exitInitZone = false;
        private System.Timers.Timer exitTimer;
        private double exitTime;
        private AxAssistance assistance;
        private bool canUseAssistance = false;
        #endregion

        #region Command
        public RelayCommand HomeViewModelCommand { get; set; }
        public RelayCommand PreviousViewModelCommand { get; set; }
        public RelayCommand StartCommand { get; set; }
        public RelayCommand StartEntrainementCommand { get; set; }
        public RelayCommand StopCommand { get; set; }
        public RelayCommand PauseCommand { get; set; }
        #endregion

        public ModifActionVisualisationViewModel()
        {
            // Service
            this.nav = SimpleIoc.Default.GetInstance<INavigation>();
            this.gameController = SimpleIoc.Default.GetInstance<IUnityGameController>();
            this.pss = SimpleIoc.Default.GetInstance<ActionRobot>();

            // Commands
            this.HomeViewModelCommand = new RelayCommand(this.GoHome);
            this.PreviousViewModelCommand = new RelayCommand(this.GoBack);
            this.StartCommand = new RelayCommand(this.Start, this.StartCommand_CanExecute);
            this.StartEntrainementCommand = new RelayCommand(this.StartEntrainement, this.StartCommand_CanExecute);
            this.StopCommand = new RelayCommand(this.Stop, this.StopCommand_CanExecute);
            this.PauseCommand = new RelayCommand(this.Pause, this.PauseCommand_CanExecute);

            this.ExerciceName = this.GetExerciceName();
            this.InitCompteur();
            this.InitCibleCoord();

            this.exitTimer = new System.Timers.Timer();
            this.exitTimer.AutoReset = true;
            this.exitTimer.Interval = 10;
            this.exitTimer.Elapsed += new System.Timers.ElapsedEventHandler(exitTimer_Elapsed);
        }

        #region Property
        public string ExerciceName
        {
            get { return exerciceName; }
            set 
            {
                exerciceName = value;
                RaisePropertyChanged("ExerciceName");
            }
        }
        public string CurrentCible
        {
            get { return currentCible; }
            set 
            {
                currentCible = value;
                RaisePropertyChanged("CurrentCible");
            }
        }
        public string MaxCible
        {
            get { return maxCible; }
            set 
            { 
                maxCible = value;
                RaisePropertyChanged("MaxCible");
            }
        }
        #endregion

        #region Methodes
        private void InitCibleCoord()
        {
            cibles.Add(new System.Windows.Point(192.0, 1008.0));    // 1
            cibles.Add(new System.Windows.Point(192.0, 864.0));     // 2
            cibles.Add(new System.Windows.Point(192.0, 720.0));     // 3
            cibles.Add(new System.Windows.Point(192.0, 576.0));     // 4
            cibles.Add(new System.Windows.Point(192.0, 432.0));     // 5
            cibles.Add(new System.Windows.Point(192.0, 288.0));     // 6
            cibles.Add(new System.Windows.Point(576.0, 1008.0));    // 7
            cibles.Add(new System.Windows.Point(576.0, 864.0));     // 8
            cibles.Add(new System.Windows.Point(576.0, 720.0));     // 9
            cibles.Add(new System.Windows.Point(576.0, 576.0));     // 10
            cibles.Add(new System.Windows.Point(576.0, 432.0));     // 11
            cibles.Add(new System.Windows.Point(576.0, 288.0));     // 12
            cibles.Add(new System.Windows.Point(960.0, 1008.0));    // 13
            cibles.Add(new System.Windows.Point(960.0, 864.0));     // 14
            cibles.Add(new System.Windows.Point(960.0, 720.0));     // 15
            cibles.Add(new System.Windows.Point(960.0, 576.0));     // 16
            cibles.Add(new System.Windows.Point(960.0, 432.0));     // 17
            cibles.Add(new System.Windows.Point(960.0, 288.0));     // 18
            cibles.Add(new System.Windows.Point(1344.0, 1008.0));   // 19
            cibles.Add(new System.Windows.Point(1344.0, 864.0));    // 20
            cibles.Add(new System.Windows.Point(1344.0, 720.0));    // 21
            cibles.Add(new System.Windows.Point(1344.0, 576.0));    // 22
            cibles.Add(new System.Windows.Point(1344.0, 432.0));    // 23
            cibles.Add(new System.Windows.Point(1344.0, 288.0));    // 24
            cibles.Add(new System.Windows.Point(1728.0, 1008.0));   // 25
            cibles.Add(new System.Windows.Point(1728.0, 864.0));    // 26
            cibles.Add(new System.Windows.Point(1728.0, 720.0));    // 27
            cibles.Add(new System.Windows.Point(1728.0, 576.0));    // 28
            cibles.Add(new System.Windows.Point(1728.0, 432.0));    // 29
            cibles.Add(new System.Windows.Point(1728.0, 288.0));    // 30
        }

        private string GetExerciceName()
        {
            string name = string.Empty;

            switch (Singleton.TypeAction)
            {
                case PdrActionType.InhibitionHemicorps:
                    name = "Inhibition Hémicorps";
                    break;
                case PdrActionType.InhibitionHemichamp:
                    name = "Inhibition Hémichamp";
                    break;
                case PdrActionType.FacilitationAction:
                    name = "Facilitation Action";
                    break;
                case PdrActionType.PasDeModificationAction:
                    name = "Pas de modification d'action";
                    break;
                case PdrActionType.EgaliteLaterale:
                    name = "Egalité Latérale";
                    break;
                case PdrActionType.EgaliteLateraleForce:
                    name = "Egalité Latérale Force";
                    break;
                case PdrActionType.BiaisLateral:
                    name = "Biais Latéral";
                    break;
                case PdrActionType.BiaisLateralForce:
                    name = "Biais Latéral Force";
                    break;
                case PdrActionType.FacilitationLaterale:
                    name = "Facilitation Latérale";
                    break;
                case PdrActionType.BiaisLateralTemps:
                    name = "Biais Latéral Temps";
                    break;
                case PdrActionType.EgaliteLateraleTemps:
                    name = "Egalité Latérale Temps";
                    break;
                default:
                    name = "ERROR";
                    break;
            }

            return name;
        }

        private void InitCompteur()
        {
            this.MaxCible = "XXX";
            this.CurrentCible = "XXX";
        }

        private void Start()
        {
            isPlaying = true;
            int lvl = 2;
            if (Singleton.TypeAction == PdrActionType.EgaliteLateraleForce || Singleton.TypeAction == PdrActionType.BiaisLateralForce)
            {
                lvl = 3;
            }
            else if (Singleton.TypeAction == PdrActionType.EgaliteLateraleTemps || Singleton.TypeAction == PdrActionType.BiaisLateralTemps)
            {
                lvl = 5;
            }
            if (isTraining == false)
            {
                this.gameController.StartGame("pdr action", 0, 0, lvl);
            }
            else
            {
                this.gameController.StartGame("pdr action", 1, 0, lvl);
            }
            this.gameController.onNewTrajectory += new AxReaLabToUnity.NewTrajectoryHandler(gameController_onNewTrajectory);
            this.gameController.onLevelStarted += new AxReaLabToUnity.NewLevelStartedHandler(gameController_onLevelStarted);
            this.gameController.onLevelStopped += new AxReaLabToUnity.NewLevelStoppedHandler(gameController_onLevelStopped);
            this.gameController.onCheckpointReached += new AxReaLabToUnity.NewCheckpointReachedHandler(gameController_onCheckpointReached);
            this.gameController.onNewValue += new AxReaLabToUnity.NewValueHandler(gameController_onNewValue);

            this.pss.Pss.PositionDataReceived += new AxCommunication.onPositionDataReceived(Pss_PositionDataReceived);
            this.pss.Pss.ForceDataReceived += new AxCommunication.onForceDataReceived(Pss_ForceDataReceived);

            // Positionne le robot au départ
            this.pss.Pss.SendCommandFrame(CommandCodes.mod_home);

            if (isTraining == false)
            {
                // Path complèt de ou devront se trouver les données sauvegardées.
                this.pathPatient = "Files/Patients/" + singleton.Patient.Nom + singleton.Patient.Prenom + singleton.Patient.DateDeNaissance.ToString().Replace("/", string.Empty);
                var option = (Singleton.TypeAction == PdrActionType.EgaliteLaterale || Singleton.TypeAction == PdrActionType.EgaliteLateraleForce || Singleton.TypeAction == PdrActionType.EgaliteLateraleTemps) ? PdrOptions.PasOptions.ToString() : Singleton.Options.ToString();
                this.pathPatient += "/" + Singleton.TypeAction.ToString() + " - " + Singleton.Zoom.ToString() + " - " + option + "/";
                this.CreatPatientDirectory();
            }
        }

        void gameController_onLevelStopped(object obj, EventArgs messageArgs)
        {
            if (Singleton.TypeAction == PdrActionType.FacilitationAction || Singleton.TypeAction == PdrActionType.FacilitationLaterale)
            {
                this.pss.Pss.SendCommandFrame(CommandCodes.mod_home);
            }
        }

        private void StartEntrainement()
        {
            isTraining = true;
            Start();
        }

        void gameController_onNewValue(object obj, AxReaLabToUnity.Models.ValueEvent valueArgs)
        {
            if (valueArgs.Key == "InitTime")
            {
                this.exitInitZone = true;
            }

            if (valueArgs.Key == "TargetY")
            {
                this.InitTraj();
            }
        }
        
        // Le niveau est correctement chargé et attends les cibles et le répétitions
        void gameController_onNewTrajectory(object obj, AxReaLabToUnity.Models.PathEvent trajectoryArgs)
        {
            if (isTraining == false)
            {
                this.gameController.SetValue("RepetitionGauche", Singleton.RepetitionGauche);
                this.gameController.SetValue("RepetitionDroit", Singleton.RepetitionDroit);
                this.gameController.SetValues("CiblesGauche", Singleton.CiblesGauche.ToArray());
                this.gameController.SetValues("CiblesDroite", Singleton.CiblesDroite.ToArray());

                if (Singleton.TypeAction == PdrActionType.BiaisLateralTemps || Singleton.TypeAction == PdrActionType.EgaliteLateraleTemps)
                {
                    this.gameController.SetValue("InitTimer", Singleton.InitTime);
                    this.gameController.SetValue("GeneralTimer", Singleton.GeneralTime);
                    this.gameController.SetValue("OutsideTimer", Singleton.OutsideTime);
                }

                this.MaxCible = ((Singleton.RepetitionGauche * Singleton.CiblesGauche.Count) + (Singleton.RepetitionDroit * Singleton.CiblesDroite.Count)).ToString();
            }
            else
            {
                this.gameController.SetValue("RepetitionGauche", 1);
                this.gameController.SetValue("RepetitionDroit",1);
                this.gameController.SetValues("CiblesGauche", new int[] {4,11,6, 5});
                this.gameController.SetValues("CiblesDroite", new int[] { 28, 23, 30, 24 });

                if (Singleton.TypeAction == PdrActionType.BiaisLateralTemps || Singleton.TypeAction == PdrActionType.EgaliteLateraleTemps)
                {
                    this.gameController.SetValue("InitTimer", Singleton.InitTime);
                    this.gameController.SetValue("GeneralTimer", Singleton.GeneralTime);
                    this.gameController.SetValue("OutsideTimer", Singleton.OutsideTime);
                }

                this.MaxCible = "6";
            }

            this.CurrentCible = "0";
        }

        void gameController_onLevelStarted(object obj, EventArgs messageArgs)
        {
            byte viscToSend = VISCMIN;
            if (Singleton.TypeAction == PdrActionType.InhibitionHemichamp)
            {
                bool gauche = Convert.ToBoolean(gameController.GetValue("Gauche"));
                
                if ((Singleton.Options == PdrOptions.Option1 && gauche == true) || (Singleton.Options == PdrOptions.Option2 && gauche == false))
                {
                    viscToSend = (byte)Singleton.Viscosite;
                }
            }
            else if (Singleton.TypeAction == PdrActionType.InhibitionHemicorps)
            {
                bool avantPause = Convert.ToBoolean(gameController.GetValue("AvantPause"));
                if ((Singleton.Options == PdrOptions.Option1 && avantPause == false) || (Singleton.Options == PdrOptions.Option2 && avantPause == true))
                {
                    viscToSend = (byte)Singleton.Viscosite;
                }
            }

            this.NewVisco(viscToSend);

            if (Singleton.TypeAction == PdrActionType.FacilitationAction || Singleton.TypeAction == PdrActionType.FacilitationLaterale)
            {
                pss.Pss.SendCommandFrame(CommandCodes.mod_suiv_traj);
                this.exitTime = 600;
                this.exitTimer.Start();
            }
            else if (Singleton.TypeAction == PdrActionType.EgaliteLateraleForce || Singleton.TypeAction == PdrActionType.BiaisLateralForce)
            {
                // ne rien faire, le robot reste bloqué au début.
            }
            else
            {
                this.pss.Pss.SendCommandFrame(CommandCodes.mode_libre_visc);
            }
            this.isGoingHome = false;

            if (isTraining == false)
            {
                canDoMath = true;
            }
        }

        void exitTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (this.exitInitZone == true || this.exitTime <= 0)
            {
                // Sorti de la zone
                this.exitTimer.Stop();

                // Lancer l'assistance
                this.assistance = new AxAssistance();
                canUseAssistance = true;
                Debug.Print("Activation de l'assistance"); 
            }
            else
            {
                this.exitTime--;
            }
        }

        void gameController_onCheckpointReached(object obj, AxReaLabToUnity.Models.MessageEvent messageArgs)
        {
            int currentCibleInt = Convert.ToInt32(this.CurrentCible) + 1;
            this.CurrentCible = currentCibleInt.ToString();

            this.pss.Pss.SendCommandFrame(CommandCodes.STOPnv);
            if (Singleton.TypeAction != PdrActionType.FacilitationAction && Singleton.TypeAction != PdrActionType.FacilitationLaterale && Singleton.TypeAction != PdrActionType.EgaliteLateraleForce && Singleton.TypeAction != PdrActionType.BiaisLateralForce)
            {
                this.pss.Pss.SendCommandFrame(CommandCodes.mod_home);
            }

            this.isGoingHome = true;
            this.exitInitZone = false;
            canUseAssistance = false;

            if (isTraining == false)
            {
                canDoMath = false;
                
                // Augementation du nombre de répétition pour la cible qui vient d'être faite
                this.tabRepetition[messageArgs.Data - 1]++; // -1 car le numero des cibles est de 1 à 30 alors que les indices du tab vont de 0 à 29

                Thread mathThread = new Thread(new ThreadStart(() => 
                {
                    // Enregistrement des données brutes pour la cible
                    this.SaveRawData(this.listPosition, this.listForce, this.listKlat, this.listClong ,"Cible_" + messageArgs.Data + "." + this.tabRepetition[messageArgs.Data - 1].ToString());

                    // Calcul des données cinématique pour le mouvement qui vient de se finir
                    if (Singleton.TypeAction != PdrActionType.EgaliteLateraleForce && Singleton.TypeAction != PdrActionType.BiaisLateralForce)
                    {
                        this.CalculResultatCinematique(messageArgs.Data);
                    }
                    else
                    {
                        this.CalculResultatForce(messageArgs.Data);
                    }

                    this.listPosition.Clear();
                    this.listForce.Clear();
                    this.listKlat.Clear();
                    this.listClong.Clear();
                }));
                mathThread.Start();
            }
        }

        private void InitTraj()
        {
            FrameConfigDataModel _configVitesseFrame = new FrameConfigDataModel();
            _configVitesseFrame.Address = ConfigAddresses.VitesseNbrsrep;
            _configVitesseFrame.Data1_2 = 0;
            _configVitesseFrame.Data3_4 = 0;
            this.pss.Pss.SendConfigFrame(_configVitesseFrame);

            FrameConfigDataModel _configAssistFrame = new FrameConfigDataModel();
            _configAssistFrame.Address = ConfigAddresses.KlatClong;
            _configAssistFrame.Data1_2 = 1;
            _configVitesseFrame.Data3_4 = 1;
            this.pss.Pss.SendConfigFrame(_configAssistFrame);

            // Init traj
            double xPixel = 1920 - Convert.ToDouble(this.gameController.GetValue("TargetX"));
            double yPixel = 1080 - Convert.ToDouble(this.gameController.GetValue("TargetY"));

            FrameConfigDataModel _configFrame = new FrameConfigDataModel();
            _configFrame.Address = ConfigAddresses.mod_hemi_pdr;
            _configFrame.Data1_2 = (short)(EchelleUtils.MiseEchelleEnvoyerX(xPixel) * 100);
            _configFrame.Data3_4 = (short)(EchelleUtils.MiseEchelleEnvoyerY(yPixel) * 100);
            this.pss.Pss.SendConfigFrame(_configFrame);
        }

        void Pss_PositionDataReceived(object sender, PositionDataModel e)
        {
            var pixelX = EchelleUtils.MiseEchelleXPosition(e.PositionX);
            var pixelY = EchelleUtils.MiseEchelleYPosition(e.PositionY);
            this.gameController.SetPositions(pixelX, pixelY);

            if (canDoMath == true)
            {
                this.listPosition.Add(e);
            }
            if (canUseAssistance == true)
            {
                this.assistance.AssistanceLoop(e.PositionX / 100, e.PositionY / 100);
                this.listKlat.Add(assistance.CurrentConfig.RaideurLat);
                this.listClong.Add(assistance.CurrentConfig.RaideurLong);
            }
        }

        void Pss_ForceDataReceived(object sender, ForceDataModel e)
        {
            if (canDoMath == true)
            {
                this.listForce.Add(e);
            }

            // Envoyer les forces uniquement pour les exercices de force
            if (Singleton.TypeAction == PdrActionType.EgaliteLateraleForce || Singleton.TypeAction == PdrActionType.BiaisLateralForce)
            {
                this.gameController.SetForces(-1 * e.ForceX, -1 * e.ForceY);
            }
        }

        private void Stop()
        {
            isPlaying = false;
            isTraining = false;
            canDoMath = false;
            canUseAssistance = false;
            exitInitZone = false;
            if (exitTimer.Enabled)
            {
                exitTimer.Stop();
            }
            this.InitCompteur();

            this.gameController.StopGame();
            this.gameController.onNewTrajectory -= new AxReaLabToUnity.NewTrajectoryHandler(gameController_onNewTrajectory);
            this.gameController.onLevelStarted -= new AxReaLabToUnity.NewLevelStartedHandler(gameController_onLevelStarted);
            this.gameController.onLevelStopped -= new AxReaLabToUnity.NewLevelStoppedHandler(gameController_onLevelStopped);
            this.gameController.onCheckpointReached -= new AxReaLabToUnity.NewCheckpointReachedHandler(gameController_onCheckpointReached);
            this.gameController.onNewValue -= new AxReaLabToUnity.NewValueHandler(gameController_onNewValue);

            this.pss.Pss.PositionDataReceived -= new AxCommunication.onPositionDataReceived(Pss_PositionDataReceived);
            this.pss.Pss.ForceDataReceived -= new AxCommunication.onForceDataReceived(Pss_ForceDataReceived);
            this.pss.Pss.SendCommandFrame(CommandCodes.STOPnv);

            // Reset de la viscosité pour être sur qu'elle ne soit pas gardé d'un exercice à l'autre
            this.NewVisco(50);
        }

        private void Pause()
        {
            pause = !pause;

            if (pause == true)
            {
                this.pss.Pss.SendCommandFrame(CommandCodes.STOPnv);
                canDoMath = false;
                canUseAssistance = false;
            }
            else
            {
                if (this.isGoingHome == false)
                {
                    if (Singleton.TypeAction == PdrActionType.FacilitationAction || Singleton.TypeAction == PdrActionType.FacilitationLaterale)
                    {
                        canUseAssistance = true;
                        this.pss.Pss.SendCommandFrame(CommandCodes.mod_suiv_traj);
                    }
                    else if (Singleton.TypeAction == PdrActionType.EgaliteLateraleForce || Singleton.TypeAction == PdrActionType.BiaisLateralForce)
                    {
                        // Rien à faire le robot ne bouge jamais
                    }
                    else
                    {
                        this.pss.Pss.SendCommandFrame(CommandCodes.mode_libre_visc);
                    }
                }
                else
                {
                    if (Singleton.TypeAction == PdrActionType.FacilitationAction || Singleton.TypeAction == PdrActionType.FacilitationLaterale)
                    {
                        this.InitTraj();
                    }
                    else if (Singleton.TypeAction == PdrActionType.EgaliteLateraleForce || Singleton.TypeAction == PdrActionType.BiaisLateralForce)
                    {
                        // Rien à faire le robot ne bouge jamais
                    }
                    else
                    {
                        this.pss.Pss.SendCommandFrame(CommandCodes.mod_home);
                    }
                }
                if (isTraining == false)
                {
                    canDoMath = true;
                }
            }

            gameController.PauseGame();
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

        private void SaveRawData(List<PositionDataModel> positions, List<ForceDataModel> forces, List<short> klats, List<short> clongs ,string fileName)
        {
            if (positions.Count > 0 && forces.Count > 0)
            {
                using (TextWriter tw = new StreamWriter(this.pathPatient + "/" + fileName + ".txt"))
                {
                    // Date et heure
                    tw.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + System.Environment.NewLine);

                    for (int i = 0; i < positions.Count; i++)
                    {
                        // Enregistrement des positions seulement dans les exercices ou la poignée bouge.
                        if (Singleton.TypeAction != PdrActionType.EgaliteLateraleForce && Singleton.TypeAction != PdrActionType.BiaisLateralForce)
                        {
                            // Positions
                            tw.WriteLine(Math.Round(positions[i].PositionXd / 100.0, 4));
                            tw.WriteLine(Math.Round(positions[i].PositionYd / 100.0, 4)); 
                        }

                        // Forces
                        // Au cas ou on a pas le meme nombre de positions que de force
                        if (forces.Count > i)
                        {
                            tw.WriteLine(forces[i].ForceX);
                            tw.WriteLine(forces[i].ForceY);
                        }
                        else
                        {
                            tw.WriteLine("0");
                            tw.WriteLine("0");
                        }

                        if (Singleton.TypeAction == PdrActionType.FacilitationAction || Singleton.TypeAction == PdrActionType.FacilitationLaterale)
                        {
                            if (klats.Count > i && clongs.Count > i)
                            {
                                tw.WriteLine(klats[i]);
                                tw.WriteLine(clongs[i] + System.Environment.NewLine);
                            }
                            else
                            {
                                tw.WriteLine("0");
                                tw.WriteLine("0" + System.Environment.NewLine);
                            }
                        }
                    }
                }
            }
        }

        private void SaveResultats(double meanSpeed, double cvSpeed, double peakSpeed, double accuracy, double straighness, double speedMetric, double jerkMetric, double initTime, int numCible)
        {
            using (TextWriter tw = new StreamWriter(this.pathPatient + "/resultats.txt", true))
            {
                tw.WriteLine(numCible + " " + this.tabRepetition[numCible - 1].ToString() + " " + initTime + " " + meanSpeed + " " + cvSpeed + " " + peakSpeed + " " + accuracy + " " + straighness + " " + speedMetric + " " + jerkMetric + System.Environment.NewLine);
            }
        }

        private void CalculResultatCinematique(int numCible)
        {
            double time = 0.008;
            double meanSpeed = 0.0, cvSpeed = 0.0, peakSpeed = 0.0, accuracy = 0.0, straightness = 0.0, speedMetric = 0.0, jerkMetric = 0.0;
            
            List<DataPosition> dataPos = new List<DataPosition>();

            for (int i = 0; i < this.listPosition.Count; i++)
            {
                var data = new DataPosition(this.listPosition[i].PositionXd / 100.0, this.listPosition[i].PositionYd / 100.0);
                dataPos.Add(data);
            }

            // Calcul pour les vitesses
            double[] tabVitesse = new double[dataPos.Count];
            meanSpeed = AxAnalyse.Ax_Vitesse.VitesseMoy(dataPos, ref peakSpeed, ref tabVitesse,time);
            cvSpeed = this.Cv(tabVitesse, meanSpeed);

            if (peakSpeed == 0.0)
                speedMetric = 0.0;
            else
                speedMetric = meanSpeed / peakSpeed;

            // Calcul de la précision
            accuracy = AxAnalyse.Ax_Generique.DistancePythagorean(dataPos.Last().X, dataPos.Last().Y, EchelleUtils.MiseEchelleEnvoyerX(cibles.ElementAt(numCible - 1).X), EchelleUtils.MiseEchelleEnvoyerY(cibles.ElementAt(numCible - 1).Y));

            // Calcul de straightness on a besoin de l'amplitude et de la distance réel
            double amplitude = AxAnalyse.Ax_Generique.DistancePythagorean(dataPos.First().X, dataPos.First().Y, EchelleUtils.MiseEchelleEnvoyerX(cibles.ElementAt(numCible - 1).X), EchelleUtils.MiseEchelleEnvoyerY(cibles.ElementAt(numCible - 1).Y));
            double distReel = AxAnalyse.Ax_Position.Distance(dataPos);

            if (distReel == 0.0)
                straightness = 0.0;
            else
            {
                straightness = amplitude / distReel;

                if (Double.IsInfinity(straightness))
                    straightness = 0.0;
            }
                
            // Calcul du jerk
            jerkMetric = AxAnalyse.Ax_Vitesse.JerkMet(dataPos, peakSpeed, time);

            double iniTime = Convert.ToDouble(gameController.GetValue("InitTime"));

            this.SaveResultats(Math.Round(meanSpeed, 4), Math.Round(cvSpeed, 4), Math.Round(peakSpeed, 4), Math.Round(accuracy, 4), Math.Round(straightness, 4), Math.Round(speedMetric, 4), Math.Round(jerkMetric, 4), Math.Round(iniTime, 4), numCible);
        }

        private void CalculResultatForce(int numCible)
        {
            using (TextWriter tw = new StreamWriter(this.pathPatient + "/resultats.txt", true))
            {
                string direction = string.Empty;
                bool gaucheForce = Convert.ToBoolean(gameController.GetValue("GaucheForce"));
                if (gaucheForce == true)
                {
                    direction = "Gauche";
                }
                else
                {
                    direction = "Droite";
                }

                string reponse = Convert.ToString(gameController.GetValue("Reponse"));
                double iniTime = Convert.ToDouble(gameController.GetValue("InitTimeForce"));
                tw.WriteLine(numCible + " " + this.tabRepetition[numCible - 1].ToString() + " " + Math.Round(iniTime, 4) + " " + direction + " " + reponse + System.Environment.NewLine);
            }
        }

        private double Cv(double[] tab, double moyenne)
        {
            double somme = 0.0;
            for (int i = 0; i < tab.Length; i++)
            {
                somme += Math.Pow((tab[i] - moyenne), 2);
            }
            double sqrt = Math.Sqrt(somme);

            if (moyenne == 0.0)
                return 0.0;
            else
                return sqrt / moyenne;
        }

        private void CreatPatientDirectory()
        {
            if (Directory.Exists(this.pathPatient + DateTime.Now.ToShortDateString().Replace('/', '-')))
            {
                // Exactement le meme exercice à été fait a cette date il faut donc changer le nom du dossier final
                var newName = FindDirectoryName(this.pathPatient, DateTime.Now.ToShortDateString().Replace('/', '-'));
                this.pathPatient += newName;
            }
            else
            {
                // Pas de dossier pour cette date on peut donc prendre simplement la date
                this.pathPatient += DateTime.Now.ToShortDateString().Replace('/', '-');
            }

            Directory.CreateDirectory(pathPatient);
        }

        private string FindDirectoryName(string path, string baseName)
        {
            int indiceFichier = 1;
            while (Directory.Exists(path + "/" + baseName))
            {
                if (indiceFichier == 1)
                    baseName += "_" + indiceFichier.ToString();
                else
                {
                    var newNom = baseName.Replace((indiceFichier - 1).ToString(), indiceFichier.ToString());
                    baseName = newNom;
                }
                indiceFichier++;
            }

            return baseName;
        }
        #endregion

        #region CanExecute
        private bool StartCommand_CanExecute()
        {
            return !isPlaying;
        }

        private bool StopCommand_CanExecute()
        {
            return isPlaying;
        }

        private bool PauseCommand_CanExecute()
        {
            return isPlaying;
        }
        #endregion

        #region Navigation
        /// <summary>
        /// Retour à l'accueil
        /// </summary>
        private void GoHome()
        {
            this.Stop();
            nav.NavigateTo<HomeViewModel>("SetIsRetour", new object[] { true });
        }

        private void GoBack()
        {
            this.Stop();
            nav.GoBack();
        }
        #endregion 
    }
}