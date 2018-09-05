using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bridge {
    public class KeyboardHook : IDisposable {
        #region imports
        [DllImport("user32", CallingConvention = CallingConvention.StdCall)]
        private static extern int SetWindowsHookEx(HookType idHook, CallbackDelegate lpfn, int hInstance, int threadId);

        [DllImport("user32", CallingConvention = CallingConvention.StdCall)]
        private static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("user32", CallingConvention = CallingConvention.StdCall)]
        private static extern int CallNextHookEx(int idHook, int nCode, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int GetCurrentThreadId();
        #endregion
        #region attributes
        public enum HookType : int {
            WH_JOURNALRECORD = 0,
            WH_JOURNALPLAYBACK = 1,
            WH_KEYBOARD = 2,
            WH_GETMESSAGE = 3,
            WH_CALLWNDPROC = 4,
            WH_CBT = 5,
            WH_SYSMSGFILTER = 6,
            WH_MOUSE = 7,
            WH_HARDWARE = 8,
            WH_DEBUG = 9,
            WH_SHELL = 10,
            WH_FOREGROUNDIDLE = 11,
            WH_CALLWNDPROCRET = 12,
            WH_KEYBOARD_LL = 13,
            WH_MOUSE_LL = 14
        }
        public enum KeyEvents {
            KeyDown = 0x0100,
            KeyUp = 0x0101,
            SKeyDown = 0x0104,
            SKeyUp = 0x0105
        }
        private int HookID = 0;
        public delegate int CallbackDelegate(int Code, int W, int L);
        CallbackDelegate hookProcDelegate;//hold on to this to prevent garbage collection
        #endregion
        #region constructor/destructor
        public KeyboardHook() {
            hookProcDelegate = new CallbackDelegate(KeybHookProc);
            HookID = SetWindowsHookEx(HookType.WH_KEYBOARD_LL, hookProcDelegate, 0, 0);
        }
        bool IsFinalized = false;
        ~KeyboardHook() {
            if (!IsFinalized) {
                UnhookWindowsHookEx(HookID);
                IsFinalized = true;
            }
        }
        public void Dispose() {
            if (!IsFinalized) {
                UnhookWindowsHookEx(HookID);
                IsFinalized = true;
            }
        }
        #endregion
        private int KeybHookProc(int Code, int W, int L) {
            if (Code >= 0) {
                Keys key = (Keys)Marshal.ReadInt32((IntPtr)L);
                switch ((KeyEvents)W) {
                    case KeyEvents.KeyDown:
                    case KeyEvents.SKeyDown:
                        Task.Run(() => OnKey(key, true));
                        break;
                    case KeyEvents.KeyUp:
                    case KeyEvents.SKeyUp:
                        Task.Run(() => OnKey(key, false));
                        break;
                    default:
                        break;
                }
            }
            return CallNextHookEx(HookID, Code, W, L);
        }
        private Dictionary<Keys, bool> keyboardState = new Dictionary<Keys, bool>();
        private void OnKey(Keys key, bool isDown) {
            if (GetForegroundWindow() != CwRam.memory.process.MainWindowHandle) return;

            if (!keyboardState.ContainsKey(key)) keyboardState.Add(key, !isDown);
            if (keyboardState[key] == isDown) return;
            keyboardState[key] = isDown;

            if (!isDown) return;//temp fix to prevent activation on release
            switch (key) {
                case Keys.D4:
                    BridgeTCPUDP.OnHotkey(Resources.HotkeyID.CtrlSpace);
                    break;
                case Keys.D5:
                    BridgeTCPUDP.OnHotkey(Resources.HotkeyID.SpecialMove2);
                    break;
                default:
                    break;
            }
        }
    }
}
