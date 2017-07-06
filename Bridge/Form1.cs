using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bridge {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void ButtonConnect_Click(object sender, EventArgs e) {
            DisableButtons();
            Task.Factory.StartNew(() => BridgeTCPUDP.Connect(this));
        }

        public void Log(string text) {
            richTextBoxChat.Invoke(new Action(() => richTextBoxChat.AppendText(text)));
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

        public void ButtonDisconnect_Click(object sender, EventArgs e) {
            EnableButtons();

            Task.Factory.StartNew(BridgeTCPUDP.Close);
        }
        
        private void TextBoxPassword_KeyDown(object sender, KeyEventArgs e) {
            if(e.KeyCode == Keys.Enter) {
                buttonConnect.PerformClick();
            }
        }

        private void buttonInfo_Click(object sender, EventArgs e) {
            new AboutBox().ShowDialog();
        }
    }
}
