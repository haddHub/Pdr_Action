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
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing.Printing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Drawing;
using AxData;
using AxError;
namespace AxView.View
{
    /// <summary>
    /// Logique d'interaction pour EvoEval.xaml
    /// </summary>
    public partial class EvoEval : UserControl
    {
        //Fields
        private Singleton singleUser = Singleton.getInstance();
        private int zoneLibre = 1;
        private int countcheck;

        //Constructeurs
        public EvoEval()
        {
            try
            {
                InitializeComponent();
                NumPage.Content = "1";
                ch1.IsChecked = true;
                ch2.IsChecked = true;
                ch3.IsChecked = true;
                ch4.IsChecked = true;
                ch5.IsChecked = true;


                btPrev.IsEnabled = false;
                btNext.IsEnabled = false;
                // Type de graphique (Cycle ou Coefficient)
                // Zone de possition du graphique (Chart1,Chart2,...,Chart5)
                // Exercice (Target, Square,...)
                // Resultat (Speed, Jerk,...)
                // Titre du graphique
                // Affichage axe X
                // Affichage axe Y
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }

        }

        //Méthode qui supprime un graphique
        private void RemoveGraph(string zone)
        {
            Chart chart = this.FindName(zone) as Chart;
            chart.ChartAreas.Clear();
            chart.Series.Clear();
            chart.Titles.Clear();
            chart.BackColor = System.Drawing.Color.Transparent;
        }

        //Méthode d'appel pouyr créer un graph
        private void Graphique(string type, string zone, string exercice, int resultat, string titre, string x, string y)
        {
            try
            {

                RemoveGraph(zone);
                double[] tabnorme = new double[2];
                List<PointEvoEval> val = new List<PointEvoEval>();

                if (type == "Cycle")
                {
                    val = ResultatEvalData.ReturnVal(resultat);
                }
                else
                {
                    val = ResultatEvalData.CoefficientVar(resultat-1);
                }

                ResultatEvalData.returnNorme(tabnorme, resultat);

                Chart chart = this.FindName(zone) as Chart;
                chart.BackColor = System.Drawing.Color.WhiteSmoke;
                StripLine sp = new StripLine();

                sp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(241)))), ((int)(((byte)(185)))), ((int)(((byte)(168)))));
                sp.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                sp.IntervalOffset = 20;
                sp.StripWidth = 50;
                sp.TextLineAlignment = System.Drawing.StringAlignment.Far;
                sp.IntervalOffset = tabnorme[0] - tabnorme[1];
                sp.StripWidth = 2.0 * tabnorme[1];


                StripLine sp2 = new StripLine();
                sp2.BackColor = System.Drawing.Color.Orange;
                sp2.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                sp2.IntervalOffset = 20;
                sp2.StripWidth = 50;
                sp2.TextLineAlignment = System.Drawing.StringAlignment.Far;
                sp2.IntervalOffset = tabnorme[0];
                sp2.StripWidth = tabnorme[0] / 100000;


                ChartArea area = new ChartArea();
                area.Area3DStyle.Inclination = 15;
                area.Area3DStyle.IsClustered = true;
                area.Area3DStyle.IsRightAngleAxes = false;
                area.Area3DStyle.Perspective = 10;
                area.Area3DStyle.Rotation = 10;
                area.Area3DStyle.WallWidth = 0;
                area.AxisX.LabelStyle.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold);


                area.AxisX.Title = x;
                area.AxisY.Title = y;
                area.AxisX.MajorGrid.Enabled = false;
                area.AxisY.MajorGrid.Enabled = false;

                area.AxisX.IsLabelAutoFit = true;
                area.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.LabelsAngleStep30;
                area.AxisX.LabelStyle.Enabled = true;
                area.AxisX.Interval = 1;

                area.Name = "area";
                area.AxisY.StripLines.Add(sp);
                area.AxisY.StripLines.Add(sp2);

                //area.AxisX.IsMarginVisible = false;

                chart.ChartAreas.Add(area);
                if (val.Count != 0)
                {
                    Series s1 = new Series();

                    Series s3 = new Series();

                    s1.IsXValueIndexed = true;

                    s3.IsXValueIndexed = true;

                    s1.ChartType = SeriesChartType.ErrorBar;

                    s3.ChartType = SeriesChartType.Point;


                    s1.XValueType = ChartValueType.DateTime;
                    s3.XValueType = ChartValueType.DateTime;

                    s1.MarkerStyle = MarkerStyle.None;
                    s1["PointWidth"] = "0.04";
                    s1["ErrorBarCenterMarkerStyle"] = "Circle";
                    s1.MarkerSize = 8;
                    //s2.MarkerSize = 1;
                    s3.MarkerSize = 0;
                    //DateTime date2 = new DateTime();


                    foreach (var el in val)
                    {
                        DataPoint p2 = new DataPoint();
                        //date2 = el.Date.AddDays(-1);
                        p2.SetValueXY(el.Date.ToOADate(), tabnorme[0]);
                        DataPoint p = new DataPoint();
                        //p.SetValueXY(el.Date.ToOADate(), el.Moyenne, el.Moyenne + el.EcartT, el.Moyenne - el.EcartT);
                        s1.Points.Add(p);

                        DataPoint p3 = new DataPoint();
                        p3.SetValueXY(el.Date.ToOADate(), tabnorme[0] + (tabnorme[1] * 2));
                        s3.Points.Add(p3);

                    }
                    //DataPoint p3 = new DataPoint();
                    //DateTime date3 = date2.AddDays(2);
                    //p3.SetValueXY(date3, tabnorme[0]);
                    //s2.Points.Add(p3);
                    //DataPoint p4 = new DataPoint();
                    //p4.SetValueXY(date3, tabnorme[0] + (tabnorme[1] * 2));
                    //s3.Points.Add(p4);
                    s1.Name = "Patient";
                    if (type == "Coefficient")
                    {
                        s1.MarkerStyle = MarkerStyle.Circle;
                    }
                    chart.Series.Add(s1);
                    chart.Series.Add(s3);
                    chart.TabIndex = 0;
                }
                Title t1 = new Title();
                t1.Text = titre;
                t1.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold);
                t1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(59)))), ((int)(((byte)(105)))));
                t1.Name = "Title1";
                t1.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                chart.Titles.Add(t1);
            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }
        }

        //Méthode qui vérifie les composants CheckBox qui sont checks et appelle la méthode précédente en fonction
        private void verifCheck()
        {
            CheckBox Ch1 = this.FindName("ch1") as CheckBox;
            CheckBox Ch2 = this.FindName("ch2") as CheckBox;
            CheckBox Ch3 = this.FindName("ch3") as CheckBox;
            CheckBox Ch4 = this.FindName("ch4") as CheckBox;
            CheckBox Ch5 = this.FindName("ch5") as CheckBox;
            CheckBox Ch6 = this.FindName("ch6") as CheckBox;
            CheckBox Ch7 = this.FindName("ch7") as CheckBox;
            CheckBox Ch8 = this.FindName("ch8") as CheckBox;
            CheckBox Ch9 = this.FindName("ch9") as CheckBox;
            CheckBox Ch10 = this.FindName("ch10") as CheckBox;
            CheckBox Ch11 = this.FindName("ch11") as CheckBox;
            CheckBox Ch12 = this.FindName("ch12") as CheckBox;
            CheckBox Ch13 = this.FindName("ch13") as CheckBox;
            CheckBox Ch14 = this.FindName("ch14") as CheckBox;
            CheckBox Ch15 = this.FindName("ch15") as CheckBox;
            CheckBox Ch16 = this.FindName("ch16") as CheckBox;
            CheckBox Ch17 = this.FindName("ch17") as CheckBox;
            CheckBox Ch18 = this.FindName("ch18") as CheckBox;
            CheckBox Ch19 = this.FindName("ch19") as CheckBox;
            CheckBox Ch20 = this.FindName("ch20") as CheckBox;
            CheckBox Ch21 = this.FindName("ch21") as CheckBox;
            CheckBox Ch22 = this.FindName("ch22") as CheckBox;
            CheckBox Ch23 = this.FindName("ch23") as CheckBox;
            CheckBox Ch24 = this.FindName("ch24") as CheckBox;
            CheckBox Ch25 = this.FindName("ch25") as CheckBox;
            CheckBox Ch26 = this.FindName("ch26") as CheckBox;
            CheckBox Ch27 = this.FindName("ch27") as CheckBox;
            CheckBox Ch28 = this.FindName("ch28") as CheckBox;
            CheckBox Ch29 = this.FindName("ch29") as CheckBox;
            CheckBox Ch30 = this.FindName("ch30") as CheckBox;
            CheckBox Ch31 = this.FindName("ch31") as CheckBox;
            CheckBox Ch32 = this.FindName("ch32") as CheckBox;
            CheckBox Ch33 = this.FindName("ch33") as CheckBox;
            CheckBox Ch34 = this.FindName("ch34") as CheckBox;
            CheckBox Ch35 = this.FindName("ch35") as CheckBox;
            CheckBox Ch36 = this.FindName("ch36") as CheckBox;
            CheckBox Ch37 = this.FindName("ch37") as CheckBox;
            CheckBox Ch38 = this.FindName("ch38") as CheckBox;
            CheckBox Ch39 = this.FindName("ch39") as CheckBox;
            CheckBox Ch40 = this.FindName("ch40") as CheckBox;
            CheckBox Ch41 = this.FindName("ch41") as CheckBox;
            CheckBox Ch42 = this.FindName("ch42") as CheckBox;
            CheckBox Ch43 = this.FindName("ch43") as CheckBox;
            CheckBox Ch44 = this.FindName("ch44") as CheckBox;
            Label NumPage = this.FindName("NumPage") as Label;

            if (zoneLibre <= 6)
            {
                RemoveGraph("Chart1");
                RemoveGraph("Chart2");
                RemoveGraph("Chart3");
                RemoveGraph("Chart4");
                RemoveGraph("Chart5");
            }
            List<CheckBox> ListCh = new List<CheckBox>{ Ch1, Ch2,Ch3,Ch4,Ch5,Ch6,Ch7,Ch8,Ch9,Ch10,Ch11,Ch12,Ch13,Ch14,Ch15,Ch16,Ch17,Ch18,Ch19,Ch20,
                                                        Ch21, Ch22,Ch23,Ch24,Ch25,Ch26,Ch27,Ch28,Ch29,Ch30,Ch31,Ch32,Ch33,Ch34,Ch35,Ch36,Ch37,Ch38,Ch39,Ch40,
                                                        Ch41,Ch42,Ch43,Ch44};
            countcheck = 0;

            foreach (CheckBox el in ListCh)
            {
                if (el.IsChecked == true)
                {
                    countcheck++;
                }
            }
            double nbrP = Math.Ceiling((double)countcheck / 5);
            if (nbrP == 0)
            { nbrP = 1; }
            int posi = int.Parse((string)NumPage.Content);
            if (posi > nbrP)
            { posi = (int)nbrP; NumPage.Content = posi.ToString(); }
            if (nbrP > 1)
            { btNext.IsEnabled = true; }
            else
            { btNext.IsEnabled = false; }
            if (posi == nbrP)
            { btNext.IsEnabled = false; }
            if (posi == 1)
            { btPrev.IsEnabled = false; }

            this.NbPage.Content = nbrP.ToString();
            zoneLibre = 1;
            string zone = "Chart" + zoneLibre;


            int page = (posi - 1) * 5;

            int compteur = 0;

            if (Ch1.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Cycle", zone, "FreeAmplitude", 1, "Free amplitude - Amplitude", "", "Amplitude (cm)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch6.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Coefficient", zone, "FreeAmplitude", 2, "Free amplitude - CV amplitude", "", " CV amplitude(%)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch9.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Cycle", zone, "FreeAmplitude", 7, "Free amplitude - Straightness", "", "Straightness"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch2.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Coefficient", zone, "FreeAmplitude", 8, "Free amplitude - CV strainghtness", "", "CV straightness (%)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch7.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Cycle", zone, "FreeAmplitude", 3, "Free amplitude - Speed", "", "Speed (cm/s)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch8.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Coefficient", zone, "FreeAmplitude", 4, "Free amplitude - CV speed", "", " CV speed(%)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch10.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Cycle", zone, "FreeAmplitude", 5, "Free amplitude - Peak speed", "", "Peak speed (cm/s)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch11.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Coefficient", zone, "FreeAmplitude", 6, "Free amplitude - CV peak speed", "", "CV peak speed (%)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch12.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Cycle", zone, "FreeAmplitude", 13, "Free amplitude - jerk metric", "", "Jerk metric (1/s²)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch13.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Coefficient", zone, "FreeAmplitude", 14, "Free amplitude - CV jerk metric", "", "CV jerk metric (%)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch14.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Cycle", zone, "FreeAmplitude", 11, "Free amplitude - speed metric", "", "Speed metric)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch15.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Coefficient", zone, "FreeAmplitude", 12, "Free amplitude - CV speed metric", "", "CV speed metric (%)"); zoneLibre++; zone = "Chart" + zoneLibre; } }


            if (Ch16.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Cycle", zone, "Target", 21, "Target - Accuracy", "", "Accuracy (cm)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch17.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Coefficient", zone, "Target", 22, "Target - CV accuracy", "", "CV accuracy (%)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch20.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Cycle", zone, "Target", 19, "Target - Straightness", "", "Straightness"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch21.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Coefficient", zone, "Target", 20, "Target - CV straightness", "", "CV straightness (%)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch18.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Cycle", zone, "Target", 33, "Target - Speed", "", "Speed (cm/s)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch19.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Coefficient", zone, "Target", 34, "Target - CV speed", "", "CV speed (%)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch22.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Cycle", zone, "Target", 23, "Target - Peak speed", "", "Peak speed (cm/s)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch23.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Coefficient", zone, "Target", 24, "Target - CV peak speed", "", "CV peak speed (%)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch24.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Cycle", zone, "Target", 25, "Target - Jerk metric", "", "Jerk metric (1/s²)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch25.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Coefficient", zone, "Target", 26, "Target - CV jerk metric", "", "CV jerk metric (%)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch3.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Cycle", zone, "Target", 27, "Target - speed metric", "", "Speed metric"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch26.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Coefficient", zone, "Target", 28, "Target - CV speed metric", "", "CV speed metric (%)"); zoneLibre++; zone = "Chart" + zoneLibre; } }


            if (Ch34.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Cycle", zone, "Square", 53, "Square - Shape accuracy", "", "Shape accuracy (cm)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch35.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Coefficient", zone, "Square", 54, "Square -CV shape accuracy", "", "CV shape accuracy (%)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch27.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Cycle", zone, "Square", 45, "Square - Speed", "", "Speed (cm/s)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch28.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Coefficient", zone, "Square", 46, "Square - CV speed", "", "CV speed (%)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch29.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Cycle", zone, "Square", 47, "Square - Peak speed", "", "Peak speed (cm/s)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch30.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Coefficient", zone, "Square", 48, "Square - CV peak speed", "", "CV peak speed (%)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch31.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Cycle", zone, "Square", 49, "Square - jerk metric", "", "Jerk metric (1/s²)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch4.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Coefficient", zone, "Square", 50, "Square - CV jerk metric", "", "CV jerk metric(%)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch32.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Cycle", zone, "Square", 51, "Square - speed metric", "", "Speed metric"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch33.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Coefficient", zone, "Square", 52, "Square - CV speed metric", "", "CV speed metric (%)"); zoneLibre++; zone = "Chart" + zoneLibre; } }


            if (Ch43.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Cycle", zone, "Circle", 43, "Circle - Shape accuracy", "", "Shape accuracy (cm)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch44.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Coefficient", zone, "Circle", 44, "Circle - CV shape accuracy", "", "CV shape accuracy (%)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch36.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Cycle", zone, "Circle", 35, "Circle - Speed", "", "Speed (cm/s)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch37.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Coefficient", zone, "Circle", 36, "Circle - CV speed", "", "CV speed (%)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch38.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Cycle", zone, "Circle", 37, "Circle - Peak speed", "", "Peak speed (cm/s)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch39.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Coefficient", zone, "Circle", 38, "Circle - CV peak speed", "", "CV peak speed (%)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch40.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Cycle", zone, "Circle", 39, "Circle - jerk metric", "", "Jerk metric (1/s²)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch41.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Coefficient", zone, "Circle", 40, "Circle - CV jerk metric", "", "CV jerk metric(%)"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch42.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Cycle", zone, "Circle", 41, "Circle - speed metric", "", "Speed metric"); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch5.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique("Coefficient", zone, "Circle", 42, "Circle - CV speed metric", "", "CV speed metric (%)"); zoneLibre++; zone = "Chart" + zoneLibre; } }

            verifVisibilityBtn();
        }

        private void ch_Checked(object sender, RoutedEventArgs e)
        {
            UiServices.SetBusyState();
            verifCheck();
        }

        //Méthode qui check tous les comboBox correspondant à l'ex FreeAmplitude
        private void chFree_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox Ch1 = this.FindName("ch1") as CheckBox;
            CheckBox Ch2 = this.FindName("ch2") as CheckBox;
            CheckBox Ch6 = this.FindName("ch6") as CheckBox;
            CheckBox Ch7 = this.FindName("ch7") as CheckBox;
            CheckBox Ch8 = this.FindName("ch8") as CheckBox;
            CheckBox Ch9 = this.FindName("ch9") as CheckBox;
            CheckBox Ch10 = this.FindName("ch10") as CheckBox;
            CheckBox Ch11 = this.FindName("ch11") as CheckBox;
            CheckBox Ch12 = this.FindName("ch12") as CheckBox;
            CheckBox Ch13 = this.FindName("ch13") as CheckBox;
            CheckBox Ch14 = this.FindName("ch14") as CheckBox;
            CheckBox Ch15 = this.FindName("ch15") as CheckBox;
            
            Ch1.IsChecked = true;
            Ch2.IsChecked = true;
            Ch6.IsChecked = true;
            Ch7.IsChecked = true;
            Ch8.IsChecked = true;
            Ch9.IsChecked = true;
            Ch10.IsChecked = true;
            Ch11.IsChecked = true;
            Ch12.IsChecked = true;
            Ch13.IsChecked = true;
            Ch14.IsChecked = true;
            Ch15.IsChecked = true;
        }

        //Méthode qui uncheck tous les comboBox correspondant à l'ex FreeAmplitude
        private void chFree_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox Ch1 = this.FindName("ch1") as CheckBox;
            CheckBox Ch2 = this.FindName("ch2") as CheckBox;
            CheckBox Ch6 = this.FindName("ch6") as CheckBox;
            CheckBox Ch7 = this.FindName("ch7") as CheckBox;
            CheckBox Ch8 = this.FindName("ch8") as CheckBox;
            CheckBox Ch9 = this.FindName("ch9") as CheckBox;
            CheckBox Ch10 = this.FindName("ch10") as CheckBox;
            CheckBox Ch11 = this.FindName("ch11") as CheckBox;
            CheckBox Ch12 = this.FindName("ch12") as CheckBox;
            CheckBox Ch13 = this.FindName("ch13") as CheckBox;
            CheckBox Ch14 = this.FindName("ch14") as CheckBox;
            CheckBox Ch15 = this.FindName("ch15") as CheckBox;

            Ch1.IsChecked = false;
            Ch2.IsChecked = false;
            Ch6.IsChecked = false;
            Ch7.IsChecked = false;
            Ch8.IsChecked = false;
            Ch9.IsChecked = false;
            Ch10.IsChecked = false;
            Ch11.IsChecked = false;
            Ch12.IsChecked = false;
            Ch13.IsChecked = false;
            Ch14.IsChecked = false;
            Ch15.IsChecked = false;
        }

        //Méthode qui check tous les comboBox correspondant à l'ex Cible
        private void chTarget_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox Ch3 = this.FindName("ch3") as CheckBox;
            CheckBox Ch16 = this.FindName("ch16") as CheckBox;
            CheckBox Ch17 = this.FindName("ch17") as CheckBox;
            CheckBox Ch18 = this.FindName("ch18") as CheckBox;
            CheckBox Ch19 = this.FindName("ch19") as CheckBox;
            CheckBox Ch20 = this.FindName("ch20") as CheckBox;
            CheckBox Ch21 = this.FindName("ch21") as CheckBox;
            CheckBox Ch22 = this.FindName("ch22") as CheckBox;
            CheckBox Ch23 = this.FindName("ch23") as CheckBox;
            CheckBox Ch24 = this.FindName("ch24") as CheckBox;
            CheckBox Ch25 = this.FindName("ch25") as CheckBox;
            CheckBox Ch26 = this.FindName("ch26") as CheckBox;

            Ch3.IsChecked = true;
            Ch16.IsChecked = true;
            Ch17.IsChecked = true;
            Ch18.IsChecked = true;
            Ch19.IsChecked = true;
            Ch20.IsChecked = true;
            Ch21.IsChecked = true;
            Ch22.IsChecked = true;
            Ch23.IsChecked = true;
            Ch24.IsChecked = true;
            Ch25.IsChecked = true;
            Ch26.IsChecked = true;
        }

        //Méthode qui uncheck tous les comboBox correspondant à l'ex Cible
        private void chTarget_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox Ch3 = this.FindName("ch3") as CheckBox;
            CheckBox Ch16 = this.FindName("ch16") as CheckBox;
            CheckBox Ch17 = this.FindName("ch17") as CheckBox;
            CheckBox Ch18 = this.FindName("ch18") as CheckBox;
            CheckBox Ch19 = this.FindName("ch19") as CheckBox;
            CheckBox Ch20 = this.FindName("ch20") as CheckBox;
            CheckBox Ch21 = this.FindName("ch21") as CheckBox;
            CheckBox Ch22 = this.FindName("ch22") as CheckBox;
            CheckBox Ch23 = this.FindName("ch23") as CheckBox;
            CheckBox Ch24 = this.FindName("ch24") as CheckBox;
            CheckBox Ch25 = this.FindName("ch25") as CheckBox;
            CheckBox Ch26 = this.FindName("ch26") as CheckBox;

            Ch3.IsChecked = false;
            Ch16.IsChecked = false;
            Ch17.IsChecked = false;
            Ch18.IsChecked = false;
            Ch19.IsChecked = false;
            Ch20.IsChecked = false;
            Ch21.IsChecked = false;
            Ch22.IsChecked = false;
            Ch23.IsChecked = false;
            Ch24.IsChecked = false;
            Ch25.IsChecked = false;
            Ch26.IsChecked = false;
        }

        //Méthode qui check tous les comboBox correspondant à l'ex Carre
        private void chSquare_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox Ch4 = this.FindName("ch4") as CheckBox;
            CheckBox Ch27 = this.FindName("ch27") as CheckBox;
            CheckBox Ch28 = this.FindName("ch28") as CheckBox;
            CheckBox Ch29 = this.FindName("ch29") as CheckBox;
            CheckBox Ch30 = this.FindName("ch30") as CheckBox;
            CheckBox Ch31 = this.FindName("ch31") as CheckBox;
            CheckBox Ch32 = this.FindName("ch32") as CheckBox;
            CheckBox Ch33 = this.FindName("ch33") as CheckBox;
            CheckBox Ch34 = this.FindName("ch34") as CheckBox;
            CheckBox Ch35 = this.FindName("ch35") as CheckBox;

            Ch4.IsChecked = true;
            Ch27.IsChecked = true;
            Ch28.IsChecked = true;
            Ch29.IsChecked = true;
            Ch30.IsChecked = true;
            Ch31.IsChecked = true;
            Ch32.IsChecked = true;
            Ch33.IsChecked = true;
            Ch34.IsChecked = true;
            Ch35.IsChecked = true;
        }

        //Méthode qui uncheck tous les comboBox correspondant à l'ex Carre
        private void chSquare_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox Ch4 = this.FindName("ch4") as CheckBox;
            CheckBox Ch27 = this.FindName("ch27") as CheckBox;
            CheckBox Ch28 = this.FindName("ch28") as CheckBox;
            CheckBox Ch29 = this.FindName("ch29") as CheckBox;
            CheckBox Ch30 = this.FindName("ch30") as CheckBox;
            CheckBox Ch31 = this.FindName("ch31") as CheckBox;
            CheckBox Ch32 = this.FindName("ch32") as CheckBox;
            CheckBox Ch33 = this.FindName("ch33") as CheckBox;
            CheckBox Ch34 = this.FindName("ch34") as CheckBox;
            CheckBox Ch35 = this.FindName("ch35") as CheckBox;

            Ch4.IsChecked = false;
            Ch27.IsChecked = false;
            Ch28.IsChecked = false;
            Ch29.IsChecked = false;
            Ch30.IsChecked = false;
            Ch31.IsChecked = false;
            Ch32.IsChecked = false;
            Ch33.IsChecked = false;
            Ch34.IsChecked = false;
            Ch35.IsChecked = false;
        }

        //Méthode qui check tous les comboBox correspondant à l'ex Cercle
        private void chCircle_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox Ch5 = this.FindName("ch5") as CheckBox;
            CheckBox Ch36 = this.FindName("ch36") as CheckBox;
            CheckBox Ch37 = this.FindName("ch37") as CheckBox;
            CheckBox Ch38 = this.FindName("ch38") as CheckBox;
            CheckBox Ch39 = this.FindName("ch39") as CheckBox;
            CheckBox Ch40 = this.FindName("ch40") as CheckBox;
            CheckBox Ch41 = this.FindName("ch41") as CheckBox;
            CheckBox Ch42 = this.FindName("ch42") as CheckBox;
            CheckBox Ch43 = this.FindName("ch43") as CheckBox;
            CheckBox Ch44 = this.FindName("ch44") as CheckBox;

            Ch5.IsChecked = true;
            Ch36.IsChecked = true;
            Ch37.IsChecked = true;
            Ch38.IsChecked = true;
            Ch39.IsChecked = true;
            Ch40.IsChecked = true;
            Ch41.IsChecked = true;
            Ch42.IsChecked = true;
            Ch43.IsChecked = true;
            Ch44.IsChecked = true;
        }

        //Méthode qui uncheck tous les comboBox correspondant à l'ex Cercle
        private void chCircle_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox Ch5 = this.FindName("ch5") as CheckBox;
            CheckBox Ch36 = this.FindName("ch36") as CheckBox;
            CheckBox Ch37 = this.FindName("ch37") as CheckBox;
            CheckBox Ch38 = this.FindName("ch38") as CheckBox;
            CheckBox Ch39 = this.FindName("ch39") as CheckBox;
            CheckBox Ch40 = this.FindName("ch40") as CheckBox;
            CheckBox Ch41 = this.FindName("ch41") as CheckBox;
            CheckBox Ch42 = this.FindName("ch42") as CheckBox;
            CheckBox Ch43 = this.FindName("ch43") as CheckBox;
            CheckBox Ch44 = this.FindName("ch44") as CheckBox;

            Ch5.IsChecked = false;
            Ch36.IsChecked = false;
            Ch37.IsChecked = false;
            Ch38.IsChecked = false;
            Ch39.IsChecked = false;
            Ch40.IsChecked = false;
            Ch41.IsChecked = false;
            Ch42.IsChecked = false;
            Ch43.IsChecked = false;
            Ch44.IsChecked = false;
        }

        //Méthode appellé quand on clique sur le boutton Next
        private void btNext_Click(object sender, RoutedEventArgs e)
        {
            int posi = int.Parse((string)NumPage.Content);
            posi++;
            NumPage.Content = posi.ToString();
            btPrev.IsEnabled = true;
            UiServices.SetBusyState();
            verifCheck();
        }

        //Méthode appellé quand on clique sur le boutton Previous
        private void btPrev_Click(object sender, RoutedEventArgs e)
        {
            int posi = int.Parse((string)NumPage.Content);
            posi--;
            NumPage.Content = posi.ToString();
            btNext.IsEnabled = true;
            UiServices.SetBusyState();
            verifCheck();
        }

        //Méthode appellé quand on clique sur le boutton Print
        private void btPrint_Click(object sender, RoutedEventArgs e)
        {

            Chart1.Printing.PrintDocument = new PrintDocument();
            Chart1.Printing.PrintDocument.DefaultPageSettings.Landscape = false;
            Chart1.Printing.PrintDocument.PrintPage += new PrintPageEventHandler(PrintPage);
            Chart1.Printing.PrintPreview();
            Chart1.Printing.Print(true);
        }

        private void CreatePdf()
        {
            Document myDocument = new Document(PageSize.A4, 10, 10, 50, 50);
            try
            {
                savepage = int.Parse((string)NumPage.Content);
                NumPage.Content = "1";
                verifCheck();
                normaliserPDF();

                double nbPagePrint = Math.Ceiling((double)(countcheck + 1) / 3);
                PdfWriter.GetInstance(myDocument, new FileStream("../../test.pdf", FileMode.Create));
                myDocument.Open();
                System.IO.MemoryStream stream;
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance("../../Resources/Image/Background/Axi_FondEcran_T.png");
                PdfPTable tableau = new PdfPTable(2);
                tableau.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                tableau.AddCell(logo);

                tableau.AddCell("Nom : " + singleUser.Patient.Nom +
                                     "\nPrenom : " + singleUser.Patient.Prenom +
                                     "\nDate de naissance : " + singleUser.Patient.DateDeNaissance);

                myDocument.Add(tableau);
                //myDocument.Add(new iTextSharp.text.Paragraph("\n"));
                normaliserPDF();
                float[] largeurs = { 5, 90, 5 };
                tableau = new PdfPTable(largeurs);
                tableau.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                iTextSharp.text.Image gif;

                ImageToPdf(myDocument, out stream, out gif, 1);
                tableau.AddCell(new iTextSharp.text.Paragraph(""));
                tableau.AddCell(gif);
                tableau.AddCell(new iTextSharp.text.Paragraph(""));

                tableau.AddCell(new iTextSharp.text.Paragraph("\n\n"));
                tableau.AddCell(new iTextSharp.text.Paragraph("\n\n"));
                tableau.AddCell(new iTextSharp.text.Paragraph("\n\n"));


                ImageToPdf(myDocument, out stream, out gif, 2);
                tableau.AddCell(new iTextSharp.text.Paragraph(""));
                tableau.AddCell(gif);
                tableau.AddCell(new iTextSharp.text.Paragraph(""));

                myDocument.Add(tableau);
                myDocument.Add(new iTextSharp.text.Paragraph("\n\n\n\n"));
                tableau = new PdfPTable(2);
                tableau.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                PdfPCell cellule = new PdfPCell(new iTextSharp.text.Paragraph(singleUser.Patient.Nom + singleUser.Patient.Prenom + singleUser.Patient.DateDeNaissance.Substring(0, 2) + singleUser.Patient.DateDeNaissance.Substring(3, 2) + singleUser.Patient.DateDeNaissance.Substring(6, 4)));
                cellule.HorizontalAlignment = 0;
                cellule.Border = iTextSharp.text.Rectangle.NO_BORDER;
                tableau.AddCell(cellule);

                cellule = new PdfPCell(new iTextSharp.text.Paragraph("1/" + nbPagePrint));
                cellule.HorizontalAlignment = 2;
                cellule.Border = iTextSharp.text.Rectangle.NO_BORDER;
                tableau.AddCell(cellule);
                myDocument.Add(tableau);

                int pagepdf = 1;
                while (pagepdf != nbPagePrint)
                {
                    tableau = new PdfPTable(largeurs);
                    tableau.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    if (compt == 6)
                    {
                        multi++;
                        compt = 1;
                    }
                    int pagePrint = pagepdf - (5 * multi);
                    if (pagePrint == 6)
                    { pagePrint = 1; }
                    switch (pagePrint)
                    {
                        case 1:
                            ImageToPdf(myDocument, out stream, out gif, 3);
                            tableau.AddCell(new iTextSharp.text.Paragraph(""));
                            tableau.AddCell(gif);
                            tableau.AddCell(new iTextSharp.text.Paragraph(""));

                            ImageToPdf(myDocument, out stream, out gif, 4);
                            tableau.AddCell(new iTextSharp.text.Paragraph(""));
                            tableau.AddCell(gif);
                            tableau.AddCell(new iTextSharp.text.Paragraph(""));

                            ImageToPdf(myDocument, out stream, out gif, 5);
                            tableau.AddCell(new iTextSharp.text.Paragraph(""));
                            tableau.AddCell(gif);
                            tableau.AddCell(new iTextSharp.text.Paragraph(""));
                            pageSuivante();
                            normaliserPDF();
                            break;
                        case 2:
                            ImageToPdf(myDocument, out stream, out gif, 1);
                            tableau.AddCell(new iTextSharp.text.Paragraph(""));
                            tableau.AddCell(gif);
                            tableau.AddCell(new iTextSharp.text.Paragraph(""));

                            ImageToPdf(myDocument, out stream, out gif, 2);
                            tableau.AddCell(new iTextSharp.text.Paragraph(""));
                            tableau.AddCell(gif);
                            tableau.AddCell(new iTextSharp.text.Paragraph(""));

                            ImageToPdf(myDocument, out stream, out gif, 3);
                            tableau.AddCell(new iTextSharp.text.Paragraph(""));
                            tableau.AddCell(gif);
                            tableau.AddCell(new iTextSharp.text.Paragraph(""));
                            break;
                        case 3:
                            ImageToPdf(myDocument, out stream, out gif, 4);
                            tableau.AddCell(new iTextSharp.text.Paragraph(""));
                            tableau.AddCell(gif);
                            tableau.AddCell(new iTextSharp.text.Paragraph(""));

                            ImageToPdf(myDocument, out stream, out gif, 5);
                            tableau.AddCell(new iTextSharp.text.Paragraph(""));
                            tableau.AddCell(gif);
                            tableau.AddCell(new iTextSharp.text.Paragraph(""));
                            int posi = int.Parse((string)NumPage.Content);
                            int test = int.Parse((string)NbPage.Content);
                            if (posi != test)
                            {
                                pageSuivante();
                                normaliserPDF();

                                ImageToPdf(myDocument, out stream, out gif, 1);
                                tableau.AddCell(new iTextSharp.text.Paragraph(""));
                                tableau.AddCell(gif);
                                tableau.AddCell(new iTextSharp.text.Paragraph(""));
                            }
                            else
                            {
                                tableau.AddCell(new iTextSharp.text.Paragraph("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n"));
                                tableau.AddCell(new iTextSharp.text.Paragraph("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n"));
                                tableau.AddCell(new iTextSharp.text.Paragraph("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n"));
                            }
                            break;
                        case 4:
                            ImageToPdf(myDocument, out stream, out gif, 2);
                            tableau.AddCell(new iTextSharp.text.Paragraph(""));
                            tableau.AddCell(gif);
                            tableau.AddCell(new iTextSharp.text.Paragraph(""));

                            ImageToPdf(myDocument, out stream, out gif, 3);
                            tableau.AddCell(new iTextSharp.text.Paragraph(""));
                            tableau.AddCell(gif);
                            tableau.AddCell(new iTextSharp.text.Paragraph(""));

                            ImageToPdf(myDocument, out stream, out gif, 4);
                            tableau.AddCell(new iTextSharp.text.Paragraph(""));
                            tableau.AddCell(gif);
                            tableau.AddCell(new iTextSharp.text.Paragraph(""));
                            break;
                        case 5:
                            ImageToPdf(myDocument, out stream, out gif, 5);
                            tableau.AddCell(new iTextSharp.text.Paragraph(""));
                            tableau.AddCell(gif);
                            tableau.AddCell(new iTextSharp.text.Paragraph(""));
                            posi = int.Parse((string)NumPage.Content);
                            test = int.Parse((string)NbPage.Content);
                            if (posi != test)
                            {
                                pageSuivante();
                                normaliserPDF();

                                ImageToPdf(myDocument, out stream, out gif, 1);
                                tableau.AddCell(new iTextSharp.text.Paragraph(""));
                                tableau.AddCell(gif);
                                tableau.AddCell(new iTextSharp.text.Paragraph(""));

                                ImageToPdf(myDocument, out stream, out gif, 2);
                                tableau.AddCell(new iTextSharp.text.Paragraph(""));
                                tableau.AddCell(gif);
                                tableau.AddCell(new iTextSharp.text.Paragraph(""));
                            }
                            else
                            {
                                tableau.AddCell(new iTextSharp.text.Paragraph("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n"));
                                tableau.AddCell(new iTextSharp.text.Paragraph("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n"));
                                tableau.AddCell(new iTextSharp.text.Paragraph("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n"));
                            }
                            break;

                    }
                    myDocument.Add(tableau);

                    tableau = new PdfPTable(2);
                    tableau.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    cellule = new PdfPCell(new iTextSharp.text.Paragraph(singleUser.Patient.Nom + singleUser.Patient.Prenom + singleUser.Patient.DateDeNaissance.Substring(0, 2) + singleUser.Patient.DateDeNaissance.Substring(3, 2) + singleUser.Patient.DateDeNaissance.Substring(6, 4)));
                    cellule.HorizontalAlignment = 0;
                    cellule.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    tableau.AddCell(cellule);

                    cellule = new PdfPCell(new iTextSharp.text.Paragraph((pagepdf + 1) + "/" + nbPagePrint));
                    cellule.HorizontalAlignment = 2;
                    cellule.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    tableau.AddCell(cellule);
                    myDocument.Add(tableau);
                    pagepdf++;
                    compt++;
                }
                printingPageIndex = 0;
                compt = 0;
                multi = 0;
                NumPage.Content = savepage.ToString();
                verifCheck();


            }
            catch (Exception ex)
            {
                GestionErreur.GerrerErreur(ex);
            }

            myDocument.Close();
        }

        private void ImageToPdf(Document myDocument, out System.IO.MemoryStream stream, out iTextSharp.text.Image gif, int chart)
        {
            stream = new System.IO.MemoryStream();


            switch (chart)
            {
                case 1:
                    Chart1.SaveImage(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    break;
                case 2:
                    Chart2.SaveImage(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    break;
                case 3:
                    Chart3.SaveImage(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    break;
                case 4:
                    Chart4.SaveImage(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    break;
                case 5:
                    Chart5.SaveImage(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    break;
            }


            Chart1.SaveImage(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            gif = iTextSharp.text.Image.GetInstance(stream.ToArray());
            gif.Alignment = Element.ALIGN_CENTER;
            //gif.ScaleAbsolute(10, 10);

        }
        private int printingPageIndex = 0;
        private int savepage = 0;
        private int compt = 0;
        private int multi = 0;
        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            normaliser();
            ev.HasMorePages = true;

            double nbPagePrint = Math.Ceiling((double)(countcheck + 1) / 3);

            // Calculate first chart position rectangle
            System.Drawing.Rectangle chartPosition = new System.Drawing.Rectangle(50, 50, Chart1.Size.Width, Chart1.Size.Height);
            if (printingPageIndex == 0)
            {
                savepage = int.Parse((string)NumPage.Content);
                NumPage.Content = "1";
                verifCheck();
                normaliser();
                // Align first chart position on the page
                chartPosition.Width = 350;
                chartPosition.Height = 180;

                System.Drawing.Image logo = System.Drawing.Image.FromFile("../../Resources/Image/Background/Axi_FondEcran_T.png");
                ev.Graphics.DrawImage(logo, chartPosition);
                chartPosition.Height = 350;
                chartPosition.X += chartPosition.Width + 10;
                string DonneePatient = "Nom : " + singleUser.Patient.Nom +
                                     "\nPrenom : " + singleUser.Patient.Prenom +
                                     "\nDate de naissance : " + singleUser.Patient.DateDeNaissance;
                System.Drawing.Font font = new System.Drawing.Font("Arial Rounded MT", 15);
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Near;
                ev.Graphics.DrawString(DonneePatient, font, System.Drawing.Brushes.Black, chartPosition, format);
                chartPosition.X -= chartPosition.Width + 10;
                chartPosition.Y += chartPosition.Height;


                chartPosition.Width = 700;
                chartPosition.Height = 350;

                Chart1.Printing.PrintPaint(ev.Graphics, chartPosition);

                chartPosition.Y += chartPosition.Height + 10;
                Chart2.Printing.PrintPaint(ev.Graphics, chartPosition);

                chartPosition.Y += chartPosition.Height + 10;
                string dossier = singleUser.Patient.Nom + singleUser.Patient.Prenom + singleUser.Patient.DateDeNaissance.Substring(0, 2) + singleUser.Patient.DateDeNaissance.Substring(3, 2) + singleUser.Patient.DateDeNaissance.Substring(6, 4);
                System.Drawing.Font font2 = new System.Drawing.Font("Arial Rounded MT", 8);
                StringFormat format2 = new StringFormat();
                format2.Alignment = StringAlignment.Near;
                ev.Graphics.DrawString(dossier, font2, System.Drawing.Brushes.Black, chartPosition, format2);
                chartPosition.X += 650;
                string page = (printingPageIndex + 1) + " / " + nbPagePrint;
                ev.Graphics.DrawString(page, font2, System.Drawing.Brushes.Black, chartPosition, format2);


            }
            else
            {
                if (compt == 6)
                {
                    multi++;
                    compt = 1;
                }
                int pagePrint = printingPageIndex - (5 * multi);
                switch (pagePrint)
                {
                    case 1:
                        addpage(ev, "Chart3", "Chart4", "Chart5", nbPagePrint);
                        pageSuivante();
                        normaliser();
                        break;
                    case 2:
                        addpage(ev, "Chart1", "Chart2", "Chart3", nbPagePrint);
                        break;
                    case 3:
                        addpage2(ev, "Chart4", "Chart5", "Chart1", nbPagePrint);
                        break;
                    case 4:
                        addpage(ev, "Chart2", "Chart3", "Chart4", nbPagePrint);
                        break;
                    case 5:
                        addpage3(ev, "Chart5", "Chart1", "Chart2", nbPagePrint);
                        break;
                }
            }



            compt++;
            printingPageIndex++;
            if (printingPageIndex == nbPagePrint || countcheck <= 2)
            {
                ev.HasMorePages = false;
                printingPageIndex = 0;
                compt = 0;
                multi = 0;
                NumPage.Content = savepage.ToString();
                verifCheck();
            }


        }

        private void denormaliser()
        {
            Chart1.BackColor = System.Drawing.Color.WhiteSmoke;
            Chart2.BackColor = System.Drawing.Color.WhiteSmoke;
            Chart3.BackColor = System.Drawing.Color.WhiteSmoke;
            Chart4.BackColor = System.Drawing.Color.WhiteSmoke;
            Chart5.BackColor = System.Drawing.Color.WhiteSmoke;
        }

        private void normaliserPDF()
        {
            Chart1.BackColor = System.Drawing.Color.White;
            Chart2.BackColor = System.Drawing.Color.White;
            Chart3.BackColor = System.Drawing.Color.White;
            Chart4.BackColor = System.Drawing.Color.White;
            Chart5.BackColor = System.Drawing.Color.White;
        }

        private void normaliser()
        {
            Chart1.BackColor = System.Drawing.Color.White;
            Chart2.BackColor = System.Drawing.Color.White;
            Chart3.BackColor = System.Drawing.Color.White;
            Chart4.BackColor = System.Drawing.Color.White;
            Chart5.BackColor = System.Drawing.Color.White;
        }

        private void pageSuivante()
        {
            int posi = int.Parse((string)NumPage.Content);
            int test = int.Parse((string)NbPage.Content);
            if (posi != test)
            {
                posi++;
                NumPage.Content = posi.ToString();
                btPrev.IsEnabled = true;
                verifCheck();
            }
        }

        private void addpage(PrintPageEventArgs ev, string cha1, string cha2, string cha3, double nbPagePrint)
        {
            Chart Graph1 = this.FindName(cha1) as Chart;
            Chart Graph2 = this.FindName(cha2) as Chart;
            Chart Graph3 = this.FindName(cha3) as Chart;
            System.Drawing.Rectangle chartPosition = new System.Drawing.Rectangle(50, 50, 700, 350);
            Graph1.Printing.PrintPaint(ev.Graphics, chartPosition);
            chartPosition.Y += chartPosition.Height + 10;
            Graph2.Printing.PrintPaint(ev.Graphics, chartPosition);
            chartPosition.Y += chartPosition.Height + 10;
            Graph3.Printing.PrintPaint(ev.Graphics, chartPosition);

            chartPosition.Y += chartPosition.Height + 10;
            string dossier = singleUser.Patient.Nom + singleUser.Patient.Prenom + singleUser.Patient.DateDeNaissance.Substring(0, 2) + singleUser.Patient.DateDeNaissance.Substring(3, 2) + singleUser.Patient.DateDeNaissance.Substring(6, 4);
            System.Drawing.Font font2 = new System.Drawing.Font("Arial Rounded MT", 8);
            StringFormat format2 = new StringFormat();
            format2.Alignment = StringAlignment.Near;
            ev.Graphics.DrawString(dossier, font2, System.Drawing.Brushes.Black, chartPosition, format2);
            chartPosition.X += 650;
            string page = (printingPageIndex + 1) + " / " + nbPagePrint;
            ev.Graphics.DrawString(page, font2, System.Drawing.Brushes.Black, chartPosition, format2);
        }
        private void addpage2(PrintPageEventArgs ev, string cha1, string cha2, string cha3, double nbPagePrint)
        {
            Chart Graph1 = this.FindName(cha1) as Chart;
            Chart Graph2 = this.FindName(cha2) as Chart;

            System.Drawing.Rectangle chartPosition = new System.Drawing.Rectangle(50, 50, 700, 350);
            Graph1.Printing.PrintPaint(ev.Graphics, chartPosition);
            chartPosition.Y += chartPosition.Height + 10;
            Graph2.Printing.PrintPaint(ev.Graphics, chartPosition);
            chartPosition.Y += chartPosition.Height + 10;
            int posi = int.Parse((string)NumPage.Content);
            int test = int.Parse((string)NbPage.Content);
            if (posi != test)
            {
                pageSuivante();
                normaliser();
                Chart Graph3 = this.FindName(cha3) as Chart;
                Graph3.Printing.PrintPaint(ev.Graphics, chartPosition);
                chartPosition.Y += chartPosition.Height + 10;
            }
            else
            {
                chartPosition.Y += chartPosition.Height + 10;
            }
            string dossier = singleUser.Patient.Nom + singleUser.Patient.Prenom + singleUser.Patient.DateDeNaissance.Substring(0, 2) + singleUser.Patient.DateDeNaissance.Substring(3, 2) + singleUser.Patient.DateDeNaissance.Substring(6, 4);
            System.Drawing.Font font2 = new System.Drawing.Font("Arial Rounded MT", 8);
            StringFormat format2 = new StringFormat();
            format2.Alignment = StringAlignment.Near;
            ev.Graphics.DrawString(dossier, font2, System.Drawing.Brushes.Black, chartPosition, format2);
            chartPosition.X += 650;

            string page = (printingPageIndex + 1) + " / " + nbPagePrint;
            ev.Graphics.DrawString(page, font2, System.Drawing.Brushes.Black, chartPosition, format2);
        }
        private void addpage3(PrintPageEventArgs ev, string cha1, string cha2, string cha3, double nbPagePrint)
        {
            Chart Graph1 = this.FindName(cha1) as Chart;

            System.Drawing.Rectangle chartPosition = new System.Drawing.Rectangle(50, 50, 700, 350);
            Graph1.Printing.PrintPaint(ev.Graphics, chartPosition);
            chartPosition.Y += chartPosition.Height + 10;

            int posi = int.Parse((string)NumPage.Content);
            int test = int.Parse((string)NbPage.Content);
            if (posi != test)
            {
                pageSuivante();
                normaliser();
                Chart Graph2 = this.FindName(cha2) as Chart;
                Chart Graph3 = this.FindName(cha3) as Chart;
                Graph2.Printing.PrintPaint(ev.Graphics, chartPosition);
                chartPosition.Y += chartPosition.Height + 10;
                Graph3.Printing.PrintPaint(ev.Graphics, chartPosition);
                chartPosition.Y += chartPosition.Height + 10;
            }
            else
            {
                chartPosition.Y += chartPosition.Height + 10;
                chartPosition.Y += chartPosition.Height + 10;
            }
            string dossier = singleUser.Patient.Nom + singleUser.Patient.Prenom + singleUser.Patient.DateDeNaissance.Substring(0, 2) + singleUser.Patient.DateDeNaissance.Substring(3, 2) + singleUser.Patient.DateDeNaissance.Substring(6, 4);
            System.Drawing.Font font2 = new System.Drawing.Font("Arial Rounded MT", 8);
            StringFormat format2 = new StringFormat();
            format2.Alignment = StringAlignment.Near;
            ev.Graphics.DrawString(dossier, font2, System.Drawing.Brushes.Black, chartPosition, format2);
            chartPosition.X += 650;
            string page = (printingPageIndex + 1) + " / " + nbPagePrint;
            ev.Graphics.DrawString(page, font2, System.Drawing.Brushes.Black, chartPosition, format2);
        }

        private void btnChart1_Click(object sender, RoutedEventArgs e)
        {
            List<CheckBox> ListCh = new List<CheckBox>{ ch1, ch6,ch9,ch2,ch7,ch8,ch10,ch11,ch12,ch13,ch14,ch15,ch16,ch17,ch20,ch21,ch18,ch19,ch22,ch23,
                                                        ch24,ch25,ch3,ch26,ch34,ch35,ch27,ch28,ch29,ch30,ch31,ch4,ch32,ch33,ch43,ch44,ch36,ch37,ch38,ch39,
                                                        ch40,ch41,ch42,ch5};
            int posi = int.Parse((string)NumPage.Content);
            int page = (posi - 1) * 5;
            int compteur = 0;
            for (int i = 0; i < 44; i++)
            {
                if (ListCh[i].IsChecked == true)
                {
                    compteur++;
                    if (compteur == page + 1)
                    {
                        ListCh[i].IsChecked = false;
                        break;
                    }
                }
            }
            verifVisibilityBtn();
        }

        private void btnChat2_Click(object sender, RoutedEventArgs e)
        {
            List<CheckBox> ListCh = new List<CheckBox>{ ch1, ch6,ch9,ch2,ch7,ch8,ch10,ch11,ch12,ch13,ch14,ch15,ch16,ch17,ch20,ch21,ch18,ch19,ch22,ch23,
                                                        ch24,ch25,ch3,ch26,ch34,ch35,ch27,ch28,ch29,ch30,ch31,ch4,ch32,ch33,ch43,ch44,ch36,ch37,ch38,ch39,
                                                        ch40,ch41,ch42,ch5};
            int posi = int.Parse((string)NumPage.Content);
            int page = (posi - 1) * 5;
            int compteur = 0;
            for (int i = 0; i < 44; i++)
            {
                if (ListCh[i].IsChecked == true)
                {
                    compteur++;
                    if (compteur == page + 2)
                    {
                        ListCh[i].IsChecked = false;
                        break;
                    }
                }
            }
            verifVisibilityBtn();
        }

        private void btnChat3_Click(object sender, RoutedEventArgs e)
        {
            List<CheckBox> ListCh = new List<CheckBox>{ ch1, ch6,ch9,ch2,ch7,ch8,ch10,ch11,ch12,ch13,ch14,ch15,ch16,ch17,ch20,ch21,ch18,ch19,ch22,ch23,
                                                        ch24,ch25,ch3,ch26,ch34,ch35,ch27,ch28,ch29,ch30,ch31,ch4,ch32,ch33,ch43,ch44,ch36,ch37,ch38,ch39,
                                                        ch40,ch41,ch42,ch5};
            int posi = int.Parse((string)NumPage.Content);
            int page = (posi - 1) * 5;
            int compteur = 0;
            for (int i = 0; i < 44; i++)
            {
                if (ListCh[i].IsChecked == true)
                {
                    compteur++;
                    if (compteur == page + 3)
                    {
                        ListCh[i].IsChecked = false;
                        break;
                    }
                }
            }
            verifVisibilityBtn();
        }

        private void btnChart4_Click(object sender, RoutedEventArgs e)
        {
            List<CheckBox> ListCh = new List<CheckBox>{ ch1, ch6,ch9,ch2,ch7,ch8,ch10,ch11,ch12,ch13,ch14,ch15,ch16,ch17,ch20,ch21,ch18,ch19,ch22,ch23,
                                                        ch24,ch25,ch3,ch26,ch34,ch35,ch27,ch28,ch29,ch30,ch31,ch4,ch32,ch33,ch43,ch44,ch36,ch37,ch38,ch39,
                                                        ch40,ch41,ch42,ch5};
            int posi = int.Parse((string)NumPage.Content);
            int page = (posi - 1) * 5;
            int compteur = 0;
            for (int i = 0; i < 44; i++)
            {
                if (ListCh[i].IsChecked == true)
                {
                    compteur++;
                    if (compteur == page + 4)
                    {
                        ListCh[i].IsChecked = false;
                        break;
                    }
                }
            }
            verifVisibilityBtn();
        }

        private void btnChart5_Click(object sender, RoutedEventArgs e)
        {
            List<CheckBox> ListCh = new List<CheckBox>{ ch1, ch6,ch9,ch2,ch7,ch8,ch10,ch11,ch12,ch13,ch14,ch15,ch16,ch17,ch20,ch21,ch18,ch19,ch22,ch23,
                                                        ch24,ch25,ch3,ch26,ch34,ch35,ch27,ch28,ch29,ch30,ch31,ch4,ch32,ch33,ch43,ch44,ch36,ch37,ch38,ch39,
                                                        ch40,ch41,ch42,ch5};
            int posi = int.Parse((string)NumPage.Content);
            int page = (posi - 1) * 5;
            int compteur = 0;
            for (int i = 0; i < 44; i++)
            {
                if (ListCh[i].IsChecked == true)
                {
                    compteur++;
                    if (compteur == page + 5)
                    {
                        ListCh[i].IsChecked = false;
                        break;
                    }
                }
            }
            verifVisibilityBtn();

        }

        private void verifVisibilityBtn()
        {
            int posi = int.Parse((string)NumPage.Content);
            int test = int.Parse((string)NbPage.Content);
            int verif = countcheck - ((posi - 1) * 5);
            if (posi == test)
            {
                if (verif < 1)
                { btnChart1.Visibility = Visibility.Hidden; }
                else
                { btnChart1.Visibility = Visibility.Visible; }
                if (verif < 2)
                { btnChart2.Visibility = Visibility.Hidden; }
                else
                { btnChart2.Visibility = Visibility.Visible; }
                if (verif < 3)
                { btnChart3.Visibility = Visibility.Hidden; }
                else
                { btnChart3.Visibility = Visibility.Visible; }
                if (verif < 4)
                { btnChart4.Visibility = Visibility.Hidden; }
                else
                { btnChart4.Visibility = Visibility.Visible; }
                if (verif < 5)
                { btnChart5.Visibility = Visibility.Hidden; }
                else
                { btnChart5.Visibility = Visibility.Visible; }
            }
        }

        //Méthode appellé quand on clique sur le boutton Pdf
        private void BtPDF_Click(object sender, RoutedEventArgs e)
        {
            CreatePdf();
            MessageBox.Show(AxLanguage.Languages.REAplan_PDF_Confirmation, AxLanguage.Languages.REAplan_Confirmation);
        }

    }
}
