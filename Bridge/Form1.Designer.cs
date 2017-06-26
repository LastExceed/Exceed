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
            this.textBoxServerIP = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.buttonInfo = new System.Windows.Forms.Button();
            this.groupBoxAccount = new System.Windows.Forms.GroupBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.groupBoxServer = new System.Windows.Forms.GroupBox();
            this.numericUpDownPort = new System.Windows.Forms.NumericUpDown();
            this.labelPort = new System.Windows.Forms.Label();
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.groupBoxAccount.SuspendLayout();
            this.groupBoxServer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPort)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBoxChat
            // 
            this.richTextBoxChat.BackColor = System.Drawing.Color.Black;
            this.richTextBoxChat.Dock = System.Windows.Forms.DockStyle.Right;
            this.richTextBoxChat.ForeColor = System.Drawing.Color.White;
            this.richTextBoxChat.Location = new System.Drawing.Point(238, 0);
            this.richTextBoxChat.Name = "richTextBoxChat";
            this.richTextBoxChat.Size = new System.Drawing.Size(258, 218);
            this.richTextBoxChat.TabIndex = 1;
            this.richTextBoxChat.Text = "richTextBoxChat";
            // 
            // listBoxPlayers
            // 
            this.listBoxPlayers.Dock = System.Windows.Forms.DockStyle.Right;
            this.listBoxPlayers.FormattingEnabled = true;
            this.listBoxPlayers.Location = new System.Drawing.Point(118, 0);
            this.listBoxPlayers.Name = "listBoxPlayers";
            this.listBoxPlayers.Size = new System.Drawing.Size(120, 218);
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
            // textBoxServerIP
            // 
            this.textBoxServerIP.Location = new System.Drawing.Point(6, 18);
            this.textBoxServerIP.Name = "textBoxServerIP";
            this.textBoxServerIP.Size = new System.Drawing.Size(91, 20);
            this.textBoxServerIP.TabIndex = 4;
            this.textBoxServerIP.Text = "exceed.rocks";
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
            this.buttonInfo.Location = new System.Drawing.Point(69, 165);
            this.buttonInfo.Name = "buttonInfo";
            this.buttonInfo.Size = new System.Drawing.Size(46, 23);
            this.buttonInfo.TabIndex = 6;
            this.buttonInfo.Text = "info";
            this.buttonInfo.UseVisualStyleBackColor = true;
            // 
            // groupBoxAccount
            // 
            this.groupBoxAccount.BackColor = System.Drawing.SystemColors.Control;
            this.groupBoxAccount.Controls.Add(this.linkLabel1);
            this.groupBoxAccount.Controls.Add(this.textBox3);
            this.groupBoxAccount.Controls.Add(this.textBox1);
            this.groupBoxAccount.Location = new System.Drawing.Point(-1, 79);
            this.groupBoxAccount.Name = "groupBoxAccount";
            this.groupBoxAccount.Size = new System.Drawing.Size(113, 86);
            this.groupBoxAccount.TabIndex = 7;
            this.groupBoxAccount.TabStop = false;
            this.groupBoxAccount.Text = "account";
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(1, 165);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(68, 23);
            this.buttonConnect.TabIndex = 7;
            this.buttonConnect.Text = "connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(13, 67);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(41, 13);
            this.linkLabel1.TabIndex = 6;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "register";
            // 
            // groupBoxServer
            // 
            this.groupBoxServer.Controls.Add(this.labelPort);
            this.groupBoxServer.Controls.Add(this.numericUpDownPort);
            this.groupBoxServer.Controls.Add(this.textBoxServerIP);
            this.groupBoxServer.Location = new System.Drawing.Point(6, 0);
            this.groupBoxServer.Name = "groupBoxServer";
            this.groupBoxServer.Size = new System.Drawing.Size(106, 73);
            this.groupBoxServer.TabIndex = 8;
            this.groupBoxServer.TabStop = false;
            this.groupBoxServer.Text = "Server";
            // 
            // numericUpDownPort
            // 
            this.numericUpDownPort.Location = new System.Drawing.Point(40, 41);
            this.numericUpDownPort.Maximum = new decimal(new int[] {
            49152,
            0,
            0,
            0});
            this.numericUpDownPort.Minimum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numericUpDownPort.Name = "numericUpDownPort";
            this.numericUpDownPort.Size = new System.Drawing.Size(57, 20);
            this.numericUpDownPort.TabIndex = 5;
            this.numericUpDownPort.Value = new decimal(new int[] {
            12345,
            0,
            0,
            0});
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.Location = new System.Drawing.Point(11, 44);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(28, 13);
            this.labelPort.TabIndex = 6;
            this.labelPort.Text = "port:";
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.Enabled = false;
            this.buttonDisconnect.Location = new System.Drawing.Point(1, 189);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(68, 23);
            this.buttonDisconnect.TabIndex = 9;
            this.buttonDisconnect.Text = "disconnect";
            this.buttonDisconnect.UseVisualStyleBackColor = true;
            this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 218);
            this.Controls.Add(this.buttonDisconnect);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.groupBoxServer);
            this.Controls.Add(this.groupBoxAccount);
            this.Controls.Add(this.buttonInfo);
            this.Controls.Add(this.listBoxPlayers);
            this.Controls.Add(this.richTextBoxChat);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(512, 9999);
            this.MinimumSize = new System.Drawing.Size(512, 256);
            this.Name = "Form1";
            this.Text = "fd";
            this.groupBoxAccount.ResumeLayout(false);
            this.groupBoxAccount.PerformLayout();
            this.groupBoxServer.ResumeLayout(false);
            this.groupBoxServer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPort)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxChat;
        private System.Windows.Forms.ListBox listBoxPlayers;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBoxServerIP;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button buttonInfo;
        private System.Windows.Forms.GroupBox groupBoxAccount;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.GroupBox groupBoxServer;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.NumericUpDown numericUpDownPort;
        private System.Windows.Forms.Button buttonDisconnect;
    }
}

