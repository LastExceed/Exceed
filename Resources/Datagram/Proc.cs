using System;

namespace Resources.Datagram {
    public class Proc {
        public DatagramID DatagramID {
            get { return (DatagramID)data[0]; }
            private set { data[0] = (byte)value; }
        }
        public ushort Target {
            get { return BitConverter.ToUInt16(data, 1); }
            set { BitConverter.GetBytes(value).CopyTo(data, 1); }
        }
        public ProcType Type {
            get { return (ProcType)(data[3]); }
            set { data[3] = (byte)value; }
        }
        public float Modifier {
            get { return BitConverter.ToSingle(data, 4); }
            set { BitConverter.GetBytes(value).CopyTo(data, 4); }
        }
        public int Duration {
            get { return BitConverter.ToInt32(data, 8); }
            set { BitConverter.GetBytes(value).CopyTo(data, 8); }
        }
        
        public byte[] data;

        public Proc() {
            data = new byte[12];
            DatagramID = DatagramID.proc;
        }

        public Proc(byte[] data) {
            this.data = data;
        }
    }
}
