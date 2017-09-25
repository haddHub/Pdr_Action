using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxModel;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using AxAnalyse;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Messaging;


namespace AxModelExercice
{
    public enum ExerciceEvalTypes
    {
        Forme,
        Mouvement
    }
    public class ExerciceEvaluation : ExerciceGeneric
    {
      
        private const double DETECT_CYCLE = 63.06;   // DETECT_CYCLE

        public const double XINIT = 42.60;
        public const double YINITTARGET = 65.06;
        public const double YINITFREEAMP = 65.06;
        public const double YINITCERCLE = 65.06;
        public const double YINITCARRE = 65.06;
        public const double DISTFREEAMP = 0.2;
        public const double XCENTRE = 42.6;
        public const double YCENTRE = 65.06;
        public const double DIST = 0.5;

        #region fields
        public bool dectectINIT = false;
        double[] tabDoub = new double[1];
        private double[] tabVitMoy = new double[10];        // tableau de stockage pour la vitesse moyenne
        private double[] tabVitMax = new double[10];        // tableau de stockage pour la vitesse maximum
        private double[] tabJerkM = new double[10];         // tableau de stockage pour le jerk
        private double[] tabResultDist = new double[10];    // tableau de stockage pour la distance
        private double[] tabStra = new double[10];          // tableau de stockage pour la straightness
        private double[] tabSpeedMet = new double[10];      // tableau de stockage pour la vitesse
        private double[] tabCoordYMax = new double[10];     // tableau de stockage pour la coordonnée max en Y
        private double[] tabPresi = new double[10];         // tableau de stockage pour la précision des formes
        private int[] indice = new int[11];
        public int cycleInfo = 0;
        double VitMoy, resultDist, Stra, SpeedMet, JerkM, Ampli, Presi, VitMax;
        public bool _carre = false;
        public bool dectectSTART = false;
        public List<DataPosition> postTempData = new List<DataPosition>(); // Data pour traitement d'un cycle
        public int cycleTot = 0;
        public int endPoints = 50;
        public DataPosition initdetextstop;
        public bool dectectSTOP = false;
        public int countCycle = 0;
        public double _detectStopAmp = 0.8;
        public bool _notCibles = false;
        public bool posforce = false;
        double[] positionXYforceXY = new double[4];
        public string text, _filename;
        public bool longdroitetrou = false;
        public bool activOF = false;
        public bool zzzTime = false;
        public bool cycleActif = false;
        public List<DataPosition> _tabtempsMvtLongDroite = new List<DataPosition>();
        public List<DataPosition> _tabtempsMvtLongDroite1 = new List<DataPosition>();
        public List<DataPosition> _tabtempsMvtLongDroite2 = new List<DataPosition>();
        public List<DataPosition> _tabtempsMvtLongDroite3 = new List<DataPosition>();
        public List<DataPosition> _tabtempsMvtLongDroite4 = new List<DataPosition>();
        public List<DataPosition> _tabtempsMvtLongDroite5 = new List<DataPosition>();
        public List<DataPosition> _tabtempsMvtLongDroite6 = new List<DataPosition>();
        public List<DataPosition> _tabtempsMvtLongDroite7 = new List<DataPosition>();
        public List<DataPosition> _tabtempsMvtLongDroite8 = new List<DataPosition>();
        public List<DataPosition> _tabtempsMvtLongDroite9 = new List<DataPosition>();
        public List<DataPosition> _tabtempsMvtLongDroite10 = new List<DataPosition>();
        public double[,] tab10Index = new double[4, 10];    // debut fin valeur en X et Y
        public bool zzzIndex = false;
        public bool carreCycle = false;
        public DataPosition tempsMvtLongDroite = new DataPosition(42.6, 60.0);

        public double VitesseMoy { get; set; }
        public double EcartTypeVMoy { get; set; }

        public double VitesseMax { get; set; }
        public double EcartTypeVMax { get; set; }

        public double SpeedMetric { get; set; }
        public double EcartTypeSM { get; set; }

        public double JerkMetric { get; set; }
        public double EcartTypeJM { get; set; }
        
        public double CVVitesseMoy { get; set; }
        public double CVVitesseMax { get; set; }
        public double CVSpeedMetric { get; set; }
        public double CVJerkMetric { get; set; }

        public bool Enfant { get; set; }
        public ThemeEvaluationModel ThemeEnfant { get; set; }
        public ExerciceEvalTypes TypeEval { get; set; }
        public Collection<DataPosition> _detectionStop = new Collection<DataPosition>();
        // Define the tolerance for variation in their values 
        List<DataPosition> tempDataFilted = new List<DataPosition>();     // Data pour enregistrement pdt exo
        public double DetectStopAmp
        {
            get
            {
                return _detectStopAmp;
            }
            set
            {
                _detectStopAmp = value;
            }
        }
        public int CountCycle
        {
            get
            {
                return countCycle;
            }
            set
            {
                countCycle = value;
                if (countCycle >= 10)
                {
                    //StartEvaluation(false);
                    //Messenger.Default.Send(CommandCodes.mod_init_traj, "MessageCommand");
                }
                //RaisePropertyChanged("CountCycle");
            }
        }
        public string FileName
        {
            get
            {
                return _filename;
            }
            set
            {
                _filename = value;
            }
        }
        #endregion

        public double returnCV(double Moyenne, double EcartType)
        {
            double CV = EcartType / Moyenne;
            CV *= 100;
            return CV;
        }

        public ExerciceEvaluation(ExerciceBaseConfig baseConf, ExerciceBorneConfig borneConf, ThemeEvaluationModel theme)
            : base(baseConf,borneConf)
        {
            initdetextstop = new DataPosition(0, 0);         

            ThemeEnfant = theme;
            this.TypeExercice = ExerciceTypes.Evaluation;
        }
        public ExerciceEvaluation()
        {
            
            initdetextstop = new DataPosition(0, 0);

        }
        public ExerciceEvaluation(int cycleTot, double vitMoy, double[] tabVitMoy, double vitMax, double[] tabVitMax, double speedMet, double[] tabSpeedMet, double jerkMet, double[] tabJekMet)
        {
            initdetextstop = new DataPosition(0, 0);


            VitesseMoy = vitMoy;
            EcartTypeVMoy = EcartType(tabVitMoy, cycleTot);
            VitesseMax = vitMax;
            EcartTypeVMax = EcartType(tabVitMax, cycleTot);
            SpeedMetric = speedMet;
            EcartTypeSM = EcartType(tabSpeedMet, cycleTot);
            JerkMetric = jerkMet;
            EcartTypeJM = EcartType(tabJekMet, cycleTot);

            CVVitesseMoy = returnCV(VitesseMoy, EcartTypeVMoy);
            CVVitesseMax = returnCV(VitesseMax, EcartTypeVMax);
            CVSpeedMetric = returnCV(SpeedMetric, EcartTypeSM);
            CVJerkMetric = returnCV(JerkMetric, EcartTypeJM);
        }
        public ExerciceEvaluation(ExerciceGeneric gen)
        {
            initdetextstop = new DataPosition(0, 0);

            this.TypeExercice = ExerciceTypes.Evaluation;
            this.BaseConfig = gen.BaseConfig;
            this.BorneConfig = gen.BorneConfig;
        }

        #region Methodes d'analyse
        public double EcartType(double[] tab, int max)
        {
            double moyenne = Moyenne(tab, max);
            double somme = 0.0;
            for (int i = 0; i < max; i++)
            {
                double delta = tab[i] - moyenne;
                somme += delta * delta;
            }
            return Math.Sqrt(somme / (max - 1));
        }
        private double Moyenne(double[] tab, int max)
        {
            int length = max;
            double somme = 0.0;
            for (int i = 0; i < length; i++)
            {
                somme += tab[i];
            }
            return somme / length;
        }
        #endregion



        public bool DistanceTab(ref List<ExerciceEvaluation> exoEvalList, ref SingletonReeducation ValeurReeducation, ref List<DataPosition> tempData)
        {
            double moyX = 0.0;
            double moyY = 0.0;
            double distance = 0.0;
            for (int i = 0; i < _detectionStop.Count; i++)
            {
                moyX += _detectionStop[i].X;
                moyY += _detectionStop[i].Y;
            }
            moyX /= _detectionStop.Count;
            moyY /= _detectionStop.Count;
            distance = Distance(moyX, moyY, _detectionStop[_detectionStop.Count - 1].X, _detectionStop[_detectionStop.Count - 1].Y);
            //distance /= 100.0;
            if (distance < DetectStopAmp)   // default : 0.8
            {
                Debug.Print(Convert.ToString(distance));

        
                Messenger.Default.Send<bool>(false, "MainViewModel");
                CountCycle++;
                Messenger.Default.Send(CountCycle, "CompteurCycle");
                if (CountCycle < 10)    // TODO : ici pour changer
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        Messenger.Default.Send(CommandCodes.mod_init_traj, "MessageCommand");
                    }), DispatcherPriority.Normal);

                    _carre = true;
                    exoEvalList[0].dectectSTART = false;
                    ValeurReeducation.StartStop = false;
                    postTempData.Clear();
                    for (int i = 0; i < tempData.Count; i++)
                    {
                        postTempData.Add(new DataPosition(tempData[i].X, tempData[i].Y));
                    }
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        Messenger.Default.Send(CommandCodes.mod_init_traj, "MessageCommand");
                    }), DispatcherPriority.Normal);

                    exoEvalList[0].dectectSTART = false;
                    _carre = false;
                    cycleTot = CountCycle;
                    ValeurReeducation.StartStop = false;
                    postTempData.Clear();
                    for (int i = 0; i < tempData.Count; i++)
                    {
                        postTempData.Add(new DataPosition(tempData[i].X, tempData[i].Y));
                    }
                    CountCycle = 0;
                    Messenger.Default.Send(CountCycle, "CompteurCycle");
                }
            }
            return true;
        }


        public bool DistanceInitConf(double x, double y, double dist, ref List<DataPosition> tempData)
        {
            double distance = 0.0;
            distance = Distance(x, y, initdetextstop.X, initdetextstop.Y);
            if (distance > dist)
            {
                Debug.Print("start" + Convert.ToString(distance));
                dectectSTOP = true;
                dectectINIT = false;
                if (_notCibles == false)
                {
                    dectectSTART = false;
                }
                else
                {
                    dectectSTART = true;
                }
                tempData.Clear();
            }
            return true;
        }

        public double Distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((y2 - y1), 2.0) + Math.Pow((x2 - x1), 2.0));
        }

        public void AddTxtLine(double fX, double fY, double pX, double pY)
        {
            if (posforce == true)
            {
                posforce = false;
            }
            // The using statement automatically closes the stream and calls  
            // IDisposable.Dispose on the stream object. 
            CultureInfo invC = new CultureInfo("en-US");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Directory.GetCurrentDirectory() + "\\test.txt", true))
            {
                file.WriteLine(string.Format(invC, "{0:N2}", fX) + " " + string.Format(invC, "{0:N2}", fY) + " " + string.Format(invC, "{0:F2}", pX) + " " + string.Format(invC, "{0:F2}", pY));
            }
        }

        public void AddNewLine(double[] positionXYforceXY) // public void AddNewLine(String text)
        {
            //if (test == false)
            //{
            if (posforce == true)
            {
                posforce = false;
            }
            //text = Convert.ToString(string.Format("{0:N2}", positionXYforceXY[0]) + " " + string.Format("{0:N2}", positionXYforceXY[1]) + " " + string.Format("{0:0.00}", (positionXYforceXY[2] / 100.0)) + " " + string.Format("{0:0.00}", (positionXYforceXY[3] / 100.0)) + " ");
            text = Convert.ToString(string.Format("{0:F2}", positionXYforceXY[0]) + " " + string.Format("{0:F2}", positionXYforceXY[1]) + " " + string.Format("{0:F2}", (positionXYforceXY[2] / 100.0)) + " " + string.Format("{0:F2}", (positionXYforceXY[3] / 100.0)) + " ");
            File.AppendAllText(FileName, text + Environment.NewLine);
            // }
        }

        public void CollecteDonneesBrutes(PositionDataModel position)
        {
            positionXYforceXY[2] = position.PositionX;
            positionXYforceXY[3] = position.PositionY;

            if (activOF == true)
            {
                AddNewLine(positionXYforceXY);
            }
        }

        public void DecoupageCycle(PositionDataModel positions, ref List<ExerciceEvaluation> exoEvalList, ref SingletonReeducation ValeurReeducation, ref List<DataPosition> tempData)
        {

            DetectionCycle(positions, exoEvalList, ref ValeurReeducation, ref tempData);

                if (longdroitetrou == true)
                {
                    EnregistrerCycle(positions, ref ValeurReeducation, ref tempData);
                }

                 //TODO : il faudra changer ca pour le démarrer après la zone de confort !
                if (dectectSTART == false)
                {
                    tempData.Add(new DataPosition(positions.PositionX / 100.0, positions.PositionY / 100.0)); // TODO : enregistrement data
                    AddTxtLine(positionXYforceXY[0], positionXYforceXY[1], positionXYforceXY[2] / 100.0, positionXYforceXY[3] / 100.0);
                }

        }

        private void EnregistrerCycle(PositionDataModel positions, ref SingletonReeducation ValeurReeducation, ref List<DataPosition> tempData)
        {
            if ((positions.PositionY / 100.0 < DETECT_CYCLE))
            {
                zzzTime = true;
            }

            if ((positions.PositionY / 100.0 >= DETECT_CYCLE) && (positions.PositionY / 100.0 > tempsMvtLongDroite.Y) && zzzTime == true)//En Cycle
            {
                _tabtempsMvtLongDroite.Add(new DataPosition(positions.PositionX / 100.0, positions.PositionY / 100.0));
                if (zzzIndex == false)
                {
                    tab10Index[0, CountCycle] = tempData.Count - 1;   // enregistrement index debut
                    zzzIndex = true;
                }
                cycleActif = true;
            }
            else if ((positions.PositionY / 100.0 < DETECT_CYCLE) && (positions.PositionY / 100.0 < tempsMvtLongDroite.Y) && cycleActif == true)//Fin de cycle
            {
                if (_tabtempsMvtLongDroite.Count != 0)
                {
                    DataPosition min = new DataPosition();
                    min.X = _tabtempsMvtLongDroite[0].X;
                    min.Y = _tabtempsMvtLongDroite[0].Y;

                    if (carreCycle == false)
                    {
                        for (int i = 0; i < _tabtempsMvtLongDroite.Count; i++)
                        {
                            if (_tabtempsMvtLongDroite[i].Y > min.Y)
                            {
                                min.X = _tabtempsMvtLongDroite[i].X;
                                min.Y = _tabtempsMvtLongDroite[i].Y;
                            }
                        }
                    }

                    _tabtempsMvtLongDroite.Clear();                 // clear tableau de detection de fin de cycle
                    tab10Index[1, CountCycle] = tempData.Count - 1;    // enregistrement index fin
                    tab10Index[2, CountCycle] = min.X;    // enregistrement index fin
                    tab10Index[3, CountCycle] = min.Y;    // enregistrement index fin

                    zzzIndex = false;

                    CountCycle++;       // incrementation compteur de cycle
                    Messenger.Default.Send(CountCycle, "CompteurCycle");
                    if (CountCycle >= 10)   // TODO : ici pour changer
                    {
                        ValeurReeducation.StartStop = false;        // activer le post traitement des données
                        longdroitetrou = false;
                        cycleTot = CountCycle;
                        Messenger.Default.Send(CountCycle, "CompteurCycle");
                        CountCycle = 0; // RAZ du compteur de cycle
                        Messenger.Default.Send(CountCycle, "CompteurCycle");
                        for (int i = 0; i < tempData.Count; i++)
                        {
                            postTempData.Add(new DataPosition(tempData[i].X, tempData[i].Y));
                        }
                    }
                    cycleActif = false;
                }
            }
            longdroitetrou = false;
            tempsMvtLongDroite.X = positions.PositionX / 100.0;
            tempsMvtLongDroite.Y = positions.PositionY / 100.0;
        }

        private void DetectionCycle(PositionDataModel positions, List<ExerciceEvaluation> exoEvalList, ref SingletonReeducation ValeurReeducation, ref List<DataPosition> tempData)
        {
            if (exoEvalList[0].TypeEval == ExerciceEvalTypes.Mouvement)
            {
                ExerciceMouvement ef = (ExerciceMouvement)exoEvalList[0];
                if (ef.TypeDroite == DroiteType.Vertical)//Target
                {
                    Target ExMvtsRythmiques = (Target)exoEvalList[0];
                    ExMvtsRythmiques.DecoupeCycle(positions, ref exoEvalList, ref ValeurReeducation, ref tempData, ref dectectINIT);
                }

                else if (ef.TypeDroite == DroiteType.VerticalLong)//FreeAmplitude
                {
                    Init(positions, XINIT, YINITFREEAMP, DISTFREEAMP, ref tempData);
                    longdroitetrou = true;
                }
            }
            else if (exoEvalList[0].TypeEval == ExerciceEvalTypes.Forme)//Ex de type Forme
            {
                ExerciceForme exF = (ExerciceForme)exoEvalList[0];
                if (exF.TypeForme == FormeType.Cercle)
                {
                    Init(positions, XINIT, YINITCERCLE, DIST, ref tempData);
                    longdroitetrou = true;
                }
                else if (exF.TypeForme == FormeType.Carré)
                {
                    Init(positions, XINIT, YINITCARRE, DIST, ref tempData);
                    longdroitetrou = true;
                    carreCycle = true;
                }
            }
        }

        public void Init(PositionDataModel positions, double x, double y, double dist, ref List<DataPosition> tempData)
        {
            if (dectectINIT == true)
            {
                initdetextstop.X = positions.PositionX / 100.0;
                initdetextstop.Y = positions.PositionY / 100.0;
                _notCibles = false;
                DistanceInitConf(x, y, dist, ref tempData);
            }
        }

        public void DataAfterEvaluation(ref List<ExerciceEvaluation> exoEvalList, ref ExerciceEvaluation exo, ref SingletonReeducation ValeurReeducation, ref List<DataPosition> tempData)
        {
            tempDataFilted = new List<DataPosition>();
            if (tempData.Count != 0)
            {
                
                VitMoy = 0.0;
                resultDist = 0.0;
                Stra = 0.0;
                SpeedMet = 0.0;
                JerkM = 0.0;
                Ampli = 0.0;
                Presi = 0.0;
                VitMax = 0.0;

                //Filtres
                tempDataFilted = FiltreSurDonnees(exoEvalList, tempDataFilted, ref ValeurReeducation, ref tempData);

                //Calculs

                Calculs(exoEvalList, tempDataFilted, ref ValeurReeducation);

                if (cycleInfo == cycleTot)  // Vérifier si la moyenne est complète + RAZ pour next cycle
                {
                    CalculMoyennes(); //On calcule les moyennes

                    if (exoEvalList[0].TypeExercice == ExerciceTypes.Evaluation)
                    {
                        if (exoEvalList[0].TypeEval == ExerciceEvalTypes.Forme)
                        {
                            ExerciceForme ef = (ExerciceForme)exoEvalList[0];
                            if(ef.TypeForme == FormeType.Carré)
                                exo = new Square(cycleTot, VitMoy, tabVitMoy, VitMax, tabVitMax, SpeedMet, tabSpeedMet, JerkM, tabJerkM, Presi, tabPresi);

                            else
                            exo = new Circle(cycleTot, VitMoy, tabVitMoy, VitMax, tabVitMax, SpeedMet, tabSpeedMet, JerkM, tabJerkM, Presi, tabPresi);
                        }

                        else
                        {
                            ExerciceMouvement ef = (ExerciceMouvement)exoEvalList[0];
                            if (ef.TypeDroite == DroiteType.VerticalLong)//FreeAmpl
                                exo = new FreeAmplitude(cycleTot, VitMoy, tabVitMoy, VitMax, tabVitMax, SpeedMet, tabSpeedMet, JerkM, tabJerkM, Stra, tabStra, Ampli, tabCoordYMax);
                            else
                                exo = new Target(cycleTot, VitMoy, tabVitMoy, VitMax, tabVitMax, SpeedMet, tabSpeedMet, JerkM, tabJerkM, Stra, tabStra, Presi, tabPresi);
                        }
                    }
                    CleanCycle(ref tempData); //On clean le cycle
                }
            }
        }

        private void Calculs(List<ExerciceEvaluation> exoEvalList, List<DataPosition> tempDataFilted, ref SingletonReeducation ValeurReeducation)
        {
            //Calculs
            tabVitMoy[cycleInfo] = 0.0;//Ax_Vitesse.VitesseMoy(tempDataFilted, ref VitMax, 0.008);    // Calc de la vitesse moyenne
            tabVitMax[cycleInfo] = VitMax;
            tabJerkM[cycleInfo] = Ax_Vitesse.JerkMet(tempDataFilted, VitMax, 0.008);            // Calc du Jerk
            tabResultDist[cycleInfo] = Ax_Position.Distance(tempDataFilted);                    // Calc de la distance réelle
            tabDoub[0] = tabResultDist[cycleInfo];

            double ampStrain = 0.0;
            double coordYMax = 0.0;
            if (exoEvalList[0].TypeEval == ExerciceEvalTypes.Mouvement)
            {
                ExerciceMouvement ef1 = (ExerciceMouvement)exoEvalList[0];
                if (ef1.TypeDroite == DroiteType.VerticalLong)//FreeAmplitude
                {
                    ampStrain = FreeAmplitude.CalAmpliFree(tempDataFilted, ref coordYMax);            // special aller/retour :p
                    tabCoordYMax[cycleInfo] = coordYMax;
                    tabStra[cycleInfo] = Ax_Position.Straightness(ampStrain, tabDoub);          // Calc de la Straightness
                }
                else if (ef1.TypeDroite == DroiteType.Vertical)
                {
                    ampStrain = Target.CalAmpli(tempDataFilted);                   // Calc de l'amplitude
                    tabStra[cycleInfo] = Ax_Position.Straightness(ampStrain, tabDoub);          // Calc de la Straightness
                    tabPresi[cycleInfo] = Target.PresciTarget(tempDataFilted);             // calc precision target droite
                }
            }
            else if (exoEvalList[0].TypeEval == ExerciceEvalTypes.Forme)
            {
                ExerciceForme ef = (ExerciceForme)exoEvalList[0];
                if (ef.TypeForme == FormeType.Cercle)
                {
                    DataPosition centre = new DataPosition(XCENTRE, YCENTRE); // 37.0 46.0
                    double Rayon = 4.0;
                    tabPresi[cycleInfo] = Circle.PreciCercle(tempDataFilted, centre, Rayon);
                }
                else
                {
                    DataPosition centre = new DataPosition(XCENTRE, YCENTRE);
                    double longCot = 6.0, Orientation = 0.0;
                    tabPresi[cycleInfo] = Square.PreciCarre(tempDataFilted, centre, longCot, Orientation);
                }
            }

            tabSpeedMet[cycleInfo] = tabVitMoy[cycleInfo] / tabVitMax[cycleInfo];               // Calc de la Speed Metrique
            cycleInfo++;
        }

        private List<DataPosition> FiltreSurDonnees(List<ExerciceEvaluation> exoEvalList, List<DataPosition> tempDataFilted, ref SingletonReeducation ValeurReeducation, ref List<DataPosition> tempData)
        {
                //Filtres sur les données en entrées
                if (exoEvalList[0].TypeEval == ExerciceEvalTypes.Mouvement)
                {
                    ExerciceMouvement ef = (ExerciceMouvement)exoEvalList[0];
                    if (ef.TypeDroite == DroiteType.Vertical)//target
                    {
                        tempDataFilted = Ax_Filter.Filtre125Hz(postTempData);
                    }
                    else
                        TraitementFiltre(ref tempDataFilted);
                }
                else
                    TraitementFiltre(ref tempDataFilted);

                //On supprime les positions en trop
                tempDataFilted.RemoveRange(0, 10);
                if (exoEvalList[0].TypeEval == ExerciceEvalTypes.Mouvement)
                {
                    ExerciceMouvement ef = (ExerciceMouvement)exoEvalList[0];
                    if (ef.TypeDroite == DroiteType.Vertical)
                    {
                        if (tempDataFilted.Count >= 50)
                        {
                            tempDataFilted.RemoveRange((tempDataFilted.Count - 50), 50);
                        }
                        ValeurReeducation.StartStop = true;
                        tempData.Clear();
                    }
                    else
                    {
                        tempDataFilted.RemoveRange((tempDataFilted.Count - 10), 10);
                    }
                }
                else
                {
                    tempDataFilted.RemoveRange((tempDataFilted.Count - 10), 10);
                } 
            return tempDataFilted;
        }

        private void CalculMoyennes()
        {
            // TODO : faire la moyenne
            for (int k = 0; k < cycleTot; k++)  // préparer les moyennes
            {
                VitMoy += tabVitMoy[k];
                VitMax += tabVitMax[k];
                JerkM += tabJerkM[k];
                resultDist += tabResultDist[k];
                SpeedMet += tabSpeedMet[k];
                Stra += tabStra[k];
                Ampli += tabCoordYMax[k];
                Presi += tabPresi[k];
            }

            VitMoy /= cycleTot;         // calculer les moyennes
            VitMax /= cycleTot;
            JerkM /= cycleTot;
            resultDist /= cycleTot;
            SpeedMet /= cycleTot;
            Stra /= cycleTot;
            Ampli /= cycleTot;
            Presi /= cycleTot;
        }

        public void CleanCycle(ref List<DataPosition> tempData)
        {
            postTempData.Clear();
            tempData.Clear();
            cycleInfo = 0;
            cycleTot = 0;   // RAZ pour l'evaluation suivante
            CountCycle = 0;
            //tempDataFilted.Clear();
            Messenger.Default.Send(CountCycle, "CompteurCycle");
        }

        private void TraitementFiltre(ref List<DataPosition> tempDataFilted)
        {
                int i;
                double difference = Math.Abs(postTempData[(int)tab10Index[0, cycleInfo]].X * .00001);
                double difference2 = Math.Abs(postTempData[(int)tab10Index[0, cycleInfo]].X * .00001);

                for (i = (int)tab10Index[0, cycleInfo]; i <= tab10Index[1, cycleInfo]; i++)
                {
                    if ((Math.Abs(postTempData[i].X - tab10Index[2, cycleInfo]) <= Math.Abs(postTempData[i].X * .00001))
                        && (Math.Abs(postTempData[i].Y - tab10Index[3, cycleInfo]) <= Math.Abs(postTempData[i].X * .00001)))
                    {
                        indice[cycleInfo + 1] = i;        // sauvegarde de l'indice
                    }
                }

                List<DataPosition> postTempDataCoupe = new List<DataPosition>();            // Data pour traitement d'un cycle

                for (int j = indice[cycleInfo]; j <= indice[cycleInfo + 1]; j++)
                {
                    postTempDataCoupe.Add(new DataPosition(postTempData[j].X, postTempData[j].Y));
                }

                tempDataFilted = Ax_Filter.Filtre125Hz(postTempDataCoupe);                  // Filtre sur les données en entrée !!! 
            }
    }
}
