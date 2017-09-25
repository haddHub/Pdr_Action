using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxModel;
namespace AxAnalyse
{
    /// <summary>
    /// 
    /// </summary>
    public static class Ax_Position
    {

        /// <summary>
        /// Calculer la distance Réelle
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static double Distance(List<DataPosition> data)
        {
            double[,] tabSommeAbs;
            double[] tabDistance;
            
            tabSommeAbs = Somme(data);
            tabDistance = Ax_Generique.DistancePark(tabSommeAbs);

            return Ax_Generique.DistanceReelle(tabDistance);
        }

        /// <summary>
        /// Calculer la distance Ppr Pc
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static double DistancePprPc(List<DataPosition> ppr, List<DataPosition> pc)
        {
            double[,] tabSommeAbs;
            double[] tabSommePpr = new double[2];
            double[] tabSommePc = new double[2];
            double nbrs;

            for (int i = 0; i < ppr.Count; i++)
            {
                tabSommePpr[0] += ppr[i].X;
                tabSommePpr[1] += ppr[i].Y;
            }

            tabSommePpr[0] /= ppr.Count;
            tabSommePpr[1] /= ppr.Count;


            for (int i = 0; i < pc.Count; i++)
            {
                tabSommePc[0] += pc[i].X;
                tabSommePc[1] += pc[i].Y;
            }

            tabSommePc[0] /= pc.Count;
            tabSommePc[1] /= pc.Count;

            nbrs = Ax_Generique.DistancePythagorean(tabSommePpr[0], tabSommePpr[1], tabSommePc[0], tabSommePc[1]);

            return nbrs;
        }

        /// <summary>
        /// Calcul de la straightness = Linéarité du mouvement = Rapport entre l'amplitude et la distance réel.
        /// </summary>
        /// <param name="amplitude"></param>
        /// <param name="tabDistReelle"></param>
        /// <returns></returns>
        public static double Straightness(double amplitude, double[] tabDistReelle)
        {
            double moyDistReelle = 0.0;

            moyDistReelle = DistReelleMoy(tabDistReelle);

            return CalcStraightness(amplitude, moyDistReelle);
        }


        /// <summary>
        /// Somme des points
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static double[,] Somme(List<DataPosition> data)
        {
            int nbrs_position = (data.Count - 1);
            double[,] tabSomme = new double[2, nbrs_position];

            for (int i = 0; i < (nbrs_position); i++)
            {
                tabSomme[0, i] = Ax_Generique.CalcSommeAbs(data[i + 1].X, data[i].X);
            }

            for (int j = 0; j < (nbrs_position); j++)
            {
                tabSomme[1, j] = Ax_Generique.CalcSommeAbs(data[j + 1].Y, data[j].Y);
            }
            return tabSomme;
        }

        /// <summary>
        /// Calcul de la Moyenne des Distances Réelles
        /// </summary>
        /// <param name="tabDistReelle"></param>
        /// <returns></returns>
        public static double DistReelleMoy(double[] tabDistReelle)
        {
            int nbrs_position = (tabDistReelle.Length);
            double moy = 0.0;

            for (int i = 0; i < nbrs_position; i++)
            {
                moy += tabDistReelle[i];
            }

            return moy / (double)nbrs_position;
        }

        /// <summary>
        /// Calculer la Straightness
        /// </summary>
        /// <param name="amplitude"></param>
        /// <param name="distReelleMoy"></param>
        /// <returns></returns>
        private static double CalcStraightness(double amplitude, double distReelleMoy)
        {
            return amplitude / distReelleMoy;
        }
        /// <summary>
        /// Calcul de l'amplitude ( coordonnée max en Y )
        /// </summary>
        /// <param name="posi"></param>
        /// <param name="?"></param>
        /// <returns></returns>
      

        public static double BestAmp(List<DataPosition> posi)
        {
            double Amp = posi.First().Y;
            foreach(var el in posi)
            {
                if (el.Y < Amp)
                { Amp = el.Y; }
            }
            return Math.Abs(Amp - 50.5);    // TODO : 50.5 A VERIF
        }

        
    }
}
