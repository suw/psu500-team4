namespace nlp_test1
{
    partial class Historical
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.lblEnd = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.lblApiLmt = new System.Windows.Forms.Label();
            this.txtApiLmtIn = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblBottom1L = new System.Windows.Forms.Label();
            this.lblBottom1R = new System.Windows.Forms.Label();
            this.lblTesting = new System.Windows.Forms.Label();
            this.lblPercent = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.btnLive = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 264);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(879, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(879, 264);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 28);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(873, 233);
            this.dataGridView1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Start:";
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStartDate.Location = new System.Drawing.Point(72, 4);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(89, 20);
            this.dtpStartDate.TabIndex = 3;
            this.dtpStartDate.Value = new System.DateTime(2014, 7, 7, 0, 0, 0, 0);
            // 
            // lblEnd
            // 
            this.lblEnd.AutoSize = true;
            this.lblEnd.Location = new System.Drawing.Point(183, 7);
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size(29, 13);
            this.lblEnd.TabIndex = 4;
            this.lblEnd.Text = "End:";
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndDate.Location = new System.Drawing.Point(215, 3);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(91, 20);
            this.dtpEndDate.TabIndex = 5;
            // 
            // lblApiLmt
            // 
            this.lblApiLmt.AutoSize = true;
            this.lblApiLmt.Location = new System.Drawing.Point(335, 8);
            this.lblApiLmt.Name = "lblApiLmt";
            this.lblApiLmt.Size = new System.Drawing.Size(47, 13);
            this.lblApiLmt.TabIndex = 6;
            this.lblApiLmt.Text = "API limit:";
            // 
            // txtApiLmtIn
            // 
            this.txtApiLmtIn.Location = new System.Drawing.Point(383, 4);
            this.txtApiLmtIn.Name = "txtApiLmtIn";
            this.txtApiLmtIn.Size = new System.Drawing.Size(63, 20);
            this.txtApiLmtIn.TabIndex = 7;
            this.txtApiLmtIn.Text = "10000";
            this.txtApiLmtIn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(466, 3);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(59, 22);
            this.btnStart.TabIndex = 8;
            this.btnStart.Text = "START";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblBottom1L
            // 
            this.lblBottom1L.AutoSize = true;
            this.lblBottom1L.Location = new System.Drawing.Point(597, 7);
            this.lblBottom1L.Name = "lblBottom1L";
            this.lblBottom1L.Size = new System.Drawing.Size(0, 13);
            this.lblBottom1L.TabIndex = 9;
            // 
            // lblBottom1R
            // 
            this.lblBottom1R.AutoSize = true;
            this.lblBottom1R.Location = new System.Drawing.Point(642, 10);
            this.lblBottom1R.Name = "lblBottom1R";
            this.lblBottom1R.Size = new System.Drawing.Size(0, 13);
            this.lblBottom1R.TabIndex = 10;
            // 
            // lblTesting
            // 
            this.lblTesting.AutoSize = true;
            this.lblTesting.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTesting.ForeColor = System.Drawing.Color.Red;
            this.lblTesting.Location = new System.Drawing.Point(4, 5);
            this.lblTesting.Name = "lblTesting";
            this.lblTesting.Size = new System.Drawing.Size(0, 13);
            this.lblTesting.TabIndex = 11;
            // 
            // lblPercent
            // 
            this.lblPercent.AutoSize = true;
            this.lblPercent.Location = new System.Drawing.Point(733, 7);
            this.lblPercent.Name = "lblPercent";
            this.lblPercent.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblPercent.Size = new System.Drawing.Size(15, 13);
            this.lblPercent.TabIndex = 12;
            this.lblPercent.Text = "%";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(772, 6);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(0, 13);
            this.lblDate.TabIndex = 13;
            // 
            // btnLive
            // 
            this.btnLive.BackColor = System.Drawing.Color.Maroon;
            this.btnLive.Location = new System.Drawing.Point(540, 2);
            this.btnLive.Name = "btnLive";
            this.btnLive.Size = new System.Drawing.Size(51, 23);
            this.btnLive.TabIndex = 14;
            this.btnLive.Text = "LIVE";
            this.btnLive.UseVisualStyleBackColor = false;
            this.btnLive.Click += new System.EventHandler(this.btnLive_Click);
            // 
            // Historical
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 286);
            this.Controls.Add(this.btnLive);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.lblPercent);
            this.Controls.Add(this.lblTesting);
            this.Controls.Add(this.lblBottom1R);
            this.Controls.Add(this.lblBottom1L);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtApiLmtIn);
            this.Controls.Add(this.lblApiLmt);
            this.Controls.Add(this.dtpEndDate);
            this.Controls.Add(this.lblEnd);
            this.Controls.Add(this.dtpStartDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "Historical";
            this.Text = "Historical";
            this.Load += new System.EventHandler(this.Historical_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.Label lblEnd;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Label lblApiLmt;
        private System.Windows.Forms.TextBox txtApiLmtIn;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblBottom1L;
        private System.Windows.Forms.Label lblBottom1R;
        private System.Windows.Forms.Label lblTesting;
        private System.Windows.Forms.Label lblPercent;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Button btnLive;
    }
}