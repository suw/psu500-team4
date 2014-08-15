namespace nlp_test1
{
    partial class SentimentToDB
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
            this.txtBottomText = new System.Windows.Forms.TextBox();
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.btnLaunchHist = new System.Windows.Forms.Button();
            this.btnRunLive = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.SuspendLayout();
            // 
            // txtBottomText
            // 
            this.txtBottomText.Location = new System.Drawing.Point(12, 286);
            this.txtBottomText.Multiline = true;
            this.txtBottomText.Name = "txtBottomText";
            this.txtBottomText.Size = new System.Drawing.Size(574, 120);
            this.txtBottomText.TabIndex = 0;
            // 
            // dgvData
            // 
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvData.Location = new System.Drawing.Point(0, 0);
            this.dgvData.Name = "dgvData";
            this.dgvData.Size = new System.Drawing.Size(803, 258);
            this.dgvData.TabIndex = 1;
            // 
            // btnLaunchHist
            // 
            this.btnLaunchHist.Location = new System.Drawing.Point(648, 283);
            this.btnLaunchHist.Name = "btnLaunchHist";
            this.btnLaunchHist.Size = new System.Drawing.Size(121, 26);
            this.btnLaunchHist.TabIndex = 2;
            this.btnLaunchHist.Text = "Load Historical";
            this.btnLaunchHist.UseVisualStyleBackColor = true;
            this.btnLaunchHist.Click += new System.EventHandler(this.btnLaunchHist_Click);
            // 
            // btnRunLive
            // 
            this.btnRunLive.Location = new System.Drawing.Point(646, 328);
            this.btnRunLive.Name = "btnRunLive";
            this.btnRunLive.Size = new System.Drawing.Size(122, 25);
            this.btnRunLive.TabIndex = 3;
            this.btnRunLive.Text = "Run Live";
            this.btnRunLive.UseVisualStyleBackColor = true;
            // 
            // SentimentToDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 418);
            this.Controls.Add(this.btnRunLive);
            this.Controls.Add(this.btnLaunchHist);
            this.Controls.Add(this.dgvData);
            this.Controls.Add(this.txtBottomText);
            this.Name = "SentimentToDB";
            this.Text = "SentimentToDB";
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBottomText;
        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.Button btnLaunchHist;
        private System.Windows.Forms.Button btnRunLive;
    }
}