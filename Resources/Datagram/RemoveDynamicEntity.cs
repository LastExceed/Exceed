using System;

namespace Resources.Datagram {
    public class RemoveDynamicEntity : Datagram {
        public ushort Guid {
            get => BitConverter.ToUInt16(data, 1);
            set => BitConverter.GetBytes(value).CopyTo(data, 1);
        }

        public RemoveDynamicEntity() {
            data = new byte[3];
            DatagramID = DatagramID.RemoveDynamicEntity;
        }
        public RemoveDynamicEntity(byte[] data) : base(data) { }
    }
}
