using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxModel;

namespace AxModelExercice
{
    public class Square : ExerciceForme
    {
        public Square(double shapeAccuracy, double ecartTypeSA, double vitesseMoy, double ecartTypeVMoy, double vitesseMax, double ecartTypeVMax, double speedMetric, double ecartTypeSM, double jerkMetric, double ecartTypeJM)
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

        public Square(ExerciceBaseConfig baseConf, ExerciceBorneConfig borneConf,ThemeEvaluationModel theme)
            : base(baseConf,borneConf,theme)
        {
            this.NbrPolygone = 4;
            this.TypeForme = FormeType.Carré;
        }

        public Square(int cycleTot, double vitMoy, double[] tabVitMoy, double vitMax, double[] tabVitMax, double speedMet, double[] tabSpeedMet, double jerkMet, double[] tabJekMet,double presi, double[] tabPresi)
            :base(cycleTot,vitMoy,tabVitMoy,vitMax,tabVitMax,speedMet,tabSpeedMet,jerkMet,tabJekMet,presi,tabPresi)
        {

        }
        public static double PreciCarre(List<DataPosition> Posi, DataPosition CentreCarre, double LongCotCarre, double OrientCarre)
        {
            double Preci = 0.0;
            List<DataPosition> PosiProj = new List<DataPosition>();
            List<DataPosition> RefShape = new List<DataPosition>();
            List<double> ListeDist = new List<double>();
            double posiProjTempX, posiProjTempY;


            double Ns = 5;
            for (int ind = 0; ind < Ns; ind++)
            {
                RefShape.Add(new DataPosition(CentreCarre.X + (LongCotCarre / 2.0) * (Math.Sin(OrientCarre + ind * Math.PI / 2.0) + Math.Cos(OrientCarre + ind * Math.PI / 2.0)), CentreCarre.Y + (LongCotCarre / 2.0) * (Math.Cos(OrientCarre + ind * Math.PI / 2.0) - Math.Sin(OrientCarre + ind * Math.PI / 2.0))));
            }

            for (int dp = 0; dp < Posi.Count; dp++)
            {
                ListeDist.Add(100.0);   // Initialisation

                for (int j = 0; j < Ns - 1; j++)    // parcours de tous les segments de la trajectoire de référence
                {
                    // Calcul du point projeté
                    double vsipc_X = Posi[dp].X - RefShape[j].X;
                    double vsipc_Y = Posi[dp].Y - RefShape[j].Y;

                    double vsisf_X = RefShape[j + 1].X - RefShape[j].X;
                    double vsisf_Y = RefShape[j + 1].Y - RefShape[j].Y;

                    double alpha = (vsipc_X * vsisf_X + vsipc_Y * vsisf_Y) / Math.Sqrt(Math.Pow(vsisf_X, 2.0) + Math.Pow(vsisf_Y, 2.0));

                    if (alpha <= 0.0)
                    {
                        posiProjTempX = RefShape[j].X;
                        posiProjTempY = RefShape[j].Y;
                    }
                    else
                    {
                        if (alpha >= 1.0)
                        {
                            posiProjTempX = RefShape[j + 1].X;
                            posiProjTempY = RefShape[j + 1].Y;
                        }
                        else
                        {
                            posiProjTempX = RefShape[j].X + alpha * vsipc_X;
                            posiProjTempY = RefShape[j].Y + alpha * vsipc_Y;
                        }
                    }
                    double DistTemp = Math.Sqrt(Math.Pow((posiProjTempX - Posi[dp].X), 2.0) + Math.Pow((posiProjTempY - Posi[dp].Y), 2.0));
                    if (DistTemp < ListeDist[dp])   // Mise à jour du point projeté le plus proche de la distance correspondante
                    {
                        ListeDist[dp] = DistTemp;
                        PosiProj.Add(new DataPosition(posiProjTempX, posiProjTempY));
                    }
                }
            }

            //m < Compteur (qui ici est 1)
            //for (int m = 0; m < 1; m++)
            //{
            //    double distance_moyenne_cycle = 0;
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
