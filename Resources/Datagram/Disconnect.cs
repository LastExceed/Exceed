using System;

namespace Resources.Datagram {
    public class Disconnect : Datagram {
        public ushort Guid {
            get => BitConverter.ToUInt16(data, 1);
            set => BitConverter.GetBytes(value).CopyTo(data, 1);
        }

        public Disconnect() {
            data = new byte[3];
            DatagramID = DatagramID.disconnect;
        }
        public Disconnect(byte[] data) : base(data) { }
    }
}
