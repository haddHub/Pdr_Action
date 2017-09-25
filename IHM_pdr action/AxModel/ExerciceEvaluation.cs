using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public enum ExerciceEvalTypes
    {
        Forme,
        Mouvement
    }
    public class ExerciceEvaluation : ExerciceGeneric
    {
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

        public ExerciceEvalTypes TypeEval { get; set; }

        public double returnCV(double Moyenne, double EcartType)
        {
            double CV = EcartType / Moyenne;
            CV *= 100;
            return CV;
        }

        public ExerciceEvaluation(ExerciceBaseConfig baseConf, ExerciceBorneConfig borneConf, ThemeModel theme)
            : base(baseConf,borneConf,theme)
        {
            this.TypeExercice = ExerciceTypes.Evaluation;
        }

        public ExerciceEvaluation()
        {

        }
        public ExerciceEvaluation(int cycleTot, double vitMoy, double[] tabVitMoy, double vitMax, double[] tabVitMax, double speedMet, double[] tabSpeedMet, double jerkMet, double[] tabJekMet)
        {
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
            this.TypeExercice = ExerciceTypes.Evaluation;
            this.BaseConfig = gen.BaseConfig;
            this.BorneConfig = gen.BorneConfig;
            this.Theme = gen.Theme;
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
    }
}
