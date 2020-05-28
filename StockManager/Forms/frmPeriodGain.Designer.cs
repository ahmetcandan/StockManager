namespace StockManager
{
    partial class frmPeriodGain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint1 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, "1,0,0,0,0,0");
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint2 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(1D, "3.5,0,0,0,0,0");
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint3 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(2D, "4,0,0,0,0,0");
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint4 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(3D, "3,0,0,0,0,0");
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint5 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(4D, "6,0,0,0,0,0");
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint6 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(5D, "5,0,0,0,0,0");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPeriodGain));
            this.chartPeriodGain = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.cbGain = new System.Windows.Forms.CheckBox();
            this.cbExceptedGain = new System.Windows.Forms.CheckBox();
            this.cbConst = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.chartPeriodGain)).BeginInit();
            this.SuspendLayout();
            // 
            // chartPeriodGain
            // 
            this.chartPeriodGain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chartPeriodGain.BackColor = System.Drawing.Color.CornflowerBlue;
            this.chartPeriodGain.BorderlineColor = System.Drawing.Color.Black;
            chartArea1.BackColor = System.Drawing.Color.AliceBlue;
            chartArea1.BackSecondaryColor = System.Drawing.Color.White;
            chartArea1.Name = "ChartArea1";
            this.chartPeriodGain.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartPeriodGain.Legends.Add(legend1);
            this.chartPeriodGain.Location = new System.Drawing.Point(0, 57);
            this.chartPeriodGain.Margin = new System.Windows.Forms.Padding(2);
            this.chartPeriodGain.Name = "chartPeriodGain";
            series1.BorderWidth = 3;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Color = System.Drawing.Color.SkyBlue;
            series1.Legend = "Legend1";
            series1.MarkerBorderColor = System.Drawing.Color.DodgerBlue;
            series1.MarkerBorderWidth = 5;
            series1.MarkerColor = System.Drawing.Color.OliveDrab;
            series1.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series1.Name = "Series1";
            series1.Points.Add(dataPoint1);
            series1.Points.Add(dataPoint2);
            series1.Points.Add(dataPoint3);
            series1.Points.Add(dataPoint4);
            series1.Points.Add(dataPoint5);
            series1.Points.Add(dataPoint6);
            series1.ShadowOffset = 1;
            series1.YValuesPerPoint = 6;
            this.chartPeriodGain.Series.Add(series1);
            this.chartPeriodGain.Size = new System.Drawing.Size(936, 336);
            this.chartPeriodGain.TabIndex = 1;
            this.chartPeriodGain.Text = "chart1";
            // 
            // cbGain
            // 
            this.cbGain.AutoSize = true;
            this.cbGain.Checked = true;
            this.cbGain.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbGain.Font = new System.Drawing.Font("Hermit", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbGain.Location = new System.Drawing.Point(177, 11);
            this.cbGain.Margin = new System.Windows.Forms.Padding(2);
            this.cbGain.Name = "cbGain";
            this.cbGain.Size = new System.Drawing.Size(64, 24);
            this.cbGain.TabIndex = 3;
            this.cbGain.Text = "gain";
            this.cbGain.UseVisualStyleBackColor = true;
            this.cbGain.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
            // 
            // cbExceptedGain
            // 
            this.cbExceptedGain.AutoSize = true;
            this.cbExceptedGain.Font = new System.Drawing.Font("Hermit", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbExceptedGain.Location = new System.Drawing.Point(325, 11);
            this.cbExceptedGain.Margin = new System.Windows.Forms.Padding(2);
            this.cbExceptedGain.Name = "cbExceptedGain";
            this.cbExceptedGain.Size = new System.Drawing.Size(145, 24);
            this.cbExceptedGain.TabIndex = 3;
            this.cbExceptedGain.Text = "excepted-gain";
            this.cbExceptedGain.UseVisualStyleBackColor = true;
            this.cbExceptedGain.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
            // 
            // cbConst
            // 
            this.cbConst.AutoSize = true;
            this.cbConst.Checked = true;
            this.cbConst.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbConst.Font = new System.Drawing.Font("Hermit", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbConst.Location = new System.Drawing.Point(11, 11);
            this.cbConst.Margin = new System.Windows.Forms.Padding(2);
            this.cbConst.Name = "cbConst";
            this.cbConst.Size = new System.Drawing.Size(73, 24);
            this.cbConst.TabIndex = 3;
            this.cbConst.Text = "const";
            this.cbConst.UseVisualStyleBackColor = true;
            this.cbConst.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
            // 
            // frmPeriodGain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.CornflowerBlue;
            this.ClientSize = new System.Drawing.Size(937, 392);
            this.Controls.Add(this.cbConst);
            this.Controls.Add(this.cbExceptedGain);
            this.Controls.Add(this.cbGain);
            this.Controls.Add(this.chartPeriodGain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(393, 262);
            this.Name = "frmPeriodGain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "period-gain";
            this.Load += new System.EventHandler(this.frmStockAnalysis_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartPeriodGain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataVisualization.Charting.Chart chartPeriodGain;
        private System.Windows.Forms.CheckBox cbGain;
        private System.Windows.Forms.CheckBox cbExceptedGain;
        private System.Windows.Forms.CheckBox cbConst;
    }
}