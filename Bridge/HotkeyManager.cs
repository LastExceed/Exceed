using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Resources;

namespace Bridge {
    public static class HotkeyManager {
        private static IntPtr hWnd;
        public const int NOMOD = 0x0000;
        public const int ALT = 0x0001;
        public const int CTRL = 0x0002;
        public const int SHIFT = 0x0004;
        public const int WIN = 0x0008;
        public const int WM_HOTKEY_MSG_ID = 0x0312;

        public static void Init(Form form) {
            hWnd = form.Handle;
            RegisterHotKey(hWnd, (int)HotkeyID.teleport_to_town, CTRL, (int)Keys.H);
            RegisterHotKey(hWnd, (int)HotkeyID.ctrlSpace, CTRL, (int)Keys.Space);
            RegisterHotKey(hWnd, (int)HotkeyID.specialmove2, CTRL, (int)Keys.X);
        }

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }
}