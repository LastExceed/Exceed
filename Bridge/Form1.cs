using ReadWriteProcessMemory;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bridge {
    public partial class Form1 : Form {
        FormEditor editor = new FormEditor();

        public Form1(string[]args) {
            InitializeComponent();
            BridgeTCPUDP.form = this;
            CwRam.form = editor;
            try {
                CwRam.memory = new ProcessMemory("Cube");
            }
            catch (IndexOutOfRangeException) {
                MessageBox.Show("CubeWorld process not found. Please start the game first");
                Environment.Exit(0);
            }
            CwRam.RemoveFog();
            HotkeyManager.Init(this);
        }
        protected override void WndProc(ref Message m) {
            if (m.Msg == HotkeyManager.WM_HOTKEY_MSG_ID) {
                BridgeTCPUDP.OnHotkey(m.WParam.ToInt32());
            }
            base.WndProc(ref m);
        }

        public void Log(string text, Color color) {
            if (InvokeRequired) {
                Invoke((Action)(() => Log(text, color)));
            }
            else {
                richTextBoxChat.SelectionStart = richTextBoxChat.TextLength;
                richTextBoxChat.SelectionLength = 0;

                richTextBoxChat.SelectionColor = color;
                richTextBoxChat.AppendText(text);
                richTextBoxChat.SelectionColor = richTextBoxChat.ForeColor;
            }
        }

        public void EnableButtons() {
            if (InvokeRequired) {
                Invoke((Action)EnableButtons);
            }
            else {
                buttonDisconnect.Enabled = false;
                buttonConnect.Enabled = true;
                groupBoxAccount.Enabled = true;
            }
        }
        public void DisableButtons() {
            if (InvokeRequired) {
                Invoke((Action)DisableButtons);
            }
            else {
                buttonDisconnect.Enabled = true;
                buttonConnect.Enabled = false;
                groupBoxAccount.Enabled = false;
            }
        }

        private void ButtonConnect_Click(object sender, EventArgs e) {
            DisableButtons();
            new Thread(new ThreadStart(BridgeTCPUDP.Connect)).Start();
        }
        public void ButtonDisconnect_Click(object sender, EventArgs e) {
            BridgeTCPUDP.Close();
            EnableButtons();
            Log("disconnected\n", Color.Red);
        }

        private void TextBoxPassword_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                buttonConnect.PerformClick();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            Environment.Exit(0);
        }

        private void ButtonEditor_Click(object sender, EventArgs e) {
            editor.Show();
        }
    }
}
