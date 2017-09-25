using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Navegar;
using Moq;
using AxViewModel;
using NUnit.Framework;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight;
using AxModel;

namespace AxNavigation.Test
{
    [TestFixture]
    class NavigationEvaluationViewModelTest
    {
        Mock<INavigation> navMock;
        Mock<IMessageBoxService> msbMock;
        EvaluationViewModel evalVM;

        [SetUp]
        public void Init()
        {
            SimpleIoc.Default.Register<INavigation, Navigation>();
            SimpleIoc.Default.Register<IMessageBoxService, MessageBoxService>();
            navMock = new Mock<INavigation>();
            msbMock = new Mock<IMessageBoxService>();
            evalVM = new EvaluationViewModel(0);
            evalVM._nav = navMock.Object;
            evalVM._messageBoxService = msbMock.Object;
        }

        [Test]
        public void NavigateToHome_Vers_HomeViewModel()
        {
            msbMock.Setup(m => m.ShowYesNo("Voulez-vous vraiment retourner à la page d'accueil ?", CustomDialogIcons.Question)).Returns(CustomDialogResults.Yes);
            evalVM.NavigateToHome();
            navMock.Verify(n => n.NavigateTo<HomeViewModel>(false));
        }
    }
}
