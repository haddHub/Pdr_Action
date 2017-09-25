using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class DataPosition
    {
        public double X { get; set; }
        public double Y { get; set; }

        public DataPosition(double x, double y)
        {
            X = x;
            Y = y;
        }

        public DataPosition()
        {
        }
    }
}
