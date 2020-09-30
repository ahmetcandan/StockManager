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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabGrid = new System.Windows.Forms.TabPage();
            this.lvList = new System.Windows.Forms.ListView();
            this.id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.period = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gain = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.const1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.expectedGain = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.endOfDay = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cumulativeEndOfDay = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabGraphich = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.chartPeriodGain)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabGrid.SuspendLayout();
            this.tabGraphich.SuspendLayout();
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
            this.chartPeriodGain.Location = new System.Drawing.Point(2, 51);
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
            this.chartPeriodGain.Size = new System.Drawing.Size(925, 315);
            this.chartPeriodGain.TabIndex = 1;
            this.chartPeriodGain.Text = "chart1";
            // 
            // cbGain
            // 
            this.cbGain.AutoSize = true;
            this.cbGain.Checked = true;
            this.cbGain.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbGain.Font = new System.Drawing.Font("Hermit", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbGain.Location = new System.Drawing.Point(179, 5);
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
            this.cbExceptedGain.Location = new System.Drawing.Point(327, 5);
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
            this.cbConst.Location = new System.Drawing.Point(13, 5);
            this.cbConst.Margin = new System.Windows.Forms.Padding(2);
            this.cbConst.Name = "cbConst";
            this.cbConst.Size = new System.Drawing.Size(73, 24);
            this.cbConst.TabIndex = 3;
            this.cbConst.Text = "const";
            this.cbConst.UseVisualStyleBackColor = true;
            this.cbConst.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabGrid);
            this.tabControl1.Controls.Add(this.tabGraphich);
            this.tabControl1.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1018, 393);
            this.tabControl1.TabIndex = 4;
            // 
            // tabGrid
            // 
            this.tabGrid.Controls.Add(this.lvList);
            this.tabGrid.Location = new System.Drawing.Point(4, 23);
            this.tabGrid.Name = "tabGrid";
            this.tabGrid.Padding = new System.Windows.Forms.Padding(3);
            this.tabGrid.Size = new System.Drawing.Size(1010, 366);
            this.tabGrid.TabIndex = 1;
            this.tabGrid.Text = "grid";
            this.tabGrid.UseVisualStyleBackColor = true;
            // 
            // lvList
            // 
            this.lvList.AllowColumnReorder = true;
            this.lvList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvList.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lvList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.id,
            this.period,
            this.gain,
            this.const1,
            this.expectedGain,
            this.endOfDay,
            this.cumulativeEndOfDay});
            this.lvList.Font = new System.Drawing.Font("Hermit", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvList.ForeColor = System.Drawing.Color.Lime;
            this.lvList.FullRowSelect = true;
            this.lvList.GridLines = true;
            this.lvList.HideSelection = false;
            this.lvList.LabelEdit = true;
            this.lvList.Location = new System.Drawing.Point(-1, 0);
            this.lvList.Name = "lvList";
            this.lvList.Size = new System.Drawing.Size(1011, 366);
            this.lvList.TabIndex = 9;
            this.lvList.UseCompatibleStateImageBehavior = false;
            this.lvList.View = System.Windows.Forms.View.Details;
            this.lvList.DoubleClick += new System.EventHandler(this.lvList_DoubleClick);
            // 
            // id
            // 
            this.id.Text = "id";
            this.id.Width = 0;
            // 
            // period
            // 
            this.period.Text = "period";
            this.period.Width = 160;
            // 
            // gain
            // 
            this.gain.Text = "gain";
            this.gain.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.gain.Width = 120;
            // 
            // const1
            // 
            this.const1.Text = "const";
            this.const1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.const1.Width = 120;
            // 
            // expectedGain
            // 
            this.expectedGain.Text = "expected-gain";
            this.expectedGain.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.expectedGain.Width = 167;
            // 
            // endOfDay
            // 
            this.endOfDay.Text = "end-of-day";
            this.endOfDay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.endOfDay.Width = 208;
            // 
            // cumulativeEndOfDay
            // 
            this.cumulativeEndOfDay.Text = "cumulative-end-of-day";
            this.cumulativeEndOfDay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cumulativeEndOfDay.Width = 182;
            // 
            // tabGraphich
            // 
            this.tabGraphich.Controls.Add(this.cbConst);
            this.tabGraphich.Controls.Add(this.chartPeriodGain);
            this.tabGraphich.Controls.Add(this.cbExceptedGain);
            this.tabGraphich.Controls.Add(this.cbGain);
            this.tabGraphich.Location = new System.Drawing.Point(4, 23);
            this.tabGraphich.Name = "tabGraphich";
            this.tabGraphich.Padding = new System.Windows.Forms.Padding(3);
            this.tabGraphich.Size = new System.Drawing.Size(927, 366);
            this.tabGraphich.TabIndex = 0;
            this.tabGraphich.Text = "graphic";
            this.tabGraphich.UseVisualStyleBackColor = true;
            // 
            // frmPeriodGain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1020, 392);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(393, 262);
            this.Name = "frmPeriodGain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "period-gain";
            this.Load += new System.EventHandler(this.frmStockAnalysis_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartPeriodGain)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabGrid.ResumeLayout(false);
            this.tabGraphich.ResumeLayout(false);
            this.tabGraphich.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataVisualization.Charting.Chart chartPeriodGain;
        private System.Windows.Forms.CheckBox cbGain;
        private System.Windows.Forms.CheckBox cbExceptedGain;
        private System.Windows.Forms.CheckBox cbConst;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabGraphich;
        private System.Windows.Forms.TabPage tabGrid;
        private System.Windows.Forms.ListView lvList;
        private System.Windows.Forms.ColumnHeader id;
        private System.Windows.Forms.ColumnHeader period;
        private System.Windows.Forms.ColumnHeader gain;
        private System.Windows.Forms.ColumnHeader const1;
        private System.Windows.Forms.ColumnHeader expectedGain;
        private System.Windows.Forms.ColumnHeader cumulativeEndOfDay;
        private System.Windows.Forms.ColumnHeader endOfDay;
    }
}