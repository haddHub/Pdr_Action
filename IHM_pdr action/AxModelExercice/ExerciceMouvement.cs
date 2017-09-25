using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxModel;

namespace AxModelExercice
{
    public enum DroiteType
    {
        Vertical,
        Horizontal,
        Oblique,
        VerticalLong,
        Tonus
    }
    public class ExerciceMouvement : ExerciceEvaluation
    {
        public double Linearite { get; set; }
        public double EcartTypeLin { get; set; }
        public double CVLinearite { get; set; }
        public DroiteType TypeDroite { get; set; }
        public byte PositionDroite { get; set; }

        public ExerciceMouvement(ExerciceBaseConfig baseConf, ExerciceBorneConfig borneConf,ThemeEvaluationModel theme)
            : base(baseConf,borneConf,theme)
        {
            this.TypeEval = ExerciceEvalTypes.Mouvement;
            this.PositionDroite = 2;
        }

        public ExerciceMouvement()
        {

        }

        public ExerciceMouvement(int cycleTot, double vitMoy, double[] tabVitMoy, double vitMax, double[] tabVitMax, double speedMet, double[] tabSpeedMet, double jerkMet, double[] tabJekMet,double line, double[] tabLine)
            : base(cycleTot, vitMoy, tabVitMoy, vitMax, tabVitMax, speedMet, tabSpeedMet, jerkMet, tabJekMet)
        {
            Linearite = line;
            EcartTypeLin = EcartType(tabLine,cycleTot);
            CVLinearite = returnCV(Linearite, EcartTypeLin);
        }
        public static double DistancePythagorean(double x1, double y1, double x2, double y2)
        {
            double result = 0.0;
            result = Pythagorean((y2 - y1), (x2 - x1));
            //Debug.Print(result.ToString());
            return result;
        }

        private static double Pythagorean(double p1, double p2)
        {
            return Math.Sqrt(Math.Pow(p1, 2) + Math.Pow(p2, 2));
        }
    }
}
