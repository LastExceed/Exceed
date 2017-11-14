using System;

namespace Resources.Datagram {
    public class OtherAction {
        public DatagramID DatagramID {
            get { return (DatagramID)data[0]; }
            private set { data[0] = (byte)value; }
        }

        public ushort Guid {
            get { return BitConverter.ToUInt16(data, 1); }
            set { BitConverter.GetBytes(value).CopyTo(data, 1); }
        }

        public byte[] data;

        public OtherAction() {
            data = new byte[3];
            DatagramID = DatagramID.petCall;
        }
        public OtherAction(byte[] data) {
            this.data = data;
        }
    }
}
