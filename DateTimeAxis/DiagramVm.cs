using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateTimeAxis
{
    public class DiagramVm : IDisposable
    {
        private LinearAxis yAxis;
        private OxyPlot.Axes.DateTimeAxis xAxis;

        private LineSeries series1;
        private LineSeries series2;

        private System.Threading.Thread updateThread;
        private Random rnd = new Random();
        private TimeSpan span = new TimeSpan(0, 0, 20);

        public DiagramVm()
        {
            PlotModel = new PlotModel();
            PlotModel.LegendOrientation = LegendOrientation.Horizontal;
            PlotModel.LegendPlacement = LegendPlacement.Outside;
            PlotModel.LegendPosition = LegendPosition.BottomLeft;

            yAxis = new LinearAxis();
            yAxis.Position = AxisPosition.Left;
            yAxis.Maximum = 100;
            yAxis.Minimum = 0;
            PlotModel.Axes.Add(yAxis);

            xAxis = new OxyPlot.Axes.DateTimeAxis();
            xAxis.StringFormat = "HH:mm:ss";
            PlotModel.Axes.Add(xAxis);

            series1 = new LineSeries();
            PlotModel.Series.Add(series1);

            series2 = new LineSeries();
            PlotModel.Series.Add(series2);


            updateThread = new System.Threading.Thread(() => UpdateDiagram());
            updateThread.Name = "DiagramVm_UpdateThread";
            updateThread.Start();
        }

        public PlotModel PlotModel { get; set; }

        private void UpdateDiagram()
        {
            while (true)
            {
                AddNewValue();

                System.Threading.Thread.Sleep(200);
            }
        }

        private void AddNewValue()
        {
            double timestamp =  OxyPlot.Axes.DateTimeAxis.ToDouble(DateTime.Now);

            xAxis.Minimum = OxyPlot.Axes.DateTimeAxis.ToDouble(DateTime.Now.Add(new TimeSpan(0, 0, -20)));
            xAxis.Maximum = timestamp;

            series1.Points.Add(new DataPoint(timestamp, rnd.Next(0, 80)));
            series2.Points.Add(new DataPoint(timestamp, rnd.Next(20, 100)));

            PlotModel.InvalidatePlot(true);
        }

        public void Dispose()
        {
            try
            {
                updateThread.Abort();
            }
            catch
            {

            }
        }
    }
}
