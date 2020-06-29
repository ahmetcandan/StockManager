using StockManager.Business;
using System;
using System.Collections.Generic;
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

        List<Series> series = new List<Series>();

        private void setTranslateMessage()
        {
            Text = Translate.GetMessage("period-gain-list");
            cbConst.Text = Translate.GetMessage("const");
            cbExceptedGain.Text = Translate.GetMessage("expected-gain");
            cbGain.Text = Translate.GetMessage("gain");
            const1.Text = Translate.GetMessage("const");
            gain.Text = Translate.GetMessage("gain");
            period.Text = Translate.GetMessage("period");
            endOfDay.Text = Translate.GetMessage("end-of-day");
            expectedGain.Text = Translate.GetMessage("expected-gain");
            tabGraphich.Text = Translate.GetMessage("graphic");
            tabGrid.Text = Translate.GetMessage("grid");
        }

        private void frmStockAnalysis_Load(object sender, EventArgs e)
        {
            var chartType = SeriesChartType.Spline;
            lvList.Items.Clear();

            chartPeriodGain.Series.Clear();
            Series totalGainSeries = new Series();
            Series totalConstSeries = new Series();
            Series expectedGainSeries = new Series();

            totalConstSeries.ChartType = chartType;
            totalConstSeries.MarkerBorderColor = Color.MediumVioletRed;
            totalConstSeries.MarkerColor = Color.MediumVioletRed;
            totalConstSeries.Color = Color.Crimson;
            totalConstSeries.MarkerSize = 5;
            totalConstSeries.MarkerStep = 1;
            totalConstSeries.MarkerBorderWidth = 5;
            totalConstSeries.BorderWidth = 3;
            totalConstSeries.MarkerStyle = MarkerStyle.Circle;
            totalConstSeries.Name = Translate.GetMessage("const");

            totalGainSeries.ChartType = chartType;
            totalGainSeries.MarkerBorderColor = Color.SteelBlue;
            totalGainSeries.MarkerColor = Color.SteelBlue;
            totalGainSeries.Color = Color.RoyalBlue;
            totalGainSeries.MarkerSize = 5;
            totalGainSeries.BorderWidth = 3;
            totalGainSeries.MarkerStep = 1;
            totalGainSeries.MarkerBorderWidth = 5;
            totalGainSeries.MarkerStyle = MarkerStyle.Circle;
            totalGainSeries.Name = Translate.GetMessage("gain");

            expectedGainSeries.ChartType = chartType;
            expectedGainSeries.MarkerBorderColor = Color.Goldenrod;
            expectedGainSeries.MarkerColor = Color.Goldenrod;
            expectedGainSeries.Color = Color.Gold;
            expectedGainSeries.MarkerSize = 5;
            expectedGainSeries.BorderWidth = 3;
            expectedGainSeries.MarkerStep = 1;
            expectedGainSeries.MarkerBorderWidth = 5;
            expectedGainSeries.MarkerStyle = MarkerStyle.Circle;
            expectedGainSeries.Name = Translate.GetMessage("expected-gain");

            foreach (var period in Session.Entities.GetPeriods().Where(c => !c.IsPublic && c.StartDate < DateTime.Now))
            {
                StockAnalysisManager analysisManager = new StockAnalysisManager(new StockAnalysisRequest
                {
                    Period = period
                });
                analysisManager.RefreshList();

                var li = new ListViewItem();
                li.Text = period.PeriodId.ToString();
                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "month",
                    Text = period.PeriodName
                });
                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "gain",
                    Text = analysisManager.TotalGain.ToMoneyStirng(2)
                });
                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "const",
                    Text = analysisManager.TotalConst.ToMoneyStirng(2)
                });
                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "expectedGain",
                    Text = analysisManager.ExpectedGain.ToMoneyStirng(2)
                });
                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "endOfDay",
                    Text = (analysisManager.TotalGain + analysisManager.ExpectedGain).ToMoneyStirng(2)
                });

                lvList.Items.Add(li);

                DataPoint pointTotalAgain = new DataPoint();
                pointTotalAgain.SetValueXY(period.PeriodName, analysisManager.TotalGain);
                pointTotalAgain.ToolTip = $"{Session.DefaultAccount.MoneyType.MoneyTypeToString()} {analysisManager.TotalGain.ToMoneyStirng(2)}";
                totalGainSeries.Points.Add(pointTotalAgain);
                DataPoint pointTotalConst = new DataPoint();
                pointTotalConst.SetValueXY(period.PeriodName, analysisManager.TotalConst);
                pointTotalConst.ToolTip = $"{Session.DefaultAccount.MoneyType.MoneyTypeToString()} {analysisManager.TotalConst.ToMoneyStirng(2)}";
                totalConstSeries.Points.Add(pointTotalConst);
                DataPoint pointExceptedGain = new DataPoint();
                pointExceptedGain.SetValueXY(period.PeriodName, analysisManager.ExpectedGain);
                pointExceptedGain.ToolTip = $"{Session.DefaultAccount.MoneyType.MoneyTypeToString()} {analysisManager.ExpectedGain.ToMoneyStirng(2)}";
                expectedGainSeries.Points.Add(pointExceptedGain);
            }

            chartPeriodGain.Series.Add(totalGainSeries);
            chartPeriodGain.Series.Add(totalConstSeries);
            series.Add(totalGainSeries);
            series.Add(totalConstSeries);
            series.Add(expectedGainSeries);
        }

        private void cb_CheckedChanged(object sender, EventArgs e)
        {
            var cb = ((CheckBox)sender);
            if (!cb.Checked && chartPeriodGain.Series.Any(c => c.Name == cb.Text))
                chartPeriodGain.Series.Remove(series.FirstOrDefault(c => c.Name == cb.Text));
            else if (cb.Checked && !chartPeriodGain.Series.Any(c => c.Name == cb.Text))
                chartPeriodGain.Series.Add(series.FirstOrDefault(c => c.Name == cb.Text));
        }
    }
}
