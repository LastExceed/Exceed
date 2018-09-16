namespace Bridge
{
    partial class FormMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonDestroy = new System.Windows.Forms.RadioButton();
            this.radioButtonDuplicate = new System.Windows.Forms.RadioButton();
            this.radioButtonReturn = new System.Windows.Forms.RadioButton();
            this.listBoxPlayers = new System.Windows.Forms.ListBox();
            this.buttonEditor = new System.Windows.Forms.Button();
            this.checkBoxZoomHack = new System.Windows.Forms.CheckBox();
            this.buttonMap = new System.Windows.Forms.Button();
            this.timerSearchProcess = new System.Windows.Forms.Timer(this.components);
            this.buttonChat = new System.Windows.Forms.Button();
            this.buttonRankings = new System.Windows.Forms.Button();
            this.labelUser = new System.Windows.Forms.Label();
            this.labelClan = new System.Windows.Forms.Label();
            this.buttonClan = new System.Windows.Forms.Button();
            this.linkLabelUser = new System.Windows.Forms.LinkLabel();
            this.linkLabelClan = new System.Windows.Forms.LinkLabel();
            this.buttonLoginRegister = new System.Windows.Forms.Button();
            this.contextMenuStripUser = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemLogout = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.contextMenuStripUser.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonDestroy);
            this.groupBox1.Controls.Add(this.radioButtonDuplicate);
            this.groupBox1.Controls.Add(this.radioButtonReturn);
            this.groupBox1.Location = new System.Drawing.Point(116, 4);
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
            this.radioButtonDestroy.Size = new System.Drawing.Size(61, 17);
            this.radioButtonDestroy.TabIndex = 2;
            this.radioButtonDestroy.Text = "Destroy";
            this.radioButtonDestroy.UseVisualStyleBackColor = true;
            // 
            // radioButtonDuplicate
            // 
            this.radioButtonDuplicate.AutoSize = true;
            this.radioButtonDuplicate.Location = new System.Drawing.Point(3, 28);
            this.radioButtonDuplicate.Name = "radioButtonDuplicate";
            this.radioButtonDuplicate.Size = new System.Drawing.Size(70, 17);
            this.radioButtonDuplicate.TabIndex = 1;
            this.radioButtonDuplicate.Text = "Duplicate";
            this.radioButtonDuplicate.UseVisualStyleBackColor = true;
            // 
            // radioButtonReturn
            // 
            this.radioButtonReturn.AutoSize = true;
            this.radioButtonReturn.Checked = true;
            this.radioButtonReturn.Location = new System.Drawing.Point(3, 13);
            this.radioButtonReturn.Name = "radioButtonReturn";
            this.radioButtonReturn.Size = new System.Drawing.Size(57, 17);
            this.radioButtonReturn.TabIndex = 0;
            this.radioButtonReturn.TabStop = true;
            this.radioButtonReturn.Text = "Return";
            this.radioButtonReturn.UseVisualStyleBackColor = true;
            // 
            // listBoxPlayers
            // 
            this.listBoxPlayers.FormattingEnabled = true;
            this.listBoxPlayers.Location = new System.Drawing.Point(1, 45);
            this.listBoxPlayers.Name = "listBoxPlayers";
            this.listBoxPlayers.Size = new System.Drawing.Size(111, 134);
            this.listBoxPlayers.TabIndex = 4;
            // 
            // buttonEditor
            // 
            this.buttonEditor.Enabled = false;
            this.buttonEditor.Location = new System.Drawing.Point(112, 157);
            this.buttonEditor.Name = "buttonEditor";
            this.buttonEditor.Size = new System.Drawing.Size(90, 23);
            this.buttonEditor.TabIndex = 6;
            this.buttonEditor.Text = "Character editor";
            this.buttonEditor.UseVisualStyleBackColor = true;
            this.buttonEditor.Click += new System.EventHandler(this.ButtonEditor_Click);
            // 
            // checkBoxZoomHack
            // 
            this.checkBoxZoomHack.AutoSize = true;
            this.checkBoxZoomHack.Enabled = false;
            this.checkBoxZoomHack.Location = new System.Drawing.Point(118, 72);
            this.checkBoxZoomHack.Name = "checkBoxZoomHack";
            this.checkBoxZoomHack.Size = new System.Drawing.Size(80, 17);
            this.checkBoxZoomHack.TabIndex = 20;
            this.checkBoxZoomHack.Text = "Zoom hack";
            this.checkBoxZoomHack.UseVisualStyleBackColor = true;
            this.checkBoxZoomHack.CheckedChanged += new System.EventHandler(this.checkBoxZoomHack_CheckedChanged);
            // 
            // buttonMap
            // 
            this.buttonMap.Location = new System.Drawing.Point(112, 135);
            this.buttonMap.Name = "buttonMap";
            this.buttonMap.Size = new System.Drawing.Size(90, 23);
            this.buttonMap.TabIndex = 5;
            this.buttonMap.Text = "Live map";
            this.buttonMap.UseVisualStyleBackColor = true;
            this.buttonMap.Click += new System.EventHandler(this.ButtonMap_Click);
            // 
            // timerSearchProcess
            // 
            this.timerSearchProcess.Enabled = true;
            this.timerSearchProcess.Interval = 200;
            this.timerSearchProcess.Tick += new System.EventHandler(this.timerSearchProcess_Tick);
            // 
            // buttonChat
            // 
            this.buttonChat.Location = new System.Drawing.Point(112, 91);
            this.buttonChat.Name = "buttonChat";
            this.buttonChat.Size = new System.Drawing.Size(90, 23);
            this.buttonChat.TabIndex = 27;
            this.buttonChat.Text = "Ingame chat";
            this.buttonChat.UseVisualStyleBackColor = true;
            this.buttonChat.Click += new System.EventHandler(this.buttonChat_Click);
            // 
            // buttonRankings
            // 
            this.buttonRankings.Location = new System.Drawing.Point(112, 113);
            this.buttonRankings.Name = "buttonRankings";
            this.buttonRankings.Size = new System.Drawing.Size(90, 23);
            this.buttonRankings.TabIndex = 28;
            this.buttonRankings.Text = "Rankings";
            this.buttonRankings.UseVisualStyleBackColor = true;
            this.buttonRankings.Click += new System.EventHandler(this.buttonRankings_Click);
            // 
            // labelUser
            // 
            this.labelUser.AutoSize = true;
            this.labelUser.Location = new System.Drawing.Point(-2, 4);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(32, 13);
            this.labelUser.TabIndex = 29;
            this.labelUser.Text = "User:";
            // 
            // labelClan
            // 
            this.labelClan.AutoSize = true;
            this.labelClan.Location = new System.Drawing.Point(-2, 26);
            this.labelClan.Name = "labelClan";
            this.labelClan.Size = new System.Drawing.Size(31, 13);
            this.labelClan.TabIndex = 30;
            this.labelClan.Text = "Clan:";
            // 
            // buttonClan
            // 
            this.buttonClan.Enabled = false;
            this.buttonClan.Location = new System.Drawing.Point(28, 22);
            this.buttonClan.Name = "buttonClan";
            this.buttonClan.Size = new System.Drawing.Size(85, 23);
            this.buttonClan.TabIndex = 31;
            this.buttonClan.Text = "Create";
            this.buttonClan.UseVisualStyleBackColor = true;
            this.buttonClan.Click += new System.EventHandler(this.buttonClan_Click);
            // 
            // linkLabelUser
            // 
            this.linkLabelUser.AutoSize = true;
            this.linkLabelUser.Location = new System.Drawing.Point(26, 5);
            this.linkLabelUser.Name = "linkLabelUser";
            this.linkLabelUser.Size = new System.Drawing.Size(71, 13);
            this.linkLabelUser.TabIndex = 32;
            this.linkLabelUser.TabStop = true;
            this.linkLabelUser.Text = "linkLabelUser";
            this.linkLabelUser.Visible = false;
            this.linkLabelUser.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelUser_LinkClicked);
            // 
            // linkLabelClan
            // 
            this.linkLabelClan.AutoSize = true;
            this.linkLabelClan.Location = new System.Drawing.Point(26, 27);
            this.linkLabelClan.Name = "linkLabelClan";
            this.linkLabelClan.Size = new System.Drawing.Size(70, 13);
            this.linkLabelClan.TabIndex = 33;
            this.linkLabelClan.TabStop = true;
            this.linkLabelClan.Text = "linkLabelClan";
            this.linkLabelClan.Visible = false;
            // 
            // buttonLoginRegister
            // 
            this.buttonLoginRegister.Enabled = false;
            this.buttonLoginRegister.Location = new System.Drawing.Point(28, 0);
            this.buttonLoginRegister.Name = "buttonLoginRegister";
            this.buttonLoginRegister.Size = new System.Drawing.Size(85, 23);
            this.buttonLoginRegister.TabIndex = 2;
            this.buttonLoginRegister.Text = "Login/Sign up";
            this.buttonLoginRegister.UseVisualStyleBackColor = true;
            this.buttonLoginRegister.Click += new System.EventHandler(this.buttonLoginRegister_Click);
            // 
            // contextMenuStripUser
            // 
            this.contextMenuStripUser.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemLogout});
            this.contextMenuStripUser.Name = "contextMenuStripUser";
            this.contextMenuStripUser.ShowImageMargin = false;
            this.contextMenuStripUser.Size = new System.Drawing.Size(88, 26);
            this.contextMenuStripUser.Text = "Account";
            this.contextMenuStripUser.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStripUser_ItemClicked);
            // 
            // toolStripMenuItemLogout
            // 
            this.toolStripMenuItemLogout.Name = "toolStripMenuItemLogout";
            this.toolStripMenuItemLogout.Size = new System.Drawing.Size(87, 22);
            this.toolStripMenuItemLogout.Text = "Logout";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.ClientSize = new System.Drawing.Size(202, 180);
            this.Controls.Add(this.buttonLoginRegister);
            this.Controls.Add(this.labelClan);
            this.Controls.Add(this.labelUser);
            this.Controls.Add(this.buttonRankings);
            this.Controls.Add(this.buttonChat);
            this.Controls.Add(this.buttonMap);
            this.Controls.Add(this.checkBoxZoomHack);
            this.Controls.Add(this.buttonEditor);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonClan);
            this.Controls.Add(this.linkLabelClan);
            this.Controls.Add(this.linkLabelUser);
            this.Controls.Add(this.listBoxPlayers);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.Text = "Exceed Bridge";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.contextMenuStripUser.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.ListBox listBoxPlayers;
        public System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.RadioButton radioButtonDestroy;
        public System.Windows.Forms.RadioButton radioButtonDuplicate;
        public System.Windows.Forms.RadioButton radioButtonReturn;
        private System.Windows.Forms.Button buttonEditor;
        private System.Windows.Forms.Button buttonMap;
        private System.Windows.Forms.Timer timerSearchProcess;
        private System.Windows.Forms.CheckBox checkBoxZoomHack;
        private System.Windows.Forms.Button buttonChat;
        private System.Windows.Forms.Button buttonRankings;
        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.Label labelClan;
        public System.Windows.Forms.Button buttonLoginRegister;
        public System.Windows.Forms.LinkLabel linkLabelUser;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripUser;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemLogout;
        public System.Windows.Forms.Button buttonClan;
        public System.Windows.Forms.LinkLabel linkLabelClan;
    }
}

