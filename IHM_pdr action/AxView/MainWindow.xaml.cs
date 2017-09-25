using System.Windows;
using AxViewModel;
using Microsoft.Windows.Controls.Ribbon;
using System;
using AxView.View;
using AxLanguage;
using AxError;
namespace AxView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        /// <summary>
        /// Full screen state
        /// </summary>
        private bool IsFullScreen { get; set; }

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
            this.WindowState = WindowState.Maximized;
            IsFullScreen = true;
        }

        /// <summary>
        /// full screen button click handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonScreen_Click(object sender, RoutedEventArgs e)
        {
            SwapFullScreenMode();
        }

        /// <summary>
        /// Change the full screen state
        /// </summary>
        private void SwapFullScreenMode()
        {
            try
            {
                if (IsFullScreen)
                {
                    this.WindowState = WindowState.Normal;
                    IsFullScreen = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                    IsFullScreen = true;
                }
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        private void ButtonAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutView fenetreApropos = new AboutView(this);
            fenetreApropos.ShowDialog();
        }
    }
}