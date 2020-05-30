using System;
using System.Windows.Forms;

namespace StockManager
{
    public partial class frmTranslateMessage : Form
    {
        TranslateMessage message;

        public frmTranslateMessage(string languageCode, string code)
        {
            InitializeComponent();
            setTranslateMessage();
            txtLanguageCode.Text = languageCode;
            message = Session.Entities.GetMessage(code, languageCode);
        }

        public frmTranslateMessage(string languageCode = "")
        {
            InitializeComponent();
            setTranslateMessage();
            message = new TranslateMessage();
            message.LanguageCode = languageCode;
        }

        private void setTranslateMessage()
        {
            btnSave.Text = Translate.GetMessage("save");
            label1.Text = $"{Translate.GetMessage("language-code")} : ";
            btnCancel.Text = Translate.GetMessage("cancel");
            label2.Text = $"{Translate.GetMessage("code")} : ";
            label3.Text = $"{Translate.GetMessage("value")} : ";
            Text = Translate.GetMessage("period");
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
            try
            {
                errorProvider1.Clear();
                if (validation())
                {
                    message.LanguageCode = txtLanguageCode.Text;
                    message.Code = txtCode.Text;
                    message.Value = txtValue.Text;
                    Session.Entities.PostMessage(message);
                    Session.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Translate.GetMessage("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
