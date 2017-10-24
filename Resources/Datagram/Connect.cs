using System;

namespace Resources.Datagram {
    public class Connect {
        public DatagramID DatagramID {
            get { return (DatagramID)data[0]; }
            private set { data[0] = (byte)value; }
        }
        public ushort Guid {
            get { return BitConverter.ToUInt16(data, 1); }
            set { BitConverter.GetBytes(value).CopyTo(data, 1);}
        }
        public int Mapseed {
            get { return BitConverter.ToInt32(data, 3);}
            set { BitConverter.GetBytes(value).CopyTo(data, 3);}
        }

        public byte[] data;

        public Connect() {
            data = new byte[7];
            DatagramID = DatagramID.connect;
        }
        public Connect(byte[] data) {
            this.data = data;
        }
    }
}
