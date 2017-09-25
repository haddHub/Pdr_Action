using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class FreeAmplitude : ExerciceMouvement
    {
        public double Amplitude { get; set; }
        public double EcartTypeAmp { get; set; }
        public double CVAmplitude { get; set; }

        public FreeAmplitude(ExerciceBaseConfig baseConf, ExerciceBorneConfig borneConf,ThemeModel theme)
            : base(baseConf,borneConf,theme)
        {
            this.TypeDroite = DroiteType.VerticalLong;
        }

        public FreeAmplitude(int cycleTot, double vitMoy, double[] tabVitMoy, double vitMax, double[] tabVitMax, double speedMet, double[] tabSpeedMet, double jerkMet, double[] tabJekMet, double line, double[] tabLine,double ampli,double[] tabAmpli)
            : base(cycleTot, vitMoy, tabVitMoy, vitMax, tabVitMax, speedMet, tabSpeedMet, jerkMet, tabJekMet,line,tabLine)
        {
            Amplitude = ampli;
            EcartTypeAmp = EcartType(tabAmpli, cycleTot);
            CVAmplitude = returnCV(Amplitude, EcartTypeAmp);
        }
        public static double CalAmpliFree(List<DataPosition> posi, ref double coordYMax)
        {
            DataPosition Bas = new DataPosition(posi.First().X, posi.First().Y);
            //DataPosition Haut = new DataPosition(posi.First().X, posi.First().Y);
            double distanceTot = 0.0;

            foreach (var dp in posi)
            {
                //if (dp.Y > Haut.Y)
                //{
                //    Haut = dp;
                //}
                if (dp.Y < Bas.Y)
                {
                    Bas = dp;
                }
            }

            coordYMax = posi.First().Y - Bas.Y;
            distanceTot = DistancePythagorean(posi.First().X, posi.First().Y, Bas.X, Bas.Y);
            distanceTot += DistancePythagorean(Bas.X, Bas.Y, posi.Last().X, posi.Last().Y);
            return distanceTot;
        }
    }
}
