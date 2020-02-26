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
    public partial class frmUser : Form
    {
        User user;
        bool passwordIsHash = false;
        string passwordHash = string.Empty;
        bool isNew = false;

        public frmUser(string userName, string passwordHash)
        {
            InitializeComponent();
            this.passwordHash = passwordHash;
            passwordIsHash = true;
            isNew = false;
            user = DB.Entities.GetUser(userName, passwordHash);
        }

        public frmUser()
        {
            InitializeComponent();
            user = new User();
            isNew = true;
            lblCurrentPassword.Visible = false;
            txtCurrentPassword.Visible = false;
        }

        private bool validation()
        {
            bool result = true;
            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                result = false;
                errorProvider1.SetError(txtUserName, Translate.GetMessage("cant-be-empty"));
            }
            else if (txtPassword.Text.Length < 3)
            {
                result = false;
                errorProvider1.SetError(txtUserName, Translate.GetMessage("username-min-length-3-charecter"));
            }

            if (!isNew && string.IsNullOrEmpty(txtCurrentPassword.Text))
            {
                result = false;
                errorProvider1.SetError(txtCurrentPassword, Translate.GetMessage("cant-be-empty"));
            }

            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                result = false;
                errorProvider1.SetError(txtPassword, Translate.GetMessage("cant-be-empty"));
            }
            else if (txtPassword.Text.Length < 4)
            {
                result = false;
                errorProvider1.SetError(txtPassword, Translate.GetMessage("password-min-length-is-4-charecter"));
            }

            if (string.IsNullOrEmpty(txtConfirmPassword.Text))
            {
                result = false;
                errorProvider1.SetError(txtConfirmPassword, "Can't be empty");
            }
            else if (txtConfirmPassword.Text != txtPassword.Text)
            {
                result = false;
                errorProvider1.SetError(txtConfirmPassword, Translate.GetMessage("password-and-confirmpassword-are-not-equals"));
            }

            return result;
        }

        private void frmAccount_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(user.UserName))
            {
                txtUserName.Text = user.UserName;
                txtPassword.Text = "PASSWORD";
                cbIsActive.Checked = user.IsActive;
            }
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
                try
                {
                    user.UserName = txtUserName.Text;
                    if (!passwordIsHash)
                        user.Password = txtPassword.Text.ComputeSha256Hash(txtUserName.Text);
                    user.IsActive = cbIsActive.Checked;
                    DB.Entities.PostUser(user, isNew);
                    DB.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Translate.GetMessage("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            passwordIsHash = false;
        }
    }
}
