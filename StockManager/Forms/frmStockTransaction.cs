using StockManager.Model;
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
    public partial class frmStockTransaction : Form
    {
        StockTransaction stockTransaction;
        Stock stock;
        decimal amount = 0;
        decimal unitPrice = 0;

        public frmStockTransaction(int? stockTransactionId = null)
        {
            InitializeComponent();
            if (stockTransactionId.HasValue)
            {
                stockTransaction = DB.Entities.GetStockTransaction(stockTransactionId.Value);
                stock = stockTransaction.Stock;
                txtAmount.Text = stockTransaction.Amount.ToMoneyStirng(2);
                txtStockCode.Text = stockTransaction.StockCode;
                txtStockName.Text = stock.Name;
                txtTotalPrice.Text = stockTransaction.TotalPrice.ToMoneyStirng(2);
                txtUnitPrice.Text = stockTransaction.UnitPrice.ToMoneyStirng(6);
                cbType.Text = stockTransaction.TransactionType == TransactionType.Buy ? "Buy" : "Sell";
                dtDate.Value = stockTransaction.Date;
            }
            else
            {
                stockTransaction = new StockTransaction();
                stock = new Stock();
            }
            dtDate.Value = DateTime.Now;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            if (validation())
            {
                stock.Name = txtStockName.Text;
                decimal amount = decimal.Parse(txtAmount.Text);
                decimal unitPrice = decimal.Parse(txtUnitPrice.Text);
                stock.StockCode = txtStockCode.Text.ToUpper();
                stock.UpdateDate = DateTime.Now;


                decimal currentAmount = DB.Entities.StockTransactions.Where(c => c.StockCode == stock.StockCode).Sum(c => c.Amount * (c.TransactionType == TransactionType.Sell ? -1 : 1));
                if (stockTransaction.StockTransactionId > 0)
                    currentAmount += stockTransaction.Amount;

                if(cbType.Text == "Sell" && amount > currentAmount)
                {
                    MessageBox.Show("There is not enough stock.", "Stock Amount Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                stockTransaction.StockCode = txtStockCode.Text.ToUpper();
                stockTransaction.Date = dtDate.Value;
                stockTransaction.UnitPrice = stock.Value;

                if (cbType.Text == "Buy")
                    stockTransaction.TransactionType = TransactionType.Buy;
                else if (cbType.Text == "Sell")
                    stockTransaction.TransactionType = TransactionType.Sell;

                stockTransaction.Amount = amount;
                stockTransaction.UnitPrice = unitPrice;
                stockTransaction.TotalPrice = stockTransaction.UnitPrice * stockTransaction.Amount;

                DB.Entities.PostStock(stock);
                DB.Entities.PostStockTransaction(stockTransaction);
                DB.Save();
                Close();
            }
        }

        private bool validation()
        {
            decimal amount;
            decimal unitPrice;
            bool result = true;
            if (string.IsNullOrEmpty(cbType.Text))
            {
                result = false;
                errorProvider1.SetError(cbType, "Can't be empty");
            }

            if (string.IsNullOrEmpty(txtAmount.Text))
            {
                result = false;
                errorProvider1.SetError(txtAmount, "Can't be empty");
            }
            else if (!decimal.TryParse(txtAmount.Text, out amount))
            {
                result = false;
                errorProvider1.SetError(txtAmount, "Enter a numerical value");
            }

            if (string.IsNullOrEmpty(txtUnitPrice.Text))
            {
                result = false;
                errorProvider1.SetError(txtUnitPrice, "Can't be empty");
            }
            else if (!decimal.TryParse(txtUnitPrice.Text, out unitPrice))
            {
                result = false;
                errorProvider1.SetError(txtUnitPrice, "Enter a numerical value");
            }

            if (string.IsNullOrEmpty(txtStockCode.Text))
            {
                result = false;
                errorProvider1.SetError(txtStockCode, "Can't be empty");
            }

            return result;
        }

        private void frmStock_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(stock.StockCode))
                getStock(stock.StockCode);
        }

        private void calculateTotalPrice()
        {
            txtTotalPrice.Text = (unitPrice * amount).ToMoneyStirng(2);
        }

        private void getStock(string stockCode)
        {
            if (!string.IsNullOrEmpty(stockCode))
            {
                stock = DB.Entities.GetStock(stockCode);
                txtStockName.Text = stock.Name;
                txtStockCode.Text = string.IsNullOrEmpty(stock.StockCode) ? stockCode.ToUpper() : stockCode;
                txtStockName.Enabled = stock.Value == 0;
            }
        }

        private void txtUnitPrice_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUnitPrice.Text))
                errorProvider1.SetError(txtUnitPrice, "Can't be empty");
            else if (!decimal.TryParse(txtUnitPrice.Text, out unitPrice))
                errorProvider1.SetError(txtUnitPrice, "Enter a numerical value");
            else
            {
                stockTransaction.UnitPrice = unitPrice;
                stock.Value = unitPrice;
                calculateTotalPrice();
            }
        }

        private void txtAmount_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAmount.Text))
                errorProvider1.SetError(txtAmount, "Can't be empty");
            else if (!decimal.TryParse(txtAmount.Text, out amount))
                errorProvider1.SetError(txtAmount, "Enter a numerical value");
            else
            {
                calculateTotalPrice();
            }
        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtStockCode_Leave(object sender, EventArgs e)
        {
            if (txtStockCode.Text.Length >= 4)
                getStock(txtStockCode.Text.ToUpper());
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
