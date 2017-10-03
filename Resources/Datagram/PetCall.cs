using System;

namespace Resources.Datagram {
    public class PetCall {
        public Database.DatagramID DatagramID {
            get { return (Database.DatagramID)data[0]; }
            private set { data[0] = (byte)value; }
        }

        public ushort Guid {
            get { return BitConverter.ToUInt16(data, 1); }
            set { BitConverter.GetBytes(value).CopyTo(data, 1); }
        }

        public byte[] data;

        public PetCall() {
            data = new byte[3];
            DatagramID = Database.DatagramID.petCall;
        }
        public PetCall(byte[] data) {
            this.data = data;
        }
    }
}
