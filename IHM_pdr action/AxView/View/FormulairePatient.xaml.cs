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

namespace AxView.View
{
    /// <summary>
    /// Logique d'interaction pour FormulairePatient.xaml
    /// </summary>
    public partial class FormulairePatient : UserControl
    {
        public FormulairePatient()
        {
            InitializeComponent();
        }

        private void buttonCo_LostFocus(object sender, RoutedEventArgs e)
        {
            dataGrid1.SelectedItem = null;
        }
    }
}
