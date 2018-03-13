using System;
using System.Text;

namespace Resources.Datagram {
    public class Chat : Datagram {
        public ushort Sender {
            get => BitConverter.ToUInt16(data, 1);
            set => BitConverter.GetBytes(value).CopyTo(data, 1);
        }
        public string Text {
            get => Encoding.ASCII.GetString(data, 3, data.Length-3);
            set {
                var sender = Sender;//gets lost when we create a new byte[]
                data = new byte[3 + value.Length];
                DatagramID = DatagramID.Chat;
                Sender = sender;
                Encoding.ASCII.GetBytes(value).CopyTo(data, 3);
            }
        }

        public Chat() {
            data = new byte[3];
            DatagramID = DatagramID.Particle;
        }
        public Chat(byte[] data) : base(data) { }
    }
}
