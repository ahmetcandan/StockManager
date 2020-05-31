using StockManager.Business;
using StockManager.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace StockManager
{
    public partial class frmStockChart : Form
    {
        Stock stock;

        public frmStockChart(string stockCode)
        {
            stock = Session.Entities.GetStock(stockCode);
            InitializeComponent();
            setTranslateMessage();
        }

        List<Series> series = new List<Series>();

        private void setTranslateMessage()
        {
            Text = $"[{stock.StockCode} - {stock.Name}] {Translate.GetMessage("stock-chart")}";
        }

        private void frmStockAnalysis_Load(object sender, EventArgs e)
        {
            var chartType = SeriesChartType.Spline;

            chartCurrentStock.Series.Clear();
            Series valueSeries = new Series();

            valueSeries.ChartType = chartType;
            valueSeries.MarkerBorderColor = Color.MediumVioletRed;
            valueSeries.MarkerColor = Color.MediumVioletRed;
            valueSeries.Color = Color.Crimson;
            valueSeries.MarkerSize = 5;
            valueSeries.MarkerStep = 1;
            valueSeries.MarkerBorderWidth = 5;
            valueSeries.BorderWidth = 3;
            valueSeries.MarkerStyle = MarkerStyle.Circle;
            valueSeries.Name = Translate.GetMessage("value");

            foreach (var stockCurrent in Session.Entities.GetCurrentStocks().Where(c => c.StockCode == stock.StockCode))
            {
                DataPoint pointTotalConst = new DataPoint();
                pointTotalConst.SetValueXY(stockCurrent.UpdateDate.ToShortDateString(), stockCurrent.Price.ToMoneyStirng(2));
                pointTotalConst.ToolTip = $"{Session.DefaultAccount.MoneyType.MoneyTypeToString()} {stockCurrent.Price.ToMoneyStirng(2)} - {stockCurrent.UpdateDate.ToShortDateString()} {stockCurrent.UpdateDate.ToShortTimeString()}";
                valueSeries.Points.Add(pointTotalConst);
            }
            chartCurrentStock.Series.Add(valueSeries);
            series.Add(valueSeries);
        }
    }
}
