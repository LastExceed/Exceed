using System;

namespace Resources.Datagram {
    public class InGameTime {
        public DatagramID DatagramID {
            get { return (DatagramID)data[0]; }
            private set { data[0] = (byte)value; }
        }
        public int Time {
            get { return BitConverter.ToInt32(data, 1); }
            set { BitConverter.GetBytes(value).CopyTo(data, 1); }
        }

        public byte[] data;

        public InGameTime() {
            data = new byte[5];
            DatagramID = DatagramID.time;
        }

        public InGameTime(byte[] data) {
            this.data = data;
        }
    }
}
