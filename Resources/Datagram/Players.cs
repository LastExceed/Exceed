using System;

namespace Resources.Datagram {
    public class Players {
        public Database.DatagramID DatagramID {
            get { return (Database.DatagramID)data[0]; }
        }
        public byte Count {
            get { return data[1]; }
            set { data[1] = value; }
        }
        public ushort Guid {
            get { return BitConverter.ToUInt16(data, 2); }
            set { BitConverter.GetBytes(value).CopyTo(data, 2);}
        }

        public byte[] data;

        public Players() {
            //data = new byte[]
        }
        public Players(byte[] data) {
            this.data = data;
        }
    }
}
