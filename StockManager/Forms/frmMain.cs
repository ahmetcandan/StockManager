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
    public partial class frmMain : Form
    {
        bool left = false;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            timer1.Start();
            refreshList();
        }

        private void refreshList()
        {
            lvList.Items.Clear();
            var list = from a in DB.Entities.Accounts
                       join al in DB.Entities.AccountTransactions on a.AccountId equals al.AccountId
                       join st in DB.Entities.StockTransactions on al.StockTransactionId equals st.StockTransactionId
                       join s in DB.Entities.Stocks on st.StockCode equals s.StockCode
                       where a.AccountId == DB.DefaultAccount.AccountId
                       orderby st.Date
                       select new
                       {
                           Stock = s,
                           StockTransaction = st
                       };
            foreach (var item in list)
            {
                var li = new ListViewItem();
                li.Text = item.StockTransaction.StockTransactionId.ToString();
                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "StockCode",
                    Text = item.Stock.StockCode
                });
                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "StockName",
                    Text = item.Stock.Name
                });
                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "UnitPrice",
                    Text = item.StockTransaction.UnitPrice.ToMoneyStirng(6)
                });
                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "Amount",
                    Text = item.StockTransaction.Amount.ToMoneyStirng(0)
                });
                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "Status",
                    Text = item.StockTransaction.TransactionType == TransactionType.Buy ? "Buy" : "Sell"
                });
                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "TotalPrice",
                    Text = (item.StockTransaction.UnitPrice * item.StockTransaction.Amount).ToMoneyStirng(2)
                });
                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "Const",
                    Text = (item.StockTransaction.TotalPrice / 1000 * 2).ToMoneyStirng(2)
                });
                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "Date",
                    Text = item.StockTransaction.Date.ToShortDateString()
                });

                lvList.Items.Add(li);
            }

            var liTotal = new ListViewItem();
            liTotal.Text = "0";
            liTotal.SubItems.Add(new ListViewItem.ListViewSubItem()
            {
                Name = "StockCode",
                Text = ""
            });
            liTotal.SubItems.Add(new ListViewItem.ListViewSubItem()
            {
                Name = "StockName",
                Text = ""
            });
            liTotal.SubItems.Add(new ListViewItem.ListViewSubItem()
            {
                Name = "UnitPrice",
                Text = ""
            });
            liTotal.SubItems.Add(new ListViewItem.ListViewSubItem()
            {
                Name = "Amount",
                Text = ""
            });
            liTotal.SubItems.Add(new ListViewItem.ListViewSubItem()
            {
                Name = "Status",
                Text = "Total"
            });
            var totalPrice = list.Sum(c => c.StockTransaction.TotalPrice * (c.StockTransaction.TransactionType == TransactionType.Buy ? -1 : 1));
            liTotal.SubItems.Add(new ListViewItem.ListViewSubItem()
            {
                Name = "TotalPrice",
                Text = totalPrice.ToMoneyStirng(2)
            });
            var totalConst = list.Sum(c => c.StockTransaction.TotalPrice / 1000 * 2);
            liTotal.SubItems.Add(new ListViewItem.ListViewSubItem()
            {
                Name = "Const",
                Text = totalConst.ToMoneyStirng(2)
            });
            liTotal.SubItems.Add(new ListViewItem.ListViewSubItem()
            {
                Name = "Date",
                Text = ""
            });
            liTotal.BackColor = Color.DarkSlateGray;

            lvList.Items.Add(liTotal);
            refreshInformations();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmStockTransaction frm = new frmStockTransaction();
            frm.ShowDialog();
            refreshList();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refreshList();
        }

        private void listMenu_Opening(object sender, CancelEventArgs e)
        {
            editToolStripMenuItem.Enabled = lvList.SelectedItems.Count > 0 && !(lvList.SelectedItems.Count == 1 && lvList.SelectedItems[0].Text == "0");
            deleteToolStripMenuItem.Enabled = editToolStripMenuItem.Enabled;
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editStockTransaction();
        }

        private void editStockTransaction()
        {
            if (lvList.SelectedItems.Count == 0 || (lvList.SelectedItems.Count == 1 && lvList.SelectedItems[0].Text == "0")) return;
            int stockTransactionId = int.Parse(lvList.SelectedItems[0].Text);
            frmStockTransaction frm = new frmStockTransaction(stockTransactionId);
            frm.ShowDialog();
            refreshList();
        }

        private void lvList_DoubleClick(object sender, EventArgs e)
        {
            editStockTransaction();
        }

        private void lvList_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                editStockTransaction();
        }

        private void lvList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvList.SelectedItems.Count > 0 && !(lvList.SelectedItems.Count == 1 && lvList.SelectedItems[0].Text == "0"))
            {
                List<int> ids = new List<int>();
                for (int i = 0; i < lvList.SelectedItems.Count; i++)
                    ids.Add(int.Parse(lvList.SelectedItems[i].Text));
                refreshInformations(ids);
            }
            else refreshInformations();
        }

        private void refreshInformations(List<int> ids = null)
        {
            lblInformations.Text = string.Empty;
            var result = from a in DB.Entities.Accounts
                         join al in DB.Entities.AccountTransactions on a.AccountId equals al.AccountId
                         join st in DB.Entities.StockTransactions on al.StockTransactionId equals st.StockTransactionId
                         join s in DB.Entities.Stocks on st.StockCode equals s.StockCode
                         where a.AccountId == DB.DefaultAccount.AccountId
                         && (ids == null || ids.Contains(st.AccountTransactionId))
                         orderby st.Date
                         select new
                         {
                             Stock = s,
                             StockTransaction = st
                         };
            var stocks = from s in DB.Entities.Stocks
                         where result.Select(c => c.Stock.StockCode).Contains(s.StockCode)
                         select new StockInformation
                         {
                             Stock = s,
                             TotalAmount = result.Where(c => c.Stock.StockCode == s.StockCode)
                                            .Sum(c => c.StockTransaction.Amount *
                                                (c.StockTransaction.TransactionType == TransactionType.Sell ? -1 : 1)),
                             TotalPrice = result.Where(c => c.Stock.StockCode == s.StockCode)
                                            .Sum(c => c.StockTransaction.UnitPrice * c.StockTransaction.Amount *
                                                (c.StockTransaction.TransactionType == TransactionType.Buy ? -1 : 1)),
                             Const = (result.Where(c => c.Stock.StockCode == s.StockCode).Sum(c => c.StockTransaction.UnitPrice * c.StockTransaction.Amount) / 1000) * 2
                         };

            foreach (var item in stocks)
            {
                lblInformations.Text += $"[Stock Code: {item.Stock.StockCode}, Total Amount: {item.TotalAmount.ToMoneyStirng(0)}, Total Price: {(item.TotalPrice).ToMoneyStirng(2)}] | ";
            }
            lblInformations.Text += $"[Const: {stocks.Sum(c => c.Const).ToMoneyStirng(2)}, Total Price: {stocks.Sum(c => c.TotalPrice - c.Const).ToMoneyStirng(2)}]";
            lblInformation2.Text = lblInformations.Text;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (lblInformations.Size.Width > label1.Size.Width)
            {
                lblInformations.Location = new Point(lblInformations.Location.X + 1, lblInformations.Location.Y);
                if (lblInformations.Location.X > label1.Size.Width)
                    lblInformations.Location = lblInformation2.Location;

                lblInformation2.Location = new Point(lblInformations.Location.X - lblInformation2.Size.Width - 50, lblInformations.Location.Y);
                lblInformation2.Visible = true;
            }
            else
            {
                lblInformation2.Visible = false;
                if (left)
                    lblInformations.Location = new Point(lblInformations.Location.X - 1, lblInformations.Location.Y);
                else
                    lblInformations.Location = new Point(lblInformations.Location.X + 1, lblInformations.Location.Y);

                if (left && lblInformations.Location.X <= 0)
                    left = false;
                else if (!left && (lblInformations.Location.X + lblInformations.Size.Width) >= label1.Size.Width)
                    left = true;
            }
        }

        private void lblInformations_TextChanged(object sender, EventArgs e)
        {
            if (lblInformations.Size.Width < label1.Size.Width)
                lblInformations.Location = new Point(0, lblInformations.Location.Y);
            else lblInformations.Location = new Point(label1.Size.Width - lblInformations.Size.Width - 500, lblInformations.Location.Y);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete record", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DB.Entities.DeleteStockTransaction(int.Parse(lvList.SelectedItems[0].Text));
                refreshList();
            }
        }

        private void analysisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> stockCodes = new List<string>();
            for (int i = 0; i < lvList.SelectedItems.Count; i++)
                stockCodes.Add(lvList.SelectedItems[i].SubItems["StockCode"].Text);
            frmStockAnalysis frm = new frmStockAnalysis(string.Join(",", stockCodes));
            frm.ShowDialog();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void changeAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAccountChoose frm = new frmAccountChoose();
            frm.ShowDialog();
            refreshList();
        }

        private void createAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAccount frm = new frmAccount();
            frm.ShowDialog();
        }
    }

    class StockInformation
    {
        public Stock Stock { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Const { get; set; }
    }
}
