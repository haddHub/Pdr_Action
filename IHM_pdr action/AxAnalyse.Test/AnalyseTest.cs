using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using AxAnalyse;
using AxModel;

namespace AxAnalyse.Test
{
    [TestFixture]
    class AnalyseTest
    {
        List<DataPosition> listePoint;
        double[] tabVitesse;
        [SetUp]
        public void InitTest()
        {
            listePoint = new List<DataPosition>();
            listePoint.Add(new DataPosition(12.4, 7.5));
            listePoint.Add(new DataPosition(20.4, 30.2));
            tabVitesse = new double[4] { 12.5, 50.3, 63.1, 3.7 };
        }

        #region Distance
        [Test]
        public void CalcSommeAbs_retourne_positif_quand_reponse_negative()
        {
            Assert.GreaterOrEqual(Ax_Generique.CalcSommeAbs(12.56, 27.53), 0);
        }
        [Test]
        public void CalcSommeAbs_retourne_positif_quand_reponse_positive()
        {
            Assert.GreaterOrEqual(Ax_Generique.CalcSommeAbs(35.23, 27.53), 0);
        }
        [Test]
        public void Distance_Entre_Deux_Points_Positive()//point 1 en-dessous du point 2
        {
            double distance = Math.Sqrt(Math.Pow(listePoint[1].X - listePoint[0].X, 2) + Math.Pow(listePoint[1].Y - listePoint[0].Y, 2));
            Assert.AreEqual(distance, Ax_Position.Distance(listePoint));
        }
        [Test]
        public void Distance_Entre_Deux_Points_Negative()//point 1 au-dessus du point 2
        {
            double distance = Math.Sqrt(Math.Pow(listePoint[0].X - listePoint[1].X, 2) + Math.Pow(listePoint[0].Y - listePoint[1].Y, 2));
            Assert.AreEqual(distance, Ax_Position.Distance(listePoint));
        }
        [Test]
        public void Distance_Entre_Plusieur_Points()//Distance réel parcourue
        {
            listePoint.Add(new DataPosition(25.4, 33));
            double distance1 = Math.Sqrt(Math.Pow(listePoint[1].X - listePoint[0].X, 2) + Math.Pow(listePoint[1].Y - listePoint[0].Y, 2));
            double distance2 = Math.Sqrt(Math.Pow(listePoint[2].X - listePoint[1].X, 2) + Math.Pow(listePoint[2].Y - listePoint[1].Y, 2));
            Assert.AreEqual(distance1 + distance2, Ax_Position.Distance(listePoint));
        }
        #endregion
        #region Vitesse
        [Test]
        public void Vitesse_Moyenne()
        {
            double moyenne = (12.5 + 50.3 + 63.1 + 3.7) / 4;
            Assert.AreEqual(moyenne, Ax_Vitesse.VitesseMoyenne(tabVitesse));
        }
        [Test]
        public void Vitesse_Peak()
        {
            Assert.AreEqual(63.1, Ax_Vitesse.VitessePeak(tabVitesse));
        }
        #endregion
    }
}
