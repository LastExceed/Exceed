using ReadWriteProcessMemory;

namespace Bridge {
    static class CwRam {
        public static Form1 form;
        public static ProcessMemory memory;
        public static int EntityStart {
            get { return memory.ReadInt(memory.ReadInt(memory.baseAddress + 0x0036b1c8) + 0x39C); }
        }
        public static int ItemStart {
            get { return EntityStart + 0x1E8 + form.listBoxItem.SelectedIndex * 280; }
        }
        public static int SlotStart {
            get { return ItemStart + 20 + 8 * form.listBoxSlots.SelectedIndex; }
        }
    }
}
