using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using System.Windows.Threading;
using AxModel;
using AxAnalyse;
using AxAction;
using GalaSoft.MvvmLight.Ioc;

namespace AxViewModel
{
    public class AxAssistance
    {
        #region Fields

        private bool _firstKlongNeg;    // Activer la klong négative si patient bon et que klong est active
        private int Cycles;            // le nombres de mouvements
        private int _initiationCount;   // Nombres de passages dans l'initialisation
        private int _initiationMax;     // Nombres de passages Maximum dans l'initialisation
        private double _straigthness;   // Straightness
        private double _smoothness;     // Smoothness
        private bool _klatNeg;          // Activer / désactiver la klat négative
        private bool _klongNeg;         // Activer / désactiver la klong négative
        private bool _pause;            // Activer / désactiver la pause
        private bool _assistance;       // Activer / désactiver l'assistance
        private bool _assistKlat;       // Activer / désactiver l'assistance latérale
        private bool _assistKlong;      // Activer / désactiver l'assistance longitudinale
        private bool _detectStop;       // Activer / désactiver la détection d'arrêt
        private bool _initiation;       // Activer / désactiver l'initation du mouvement
        private bool _initFirst;        // Changement de la config du robot au premier passage de l'init
        private int Loop;               // boucle de l'assistance
        private int _detectStopTime;    // pour activer une fois par seconde
        private bool _firstKlong;       // premier passage dans l'assistance longitudinale
        private bool _actifKlat;
        private bool _klongOut;         // désactiver la klong derrière les balles

        private const double KLONG_TIME = 3.0;         // intervale de temps pour l'assistance longitudinale
        private const double KLAT2_TIME = 3.0;         // intervale de temps pour l'assistance latérale 2
        private const double KLONG_VALUE = 0.91;        // valeur pour l'assistance longitudinale
        private const double KLONG_VALUE_MIN = 0.91;
        private const double KLONG_VALUE_MAX = 1.09;
        private const double KLAT_VALUE_MIN = 0.94;     // valeur pour l'assistance laterale minimum
        private const double KLAT_VALUE_MAX = 1.06;     // valeur pour l'assistance longitudinale maximum
        private const int ASSIST_TIME = 5;              // 25hz
        private const double DATA_TIME = 0.04;          // 1/25 hz = 0.04s
        private const double KLAT2_DIST = 1.0;          // distance pour l'assistance latérale 2

        private const int KLAT_MIN = 1;
        private const int KLONG_MIN = 2;

        public List<DataPosition> TabVitesseMoy { get; set; }   // enregister position pour vitesse moyenne

        private List<DataPosition> TabPPrMoy { get; set; }   // enregister ppr pour ppr moyenne
        public List<DataPosition> TabPcMoy { get; set; }    // enregister pc pour ppr moyenne

        private List<DataPosition> _tabKlat;    // Tableau de stockage des positions pour l'assistance latérale
        private List<DataPosition> _tabKlong;   // Tableau de stockage des positions pour l'assistance longitudinale
        public ExerciceBaseConfig CurrentConfig;
        private ActionRobot pss;
        public bool Sens { get; set; }

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the AxAssistanceViewModel class.
        /// </summary>
        public AxAssistance()
        {
            this.pss = SimpleIoc.Default.GetInstance<ActionRobot>();
            Init();
        }
        #endregion


        /// <summary>
        /// Initialisation de l'assistance
        /// </summary>
        public void Init()
        {
            _tabKlong = new List<DataPosition>();
            _tabKlat = new List<DataPosition>();
            TabPcMoy = new List<DataPosition>();
            TabPPrMoy = new List<DataPosition>();
            _firstKlongNeg = false;
            TabVitesseMoy = new List<DataPosition>();
            _firstKlong = false;
            _actifKlat = false;
            CurrentConfig = new ExerciceBaseConfig();
            CurrentConfig.Vitesse = 5;
            EnvoyerNewVitesseConfigPort(5);
            CurrentConfig.RaideurLong = 1;
            CurrentConfig.RaideurLat = 1;
            Sens = true;
            Cycles = 0;     // RAZ Nbrs de cycles
            Loop = 0;
        }

        /// <summary>
        /// Assistance latérale
        /// </summary>
        public void KlatAssist(int a, double x, double y)
        {
            if (CurrentConfig.Vitesse != 0 && Sens == true)
            {
                if (a == 0)
                {
                    TabPcMoy.Add(new DataPosition(x, y));
                }
                else
                {
                    TabPPrMoy.Add(new DataPosition(x, y));

                    if (TabPPrMoy.Count >= (KLAT2_TIME * 2.0) && TabPcMoy.Count >= KLAT2_TIME)   // assistance latérale pdt l'exercice avec ppr
                    {
                        double MoyPpcPc;
                        MoyPpcPc = Ax_Position.DistancePprPc(TabPPrMoy, TabPcMoy);  // calc distance moyenne
                        if (MoyPpcPc > KLAT2_DIST || MoyPpcPc < -KLAT2_DIST)
                        {
                            if (CurrentConfig.RaideurLat <= 95) // vérifie si klat peut être augmenté
                            {
                                if (CurrentConfig.RaideurLat < 5)
                                {
                                    EnvoyerNewLatConfigPort(5 - CurrentConfig.RaideurLat);     // augmenter la klat
                                }
                                else
                                {
                                    EnvoyerNewLatConfigPort(5);     // augmenter la klat
                                }
                            }
                            //Debug.Print("klat2 +5");
                        }
                        else
                        {
                            if (CurrentConfig.RaideurLat >= 5)    // ajout de -5 pour la klat2assist
                            {
                                EnvoyerNewLatConfigPort(-5);     // diminuer la klat
                            }
                            //Debug.Print("klat2 -5");
                        }
                        TabPPrMoy.Clear();  // remise à zero du tableau
                        TabPcMoy.Clear();   // remise à zero du tableau
                    }
                }
            }
            else
            {
                if (CurrentConfig.RaideurLat > KLAT_MIN)    // ajout de -5 pour la klat2assist
                {
                    EnvoyerNewLatConfigPort(-5);     // diminuer la klat
                }
                //Debug.Print(a.ToString());
                //Debug.Print("klat2 --5");
            }
        }

        /// <summary>
        /// Assistance longitudinale
        /// </summary>
        private void KlongAssist(double x, double y)
        {
            if (CurrentConfig.Vitesse != 0 && Sens == true) // si la vitesse est différente de 0
            {
                _tabKlong.Add(new DataPosition(x, y));  // ajouter position

                if (_tabKlong.Count >= KLONG_TIME)
                {
                    double peakV = 0.0;
                    double resVit = 0.0;
                    double resVitRC = 0.0;

                    _firstKlong = true;

                    // Vitesse
                    double[] truc = new double[] { };
                    resVit = Ax_Vitesse.VitesseMoy(_tabKlong, ref peakV, ref truc, DATA_TIME);     // 1/25hz = 0.04s
                    //Debug.Print("Vmoy :" + resVit.ToString());

                    resVitRC = CurrentConfig.Vitesse / resVit;

                    //Debug.Print("resVitRC :" + resVitRC.ToString());
                    if (resVit < (CurrentConfig.Vitesse - 0.05))
                    {
                        if (CurrentConfig.RaideurLong <= 95)
                        {
                            if (CurrentConfig.RaideurLong < 5)
                            {
                                EnvoyerNewLongConfigPort(5 - CurrentConfig.RaideurLong);     // augmenter la klong
                            }
                            else
                            {
                                if (CurrentConfig.RaideurLong <= 95)
                                {
                                    EnvoyerNewLongConfigPort(5);     // augmenter la klong
                                }
                            }
                        }
                        //Debug.Print("klong +5");
                    }
                    else
                    {
                        if (CurrentConfig.RaideurLong >= 5)
                        {
                            EnvoyerNewLongConfigPort(-5);     // diminuer la klong
                        }
                        //Debug.Print("klong -5");
                    }
                    _tabKlong.Clear();
                }
                else
                {
                    //Debug.Print("klong OUT");
                }
            }
            else
            {
                if (CurrentConfig.RaideurLong >= 5)
                {
                    EnvoyerNewLongConfigPort(-5);     // diminuer la klong
                    //Debug.Print("klong --5");
                }
            }
        }

        /// <summary>
        /// Assistance
        /// </summary>
        public void AssistanceLoop(double x, double y)
        {
            Loop++;
            if (Loop == ASSIST_TIME)
            {
                KlongAssist(x, y);      // appel de l'assistance longitudinale
                KlatAssist(0, x, y);       // appel de l'assistance latérale
                Loop = 0;
            }
        }

        /// <summary>
        /// envoyer nouveau klat
        /// </summary>
        /// <param name="plus"></param>
        public void EnvoyerNewLatConfigPort(int plus)
        {
            FrameConfigDataModel newConfig = new FrameConfigDataModel();
            newConfig.Address = ConfigAddresses.KlatClong;
            CurrentConfig.RaideurLat = (short)(CurrentConfig.RaideurLat + plus);
            newConfig.Data3_4 = (short)(CurrentConfig.RaideurLong);
            if (CurrentConfig.RaideurLat <= 0)
            {
                CurrentConfig.RaideurLat = KLAT_MIN;
                newConfig.Data1_2 = (short)(KLAT_MIN);
            }
            else
            {
                newConfig.Data1_2 = (short)(CurrentConfig.RaideurLat);
            }

            this.pss.Pss.SendConfigFrame(newConfig);
            //Messenger.Default.Send(newConfig, "VisualisationViewModel");
            //Debug.Print("assist Lat.: {0} {1} {2}", CurrentConfig.RaideurLat, CurrentConfig.RaideurLong, CurrentConfig.Vitesse);
        }

        /// <summary>
        /// Envoyer nouveau klong
        /// </summary>
        /// <param name="plus"></param>
        private void EnvoyerNewLongConfigPort(int plus)
        {
            FrameConfigDataModel newConfig = new FrameConfigDataModel();
            newConfig.Address = ConfigAddresses.KlatClong;
            newConfig.Data1_2 = (short)(CurrentConfig.RaideurLat);
            CurrentConfig.RaideurLong += (short)plus; // SHORT TO BYTE
            if (CurrentConfig.RaideurLong <= 0)
            {
                CurrentConfig.RaideurLong = KLONG_MIN;
                newConfig.Data3_4 = (short)(KLONG_MIN);
            }
            else
            {
                newConfig.Data3_4 = (short)(CurrentConfig.RaideurLong);
            }
            this.pss.Pss.SendConfigFrame(newConfig);
            //Messenger.Default.Send(newConfig, "VisualisationViewModel");
            //Debug.Print("assist Long.: {0} {1} {2}", CurrentConfig.RaideurLat, CurrentConfig.RaideurLong, CurrentConfig.Vitesse);
        }

        /// <summary>
        /// envoyer nouvelle vitesse
        /// </summary>
        /// <param name="vitesse">valeur vitesse</param>
        public void EnvoyerNewVitesseConfigPort(short vitesse)
        {
            FrameConfigDataModel _configFrame2 = new FrameConfigDataModel();
            CurrentConfig.Vitesse = Convert.ToByte(vitesse); // SHORT TO BYTE
            _configFrame2.Address = ConfigAddresses.VitesseNbrsrep;
            _configFrame2.Data1_2 = CurrentConfig.Vitesse;
            _configFrame2.Data3_4 = 100;
            //Messenger.Default.Send(_configFrame2, "VisualisationViewModel");
            this.pss.Pss.SendConfigFrame(_configFrame2);
        }

        /// <summary>
        /// RAZ
        /// </summary>
        public void ClearAssistance()
        {
            StopP();
        }

        public void StopP()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Messenger.Default.Send(CommandCodes.STOPnv, "MessageCommand");
            }), DispatcherPriority.Normal);
        }
    }
}
