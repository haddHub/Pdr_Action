using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using AxModel.Message;
using AxModel;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

namespace AxViewModel
{
    class EvaluationMainViewModel : ViewModelBase, IPageViewModel
    {

        #region Fields

        RelayCommand _cancelCommand;
        ReaPlanExercices _reaPlanExercices;
        WizardPageViewModelBase _currentPage;
        RelayCommand _moveNextCommand;
        RelayCommand _movePreviousCommand;
        ReadOnlyCollection<WizardPageViewModelBase> _pages;
        private double _pagesNbrs = 0;
        private double _pagesActu = 1;
        private const string LabelPatientPropertyName = "LabelPatient";
        private string _labelPatient = null;

        private const string PatientValuePropertyName = "PatientValue";
        private ListePatientDataGrid _patientValue = null;

        #endregion  // Fields

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the ReeducationWizardViewModel class.
        /// </summary>
        public EvaluationMainViewModel()
        {
            Messenger.Default.Register<PatientMessage>(this, OnRegister);
            _reaPlanExercices = new ReaPlanExercices();
            this.CurrentPage = this.Pages[0];
            //Messenger.Default.Register<bool>(this, "ReeducationKidWizardSelectExViewModel", EnableMovaToNextPage);     // Message pour activer NextCommand
            //Debug.Print("ReeducationKidWizardViewModel OK");
        }
        

        #endregion

        #region Properties
        public ListePatientDataGrid PatientValue
        {
            get { return _patientValue; }
            set
            {
                if (_patientValue == value)
                {
                    _patientValue = value;
                }

                RaisePropertyChanging(PatientValuePropertyName);
                _patientValue = value;
                RaisePropertyChanged(PatientValuePropertyName);

            }
        }


        public string LabelPatient
        {
            get { return _labelPatient; }
            set
            {
                if (_labelPatient == value)
                {
                    _labelPatient = value;
                }

                RaisePropertyChanging(LabelPatientPropertyName);
                _labelPatient = value;
                RaisePropertyChanged(LabelPatientPropertyName);
            }
        }

        public WizardPageViewModelBase CurrentPage
        {
            get { return _currentPage; }
            private set
            {
                if (value == _currentPage)
                    return;

                if (_currentPage != null)
                    _currentPage.IsCurrentPage = false;

                _currentPage = value;

                if (_currentPage != null)
                    _currentPage.IsCurrentPage = true;

                this.RaisePropertyChanged("CurrentPage");
                this.RaisePropertyChanged("IsOnLastPage");
            }
        }

        /// <summary>
        /// Returns true if the user is currently viewing the last page 
        /// in the workflow. This property is used by WizardView
        /// to switch the Next button's text to "Finish" when the user
        /// has reached the final page.
        /// </summary>
        public bool IsOnLastPage
        {
            get { return this.CurrentPageIndex == this.Pages.Count - 1; }
        }

        /// <summary>
        /// Returns a read-only collection of all page ViewModels.
        /// </summary>
        public ReadOnlyCollection<WizardPageViewModelBase> Pages
        {
            get
            {
                if (_pages == null)
                    this.CreatePages();

                return _pages;
            }
        }

        int CurrentPageIndex
        {
            get
            {

                if (this.CurrentPage == null)
                {
                    Debug.Fail("Why is the current page null?");
                    return -1;
                }

                return this.Pages.IndexOf(this.CurrentPage);
            }
        }

        public double PagesActu
        {
            get
            {
                return this._pagesActu;
            }
            set
            {
                this._pagesActu = value;
                RaisePropertyChanged("PagesActu");
            }
        }

        public double PagesNbrs
        {
            get
            {
                return this._pagesNbrs;
            }
            set
            {
                this._pagesNbrs = value;
                RaisePropertyChanged("PagesNbrs");
            }
        }

        #endregion

        #region Methods
        void CreatePages()
        {
            var homeVM = new EvaluationChoixViewModel();
            var visuVM = new EvaluationVisuViewModel();
            var finalVM = new EvaluationResutatViewModel();

            var pages = new List<WizardPageViewModelBase>();

            pages.Add(homeVM);
            pages.Add(visuVM);
            pages.Add(finalVM);

            _pages = new ReadOnlyCollection<WizardPageViewModelBase>(pages);
            PagesNbrs = pages.Count;
        }

        private void OnRegister(PatientMessage obj)
        {
            PatientValue = obj.DataPatient;
            LabelPatient = PatientValue.Nom + " " + PatientValue.Prenom;
        }

        void OnRequestClose()
        {
            try
            {
                this.CurrentPage = this.Pages[0];
                PagesActu = 1;
                MovePreviousCommand.RaiseCanExecuteChanged();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message, "Erreur !", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public string Name
        {
            get { return "Evaluation"; }
        }
        #endregion

        #region RelayCommand

        /// <summary>
        /// Returns the command which, when executed, cancels the order 
        /// and causes the Wizard to be removed from the user interface.
        /// </summary>
        public RelayCommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                    _cancelCommand = new RelayCommand(() => this.CancelOrder());

                return _cancelCommand;
            }
        }

        /// <summary>
        /// Returns the command which, when executed, causes the CurrentPage 
        /// property to reference the previous page in the workflow.
        /// </summary>
        public RelayCommand MovePreviousCommand
        {
            get
            {
                if (_movePreviousCommand == null)
                    _movePreviousCommand = new RelayCommand(
                        () => this.MoveToPreviousPage(),
                        () => this.CanMoveToPreviousPage);

                return _movePreviousCommand;
            }
        }

        bool CanMoveToPreviousPage
        {
            get { return 0 < this.CurrentPageIndex; }
        }

        /// <summary>
        /// Returns the command which, when executed, causes the CurrentPage 
        /// property to reference the next page in the workflow.  If the user
        /// is viewing the last page in the workflow, this causes the Wizard
        /// to finish and be removed from the user interface.
        /// </summary>
        public RelayCommand MoveNextCommand
        {
            get
            {
                if (_moveNextCommand == null)
                    _moveNextCommand = new RelayCommand(
                        () => this.MoveToNextPage(),
                        () => this.CanMoveToNextPage);

                return _moveNextCommand;
            }
        }

        bool CanMoveToNextPage
        {
            get { return this.CurrentPage != null && this.CurrentPage.IsValid(); }
        }

        void EnableMovaToNextPage(bool bo)
        {
            MoveNextCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Actions

        void CancelOrder()
        {
            _reaPlanExercices = null;
        }

        void MoveToPreviousPage()
        {
            if (this.CanMoveToPreviousPage)
            {
                PagesActu--;
                this.CurrentPage = this.Pages[this.CurrentPageIndex - 1];
                MovePreviousCommand.RaiseCanExecuteChanged();
                MoveNextCommand.RaiseCanExecuteChanged();
            }
        }

        void MoveToNextPage()
        {
            if (this.CanMoveToNextPage)
            {
                if (this.CurrentPageIndex < this.Pages.Count - 1)
                {
                    this.CurrentPage = this.Pages[this.CurrentPageIndex + 1];
                    PagesActu++;
                    MovePreviousCommand.RaiseCanExecuteChanged();
                    MoveNextCommand.RaiseCanExecuteChanged();
                }
                else
                {
                    this.OnRequestClose();
                }
            }
        }

        #endregion
    }
}
