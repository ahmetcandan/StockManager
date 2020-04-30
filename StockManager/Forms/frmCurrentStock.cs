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
    public partial class frmCurrentStock : Form
    {
        StockCurrent entity;

        public frmCurrentStock()
        {
            InitializeComponent();
            setTranslateMessage();
            entity = new StockCurrent();
        }

        private void setTranslateMessage()
        {
            btnSave.Text = Translate.GetMessage("save");
            label1.Text = $"{Translate.GetMessage("stock-code")} : ";
            btnCancel.Text = Translate.GetMessage("cancel");
            label2.Text = $"{Translate.GetMessage("current-price")} : ";
            label3.Text = $"{Translate.GetMessage("update-date")} : ";
            Text = Translate.GetMessage("current-stock-information");
        }

        private bool validation()
        {
            bool result = true;
            if (string.IsNullOrEmpty(txtStockCode.Text))
            {
                result = false;
                errorProvider1.SetError(txtStockCode, Translate.GetMessage("cant-be-empty"));
            }

            decimal price;

            if (string.IsNullOrEmpty(txtCurrentPrice.Text))
            {
                result = false;
                errorProvider1.SetError(txtCurrentPrice, Translate.GetMessage("cant-be-empty"));
            }
            else if (!decimal.TryParse(txtCurrentPrice.Text, out price))
            {
                result = false;
                errorProvider1.SetError(txtCurrentPrice, Translate.GetMessage("enter-a-numerical-value"));
            }

            if (dtUpdateDate.Value > DateTime.Now)
            {
                result = false;
                errorProvider1.SetError(dtUpdateDate, Translate.GetMessage("updatedate-cannot-be-greater-than-the-now"));
            }

            return result;
        }

        private void frmAccount_Load(object sender, EventArgs e)
        {
            dtUpdateDate.Value = DateTime.Now;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            if (validation())
            {
                entity.StockCode = txtStockCode.Text.ToUpper();
                entity.Price = decimal.Parse(txtCurrentPrice.Text);
                entity.UpdateDate = dtUpdateDate.Value;
                entity.CreatedDate = DateTime.Now;
                Session.Entities.CurrentStocks.Add(entity);
                Session.SaveChanges();
            }
        }

        private void txtStockCode_Leave(object sender, EventArgs e)
        {
            txtStockCode.Text = txtStockCode.Text.ToUpper();
        }
    }
}
