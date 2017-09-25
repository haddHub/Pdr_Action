using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;

namespace Navegar.Win8.Exemple.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region properties

        public RelayCommand GoToPage1Command { get; set; }

        #endregion

        public MainViewModel()
        {
            GoToPage1Command = new RelayCommand(GoToPage1);
        }

        private void GoToPage1()
        {
            SimpleIoc.Default.GetInstance<INavigation>().NavigateTo<BlankPage1ViewModel>();
        }
    }
}