using System;

namespace Resources.Datagram {
    public class Proc {
        public Database.DatagramID DatagramID {
            get { return (Database.DatagramID)data[0]; }
            private set { data[0] = (byte)value; }
        }
        public ushort Target {
            get { return BitConverter.ToUInt16(data, 1); }
            set { BitConverter.GetBytes(value).CopyTo(data, 1); }
        }
        public Database.ProcType Type {
            get { return (Database.ProcType)(data[3]); }
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
            DatagramID = Database.DatagramID.proc;
        }

        public Proc(byte[] data) {
            this.data = data;
        }
    }
}
