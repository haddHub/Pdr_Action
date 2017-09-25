using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace AxAnalyse
{
    static public class Ax_Generique
    {
        // TODO : changer les commantaires !!!
        /// <summary>
        /// Valeur absolue de la soustraction de deux nombres
        /// </summary>
        /// <param name="position2"></param>
        /// <param name="position1"></param>
        /// <returns></returns>
        public static double CalcSommeAbs(double position2, double position1)
        {
            return Math.Abs(position2 - position1);
        }

        /// <summary>
        /// Calcul de la distance parcourue
        /// </summary>
        /// <param name="tabSommeAbs"></param>
        /// <returns></returns>
        public static double[] DistancePark(double[,] tabSommeAbs)
        {
            int nbrs_position = (tabSommeAbs.Length / 2);
            double[] tabDistance = new double[nbrs_position];

            for (int i = 0; i < nbrs_position; i++)
            {
                tabDistance[i] = Pythagorean(tabSommeAbs[0, i], tabSommeAbs[1, i]);
            }

            return tabDistance;
        }

        /// <summary>
        /// Calcul de la distance Réelle
        /// </summary>
        /// <param name="tabDistancePark"></param>
        /// <returns></returns>
        public static double DistanceReelle(double[] tabDistancePark)
        {
            double distReelle = 0.0;
            for (int i = 0; i < tabDistancePark.Length; i++)
            {
                distReelle += tabDistancePark[i];
            }

            return distReelle;
        }

        /// <summary>
        /// Implement the Pythagorean Theorem.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double Pythagorean(double p1, double p2)
        {
            return Math.Sqrt(Math.Pow(p1, 2) + Math.Pow(p2, 2));
        }

        /// <summary>

        /// Distance entre deux points.

        /// </summary>

        /// <param name="p1"></param>

        /// <param name="p2"></param>

        /// <returns></returns>

        public static double DistancePythagorean(double x1, double y1, double x2, double y2)
        {

            double result = 0.0;

            result = Pythagorean((y2 - y1), (x2 - x1));

            //Debug.Print(result.ToString());

            return result;

        }

        /// <summary>
        /// Distance entre deux points.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
    }
}
