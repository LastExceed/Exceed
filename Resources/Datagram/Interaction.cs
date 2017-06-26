using System;

namespace Resources.Datagram {
    public class Interaction {
        public Database.DatagramID DatagramID {
            get { return (Database.DatagramID)data[0]; }
            private set { data[0] = (byte)value; }
        }

        public ushort ChunkX {
            get { return BitConverter.ToUInt16(data, 1); }
            set { BitConverter.GetBytes(value).CopyTo(data, 1); }
        }
        public ushort ChunkY {
            get { return BitConverter.ToUInt16(data, 3); }
            set { BitConverter.GetBytes(value).CopyTo(data, 3); }
        }
        public ushort Index {
            get { return BitConverter.ToUInt16(data, 5); }
            set { BitConverter.GetBytes(value).CopyTo(data, 5); }
        }

        public byte[] data;

        public Interaction() {
            data = new byte[7];
            DatagramID = Database.DatagramID.interaction;
        }
        public Interaction(byte[] data) {
            this.data = data;
        }
    }
}
