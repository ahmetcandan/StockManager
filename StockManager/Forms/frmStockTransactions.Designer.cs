namespace StockManager
{
    partial class frmStockTransactions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStockTransactions));
            this.lblInformations = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lvList = new System.Windows.Forms.ListView();
            this.StockTransactionId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.UnitPrice = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Amount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Total = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Const = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TransactionType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Date = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lblInformations
            // 
            this.lblInformations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInformations.AutoSize = true;
            this.lblInformations.BackColor = System.Drawing.Color.DarkSlateGray;
            this.lblInformations.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblInformations.Font = new System.Drawing.Font("Hermit", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInformations.ForeColor = System.Drawing.Color.PaleGreen;
            this.lblInformations.Location = new System.Drawing.Point(1, 339);
            this.lblInformations.Name = "lblInformations";
            this.lblInformations.Size = new System.Drawing.Size(110, 22);
            this.lblInformations.TabIndex = 10;
            this.lblInformations.Text = "information";
            this.lblInformations.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.DarkSlateGray;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Hermit", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.PaleGreen;
            this.label1.Location = new System.Drawing.Point(1, 339);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(930, 25);
            this.label1.TabIndex = 9;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lvList
            // 
            this.lvList.AllowColumnReorder = true;
            this.lvList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvList.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lvList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.StockTransactionId,
            this.UnitPrice,
            this.Amount,
            this.Total,
            this.Const,
            this.TransactionType,
            this.Date});
            this.lvList.Font = new System.Drawing.Font("Hermit", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvList.ForeColor = System.Drawing.Color.Lime;
            this.lvList.FullRowSelect = true;
            this.lvList.GridLines = true;
            this.lvList.HideSelection = false;
            this.lvList.LabelEdit = true;
            this.lvList.Location = new System.Drawing.Point(1, -3);
            this.lvList.Name = "lvList";
            this.lvList.Size = new System.Drawing.Size(932, 340);
            this.lvList.TabIndex = 8;
            this.lvList.UseCompatibleStateImageBehavior = false;
            this.lvList.View = System.Windows.Forms.View.Details;
            // 
            // StockTransactionId
            // 
            this.StockTransactionId.Width = 0;
            // 
            // UnitPrice
            // 
            this.UnitPrice.Text = "stock-code";
            this.UnitPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.UnitPrice.Width = 155;
            // 
            // Amount
            // 
            this.Amount.Text = "status";
            this.Amount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Amount.Width = 151;
            // 
            // Total
            // 
            this.Total.Text = "total";
            this.Total.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Total.Width = 138;
            // 
            // Const
            // 
            this.Const.Text = "total-amount";
            this.Const.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Const.Width = 150;
            // 
            // TransactionType
            // 
            this.TransactionType.Text = "buy-price";
            this.TransactionType.Width = 150;
            // 
            // Date
            // 
            this.Date.Text = "date";
            this.Date.Width = 135;
            // 
            // frmStockTransactions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(931, 364);
            this.Controls.Add(this.lblInformations);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lvList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(709, 286);
            this.Name = "frmStockTransactions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "stock-transactions";
            this.Load += new System.EventHandler(this.frmStockTransactions_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblInformations;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lvList;
        private System.Windows.Forms.ColumnHeader Const;
        private System.Windows.Forms.ColumnHeader TransactionType;
        private System.Windows.Forms.ColumnHeader Amount;
        private System.Windows.Forms.ColumnHeader UnitPrice;
        private System.Windows.Forms.ColumnHeader Date;
        private System.Windows.Forms.ColumnHeader StockTransactionId;
        private System.Windows.Forms.ColumnHeader Total;
    }
}