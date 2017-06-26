using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bridge
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ButtonConnect_Click(object sender, EventArgs e) {
            buttonConnect.Enabled = false;
            buttonDisconnect.Enabled = true;
            groupBoxServer.Enabled = false;
            groupBoxAccount.Enabled = false;

            Task.Factory.StartNew(() => {
                try {
                    BridgeTCPUDP.Start(textBoxServerIP.Text, (int)numericUpDownPort.Value);
                    richTextBoxChat.Invoke(new Action(() => richTextBoxChat.AppendText("connected")));
                } catch(Exception ex) {
                    richTextBoxChat.Invoke(new Action(() => richTextBoxChat.AppendText("Connection failed")));
                    buttonDisconnect.Invoke(new Action(() => ButtonDisconnect_Click(sender, e)));
                }
            });
        }

        private void ButtonDisconnect_Click(object sender, EventArgs e) {
            buttonDisconnect.Enabled = false;
            buttonConnect.Enabled = true;
            groupBoxServer.Enabled = true;
            groupBoxAccount.Enabled = true;
        }
    }
}
