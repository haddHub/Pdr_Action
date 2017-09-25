using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public enum FormeType
    {
        Carré,
        Triangle,
        Cercle
    }

    public class ExerciceForme : ExerciceEvaluation
    {
        public FormeType TypeForme { get; set; }
        public double ShapeAccuracy { get; set; }
        public double EcartTypeSA { get; set; }
        public double CVShapeAccuracy { get; set; }
        public byte Taille { get; set; }
        public byte Origine { get; set; }
        public bool AllerRetour { get; set; }
        public int NbrPolygone { get; set; }

        public ExerciceForme(ExerciceBaseConfig baseConf, ExerciceBorneConfig borneConf,ThemeModel theme)
            : base(baseConf,borneConf,theme)
        {
            this.TypeEval = ExerciceEvalTypes.Forme;
            this.Taille = 1;
            this.Origine = 2;
            this.AllerRetour = false;
        }

        public ExerciceForme()
        {

        }

        public ExerciceForme(int cycleTot, double vitMoy, double[] tabVitMoy, double vitMax, double[] tabVitMax, double speedMet, double[] tabSpeedMet, double jerkMet, double[] tabJekMet, double presi, double[] tabPresi)
            : base(cycleTot, vitMoy, tabVitMoy, vitMax, tabVitMax, speedMet, tabSpeedMet, jerkMet, tabJekMet)
        {
            ShapeAccuracy = presi;
            EcartTypeSA = EcartType(tabPresi, cycleTot);
            CVShapeAccuracy = returnCV(ShapeAccuracy, EcartTypeSA);
        }
    }
}
