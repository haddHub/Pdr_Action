using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using AxModelExercice;
using AxModel;
using AxTheme;
using AxExerciceGenerator;
using System.Windows;
using AxError;

namespace AxViewModel
{
    public class ExercicesReeducationMouvement : BlankViewModelBase
    {
        #region Fields
        //private readonly ThemesModel db;
        private readonly ObservableCollection<ThemeModel> themes;
        public ObservableCollection<ExerciceReeducation> Exercices { get; set; }
        private readonly ICollectionView collectionView;
        private int _ItemCount;
        private string difficulte;

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the ReeducationKidWizardSelectExViewModel class.
        /// </summary>
        public ExercicesReeducationMouvement( ObservableCollection<ExerciceReeducation> Exercices) // TODO : chargement des themes en async !
        {
            try
            {
                this._ItemCount = 0;
                //this.db = new ThemesModel();

                this.Exercices = Exercices;
                this.themes = new ObservableCollection<ThemeModel>();

                //Parallel.ForEach(this.db.Themes, theme =>   // Bibliothèque parallèle de tâches, ForEach définit une boucle dans laquelle les itérations peuvent s’exécuter en parallèle
                //{
                //    this.themes.Add(new ThemeViewModel(theme));
                //});
                foreach (ThemeModel theme in GestionThemes.LoadAllReeducationTheme())
                {
                    this.themes.Add(theme);
                }

                this.collectionView = CollectionViewSource.GetDefaultView(this.themes);
                if (this.collectionView == null)
                    throw new NullReferenceException("collectionView");

                this.collectionView.CurrentChanged += new EventHandler(this.OnCollectionViewCurrentChanged);    // création de l'évenement de changement de thème selectionné
                SelectedTheme = themes[0];
                if (IsInDesignMode)
                {
                    // Code runs in Blend --> create design time data.
                }
                else
                {
                    // Code runs "for real": Connect to service, etc...
                }
                // Subscribe to CollectionChanged event
                Exercices.CollectionChanged += OnExercicesListChanged;
                CreateCommands();
                GetNomExercice("Discret statique");
                Messenger.Default.Register<List<string>>(this, "DifficulteMessage", GetDifficulte);
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex); 
            }
        }

        #endregion

        #region Properties

        public ObservableCollection<ThemeModel> Themes  // liste de Themes pour la ListBox
        {
            get { return this.themes; } // TODO : chargement des themes en async !
        }

        /// <summary>
        /// The number of items in the list.
        /// </summary>
        public int ItemCount
        {
            get { return _ItemCount; }

            set
            {
                _ItemCount = value;
                RaisePropertyChanged("ItemCount");
            }
        }


        private ExerciceReeducation _selectedItem;

        public ExerciceReeducation SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; }
        }

        private string nomExerciceSelectionne;

        private ThemeModel _selectedTheme;
        public ThemeModel SelectedTheme // Theme courant l'interface se bind dessus pour l'affichage
        {
            get
            {
                //ReaPlanExercices.Theme = (this.collectionView.CurrentItem as ThemeViewModel).Theme;
                return this.collectionView.CurrentItem as ThemeModel;
            }
            set
            {
                _selectedTheme = value;
                RaisePropertyChanged("SelectedTheme");
            }
        }

        private void OnCollectionViewCurrentChanged(object sender, EventArgs e)
        {
            RaisePropertyChanged("SelectedTheme");  // Notification du changement de Theme courant
        }

        #endregion

        #region Methods

        private void CreateCommands()
        {
            AddItem = new RelayCommand(AdItem, AdItem_CanExecute);
            DeleteItem = new RelayCommand(DelItem, DelItem_CanExecute);
            GetNomExerciceSelectionne = new RelayCommand<string>((p) => GetNomExercice(p));
        }

        private void GetNomExercice(string s)
        {
            nomExerciceSelectionne = s;
        }
        /// <summary>
        /// Updates the ItemCount Property when the PositionList collection changes.
        /// </summary>
        private void OnExercicesListChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Update item count
            try
            {
                this.ItemCount = this.Exercices.Count;
                if (ItemCount != 0)
                {

                }
                Messenger.Default.Send(true, "ReeducationViewModel");  // Message pour activer NextCommand
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        private string imageDifficulte;
        private void GetDifficulte(List<string> s)
        {
            difficulte = s[0];
            imageDifficulte = s[1];
        }

        #endregion

        #region RelayCommand
        
        public RelayCommand<string> GetNomExerciceSelectionne
        {
            get;
            set;
        }


        public RelayCommand AddItem
        {
            get;
            private set;
        }

        public RelayCommand DeleteItem
        {
            get;
            private set;
        }

        #endregion

        #region Actions

        private void AdItem()
        {
            try
            {
                ExerciceJeu exJeu;
                switch (nomExerciceSelectionne)
                {
                    case "Discret statique": exJeu = ExerciceGenerator.GetExerciceJeu(SelectedTheme, difficulte);
                        exJeu.StaticDyn = false;
                        exJeu.ImageDifficulte = imageDifficulte;
                        exJeu.NomExercice = "DS";
                        Exercices.Add(exJeu);
                        break;

                    case "Discret dynamique": exJeu = ExerciceGenerator.GetExerciceJeu(SelectedTheme, difficulte);
                        exJeu.StaticDyn = true;
                        exJeu.ImageDifficulte = imageDifficulte;
                        exJeu.NomExercice = "DD";
                        Exercices.Add(exJeu);
                        break;

                    case "Discret cognitif": ;
                        break;

                    case "Cyclique": ;
                        break;

                    case "Complexe": ;
                        break;

                    default:
                        break;
                }
                Messenger.Default.Send<string>("", "DefilementListe");
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        private bool AdItem_CanExecute()
        {
            if (SelectedTheme == null || Exercices == null)
                return false;
            else
                return true;
        }

        private void DelItem()
        {
            try
            {
                Exercices.Remove(SelectedItem);
                Messenger.Default.Send<string>("", "DefilementListeSupp");
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        private bool DelItem_CanExecute()
        {
            return (SelectedItem != null);
        } 

        #endregion
    }
}
