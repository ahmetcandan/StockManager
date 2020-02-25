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
    public partial class frmPeriod : Form
    {
        Period period;

        public frmPeriod(int? periodId = null)
        {
            InitializeComponent();
            if (periodId.HasValue)
                period = DB.Entities.GetPeriod(periodId.Value);
            else
                period = new Period();
        }

        private bool validation()
        {
            bool result = true;
            if (string.IsNullOrEmpty(txtPeriodName.Text))
            {
                result = false;
                errorProvider1.SetError(txtPeriodName, "Can't be empty");
            }

            if (dtStartDate.Value >= dtEndDate.Value)
            {
                result = false;
                errorProvider1.SetError(dtEndDate, "Start date cannot be greater than the end date");
            }

            if (DB.Entities.Periods.Where(c => c.PeriodId != period.PeriodId && ((dtStartDate.Value <= c.EndDate && dtStartDate.Value >= c.StartDate) || (dtEndDate.Value <= c.EndDate && dtEndDate.Value >= c.StartDate))).Any())
            {
                result = false;
                MessageBox.Show("This date range is used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return result;
        }

        private void frmAccount_Load(object sender, EventArgs e)
        {
            txtPeriodName.Text = period.PeriodName;
            if (period.StartDate != DateTime.MinValue)
                dtStartDate.Value = period.StartDate;
            if (period.EndDate != DateTime.MinValue)
                dtEndDate.Value = period.EndDate;
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
                period.PeriodName = txtPeriodName.Text;
                period.EndDate = dtEndDate.Value.SmallDate();
                period.StartDate = dtStartDate.Value.SmallDate();
                DB.Entities.PostPeriod(period);
                DB.Save();
            }
        }
    }
}
