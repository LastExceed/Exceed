namespace Bridge {
    partial class FormRegister {
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
            this.buttonRegister = new System.Windows.Forms.Button();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.textBoxEmail = new System.Windows.Forms.TextBox();
            this.labelUsername = new System.Windows.Forms.Label();
            this.labelPassword = new System.Windows.Forms.Label();
            this.labelEmail = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.labelUsernameResult = new System.Windows.Forms.Label();
            this.labelEmailResult = new System.Windows.Forms.Label();
            this.buttonLogin = new System.Windows.Forms.Button();
            this.linkLabelReset = new System.Windows.Forms.LinkLabel();
            this.buttonCreate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonRegister
            // 
            this.buttonRegister.Enabled = false;
            this.buttonRegister.Location = new System.Drawing.Point(220, 43);
            this.buttonRegister.Name = "buttonRegister";
            this.buttonRegister.Size = new System.Drawing.Size(74, 21);
            this.buttonRegister.TabIndex = 0;
            this.buttonRegister.Text = "Register";
            this.buttonRegister.UseVisualStyleBackColor = true;
            this.buttonRegister.Click += new System.EventHandler(this.buttonRegister_Click);
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Location = new System.Drawing.Point(56, 1);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(164, 20);
            this.textBoxUsername.TabIndex = 1;
            this.textBoxUsername.TextChanged += new System.EventHandler(this.textBoxUsername_TextChanged);
            // 
            // textBoxEmail
            // 
            this.textBoxEmail.Location = new System.Drawing.Point(55, 22);
            this.textBoxEmail.Name = "textBoxEmail";
            this.textBoxEmail.Size = new System.Drawing.Size(165, 20);
            this.textBoxEmail.TabIndex = 2;
            this.textBoxEmail.Visible = false;
            this.textBoxEmail.TextChanged += new System.EventHandler(this.textBoxEmail_TextChanged);
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Location = new System.Drawing.Point(-2, 3);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(58, 13);
            this.labelUsername.TabIndex = 4;
            this.labelUsername.Text = "Username:";
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(-2, 25);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(56, 13);
            this.labelPassword.TabIndex = 5;
            this.labelPassword.Text = "Password:";
            // 
            // labelEmail
            // 
            this.labelEmail.AutoSize = true;
            this.labelEmail.Location = new System.Drawing.Point(-2, 25);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(35, 13);
            this.labelEmail.TabIndex = 6;
            this.labelEmail.Text = "Email:";
            this.labelEmail.Visible = false;
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(56, 22);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(164, 20);
            this.textBoxPassword.TabIndex = 3;
            this.textBoxPassword.UseSystemPasswordChar = true;
            // 
            // labelUsernameResult
            // 
            this.labelUsernameResult.AutoSize = true;
            this.labelUsernameResult.ForeColor = System.Drawing.Color.Red;
            this.labelUsernameResult.Location = new System.Drawing.Point(220, 4);
            this.labelUsernameResult.Name = "labelUsernameResult";
            this.labelUsernameResult.Size = new System.Drawing.Size(48, 13);
            this.labelUsernameResult.TabIndex = 8;
            this.labelUsernameResult.Text = "too short";
            // 
            // labelEmailResult
            // 
            this.labelEmailResult.AutoSize = true;
            this.labelEmailResult.ForeColor = System.Drawing.Color.Red;
            this.labelEmailResult.Location = new System.Drawing.Point(220, 25);
            this.labelEmailResult.Name = "labelEmailResult";
            this.labelEmailResult.Size = new System.Drawing.Size(37, 13);
            this.labelEmailResult.TabIndex = 9;
            this.labelEmailResult.Text = "invalid";
            // 
            // buttonLogin
            // 
            this.buttonLogin.Location = new System.Drawing.Point(0, 43);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(56, 21);
            this.buttonLogin.TabIndex = 31;
            this.buttonLogin.Text = "Login";
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            // 
            // linkLabelReset
            // 
            this.linkLabelReset.AutoSize = true;
            this.linkLabelReset.Location = new System.Drawing.Point(142, 46);
            this.linkLabelReset.Name = "linkLabelReset";
            this.linkLabelReset.Size = new System.Drawing.Size(78, 13);
            this.linkLabelReset.TabIndex = 30;
            this.linkLabelReset.TabStop = true;
            this.linkLabelReset.Text = "reset password";
            // 
            // buttonCreate
            // 
            this.buttonCreate.Location = new System.Drawing.Point(55, 43);
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Size = new System.Drawing.Size(88, 21);
            this.buttonCreate.TabIndex = 29;
            this.buttonCreate.Text = "Create account";
            this.buttonCreate.UseVisualStyleBackColor = true;
            this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
            // 
            // FormRegister
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(221, 64);
            this.Controls.Add(this.linkLabelReset);
            this.Controls.Add(this.buttonCreate);
            this.Controls.Add(this.buttonLogin);
            this.Controls.Add(this.labelEmailResult);
            this.Controls.Add(this.labelUsernameResult);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.labelEmail);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.labelUsername);
            this.Controls.Add(this.textBoxEmail);
            this.Controls.Add(this.textBoxUsername);
            this.Controls.Add(this.buttonRegister);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormRegister";
            this.Text = "Login";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.TextBox textBoxEmail;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.Label labelEmail;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label labelUsernameResult;
        private System.Windows.Forms.Label labelEmailResult;
        private System.Windows.Forms.LinkLabel linkLabelReset;
        private System.Windows.Forms.Button buttonCreate;
        public System.Windows.Forms.Button buttonLogin;
        public System.Windows.Forms.Button buttonRegister;
    }
}