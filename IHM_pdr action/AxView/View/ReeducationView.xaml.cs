using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Messaging;

namespace AxView.View
{
    /// <summary>
    /// Logique d'interaction pour ReeducationKidWizardView.xaml
    /// </summary>
    public partial class ReeducationView : UserControl
    {
        public ReeducationView()
        {
            InitializeComponent();
            Messenger.Default.Register<string>("","DefilementListe", DefilementListe);
            Messenger.Default.Register<string>("", "DefilementListeSupp", DefilementListeSupp);
        }

        private void DefilementListe(string s)
        {
            if (test == null) throw new ArgumentNullException("listbox", "Argument listbox cannot be null");
            if (!test.IsInitialized) throw new InvalidOperationException("ListBox is in an invalid state: IsInitialized == false");

            if (test.Items.Count == 0)
            {
                return;
            }

            test.ScrollIntoView(test.Items[test.Items.Count - 1]);
        }

        private void DefilementListeSupp(string s)
        {
            if(test.SelectedIndex != test.Items.Count - 1)
                test.SelectedIndex = test.Items.Count - 1;
        }
    }
}
