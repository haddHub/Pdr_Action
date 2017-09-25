using GalaSoft.MvvmLight;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows;
using System.Windows.Threading;
using System;
using GalaSoft.MvvmLight.Messaging;
using AxCommunication;
using AxModel;
using AxModel.Helpers;
using AxModelExercice;
using AxAction;
using System.Collections.Generic;

namespace AxViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm/getstarted
    /// </para>
    /// </summary>
    public class MainPViewModel : ViewModelBase
    {
        #region Fields
        
        object locker = new object();

        private readonly ActionRobot _axrobot;
        private readonly IMessageBoxService _messageBoxService;

        private PointCollection _positionRobotLive;     // Collection de points pour le traçage à l'écran de la Polyline
        private PointCollection _position2RobotLive;    // Collection de points pour le traçage à l'écran de la Polyline

        private PositionDataModel _positionUiBuffer, _position2UiBuffer;    // Buffer pour l'affichage des positions

        private PolylineConfig _styleRobotLive, _styleRobotLive2;   // Configuration de l'affichage des lignes et points à l'écran

        private System.Timers.Timer _mainLoop;      // Timer pour l'affichage @50hz

        private string _fondUi;

        private ExercicePoulies _exoPoulies;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the MainPViewModel class.
        /// </summary>
        public MainPViewModel(ActionRobot _axrob, IMessageBoxService _mbs)
        {
            _messageBoxService = _mbs;
            _axrobot = _axrob;

            FondUi = "Resources\\Image\\Background\\Axi_FondEcran_T.png";

            _positionUiBuffer = new PositionDataModel();
            _position2UiBuffer = new PositionDataModel();

            this._mainLoop = new System.Timers.Timer();
            _mainLoop = new System.Timers.Timer(20);      // Create a timer with a twenty second interval. 50Hz
            _mainLoop.Elapsed += new System.Timers.ElapsedEventHandler(OnMainLoopSequenceEvent);    // Hook up the Elapsed event for the timer.
            _mainLoop.Enabled = true;

            _positionRobotLive = new PointCollection();
            _position2RobotLive = new PointCollection();

            _styleRobotLive = new PolylineConfig(false, 5, "Continu", Brushes.Orange);//7.5
            _styleRobotLive2 = new PolylineConfig(false, 5, "Continu", Brushes.Orange);//7.5

            _axrobot.Pss.PositionDataReceived += new onPositionDataReceived(_portSerieService_PositionDataReceived);
            _axrobot.Pss.Position2DataReceived += new onPosition2DataReceived(_portSerieService_Position2DataReceived);

            
            CreateMessages();

            Debug.Print("MainPViewModel OK");
        }
        #endregion

        #region Properties

        public PointCollection PositionRobotLive
        {
            get
            {
                return _positionRobotLive.Clone();  // create a new instance of PointCollection for binding
            }
            set
            {
                _positionRobotLive = value;
            }
        }

        public PointCollection Position2RobotLive
        {
            get
            {
                return _position2RobotLive.Clone();  // create a new instance of PointCollection for binding
            }
            set
            {
                _position2RobotLive = value;
            }
        }


        public PolylineConfig StyleRobotLive
        {
            get
            {
                return _styleRobotLive;
            }
            set
            {
                _styleRobotLive = value;
                RaisePropertyChanged("StyleRobotLive");
            }
        }

        public PolylineConfig StyleRobotLive2
        {
            get
            {
                return _styleRobotLive2;
            }
            set
            {
                _styleRobotLive2 = value;
                RaisePropertyChanged("StyleRobotLive2");
            }
        }

               
        public string FondUi
        {
            get { return _fondUi; }
            set 
            {
                _fondUi = value;
                RaisePropertyChanged("FondUi");
            }
        }

        public ExercicePoulies ExoPoulies
        {
            get { return _exoPoulies; }
            set 
            { 
                _exoPoulies = value;
                RaisePropertyChanged("ExoPoulies");
            }
        }

        private bool _pause;

        public bool Pause
        {
            get { return _pause; }
            set 
            {
                _pause = value;
                RaisePropertyChanged("Pause");
            }
        }

        private int _tempsPause;

        public int TempsPause
        {
            get { return _tempsPause; }
            set 
            {
                _tempsPause = value;
                RaisePropertyChanged("TempsPause");
            }
        }

        private int _score;

        public int Score
        {
            get { return _score; }
            set 
            {
                _score = value;
                RaisePropertyChanged("Score");
            }
        }

        private int _highScore;

        public int HighScore
        {
            get { return _highScore; }
            set 
            {
                _highScore = value;
                RaisePropertyChanged("HighScore");
            }
        }


        #endregion

        #region Methods

        private void CreateMessages()
        {
            Messenger.Default.Register<List<ExerciceGeneric>>(this, "NextExercice", FentreUpdateExercicesMessage);
            Messenger.Default.Register<List<ExerciceGeneric>>(this, "EvaluationViewModel", FentreUpdateExercicesMessage);// abonnement aux messages envoyé par Evaluation pour envoyer au µc
            Messenger.Default.Register<double[]>(this, "FaireMonterPlateau", FaireMonterPlateau);
            Messenger.Default.Register<double>(this, "FaireTournerPlateau", FaireTournerPlateau);
            Messenger.Default.Register<int[]>(this, "PoulieEnPauseGui", PouliesEnPause);
            Messenger.Default.Register<bool>(this, "ClearGui", ClearUI);
        }

        private void ClearUI(bool b)
        {
            this.ExoPoulies.PoulieGauche.Points.Clear();
            this.ExoPoulies.PoulieDroite.Points.Clear();
            _positionRobotLive.Clear();
            _position2RobotLive.Clear();
            StyleRobotLive.Visibility = false;
            StyleRobotLive2.Visibility = false;
            FondUi = "Resources\\Image\\Background\\Axi_FondEcran_T.png";
            RaisePropertyChanged("PositionRobotLive");
            RaisePropertyChanged("PositionRobotBase");
            RaisePropertyChanged("PositionRobotLive2");
            RaisePropertyChanged("PositionRobotBase2");
            RaisePropertyChanged("StyleRobotLive");
            RaisePropertyChanged("StyleRobotLive2");
        }

        public int MaFonct(int j)
        {
            j++;
            return j;
        }

        #endregion

        #region RelayCommand

        #endregion

        #region Actions

        void _portSerieService_PositionDataReceived(object sender, PositionDataModel e)
        {
            //lock (locker)
            //{
            _positionUiBuffer.PositionX = (int)EchelleUtils.MiseEchelleXPosition(e.PositionX);
            _positionUiBuffer.PositionY = (int)EchelleUtils.MiseEchelleYPosition(e.PositionY);
            //}
        }

        void _portSerieService_Position2DataReceived(object sender, Position2DataModel e)
        {
            //lock (locker)
            //{
            _position2UiBuffer.PositionX = (int)EchelleUtils.MiseEchelleXPosition2(e.PositionX);
            _position2UiBuffer.PositionY = (int)EchelleUtils.MiseEchelleYPosition2(e.PositionY);
            //}
        }

        // Specify what you want to happen when the Elapsed event is raised.
        private void OnMainLoopSequenceEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            if (Application.Current != null)    // pour éviter l'exception null...
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => // on UI Thread
                {
                    _positionRobotLive.Add(new Point(_positionUiBuffer.PositionX, _positionUiBuffer.PositionY));
                }), DispatcherPriority.Normal);

                Application.Current.Dispatcher.BeginInvoke(new Action(() => // on UI Thread
                {
                    if (PositionRobotLive.Count >= 100)
                    {
                        _positionRobotLive.RemoveAt(0);
                    }
                }), DispatcherPriority.Normal);
            }

            RaisePropertyChanged("PositionRobotLive");

            if (Application.Current != null)    // pour éviter l'exception null...
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => // on UI Thread
                {
                    _position2RobotLive.Add(new Point(_position2UiBuffer.PositionX, _position2UiBuffer.PositionY));
                }), DispatcherPriority.Normal);

                Application.Current.Dispatcher.BeginInvoke(new Action(() => // on UI Thread
                {
                    if (Position2RobotLive.Count >= 100)
                    {
                        _position2RobotLive.RemoveAt(0);
                    }
                }), DispatcherPriority.Normal);
            }

            RaisePropertyChanged("Position2RobotLive");
        }

        public void FentreUpdateExercicesMessage(List<ExerciceGeneric> exercice)
        {
            StyleRobotLive.Visibility = true;
            StyleRobotLive2.Visibility = true;
            this.ExoPoulies = exercice[0] as ExercicePoulies;
            if (this.ExoPoulies.ThemeEnfant.Fond.Remove(0, 28) == "ParDefaut.jpg")//theme enfnat
            {
                FondUi = "../../Theme/Evaluation/Fond/background_blank.png";
            }
                       
            RaisePropertyChanged("PositionRobotBase");
            RaisePropertyChanged("Position2RobotBase");
            RaisePropertyChanged("StyleRobotLive");
            RaisePropertyChanged("StyleRobotLive2");
        }

        /// <summary>
        /// Monte et tourne le plateau si besoin
        /// </summary>
        /// <param name="vitesseMains">Tableau des vitesse de chaque main. [0] = gauche [1] = droite</param>
        private void FaireMonterPlateau(double[] vitesseMains)
        {
            this.ExoPoulies.MonterPlateau(vitesseMains[0], vitesseMains[1]);
        }

        private void FaireTournerPlateau(double angle)
        {
            this.ExoPoulies.TournerPlateau(angle);
        }

        private void PouliesEnPause(int[] p)
        {
            // temps de pause = -1 veut dire que il n'y a pas de pause automatique. C'est le thérapeute qui relance l'exercice.
            //Le score doit quand même etre affiché.
            if (p[0] > 0 || p[0] == -1)
            {
                Pause = true;

                if (p[0] != -1)
                    TempsPause = p[0];
                else
                    TempsPause = 0;

                Score = p[1];

                HighScore = p[2];
            }
            else
                Pause = false;
        }
        #endregion
    }
}