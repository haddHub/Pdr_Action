using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using System.Windows;
using System.Collections.ObjectModel;
using AxModel.Helpers;

namespace AxModelExercice
{

    public enum TypePoulies
    {
        Gauche,
        Droite
    }

    public class Poulie : ViewModelBase
    {
        #region Proprietes
        private PointCollection _points;

        public PointCollection Points
        {
            get { return _points; }
            set
            {
                _points = value;
                RaisePropertyChanged("Points");
            }
        }

        private double _rayon;

        public double Rayon
        {
            get { return _rayon; }
            set
            {
                _rayon = value;
                RaisePropertyChanged("Rayon");
            }
        }

        private double _x;

        public double X
        {
            get { return _x; }
            set
            {
                _x = value;
                RaisePropertyChanged("X");
            }
        }

        private double _y;

        public double Y
        {
            get { return _y; }
            set
            {
                _y = value;
                RaisePropertyChanged("Y");
            }
        }

        private int _taille;

        public int Taille
        {
            get { return _taille; }
            set
            {
                _taille = value;
                RaisePropertyChanged("Taille");
            }
        }

        private SolidColorBrush _couleur;

        public SolidColorBrush Couleur
        {
            get { return _couleur; }
            set
            {
                _couleur = value;
                RaisePropertyChanged("Couleur");
            }
        }
        #endregion

        public TypePoulies Type { get; set; }

        private int _nbrSegements;

        public Poulie(double x, TypePoulies type, TypeSymetriePoulies typeSymetrie)
        {
            _nbrSegements = 50;
            Points = new PointCollection();

            Rayon = 8;
            Y = 89;
            Taille = 15;//30

            MiseEchelle();

            this.Type = type;
            DefinirX(typeSymetrie, x);

            Couleur = new SolidColorBrush(Color.FromRgb(153, 76, 0));
            
            CreerCercle();
        }

        //Ajuste le X de la poulie en fonction de la symétrie choisie
        private void DefinirX(TypeSymetriePoulies typeSymetrie, double x)
        {
            double tempX;
            if (Type == TypePoulies.Gauche)
            {
                if (typeSymetrie == TypeSymetriePoulies.VsHorloge || typeSymetrie == TypeSymetriePoulies.JssHorloge)
                    X = (x + Rayon);
                else if(typeSymetrie == TypeSymetriePoulies.VsAntiHorloge || typeSymetrie == TypeSymetriePoulies.JssAntiHorloge)
                    X = (x - Rayon);
                tempX = (int)EchelleUtils.MiseEchelleEnvoyerX(this.X);
                this.X = (int)EchelleUtils.MiseEchelleX(tempX);
            }
            else if (Type == TypePoulies.Droite)
            {
                //if (typeSymetrie == TypeSymetriePoulies.VsHorloge || typeSymetrie == TypeSymetriePoulies.JssAntiHorloge)
                //    this.X = (x + Rayon);
                if (typeSymetrie == TypeSymetriePoulies.VsAntiHorloge || typeSymetrie == TypeSymetriePoulies.JssHorloge)
                    this.X = (x - (2 * Rayon));
                else
                    this.X = x;
                //tempX = (int)EchelleUtils.MiseEchelleEnvoyerX2(this.X);
                //this.X = (int)EchelleUtils.MiseEchelleX2(tempX);
            }
        }

        private void MiseEchelle()
        {
            this.Rayon = EchelleUtils.MiseEchelle(this.Rayon);
            this.Y = EchelleUtils.MiseEchelleY(this.Y - 41.5);
        }

        private void CreerCercle()
        {
            double theta = 1.0 * Math.PI / 1.0;
            double delta_teta = 2.0 * Math.PI / this._nbrSegements;
            var r = new Random(385);
            double dy = 0.0;
            double y = 0.0;

            for (int i = 0; i < this._nbrSegements; i++)
            {
                dy += r.NextDouble() * 2.0 - 1.0;
                y += dy;
                Points.Add(new Point(X + Rayon * Math.Sin(delta_teta * i + theta), Y - Rayon * Math.Cos(delta_teta * i + theta)));
            }
            Points.Add(Points[0]);//Ajout d'un point pour completer le cercle.
            Points.Add(Points[1]);
        }
    }
}
