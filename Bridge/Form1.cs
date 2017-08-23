using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bridge {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
            BridgeTCPUDP.form = this;
        }

        public void Log(string text, Color color) {
            if (InvokeRequired) {
                Invoke((Action)(() => Log(text, color)));
            } else {
                richTextBoxChat.SelectionStart = richTextBoxChat.TextLength;
                richTextBoxChat.SelectionLength = 0;

                richTextBoxChat.SelectionColor = color;
                richTextBoxChat.AppendText(text);
                richTextBoxChat.SelectionColor = richTextBoxChat.ForeColor;
            }
        }

        public void EnableButtons() {
            if(InvokeRequired) {
                Invoke((Action)EnableButtons);
            } else {
                buttonDisconnect.Enabled = false;
                buttonConnect.Enabled = true;
                groupBoxServer.Enabled = true;
                groupBoxAccount.Enabled = true;
            }
        }
        public void DisableButtons() {
            if(InvokeRequired) {
                Invoke((Action)DisableButtons);
            } else {
                buttonDisconnect.Enabled = true;
                buttonConnect.Enabled = false;
                groupBoxServer.Enabled = false;
                groupBoxAccount.Enabled = false;
            }
        }

        private void ButtonConnect_Click(object sender, EventArgs e) {
            DisableButtons();
            Task.Factory.StartNew(BridgeTCPUDP.Connect);
        }
        public void ButtonDisconnect_Click(object sender, EventArgs e) {
            EnableButtons();
            Log("disconnected\n", Color.Red);
            Task.Factory.StartNew(BridgeTCPUDP.Close);
        }
        
        private void TextBoxPassword_KeyDown(object sender, KeyEventArgs e) {
            if(e.KeyCode == Keys.Enter) {
                buttonConnect.PerformClick();
            }
        }

        private void ButtonInfo_Click(object sender, EventArgs e) {
            new AboutBox().ShowDialog();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            Environment.Exit(0);
        }
    }
}
