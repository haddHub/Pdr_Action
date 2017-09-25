using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using System.Diagnostics;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using AxModel;
using GalaSoft.MvvmLight.Ioc;
using Navegar;
using AxModelExercice;
using AxData;
using AxError;

namespace AxViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm/getstarted
    /// </para>
    /// </summary>
    public class HomeViewModel : BlankViewModelBase
    {
        #region Fields

        private bool _isEnabled = false;
        public INavigation _nav;
        public IMessageBoxService _msbs;
        private List<ViewModelBase> _pagesInternes;

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the HomeViewModel class.
        /// </summary>
        public HomeViewModel()
        {
            try
            {
                _nav = SimpleIoc.Default.GetInstance<INavigation>();
                _msbs = SimpleIoc.Default.GetInstance<IMessageBoxService>();
                Singleton single = Singleton.getInstance();
                LabelUtilisateur = single.Admin.ToString();
                Debug.Print("HomeViewModel OK");
                CreateCommands();
                InitNavigation();
                CreateMessengers();
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex); 
            }
        }
        //ctor pour pouvoir faire des tests unitaires sur la navigation
        public HomeViewModel(int i)
        {
            CreateCommands();
        }
        #endregion

        #region Properties

        private string _labelUtilisateur;

        public string LabelUtilisateur
        {
            get { return _labelUtilisateur; }
            set
            {
                if (_labelUtilisateur != value)
                {
                    _labelUtilisateur = value;
                    RaisePropertyChanged("LabelUtilisateur");
                }
            }
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
                {
                    _isEnabled = value;
                    RaisePropertyChanged("IsEnabled");
                }
            }
        }


        /// <summary>
        /// Possibilitée de cliquer sur le bouton connexion 
        /// </summary>
        ///

        public List<ViewModelBase> PagesInternes
        {
            get
            {
                if (_pagesInternes == null)
                    _pagesInternes = new List<ViewModelBase>();

                return _pagesInternes;
            }
        }


        #endregion

        #region Methods

        private void CreateCommands()
        {
            ChangeInternNavigation = new RelayCommand<string>((p) => changeInternView(p));
            MoveToEvalCommand = new RelayCommand(MoveToEval, ExerciceCommandCanExecute);
            MoveToReeducCommand = new RelayCommand(MoveToReeduc, ExerciceCommandCanExecute);
            MoveToEvoEvalCommand = new RelayCommand(MoveToEvoEval, ExerciceCommandCanExecute);
            MoveToEvoReeducCommand = new RelayCommand(MoveToEvoReeduc, ExerciceCommandCanExecute);
            DecoAdminCommand = new RelayCommand(DeconnexionAdmin);
        }

        private bool ExerciceCommandCanExecute()
        {
            if (IsEnabled == true && Singleton.GetRobotError() == false)//on ne peut pas lancer d'exo si erreur robot, pas connecté et pas calibré
                return true;
            else
                return false;
        }

        private void InitNavigation()
        {
            PagesInternes.Add(new FormulairePatientViewModel());
            PagesInternes.Add(new FormulaireInscriptionPatientViewModel("1"));
            PagesInternes.Add(new FormulaireInscriptionPatientViewModel("2"));

            InternView = PagesInternes[0]; 
        }

        public void CreateMessengers()
        {
            Messenger.Default.Register<Singleton>(this, "Singleton", OnConnected);
            Messenger.Default.Register<string>(this, "RaiseCanExecuteHomeVM", RaiseCanExecute);
        }

        private void RaiseCanExecute(string s)
        {
            MoveToEvalCommand.RaiseCanExecuteChanged();
            MoveToEvoReeducCommand.RaiseCanExecuteChanged();
        }

        private void OnConnected(Singleton obj)
        {
            IsEnabled = true;
        }

        private void PostTraitementSupression()
        {
            Messenger.Default.Send(false, "ConnSupp");
            InternView = PagesInternes[0];
        }

        private void ToLoad()
        {
            try
            {
                Messenger.Default.Send(false, "ConnInsc");
                IsEnabled = true;
                InternView = PagesInternes[0];
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
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }

        }

        private void DeconnexionAdmin()
        {
            if((_msbs.ShowYesNo(AxLanguage.Languages.REAplan_Unconnect2,CustomDialogIcons.Question) == CustomDialogResults.Yes))
            {
                try
                {
                    Messenger.Default.Send(false, "DecoUtilisateur");
                    Singleton.logOff();
                    Messenger.Default.Send("n", "StopRobot");//stop le robot et reset l'ecran de jeu
                    Messenger.Default.Send("", "ResetCurentListExercice");
                    _nav.NavigateTo<ConnexionTherapeuteViewModel>(true);
                }
                catch (Exception ex)
                {
                    GestionErreur.GerrerErreur(ex);
                }
            }

        }

        private void MoveToEval()
        {
            try
            {
                _nav.NavigateTo<CalibrationActionViewModel>(this, null, true);
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        private void MoveToEvoEval()
        {
            try
            {
                _nav.NavigateTo<ModifPerceptionOptionViewModel>(this, null, true);
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        private void MoveToReeduc()
        {
            try
            {
                _nav.NavigateTo<ModifActionOptionViewModel>(this, null, true);
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        private void MoveToEvoReeduc()
        {
            try
            {
                _nav.NavigateTo<EvolutionReeducationViewModel>(this, null, true);
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        #endregion

        #region RelayCommand

        public RelayCommand<string> ChangeInternNavigation { get; set; }
        public RelayCommand MoveToEvalCommand { get; set; }
        public RelayCommand MoveToReeducCommand { get; set; }
        public RelayCommand MoveToEvoReeducCommand { get; set; }
        public RelayCommand MoveToEvoEvalCommand { get; set; }
        public RelayCommand DecoCommand { get; set; }
        public RelayCommand DecoAdminCommand { get; set; }
        public RelayCommand TestCommand { get; set; }

        #endregion

        #region Actions

        #endregion
    }
}