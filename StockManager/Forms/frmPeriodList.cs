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
    public partial class frmPeriodList : Form
    {
        public frmPeriodList()
        {
            InitializeComponent();
        }

        private void setTranslateMessage()
        {
            PeriodName.Text = Translate.GetMessage("period-name");
            StartDate.Text = Translate.GetMessage("start-date");
            EndDate.Text = Translate.GetMessage("end-date");
            editToolStripMenuItem.Text = Translate.GetMessage("edit");
            addToolStripMenuItem.Text = Translate.GetMessage("add");
            deleteToolStripMenuItem.Text = Translate.GetMessage("delete");
            refreshToolStripMenuItem.Text = Translate.GetMessage("refresh");
            Text = Translate.GetMessage("period-list");
        }

        private void frmStockAnalysis_Load(object sender, EventArgs e)
        {
            refreshList();
        }

        private void refreshList()
        {
            lvList.Items.Clear();
            foreach (var period in DB.Entities.Periods.Where(c => c.AccountId == DB.DefaultAccount.AccountId))
            {
                var li = new ListViewItem();
                li.Text = period.PeriodId.ToString();
                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "PeriodName",
                    Text = period.PeriodName
                });
                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "StartDate",
                    Text = period.StartDate.ToShortDateString()
                });
                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "EndDate",
                    Text = period.EndDate.ToShortDateString()
                });

                lvList.Items.Add(li);
            }
        }

        private void lvList_DoubleClick(object sender, EventArgs e)
        {
            if (lvList.SelectedItems.Count > 0)
            {
                int periodId = int.Parse(lvList.SelectedItems[0].Text);
                frmPeriod frm = new frmPeriod(periodId);
                frm.ShowDialog();
                refreshList();
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refreshList();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPeriod frm = new frmPeriod();
            frm.ShowDialog();
            refreshList();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvList_DoubleClick(sender, e);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Translate.GetMessage("delete-period"), Translate.GetMessage("delete"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int periodId = int.Parse(lvList.SelectedItems[0].Text);
                DB.Entities.Periods.RemoveAll(c => c.PeriodId == periodId);
                DB.SaveChanges();
                refreshList();
            }
        }

        private void listMenu_Opening(object sender, CancelEventArgs e)
        {
            editToolStripMenuItem.Enabled = lvList.SelectedItems.Count == 1;
            deleteToolStripMenuItem.Enabled = editToolStripMenuItem.Enabled;
        }
    }
}
