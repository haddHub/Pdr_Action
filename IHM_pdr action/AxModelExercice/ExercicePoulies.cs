using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxModel;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows;

namespace AxModelExercice
{
    public enum TypeSymetriePoulies
    {
        VsHorloge,
        VsAntiHorloge,
        JssHorloge,
        JssAntiHorloge
    }

    public enum TypeExercicePoulies
    {
        TestVitesseLibre,
        TestVitesseContrainte,
        Entrainement
    }

    public enum TypeRessort
    {
        AvecRessort,
        SansRessort
    }
    public class ExercicePoulies : Circle
    {
        public Poulie PoulieGauche { get; set; }
        public Poulie PoulieDroite { get; set; }
        public ObservableCollection<Plateau> Plats { get; set; }
        public TypeSymetriePoulies TypeSymetrie { get; set; }
        public TypeExercicePoulies TypeExercice { get; set; }
        public PointCollection BarreVerticale { get; set; }
        public InteractionConfig ConfigInteraction { get; set; }
        public string Description { get; set; }
        public double PourcentageAssistance { get; set; }

        private int _numeroExercice;
        public int NumeroExercice
        {
            get { return _numeroExercice; }
            set 
            {
                _numeroExercice = value;
                DefinirDescription();
            }
        }


        /// <summary>
        /// Duree de l'exercice en seconde
        /// </summary>
        public int Duree { get; set; }
        /// <summary>
        /// Temps de pause entre deux exercice en seconde
        /// </summary>
        public int TempsDePause { get; set; }

        public double VitesseContrainte { get; set; }

        private TypeRessort _ressort;

        public TypeRessort Ressort
        {
            get { return _ressort; }
            set 
            { 
                _ressort = value;
                DefinirConfig();
            }
        }

        public ExercicePoulies(ExerciceBaseConfig baseConf, ExerciceBorneConfig borneConf, ThemeEvaluationModel theme,InteractionConfig configInt ,TypeSymetriePoulies typeS, TypeExercicePoulies typeE)
            : base(baseConf, borneConf, theme)
        {
            TypeSymetrie = typeS;
            TypeExercice = typeE;

            ConfigInteraction = configInt;

            if (this.TypeSymetrie == TypeSymetriePoulies.JssHorloge || this.TypeSymetrie == TypeSymetriePoulies.JssAntiHorloge)
                ConfigInteraction.Jss = true;
            else
                ConfigInteraction.Vs = true;

            Plats = new ObservableCollection<Plateau>();
            Plateau plat = new Plateau(850, 70, TypePlateau.Normal,typeE);
            Plateau platFantome = new Plateau(850, 70, TypePlateau.Fantome,typeE);
            Plats.Add(platFantome);
            Plats.Add(plat);

            //ajuste l'emplacement des poulies en fonction de la taille du plateau.
            double xPoulieGauche = Plats[1].X;
            PoulieGauche = new Poulie(xPoulieGauche, TypePoulies.Gauche, TypeSymetrie);

            double xPoulieDroite = PoulieGauche.X + Plats[1].Width;
            PoulieDroite = new Poulie(xPoulieDroite, TypePoulies.Droite, TypeSymetrie);

            //Permet aux plateaux de ne pas dépacer les poulies
            Plats[0].HauteurMax = PoulieGauche.Y + PoulieGauche.Rayon + PoulieGauche.Taille;
            Plats[1].HauteurMax = PoulieGauche.Y + PoulieGauche.Rayon + PoulieGauche.Taille;

            BarreVerticale = new PointCollection();
            BarreVerticale.Add(new Point(Plats[1].X + Plats[1].Width / 2,(PoulieGauche.Y + PoulieGauche.Rayon + PoulieGauche.Taille) - 50));
            BarreVerticale.Add(new Point(Plats[1].X + Plats[1].Width / 2, Plats[1].Y + Plats[1].Height / 2));
            this.PourcentageAssistance = 0;
            DefinirDureePause();
        }

        private void DefinirDureePause()
        {
            if (TypeExercice == TypeExercicePoulies.TestVitesseLibre || TypeExercice == TypeExercicePoulies.TestVitesseContrainte)
            {
                Duree = 20;
                TempsDePause = 5;
            }
            else
            {
                Duree = 60;
                TempsDePause = 25;
            }
        }

        public void MonterPlateau(double g, double d)
        {
            if (this.TypeExercice == TypeExercicePoulies.TestVitesseLibre || this.TypeExercice == TypeExercicePoulies.Entrainement)
                this.Plats[0].DeplacerVerticalement(g, d);
            else //Faire bouger le fantôme a la vitesse impossée.
                this.Plats[0].DeplacerVerticalement(this.VitesseContrainte, this.VitesseContrainte);
            this.Plats[1].DeplacerVerticalement(g, d);
        }

        public void TournerPlateau(double a)
        {
            this.Plats[1].Tourner(a);
        }

        private void DefinirConfig()
        {
            if (this.TypeExercice == TypeExercicePoulies.Entrainement)
            {
                if (this.Ressort == TypeRessort.AvecRessort)
                {
                    this.ConfigInteraction.KInteracL = 210;//200
                    this.ConfigInteraction.KInteractR = 210;//200
                    this.ConfigInteraction.CInteractL = 50;//100
                    this.ConfigInteraction.CInteractR = 50;//100
                    this.PourcentageAssistance = (this.ConfigInteraction.KInteracL / 210.0) * 100;
                    this.PourcentageAssistance = Math.Round(this.PourcentageAssistance, 0);
                    //Actualise la description en fonction de la config.
                    this.DefinirDescription();
                } 
            }
        }

        /// <summary>
        /// Méthode utilisée pour faire dimminuer la config de l'exercice en entrainement.
        /// </summary>
        /// <param name="valeurK"></param>
        /// <param name="valeurC"></param>
        public void ActualiserConfig(byte valeurK, byte valeurC)
        {
            if (this.TypeExercice == TypeExercicePoulies.Entrainement)
            {
                if (this.Ressort == TypeRessort.AvecRessort)
                {
                    this.ConfigInteraction.KInteracL = valeurK;
                    this.ConfigInteraction.KInteractR = valeurK;
                    this.ConfigInteraction.CInteractL = valeurC;
                    this.ConfigInteraction.CInteractR = valeurC;
                    this.PourcentageAssistance = ((this.ConfigInteraction.KInteracL) / 210.0) * 100;
                    this.PourcentageAssistance = Math.Round(this.PourcentageAssistance, 0);
                    //Actualise la description en fonction de la config.
                    this.DefinirDescription();
                }
            }
        }

        private void DefinirDescription()
        {
            switch (this.TypeExercice)
            {
                case TypeExercicePoulies.TestVitesseLibre: this.Description = "Test\nVitesse Libre\n";
                    break;
                case TypeExercicePoulies.TestVitesseContrainte: this.Description = "Test\nVitesse Imposée\n" + this.VitesseContrainte + "cm/s\n";
                    break;
                case TypeExercicePoulies.Entrainement: this.Description = "Entrainement\n";
                    if (this.Ressort == TypeRessort.AvecRessort)
                        this.Description += "Assistance " + this.PourcentageAssistance + "%\n";
                    break;
                default: this.Description = "Test Vitesse Libre\n";
                    break;
            }

            this.Description += "Exercice Numéro : " + this.NumeroExercice.ToString();
        }

        public void RaferchirSymetriePoulies()
        {
            if (this.ConfigInteraction.Jss)
                this.TypeSymetrie = TypeSymetriePoulies.JssHorloge;
            else
                this.TypeSymetrie = TypeSymetriePoulies.VsHorloge;

            double xPoulieGauche = Plats[1].X;
            PoulieGauche = new Poulie(xPoulieGauche, TypePoulies.Gauche, TypeSymetrie);

            double xPoulieDroite = PoulieGauche.X + Plats[1].Width;
            PoulieDroite = new Poulie(xPoulieDroite, TypePoulies.Droite, TypeSymetrie);
        }
    }
}
