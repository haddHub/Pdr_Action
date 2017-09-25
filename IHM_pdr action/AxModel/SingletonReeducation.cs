using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public class SingletonReeducation
    {
        private static SingletonReeducation instance = null;

        private SingletonReeducation(){ }
        
        public List<double> Init { get; set; }
        public List<double> Klat { get; set; }
        public List<double> Klong { get; set; }
        public double Temps { get; set; }
        public double Mouv { get; set; }
        public bool StartStop { get; set; }

        public static void RecupValeur()
        {
            if (instance == null)
            {
                instance = new SingletonReeducation();
            }
        }

        public static void logOff()
        {
            instance = null;

        }

        public static SingletonReeducation getInstance()
        {
            return instance;
        }
    }
}
