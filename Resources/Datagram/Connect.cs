using System;

namespace Resources.Datagram {
    public class Connect : Datagram {
        public ushort Guid {
            get => BitConverter.ToUInt16(data, 1);
            set => BitConverter.GetBytes(value).CopyTo(data, 1);
        }
        public int Mapseed {
            get => BitConverter.ToInt32(data, 3);
            set => BitConverter.GetBytes(value).CopyTo(data, 3);
        }

        public Connect() {
            data = new byte[7];
            DatagramID = DatagramID.connect;
        }
        public Connect(byte[] data) : base(data) { }
    }
}
