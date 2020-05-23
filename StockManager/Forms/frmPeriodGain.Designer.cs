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
            ((System.ComponentModel.ISupportInitialize)(this.chartPeriodGain)).BeginInit();
            this.SuspendLayout();
            // 
            // chartPeriodGain
            // 
            this.chartPeriodGain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chartPeriodGain.BackColor = System.Drawing.Color.SteelBlue;
            chartArea1.BackColor = System.Drawing.Color.AliceBlue;
            chartArea1.BackSecondaryColor = System.Drawing.Color.White;
            chartArea1.Name = "ChartArea1";
            this.chartPeriodGain.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartPeriodGain.Legends.Add(legend1);
            this.chartPeriodGain.Location = new System.Drawing.Point(0, 0);
            this.chartPeriodGain.Name = "chartPeriodGain";
            series1.BorderWidth = 3;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Color = System.Drawing.Color.RoyalBlue;
            series1.Legend = "Legend1";
            series1.MarkerBorderColor = System.Drawing.Color.RoyalBlue;
            series1.MarkerBorderWidth = 5;
            series1.MarkerColor = System.Drawing.Color.CornflowerBlue;
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
            this.chartPeriodGain.Size = new System.Drawing.Size(835, 460);
            this.chartPeriodGain.TabIndex = 1;
            this.chartPeriodGain.Text = "chart1";
            // 
            // frmPeriodGain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 459);
            this.Controls.Add(this.chartPeriodGain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(538, 506);
            this.Name = "frmPeriodGain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "period-gain";
            this.Load += new System.EventHandler(this.frmStockAnalysis_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartPeriodGain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataVisualization.Charting.Chart chartPeriodGain;
    }
}