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
            else if (false) labelUsernameResult.Text = "already in use";//check availability
            else {
                labelUsernameResult.ForeColor = Color.Green;
                labelUsernameResult.Text = "available";
                if (labelEmailResult.ForeColor == Color.Green) buttonRegister.Enabled = true;
            };
        }

        Regex validEmailRegex = new Regex(@"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$", RegexOptions.IgnoreCase);
        private void textBoxEmail_TextChanged(object sender, EventArgs e) {
            buttonRegister.Enabled = false;
            labelEmailResult.ForeColor = Color.Red;
            if (!validEmailRegex.IsMatch(textBoxEmail.Text)) labelEmailResult.Text = "invalid";
            else if (false) labelEmailResult.Text = "already in use";
            else {
                labelEmailResult.ForeColor = Color.Green;
                labelEmailResult.Text = "available";
                if (labelUsernameResult.ForeColor == Color.Green) buttonRegister.Enabled = true;
            }
        }

        private void buttonRegister_Click(object sender, EventArgs e) {
            new Thread(new ThreadStart(() => BridgeTCPUDP.Register(textBoxUsername.Text, textBoxEmail.Text))).Start();
        }
    }
}
