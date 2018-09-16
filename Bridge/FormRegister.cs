using Resources;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Bridge {
    public partial class FormRegister : Form {
        public FormRegister() {
            InitializeComponent();
        }

        Regex alphaNumeric = new Regex("^[a-zA-Z0-9]*$");
        private void textBoxUsername_TextChanged(object sender, EventArgs e) {
            buttonRegister.Enabled = false;
            labelUsernameResult.ForeColor = Color.Red;
            string text = textBoxUsername.Text;
            if (text.Length == 0) labelUsernameResult.Text = "too short";
            else if (text.Length > 11) labelUsernameResult.Text = "too long";
            else if (!alphaNumeric.IsMatch(text)) labelUsernameResult.Text = "only a-z,0-9";
            //else if (false) labelUsernameResult.Text = "already in use";//check availability
            else {
                labelUsernameResult.ForeColor = Color.Green;
                labelUsernameResult.Text = "available";
                if (labelEmailResult.ForeColor == Color.Green) buttonRegister.Enabled = true;
            };
        }

        private void textBoxEmail_TextChanged(object sender, EventArgs e) {
            buttonRegister.Enabled = false;
            labelEmailResult.ForeColor = Color.Red;
            if (!Resources.Tools.validEmailRegex.IsMatch(textBoxEmail.Text)) labelEmailResult.Text = "invalid";
            //else if (false) labelEmailResult.Text = "already in use";
            else {
                labelEmailResult.ForeColor = Color.Green;
                labelEmailResult.Text = "available";
                if (labelUsernameResult.ForeColor == Color.Green) buttonRegister.Enabled = true;
            }
        }

        private void buttonRegister_Click(object sender, EventArgs e) {
            buttonRegister.Enabled = false;
            BridgeTCPUDP.Register(textBoxUsername.Text, textBoxEmail.Text, textBoxPassword.Text);
        }

        private void buttonCreate_Click(object sender, EventArgs e) {
            SetLayout(true);
        }
        public void SetLayout(bool registration) {
            buttonLogin.Visible = !registration;
            linkLabelReset.Visible = !registration;

            labelEmail.Visible = registration;
            textBoxEmail.Visible = registration;

            buttonRegister.Enabled = registration;
            if (registration) {
                labelPassword.Top += 21;
                textBoxPassword.Top += 21;
                this.Width += 73;
                this.Text = "Account registration";
            }
            else {
                labelPassword.Top -= 21;
                textBoxPassword.Top -= 21;
                this.Width -= 73;
                this.Text = "Login";
            }
            buttonCreate.Visible = !registration;
        }

        private void buttonLogin_Click(object sender, EventArgs e) {
            buttonLogin.Enabled = false;
            buttonCreate.Enabled = false;
            linkLabelReset.Enabled = false;
            textBoxUsername.ReadOnly = true;
            textBoxEmail.ReadOnly = true;
            textBoxPassword.ReadOnly = true;
            BridgeTCPUDP.Login(textBoxUsername.Text, textBoxPassword.Text);
        }

        public void OnLoginResponse(AuthResponse authResponse) {
            switch (authResponse) {
                case AuthResponse.Success:
                    BridgeTCPUDP.form.Log("success\n", Color.Green);
                    BridgeTCPUDP.form.buttonLoginRegister.Visible = false;
                    BridgeTCPUDP.form.linkLabelUser.Text = textBoxUsername.Text;
                    BridgeTCPUDP.form.linkLabelUser.Visible = true;
                    BridgeTCPUDP.form.buttonClan.Enabled = true;
                    BridgeTCPUDP.form.buttonClan.Visible = false;
                    BridgeTCPUDP.form.linkLabelClan.Text = "Prisoners of Irreality";
                    BridgeTCPUDP.form.linkLabelClan.Visible = true;
                    this.Close();
                    break;
                case AuthResponse.UnknownUser:
                    BridgeTCPUDP.form.Log("username does not exist\n", Color.Red);
                    goto default;
                case AuthResponse.WrongPassword:
                    BridgeTCPUDP.form.Log("wrong password\n", Color.Red);
                    goto default;
                case AuthResponse.Banned:
                    BridgeTCPUDP.form.Log("you are banned\n", Color.Red);
                    goto default;
                case AuthResponse.AccountAlreadyActive:
                    BridgeTCPUDP.form.Log("account already in use\n", Color.Red);
                    goto default;
                case AuthResponse.Unverified:
                    BridgeTCPUDP.form.Log("unverified (this shouldnt happen)\n", Color.Red);
                    goto default;
                case AuthResponse.UserAlreadyLoggedIn:
                    BridgeTCPUDP.form.Log("you are already logged in (this shouldn't happen)\n", Color.Red);
                    goto default;
                default:
                    buttonLogin.Enabled = true;
                    buttonCreate.Enabled = true;
                    linkLabelReset.Enabled = true;
                    textBoxUsername.ReadOnly = false;
                    textBoxEmail.ReadOnly = false;
                    textBoxPassword.ReadOnly = false;
                    break;
            }
        }
    }
}
