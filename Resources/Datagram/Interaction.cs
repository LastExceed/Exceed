using System;

namespace Resources.Datagram {
    public class Interaction : Datagram {
        public ushort ChunkX {
            get => BitConverter.ToUInt16(data, 1);
            set => BitConverter.GetBytes(value).CopyTo(data, 1);
        }
        public ushort ChunkY {
            get => BitConverter.ToUInt16(data, 3);
            set => BitConverter.GetBytes(value).CopyTo(data, 3);
        }
        public ushort Index {
            get => BitConverter.ToUInt16(data, 5);
            set => BitConverter.GetBytes(value).CopyTo(data, 5);
        }

        public Interaction() {
            data = new byte[7];
            DatagramID = DatagramID.Interaction;
        }
        public Interaction(byte[] data) : base(data) { }
    }
}
