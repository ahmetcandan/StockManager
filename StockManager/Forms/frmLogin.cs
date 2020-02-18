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
                errorProvider1.SetError(txtUserName, "Can't be empty");
            }

            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                result = false;
                errorProvider1.SetError(txtPassword, "Can't be empty");
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
                            MessageBox.Show("Account not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show("User name or password is incorrect", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            DB.Save();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            try
            {
                DB.Entities = new DataAccess();
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
