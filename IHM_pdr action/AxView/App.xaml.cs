using System.Windows;
using GalaSoft.MvvmLight.Threading;
using System.Collections.Generic;
using System.Diagnostics;

namespace AxView
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        SingletonApplicationEnforcer mediator = new SingletonApplicationEnforcer(DoSomethingWithArgs);

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow WinInternal = new MainWindow();
            MainWindowP WinExternal = new MainWindowP();

            WinInternal.Show();
            WinExternal.Left = -1920; // -1920
            WinExternal.Top = 0;
            WinExternal.Show();
            WinExternal.Owner = WinInternal;
        }

        static void DoSomethingWithArgs(IEnumerable<string> args)
        {
            foreach (var arg in args)
            {
                Debug.WriteLine("Arg received:" + arg);
            }
            MessageBox.Show("REAplan 2D est déjà lancé!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public App()
        {
            if (mediator.ShouldApplicationExit())
            {
                Shutdown();
            }
        }

        static App()
        {
            DispatcherHelper.Initialize();
        }
    }
}
