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
    public partial class frmTranslateMessage : Form
    {
        TranslateMessage message;

        public frmTranslateMessage(string code = "")
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(code))
                message = new TranslateMessage();
            else
                message = DB.Entities.GetMessage(code);
        }

        private bool validation()
        {
            bool result = true;
            if (string.IsNullOrEmpty(txtLanguageCode.Text))
            {
                result = false;
                errorProvider1.SetError(txtLanguageCode, Translate.GetMessage("cant-be-empty"));
            }

            if (string.IsNullOrEmpty(txtCode.Text))
            {
                result = false;
                errorProvider1.SetError(txtCode, Translate.GetMessage("cant-be-empty"));
            }

            if (string.IsNullOrEmpty(txtValue.Text))
            {
                result = false;
                errorProvider1.SetError(txtValue, Translate.GetMessage("cant-be-empty"));
            }

            return result;
        }

        private void frmTranslateMessage_Load(object sender, EventArgs e)
        {
            txtLanguageCode.Text = message.LanguageCode;
            txtCode.Text = message.Code;
            txtValue.Text = message.Value;
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
                message.LanguageCode = txtLanguageCode.Text;
                message.Code = txtCode.Text;
                message.Value = txtValue.Text;
                DB.Entities.PostMessage(message);
                DB.SaveChanges();
            }
        }
    }
}
