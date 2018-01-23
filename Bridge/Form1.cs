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
            new Thread(new ThreadStart(BridgeTCPUDP.Connect)).Start();
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

 
        private void TextBoxPassword_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                //buttonConnect.PerformClick();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            Environment.Exit(0);
        }

        private void ButtonEditor_Click(object sender, EventArgs e) {
            editor.Show();
        }

        private void ButtonLogin_Click(object sender, EventArgs e) {
            buttonLogin.Enabled = false;
            textBoxUsername.Enabled = false;
            textBoxPassword.Enabled = false;
            new Thread(new ThreadStart(BridgeTCPUDP.Login)).Start();
        }

        private void buttonLogout_Click(object sender, EventArgs e) {
            new Thread(new ThreadStart(BridgeTCPUDP.Logout)).Start();
            buttonLogout.Enabled = false;
            buttonLogin.Enabled = true;
        }
    }
}
