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
using AxModel;
namespace AxView.Resources.Elements
{
    /// <summary>
    /// Logique d'interaction pour PositionControl.xaml
    /// </summary>
    public partial class PositionControl : UserControl
    {
        #region Fields

        #endregion

        public PositionControl()
        {
            InitializeComponent();

            this.Loaded += ForceUi_Loaded;
        }

        void ForceUi_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public static readonly DependencyProperty PosProperty =
            DependencyProperty.Register("Pos", typeof(PositionDataModel), typeof(PositionControl), new UIPropertyMetadata(new PositionDataModel()));

        public PositionDataModel Pos
        {
            get
            {
                return (PositionDataModel)GetValue(PosProperty);  // create a new instance of PointCollection for binding
            }
            set
            {
                SetValue(PosProperty, value);
                PolylineLive.Points.Add(new Point(value.PositionX, value.PositionY));
                PolylineLive.Points.Add(new Point(50.0, 50.0));
                SetValue(PosXProperty, value.PositionX);
                SetValue(PosYProperty, value.PositionY);
            }
        }

        public static readonly DependencyProperty PosXProperty =
            DependencyProperty.Register("PosX", typeof(double), typeof(PositionControl), new UIPropertyMetadata(0d));

        public double PosX
        {
            get
            {
                return (double)GetValue(PosXProperty);  // create a new instance of PointCollection for binding
            }
            set
            {
                SetValue(PosXProperty, value);
            }
        }

        public static readonly DependencyProperty PosYProperty =
            DependencyProperty.Register("PosY", typeof(double), typeof(PositionControl), new UIPropertyMetadata(0d));

        public double PosY
        {
            get
            {
                return (double)GetValue(PosYProperty);  // create a new instance of PointCollection for binding
            }
            set
            {
                SetValue(PosYProperty, value);
            }
        }

        //public static readonly DependencyProperty ForceXAddPositionRobotLive =
        //    DependencyProperty.Register("AddPositionRobotLive", typeof(PositionDataModel), typeof(PositionControl), new UIPropertyMetadata(0d));

        //public void AddPositionRobotLive(PositionDataModel posData)
        //{
        //    double dynX, dynY;

        //        dynX = (posData.PositionX);
        //        dynY = (posData.PositionY);
        //    PositionRobotLiveProperty.
        //        PositionRobotLiveProperty.Add(new Point(dynX, dynY));

        //        if (PositionRobotLive.Count >= 100)
        //        {
        //            PositionRobotLiveProperty.RemoveAt(0);
        //        }
        //}
    }
}
