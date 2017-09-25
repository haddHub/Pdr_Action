using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
using Navegar;
using AxViewModel;
using GalaSoft.MvvmLight.Ioc;
using AxModel;
using AxAction;
namespace AxNavigation.Test
{
    [TestFixture]
    class NavigationVisualisationViewModelTest
    {
        Mock<INavigation> navMock;
        Mock<IMessageBoxService> msbMock;
        VisualisationViewModel visuVM;
        [SetUp]
        public void Init()
        {
            SimpleIoc.Default.Register<INavigation, Navigation>();
            SimpleIoc.Default.Register<IMessageBoxService, MessageBoxService>();
            navMock = new Mock<INavigation>();
            msbMock = new Mock<IMessageBoxService>();
            visuVM = new VisualisationViewModel();
            visuVM._nav = navMock.Object;
            visuVM._msbs = msbMock.Object;
        }
        [Test]
        public void Quitter_Exercice_En_Cours_Oui_Vers_HomeViewModel()//Accepte le fait de quitter un exo en cours pour revenir sur le home
        {
            visuVM.canPrecedent = false;
            msbMock.Setup(m => m.ShowYesNo("Voulez-vous vraiment retourner à la page d'accueil ?", CustomDialogIcons.Question)).Returns(CustomDialogResults.Yes);
            visuVM.NavigateToHome();
            navMock.Verify(n => n.NavigateTo<HomeViewModel>(false));
        }
    }
}
