using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bridge {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void ButtonConnect_Click(object sender, EventArgs e) {
            buttonConnect.Enabled = false;
            buttonDisconnect.Enabled = true;
            groupBoxServer.Enabled = false;
            groupBoxAccount.Enabled = false;

            Task.Factory.StartNew(() => {
                try {
                    string[] split = textBoxServerIP.Text.Split(':');
                    int port = 12345;
                    if(split.Length == 2) {
                        if(!int.TryParse(split[2], out port)) {
                            Log("Invalid port");
                            return;
                        }
                    }

                    BridgeTCPUDP.Start(split[0], port);
                    Log($"Connected to {textBoxServerIP.Text}");
                } catch(Exception ex) {
                    Log($"Connection failed with message {ex.Message}");
                    buttonDisconnect.Invoke(new Action(() => ButtonDisconnect_Click(sender, e)));
                }
            });
        }

        public void Log(string text) {
            richTextBoxChat.Invoke(new Action(() => richTextBoxChat.AppendText(text)));
        }

        private void ButtonDisconnect_Click(object sender, EventArgs e) {
            buttonDisconnect.Enabled = false;
            buttonConnect.Enabled = true;
            groupBoxServer.Enabled = true;
            groupBoxAccount.Enabled = true;
        }
    }
}
