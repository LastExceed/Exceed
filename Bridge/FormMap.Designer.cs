namespace Bridge {
    partial class FormMap {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.Refreshtimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // Refreshtimer
            // 
            this.Refreshtimer.Enabled = true;
            this.Refreshtimer.Interval = 500;
            this.Refreshtimer.Tick += new System.EventHandler(this.Refreshtimer_Tick);
            // 
            // FormMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 474);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormMap";
            this.Text = "FormMap";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer Refreshtimer;
    }
}