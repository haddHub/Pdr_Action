using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Ioc;
using Navegar;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using AxModelExercice;
using AxConfiguration;
using AxError;
using AxModel.Message;

namespace AxViewModel
{
    public class ZoomViewModel : BlankViewModelBase
    {
        #region fields

        public ObservableCollection<ExerciceReeducation> Exercices { get; set; }
        private string _imageZoom = "\\Resources\\Image\\Zoom\\Axi_FondEcran_NT_ZoomGrand.png";
        public INavigation _nav;
        #endregion


        public ZoomViewModel()
        {
            typeZoom = "2";
            CreateCommands();
            _nav = SimpleIoc.Default.GetInstance<INavigation>();
            Messenger.Default.Register<RobotErrorMessage>(this, "NewRobotError", OnRobotError);
        }

        public string ImageZoom
        {
            get
            {
                return _imageZoom;
            }
            set
            {
                _imageZoom = value;
                RaisePropertyChanged("ImageZoom");
            }
        }

        #region Methods

        public void CreateCommands()
        {
            ZoomCommand = new RelayCommand<string>((p) => changeImageZoom(p));
            ValiderReeducationCommand = new RelayCommand(ValidReeducation);
            MainViewModelCommand = new RelayCommand(NavigateToHome);
            PreviousViewModelCommand = new RelayCommand(GoBack, CanGoBack);
        }

        private bool CanGoBack()
        {
            return SimpleIoc.Default.GetInstance<INavigation>().CanGoBack();
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

        private string typeZoom;
        public void changeImageZoom(string p)
        {
            try
            {
                typeZoom = p;

                if (p == "0")
                    ImageZoom = "\\Resources\\Image\\Zoom\\Axi_FondEcran_NT_ZoomPetit.png";
                if (p == "1")
                    ImageZoom = "\\Resources\\Image\\Zoom\\Axi_FondEcran_NT_ZoomMoyen.png";
                if (p == "2")
                    ImageZoom = "\\Resources\\Image\\Zoom\\Axi_FondEcran_NT_ZoomGrand.png";
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        public void LoadEx(ObservableCollection<ExerciceReeducation> exerc)
        {
            this.Exercices = exerc;
        }

        void ValidReeducation()
        {
            try
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    
                }), DispatcherPriority.Normal);
                foreach (var item in Exercices)
                {
                    if (typeZoom == "0")
                        item.BorneConfig = ExerciceConfig.GetSmallBorne();
                    if (typeZoom == "1")
                        item.BorneConfig = ExerciceConfig.GetMediumBorne();
                    if (typeZoom == "2")
                        item.BorneConfig = ExerciceConfig.GetBigBorne();
                }
                List<ExerciceGeneric> listExGen = new List<ExerciceGeneric>(Exercices);
                //_nav.NavigateTo<VisualisationViewModel>(this, null, false);
                Messenger.Default.Send(listExGen, "ReeducationKidWizardViewModel");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message, "Erreur !", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void NavigateToHome()
        {
            try
            {
                if (MessageBox.Show(AxLanguage.Languages.REAplan_Accueil_Confirmation, AxLanguage.Languages.REAplan_Confirmation, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _nav.NavigateTo<HomeViewModel>(false); 
                }
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        private void OnRobotError(RobotErrorMessage e)
        {
            if (_nav.GetTypeViewModelToBack() == typeof(ReeducationViewViewModel))
                NavigateToHome();
        }

        #endregion

        #region Commands

        public RelayCommand<string> ZoomCommand { get; set; }
        public RelayCommand ValiderReeducationCommand { get; set; }
        public RelayCommand MainViewModelCommand { get; set; }
        public RelayCommand PreviousViewModelCommand { get; set; }

        #endregion
    }
}
