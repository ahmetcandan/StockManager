using System;
using System.Linq;
using System.Windows.Forms;

namespace StockManager
{
    public partial class frmLogin : Form
    {
        bool passwordIsHash = false;

        public frmLogin()
        {
            InitializeComponent();
            setTranslateMessage();
        }

        private void setTranslateMessage()
        {
            btnLogin.Text = Translate.GetMessage("login");
            label1.Text = $"{Translate.GetMessage("user-name")} : ";
            label2.Text = $"{Translate.GetMessage("password")} : ";
            btnCreate.Text = Translate.GetMessage("create");
            cbRememberUserName.Text = Translate.GetMessage("remember-user-name");
            cbRememberPassword.Text = Translate.GetMessage("remember-password");
            Text = Translate.GetMessage("login");
        }

        private bool validation()
        {
            bool result = true;
            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                result = false;
                errorProvider1.SetError(txtUserName, Translate.GetMessage("cant-be-empty"));
            }

            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                result = false;
                errorProvider1.SetError(txtPassword, Translate.GetMessage("cant-be-empty"));
            }

            return result;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            frmUser frm = new frmUser();
            frm.ShowDialog();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (validation())
            {
                var user = Session.Entities.GetUser(txtUserName.Text, passwordIsHash ? Session.Entities.Setting.PasswordHash : txtPassword.Text.ComputeSha256Hash(txtUserName.Text));
                if (!string.IsNullOrEmpty(user.LanguageCode))
                    Session.Entities.Setting.LanguageCode = user.LanguageCode;
                if (user != null)
                {
                    SettingSave();
                    frmMain frmMain = new frmMain();
                    Session.User = user;

                    if (Session.User.Accounts.Count == 0)
                    {
                        frmAccount frm = new frmAccount();
                        frm.ShowDialog();
                        Session.DefaultAccount = Session.User.Accounts.FirstOrDefault();
                        if (Session.DefaultAccount != null)
                        {
                            Hide();
                            frmMain.Show();
                        }
                        else
                            MessageBox.Show(Translate.GetMessage("account-not-found"), Translate.GetMessage("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (Session.User.Accounts.Count == 1)
                    {
                        Hide();
                        Session.DefaultAccount = user.Accounts.FirstOrDefault();
                        frmMain.Show();
                    }
                    else if (Session.User.Accounts.Any(c => c.DefaultAccount))
                    {
                        Hide();
                        Session.DefaultAccount = user.Accounts.FirstOrDefault(c => c.DefaultAccount);
                        frmMain.Show();
                    }
                    else
                    {
                        frmAccountChoose frmAccountChoose = new frmAccountChoose();
                        frmAccountChoose.ShowDialog();
                        if (Session.DefaultAccount != null)
                        {
                            frmMain.Show();
                            Hide();
                        }
                    }
                }
                else
                {
                    MessageBox.Show(Translate.GetMessage("username-or-password-incorrect"), Translate.GetMessage("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SettingSave()
        {
            Session.Entities.Setting.RememberUserName = cbRememberUserName.Checked;
            Session.Entities.Setting.RememberPassword = cbRememberPassword.Checked;
            if (cbRememberUserName.Checked)
                Session.Entities.Setting.UserName = txtUserName.Text;
            else
                Session.Entities.Setting.UserName = string.Empty;
            if (cbRememberPassword.Checked)
                Session.Entities.Setting.PasswordHash = passwordIsHash ? Session.Entities.Setting.PasswordHash : txtPassword.Text.ComputeSha256Hash(txtUserName.Text);
            else
                Session.Entities.Setting.PasswordHash = string.Empty;
            Session.SaveChanges();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            try
            {
                Session.DefaultAccount = null;
                cbRememberUserName.Checked = Session.Entities.Setting.RememberUserName;
                cbRememberPassword.Checked = Session.Entities.Setting.RememberPassword;
                passwordIsHash = Session.Entities.Setting.RememberPassword;
                txtUserName.Text = Session.Entities.Setting.UserName;
                if (Session.Entities.Setting.RememberPassword)
                {
                    txtPassword.Text = "PASSWORD";
                    passwordIsHash = true;
                }

                if (Session.Entities.Setting.RememberUserName)
                    txtPassword.Focus();
                if (Session.Entities.Setting.RememberPassword)
                    txtUserName.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Translate.GetMessage("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbRememberUserName_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbRememberUserName.Checked)
                cbRememberPassword.Checked = false;
            cbRememberPassword.Enabled = cbRememberUserName.Checked;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            passwordIsHash = false;
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            txtPassword.Clear();
        }
    }
}
