using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxModel;

namespace AxModelExercice
{
    public class Target : ExerciceMouvement
    {
        public double Precision { get; set; }
        public double EcartTypePre { get; set; }
        public double CVPrecision { get; set; }

        public Target(double precision, double ecartTypePre, double linearite, double ecartTypeLin, double vitesseMoy, double ecartTypeVMoy, double vitesseMax, double ecartTypeVMax, double speedMetric, double ecartTypeSM, double jerkMetric, double ecartTypeJM)
        {
            Precision = precision;
            EcartTypePre = ecartTypePre;

            Linearite = linearite;
            EcartTypeLin = ecartTypeLin;

            VitesseMoy = vitesseMoy;
            EcartTypeVMoy = ecartTypeVMoy;

            VitesseMax = vitesseMax;
            EcartTypeVMax = ecartTypeVMax;

            SpeedMetric = speedMetric;
            EcartTypeSM = ecartTypeSM;

            JerkMetric = jerkMetric;
            EcartTypeJM = ecartTypeJM;

            CVPrecision = returnCV(Precision, EcartTypePre);
            CVLinearite = returnCV(Linearite, EcartTypeLin);
            CVVitesseMoy = returnCV(VitesseMoy, EcartTypeVMoy);
            CVVitesseMax = returnCV(VitesseMax, EcartTypeVMax);
            CVSpeedMetric = returnCV(SpeedMetric, EcartTypeSM);
            CVJerkMetric = returnCV(JerkMetric, EcartTypeJM);
        }

        public Target(ExerciceBaseConfig baseConf, ExerciceBorneConfig borneConf,ThemeEvaluationModel theme)
            : base(baseConf,borneConf,theme)
        {
            this.TypeDroite = DroiteType.Vertical;
        }
        public Target(int cycleTot, double vitMoy, double[] tabVitMoy, double vitMax, double[] tabVitMax, double speedMet, double[] tabSpeedMet, double jerkMet, double[] tabJekMet,double line, double[] tabLine,double presi,double[] tabPresi)
            : base(cycleTot, vitMoy, tabVitMoy, vitMax, tabVitMax, speedMet, tabSpeedMet, jerkMet, tabJekMet,line,tabLine)
        {
            Precision = presi;
            EcartTypePre = EcartType(tabPresi, cycleTot);
            CVPrecision = returnCV(Precision, EcartTypePre);
        }

        public static double PresciTarget(List<DataPosition> posi)
        {
            // TODO : Distance aussi en X !!!
            double distance = 0.0;
            DataPosition dt = new DataPosition(posi.First().X, posi.First().Y);
            foreach (var el in posi)
            {
                if (el.Y < dt.Y)
                {
                    dt.Y = el.Y;
                }

                if (el.X < dt.X)
                {
                    dt.X = el.X;    // TODO : A CHANGER !
                }
            }
            distance = (DistancePythagorean(dt.X, dt.Y, 42.6, 44.2));
            return distance;
        }
        public static double CalAmpli(List<DataPosition> posi)
        {
            // TODO : Distance aussi en X !!!
            DataPosition depart = new DataPosition(posi.First().X, posi.First().Y);
            DataPosition best = new DataPosition(posi.First().X, posi.First().Y);

            foreach (var dp in posi)
            {
                if (dp.Y < best.Y)
                {
                    best.Y = dp.Y;
                    best.X = dp.X;
                }

                //if (dp.X < best.X)
                //{
                //    best.X = dp.X;
                //}
            }
            return DistancePythagorean(depart.X, depart.Y, best.X, best.Y);

        }
        public void DecoupeCycle(PositionDataModel positions, ref List<ExerciceEvaluation> exoEvalList, ref SingletonReeducation ValeurReeducation, ref List<DataPosition> tempData, ref bool dectectINIT)
        {
            dectectSTART = false;
            if (dectectSTOP == true)//Pour détecter quand le mouvement s'arrête
            {
                _detectionStop.Add(new DataPosition(positions.PositionX / 100.0, positions.PositionY / 100.0));
                if (_detectionStop.Count >= endPoints)
                {
                    DistanceTab(ref exoEvalList, ref ValeurReeducation, ref tempData);//Si les 50 derniers points sont compris dans un périmètre (default 0.8), on considère que le mouvement est terminé
                    _detectionStop.Clear();
                }
            }
            else
            {
                Init(positions, XINIT, YINITTARGET, DIST, ref tempData);
                longdroitetrou = true;
            }
        }
    }
}
