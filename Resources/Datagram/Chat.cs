using System;
using System.Text;

namespace Resources.Datagram {
    public class Chat : Datagram {
        public ushort Sender {
            get => BitConverter.ToUInt16(data, 1);
            set => BitConverter.GetBytes(value).CopyTo(data, 1);
        }
        public byte Length {
            get => data[3];
            set => data[3] = value;
        }
        public string Text {
            get => Encoding.ASCII.GetString(data, 4, Length);
            set => Encoding.ASCII.GetBytes(value).CopyTo(data, 4);
        }

        public Chat(string message) {
            if(message.Length <= 255) {
                data = new byte[4 + message.Length];
                DatagramID = DatagramID.Chat;
                Length = (byte)message.Length;
                Text = message;
            } else {
                throw new OverflowException("Message to long");
            }
        }

        public Chat(byte[] data) : base(data) { }
    }
}
