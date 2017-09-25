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
    class NavigationMainViewModelTest
    {
        Mock<INavigation> navMock;
        Mock<IMessageBoxService> msbMock;
        Mock<ActionRobot> robotMock;
        MainViewModel mainVM; 
        [SetUp]
        public void Init()
        {
            SimpleIoc.Default.Register<INavigation, Navigation>();
            SimpleIoc.Default.Register<IMessageBoxService, MessageBoxService>();
            SimpleIoc.Default.Register<ActionRobot, ActionRobot>();
            navMock = new Mock<INavigation>();
            msbMock = new Mock<IMessageBoxService>();
            robotMock = new Mock<ActionRobot>();
            mainVM = new MainViewModel();
            mainVM._nav = navMock.Object;
            mainVM._messageBoxService = msbMock.Object;
            mainVM._axrobot = robotMock.Object;
            Singleton.identification();
            Singleton.getInstance().Patient = new ListePatientDataGrid("ef", "fe", "1255");
        }
        [Test]
        public void DecoPatient_Oui_Vers_HomeViewModel()//Accepter la déco du patient navigue vers HomeViewModel
        {
            msbMock.Setup(m => m.ShowYesNo(AxLanguage.Languages.REAplan_Unconnect_Patient2, CustomDialogIcons.Question)).Returns(CustomDialogResults.Yes);
            mainVM.DecoCommand.Execute(null); 
            navMock.Verify(n => n.NavigateTo<HomeViewModel>(true));
        }
    }
}
