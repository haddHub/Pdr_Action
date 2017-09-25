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
    /// Logique d'interaction pour EvolutionReeducation.xaml
    /// </summary>
    public partial class EvolutionReeducation : UserControl
    {
        //Fields
        private Singleton singleUser = Singleton.getInstance();
        private int zoneLibre = 1;
        private int countcheck;

        //Constructeurs
        public EvolutionReeducation()
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
        private void Graphique(string zone, string titre, string x, string y, int param)
        {
            RemoveGraph(zone);
            List<PointEvoEval> val = new List<PointEvoEval>();
            val = EvolReeducationData.ReturnPoint(param, "MvtsComplexes");

            Chart chart = this.FindName(zone) as Chart;
            chart.BackColor = System.Drawing.Color.WhiteSmoke;

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
            area.AxisX.IsMarginVisible = true; ;

            chart.ChartAreas.Add(area);
            if (val.Count != 0)
            {
                Series s1 = new Series();
                s1.IsXValueIndexed = true;
                s1.ChartType = SeriesChartType.ErrorBar;
                s1.XValueType = ChartValueType.DateTime;
                s1.MarkerStyle = MarkerStyle.None;
                s1["PointWidth"] = "0.04";
                s1["ErrorBarCenterMarkerStyle"] = "Circle";
                s1.MarkerSize = 8;

                foreach (var v in val)
                {
                    DataPoint p = new DataPoint();
                    if (y == "CV")
                        p.SetValueXY(v.Date, v.CV);
                    else
                        p.SetValueXY(v.Date, 0);
                    s1.Points.Add(p);
                }

                s1.Name = "Patient";
                chart.Series.Add(s1);
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

            Label NumPage = this.FindName("NumPage") as Label;

            if (zoneLibre <= 6)
            {
                RemoveGraph("Chart1");
                RemoveGraph("Chart2");
                RemoveGraph("Chart3");
                RemoveGraph("Chart4");
                RemoveGraph("Chart5");
            }
            List<CheckBox> ListCh = new List<CheckBox>{ Ch1, Ch2,Ch3,Ch4,Ch5,Ch6,Ch7,Ch8,Ch9 };
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

            if (Ch5.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique(zone, "Nombre de mouvements", " ", "Nombre de mouvements", 61); zoneLibre++; zone = "Chart" + zoneLibre; } }

            if (Ch1.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique(zone, "Vitesse", " ", "Vitesse", 56); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch6.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique(zone, "Vitesse CV", " ", "CV", 56); zoneLibre++; zone = "Chart" + zoneLibre; } }

            if (Ch3.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique(zone, "Assistance latérale", " ", "Assistance latérale", 58); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch7.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique(zone, "Assistance latérale CV", " ", "CV", 58); zoneLibre++; zone = "Chart" + zoneLibre; } }

            if (Ch4.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique(zone, "Assistance longitudinale", " ", "Assistance longitudinale", 59); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch8.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique(zone, "Assistance longitudinale", " ", "CV", 59); zoneLibre++; zone = "Chart" + zoneLibre; } }

            if (Ch2.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique(zone, "Initial", " ", "Initial", 57); zoneLibre++; zone = "Chart" + zoneLibre; } }
            if (Ch9.IsChecked == true && zoneLibre < 6)
            { if (compteur < page) { compteur++; } else { Graphique(zone, "Initial CV", " ", "CV", 57); zoneLibre++; zone = "Chart" + zoneLibre; } }
            
           
            verifVisibilityBtn();
        }

        private void ch_Checked(object sender, RoutedEventArgs e)
        {
            UiServices.SetBusyState();
            verifCheck();
        }

        //Méthode qui check tous les comboBox correspondant à l'ex Mouvements complexes
        private void chMvtsComplexes_Checked(object sender, RoutedEventArgs e)
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

            Ch1.IsChecked = true;
            Ch2.IsChecked = true;
            Ch3.IsChecked = true;
            Ch4.IsChecked = true;
            Ch5.IsChecked = true;
            Ch6.IsChecked = true;
            Ch7.IsChecked = true;
            Ch8.IsChecked = true;
            Ch9.IsChecked = true;
        }

        //Méthode qui uncheck tous les comboBox correspondant à l'ex Mouvements complexes
        private void chMvtsComplexes_Unchecked(object sender, RoutedEventArgs e)
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

            Ch1.IsChecked = false;
            Ch2.IsChecked = false;
            Ch3.IsChecked = false;
            Ch4.IsChecked = false;
            Ch5.IsChecked = false;
            Ch6.IsChecked = false;
            Ch7.IsChecked = false;
            Ch8.IsChecked = false;
            Ch9.IsChecked = false;
        }

        //Méthode qui check tous les comboBox correspondant à l'ex Mouvements rythmiques
        private void chMvtsRythmiques_Checked(object sender, RoutedEventArgs e)
        {
}

        //Méthode qui uncheck tous les comboBox correspondant à l'ex Mouvements rythmiques
        private void chMvtsRythmiques_Unchecked(object sender, RoutedEventArgs e)
        {
            
        }

        //Méthode qui check tous les comboBox correspondant à l'ex Mouvements simples
        private void chMvtsSimples_Checked(object sender, RoutedEventArgs e)
        {
           
        }

        //Méthode qui uncheck tous les comboBox correspondant à l'ex Mouvements simples
        private void chMvtsSimples_Unchecked(object sender, RoutedEventArgs e)
        {
            
        }

        //Méthode qui check tous les comboBox correspondant à l'ex Mouvements cognitifs
        private void chMvtsCogni_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        //Méthode qui uncheck tous les comboBox correspondant à l'ex Mouvements cognitifs
        private void chMvtsCogni_Unchecked(object sender, RoutedEventArgs e)
        {
           
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
            List<CheckBox> ListCh = new List<CheckBox> { ch1, ch2, ch3, ch4, ch5 };
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

        private void btnChart2_Click(object sender, RoutedEventArgs e)
        {
            List<CheckBox> ListCh = new List<CheckBox> { ch1, ch2, ch3, ch4, ch5 };
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

        private void btnChart3_Click(object sender, RoutedEventArgs e)
        {
            List<CheckBox> ListCh = new List<CheckBox> { ch1, ch2, ch3, ch4, ch5 };
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
            List<CheckBox> ListCh = new List<CheckBox> { ch1, ch2, ch3, ch4, ch5 };
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
            List<CheckBox> ListCh = new List<CheckBox> { ch1, ch2, ch3, ch4, ch5 };
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
