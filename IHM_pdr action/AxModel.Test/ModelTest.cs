using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using AxModel;
using AxModelExercice;

namespace AxModel.Test
{
    [TestFixture]
    public class ModelTest
    {
        ExerciceEvaluation eval;
        double[] tabValeur;
        [SetUp]
        public void InitTest()
        {
             eval = new ExerciceEvaluation();
             tabValeur = new double[] { 10, 5, 12, 14, 56, 26 };
        }
        #region ExerciceEvaluation
        [Test]
        public void Ecart_Type()
        {
            double ecartType = Math.Sqrt(1755.5 / 5);                                                                                                     
            Assert.AreEqual(eval.EcartType(tabValeur,6),ecartType);
        }
        #endregion
    }
}
