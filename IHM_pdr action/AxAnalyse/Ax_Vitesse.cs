using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using AxModel;

namespace AxAnalyse
{
    public static class Ax_Vitesse
    {
        /// <summary>
        /// Calculer la vitesse Moyenne + Peak
        /// </summary>
        /// <param name="data"></param>
        /// <param name="peakV"></param>
        /// <param name="temps"></param>
        /// <returns></returns>
        public static double VitesseMoy(List<DataPosition> data, ref double peakV, ref double[]tabVitesse,double temps)
        {
            double[,] tabSomme;
            //double[] tabVitesse;
            //double[,] tabSomme2;
            int tabLong;

            tabSomme = SommeTemps(data, temps);

            tabLong = tabSomme.Length / 2;
            //tabSomme2 = new double[2, tabLong - 20];

            //for (int i = 0; i < tabLong; i++)
            //{
            //    if ((i > 9) && (i < tabLong - 21))
            //    {
            //        tabSomme2[0, i] = tabSomme[0, i];
            //        tabSomme2[1, i] = tabSomme[1, i];
            //    }

            //}

            tabVitesse = Ax_Generique.DistancePark(tabSomme);
            
            peakV = VitessePeak(tabVitesse);

            return VitesseMoyenne(tabVitesse);    // vitesse moyenne
        }

        public static double VitesseMoyDroite(List<DataPosition> data, ref double peakV, double temps)
        {
            double[,] tabSomme;
            double[] tabVitesse;
            //double[,] tabSomme2;
            int tabLong;

            tabSomme = SommeTemps(data, temps);

            tabLong = tabSomme.Length / 2;
            //tabSomme2 = new double[2, tabLong - 20];

            //for (int i = 0; i < tabLong; i++)
            //{
            //    if ((i > 9) && (i < tabLong - 21))
            //    {
            //        tabSomme2[0, i] = tabSomme[0, i];
            //        tabSomme2[1, i] = tabSomme[1, i];
            //    }

            //}

            tabVitesse = Ax_Generique.DistancePark(tabSomme);

            peakV = VitessePeak(tabVitesse);

            return VitesseMoyenne(tabVitesse);    // vitesse moyenne
        }

        public static double JerkMet(List<DataPosition> data, double peakV, double temps)
        {
            if (peakV != 0.0)
            {
                double[,] tabVit;
                List<DataPosition> dataVit = new List<DataPosition>();
                double[,] tabAcc;
                List<DataPosition> dataAcc = new List<DataPosition>();
                double[,] tabJerk;
                double SommeJerk = 0;


                tabVit = SommeTemps(data, temps);

                int nbrs_position = (tabVit.Length / 2);
                for (int i = 0; i < nbrs_position; i++)
                {
                    DataPosition dp = new DataPosition(tabVit[0, i], tabVit[1, i]);
                    dataVit.Add(dp);
                }
                tabAcc = SommeTemps(dataVit, temps);

                nbrs_position = (tabAcc.Length / 2);
                for (int i = 0; i < nbrs_position; i++)
                {
                    DataPosition dp = new DataPosition(tabAcc[0, i], tabAcc[1, i]);
                    dataAcc.Add(dp);
                }
                tabJerk = SommeTemps(dataAcc, temps);



                nbrs_position = (tabJerk.Length / 2);
                for (int i = 0; i < nbrs_position; i++)
                {
                    SommeJerk += Math.Sqrt(Math.Pow(Math.Abs(tabJerk[0, i]), 2) + Math.Pow(Math.Abs(tabJerk[1, i]), 2));
                }
                SommeJerk /= nbrs_position;

                return (SommeJerk / (peakV));    // vitesse peak 
            }
            else
                return 0.0;
        }

        /// <summary>
        /// Somme des vitesse sur le temps en sec sync
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static double[,] SommeTemps(List<DataPosition> data, double temps)
        {
            int nbrs_position = (data.Count - 1);
            double[,] tabSomme = new double[2, nbrs_position];

            for (int i = 0; i < (nbrs_position); i++)
            {
                tabSomme[1, i] = - (data[i + 1].X - data[i].X) / temps;
            }

            for (int j = 0; j < (nbrs_position); j++)
            {
                tabSomme[0, j] = (data[j + 1].Y - data[j].Y) / temps;
            }

            return tabSomme;
        }

        /// <summary>
        /// Calcul de la vitesse moyenne
        /// </summary>
        /// <param name="tabVitesseMoy"></param>
        /// <returns></returns>
        public static double VitesseMoyenne(double[] tabVitesseMoy)
        {
            double distReelle = 0.0;
            for (int i = 0; i < tabVitesseMoy.Length; i++)
            {
                distReelle += tabVitesseMoy[i];
            }

            return distReelle / tabVitesseMoy.Length;
        }

        /// <summary>
        /// Calcul de la vitesse Max
        /// </summary>
        /// <param name="tabVitesse"></param>
        /// <returns></returns>
        public static double VitessePeak(double[] tabVitesse)
        {
            double vitesseMax = 0.0;
            vitesseMax = tabVitesse.Max();
            //Debug.Print(vitesseMax.ToString());
            return vitesseMax;
        }

        /// <summary>
        /// Calcul de la vitesse de reférence
        /// </summary>
        /// <param name="vitesseInterface"></param>
        /// <returns></returns>
        public static double VitesseRef(double vitesseInterface, double init)
        {
            double resultVR = 0.0;
            resultVR = (((33.8 * 2 * Math.PI) / 60.0) / 15.0) * vitesseInterface;  // 33.8 = vitesse rpm Epos quand slider GUI == 1
            resultVR -= init * 0.019;    // ligne qui prend en compte la dif de vitesse avec ou sans l'init
            return resultVR;
        }
    }
}
