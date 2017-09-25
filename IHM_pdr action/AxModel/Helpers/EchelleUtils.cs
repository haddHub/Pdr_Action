using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows;

namespace AxModel.Helpers
{
    static public class Constants
    {
        public const int MIN_X = 5;
        public const int MAX_X = 5;
        public const int MIN_Y = 6;
        public const int MAX_Y = 6;
        public const double POSITION_X_MAIN_JEU = 6.0;
        public const double POSITION_Y_MAIN_JEU = 25.3;
        public const double POSITION_X_MAIN = 38.5; // 35
        public const double POSITION2_X_MAIN = 38.5; // 35
        public const double POSITION_Y_MAIN = 67.4; // 22.9 | 25
        public const double POSITION2_Y_MAIN = 67.4; // 22.9 | 25
        public const double POSITION_X_GAUCHE = 30.0;
        public const double POSITION2_X_GAUCHE = 30.0;
        public const double POSITION_Y_CERCLE = 55.0;
        public const double POSITION2_Y_CERCLE = 80.0;
        public const double POSITION_X_DROITE = 52.5;
        public const double POSITION_Y_BAS = 20.2;
        public const double POSITION_Y_HAUT = 50.2;

        public const double POSITION_Y_BASV = 31.2; // 29.2
        public const double POSITION_Y_HAUTV = 1.0;

        public const double POSITION_Y_BASVD = 20.5;    // 18.5 Donuts
        public const double POSITION_Y_HAUTD = 30.0;    // 28.0
        public const double POSITION_X_MAIND = 36.0;

        public const double POSITION_Y_BASE = 24.2;

        public const double GAME_X_MIN = 5.0;
        public const double GAME_X_MAX = 65.0;
        public const double GAME_Y_MIN = 36.0;  // 32.0
        public const double GAME_Y_MAX = 58.0;  // 55.0

        public const double ECHELLE_TV = (1920.0 / 103.8);
    }

    public static class EchelleUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static double MiseEchelle(double val)
        {
            return (val * Constants.ECHELLE_TV);    // 22.75
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valX"></param>
        /// <returns></returns>
        public static double MiseEchelleX(double valX)
        {
            return Math.Round((valX * Constants.ECHELLE_TV) - (9 * Constants.ECHELLE_TV));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valY"></param>
        /// <returns></returns>
        public static double MiseEchelleY(double valY)
        {
            return ((valY * Constants.ECHELLE_TV) - (38 * Constants.ECHELLE_TV)); // -680
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valX"></param>
        /// <returns></returns>
        public static double MiseEchelleX2(double valX)
        {
            return Math.Round(((valX * Constants.ECHELLE_TV) - (9 * Constants.ECHELLE_TV)) - (Constants.ECHELLE_TV * 1.5 ));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valY"></param>
        /// <returns></returns>
        public static double MiseEchelleY2(double valY)
        {
            return ((valY * Constants.ECHELLE_TV)) - 680.0;
        }

        /// <summary>
        /// Mise à l'echelle des positions X pour le robot
        /// </summary>
        /// <param name="valX"></param>
        /// <returns></returns>
        public static double MiseEchelleEnvoyerX(double valX)
        {
            //return ((valX + (9.0 * Constants.ECHELLE_TV)) / Constants.ECHELLE_TV) - 7.0;
            //return Math.Round((valX * Constants.ECHELLE_TV) - (9 * Constants.ECHELLE_TV));
            //return (valX + (9 * (1920.0 / 93.5))) / (1920.0 / 93.5);
            return ((valX - 85) / (1920.0 / 93.5));
        }

        /// <summary>
        /// Mise à l'echelle des positions X pour le robot
        /// </summary>
        /// <param name="valX"></param>
        /// <returns></returns>
        public static double MiseEchelleEnvoyerX2(double valX)
        {
            //return Math.Round(((valX + (9 * Constants.ECHELLE_TV))) / Constants.ECHELLE_TV);
            return (((1920 - valX) + (3.7 * Constants.ECHELLE_TV)) / Constants.ECHELLE_TV);
        }

        /// <summary>
        /// Mise à l'echelle des positions Y pour le robot
        /// </summary>
        /// <param name="valY"></param>
        /// <returns></returns>
        public static double MiseEchelleEnvoyerY(double valY)
        {
            //return ((((valY + (38 * Constants.ECHELLE_TV)) / Constants.ECHELLE_TV)) + 41.5) - 57; //+680
           // return (valY + 153) / Constants.ECHELLE_TV;
            //return (((valY - 140.0) / 21.6)) + 20.0;
            return (valY + 360.0) / (1920.0 / 93.5);

        }

        public static double MiseEchelleEnvoyerY2(double valY)
        {
            return -(((valY + 153) / Constants.ECHELLE_TV) - 133.8);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static double MiseEchelleEnvoyer(double val)
        {
            return (val / Constants.ECHELLE_TV);    // 22.75
        }

        /// <summary>
        /// Mise à l'echelle des positions X pour l'affichage
        /// </summary>
        /// <param name="valX"></param>
        /// <returns></returns>
        public static double MiseEchelleXPosition(double valX)
        {
            //return (((valX / 100.0) * Constants.ECHELLE_TV) - (2.8 * Constants.ECHELLE_TV));//1.183
            return -(((valX / 100.0) * (1920 / 93.5)) + 85.0) + 1920.0;
        }

        /// <summary>
        /// Mise à l'echelle des positions Y pour l'affichage
        /// </summary>
        /// <param name="valY"></param>
        /// <returns></returns>
        public static double MiseEchelleYPosition(double valY)
        {
            //return (((((valY / 100.0) * Constants.ECHELLE_TV))) - (22.5 * Constants.ECHELLE_TV)); //-480
            //return ((valY / 100.0) * Constants.ECHELLE_TV) - 153.0;
            return ((((valY / 100.0) * (1920 / 93.5)))) - 360.0;
        }

        /// <summary>
        /// Mise à l'echelle des positions X pour l'affichage
        /// </summary>
        /// <param name="valX"></param>
        /// <returns></returns>
        public static double MiseEchelleXPosition2(double valX)
        {
           // return 1920 - (int)(((valX / 100.0) * Constants.ECHELLE_TV) - (2.0 * Constants.ECHELLE_TV));  // -20
            return 1920 - (((valX / 100.0) * Constants.ECHELLE_TV) - (3.7 * Constants.ECHELLE_TV));
        }

        /// <summary>
        /// Mise à l'echelle des positions Y pour l'affichage
        /// </summary>
        /// <param name="valY"></param>
        /// <returns></returns>
        public static double MiseEchelleYPosition2(double valY)
        {
            return  (((((133.8 - (valY / 100.0)) * Constants.ECHELLE_TV))) - 153); // -153
        }
    }
}
