using System;

namespace Resources.Datagram {
    public class Block {
        public Database.DatagramID DatagramID {
            get { return (Database.DatagramID)data[0]; }
            private set { data[0] = (byte)value; }
        }
        public short Length {
            get { return BitConverter.ToInt16(data, 1); }
            set { BitConverter.GetBytes(value).CopyTo(data, 1); }
        }
        /*
        public var Compressed {
            get { return data[3]; }
            set { data[3] = value; }
        }*/

        public byte[] data;
        public Block() {
            data = new byte[3];
            DatagramID = Database.DatagramID.block;
        }

        public Block(byte[] data) {
            this.data = data;
        }
    }
}
