using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    class PointReeducation
    {
        public DateTime DateSeance { get; set; }
        public double Moyenne { get; set; }
        
        public PointReeducation(DateTime date, double moyenne)
        {
            DateSeance = date;
            Moyenne = moyenne;
        }
    }
}
