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
    public partial class frmAccount : Form
    {
        Account account;

        public frmAccount(int? accountId = null)
        {
            InitializeComponent();
            if (accountId.HasValue)
                account = DB.Entities.GetAccount(accountId.Value);
            else
                account = new Account();
        }

        private bool validation()
        {
            bool result = true;
            if (string.IsNullOrEmpty(txtAccountName.Text))
            {
                result = false;
                errorProvider1.SetError(txtAccountName, Translate.GetMessage("cant-be-empty"));
            }

            if (string.IsNullOrEmpty(cbMoneyType.Text))
            {
                result = false;
                errorProvider1.SetError(cbMoneyType, Translate.GetMessage("cant-be-empty"));
            }

            return result;
        }

        private void frmAccount_Load(object sender, EventArgs e)
        {
            txtAccountName.Text = account.AccountName;
            cbMoneyType.Text = account.MoneyType.MoneyTypeToString();
            if (account.AccountId > 0)
                cbSetDefault.Checked = DB.User.Accounts.FirstOrDefault(c => c.AccountId == account.AccountId).DefaultAccount;
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
                account.AccountName = txtAccountName.Text;
                switch (cbMoneyType.Text)
                {
                    case "TRY":
                        account.MoneyType = MoneyType.TRY;
                        break;
                    case "USD":
                        account.MoneyType = MoneyType.USD;
                        break;
                    case "EUR":
                        account.MoneyType = MoneyType.EUR;
                        break;
                    default:
                        break;
                }
                account.DefaultAccount = cbSetDefault.Checked;
                DB.Entities.PostAccount(account);
                DB.SaveChanges();
            }
        }
    }
}
