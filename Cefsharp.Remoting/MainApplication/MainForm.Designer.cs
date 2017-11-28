namespace MainApplication {
    partial class MainForm {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent() {
            this.ShowBrowserDlgBtn = new System.Windows.Forms.Button();
            this.ShowBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ShowBrowserDlgBtn
            // 
            this.ShowBrowserDlgBtn.Location = new System.Drawing.Point(12, 58);
            this.ShowBrowserDlgBtn.Name = "ShowBrowserDlgBtn";
            this.ShowBrowserDlgBtn.Size = new System.Drawing.Size(134, 72);
            this.ShowBrowserDlgBtn.TabIndex = 3;
            this.ShowBrowserDlgBtn.Text = "Show web browser as a dialog";
            this.ShowBrowserDlgBtn.UseVisualStyleBackColor = true;
            this.ShowBrowserDlgBtn.Click += new System.EventHandler(this.ShowBrowserDlgBtn_Click);
            // 
            // ShowBtn
            // 
            this.ShowBtn.Location = new System.Drawing.Point(12, 12);
            this.ShowBtn.Name = "ShowBtn";
            this.ShowBtn.Size = new System.Drawing.Size(134, 40);
            this.ShowBtn.TabIndex = 2;
            this.ShowBtn.Text = "Show web browser";
            this.ShowBtn.UseVisualStyleBackColor = true;
            this.ShowBtn.Click += new System.EventHandler(this.ShowBtn_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.ShowBrowserDlgBtn);
            this.Controls.Add(this.ShowBtn);
            this.Name = "MainForm";
            this.Text = "Main form";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ShowBrowserDlgBtn;
        private System.Windows.Forms.Button ShowBtn;
    }
}

