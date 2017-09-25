using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using System.Windows.Media;
using System.Diagnostics;
using System.IO;

namespace AxModelExercice
{
    public enum TypePlateau
    {
        Normal,
        Fantome
    }

    public class Plateau : ViewModelBase
    {
        private const int maxAngle = 30;
        private const int minAngle = -30;

        private TypePlateau Type;

        #region Propriétés
        private int _x;

        public int X
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

        private int _height;

        public int Height
        {
            get { return _height; }
            set
            {
                _height = value;
                RaisePropertyChanged("Height");
            }
        }

        private int _width;

        public int Width
        {
            get { return _width; }
            set
            {
                _width = value;
                RaisePropertyChanged("Width");
            }
        }

        private double _angle;

        public double Angle
        {
            get { return _angle; }
            set
            {
                _angle = value;
                RaisePropertyChanged("Angle");
            }
        }

        private int _centreX;

        public int CentreX
        {
            get { return _centreX; }
            set
            {
                _centreX = value;
                RaisePropertyChanged("CentreX");
            }
        }

        private int _centreY;

        public int CentreY
        {
            get { return _centreY; }
            set
            {
                _centreY = value;
                RaisePropertyChanged("CentreY");
            }
        }

        private int _epaisseur;

        public int Epaisseur
        {
            get { return _epaisseur; }
            set
            {
                _epaisseur = value;
                RaisePropertyChanged("Epaisseur");
            }
        }

        private double _opaque;

        public double Opaque
        {
            get { return _opaque; }
            set
            {
                _opaque = value;
                RaisePropertyChanged("Opaque");
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

        public double HauteurMax { get; set; }

        public TypeExercicePoulies TypeExercice { get; set; }

        #endregion

        public Plateau(int w, int h, TypePlateau type, TypeExercicePoulies typeExo)
        {
            Height = h;
            Width = w;
            Angle = 0;
            X = (1920 / 2) - (Width / 2);
            Y = (1080 - Height * 2);
            CentreX = (Width / 2);
            CentreY = (Height / 2);
            Type = type;
            TypeExercice = typeExo;
            if (type == TypePlateau.Normal)
            {
                Couleur = new SolidColorBrush(Colors.Gray);
                Epaisseur = 1;
                Opaque = 1;
            }
            if (type == TypePlateau.Fantome)
            {
                Couleur = new SolidColorBrush(Colors.White);
                Epaisseur = 2;
                Opaque = 0.3;
            }
        }

        public void DeplacerVerticalement(double g, double d)
        {
            //bouger verticalement.
            //Transformation de la vitesse des main (cm/s) en pixel secondes.
            //double v = ((Math.Max(g, d)) * 1080) / 58.3;
            double v = (((g + d) / 2) * 1080) / 58.3;
            if (this.Type == TypePlateau.Normal || (this.Type == TypePlateau.Fantome && this.TypeExercice == TypeExercicePoulies.TestVitesseLibre) || (this.Type == TypePlateau.Fantome && this.TypeExercice == TypeExercicePoulies.Entrainement))
            {
                //0.00625 * 2 car on calcule les vitesse toutes les 2 aquisition.
                //divisé par deux car la vitesse calculé de maniere physique est trop rapide.
                this.Y = this.Y - ((v) * (2 * 0.00625));
            }
            else
                //La vitesse ne doit pas être divisée par deux quand le plateau est fantôme et que sa vitesse est imposée.
                this.Y = this.Y - (v * (2 * 0.00625)); 
            
            //le plateau est remit en bas si il atteint le haut de l'écran.
            if (Y <= HauteurMax)
                Y = (1080 - Height * 2);           
        }

        private void ChangementCouleur()
        {
            if (this.Angle > 20 || this.Angle < -20)
                this.Couleur = new SolidColorBrush(Colors.Red);
            else if (this.Angle > 5 || this.Angle < -5)
                this.Couleur = new SolidColorBrush(Colors.Orange);
            else
                this.Couleur = new SolidColorBrush(Colors.Green);
        }

        //on calcule la difference de vitesse entre la main gauche et droite pour savoir dans quelle sense on fait evoluer l'angle.
        //L'angle ajouté dépend de la difference de vitesse.
        public void Tourner(double angle)
        {
            double nouvelAngle = angle;

            if (nouvelAngle < -30)
                nouvelAngle = -30;
            else if (nouvelAngle > 30)
                nouvelAngle = 30;

            this.Angle = nouvelAngle;

            if (this.Type == TypePlateau.Normal)
            {
                ChangementCouleur();
            }
        }

        private double DegreEnRadian(double deg)
        {
            return (Math.PI * deg) / 180.0;
        }

        private double RadianEnDegre(double rad)
        {
            return (180.0 * rad) / (Math.PI);
        }
    }
}
