using StockManager.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using StockManager.Business;

namespace StockManager
{
    public partial class frmStockAnalysis : Form
    {
        List<Stock> stocks = new List<Stock>();
        StockAnalysisRequest request = new StockAnalysisRequest();

        public frmStockAnalysis(StockAnalysisRequest request)
        {
            this.request = request;
            InitializeComponent();
            setTranslateMessage();
            if (request.StockCodes.Count == 0)
                stocks = Session.Entities.GetStocks();
            else
                foreach (var stockCode in request.StockCodes)
                {
                    var stock = Session.Entities.GetStock(stockCode);
                    if (stock != null) stocks.Add(stock);
                }
            Text = $"{request.Period.PeriodName} - {Translate.GetMessage("stock-analysis")}";
        }

        private void setTranslateMessage()
        {
            lblInformations.Text = Translate.GetMessage("information");
            refreshToolStripMenuItem.Text = Translate.GetMessage("refresh");
            deleteToolStripMenuItem.Text = Translate.GetMessage("delete");
            addToolStripMenuItem.Text = Translate.GetMessage("add");
            editToolStripMenuItem.Text = Translate.GetMessage("edit");
            StockCode.Text = Translate.GetMessage("stock-code");
            Status.Text = Translate.GetMessage("status");
            TotalAmount.Text = Translate.GetMessage("total-amount");
            BuyPrice.Text = Translate.GetMessage("buy-price");
            SellPrice.Text = Translate.GetMessage("sell-price");
            Gain.Text = Translate.GetMessage("gain");
            TotalValue.Text = Translate.GetMessage("total-value");
            Text = Translate.GetMessage("stock-analysis");
            Date.Text = Translate.GetMessage("date");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void frmStockAnalysis_Load(object sender, EventArgs e)
        {
            lblInformations.Text = string.Empty;
            StockAnalysisManager stockAnaliysisManager = new StockAnalysisManager(request);
            stockAnaliysisManager.RefreshList();
            gridFill(stockAnaliysisManager);
        }

        private void gridFill(StockAnalysisManager analysisManager)
        {
            #region Grid
            decimal totalValue = 0;
            foreach (var item in analysisManager.StockAnalyses.OrderBy(c => c.LastTransactionDate))
            {
                var li = new ListViewItem();
                li.Text = item.StockCode;
                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "Status",
                    Text = item.TotalAmount > 0 ? Translate.GetMessage("available") : ((item.TotalValue - item.TotalConst) > 0 ? Translate.GetMessage("avantage") : Translate.GetMessage("loss"))
                });
                if (item.TotalAmount == 0)
                    if ((item.TotalValue - item.TotalConst) > 0)
                    {
                        li.BackColor = Color.PaleGreen;
                        li.ForeColor = Color.Black;
                    }
                    else
                    {
                        li.BackColor = Color.PaleVioletRed;
                        li.ForeColor = Color.Black;
                    }
                else
                {
                    li.BackColor = Color.Tan;
                    li.ForeColor = Color.Black;
                }

                decimal? curValue = null;
                if (Session.Entities.GetCurrentStocks().Any(c => c.StockCode == item.StockCode && c.CreatedDate <= request.Period.EndDate))
                    curValue = Session.Entities.GetCurrentStocks().Where(c => c.CreatedDate <= request.Period.EndDate).OrderByDescending(c => c.CreatedDate).FirstOrDefault(c => c.StockCode == item.StockCode).Price;
                decimal value = item.TotalAmount == 0 ? (item.TotalSellAmount * item.SellPrice - item.TotalConst) : (item.TotalBuyAmount * (curValue.HasValue ? curValue.Value : item.BuyPrice));
                if (item.TotalAmount > 0)
                    totalValue += value;

                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "TotalAmount",
                    Text = item.TotalAmount == 0 ? item.StockTransactions.Where(c => c.TransactionType == TransactionType.Buy).Sum(c => c.Amount).ToMoneyStirng(0) : item.TotalAmount.ToMoneyStirng(0)
                });
                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "BuyPrice",
                    Text = item.BuyPrice.ToMoneyStirng(6)
                });
                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "SellPrice",
                    Text = item.TotalAmount > 0 && curValue.HasValue ? $"[{curValue.Value.ToMoneyStirng(6)}]" : (item.SellPrice > 0 ? item.SellPrice.ToMoneyStirng(6) : "")
                });
                decimal gain = item.TotalAmount == 0 ? (item.TotalValue - item.TotalConst) : (curValue.HasValue ? (item.TotalBuyAmount * curValue.Value - item.TotalBuyAmount * item.BuyPrice - item.TotalConst) : 0);
                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "Gain",
                    Text = gain == 0 ? "" : gain.ToMoneyStirng(2)
                });
                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "Date",
                    Text = item.LastTransactionDate.ToShortDateString()
                });
                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "TotalValue",
                    Text = value.ToMoneyStirng(2)
                });

                lvList.Items.Add(li);
            }
            lblInformations.Text = $"[{Translate.GetMessage("total-gain")}: {analysisManager.TotalGain.ToMoneyStirng(2)}, {Translate.GetMessage("total-const")}: {analysisManager.TotalConst.ToMoneyStirng(2)}, {Translate.GetMessage("available-value")}: {totalValue.ToMoneyStirng(2)}, {Translate.GetMessage("expected-gain")}: {analysisManager.ExpectedGain.ToMoneyStirng(2)}]";
            #endregion
        }
    }
}
