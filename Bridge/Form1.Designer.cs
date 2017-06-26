namespace Bridge
{
    partial class Form1
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
            this.richTextBoxChat = new System.Windows.Forms.RichTextBox();
            this.listBoxPlayers = new System.Windows.Forms.ListBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.buttonInfo = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonLogin = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.labelIP = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBoxChat
            // 
            this.richTextBoxChat.BackColor = System.Drawing.Color.Black;
            this.richTextBoxChat.Dock = System.Windows.Forms.DockStyle.Right;
            this.richTextBoxChat.ForeColor = System.Drawing.Color.White;
            this.richTextBoxChat.Location = new System.Drawing.Point(238, 0);
            this.richTextBoxChat.Name = "richTextBoxChat";
            this.richTextBoxChat.Size = new System.Drawing.Size(264, 164);
            this.richTextBoxChat.TabIndex = 1;
            this.richTextBoxChat.Text = "richTextBoxChat";
            // 
            // listBoxPlayers
            // 
            this.listBoxPlayers.Dock = System.Windows.Forms.DockStyle.Right;
            this.listBoxPlayers.FormattingEnabled = true;
            this.listBoxPlayers.Location = new System.Drawing.Point(118, 0);
            this.listBoxPlayers.Name = "listBoxPlayers";
            this.listBoxPlayers.Size = new System.Drawing.Size(120, 164);
            this.listBoxPlayers.TabIndex = 2;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(6, 44);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "asdfasdf";
            this.textBox1.UseSystemPasswordChar = true;
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(7, 16);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(100, 20);
            this.textBoxIP.TabIndex = 4;
            this.textBoxIP.Text = "exceed.rocks";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(6, 18);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 20);
            this.textBox3.TabIndex = 5;
            this.textBox3.Text = "BLACKROCK";
            // 
            // buttonInfo
            // 
            this.buttonInfo.Location = new System.Drawing.Point(18, 136);
            this.buttonInfo.Name = "buttonInfo";
            this.buttonInfo.Size = new System.Drawing.Size(75, 23);
            this.buttonInfo.TabIndex = 6;
            this.buttonInfo.Text = "info";
            this.buttonInfo.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.buttonLogin);
            this.groupBox1.Controls.Add(this.linkLabel1);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Location = new System.Drawing.Point(1, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(113, 94);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "account";
            // 
            // buttonLogin
            // 
            this.buttonLogin.Location = new System.Drawing.Point(44, 66);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(63, 23);
            this.buttonLogin.TabIndex = 7;
            this.buttonLogin.Text = "login";
            this.buttonLogin.UseVisualStyleBackColor = true;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(4, 70);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(41, 13);
            this.linkLabel1.TabIndex = 6;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "register";
            // 
            // labelIP
            // 
            this.labelIP.AutoSize = true;
            this.labelIP.Location = new System.Drawing.Point(7, 0);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(52, 13);
            this.labelIP.TabIndex = 8;
            this.labelIP.Text = "server IP:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 164);
            this.Controls.Add(this.labelIP);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonInfo);
            this.Controls.Add(this.textBoxIP);
            this.Controls.Add(this.listBoxPlayers);
            this.Controls.Add(this.richTextBoxChat);
            this.Name = "Form1";
            this.Text = "fd";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxChat;
        private System.Windows.Forms.ListBox listBoxPlayers;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button buttonInfo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonLogin;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label labelIP;
    }
}

