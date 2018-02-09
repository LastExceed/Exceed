using System;

namespace Resources.Datagram {
    public class Block : Datagram {
        public short Length {
            get => BitConverter.ToInt16(data, 1);
            set => BitConverter.GetBytes(value).CopyTo(data, 1);
        }
        public byte Compressed {
            get => data[3];
            set => data[3] = value;
        }

        public Block() {
            data = new byte[3];
            DatagramID = DatagramID.Block;
        }

        public Block(byte[] data) : base(data) { }
    }
}
