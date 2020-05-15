using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace StockManager
{
    public partial class frmTranslateMessageList : Form
    {
        public frmTranslateMessageList()
        {
            InitializeComponent();
            setTranslateMessage();
        }

        private void cbLanguage_Fill()
        {
            foreach (var language in Session.Entities.GetLanguageCodeList())
            {
                cbLanguage.Items.Add(new ComboboxItem
                {
                    Code = language,
                    Object = language,
                    Value = language
                });
            }
        }

        private void setTranslateMessage()
        {
            PeriodName.Text = Translate.GetMessage("language-code");
            StartDate.Text = Translate.GetMessage("code");
            EndDate.Text = Translate.GetMessage("value");
            editToolStripMenuItem.Text = Translate.GetMessage("edit");
            addToolStripMenuItem.Text = Translate.GetMessage("add");
            deleteToolStripMenuItem.Text = Translate.GetMessage("delete");
            refreshToolStripMenuItem.Text = Translate.GetMessage("refresh");
            Text = Translate.GetMessage("period-list");
            label2.Text = $"{Translate.GetMessage("language")} : ";
        }

        private void frmStockAnalysis_Load(object sender, EventArgs e)
        {
            cbLanguage_Fill();
            cbLanguage.Text = Session.Entities.Setting.LanguageCode;
            refreshList();
        }

        private void refreshList()
        {
            lvList.Items.Clear();
            foreach (var message in Session.Entities.TranslateMessages.Where(c => c.LanguageCode == cbLanguage.Text &&
            (c.Value.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0 || c.Code.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0)
            ).OrderBy(c => c.LanguageCode).OrderBy(c => c.Code))
            {
                var li = new ListViewItem();
                li.Text = message.Code.ToString();
                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "LanguageCode",
                    Text = message.LanguageCode
                });
                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "Code",
                    Text = message.Code
                });
                li.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Name = "Value",
                    Text = message.Value
                });

                lvList.Items.Add(li);
            }
        }

        private void lvList_DoubleClick(object sender, EventArgs e)
        {
            if (lvList.SelectedItems.Count > 0)
            {
                string code = lvList.SelectedItems[0].Text;
                frmTranslateMessage frm = new frmTranslateMessage(code);
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
            frmTranslateMessage frm = new frmTranslateMessage();
            frm.ShowDialog();
            refreshList();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvList_DoubleClick(sender, e);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Translate.GetMessage("delete-message"), Translate.GetMessage("delete"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string code = lvList.SelectedItems[0].Text;
                Session.Entities.TranslateMessages.RemoveAll(c => c.Code == code);
                Session.SaveChanges();
                refreshList();
            }
        }

        private void listMenu_Opening(object sender, CancelEventArgs e)
        {
            editToolStripMenuItem.Enabled = lvList.SelectedItems.Count == 1;
            deleteToolStripMenuItem.Enabled = editToolStripMenuItem.Enabled;
        }

        private void cbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            refreshList();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            refreshList();
        }
    }
}
