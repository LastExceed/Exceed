using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        private void buttonConnect_Click(object sender, EventArgs e) {
            buttonConnect.Enabled = false;
            buttonDisconnect.Enabled = true;
            groupBoxServer.Enabled = false;
            groupBoxAccount.Enabled = false;

            if (!BridgeTCPUDP.Start(textBoxServerIP.Text, (int)numericUpDownPort.Value)) {
                richTextBoxChat.Text = "connection failed";
                buttonDisconnect_Click(sender, e);
            }
        }

        private void buttonDisconnect_Click(object sender, EventArgs e) {
            buttonDisconnect.Enabled = false;
            buttonConnect.Enabled = true;
            groupBoxServer.Enabled = true;
            groupBoxAccount.Enabled = true;
        }
    }
}
