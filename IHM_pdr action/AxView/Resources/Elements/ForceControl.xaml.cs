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

namespace AxView.Resources.Elements
{
    /// <summary>
    /// Logique d'interaction pour ForceControl.xaml
    /// </summary>
    public partial class ForceControl : UserControl
    {
        public ForceControl()
        {
            InitializeComponent();

            this.Loaded += ForceUi_Loaded;
        }

        void ForceUi_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        public static readonly DependencyProperty ForceXProperty =
            DependencyProperty.Register("ForceX", typeof(double), typeof(ForceControl), new UIPropertyMetadata(0d));

        public double ForceX
        {
            get
            {
                return (double)GetValue(ForceXProperty);
            }
            set
            {
                SetValue(ForceXProperty, value);
            }
        }

        public static readonly DependencyProperty ForceYProperty =
            DependencyProperty.Register("ForceY", typeof(double), typeof(ForceControl), new UIPropertyMetadata(0d));

        public double ForceY
        {
            get
            {
                return (double)GetValue(ForceYProperty);
            }
            set
            {
                SetValue(ForceYProperty, value);
            }
        }
    }
}
