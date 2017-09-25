using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class PointEvoEval
    {
        public DateTime Date { get; set; }
        public double Resultat { get; set; }
        public double EcartT { get; set; }
        public double CV { get; set; }
        public ChartsParam Param { get; set; }
        public double[] TabNorme { get; set; }
    }
}
