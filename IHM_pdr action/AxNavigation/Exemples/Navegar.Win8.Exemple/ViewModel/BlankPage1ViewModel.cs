using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;

namespace Navegar.Win8.Exemple.ViewModel
{
    public class BlankPage1ViewModel : BlankPageViewModelBase
    {
        #region properties

        public RelayCommand GoToPage2Command { get; set; }
        public RelayCommand GoToPage2WithoutParamCommand { get; set; }
        public string SendToPage { get; set; }
        #endregion

        public BlankPage1ViewModel()
        {
            GoToPage2Command = new RelayCommand(GoToPage2);
            GoToPage2WithoutParamCommand = new RelayCommand(GoToPage2WithoutParam);
        }

        private void GoToPage2()
        {
            SimpleIoc.Default.GetInstance<INavigation>().NavigateTo<BlankPage2ViewModel>(this, null, "Initialized", new object[] { SendToPage }, true);
        }

        private void GoToPage2WithoutParam()
        {
            SimpleIoc.Default.GetInstance<INavigation>().NavigateTo<BlankPage2ViewModel>(this, null, "Initialized", null, true);
        }
    }
}
