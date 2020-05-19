using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace StockManager
{
    public partial class frmPeriodGain : Form
    {
        public frmPeriodGain()
        {
            InitializeComponent();
            setTranslateMessage();
        }

        private void setTranslateMessage()
        {
            Text = Translate.GetMessage("period-list");
        }

        private void frmStockAnalysis_Load(object sender, EventArgs e)
        {

        }
    }
}
