using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxModel;

namespace AxAnalyse
{
    public static class Ax_Filter
    {
        static double[] x1 = new double[2];
        static double[] x2 = new double[2];
        static double[] x3 = new double[2];
        static double[] x4 = new double[2];

        const double A = 0.5255;
        const double B = 0.6711;
        const double C = 0.5393;
        const double D = 0.2373;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static List<DataPosition> Filtre125Hz(List<DataPosition> Data)
        {
            List<DataPosition> temps1 = new List<DataPosition>();
            List<DataPosition> temps2 = new List<DataPosition>();
            double offset1, offset2, offset3, offset4;
            
            offset1 = Data[0].X;
            offset2 = Data[0].Y;

            for (int n = 0; n < Data.Count; n++)
            {
                Data[n].X -= offset1;
                Data[n].Y -= offset2;
            }
            
            for (int n = 0; n < Data.Count; n++)
            {
                #region X
                x1[0] = x1[1];
                x1[1] = A * x1[0] + B * Data[n].X;
                #endregion

                #region Y
                x2[0] = x2[1];
                x2[1] = A * x2[0] + B * Data[n].Y;
                #endregion

                temps1.Add(new DataPosition(C * x1[0] + D * Data[n].X, C * x2[0] + D * Data[n].Y));
            }

            temps1.Reverse();

            offset3 = temps1[0].X;
            offset4 = temps1[0].Y;

            for (int n = 0; n < temps1.Count; n++)
            {
                temps1[n].X -= offset3;
                temps1[n].Y -= offset4;
            }

            // Filtre N°2 sur X,Y
            for (int n = 0; n < temps1.Count; n++)
            {
                #region X
                x3[0] = x3[1];
                x3[1] = A * x3[0] + B * temps1[n].X;
                #endregion

                #region Y
                x4[0] = x4[1];
                x4[1] = A * x4[0] + B * temps1[n].Y;
                #endregion

                temps2.Add(new DataPosition(C * x3[0] + D * temps1[n].X, C * x4[0] + D * temps1[n].Y));
            }

            temps2.Reverse();

            for (int n = 0; n < temps2.Count; n++)
            {
                temps2[n].X += (offset1 + offset3);
                temps2[n].Y += (offset2 + offset4);
            }

            return temps2;
        }

        public static List<DataPosition> Filtre125HzOld(List<DataPosition> Data)
        {


            //Butterworth
            List<DataPosition> dataTemp = new List<DataPosition>();
            List<DataPosition> dataTemp2 = new List<DataPosition>();

            foreach (var init in Data)
            { 
                dataTemp.Add(new DataPosition(0.0,0.0));
            }
            dataTemp.RemoveAt(dataTemp.Count()-1);

            for (int i = dataTemp.Count(); i > 0; i--)
            {
                dataTemp[i - 1] = (new DataPosition(Data[i - 1].X + Data[i].X + 0.5254571004 * dataTemp[i - 1].X, Data[i - 1].Y + Data[i].Y + 0.5254571004 * dataTemp[i - 1].Y));
            }
            dataTemp.Reverse();

            foreach (var init in dataTemp)
            {
                dataTemp2.Add(new DataPosition(0.0, 0.0));
            }
            dataTemp2.RemoveAt(dataTemp2.Count()-1);
            for (int i = dataTemp2.Count(); i > 0; i--)
            {
                dataTemp2[i-1]=(new DataPosition(dataTemp[i - 1].X + dataTemp[i].X + 0.5254571004 * dataTemp2[i - 1].X, dataTemp[i - 1].Y + dataTemp[i].Y + 0.5254571004 * dataTemp2[i - 1].Y));
            }
            dataTemp2.Reverse();

            return dataTemp2;


            //List<DataPosition> dataTemp = new List<DataPosition>();
            //dataTemp.Add(new DataPosition(Data.First().X, Data.First().Y));
            //for (int i = 1; i < Data.Count(); i++)
            //{
            //    dataTemp.Add(new DataPosition(FiltrePasseBas(Data[i].X, dataTemp[i - 1].X), FiltrePasseBas(Data[i].Y, dataTemp[i - 1].Y)));
            //}
            //return dataTemp;
        }

        public static double FiltrePasseBas(double xn, double xnPrec)
        {
            double yn;
            int Fc = 12, Fe = 125;
            yn = ((Math.PI * Fc) * xn) / (Math.PI * Fc + Fe) + ((Math.PI * Fc) * xnPrec) / (Math.PI * Fc + Fe);
            return yn;
        }
    }
}