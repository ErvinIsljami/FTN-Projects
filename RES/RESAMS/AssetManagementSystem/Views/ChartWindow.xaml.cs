using AssetManagementSystem.DB;
using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AssetManagementSystem.Views
{
    /// <summary>
    /// Interaction logic for ChartWindow.xaml
    /// </summary>
    public partial class ChartWindow : Window
    {
        public ChartWindow(int deviceId, long from, long to)
        {
            InitializeComponent();

            AMSDbContext dbContext = new AMSDbContext();
            var deviceMeasurements = dbContext.Measurements.Where(x => x.AMSDeviceId == deviceId).OrderBy(a => a.TimeStamp).ToList();

            long minTime = deviceMeasurements[0].TimeStamp;
            long maxTime = deviceMeasurements[deviceMeasurements.Count - 1].TimeStamp;
            maxTime -= minTime;
            minTime = 0;

            double maxValue = deviceMeasurements.Max(x => x.Value);
            double minValue = deviceMeasurements.Min(x => x.Value);

            OxyPlot.Wpf.PlotView Plot;
            double[] graphData = new double[deviceMeasurements.Count];
            LineSeries fLine = new LineSeries { Title = "Value", StrokeThickness = 1 };

            Plot = new OxyPlot.Wpf.PlotView();
            Plot.Model = new PlotModel();
            Plot.Model.PlotType = PlotType.XY;
            Plot.Model.Background = OxyColor.FromRgb(255, 255, 255);

            groupBox_graph.Content = Plot;
            for (int i = 0; i < deviceMeasurements.Count; i++)
            {
                graphData[i] = deviceMeasurements[i].Value;
                fLine.Points.Add(new DataPoint(i, graphData[i]));
            }

            

            OxyPlot.Axes.LinearAxis fAxis = new OxyPlot.Axes.LinearAxis() { Position = OxyPlot.Axes.AxisPosition.Left, Minimum = minValue, Maximum = maxValue };
            fAxis.Key = "Value"; //Sets the key for the and amplitude.
            fAxis.Title = "Value";
            fLine.YAxisKey = fAxis.Key; //Assigns the key to the series.
            Plot.Model.Series.Add(fLine); //Adds the data for the frequency.
            Plot.Model.Axes.Add(new OxyPlot.Axes.LinearAxis() { Position = OxyPlot.Axes.AxisPosition.Bottom, Minimum = 0, Maximum = deviceMeasurements.Count }); //Adds the X axis.
            Plot.Model.Axes.Add(fAxis); //Adds the Y Axis for the frequency





        }
    }
}
