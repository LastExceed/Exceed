using ReadWriteProcessMemory;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Bridge {
    public partial class FormMain : Form {
        public FormEditor editor = null;
        public FormMap map = new FormMap();
        public FormRegister register = new FormRegister();

        public FormMain(string[]args) {
            InitializeComponent();
        }
        private void FormMain_Shown(object sender, EventArgs e) {
            BridgeTCPUDP.form = this;
            CwRam.formMain = this;
            new Thread(new ThreadStart(BridgeTCPUDP.ListenFromClientTCP)).Start();
            new Thread(new ThreadStart(BridgeTCPUDP.Connect)).Start();
        }
        protected override void WndProc(ref Message m) {
            if (m.Msg == HotkeyManager.WM_HOTKEY_MSG_ID) {
                BridgeTCPUDP.OnHotkey(m.WParam.ToInt32());
            }
            base.WndProc(ref m);
        }
        private void timerSearchProcess_Tick(object sender, EventArgs e) {
            if (buttonEditor.Enabled) {
                try {
                    CwRam.RemoveFog();
                    if (BridgeTCPUDP.status == Resources.BridgeStatus.Playing) CwRam.SetName(textBoxUsername.Text);
                }
                catch (Exception) {
                    HotkeyManager.Deinit();
                    if (editor != null && !editor.IsDisposed) editor.Dispose();
                    buttonEditor.Enabled = false;
                    checkBoxZoomHack.Enabled = false;
                }
            }
            else {
                var processes = Process.GetProcessesByName("Cube");
                if (processes.Length == 0) return;
                try {
                    CwRam.memory = new ProcessMemory(processes[0]);
                }
                catch (System.ComponentModel.Win32Exception) {
                    return; //process just started and isnt fully available yet, try again next iteration
                }
                buttonEditor.Enabled = true;
                checkBoxZoomHack.Enabled = true;
                HotkeyManager.Init(this);
            }
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

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            Environment.Exit(0);
        }

        private void ButtonEditor_Click(object sender, EventArgs e) {
            if (editor == null || editor.IsDisposed) editor = new FormEditor();
            editor.Show();
            editor.WindowState = FormWindowState.Normal;
            editor.Focus();
        }

        private void ButtonLogin_Click(object sender, EventArgs e) {
            buttonLogin.Enabled = false;
            textBoxUsername.Enabled = false;
            textBoxPassword.Enabled = false;
            new Thread(new ThreadStart(() => BridgeTCPUDP.Login(textBoxUsername.Text, textBoxPassword.Text))).Start();
        }
        private void ButtonLogout_Click(object sender, EventArgs e) {
            new Thread(new ThreadStart(BridgeTCPUDP.Logout)).Start();
        }

        private void ButtonMap_Click(object sender, EventArgs e) {
            if (map.IsDisposed) map = new FormMap();
            map.Show();
            map.WindowState = FormWindowState.Normal;
            map.Focus();
        }

        private void checkBoxZoomHack_CheckedChanged(object sender, EventArgs e) {
            CwRam.ZoomHack(checkBoxZoomHack.Checked);
        }

        private void buttonRegister_Click(object sender, EventArgs e) {
            if (register.IsDisposed) register = new FormRegister();
            register.Show();
            register.WindowState = FormWindowState.Normal;
            register.Focus();
        }
    }
}
