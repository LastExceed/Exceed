using System;

namespace Resources.Datagram {
    public class SpecialMove {
        public DatagramID DatagramID {
            get { return (DatagramID)data[0]; }
            private set { data[0] = (byte)value; }
        }

        public ushort Guid {
            get { return BitConverter.ToUInt16(data, 1); }
            set { BitConverter.GetBytes(value).CopyTo(data, 1); }
        }
        public SpecialMoveID Id {
            get { return (SpecialMoveID)data[3]; }
            set { data[3] = (byte)value; }
        }

        public byte[] data;

        public SpecialMove() {
            data = new byte[4];
            DatagramID = DatagramID.specialMove;
        }
        public SpecialMove(byte[] data) {
            this.data = data;
        }
    }
}
