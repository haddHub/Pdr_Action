using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OxyPlot;

namespace AxModel
{
    public interface IChartService
    {
        PlotModel CreateChart(string title, string subtitle, IEnumerable<Object> data);

        void RemoveChart(string title, string subtitle, IEnumerable<PlotModel> list);
    }
}
