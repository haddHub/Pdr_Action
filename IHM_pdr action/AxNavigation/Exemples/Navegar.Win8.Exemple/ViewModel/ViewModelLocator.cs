using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Navegar.Win8.Exemple.Views;
using Windows.UI.Popups;

namespace Navegar.Win8.Exemple.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //Enregistrer la classe de navigation dans l'IOC et les ViewModels
            SimpleIoc.Default.Register<INavigation, Navigation>();
            
            //Enregistrement d'une action à effectuer à chaque navigation
            SimpleIoc.Default.GetInstance<INavigation>().PreviewNavigate += PreNavigate;
            SimpleIoc.Default.GetInstance<INavigation>().NavigationCanceledOnPreviewNavigate += ViewModelLocator_NavigationCanceled;
            SimpleIoc.Default.Register<MainViewModel>();

            //Association des vues avec leur modéle de vue
            SimpleIoc.Default.GetInstance<INavigation>().RegisterView<BlankPage1ViewModel, BlankPage1>();
            SimpleIoc.Default.GetInstance<INavigation>().RegisterView<BlankPage2ViewModel, BlankPage2>();
        }

        async void ViewModelLocator_NavigationCanceled(object sender, EventArgs args)
        {
            var dialog = new MessageDialog("La navigation a été annulée","Navegar.Win8 exemple");
            await dialog.ShowAsync().AsTask();
        }

        private bool PreNavigate(ViewModelBase currentViewModelInstance, Type currentViewModel, Type viewModelToNavigate)
        {
            return true;
        }        

        public MainViewModel Main
        {
            get { return SimpleIoc.Default.GetInstance<MainViewModel>(); }
        }

        public BlankPage1ViewModel BlankPage1ViewModel
        {
            get { return SimpleIoc.Default.GetInstance<INavigation>().GetViewModelInstance<BlankPage1ViewModel>(); }
        }

        public BlankPage2ViewModel BlankPage2ViewModel
        {
            get { return SimpleIoc.Default.GetInstance<INavigation>().GetViewModelInstance<BlankPage2ViewModel>(); }
        }
    }
}