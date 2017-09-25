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
    /// Logique d'interaction pour FormulaireTherapeute.xaml
    /// </summary>
    public partial class FormulaireTherapeute : UserControl
    {
        public FormulaireTherapeute()
        {
            InitializeComponent();
        }

        private void tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnCo.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }
    }
}
