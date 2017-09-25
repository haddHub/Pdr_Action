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
    public class NavigationHomeViewModelTest
    {
        Mock<INavigation> navMock;
        Mock<IMessageBoxService> msbMock;
        HomeViewModel homeVM;

        [SetUp]
        public void Init()
        {
            SimpleIoc.Default.Register<INavigation, Navigation>();
            SimpleIoc.Default.Register<IMessageBoxService, MessageBoxService>();
            navMock = new Mock<INavigation>();
            msbMock = new Mock<IMessageBoxService>();

            Singleton.identificationAdmin();
            Singleton.getInstance().Admin = new Admin("test", "test", "test", "test");
            homeVM = new HomeViewModel(0);
            homeVM._nav = navMock.Object;
            homeVM._msbs = msbMock.Object;
        }
        [Test]
        public void DecoThérapeute_Oui_Vers_ConnexionTherapeuteViewModel()//Accepter la déco du thérapeute navigue vers ConnexionTherapeuteViewModel
        {
            msbMock.Setup(m => m.ShowYesNo("Êtes-vous sur de vouloir vous déconnecter ?", CustomDialogIcons.Question)).Returns(CustomDialogResults.Yes);
            homeVM.DecoAdminCommand.Execute(null);
            navMock.Verify(n => n.NavigateTo<ConnexionTherapeuteViewModel>(true));
        }
        
    }
}
