using StockManager.Model;
using StockManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                stocks = DB.Entities.Stocks;
            else
                foreach (var stockCode in request.StockCodes)
                {
                    var stock = DB.Entities.GetStock(stockCode);
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
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void frmStockAnalysis_Load(object sender, EventArgs e)
        {
            lblInformations.Text = string.Empty;
            refreshList();
        }

        private void refreshList()
        {
            lvList.Items.Clear();
            List<StockAnalysis> stockAnalyses = new List<StockAnalysis>();
            StockAnalysis stockAnalysis = new StockAnalysis();
            decimal avarageConst = 0;
            decimal totalValue = 0;
            decimal totalGain = 0;
            decimal expectedGain = 0;
            List<StockTransaction> stockTransactions = new List<StockTransaction>();
            stockTransactions = DB.Entities.StockTransactions.Where(c => c.Date >= request.Period.StartDate && c.Date <= request.Period.EndDate).ToList();

            {
                var lastPeriodStockTransaciton = from st in DB.Entities.StockTransactions.DeepCopy()
                                                 where st.Date < request.Period.StartDate
                                                 group st by st.StockCode into StockTransaction
                                                 select new { StockCode = StockTransaction.Key, StockTransaction };

                foreach (var stock in lastPeriodStockTransaciton)
                {
                    if (stock.StockTransaction.Sum(c => c.Amount * (c.TransactionType == TransactionType.Sell ? -1 : 1)) > 0)
                    {
                        foreach (var stockTransaction in stock.StockTransaction.OrderBy(c => c.Date).Where(c => c.TransactionType == TransactionType.Sell))
                        {
                            decimal sellAmount = stockTransaction.Amount;

                            while (sellAmount > 0)
                            {
                                var buyTransaction = stock.StockTransaction.OrderBy(c => c.Date).FirstOrDefault(c => c.TransactionType == TransactionType.Buy && c.Amount > 0);
                                if (sellAmount > buyTransaction.Amount)
                                {
                                    sellAmount -= buyTransaction.Amount;
                                    buyTransaction.Amount = 0;
                                }
                                else
                                {
                                    buyTransaction.Amount -= sellAmount;
                                    sellAmount = 0;
                                }
                            }
                        }

                        decimal sumBuyAmount = stock.StockTransaction.Where(c => c.TransactionType == TransactionType.Buy && c.Amount > 0).Sum(c => c.Amount);
                        decimal sumBuyPrice = stock.StockTransaction.Where(c => c.TransactionType == TransactionType.Buy && c.Amount > 0).Sum(c => c.UnitPrice * c.Amount);
                        DateTime buyDate = stock.StockTransaction.Where(c => c.TransactionType == TransactionType.Buy && c.Amount > 0).Min(c => c.Date);
                        int stockTransactionId = stock.StockTransaction.OrderBy(c => c.Date).FirstOrDefault(c => c.TransactionType == TransactionType.Buy && c.Amount > 0).StockTransactionId;
                        stockTransactions.Add(new StockTransaction
                        {
                            Amount = sumBuyAmount,
                            UnitPrice = sumBuyPrice / sumBuyAmount,
                            TotalPrice = sumBuyPrice,
                            StockCode = stock.StockCode,
                            Date = buyDate,
                            TransactionType = TransactionType.Buy,
                            StockTransactionId = stockTransactionId,
                        });
                    }
                }
            }

            var result = from a in DB.Entities.Accounts
                         join al in DB.Entities.AccountTransactions on a.AccountId equals al.AccountId
                         join st in stockTransactions on al.StockTransactionId equals st.StockTransactionId
                         join s in DB.Entities.Stocks on st.StockCode equals s.StockCode
                         join s1 in stocks.Distinct() on s.StockCode equals s1.StockCode
                         where a.AccountId == DB.DefaultAccount.AccountId && s1 != null
                         orderby st.Date
                         group st by s into StockTransactions
                         select new { Stock = StockTransactions.Key, StockTransactions };

            foreach (var item in result.Where(c => c.StockTransactions.Sum(t => t.Amount * (t.TransactionType == TransactionType.Sell ? -1 : 1)) > 0))
                DB.Entities.GetStockService(item.Stock.StockCode);

            foreach (var stock in result)
            {
                stockAnalysis = new StockAnalysis();
                foreach (var stockTransaction in stock.StockTransactions)
                {
                    stockAnalysis.StockCode = stock.Stock.StockCode;
                    stockAnalysis.StockTransactions.Add(new StockAnalysisTransaction
                    {
                        Amount = stockTransaction.Amount,
                        Date = stockTransaction.Date,
                        StockTransactionId = stockTransaction.StockTransactionId,
                        TransactionType = stockTransaction.TransactionType,
                        UnitPrice = stockTransaction.UnitPrice
                    });

                    if (stockAnalysis.TotalAmount == 0)
                    {
                        stockAnalyses.Add(stockAnalysis);
                        stockAnalysis = new StockAnalysis();
                    }
                }

                if (stockAnalysis.StockTransactions.Count > 0)
                {
                    var partialStockAnalysis = new StockAnalysis();
                    partialStockAnalysis.StockCode = stockAnalysis.StockCode;
                    decimal diffarenceAmount = stockAnalysis.StockTransactions.Sum(c => c.Amount * (c.TransactionType == TransactionType.Sell ? -1 : 1));
                    bool isPartial = diffarenceAmount > 0 && stockAnalysis.StockTransactions.Any(c => c.TransactionType == TransactionType.Sell);
                    if (isPartial)
                    {
                        foreach (var stockTransaction in stockAnalysis.StockTransactions.Where(c => c.TransactionType == TransactionType.Buy).OrderByDescending(c => c.Date))
                        {
                            var partialStockTransaction = new StockAnalysisTransaction()
                            {
                                Amount = 0,
                                Date = stockTransaction.Date,
                                StockTransactionId = stockTransaction.StockTransactionId,
                                TransactionType = stockTransaction.TransactionType,
                                UnitPrice = stockTransaction.UnitPrice
                            };
                            if (stockTransaction.Amount >= diffarenceAmount)
                            {
                                stockTransaction.Amount -= diffarenceAmount;
                                partialStockTransaction.Amount = diffarenceAmount;
                                partialStockAnalysis.StockTransactions.Add(partialStockTransaction);
                                break;
                            }
                            else
                            {
                                diffarenceAmount -= stockTransaction.Amount;
                                partialStockTransaction.Amount = stockTransaction.Amount;
                                partialStockAnalysis.StockTransactions.Add(partialStockTransaction);
                                stockTransaction.Amount = 0;
                            }
                        }
                        stockAnalysis.StockTransactions.RemoveAll(c => c.Amount == 0);
                    }

                    stockAnalyses.Add(stockAnalysis);
                    if (isPartial) stockAnalyses.Add(partialStockAnalysis);
                    avarageConst = stockAnalysis.BuyPrice;
                }
            }

            foreach (var item in stockAnalyses)
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
                if (DB.Entities.CurrentStocks.Any(c => c.StockCode == item.StockCode))
                    curValue = DB.Entities.CurrentStocks.OrderByDescending(c => c.CreatedDate).FirstOrDefault(c => c.StockCode == item.StockCode).Price;
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
                if (item.TotalAmount > 0)
                    expectedGain += gain;
                else
                    totalGain += gain;
                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "TotalValue",
                    Text = value.ToMoneyStirng(2)
                });

                lvList.Items.Add(li);
            }

            lblInformations.Text = $"[{Translate.GetMessage("total-gain")}: {totalGain.ToMoneyStirng(2)}, {Translate.GetMessage("total-const")}: {stockAnalyses.Sum(c => c.TotalConst).ToMoneyStirng(2)}, {Translate.GetMessage("available-value")}: {totalValue.ToMoneyStirng(2)}, {Translate.GetMessage("expected-gain")}: {expectedGain.ToMoneyStirng(2)}]";
        }
    }

    class StockAnalysis
    {
        public StockAnalysis()
        {
            StockTransactions = new List<StockAnalysisTransaction>();
        }

        public string StockCode { get; set; }
        public decimal Gain { get { return TotalAmount == 0 ? (TotalValue - TotalConst) : 0; } }
        public decimal TotalAmount { get { return StockTransactions.Sum(c => c.Amount * (c.TransactionType == TransactionType.Sell ? -1 : 1)); } }
        public decimal TotalBuyAmount { get { return StockTransactions.Where(c => c.TransactionType == TransactionType.Buy).Sum(c => c.Amount); } }
        public decimal TotalSellAmount { get { return StockTransactions.Where(c => c.TransactionType == TransactionType.Sell).Sum(c => c.Amount); } }
        public decimal TotalConst { get { return StockTransactions.Sum(c => c.Const); } }
        public decimal BuyPrice
        {
            get
            {
                if (!StockTransactions.Any(c => c.TransactionType == TransactionType.Buy))
                    return 0;
                return StockTransactions.Where(c => c.TransactionType == TransactionType.Buy).Sum(c => c.UnitPrice * c.Amount) / StockTransactions.Where(c => c.TransactionType == TransactionType.Buy).Sum(c => c.Amount);
            }
        }
        public decimal SellPrice
        {
            get
            {
                if (!StockTransactions.Any(c => c.TransactionType == TransactionType.Sell))
                    return 0;
                return StockTransactions.Where(c => c.TransactionType == TransactionType.Sell).Sum(c => c.UnitPrice * c.Amount) / StockTransactions.Where(c => c.TransactionType == TransactionType.Sell).Sum(c => c.Amount);
            }
        }
        public decimal TotalValue { get { return StockTransactions.Sum(c => c.Value * (c.TransactionType == TransactionType.Buy ? -1 : 1)); } }
        public List<StockAnalysisTransaction> StockTransactions { get; set; }
    }

    class StockAnalysisTransaction
    {
        public int StockTransactionId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Value { get { return Amount * UnitPrice; } }
        public decimal Const { get { return Value / 1000 * 2; } }
        public TransactionType TransactionType { get; set; }
    }
}
