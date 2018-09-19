using ReadWriteProcessMemory;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Bridge {
    public partial class FormMain : Form {
        public FormEditor editor = new FormEditor();
        public FormMap map = new FormMap();
        public FormRegister register = new FormRegister();
        public FormChat chat = new FormChat();
        public FormRankings rankings = new FormRankings();
        private bool processAttached = false;
        private KeyboardHook keyboardHook;

        public FormMain(string[]args) {
            InitializeComponent();
        }
        private void FormMain_Shown(object sender, EventArgs e) {
            chat.Show();
            chat.Top = this.Top;
            chat.Left = Left + Width;
            BridgeTCPUDP.form = this;
            CwRam.formMain = this;
            new Thread(BridgeTCPUDP.ListenFromClientTCP).Start();
            new Thread(BridgeTCPUDP.Connect).Start();
            keyboardHook = new KeyboardHook();
        }
        private void timerSearchProcess_Tick(object sender, EventArgs e) {
            if (processAttached) {
                if (CwRam.memory.process.HasExited) {
                    //HotkeyManager.Deinit();
                    if (editor != null && !editor.IsDisposed) editor.Dispose();
                    processAttached = false;
                    buttonEditor.Enabled = false;
                    checkBoxZoomHack.Enabled = false;
                }
                else {
                    CwRam.RemoveFog();
                    if (BridgeTCPUDP.status == Resources.BridgeStatus.Playing) CwRam.SetName(linkLabelUser.Text);
                }
            }
            else {
                var processes = Process.GetProcessesByName("Cube");
                if (processes.Length == 0) {
                    Text = "Exceed Bridge (detached)";
                    return;
                }
                try {
                    CwRam.memory = new ProcessMemory(processes[0]);
                }
                catch (System.ComponentModel.Win32Exception) {
                    return; //process just started and isnt fully available yet, try again next iteration
                }
                Text = "Exceed Bridge (attached)";
                buttonEditor.Enabled = true;
                checkBoxZoomHack.Enabled = true;
                processAttached = true;
                //HotkeyManager.Init(this);
            }
        }
        public void Log(string text, Color color) {
            if (InvokeRequired) {
                Invoke((Action)(() => Log(text, color)));
            }
            else {
                chat.richTextBoxChat.SelectionStart = chat.richTextBoxChat.TextLength;
                chat.richTextBoxChat.SelectionLength = 0;
               
                chat.richTextBoxChat.SelectionColor = color;
                chat.richTextBoxChat.AppendText(text);
                chat.richTextBoxChat.SelectionColor = chat.richTextBoxChat.ForeColor;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            Environment.Exit(0);
        }

        private void ButtonEditor_Click(object sender, EventArgs e) {
            if (editor.IsDisposed) editor = new FormEditor();
            editor.Show();
            editor.WindowState = FormWindowState.Normal;
            editor.Focus();
        }

        private void ButtonMap_Click(object sender, EventArgs e) {
            if (map.IsDisposed) map = new FormMap();
            ShowWindow(map);
        }

        private void checkBoxZoomHack_CheckedChanged(object sender, EventArgs e) {
            CwRam.ZoomHack(checkBoxZoomHack.Checked);
        }

        private void ShowWindow(Form form) {
            form.Show();
            form.WindowState = FormWindowState.Normal;
            form.Focus();
        }

        private void buttonLoginRegister_Click(object sender, EventArgs e) {
            if (register.IsDisposed) register = new FormRegister();
            ShowWindow(register);
            register.Location = this.Location;
        }

        private void contextMenuStripUser_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {
            BridgeTCPUDP.Logout();
            OnLogout();
        }
        public void OnLogout() {
            Log("logged out\n", Color.Gray);
            buttonLoginRegister.Visible = true;
            linkLabelUser.Text = "linkLabelUser";
            linkLabelUser.Visible = false;
            buttonClan.Enabled = false;
            linkLabelClan.Text = "linkLabelClan";
            linkLabelClan.Visible = false;
        }

        private void linkLabelUser_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            contextMenuStripUser.Show();
            contextMenuStripUser.Top = Cursor.Position.Y;
            contextMenuStripUser.Left = Cursor.Position.X;
        }

        private void buttonClan_Click(object sender, EventArgs e) {
            MessageBox.Show("TODO");
        }

        private void buttonRankings_Click(object sender, EventArgs e) {
            if (rankings.IsDisposed) rankings = new FormRankings();
            ShowWindow(rankings);
            rankings.Location = this.Location;
        }

        private void buttonChat_Click(object sender, EventArgs e) {
            if (chat.IsDisposed) chat = new FormChat();
            ShowWindow(chat);
        }
    }
}
