using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class PointsDSReed
    {
        public double Moyenne { get; set; }
        public double DevStand { get; set; }
        
        public PointsDSReed(double moyenne,double devStand)
        {
            Moyenne = moyenne;
            DevStand = devStand;
        }
    }
}
