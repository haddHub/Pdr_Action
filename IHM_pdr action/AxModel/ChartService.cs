using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OxyPlot;
using OxyPlot.Series;
using System.Collections.ObjectModel;
using OxyPlot.Axes;
using System.Globalization;

namespace AxModel
{
    public class ChartService : IChartService
    {
       

        public PlotModel CreateChart(string title, string subtitle, IEnumerable<Object> data)
        {
            var newChart = new PlotModel { Title = title, Subtitle = subtitle,};

            if (data is IEnumerable<PointEvoEval>)
            {
                double dateMax = DateTimeAxis.ToDouble(DateTime.Now.AddDays(1));
                double dateMin = DateTimeAxis.ToDouble(DateTime.Now.AddMonths(-1));
                newChart.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Title = "Date", TickStyle = TickStyle.Crossing, Minimum = dateMin, Maximum = dateMax, TimeZone = TimeZoneInfo.Local });
                newChart.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = subtitle });
                var lineSerie = new LineSeries() { MarkerType = MarkerType.Circle, MarkerSize = 4, MarkerFill = OxyColors.Blue, LineStyle = LineStyle.None };
                var area = new AreaSeries();
                var moyenneSerie = new LineSeries() { Color = OxyColors.Green, StrokeThickness = 4 };

                newChart.Series.Add(area);
                newChart.Series.Add(moyenneSerie);
                newChart.Series.Add(lineSerie);

                if (data.Count() > 0)
                {
                    //ajoute de la norme si besoin
                    if ((data as IEnumerable<PointEvoEval>).First().TabNorme != null)
                    {
                        var norme = (data as IEnumerable<PointEvoEval>).First().TabNorme;
                        if (norme[1] > 0)
                        {
                            
                            var moyenne = newChart.Series[1] as LineSeries;
                            var areaSeries = newChart.Series[0] as AreaSeries;

                            moyenne.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddYears(-10)), norme[0]));
                            moyenne.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddYears(10)), norme[0]));

                            areaSeries.Fill = OxyColors.LightBlue;
                            areaSeries.Color = OxyColors.Green;
                            areaSeries.Color2 = OxyColors.Green;

                            areaSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddYears(-10)), norme[0] + norme[1]));
                            areaSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddYears(+10)), norme[0] + norme[1]));
                            areaSeries.Points2.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddYears(-10)), norme[0] - norme[1]));
                            areaSeries.Points2.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddYears(+10)), norme[0] - norme[1])); 
                        }
                    }

                    foreach (var item in data as IEnumerable<PointEvoEval>)
                    {
                        var lineSeries = newChart.Series[2] as LineSeries;

                        double valeur = 0.0;

                        
                        if (item.Param.ToString().Contains("CV"))
                        {
                            valeur = item.CV;
                        }
                        else
                        {
                            valeur = item.Resultat;
                        }
                        lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(item.Date), valeur));
                    }
                }
            }

            return newChart;
        }

        public void RemoveChart(string title, string subtitle, IEnumerable<PlotModel> listCharts)
        {
            PlotModel chartToDelete = (from c in listCharts
                                       where (c.Title == title) && (c.Subtitle == subtitle)
                                       select c).FirstOrDefault();
            if (chartToDelete != null)
            {
                (listCharts as ObservableCollection<PlotModel>).Remove(chartToDelete);
            }
        }
    }
}
