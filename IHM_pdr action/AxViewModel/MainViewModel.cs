using GalaSoft.MvvmLight;
using AxModel;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using System.Windows.Threading;
using AxModel.Helpers;
using System.Timers;
using AxCommunication;
using AxAction;
using AxError;
using AxModelExercice;
using AxError.Exceptions;
using AxModel.Message;
using AxData;
using AxConfiguration;
using Navegar;
using GalaSoft.MvvmLight.Ioc;
using System.IO;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using AxVolume;
using System.Xml.Linq;
namespace AxViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm/getstarted
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Fields
        
        private string _visibility = "hidden";
        private readonly DispatcherTimer _timerLoadAccueil;
        public IMessageBoxService _messageBoxService;
        public ActionRobot _axrobot;
        public INavigation _nav;
        private bool allowWindowToClose = true; // Should we let our window close?
        private List<IPageViewModel> _pageViewModels;   // Liste de page pour la navigation dans l'application
        private IPageViewModel _currentPageViewModel;   // La vue courante (affichée)
        
        private bool _isBusy;
        private string _isBusyString;

        private int _startingSequence;
        private bool _startingSequenceActivated;
        /// <summary>
        /// Variable utilise pour le canExecute des boutton de la visualisation
        /// </summary>
        private bool _pause = false;
        private bool _reset = false;
        private bool _stop = false;
        private bool _inTraining = false;
        private bool _inExercice = false;
        private bool _isRestarting = false;
        private bool _inStop = false;
        private bool _canPause = false;
        private bool _isStarting = false;
        private bool _canAfterStop = true;
        //permet de savoir si on peut reenvoyé ou non le message pour continuer l'exercice.
        private bool _canExerciceSuivant = true;
        private static System.Timers.Timer _startingSequenceTimer;

        private bool notOnStart = false; //permet de ne pas executer le changement de langage au démarrage
        private bool notOnFirstClick = false;
        private bool _isInEvaluation;
        private bool infoSTOP = true;

        private bool _up, _down;

        private List<ExerciceGeneric> _currentExercice;
        private int _nbrAckStop = 0;

        SingletonReeducation ValReed;
        
        GestionVolume volume; //gestionaire du volmue de Windows.
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            try
            {
                _up = true;
                _down = true;
                SingletonReeducation.RecupValeur();
                Singleton.CriticalError = false;
                ValReed = SingletonReeducation.getInstance();
                _axrobot = SimpleIoc.Default.GetInstance<ActionRobot>();
                _messageBoxService = SimpleIoc.Default.GetInstance<IMessageBoxService>();
                _nav = SimpleIoc.Default.GetInstance<INavigation>();
                CreateCommands();       // création des commandes de la page
                CreateMessengers();     // création des messages
                InitApp();
                //Permet d'éviter l'appel à la navigation pendant le chargement du viewmodel principal. Sans ceci le chargement du MainViewModel
                //ne se passe pas correctement puisque le chargement du viewmodel n'est pas possible
                _timerLoadAccueil = new DispatcherTimer
                {
                    Interval = new TimeSpan(0, 0, 0, 0, 500)
                };
                _timerLoadAccueil.Tick += LoadAccueil;
                _timerLoadAccueil.Start();
                ChargementConfig();
                Debug.Print("MainViewModel OK");
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        #endregion

        #region Properties

        private List<string> _cmbLangage;

        public List<string> CmbLangage
        {
            get { return _cmbLangage; }
            set 
            { 
                _cmbLangage = value;
                RaisePropertyChanged("CmbLangage");
            }
        }

        private string _selectedLangage;

        public string SelectedLangage
        {
            get { return _selectedLangage; }
            set 
            { 
                _selectedLangage = value;
                ChangementCulture(); 
                RaisePropertyChanged("SelectedLangage");
            }
        }


        private string _labelPatient;
        public string LabelPatient
        {
            get { return _labelPatient; }
            set
            {
                if (_labelPatient != value)
                {
                    _labelPatient = value;
                    RaisePropertyChanged("LabelPatient");
                }
            }
        }

        public string Visibility
        {
            get { return _visibility; }
            set
            {
                if (_visibility != value)
                {
                    _visibility = value;
                    RaisePropertyChanged("Visibility");
                }
            }
        }
        /// <summary>
        /// Permet de contrôler la propriété CurrentView
        /// </summary>
        private ViewModelBase _currentView;

        /// <summary>
        /// L'attribut CurrentViewNavigation permet de définir automatiquement, quelle propriété du viewmodel
        /// devra être utilisé pour charger le viewmodel vers lequel la navigation va s'effectuer
        /// </summary>
        [CurrentViewNavigation]
        public ViewModelBase CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                RaisePropertyChanged("CurrentView");
            }
        }
        /// <summary>
        /// Récupération de la liste des pages
        /// </summary>
        public List<IPageViewModel> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new List<IPageViewModel>();

                return _pageViewModels;
            }
        }

        /// <summary>
        /// Page courante (affichée)
        /// </summary>
        public IPageViewModel CurrentPageViewModel
        {
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                    RaisePropertyChanged("CurrentPageViewModel");
                }
            }
        }

        /// <summary>
        /// Liste de port serie
        /// </summary>
        public ObservableCollection<string> PortName
        {
            get { return this._axrobot.GetPortNameCollection(); }
        }

        /// <summary>
        /// port serie selectionné
        /// </summary>
        public string SelectedPortName
        {
            get
            {
                return _axrobot.Pss.SelectedPortName;
            }
            set
            {
                _axrobot.Pss.SelectedPortName = value;
                RaisePropertyChanged("SelectedPortName");
            }
        }

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }

        public string IsBusyString
        {
            get
            {
                return _isBusyString;
            }
            set
            {
                _isBusyString = value;
                RaisePropertyChanged("IsBusyString");
            }
        }

        public bool StartingSequenceActivated
        {
            get
            {
                return _startingSequenceActivated;
            }
            set
            {
                _startingSequenceActivated = value;
                RaisePropertyChanged("StartingSequenceActivated");
            }
        }

        public int StartingSequence
        {
            get
            {
                return _startingSequence;
            }
            set
            {
                _startingSequence = value;
                RaisePropertyChanged("StartingSequence");
            }
        }

        #endregion

        #region Methods
        private void OnConnected(Singleton obj)
        {
            Visibility = "Visible";
            LabelPatient = "" + obj.PatientSingleton.Prenom + " " + obj.PatientSingleton.Nom;
        }
        /// <summary>
        /// Navigation vers le premier viewmodel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadAccueil(object sender, EventArgs e)
        {
            _timerLoadAccueil.Stop();
            if (IsInDesignMode)
                _nav.NavigateTo<FormulaireInscriptionAdministrateurViewModel>();
            else
            {
                //sablier si la recherche d'admin est longue
                UiServices.SetBusyState();
                if (AdminData.AdminInBd())
                    _nav.NavigateTo<ConnexionTherapeuteViewModel>();
                else
                    _nav.NavigateTo<FormulaireInscriptionAdministrateurViewModel>();
            }
            //Initialise le module de gestion du volume.
            //if (!IsInDesignMode)
                //volume = new GestionVolume(); 
        }
        /// <summary>
        /// Initialisation des commandes de la page
        /// </summary>
        private void CreateCommands()
        {
            AboutCommand = new RelayCommand(() => About());
            ExitCommand = new RelayCommand(() => Exit());
            ChangePageCommand = new RelayCommand<string>((p) => ChangeViewModel(p));
            HomeComCommand = new RelayCommand(Home, Home_CanExecute);
            HomeExComCommand = new RelayCommand(HomeEx, HomeEx_CanExecute);
            HomingComCommand = new RelayCommand(Homing, Homing_CanExecute);
            LibreComCommand = new RelayCommand(Libre, Libre_CanExecute);
            OpenComCommand = new RelayCommand(OpenCom, OpenCom_CanExecute);
            CloseComCommand = new RelayCommand(CloseCom, CloseCom_CanExecute);
            StopComCommand = new RelayCommand(() => Stop());
            EnvoyerComCommand = new RelayCommand(() => DemarerExercice(), StartCanExecute);
            EnvoyerNextComCommand = new RelayCommand(() => EnvoyerNextComPort(), ExericeSuivantCanExecute);
            StopNVComCommand = new RelayCommand(() => StopNv(), StopCanExecute);
            Go_DebutCommand = new RelayCommand(() => go_Debut(), RecommencerCanExecute);
            PauseCommand = new RelayCommand(() => Pause(), PauseCanExecute);
            TrainingCommand = new RelayCommand(() => Training(), TrainingCanExecute);
            UpComCommand = new RelayCommand(UpCommand, UpCommand_CanExecute);
            DownComCommand = new RelayCommand(DownCommand, DownCommand_CanExecute);
            DecoCommand = new RelayCommand(Deconnexion);
            this.CloseCommand = new RelayCommand(() => Close(), () => this.Close_CanExecute());
        }

        private void Deconnexion2(bool y)
        {
            Singleton.logOffPatient();
            LabelPatient = null;
            Visibility = "Hidden";

            _nav.NavigateTo<HomeViewModel>(true);
        }


        /// <summary>
        /// Initialisation des messages de la page
        /// </summary>
        private void CreateMessengers()
        {
            Messenger.Default.Register<List<ExerciceGeneric>>(this, "ReeducationKidWizardViewModel", TraitementExercice);   // abonnement aux messages envoyé par ReeducationKidWizard pour envoyer au µc
            Messenger.Default.Register<List<ExerciceGeneric>>(this, "EvaluationViewModel", TraitementExercice);               // abonnement aux messages envoyé par Evaluation pour envoyer au µc
            Messenger.Default.Register<FrameConfigDataModel>(this, "VisualisationViewModel", TraitementConfigExercice); // message de LudiqueViewModel
            Messenger.Default.Register<FrameExerciceDataModel>(this, "VisualisationViewModelConfigExercice", TraitementConfigRegulateur); // message de LudiqueViewModel
            Messenger.Default.Register<CommandCodes>(this, "MessageCommand", TraitementCommande);
            Messenger.Default.Register<FrameExerciceDataModel>(this, "MainPViewModel", TraitementPositionGameExercice); // message de MainPViewModel
            Messenger.Default.Register<FrameExerciceDataModel>(this, "MainPViewModelDyn", TraitementPositionGameExerciceDyn); // message de MainPViewModel
            Messenger.Default.Register<String>(this, "InscriptionViewModel", ChangeViewModel);
            Messenger.Default.Register<Singleton>(this, "Singleton", OnConnected);
            Messenger.Default.Register<bool>(this, "StartCible", StartCible);
            Messenger.Default.Register<bool>(this, "DecoUtilisateur", Deconnexion2);
            Messenger.Default.Register<string>(this, "StopRobot", StopByHome);
            Messenger.Default.Register<string>(this, "ResetCurentListExercice", ResetCurrentList);
            Messenger.Default.Register<string>(this, "ResetCanExecuteMainVM", ResetCanExecute);
            Messenger.Default.Register<bool>(this, "StopSendPositions", e => infoSTOP = e);
            Messenger.Default.Register<bool>(this, "StartStopGame", StartStopGame);
            Messenger.Default.Register<bool>(this, "PoulieEnPause", PouliesEnPause);
            Messenger.Default.Register<bool>(this, "StartExerciceAutomatique", StartExerciceAutomatique);
            Messenger.Default.Register<bool>(this, "BloquerStart", BloquerStart);
        }
        private bool BouttonCanExecute()
        {
            return !Singleton.GetRobotError();
        }
        private bool PauseCanExecute()//active le bouton de pause seulement quand un exercice est en cours et pas d'erreur robot
        {
            if (Singleton.GetRobotError() || (_inTraining==false && _inExercice==false) || _canPause==false || _isRestarting==true || _isStarting==true)
                return false;
            else
                return true;
        }
        private bool StartCanExecute()//active le bouton de démarage seulement quand pas d'erreur robot ou qu'un exercice n'est pas lancé
        {
            if (Singleton.GetRobotError() || _inStop == true || _canPause == true || _isStarting == true || IsBusy == true)
                return false;
            else
                return true;
        }
        private bool TrainingCanExecute()//active le bouton d'entrainement seulement quand pas d'erreur robot ou qu'un exercice n'est pas lancé
        {
            if (Singleton.GetRobotError() || (_inTraining == false && _inExercice == true) || _inStop == true || _isRestarting == true || _canPause == true || _isStarting == true)
                return false;
            else
                return true;
        }
        private bool StopCanExecute()
        {
            if (Singleton.GetRobotError() || _stop == false || _isRestarting == true || _isStarting == true || IsBusy == true)
                return false;
            else
                return true;
        }
        private bool RecommencerCanExecute()//active le bouton recommencer seulement quand pas d'erreur robot ou qu'un exercice est lancé
        {
            if (Singleton.GetRobotError() || _reset == false || _isStarting == true)
                return false;
            else
                return true;
        }
        private bool ExericeSuivantCanExecute()
        {
            if (Singleton.GetRobotError() || _currentExercice.Count <= 1 || _isStarting == true)
                return false;
            else
                return true;
        }
        private void ResetCanExecute(string s)
        {
            _reset = false;
            _pause = false;
            _stop = false;
            _inExercice = false;
            _inTraining = false;
            _inStop = false;
            _canPause = false;
            _isStarting = false;
            _canAfterStop = true;
        }
        private void StartStopGame(bool s)
        {
            if (s == false)
            {
                _stop = false;
                _canPause = false;
                _canAfterStop = false;
                PauseCommand.RaiseCanExecuteChanged();
            }
        }
        private void StartCible(bool yu)
        {
            try
            {
                EnvoyerComPort();
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        private void InitApp()
        {
            _currentExercice = new List<ExerciceGeneric>();
            _isBusy = false;
            _isBusyString = string.Empty;

            _isInEvaluation = false;

            _startingSequence = 0;                  // Var pour le démarrage d'un exercice
            _startingSequenceActivated = false;     // Var pour l'affichage du démarrage de l'exercice
            _startingSequenceTimer = new System.Timers.Timer(1000); // Create a timer with a ten second interval.
            _startingSequenceTimer.Elapsed += new ElapsedEventHandler(OnStartingSequenceEvent);    // Hook up the Elapsed event for the timer.
            _startingSequenceTimer.Enabled = false;

            // _portSerie = new PortSerieModel(); // object port série
            _axrobot.Pss.aXdataReceived += new onaXdataReceived(_portSerieModel_aXdataReceived);  // abonnement aux evennements du port serie
            _axrobot.Pss.CoupleDataReceived += new onCoupleDataReceived(_portSerieModel_CoupleDataReceived);
            _axrobot.Pss.PositionDataReceived += new onPositionDataReceived(_portSerieModel_PositionDataReceived);
            _axrobot.Pss.Position2DataReceived += new onPosition2DataReceived(_portSerieModel_Position2DataReceived);
            _axrobot.Pss.ForceDataReceived += new onForceDataReceived(_portSerieModel_ForceDataReceived);
            _axrobot.Pss.Force2DataReceived += new onForce2DataReceived(_portSerieModel_Force2DataReceived);
            _axrobot.Pss.ForceRapDataReceived += new onForceRapDataReceived(_portSerieModel_ForceRapDataReceived);
            _axrobot.Pss.ForceRap2DataReceived += new onForceRap2DataReceived(_portSerieModel_ForceRap2DataReceived);
            _axrobot.Pss.AcosTDataReceived += new onAcosTDataReceived(_portSerieModel_AcosTDataReceived);
            //_axrobot.Pss.VitesseDataReceived += new onVitesseDataReceived(_portSerieService_VitesseDataReceived);
            //_axrobot.Pss.Vitesse2DataReceived += new onVitesse2DataReceived(_portSerieService_Vitesse2DataReceived);
            _axrobot.Pss.ACKDataReceived += new onACKDataReceived(_portSerieModel_ACKDataReceived);
            _axrobot.Pss.FrameConfigDataReceived += new onFrameConfigDataReceived(_portSerieModel_FrameConfigDataReceived);
            _axrobot.Pss.ErrorDataReceived += new onErrorDataReceived(_portSerieService_ErrorDataReceived);
            //_axrobot.Pss.FrameExerciceDataReceived += new onFrameExerciceDataReceived(_portSerieService_FrameExerciceDataReceived);
            if (PortName.Count != 0)
            {
                SelectedPortName = PortName[0];
            }
            Singleton.IsCalibre = false;
        }

        private void ChargementConfig()
        {
            if (IsInDesignMode == false)//correction bug empeche de charger le fichier de config en debug
            {
                //Chargement des info a partir du fichier de config
                ExerciceConfig.InitConfigurationProgramme();

                //Changement de la langue si nécessaire
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(Singleton.ConfigProg.Culture);
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Singleton.ConfigProg.Culture);

                //Permet de recuperer la liste des langages dispo dans l'application
                CmbLangage = new List<string>();
                string executablePath = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
                string[] directories = Directory.GetDirectories(executablePath);
                CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
                foreach (string s in directories)
                {
                    DirectoryInfo langDirectory = new DirectoryInfo(s);
                    bool culturePresente = false;
                    foreach (var st in cultures)
                    {
                        if (st.Name == langDirectory.Name)
                            culturePresente = true;
                    }
                    if (culturePresente == true)
                    {
                        //ne s'occupe que des ressource lié aux langues
                        int numParenthese = CultureInfo.GetCultureInfo(langDirectory.Name).DisplayName.IndexOf('(');
                        string lang = CultureInfo.GetCultureInfo(langDirectory.Name).DisplayName;
                        if (numParenthese == -1)
                            CmbLangage.Add(lang);
                        else
                            CmbLangage.Add(lang.Remove(numParenthese, lang.Length - numParenthese)); //si ( dans le DisplayName, ne prend que le nom de langue 
                        //regle la selection de la langue par défaut sur la current culture
                        if (Singleton.ConfigProg.Culture == CultureInfo.GetCultureInfo(langDirectory.Name).Name)
                            SelectedLangage = CmbLangage[CmbLangage.Count - 1];
                    }
                }
                //Mauvaise fermeture de l'application (ex : coupure de courant)
                if (!Singleton.ConfigProg.BonneFermeture)
                    throw new MauvaiseFermetureException();
            }
        }

        private void ChangementCulture()
        {
            if (notOnStart == true)
            {
                //Change la culture dans le fichier seulement si elle est presente dans les dossier
                if (notOnFirstClick ==true)//correction bug, quand on clik la premiere fois sur le bouton "langage" = compte comme un choix de langue
                {
                    CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
                    foreach (var cult in cultures)
                    {
                        if (cult.DisplayName.Contains(SelectedLangage))
                        {
                            string executablePath = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
                            string[] directories = Directory.GetDirectories(executablePath);
                            foreach (string s in directories)
                            {
                                DirectoryInfo langDirectory = new DirectoryInfo(s);
                                if (langDirectory.Name == cult.Name)
                                {
                                    ConfigData.ChangerCulture(cult.Name);
                                    _messageBoxService.ShowInformation(AxLanguage.Languages.REAplan_Changer_Langage);
                                }
                            }
                        }
                    }  
                }
                notOnFirstClick = true;
            }
            notOnStart = true;
        }

        private void Deconnexion()
        {
            if ((_messageBoxService.ShowYesNo(AxLanguage.Languages.REAplan_Unconnect_Patient2, CustomDialogIcons.Question) == CustomDialogResults.Yes))
            {
                Singleton.logOffPatient();
                LabelPatient = null;
                Visibility = "Hidden";
                Messenger.Default.Send("n", "StopRobot");//stop le robot et reset l'ecran de jeu
                Messenger.Default.Send("", "ResetCurentListExercice");
                _nav.NavigateTo<HomeViewModel>(true);
            }
        }

        private void TraitementExercice(List<ExerciceGeneric> listExercice)
        {
        }

        private void TraitementConfigExercice(FrameConfigDataModel newConfigExercice)
        {
            try
            {
                _axrobot.SendConfigFrame(newConfigExercice);
                System.Threading.Thread.Sleep(10);
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        private void TraitementConfigRegulateur(FrameExerciceDataModel newConfigRegulateur)
        {
            _axrobot.SendExerciceFrame(newConfigRegulateur);
        }

        private void TraitementCommande(CommandCodes newCmdExercice)
        {
            try
            {
                switch (newCmdExercice)
                {
                    case CommandCodes.mod_init_traj: _axrobot.SendCommandFrame(CommandCodes.mod_init_traj);
                        //System.Threading.Thread.Sleep(10);
                        break;
                    case CommandCodes.mod_suiv_traj: _axrobot.SendCommandFrame(CommandCodes.mod_suiv_traj);
                        //System.Threading.Thread.Sleep(10);
                        break;
                    case CommandCodes.STOPnv: _axrobot.SendCommandFrame(CommandCodes.STOPnv);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        /// <summary>
        /// Envoyer nouvelle position au µc en mode jeu
        /// </summary>
        /// <param name="newPositionGameExercice"></param>
        private void TraitementPositionGameExercice(FrameExerciceDataModel newPositionGameExercice)
        {
            newPositionGameExercice.Address = ConfigAddresses.mod_game_position;
            try
            {
                _axrobot.SendExerciceGameFrame(newPositionGameExercice);
                System.Threading.Thread.Sleep(10);
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        /// <summary>
        /// Envoyer nouvelle position au µc en mode jeu
        /// </summary>
        /// <param name="newPositionGameExercice"></param>
        private void TraitementPositionGameExerciceDyn(FrameExerciceDataModel newPositionGameExercice)
        {
            newPositionGameExercice.Address = ConfigAddresses.mod_game_position_dyn;
            try
            {
                _axrobot.SendExerciceGameFrame(newPositionGameExercice);
                System.Threading.Thread.Sleep(10);
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }
        private void Close()
        {
            // arret du jeu
            try
            {
                PauseCommand.RaiseCanExecuteChanged();
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
            // TODO : do close window work !
        }

        private bool Close_CanExecute()
        {
            if (Singleton.CriticalError == false)
            {
                if (this._messageBoxService.ShowYesNo(AxLanguage.Languages.REAplan_Confirmation_Quitter, CustomDialogIcons.Question) == CustomDialogResults.Yes)
                {
                    return allowWindowToClose == true;
                }
                else
                {
                    return false;
                }
            }
            else
                return true;
        }
        #endregion

        #region RelayCommands

        public RelayCommand DecoCommand
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the close command.
        /// </summary>
        /// <value>The close command.</value>
        public RelayCommand CloseCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the close failed command.
        /// </summary>
        /// <value>The close command.</value>
        public RelayCommand CloseFailCommand
        {
            get;
            private set;
        }

        public RelayCommand AboutCommand
        {
            get;
            private set;
        }

        public RelayCommand ExitCommand
        {
            get;
            private set;
        }

        public RelayCommand<string> ChangePageCommand
        {
            get;
            private set;
        }

        public RelayCommand HomeComCommand
        {
            get;
            private set;
        }

        public RelayCommand HomeExComCommand
        {
            get;
            private set;
        }

        public RelayCommand LibreComCommand
        {
            get;
            private set;
        }

        public RelayCommand HomingComCommand
        {
            get;
            private set;
        }

        public RelayCommand OpenComCommand
        {
            get;
            private set;
        }

        public RelayCommand CloseComCommand
        {
            get;
            private set;
        }

        public RelayCommand StopComCommand
        {
            get;
            private set;
        }

        public RelayCommand EnvoyerComCommand
        {
            get;
            private set;
        }

        public RelayCommand EnvoyerNextComCommand
        {
            get;
            private set;
        }

        public RelayCommand StopNVComCommand
        {
            get;
            private set;
        }

        public RelayCommand Go_DebutCommand
        {
            get;
            private set;
        }

        public RelayCommand PauseCommand
        {
            get;
            private set;
        }

        public RelayCommand UpComCommand
        {
            get;
            private set;
        }

        public RelayCommand DownComCommand
        {
            get;
            private set;
        }

        public RelayCommand TestErreurCommand
        {
            get;
            private set;
        }

        public RelayCommand TrainingCommand
        {
            get;
            private set;
        }
        #endregion

        #region Actions

        private void About()
        {
            // TODO : trouver une solution pour show et show dialog
        }

        /// <summary>
        /// Appelé par ExitCommand, ferme l'application
        /// </summary>
        private void Exit()
        {
            try
            {
                Application.Current.MainWindow.Close();
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            } //  Shutdown
            //System.Windows.Application.Current.Shutdown();
        }

        /// <summary>
        /// Appelé par ChangePageCommand, change la page courante (affichée)
        /// </summary>
        /// <param name="viewModel"></param>
        /// 

        private void ChangeViewModel(string viewModel)
        {
            try
            {
                if (viewModel == "0")
                {
                    //PageViewModels[0] = new HomeViewModel(0);
                    //PageViewModels[1] = new HomeViewModel(1);
                    //PageViewModels[2] = new HomeViewModel(2);
                    GC.Collect();   // TODO : perf issue ne pas utiliser, utilisé ici pour corriger un bug de messenger !
                    CurrentPageViewModel = PageViewModels[Convert.ToInt32(viewModel, 10)];
                }
                else
                {
                    if (viewModel == "3")
                    {
                        Messenger.Default.Send(false, "MainViewModel");//permettre a accueil de cloturer l'exercice en cours
                        //PageViewModels[8] = new EvaluationViewModel();
                        GC.Collect();       // TODO : perf issue ne pas utiliser, utilisé ici pour corriger un bug de messenger !
                        CurrentPageViewModel = PageViewModels[Convert.ToInt32(viewModel, 10)];
                    }
                    else
                    {
                        CurrentPageViewModel = PageViewModels[Convert.ToInt32(viewModel, 10)];
                    }
                }
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        /// <summary>
        /// Appelé par HomeComCommand,
        /// </summary>
        private void Home()
        {
            try
            {
                _axrobot.SendCommandFrame(CommandCodes.mod_home);
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        /// <summary>
        /// Appelé par HomeComCommand,
        /// </summary>
        /// <returns></returns>
        private bool Home_CanExecute()
        {
            return true;
        }

        /// <summary>
        /// Appelé par HomeExComCommand,
        /// </summary>
        private void HomeEx()
        {
            // Home
            try
            {
                _axrobot.SendCommandFrame(CommandCodes.mod_init_traj);
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        /// <summary>
        /// Appelé par HomeExComCommand,
        /// </summary>
        /// <returns></returns>
        private bool HomeEx_CanExecute()
        {
            return true;
        }

        /// <summary>
        /// Appelé par HomingComCommand,
        /// </summary>
        private void Homing()
        {
            if (_isInEvaluation == true)
            {
                Messenger.Default.Send(false, "MainViewModel");     // arrêter l'evaluation
            }
            else
            {

            }
            // Home Calibration
            try
            {
                _axrobot.SendCommandFrame(CommandCodes.mod_homing);
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        /// <summary>
        /// Appelé par HomingComCommand,
        /// </summary>
        /// <returns></returns>
        private bool Homing_CanExecute()
        {
            if (_axrobot.IsOpen() && IsBusy ==false)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Appelé par LibreComCommand,
        /// </summary>
        private void Libre()
        {
            MessageBoxResult result = MessageBox.Show("Êtes-vous sur de vouloir lancer le mode libre ?", "Attention !", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Cancel);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    // Libre
                    try
                    {
                        _axrobot.SendCommandFrame(CommandCodes.mode_libre);
                    }
                    catch (Exception ex)
                    {
                        GestionErreur.GerrerErreur(ex);
                    }
                    break;
                case MessageBoxResult.No:
                    break;
                case MessageBoxResult.Cancel:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Appelé par LibreComCommand,
        /// </summary>
        /// <returns></returns>
        private bool Libre_CanExecute()
        {
            return true;
        }

        /// <summary>
        /// Appelé par OpenComCommand, ouvre le port série
        /// </summary>
        private void OpenCom()
        {
            try
            {
                _axrobot.Open(); 
            }
            catch (IOException)
            {//obligé de refaie un  y catch pour throw la nouvelle exception et pour que sont stacktrace soit garni
                try
                {
                    throw new WrongSerialPortException();
                }
                catch (Exception ex)
                { GestionErreur.GerrerErreur(ex); }
            }
            catch (ArgumentOutOfRangeException)
            {
                try
                {
                    throw new WrongSerialPortException();
                }
                catch (Exception ex)
                { GestionErreur.GerrerErreur(ex); }
            }
            catch (InvalidOperationException)
            {
                try
                {
                    throw new AlreadyOpenSerialPortException();
                }
                catch (Exception ex)
                { GestionErreur.GerrerErreur(ex); }
            }
            finally
            {
                CloseComCommand.RaiseCanExecuteChanged();
                OpenComCommand.RaiseCanExecuteChanged();
                HomingComCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Appelé par OpenComCommand, vérifie si le port est ouvert
        /// </summary>
        /// <returns></returns>
        private bool OpenCom_CanExecute()
        {
            return !_axrobot.IsOpen();
        }

        /// <summary>
        /// Appelé par CloseComCommand, ferme le port série
        /// </summary>
        private void CloseCom()
        {
            if (MessageBox.Show("Voulez-vous vraiment couper la connexion avec le robot ?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    _axrobot.Close();
                    Singleton.IsCalibre = false;
                }
                catch (Exception ex)
                {
                    GestionErreur.GerrerErreur(ex);
                }
                finally
                {
                    CloseComCommand.RaiseCanExecuteChanged();
                    OpenComCommand.RaiseCanExecuteChanged();
                    HomingComCommand.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// Appelé par CloseComCommand, vérifie si le port est fermé
        /// </summary>
        /// <returns></returns>
        private bool CloseCom_CanExecute()
        {
            return _axrobot.IsOpen();
        }

        private void Stop()
        {
            if (_isInEvaluation == true)
            {
                Messenger.Default.Send(false, "MainViewModel");     // arrêter l'evaluation
                try
                {
                    _axrobot.SendCommandFrame(CommandCodes.STOP);
                }
                catch (Exception ex)
                {
                    GestionErreur.GerrerErreur(ex);
                }
            }
            else
            {
                Messenger.Default.Send<bool>(false, "StartStopGame"); // arrêter de reeducation jeu
                try
                {
                    _axrobot.SendCommandFrame(CommandCodes.STOP);
                }
                catch (Exception ex)
                {
                    GestionErreur.GerrerErreur(ex);
                }
                // arrêter reeducation jeu
            }
        }

        private void DemarerExercice()
        {
            _inExercice = true;
            _inTraining = false;
            _canExerciceSuivant = true;
            EnvoyerComPort();
        }

        private void EnvoyerComPort()
        {
            try
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    _isStarting = true;
                    PauseCommand.RaiseCanExecuteChanged();
                    StartingSequence = 5;
                    StartingSequenceActivated = true;
                    _startingSequenceTimer.Stop();
                    _startingSequenceTimer.Start();
                    Messenger.Default.Send(false, "VisualisationCanPrecedent");
                    Messenger.Default.Send(true, "CleanDePause");
                    _reset = true;
                    _stop = true;
                }), DispatcherPriority.Normal);
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }


        private void EnvoyerNextComPort()
        {
            try
            {
                Messenger.Default.Send<bool>(true, "InitAnalyseEval");
                ResetCanExecute("");
                if (_currentExercice.Count != 0)
                {
                    _currentExercice.RemoveAt(0);
                    if (_currentExercice.Count != 0)
                    {
                        TraitementExercice(_currentExercice);
                        Messenger.Default.Send(_currentExercice, "NextExercice");
                        Messenger.Default.Send(false, "VisualisationCanPrecedent");
                        if(_isInEvaluation==true)
                            Messenger.Default.Send(_currentExercice, "RefreshListe");
                    }
                    else
                    {
                        MessageBox.Show("Erreur : Exercice vide !", "Erreur !", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Erreur : Exercice vide !", "Erreur !", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        private void StopNv()
        {
            try
            {
                _axrobot.SendCommandFrame(CommandCodes.STOPnv);
                infoSTOP = true;
                _canPause = false;
                _stop = false;
                _pause = false;
                EnvoyerComCommand.RaiseCanExecuteChanged();
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        private void PouliesEnPause(bool status)
        {
            if (status)
            {
                //Grise les bouton quand le jeu Poulies est en pause
                _stop = false;
                _pause = true;
                _canPause = true;
                infoSTOP = true;
            }
            else
            {
                //Dégrise les bouton quand le jeu Poulies est en pause
                _pause = false;
                _canPause = false;
            }
            EnvoyerComCommand.RaiseCanExecuteChanged();
        }

        private void StartExerciceAutomatique(bool b)
        {
            this.DemarerExercice();
        }

        private void BloquerStart(bool b)
        {
            this._canPause = true;
            EnvoyerComCommand.RaiseCanExecuteChanged();
        }

        private void StopByHome(string s)
        {
            //Dans le cas d'une deco utilisateur/admin coupe le robot et reset l'ecran sans passer par stopNV
            if (s == "n" || s== "k")
            {
                infoSTOP = true;
                _axrobot.SendCommandFrame(CommandCodes.STOPnv);
                if (s == "k")
                    _canExerciceSuivant = false;
            }
            else
                StopNv();
        }

        private void ResetCurrentList(string s)
        {
            _currentExercice.Clear();
            ResetCanExecute("");//Reset les canExecute quand on sort de la visualisation
        }
        /// <summary>
        /// Méthode qui met le robot au debut de l'exercice
        /// </summary>
        private void go_Debut()
        {
            Messenger.Default.Send<bool>(true, "InitAnalyseEval");
            _isRestarting = true;
            Messenger.Default.Send(_currentExercice, "EvaluationViewModel");
            Messenger.Default.Send(false, "VisualisationCanPrecedent");
        }
        private void Training()
        {
            _inTraining = true;
            _inExercice = false;
            Messenger.Default.Send<bool>(_inTraining, "Training");//signal  a EauationVM qu'on est en entrainement
            EnvoyerComPort();
        }
        private void Pause()
        {
            try
            {
                if (_pause == true)
                {
                    Messenger.Default.Send(_pause, "GamePause");
                    _axrobot.SendCommandFrame(CommandCodes.STOPnv);
                    _pause = false;
                }
                else
                {
                    EnvoyerComPort();
                }
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpCommand()
        {
            _up = true;
            _down = false;
            UpComCommand.RaiseCanExecuteChanged();
            DownComCommand.RaiseCanExecuteChanged();
            _axrobot.SendCommandFrame(CommandCodes.deviceDate);
            //Debug.Print("up");
            _up = true;
            _down = true;
            UpComCommand.RaiseCanExecuteChanged();
            DownComCommand.RaiseCanExecuteChanged();
        }

        private bool UpCommand_CanExecute()
        {
            if (_up == true && _axrobot.IsOpen() == true)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 
        /// </summary>
        private void DownCommand()
        {
            _up = true;
            _down = false;
            UpComCommand.RaiseCanExecuteChanged();
            DownComCommand.RaiseCanExecuteChanged();
            _axrobot.SendCommandFrame(CommandCodes.deviceSerialNumber);
            //Debug.Print("down");
            _up = true;
            _down = true;
            UpComCommand.RaiseCanExecuteChanged();
            DownComCommand.RaiseCanExecuteChanged();
        }

        private bool DownCommand_CanExecute()
        {
            if (_down == true && _axrobot.IsOpen() == true)
                return true;
            else
                return false;
        }

        void _portSerieModel_ForceDataReceived(object sender, ForceDataModel e)
        {
            // TODO : binder les receptions
            Messenger.Default.Send(e, "NewForce");      // Message Force
        }

        void _portSerieModel_Force2DataReceived(object sender, Force2DataModel e)
        {
            // TODO : binder les receptions
            Messenger.Default.Send(e, "NewForce2");      // Message Force
        }

        void _portSerieModel_ForceRapDataReceived(object sender, ForceRapDataModel e)
        {
            // TODO : binder les receptions
            if (infoSTOP == false)
            {
                Messenger.Default.Send(e, "NewForceRap");      // Message Force
            }
        }

        void _portSerieModel_ForceRap2DataReceived(object sender, ForceRap2DataModel e)
        {
            // TODO : binder les receptions
            if (infoSTOP == false)
            {
                Messenger.Default.Send(e, "NewForceRap2");      // Message Force
            }
        }


        void _portSerieModel_AcosTDataReceived(object sender, AcosTDataModel e)
        {
            // TODO : binder les receptions
            if (infoSTOP == false)
            {
                Messenger.Default.Send(e, "NewAcosT");      // Message Force
            }
        }

        void _portSerieModel_PositionDataReceived(object sender, PositionDataModel e)
        {
            //Debug.Print("X: " + (e.PositionX / 100.0).ToString() + " Y: " + (e.PositionY / 100.0).ToString());
            if (infoSTOP == false)
            {
                Messenger.Default.Send(e, "NewPosition");   // Message Position
            }
        }

        void _portSerieModel_Position2DataReceived(object sender, Position2DataModel e)
        {
            //Debug.Print("X: " + (e.PositionX / 100.0).ToString() + " Y: " + (e.PositionY / 100.0).ToString());
            if (infoSTOP == false)
            {

                Messenger.Default.Send(e, "NewPosition2");   // Message Position
            }
        }

        void _portSerieModel_CoupleDataReceived(object sender, CoupleDataModel e)
        {
        }

        void _portSerieModel_ACKDataReceived(object sender, ACKDataModel e)
        {
            if (e != null)
            {
                switch (e.Donnee[2])
                {
                    case ((byte)FrameHeaders.ACK_Stop):
                        break;
                    case ((byte)FrameHeaders.ACK_mod_suiv_traj):
                        //Previent EvaluationViewModel que le robot a bien démarré.
                        Messenger.Default.Send<bool>(true, "MainViewModel");
                        break;
                    case ((byte)FrameHeaders.ACK_mode_libre):
                        //_portSerie.ACK_ok();
                        //Etat = this.etats.Statuts[1];
                        break;
                    case ((byte)FrameHeaders.ACK_mod_traj):
                        //_portSerie.ACK_ok();
                        //Etat = this.etats.Statuts[0];
                        IsBusyString = "Positionnement...";
                        IsBusy = true;
                        break;
                    case ((byte)FrameHeaders.ACK_mod_homing):
                        //_portSerie.ACK_ok();
                        if (IsBusy == false)
                        {
                            //Etat = this.etats.Statuts[0];
                            IsBusyString = "Calibration...";
                            IsBusy = true;
                        }
                        else
                        {
                            //Etat = this.etats.Statuts[4];
                            IsBusy = false;
                                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    Singleton.IsCalibre = true;
                                    if (Singleton.GetRobotError())
                                    {
                                        Messenger.Default.Send<bool>(true, "InitAnalyseEval");
                                        Singleton.ChangeErrorStatu(false);//indique qu'il n'y a plus d'erreur
                                        RetablissementExerciceApresRobotErreur();
                                        ResetCanExecute("");
                                        Messenger.Default.Send(false, "VisualisationCanPrecedent");                                       
                                    }
                                    HomeComCommand.RaiseCanExecuteChanged();
                                    Messenger.Default.Send("", "RaiseCanExecuteHomeVM");
                                }), DispatcherPriority.DataBind);
                        }
                        break;
                    case ((byte)FrameHeaders.ACK_mod_init_traj):
                        if (IsBusy == false)
                        {
                            //Etat = this.etats.Statuts[0];
                            IsBusyString = "Positionnement...";
                            IsBusy = true;
                        }
                        else
                        {
                            Messenger.Default.Send(true, "Init_Traj");
                            // _portSerie.ACK_ok();
                            //Etat = this.etats.Statuts[4];
                            IsBusy = false;
                            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                if (_isRestarting == true)
                                {
                                    if (_inExercice == true)
                                        DemarerExercice();
                                    else if (_inTraining == true)
                                        Training();
                                }
                                if (_inStop == true)
                                {
                                    ResetCanExecute("");
                                    _inStop = false;
                                }
                                if (_isStarting == true)
                                    _isStarting = false;
                                PauseCommand.RaiseCanExecuteChanged();//réafiche correctement les bouttons
                            }), DispatcherPriority.DataBind);
                        }
                        break;
                    case ((byte)FrameHeaders.ACK_mod_home):
                        if (IsBusy == false)
                        {
                            //  _portSerie.ACK_ok();
                            //Etat = this.etats.Statuts[0];
                            IsBusyString = "Homing...";
                            IsBusy = true;
                        }
                        else
                        {
                            //Etat = this.etats.Statuts[4];
                            IsBusy = false;
                        }
                        break;
                    case ((byte)FrameHeaders.ACK_mod_game):// _portSerie.ACK_ok();
                        break;
                    case ((byte)FrameHeaders.ACK_Cibles):// _portSerie.ACK_ok();
                        break;
                    case ((byte)FrameHeaders.ACK_Formes):// _portSerie.ACK_ok();
                        break;
                    case ((byte)FrameHeaders.ACK_Mouvements): //_portSerie.ACK_ok();
                        break;
                    case ((byte)FrameHeaders.ACK_Xdent): //_portSerieModel.ACK_ok(); 
                        Messenger.Default.Send(e, "ACK_X_Dent");
                        break;
                    case ((byte)FrameHeaders.ACK_StopNv):
                        if (_nbrAckStop > 0)
                        {
                            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                if (_canExerciceSuivant)
                                {
                                    Messenger.Default.Send<bool>(false, "MainViewModel"); 
                                }
                            }), DispatcherPriority.Normal);
                            Debug.Print("Second ack stop : " + _nbrAckStop);
                            _nbrAckStop = 0;
                        }
                        else
                        {
                            Debug.Print("First ack stop : " + _nbrAckStop);
                            _nbrAckStop++;
                        }
                        break;
                    
                }
            }
        }

        void _portSerieModel_FrameConfigDataReceived(object sender, FrameConfigDataModel e)
        {
            Messenger.Default.Send(e, "NewUcConfig");
           
        }

        void _portSerieModel_aXdataReceived(object sender, aXdataModel e)
        {

        }

        public void _portSerieService_ErrorDataReceived(object sender, ErrorDataModel e)
        {
            //if (e != null)
            //{
            //    switch (e.Address)
            //    {
            //        case FrameHeaders.Error_HB:
            //        case FrameHeaders.Error_Emcy:
            //        case FrameHeaders.Error_SDO:
            //            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            //            {
            //                Messenger.Default.Send(e.GetMessage(),"AddNotif");     // arrêter l'evaluation
            //            }), DispatcherPriority.Background);
            //            break;
            //        case FrameHeaders.ACK_Recovery:
            //            this._messageBoxService.ShowError("Recovery !");
            //            break;
            //        default:
            //            break;
            //    }
            //}
            //this._messageBoxService.ShowError(e.GetMessage());
            if (e != null)
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                switch (e.Address)
                {
                    case FrameHeaders.Hardware: GestionErreur.GerrerErreur(new RobotHardwareException(e.NodeId, e.Address, e.ErrorCode, e.GetMessage()));
                        break;
                    case FrameHeaders.Software: GestionErreur.GerrerErreur(new RobotSoftwareException(e.NodeId, e.Address, e.ErrorCode, e.GetMessage()));
                        break;
                    default:
                        break;
                }
                Singleton.ChangeErrorStatu(true);
                Messenger.Default.Send<RobotErrorMessage>(new RobotErrorMessage { ErrorData = e }, "NewRobotError");
                OnRobotError(e);
                }), DispatcherPriority.DataBind);
            }
        }

        private void OnRobotError(ErrorDataModel e)
        {
            _startingSequenceTimer.Stop();
            StopNv();
        }

        /// <summary>
        /// Si on est toujours sur la visualisation restar l'exercice depuis le debut
        /// Dégrise les boutons
        /// </summary>
        private void RetablissementExerciceApresRobotErreur()
        {
            if (_currentExercice.Count > 0)
            {
                Messenger.Default.Send(_currentExercice, "NextExercice");
                TraitementExercice(_currentExercice);
                EnvoyerComCommand.RaiseCanExecuteChanged();
                EnvoyerNextComCommand.RaiseCanExecuteChanged();
                StopNVComCommand.RaiseCanExecuteChanged();
                PauseCommand.RaiseCanExecuteChanged();
            }
        }

        private void StartEx()
        {
            try
            {
                _startingSequenceTimer.Stop();
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    _isStarting = false;
                    StartingSequenceActivated = false;
                    _pause = true;
                    _canPause = true;
                    PauseCommand.RaiseCanExecuteChanged();
                    Messenger.Default.Send(true, "MainViewModel");
                }), DispatcherPriority.DataBind);
                
                infoSTOP = false;
                
                if(_isRestarting ==true)
                    _isRestarting = false;
                _axrobot.SendCommandFrame(CommandCodes.mod_suiv_traj);
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        // Specify what you want to happen when the Elapsed event is raised.
        private void OnStartingSequenceEvent(object source, ElapsedEventArgs e)
        {
            //empeche le timer de rester a 1 apres un retour a l'accueil
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                StartingSequence--;
                if (StartingSequence <= 0)
                {
                    ValReed.StartStop = true;
                    StartEx();
                }
            }), DispatcherPriority.Normal);
        }

        #endregion

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public override void Cleanup()
        {
            // Clean up if needed
            base.Cleanup();
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                _axrobot.Dispose();  // kill les Threads !!!
            }), DispatcherPriority.Normal);
        }
    }
}