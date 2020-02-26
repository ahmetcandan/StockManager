﻿using StockManager.Model;
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
    public partial class frmTranslateMessageList : Form
    {
        public frmTranslateMessageList()
        {
            InitializeComponent();
        }

        private void frmStockAnalysis_Load(object sender, EventArgs e)
        {
            refreshList();
        }

        private void refreshList()
        {
            lvList.Items.Clear();
            foreach (var message in DB.Entities.TranslateMessages.OrderBy(c => c.LanguageCode).OrderBy(c => c.Code))
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
                DB.Entities.TranslateMessages.RemoveAll(c => c.Code == code);
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