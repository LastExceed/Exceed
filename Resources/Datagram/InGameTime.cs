using System;

namespace Resources.Datagram {
    public class InGameTime : Datagram {
        public int Time {
            get => BitConverter.ToInt32(data, 1);
            set => BitConverter.GetBytes(value).CopyTo(data, 1);
        }

        public InGameTime() {
            data = new byte[5];
            DatagramID = DatagramID.time;
        }

        public InGameTime(byte[] data) : base(data) { }
    }
}
