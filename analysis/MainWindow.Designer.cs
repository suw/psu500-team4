namespace nlp_test1
{
    partial class MainWindow
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
            this.btnHistSent = new System.Windows.Forms.Button();
            this.btnLive = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnHistSent
            // 
            this.btnHistSent.Location = new System.Drawing.Point(31, 21);
            this.btnHistSent.Name = "btnHistSent";
            this.btnHistSent.Size = new System.Drawing.Size(91, 59);
            this.btnHistSent.TabIndex = 0;
            this.btnHistSent.Text = "Load Sent";
            this.btnHistSent.UseVisualStyleBackColor = true;
            this.btnHistSent.Click += new System.EventHandler(this.btnHistSent_Click);
            // 
            // btnLive
            // 
            this.btnLive.Location = new System.Drawing.Point(170, 21);
            this.btnLive.Name = "btnLive";
            this.btnLive.Size = new System.Drawing.Size(94, 58);
            this.btnLive.TabIndex = 1;
            this.btnLive.Text = "Run Live";
            this.btnLive.UseVisualStyleBackColor = true;
            this.btnLive.Click += new System.EventHandler(this.btnLive_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 100);
            this.Controls.Add(this.btnLive);
            this.Controls.Add(this.btnHistSent);
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnHistSent;
        private System.Windows.Forms.Button btnLive;
    }
}