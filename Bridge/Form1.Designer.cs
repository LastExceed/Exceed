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
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.linkLabelReset = new System.Windows.Forms.LinkLabel();
            this.buttonLogout = new System.Windows.Forms.Button();
            this.buttonLogin = new System.Windows.Forms.Button();
            this.buttonRegister = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonDestroy = new System.Windows.Forms.RadioButton();
            this.radioButtonDuplicate = new System.Windows.Forms.RadioButton();
            this.radioButtonNormal = new System.Windows.Forms.RadioButton();
            this.listBoxPlayers = new System.Windows.Forms.ListBox();
            this.richTextBoxChat = new System.Windows.Forms.RichTextBox();
            this.buttonEditor = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.radioButtonRankingsClan = new System.Windows.Forms.RadioButton();
            this.radioButtonRankingsUsers = new System.Windows.Forms.RadioButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.buttonChangeEmail = new System.Windows.Forms.Button();
            this.buttonChangePassword = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(1, 43);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(117, 20);
            this.textBoxPassword.TabIndex = 1;
            this.textBoxPassword.Text = "apple";
            this.textBoxPassword.UseSystemPasswordChar = true;
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Location = new System.Drawing.Point(1, 22);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(117, 20);
            this.textBoxUsername.TabIndex = 0;
            this.textBoxUsername.Text = "public_test_id";
            // 
            // linkLabelReset
            // 
            this.linkLabelReset.AutoSize = true;
            this.linkLabelReset.Location = new System.Drawing.Point(0, 107);
            this.linkLabelReset.Name = "linkLabelReset";
            this.linkLabelReset.Size = new System.Drawing.Size(78, 13);
            this.linkLabelReset.TabIndex = 25;
            this.linkLabelReset.TabStop = true;
            this.linkLabelReset.Text = "reset password";
            // 
            // buttonLogout
            // 
            this.buttonLogout.Enabled = false;
            this.buttonLogout.Location = new System.Drawing.Point(59, 63);
            this.buttonLogout.Name = "buttonLogout";
            this.buttonLogout.Size = new System.Drawing.Size(60, 23);
            this.buttonLogout.TabIndex = 25;
            this.buttonLogout.Text = "Logout";
            this.buttonLogout.UseVisualStyleBackColor = true;
            this.buttonLogout.Click += new System.EventHandler(this.buttonLogout_Click);
            // 
            // buttonLogin
            // 
            this.buttonLogin.Enabled = false;
            this.buttonLogin.Location = new System.Drawing.Point(0, 63);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(60, 23);
            this.buttonLogin.TabIndex = 24;
            this.buttonLogin.Text = "Login";
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.ButtonLogin_Click);
            // 
            // buttonRegister
            // 
            this.buttonRegister.Location = new System.Drawing.Point(0, 85);
            this.buttonRegister.Name = "buttonRegister";
            this.buttonRegister.Size = new System.Drawing.Size(119, 23);
            this.buttonRegister.TabIndex = 22;
            this.buttonRegister.Text = "Register new account";
            this.buttonRegister.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonDestroy);
            this.groupBox1.Controls.Add(this.radioButtonDuplicate);
            this.groupBox1.Controls.Add(this.radioButtonNormal);
            this.groupBox1.Location = new System.Drawing.Point(70, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(75, 62);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "dropmode";
            // 
            // radioButtonDestroy
            // 
            this.radioButtonDestroy.AutoSize = true;
            this.radioButtonDestroy.Location = new System.Drawing.Point(3, 43);
            this.radioButtonDestroy.Name = "radioButtonDestroy";
            this.radioButtonDestroy.Size = new System.Drawing.Size(59, 17);
            this.radioButtonDestroy.TabIndex = 2;
            this.radioButtonDestroy.Text = "destroy";
            this.radioButtonDestroy.UseVisualStyleBackColor = true;
            // 
            // radioButtonDuplicate
            // 
            this.radioButtonDuplicate.AutoSize = true;
            this.radioButtonDuplicate.Location = new System.Drawing.Point(3, 28);
            this.radioButtonDuplicate.Name = "radioButtonDuplicate";
            this.radioButtonDuplicate.Size = new System.Drawing.Size(68, 17);
            this.radioButtonDuplicate.TabIndex = 1;
            this.radioButtonDuplicate.Text = "duplicate";
            this.radioButtonDuplicate.UseVisualStyleBackColor = true;
            // 
            // radioButtonNormal
            // 
            this.radioButtonNormal.AutoSize = true;
            this.radioButtonNormal.Checked = true;
            this.radioButtonNormal.Location = new System.Drawing.Point(3, 13);
            this.radioButtonNormal.Name = "radioButtonNormal";
            this.radioButtonNormal.Size = new System.Drawing.Size(56, 17);
            this.radioButtonNormal.TabIndex = 0;
            this.radioButtonNormal.TabStop = true;
            this.radioButtonNormal.Text = "normal";
            this.radioButtonNormal.UseVisualStyleBackColor = true;
            // 
            // listBoxPlayers
            // 
            this.listBoxPlayers.FormattingEnabled = true;
            this.listBoxPlayers.Location = new System.Drawing.Point(1, 125);
            this.listBoxPlayers.Name = "listBoxPlayers";
            this.listBoxPlayers.Size = new System.Drawing.Size(117, 108);
            this.listBoxPlayers.TabIndex = 4;
            // 
            // richTextBoxChat
            // 
            this.richTextBoxChat.BackColor = System.Drawing.Color.Black;
            this.richTextBoxChat.Dock = System.Windows.Forms.DockStyle.Right;
            this.richTextBoxChat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxChat.ForeColor = System.Drawing.SystemColors.Window;
            this.richTextBoxChat.Location = new System.Drawing.Point(333, 0);
            this.richTextBoxChat.Name = "richTextBoxChat";
            this.richTextBoxChat.ReadOnly = true;
            this.richTextBoxChat.Size = new System.Drawing.Size(255, 257);
            this.richTextBoxChat.TabIndex = 18;
            this.richTextBoxChat.Text = "";
            // 
            // buttonEditor
            // 
            this.buttonEditor.Location = new System.Drawing.Point(0, 233);
            this.buttonEditor.Name = "buttonEditor";
            this.buttonEditor.Size = new System.Drawing.Size(119, 23);
            this.buttonEditor.TabIndex = 22;
            this.buttonEditor.Text = "Open item editor";
            this.buttonEditor.UseVisualStyleBackColor = true;
            this.buttonEditor.Click += new System.EventHandler(this.ButtonEditor_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(119, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(213, 256);
            this.tabControl1.TabIndex = 23;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.radioButtonRankingsClan);
            this.tabPage1.Controls.Add(this.radioButtonRankingsUsers);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(205, 230);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Rankings";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // radioButtonRankingsClan
            // 
            this.radioButtonRankingsClan.AutoSize = true;
            this.radioButtonRankingsClan.Location = new System.Drawing.Point(6, 26);
            this.radioButtonRankingsClan.Name = "radioButtonRankingsClan";
            this.radioButtonRankingsClan.Size = new System.Drawing.Size(51, 17);
            this.radioButtonRankingsClan.TabIndex = 1;
            this.radioButtonRankingsClan.TabStop = true;
            this.radioButtonRankingsClan.Text = "Clans";
            this.radioButtonRankingsClan.UseVisualStyleBackColor = true;
            // 
            // radioButtonRankingsUsers
            // 
            this.radioButtonRankingsUsers.AutoSize = true;
            this.radioButtonRankingsUsers.Location = new System.Drawing.Point(5, 6);
            this.radioButtonRankingsUsers.Name = "radioButtonRankingsUsers";
            this.radioButtonRankingsUsers.Size = new System.Drawing.Size(52, 17);
            this.radioButtonRankingsUsers.TabIndex = 0;
            this.radioButtonRankingsUsers.TabStop = true;
            this.radioButtonRankingsUsers.Text = "Users";
            this.radioButtonRankingsUsers.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.buttonChangeEmail);
            this.tabPage2.Controls.Add(this.buttonChangePassword);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(205, 230);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Account";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // buttonChangeEmail
            // 
            this.buttonChangeEmail.Location = new System.Drawing.Point(24, 159);
            this.buttonChangeEmail.Name = "buttonChangeEmail";
            this.buttonChangeEmail.Size = new System.Drawing.Size(102, 23);
            this.buttonChangeEmail.TabIndex = 1;
            this.buttonChangeEmail.Text = "change email";
            this.buttonChangeEmail.UseVisualStyleBackColor = true;
            // 
            // buttonChangePassword
            // 
            this.buttonChangePassword.Location = new System.Drawing.Point(22, 188);
            this.buttonChangePassword.Name = "buttonChangePassword";
            this.buttonChangePassword.Size = new System.Drawing.Size(104, 23);
            this.buttonChangePassword.TabIndex = 0;
            this.buttonChangePassword.Text = "change password";
            this.buttonChangePassword.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(205, 230);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Clans";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(34, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 18);
            this.label1.TabIndex = 26;
            this.label1.Text = "Login:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.ClientSize = new System.Drawing.Size(588, 257);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.linkLabelReset);
            this.Controls.Add(this.buttonEditor);
            this.Controls.Add(this.buttonLogout);
            this.Controls.Add(this.textBoxUsername);
            this.Controls.Add(this.buttonLogin);
            this.Controls.Add(this.richTextBoxChat);
            this.Controls.Add(this.buttonRegister);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.listBoxPlayers);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 249);
            this.Name = "Form1";
            this.Text = "Exceed Bridge";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.TextBox textBoxPassword;
        public System.Windows.Forms.TextBox textBoxUsername;
        public System.Windows.Forms.ListBox listBoxPlayers;
        public System.Windows.Forms.RichTextBox richTextBoxChat;
        public System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.RadioButton radioButtonDestroy;
        public System.Windows.Forms.RadioButton radioButtonDuplicate;
        public System.Windows.Forms.RadioButton radioButtonNormal;
        private System.Windows.Forms.Button buttonRegister;
        private System.Windows.Forms.LinkLabel linkLabelReset;
        private System.Windows.Forms.Button buttonEditor;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.RadioButton radioButtonRankingsClan;
        private System.Windows.Forms.RadioButton radioButtonRankingsUsers;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button buttonChangeEmail;
        private System.Windows.Forms.Button buttonChangePassword;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Button buttonLogin;
        public System.Windows.Forms.Button buttonLogout;
    }
}

