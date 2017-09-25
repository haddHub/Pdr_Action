/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:REAplan2Db.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using AxModel;
using AxCommunication;
using AxAction;
using System;
using AxError;
using Navegar;
using Microsoft.Practices.ServiceLocation;


namespace AxViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            try
            {
                ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

                //1. Enregistrer la classe de navigation dans l'IOC
                SimpleIoc.Default.Register<ActionRobot, ActionRobot>();
                SimpleIoc.Default.Register<IMessageBoxService, MessageBoxService>();
                SimpleIoc.Default.Register<INavigation, Navigation>();
                SimpleIoc.Default.Register<IUnityGameController, UnityGameController>();

                //2. Générer le viewmodel principal, le type du viewmodel
                //peut être n'importe lequel
                //Cette génération va permettre de créer au sein de la 
                //navigation, une instance unique pour le viewmodel principal,
                //qui sera utilisée par la classe de navigation
                SimpleIoc.Default.GetInstance<INavigation>()
                                 .GenerateMainViewModelInstance<MainViewModel>();
                SimpleIoc.Default.Register<MainPViewModel>();
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        public MainViewModel Main
        {
            get
            {
                return SimpleIoc.Default.GetInstance<INavigation>()
                .GetMainViewModelInstance<MainViewModel>();
            }
        }

        public MainPViewModel MainP
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainPViewModel>();
            }
        }



        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}