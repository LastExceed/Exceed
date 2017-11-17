using System;

namespace Resources.Datagram {
    public class SpecialMove : Datagram {
        public ushort Guid {
            get => BitConverter.ToUInt16(data, 1);
            set => BitConverter.GetBytes(value).CopyTo(data, 1);
        }
        public SpecialMoveID Id {
            get => (SpecialMoveID)data[3];
            set => data[3] = (byte)value;
        }

        public SpecialMove() {
            data = new byte[4];
            DatagramID = DatagramID.specialMove;
        }
        public SpecialMove(byte[] data) : base(data) { }
    }
}
