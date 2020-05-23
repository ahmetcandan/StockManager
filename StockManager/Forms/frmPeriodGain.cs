using StockManager.Business;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace StockManager
{
    public partial class frmPeriodGain : Form
    {
        public frmPeriodGain()
        {
            InitializeComponent();
            setTranslateMessage();
        }

        private void setTranslateMessage()
        {
            Text = Translate.GetMessage("period-gain-list");
        }

        private void frmStockAnalysis_Load(object sender, EventArgs e)
        {
            int i = 0;
            var chartType = SeriesChartType.Spline;

            chartPeriodGain.Series.Clear();
            Series totalGainSeries = new Series();
            Series totalConstSeries = new Series();
            totalConstSeries.ChartType = chartType;
            totalConstSeries.MarkerBorderColor = Color.Crimson;
            totalConstSeries.MarkerColor = Color.Crimson;
            totalConstSeries.MarkerSize = 5;
            totalConstSeries.MarkerStep = 1;
            totalConstSeries.MarkerBorderWidth = 5;
            totalConstSeries.BorderWidth = 3;
            totalConstSeries.MarkerStyle = MarkerStyle.Circle;
            totalConstSeries.Name = Translate.GetMessage("const");
            totalGainSeries.ChartType = chartType;
            totalGainSeries.MarkerBorderColor = Color.RoyalBlue;
            totalGainSeries.MarkerColor = Color.RoyalBlue;
            totalGainSeries.MarkerSize = 5;
            totalGainSeries.BorderWidth = 3;
            totalGainSeries.MarkerStep = 1;
            totalGainSeries.MarkerBorderWidth = 5;
            totalGainSeries.MarkerStyle = MarkerStyle.Circle;
            totalGainSeries.Name = Translate.GetMessage("gain");
            foreach (var period in Session.Entities.GetPeriods().Where(c => !c.IsPublic && c.StartDate < DateTime.Now))
            {
                StockAnalysisManager analysisManager = new StockAnalysisManager(new StockAnalysisRequest
                {
                    Period = period
                });
                analysisManager.RefreshList();
                totalGainSeries.Points.AddXY(period.PeriodName, analysisManager.TotalGain);
                totalConstSeries.Points.AddXY(period.PeriodName, analysisManager.TotalConst);
                i++;
            }
            chartPeriodGain.Series.Add(totalGainSeries);
            chartPeriodGain.Series.Add(totalConstSeries);
        }
    }
}
