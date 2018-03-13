using System;

namespace Resources.Datagram {
    public class Proc : Datagram {
        public ushort Target {
            get => BitConverter.ToUInt16(data, 1);
            set => BitConverter.GetBytes(value).CopyTo(data, 1);
        }
        public ProcType Type {
            get => (ProcType)(data[3]);
            set => data[3] = (byte)value;
        }
        public float Modifier {
            get => BitConverter.ToSingle(data, 4);
            set => BitConverter.GetBytes(value).CopyTo(data, 4);
        }
        public int Duration {
            get => BitConverter.ToInt32(data, 8);
            set => BitConverter.GetBytes(value).CopyTo(data, 8);
        }

        public Proc() {
            data = new byte[12];
            DatagramID = DatagramID.Proc;
        }
        public Proc(byte[] data) : base(data) { }
    }
}
