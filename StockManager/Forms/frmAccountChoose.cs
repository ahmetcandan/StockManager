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
    public partial class frmAccountChoose : Form
    {
        public frmAccountChoose()
        {
            InitializeComponent();
            setTranslateMessage();
        }

        private void frmStockAnalysis_Load(object sender, EventArgs e)
        {
            refreshList();
        }

        private void setTranslateMessage()
        {
            AccountName.Text = Translate.GetMessage("account-name");
            MoneyType.Text = Translate.GetMessage("money-type");
            chooseAccountToolStripMenuItem.Text = Translate.GetMessage("choose-account");
            refreshToolStripMenuItem.Text = Translate.GetMessage("refresh");
            editToolStripMenuItem.Text = Translate.GetMessage("edit");
            addToolStripMenuItem.Text = Translate.GetMessage("add");
            deleteToolStripMenuItem.Text = Translate.GetMessage("delete");
            Text = Translate.GetMessage("bank-choose");
        }

        private void refreshList()
        {
            lvList.Items.Clear();
            foreach (var account in Session.User.Accounts)
            {
                var li = new ListViewItem();
                li.Text = account.AccountId.ToString();
                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "AccountName",
                    Text = account.AccountName
                }); ;
                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "MoneyType",
                    Text = account.MoneyType.MoneyTypeToString()
                });

                lvList.Items.Add(li);
            }
        }

        private void lvList_DoubleClick(object sender, EventArgs e)
        {
            if (lvList.SelectedItems.Count > 0)
            {
                Session.DefaultAccount = Session.Entities.GetAccount(int.Parse(lvList.SelectedItems[0].Text));
                Close();
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refreshList();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAccount frm = new frmAccount();
            frm.ShowDialog();
            refreshList();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAccount frm = new frmAccount(int.Parse(lvList.SelectedItems[0].Text));
            frm.ShowDialog();
            refreshList();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"{Translate.GetMessage("delete-account")}?", $"{Translate.GetMessage("delete")}?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int accountId = int.Parse(lvList.SelectedItems[0].Text);
                Session.User.Accounts.RemoveAll(c => c.AccountId == accountId);
                Session.Entities.Accounts.RemoveAll(c => c.AccountId == accountId);
                Session.SaveChanges();
                refreshList();
            }
        }

        private void chooseAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvList_DoubleClick(sender, e);
        }

        private void listMenu_Opening(object sender, CancelEventArgs e)
        {
            editToolStripMenuItem.Enabled = lvList.SelectedItems.Count == 1;
            deleteToolStripMenuItem.Enabled = editToolStripMenuItem.Enabled;
            chooseAccountToolStripMenuItem.Enabled = editToolStripMenuItem.Enabled;
        }
    }
}
