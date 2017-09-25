using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Navegar;
using AxModelExercice;
using System.Collections.ObjectModel;
using System.Diagnostics;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using AxError;
using AxModel.Message;

namespace AxViewModel
{
    public class ReeducationViewViewModel : BlankViewModelBase
    {
        #region Fields

        private List<ViewModelBase> _pagesInternes;
        private List<ViewModelBase> _pagesInternesThemes;

        //private ExerciceJeu ex;
        //ReaPlanExercices _reaPlanExercices;

        private bool _isEnabled = false;

        public ObservableCollection<ExerciceReeducation> Exercices { get; set; }
        public INavigation _nav;
        #endregion  // Fields

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the ReeducationWizardViewModel class.
        /// </summary>
        public ReeducationViewViewModel()
        {
            try
            {
                this.Exercices = new ObservableCollection<ExerciceReeducation>();
                InitNavigation();
                CreateCommands();
                CreateMessengers();
                GetDifficulte("Moyen");
                Debug.Print("ReeducationKidWizardViewModel OK");
                _nav = SimpleIoc.Default.GetInstance<INavigation>();
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        private void CreateMessengers()
        {
           // Messenger.Default.Register<ExerciceJeu>(this, "DifficulteReeducationViewModel", TraitementDifficulte);
            Messenger.Default.Register<bool>(this, "ReeducationViewModel", SetIsEnabled);
            Messenger.Default.Register<RobotErrorMessage>(this, "NewRobotError", OnRobotError);
        }

        #endregion

        #region Methods

        private void InitNavigation()
        {
            PagesInternes.Add(new ExercicesReeducationMouvement(Exercices));
            PagesInternes.Add(new ConnexionTherapeuteViewModel()); //popur l'exemple de changement de page
            PagesInternesThemes.Add(new ThemesReeducationMouvement());
            PagesInternesThemes.Add(new ThemesExercicesEvaluationCinematiqueViewModel());//popur l'exemple de changement de page

            InternView = PagesInternes[0];
            InternViewThemes = PagesInternesThemes[0];

        }

        private void SetIsEnabled(bool b)
        {
            IsEnabled = true;
        }

        private ViewModelBase _internView;
        public ViewModelBase InternView
        {
            get { return _internView; }
            set
            {
                if (_internView != value)
                {
                    _internView = value;
                    RaisePropertyChanged("InternView");
                }
            }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (_isEnabled != value)
                    _isEnabled = value;
                RaisePropertyChanged("IsEnabled");
            }
        }

        private ViewModelBase _internViewThemes;
        public ViewModelBase InternViewThemes
        {
            get { return _internViewThemes; }
            set
            {
                if (_internViewThemes != value)
                {
                    _internViewThemes = value;
                    RaisePropertyChanged("InternViewThemes");
                }
            }
        }

        public List<ViewModelBase> PagesInternes
        {
            get
            {
                if (_pagesInternes == null)
                    _pagesInternes = new List<ViewModelBase>();

                return _pagesInternes;
            }
        }

        public List<ViewModelBase> PagesInternesThemes
        {
            get
            {
                if (_pagesInternesThemes == null)
                    _pagesInternesThemes = new List<ViewModelBase>();

                return _pagesInternesThemes;
            }
        }

        private bool _facile = false;
        public bool Facile
        {
            get
            {
                return _facile;
            }
            set
            {
                _facile = value;
                RaisePropertyChanged("Facile");
            }
        }

        private bool _moyen = true;
        public bool Moyen
        {
            get
            {
                return _moyen;
            }
            set
            {
                _moyen = value;
                RaisePropertyChanged("Moyen");
            }
        }

        private bool _difficile = false;
        public bool Difficile
        {
            get
            {
                return _difficile;
            }
            set
            {
                _difficile = value;
                RaisePropertyChanged("Difficile");
            }
        }

        private bool _expert = false;
        public bool Expert
        {
            get
            {
                return _expert;
            }
            set
            {
                _expert = value;
                RaisePropertyChanged("Expert");
            }
        }

        private string _imageDifficulte;
        public string ImageDifficulte
        {
            get
            {
                return _imageDifficulte;
            }
            set
            {
                _imageDifficulte = value;
                RaisePropertyChanged("ImageDifficulte");
            }
        }

        public void NavigateToHome()
        {
            try
            {
                if (MessageBox.Show(AxLanguage.Languages.REAplan_Accueil_Confirmation, AxLanguage.Languages.REAplan_Confirmation, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    SimpleIoc.Default.GetInstance<INavigation>().NavigateTo<HomeViewModel>(false); 
                }
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        private void CreateCommands()
        {
            PreviousViewModelCommand = new RelayCommand(GoBack, CanGoBack);
            ChangeInternNavigation = new RelayCommand<string>((p) => changeInternView(p));
            NextViewModelCommand = new RelayCommand(NextViewModel, NextCanExecute);
            MainViewModelCommand = new RelayCommand(NavigateToHome);
            GetDifficulteExercice = new RelayCommand<string>((p) => GetDifficulte(p));
        }

        private void GetDifficulte(string s)
        {
            List<string> listeDifficulte = new List<string>();
            listeDifficulte.Add(s);
            switch (s)
            {
                case "Facile" : listeDifficulte.Add("\\Resources\\Image\\Difficulte\\Etoile_Difficulte_Facile.png");
                    break;
                case "Moyen": listeDifficulte.Add("\\Resources\\Image\\Difficulte\\Etoile_Difficulte_Moyen.png");
                    break;
                case "Difficile": listeDifficulte.Add("\\Resources\\Image\\Difficulte\\Etoile_Difficulte_Difficile.png");
                    break;
                case "Expert": listeDifficulte.Add("\\Resources\\Image\\Difficulte\\Etoile_Difficulte_Expert.png");
                    break;
                default:
                    break;
            }
            Messenger.Default.Send(listeDifficulte, "DifficulteMessage");
        }

        private bool CanGoBack()
        {
            return _nav.CanGoBack();
        }

        //Permet de revenir sur le premier ViewModel
        private void GoBack()
        {
            try
            {
                if (_nav.GetTypeViewModelToBack() == typeof(HomeViewModel))
                {
                    _nav.GoBack("SetIsRetour", new object[] { true });
                }
                else
                {
                    _nav.GoBack();
                }
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        private void changeInternView(string p)
        {
            try
            {
                InternView = PagesInternes[Convert.ToInt32(p, 10)];
                InternViewThemes = PagesInternesThemes[Convert.ToInt32(p, 10)];
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        private bool NextCanExecute()
        {
            if (Exercices.Count == 0)
                return false;
            else
                return true;
        }

        private void NextViewModel()
        {
            try
            {
                _nav.NavigateTo<ZoomViewModel>(this, null, "LoadEx", new object[] { this.Exercices }, true); ;
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        private void OnRobotError(RobotErrorMessage e)
        {
            if (_nav.GetTypeViewModelToBack() == typeof(HomeViewModel))
                NavigateToHome();
        }


        #endregion

        #region RelayCommand

        public RelayCommand<string> GetDifficulteExercice { get; set; }
        public RelayCommand PreviousViewModelCommand { get; set; }
        public RelayCommand<string> ChangeInternNavigation { get; set; }
        public RelayCommand NextViewModelCommand { get; set; }
        public RelayCommand MainViewModelCommand { get; set; }

        #endregion

        #region Actions


        #endregion
    }
}
