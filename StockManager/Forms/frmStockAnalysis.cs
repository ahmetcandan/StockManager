using Borsa.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Borsa
{
    public partial class frmStockAnalysis : Form
    {
        List<Stock> stocks = new List<Stock>();

        public frmStockAnalysis(string stockCodes = "")
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(stockCodes))
                stocks = DB.Entities.Stocks;
            else
                foreach (var stockCode in stockCodes.Split(','))
                {
                    var stock = DB.Entities.GetStock(stockCode);
                    if (stock != null) stocks.Add(stock);
                }
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
            decimal totalConst = 0;

            var result = from a in DB.Entities.Accounts
                         join al in DB.Entities.AccountTransactions on a.AccountId equals al.AccountId
                         join st in DB.Entities.StockTransactions on al.StockTransactionId equals st.StockTransactionId
                         join s in DB.Entities.Stocks on st.StockCode equals s.StockCode
                         join s1 in stocks.Distinct() on s.StockCode equals s1.StockCode
                         where a.AccountId == DB.DefaultAccount.AccountId && s1 != null
                         orderby st.Date
                         group st by s into StockTransactions
                         select new { Stock = StockTransactions.Key, StockTransactions };

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
                    Text = item.TotalAmount > 0 ? "Available" : ((item.TotalValue - item.TotalConst) > 0 ? "Advantage" : "Loss")
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
                    Text = item.SellPrice == 0 ? "" : item.SellPrice.ToMoneyStirng(6)
                });
                if (item.TotalAmount == 0)
                    totalValue += item.TotalValue - item.TotalConst;
                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "TotalValue",
                    Text = Math.Abs(item.TotalValue - item.TotalConst).ToMoneyStirng(2)
                });

                lvList.Items.Add(li);
            }

            lblInformations.Text = $"[Const: {totalConst.ToMoneyStirng(2)}, Total Value: {totalValue.ToMoneyStirng(2)}]";
            //lblInformations.Text = $"[ Total Amount: {result.Sum(c => c.Amount * (c.TransactionType == TransactionType.Sell ? -1 : 1)).ToMoneyStirng(0)}{(avarageConst != 0 ? $", Avarage Const: {avarageConst.ToMoneyStirng(2)}" : "") }, {(totalValue > 0 ? "Advantage" : "Loss")} : {Math.Abs(totalValue).ToMoneyStirng(2)}]";
        }
    }

    class StockAnalysis
    {
        public StockAnalysis()
        {
            StockTransactions = new List<StockAnalysisTransaction>();
        }

        public string StockCode { get; set; }
        public decimal TotalAmount { get { return StockTransactions.Sum(c => c.Amount * (c.TransactionType == TransactionType.Sell ? -1 : 1)); } }
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
