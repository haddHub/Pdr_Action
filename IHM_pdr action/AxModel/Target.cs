using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class Target : ExerciceMouvement
    {
        public double Precision { get; set; }
        public double EcartTypePre { get; set; }
        public double CVPrecision { get; set; }

        public Target(ExerciceBaseConfig baseConf, ExerciceBorneConfig borneConf,ThemeModel theme)
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
    }
}
