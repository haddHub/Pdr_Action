using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxModel;

namespace AxModelExercice
{
    public class Circle : ExerciceForme
    {
        //a supp
        public Circle(double shapeAccuracy, double ecartTypeSA, double vitesseMoy, double ecartTypeVMoy, double vitesseMax, double ecartTypeVMax, double speedMetric, double ecartTypeSM, double jerkMetric, double ecartTypeJM)
        {
            ShapeAccuracy = shapeAccuracy;
            EcartTypeSA = ecartTypeSA;

            VitesseMoy = vitesseMoy;
            EcartTypeVMoy = ecartTypeVMoy;

            VitesseMax = vitesseMax;
            EcartTypeVMax = ecartTypeVMax;

            SpeedMetric = speedMetric;
            EcartTypeSM = ecartTypeSM;

            JerkMetric = jerkMetric;
            EcartTypeJM = ecartTypeJM;

            CVShapeAccuracy = returnCV(ShapeAccuracy, EcartTypeSA);
            CVVitesseMoy = returnCV(VitesseMoy, EcartTypeVMoy);
            CVVitesseMax = returnCV(VitesseMax, EcartTypeVMax);
            CVSpeedMetric = returnCV(SpeedMetric, EcartTypeSM);
            CVJerkMetric = returnCV(JerkMetric, EcartTypeJM);
        }

        public Circle(ExerciceBaseConfig baseConf, ExerciceBorneConfig borneConf,ThemeEvaluationModel theme)
            : base(baseConf,borneConf,theme)
        {
            this.NbrPolygone = 40;
            this.TypeForme = FormeType.Cercle;
        }

        public Circle(int cycleTot, double vitMoy, double[] tabVitMoy, double vitMax, double[] tabVitMax, double speedMet, double[] tabSpeedMet, double jerkMet, double[] tabJekMet,double presi, double[] tabPresi)
            :base(cycleTot,vitMoy,tabVitMoy,vitMax,tabVitMax,speedMet,tabSpeedMet,jerkMet,tabJekMet,presi,tabPresi)
        {

        }
        public Circle()
        {

        }
        public static double PreciCercle(List<DataPosition> Posi, DataPosition CentreCercle, double RayonCercle)
        {
            double Preci = 0.0;
            List<DataPosition> PosiProj = new List<DataPosition>();
            List<double> ListeDist = new List<double>();

            for (int dp = 0; dp < Posi.Count; dp++)
            {
                double vccpc_X = Posi[dp].X - CentreCercle.X;
                double vccpc_Y = Posi[dp].Y - CentreCercle.Y;

                double alpha = RayonCercle / Math.Sqrt(Math.Pow(vccpc_X, 2) + Math.Pow(vccpc_Y, 2));

                PosiProj.Add(new DataPosition(CentreCercle.X + alpha * vccpc_X, CentreCercle.Y + alpha * vccpc_Y));

                ListeDist.Add(Math.Sqrt(Math.Pow((PosiProj[dp].X - Posi[dp].X), 2) + Math.Pow((PosiProj[dp].Y - Posi[dp].Y), 2)));
            }

            //m < Compteur (qui ici est 1)
            //for (int m = 0; m < 1; m++)
            //{
            //    double distance_moyenne_cycle = 0.0;
            //    int PointDep = 1;
            //    distance_moyenne_cycle = distance_moyenne_cycle + ListeDist[PointDep - 1];
            //    Preci += distance_moyenne_cycle;
            //}
            for (int m = 0; m < ListeDist.Count; m++)
            {
                //double distance_moyenne_cycle = 0.0;
                //int PointDep = 1;
                Preci += ListeDist[m];
                //Preci += distance_moyenne_cycle;
            }

            return Preci / ListeDist.Count;
        }

    }
}
