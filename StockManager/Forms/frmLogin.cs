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
    public partial class frmLogin : Form
    {
        bool passwordIsHash = false;

        public frmLogin()
        {
            InitializeComponent();
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
                var user = DB.Entities.GetUser(txtUserName.Text, passwordIsHash ? DB.Entities.Setting.PasswordHash : txtPassword.Text.ComputeSha256Hash(txtUserName.Text));
                if (!string.IsNullOrEmpty(user.LanguageCode))
                    DB.LanguageCode = user.LanguageCode;
                if (user != null)
                {
                    SettingSave();
                    frmMain frmMain = new frmMain();
                    DB.User = user;

                    if (DB.User.Accounts.Count == 0)
                    {
                        frmAccount frm = new frmAccount();
                        frm.ShowDialog();
                        DB.DefaultAccount = DB.User.Accounts.FirstOrDefault();
                        if (DB.DefaultAccount != null)
                        {
                            Hide();
                            frmMain.Show();
                        }
                        else
                            MessageBox.Show(Translate.GetMessage("account-not-found"), Translate.GetMessage("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (DB.User.Accounts.Count == 1)
                    {
                        Hide();
                        DB.DefaultAccount = user.Accounts.FirstOrDefault();
                        frmMain.Show();
                    }
                    else if (DB.User.Accounts.Any(c => c.DefaultAccount))
                    {
                        Hide();
                        DB.DefaultAccount = user.Accounts.FirstOrDefault(c => c.DefaultAccount);
                        frmMain.Show();
                    }
                    else
                    {
                        frmAccountChoose frmAccountChoose = new frmAccountChoose();
                        frmAccountChoose.ShowDialog();
                        if (DB.DefaultAccount != null)
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
            DB.Entities.Setting.RememberUserName = cbRememberUserName.Checked;
            DB.Entities.Setting.RememberPassword = cbRememberPassword.Checked;
            if (cbRememberUserName.Checked)
                DB.Entities.Setting.UserName = txtUserName.Text;
            else
                DB.Entities.Setting.UserName = string.Empty;
            if (cbRememberPassword.Checked)
                DB.Entities.Setting.PasswordHash = passwordIsHash ? DB.Entities.Setting.PasswordHash : txtPassword.Text.ComputeSha256Hash(txtUserName.Text);
            else
                DB.Entities.Setting.PasswordHash = string.Empty;
            DB.SaveChanges();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            try
            {
                DB.DefaultAccount = null;
                cbRememberUserName.Checked = DB.Entities.Setting.RememberUserName;
                cbRememberPassword.Checked = DB.Entities.Setting.RememberPassword;
                passwordIsHash = DB.Entities.Setting.RememberPassword;
                txtUserName.Text = DB.Entities.Setting.UserName;
                if (DB.Entities.Setting.RememberPassword)
                {
                    txtPassword.Text = "PASSWORD";
                    passwordIsHash = true;
                }

                if (DB.Entities.Setting.RememberUserName)
                    txtPassword.Focus();
                if (DB.Entities.Setting.RememberPassword)
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
