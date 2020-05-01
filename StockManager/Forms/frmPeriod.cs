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
            setTranslateMessage();
            if (periodId.HasValue)
                period = Session.Entities.GetPeriod(periodId.Value);
            else
                period = new Period();
        }

        private void setTranslateMessage()
        {
            btnSave.Text = Translate.GetMessage("save");
            label1.Text = $"{Translate.GetMessage("period-name")} : ";
            btnCancel.Text = Translate.GetMessage("cancel");
            label2.Text = $"{Translate.GetMessage("start-date")} : ";
            label3.Text = $"{Translate.GetMessage("end-date")} : ";
            Text = Translate.GetMessage("period");
            lblIsPublic.Text = Translate.GetMessage("is-public");
        }

        private bool validation()
        {
            bool result = true;
            if (string.IsNullOrEmpty(txtPeriodName.Text))
            {
                result = false;
                errorProvider1.SetError(txtPeriodName, Translate.GetMessage("cant-be-empty"));
            }

            if (dtStartDate.Value.DayStart() >= dtEndDate.Value.DayEnd())
            {
                result = false;
                errorProvider1.SetError(dtEndDate, Translate.GetMessage("startdate-cannot-be*greater-than-the-enddate"));
            }
            
            if (Session.Entities.Periods.Where(c => c.PeriodId != period.PeriodId && ((dtStartDate.Value.DayStart() <= c.EndDate.DayEnd() && dtStartDate.Value.DayStart() >= c.StartDate.DayStart()) || (dtEndDate.Value.DayEnd() <= c.EndDate.DayEnd() && dtEndDate.Value.DayEnd() >= c.StartDate.DayStart())) && !c.IsPublic && !cbIsPublic.Checked).Any())
            {
                result = false;
                MessageBox.Show(Translate.GetMessage("this-date-range-is-used"), Translate.GetMessage("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            cbIsPublic.Checked = period.IsPublic;
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
                period.IsPublic = cbIsPublic.Checked;
                Session.Entities.PostPeriod(period);
                Session.SaveChanges();
            }
        }
    }
}
