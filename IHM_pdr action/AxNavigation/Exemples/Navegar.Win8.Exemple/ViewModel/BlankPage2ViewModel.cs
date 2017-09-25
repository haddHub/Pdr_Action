using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;

namespace Navegar.Win8.Exemple.ViewModel
{
    public class BlankPage2ViewModel : BlankPageViewModelBase
    {
        #region properties

        public string ReceiveFromPage { get; set; }
        public RelayCommand GoBackCommand { get; set; }
        #endregion

        public BlankPage2ViewModel()
        {
            GoBackCommand = new RelayCommand(GoBack, CanGoBack);
        }

        public void Initialized()
        {
            ReceiveFromPage = "Sans paramètres";
        }

        public void Initialized(string receive)
        {
            ReceiveFromPage = receive;
        }

        private bool CanGoBack()
        {
            return SimpleIoc.Default.GetInstance<INavigation>().CanGoBack();
        }

        private void GoBack()
        {
            if (SimpleIoc.Default.GetInstance<INavigation>().GetTypeViewModelToBack() == typeof(BlankPage1ViewModel))
            {
                SimpleIoc.Default.GetInstance<INavigation>().GoBack("SetIsRetour", new object[] { true });    
            }
            else
            {
                SimpleIoc.Default.GetInstance<INavigation>().GoBack();    
            }
        }
    }
}
