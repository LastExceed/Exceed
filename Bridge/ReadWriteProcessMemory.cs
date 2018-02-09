using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ReadWriteProcessMemory {
    public class ProcessMemory {
        [DllImport("kernel32.dll")] public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        [DllImport("kernel32.dll")] public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);
        [DllImport("kernel32.dll", SetLastError = true)] static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);
        byte[] buffer = new byte[4];
        public IntPtr handle;
        public int baseAddress;
        public Process process;

        //constructor
        public ProcessMemory(Process process) {
            this.process = process;
            handle = OpenProcess(0x001F0FFF, false, process.Id);
            baseAddress = process.MainModule.BaseAddress.ToInt32();
        }

        //parents
        public byte[] ReadBytes(int pAddress, int pSize) {
            byte[] buffer = new byte[pSize];
            int read = 0;
            ReadProcessMemory((int)handle, pAddress, buffer, pSize, ref read);
            return buffer;
        }
        public void WriteBytes(int pAddress, byte[] pBuffer) {
            int written = 0;
            WriteProcessMemory((int)handle, pAddress, pBuffer, pBuffer.Length, ref written);
        }

        //reading children
        public byte ReadByte(int pAddress) {
            return ReadBytes(pAddress, 1)[0];
        }
        public sbyte ReadSByte(int pAddress) {
            return (sbyte)ReadBytes(pAddress, 1)[0];
        }
        public short ReadShort(int pAddress) {
            return BitConverter.ToInt16(ReadBytes(pAddress, 2), 0);
        }
        public ushort ReadUShort(int pAddress) {
            return BitConverter.ToUInt16(ReadBytes(pAddress, 2), 0);
        }
        public int ReadInt(int pAddress) {
            return BitConverter.ToInt32(ReadBytes(pAddress, 4), 0);
        }
        public uint ReadUInt(int pAddress) {
            return BitConverter.ToUInt32(ReadBytes(pAddress, 4), 0);
        }
        public long ReadLong(int pAddress) {
            return BitConverter.ToInt64(ReadBytes(pAddress, 8), 0);
        }
        public ulong ReadULong(int pAddress) {
            return BitConverter.ToUInt64(ReadBytes(pAddress, 8), 0);
        }
        public float ReadSingle(int pAddress) {
            return BitConverter.ToSingle(ReadBytes(pAddress, 4), 0);
        }
        public double ReadDouble(int pAddress) {
            return BitConverter.ToDouble(ReadBytes(pAddress, 8), 0);
        }
        //writing children
        public void WriteByte(int pAddress, byte pValue) {
            WriteBytes(pAddress, new byte[] { pValue });
        }
        public void WriteSByte(int pAddress, sbyte pValue) {
            //writeBytes(pAddress, BitConverter.GetBytes(pValue));
            WriteBytes(pAddress, new byte[] { (byte)pValue });
        }
        public void WriteShort(int pAddress, short pValue) {
            WriteBytes(pAddress, BitConverter.GetBytes(pValue));
        }
        public void WriteUShort(int pAddress, ushort pValue) {
            WriteBytes(pAddress, BitConverter.GetBytes(pValue));
        }
        public void WriteInt(int pAddress, int pValue) {
            WriteBytes(pAddress, BitConverter.GetBytes(pValue));
        }
        public void WriteUInt(int pAddress, uint pValue) {
            WriteBytes(pAddress, BitConverter.GetBytes(pValue));
        }
        public void WriteLong(int pAddress, long pValue) {
            WriteBytes(pAddress, BitConverter.GetBytes(pValue));
        }
        public void WriteULong(int pAddress, ulong pValue) {
            WriteBytes(pAddress, BitConverter.GetBytes(pValue));
        }
        public void WriteSingle(int pAddress, float pValue) {
            WriteBytes(pAddress, BitConverter.GetBytes(pValue));
        }
        public void WriteDouble(int pAddress, double pValue) {
            WriteBytes(pAddress, BitConverter.GetBytes(pValue));
        }
    }
}