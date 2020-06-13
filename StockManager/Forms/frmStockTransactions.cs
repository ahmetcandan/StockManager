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
    public partial class frmStockTransactions : Form
    {
        List<Stock> stocks = new List<Stock>();
        StockAnalysis stockAnalysis;

        public frmStockTransactions(StockAnalysis stockAnalysis)
        {
            InitializeComponent();
            setTranslateMessage();
            this.stockAnalysis = stockAnalysis;
            Text = $"{stockAnalysis.StockCode} - {Translate.GetMessage("stock-transactions")}";
        }

        private void setTranslateMessage()
        {
            lblInformations.Text = Translate.GetMessage("information");
            UnitPrice.Text = Translate.GetMessage("unit-price");
            Amount.Text = Translate.GetMessage("amount");
            Const.Text = Translate.GetMessage("const");
            Total.Text = Translate.GetMessage("total");
            TransactionType.Text = Translate.GetMessage("transaction-type");
            Date.Text = Translate.GetMessage("date");
        }

        private void frmStockTransactions_Load(object sender, EventArgs e)
        {
            gridFill();
        }

        private void gridFill()
        {
            #region Grid
            foreach (var item in stockAnalysis.StockTransactions.OrderBy(c => c.Date))
            {
                var li = new ListViewItem();
                li.Text = item.StockTransactionId.ToString();
                if (item.TransactionType == Model.TransactionType.Buy)
                {
                    li.BackColor = Color.LightSkyBlue;
                    li.ForeColor = Color.Black;
                }
                else
                {
                    li.BackColor = Color.LightPink;
                    li.ForeColor = Color.Black;
                }

                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "UnitPrice",
                    Text = item.UnitPrice.ToMoneyStirng(6)
                });
                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "Amount",
                    Text = item.Amount.ToMoneyStirng(0)
                });
                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "Total",
                    Text = (item.Amount * item.UnitPrice).ToMoneyStirng(2)
                });
                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "Const",
                    Text = item.Const.ToMoneyStirng(2)
                });
                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "TransactionType",
                    Text = item.TransactionType == Model.TransactionType.Buy ? Translate.GetMessage("buy") : Translate.GetMessage("sell")
                });
                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "Date",
                    Text = item.Date.ToShortDateString()
                });

                lvList.Items.Add(li);
            }

            decimal totalConst = stockAnalysis.StockTransactions.Sum(c => c.Const);
            decimal gain = stockAnalysis.StockTransactions.Sum(c => c.Amount * c.UnitPrice * (c.TransactionType == Model.TransactionType.Buy ? -1 : 1));
            lblInformations.Text = $"[{Translate.GetMessage("total-buy")}: {stockAnalysis.StockTransactions.Where(c => c.TransactionType == Model.TransactionType.Buy).Sum(c => c.Amount).ToMoneyStirng(0)}, {Translate.GetMessage("total-sell")}: {stockAnalysis.StockTransactions.Where(c => c.TransactionType == Model.TransactionType.Sell).Sum(c => c.Amount).ToMoneyStirng(0)}, {Translate.GetMessage("total-const")}: {totalConst.ToMoneyStirng(2)}, {Translate.GetMessage("gain")}: {(gain - totalConst).ToMoneyStirng(2)}]";
            #endregion
        }
    }
}
